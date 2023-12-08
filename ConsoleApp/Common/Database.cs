using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class Database
    {
        static string connectionString = "mongodb://localhost:27017";
        static string dbName = "test";

        static MongoClient _client;
        static MongoClient Client
        {
            get
            {
                if (_client is null)
                {
                    _client = new MongoClient(connectionString);
                }
                return _client;
            }
        }

        static IMongoDatabase _db;
        public static IMongoDatabase Db
        {
            get
            {
                if (_db is null)
                {
                    _db = Client.GetDatabase(dbName);
                }
                return _db;
            }
        }
    }
}
