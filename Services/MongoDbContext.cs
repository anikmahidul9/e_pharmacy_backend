using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using e_pharmacy.Models;

namespace e_pharmacy.Services
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("MongoDB:ConnectionString").Value;
            var databaseName = configuration.GetSection("MongoDB:DatabaseName").Value;
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    }
}
