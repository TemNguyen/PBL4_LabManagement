using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using PC_Heal_ServerService.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;


namespace PC_Heal_ServerService
{
    public partial class Service1 : ServiceBase
    {
        const int PortNumber = 5000;
        static TcpListener _listener;

        static readonly MongoClient _mongoClient = new MongoClient("mongodb+srv://ndthinhdut19:pbl2021mongodb@labmanagementdb.j4waq.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
        static IMongoDatabase _mongoDatabase = _mongoClient.GetDatabase("LabManagementDatabase");
        static IMongoCollection<BsonDocument> _mongoCollection = null;

        public Service1()
        {
            InitializeComponent();
        }

        protected override async void OnStart(string[] args)
        {
            try
            {
                try
                {
                    //init default Account
                    _mongoCollection = _mongoDatabase.GetCollection<BsonDocument>("accounts");

                    var key = Builders<BsonDocument>.IndexKeys.Ascending("computerName");
                    var indexOption = new CreateIndexOptions() { Unique = true };
                    var model = new CreateIndexModel<BsonDocument>(key, indexOption);
                    await _mongoCollection.Indexes.CreateOneAsync(model);

                    var defaultAccount = new Account
                    {
                        Username = "admin",
                        Password = "admin"
                    };
                    var document = defaultAccount.ToBsonDocument();
                    await _mongoCollection.InsertOneAsync(document);
                }
                catch (Exception)
                {

                }

                _listener = new TcpListener(IPAddress.Any, PortNumber);
                _listener.Start();

                while (true)
                {
                    var client = _listener.AcceptTcpClient();
                    await SaveData(client);
                }
            }
            catch (Exception)
            {
                _listener.Stop();
            }
        }

        protected override void OnStop()
        {

        }

        static async Task SaveData(TcpClient client)
        {
            await Task.Yield();
            try
            {
                var stream = client.GetStream();
                var reader = new StreamReader(stream);

                var serializer = new JsonSerializer();
                var data = serializer.Deserialize(reader, typeof(CI)) as CI;

                //for SQL
                //try
                //{
                //    if (BLL.BLL.Instance.IsExist(data.ComputerName))
                //    {
                //        BLL.BLL.Instance.Update(data);
                //    }
                //    else
                //    {
                //        BLL.BLL.Instance.Add(data);
                //    }
                //}
                //catch (Exception)
                //{

                //}

                //for mongoDB
                try
                {
                    _mongoCollection = _mongoDatabase.GetCollection<BsonDocument>("devices");
                    //set unique
                    try
                    {
                        var key = Builders<BsonDocument>.IndexKeys.Ascending("computerName");
                        var indexOption = new CreateIndexOptions() { Unique = true };
                        var model = new CreateIndexModel<BsonDocument>(key, indexOption);
                        await _mongoCollection.Indexes.CreateOneAsync(model);
                    }
                    catch (Exception)
                    {

                    }

                    var document = data.ToBsonDocument();
                    var filter = Builders<BsonDocument>.Filter.Eq("computerName", data.ComputerName);
                    var computerInDb = await _mongoCollection.Find(filter).FirstOrDefaultAsync();

                    if (computerInDb == null)
                    {
                        try
                        {
                            await _mongoCollection.InsertOneAsync(document);
                        }
                        catch (Exception)
                        {

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
                            await _mongoCollection.UpdateOneAsync(filter, update);

                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                catch (Exception)
                {

                }

                stream.Close();
            }
            catch (Exception)
            {

            }
        }
    }
}
