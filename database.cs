using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace MyApp
{
    class Database
    {
        MongoClient client;
        IMongoDatabase database;
        public IMongoCollection<BsonDocument> recipes;

        public Database()
        {
            client = new MongoClient("mongodb://127.0.0.1:27017");
            database = client.GetDatabase("recipe_db");
            recipes = database.GetCollection<BsonDocument>("recipes");
        }
    }
}