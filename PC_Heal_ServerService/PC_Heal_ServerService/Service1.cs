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
        private System.Timers.Timer timer = null;

        static readonly MongoClient _mongoClient = new MongoClient("mongodb+srv://ndthinhdut19:pbl2021mongodb@labmanagementdb.j4waq.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
        static IMongoDatabase _mongoDatabase = _mongoClient.GetDatabase("LabManagementDatabase");
        static IMongoCollection<BsonDocument> _mongoCollection = null;

        private static Dictionary<string, TcpClient> _currentClients = new Dictionary<string, TcpClient>();
        private static HashSet<string> _totalClients = new HashSet<string>();

        public Service1()
        {
            InitializeComponent();
            InitDefaultAccount();
        }

        protected override void OnStart(string[] args)
        {
            timer = new System.Timers.Timer();
            //check every 10s
            timer.Interval = 10000;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            //start new thread to handle
            new Thread(StartListening).Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (var client in _totalClients)
            {
                if (IsClientDispose(_currentClients[client]))
                {
                    try
                    {
                        _mongoCollection = _mongoDatabase.GetCollection<BsonDocument>("devices");
                        var filter = Builders<BsonDocument>.Filter.Eq("IPAddress", client);
                        var computerInDb = _mongoCollection.Find(filter).FirstOrDefault();

                        try
                        {
                            var update = Builders<BsonDocument>.Update.Set("isOnline", false);
                            _mongoCollection.UpdateOne(filter, update);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        private void StartListening()
        {
            _listener = new TcpListener(IPAddress.Any, PortNumber);
            _listener.Start(100);

            while (true)
            {
                try
                {
                    var client = _listener.AcceptTcpClient();
                    //get remoteIP
                    var remoteIP = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                    _totalClients.Add(remoteIP);

                    if (_currentClients.ContainsKey(remoteIP))
                    {
                        _currentClients[remoteIP] = client;
                    }
                    else
                    {
                        _currentClients.Add(remoteIP, client);
                    }

                    new Thread(SaveData).Start(client);


                }
                catch (SocketException)
                {

                }
                catch (Exception)
                {

                }
            }
        }

        static bool IsClientDispose(TcpClient client)
        {
            var s = client.Client;
            bool part1, part2;
            try
            {
                part1 = s.Poll(1000, SelectMode.SelectRead);
                part2 = (s.Available == 0);
            }
            catch (Exception e)
            {
                return true;
            }
            if (part1 && part2)
                return true;
            else
                return false;
        }

        static void SaveData(object state)
        {
            var client = state as TcpClient;
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
                        _mongoCollection.Indexes.CreateOne(model);
                    }
                    catch (Exception)
                    {

                    }

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

        static void InitDefaultAccount()
        {
            try
            {
                //init default Account
                _mongoCollection = _mongoDatabase.GetCollection<BsonDocument>("accounts");

                var key = Builders<BsonDocument>.IndexKeys.Ascending("username");
                var indexOption = new CreateIndexOptions() { Unique = true };
                var model = new CreateIndexModel<BsonDocument>(key, indexOption);
                _mongoCollection.Indexes.CreateOne(model);

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

                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
