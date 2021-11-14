using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace PC_Heal_ServerService.BLL
{
    class MongoDb
    {
        private static readonly string ConnectionString =
            "mongodb+srv://ndthinhdut19:pbl2021mongodb@labmanagementdb.j4waq.mongodb.net/myFirstDatabase?retryWrites=true&w=majority";
        private static readonly string DatabaseName = "LabManagementDatabase";
        private static readonly MongoClient MongoClient = new MongoClient(ConnectionString);
        private static readonly IMongoDatabase MongoDatabase = MongoClient.GetDatabase(DatabaseName);
        static IMongoCollection<BsonDocument> _mongoCollection = null;

        public static MongoDb Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MongoDb();
                }
                return _instance;
            }
            private set
            {
            }
        }
        private static MongoDb _instance;
        private MongoDb()
        {

        }

        public void Save(CI data)
        {
            try
            {
                const string collectionName = "devices";
                _mongoCollection = MongoDatabase.GetCollection<BsonDocument>(collectionName);

                var document = data.ToBsonDocument();
                var filter = Builders<BsonDocument>.Filter.Eq("computerName", data.ComputerName);
                var computerInDb = _mongoCollection.Find(filter).FirstOrDefault();

                if (computerInDb == null)
                {
                    try
                    {
                        _mongoCollection.InsertOne(document);
                    }
                    catch (Exception)
                    {
                        //ignore
                    }
                }
                else
                {
                    try
                    {
                        var update = Builders<BsonDocument>.Update.Set("CPUUsage", data.CpuUsage)
                            .Set("numThread", data.NumThread)
                            .Set("numProcess", data.NumProcess)
                            .Set("diskUsage", data.DiskUsage)
                            .Set("RAMUsage", data.RamUsage)
                            .Set("GPUUsage", data.GpuUsage)
                            .Set("activeTime", data.ActiveTime)
                            .Set("isOnline", data.IsOnline);
                        _mongoCollection.UpdateOne(filter, update);

                    }
                    catch (Exception)
                    {
                        //ignore
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }


        public void UpdateComputerState(string clientIp)
        {
            try
            {
                const string collectionName = "devices";
                _mongoCollection = MongoDatabase.GetCollection<BsonDocument>(collectionName);
                var filter = Builders<BsonDocument>.Filter.Eq("IPAddress", clientIp);
                var computerInDb = _mongoCollection.Find(filter).FirstOrDefault();

                try
                {
                    var update = Builders<BsonDocument>.Update.Set("isOnline", false);
                    _mongoCollection.UpdateOne(filter, update);
                }
                catch (Exception)
                {
                    //ignore
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        public void Initiation()
        {
            //set Unique field of collection devices
            _mongoCollection = MongoDatabase.GetCollection<BsonDocument>("devices");
            try
            {
                var key1 = Builders<BsonDocument>.IndexKeys.Ascending("computerName");
                var indexOption1 = new CreateIndexOptions() { Unique = true };
                var model1 = new CreateIndexModel<BsonDocument>(key1, indexOption1);
                _mongoCollection.Indexes.CreateOne(model1);
            }
            catch (Exception)
            {
                //ignore
            }

            //set Unique field of collection accounts
            try
            {
                _mongoCollection = MongoDatabase.GetCollection<BsonDocument>("accounts");
                var key2 = Builders<BsonDocument>.IndexKeys.Ascending("username");
                var indexOption2 = new CreateIndexOptions() { Unique = true };
                var model2 = new CreateIndexModel<BsonDocument>(key2, indexOption2);
                _mongoCollection.Indexes.CreateOne(model2);
            }
            catch (Exception)
            {
                //ignore
            }

            //Initiation default account
            try
            {
                var defaultAccount = new Account { Username = "admin", Password = "admin" };
                var document = defaultAccount.ToBsonDocument();

                var filter = Builders<BsonDocument>.Filter.Eq("username", defaultAccount.Username);
                var computerInDb = _mongoCollection.Find(filter).FirstOrDefault();

                if (computerInDb == null)
                {
                    try
                    {
                        _mongoCollection.InsertOne(document);
                    }
                    catch (Exception)
                    {
                        //ignore
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }
    }
}
