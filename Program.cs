using e_pharmacy.Models;
using e_pharmacy.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BCrypt.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<UserService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/users/register", async (UserService userService, CreateUserDto userDto) =>
{
    var existingUser = await userService.GetByUsernameAsync(userDto.Username);
    if (existingUser != null)
    {
        return Results.Conflict("User with this username already exists.");
    }

    var user = new User
    {
        Username = userDto.Username,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
        Roles = userDto.Roles
    };

    await userService.CreateAsync(user);

    return Results.Created($"/api/users/{user.Id}", user);
})
.WithName("RegisterUser")
.WithOpenApi();

app.Run();

record CreateUserDto(string Username, string Password, List<string> Roles);