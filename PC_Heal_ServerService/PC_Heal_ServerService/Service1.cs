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
        const int PORT_NUMBER = 5000;
        static TcpListener listener;
        static HashSet<TcpClient> clients;

        System.Timers.Timer timer = null;

        static MongoClient mongoClient = new MongoClient("mongodb+srv://ndthinhdut19:pbl2021mongodb@labmanagementdb.j4waq.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
        static IMongoDatabase mongoDatabase = mongoClient.GetDatabase("LabManagementDatabase");
        static IMongoCollection<BsonDocument> mongoCollection = null;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            clients = new HashSet<TcpClient>();
            timer = new System.Timers.Timer();
            timer.Interval = 2000;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            try
            {
                try
                {
                    //init default Account
                    mongoCollection = mongoDatabase.GetCollection<BsonDocument>("Account");
                    var defaultAccount = new Account
                    {
                        Username = "admin",
                        Password = "admin"
                    };
                    var document = defaultAccount.ToBsonDocument();
                    mongoCollection.InsertOne(document);
                }
                catch (Exception)
                {

                }

                listener = new TcpListener(IPAddress.Any, PORT_NUMBER);
                listener.Start();
            }
            catch (Exception)
            {

            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    new Thread(Processing).Start();
                }
            }
            catch (Exception)
            {

            }
        }

        protected override void OnStop()
        {

        }

        static void Processing()
        {
            try
            {
                while (true)
                {
                    var client = listener.AcceptTcpClient();
                    //if(clients.Count < 10)
                    //    clients.Add(client);
                    try
                    {
                        var stream = client.GetStream();
                        var reader = new StreamReader(stream);

                        var serializer = new JsonSerializer();
                        var data = serializer.Deserialize(reader, typeof(CI)) as CI;
                        //for SQL
                        try
                        {
                            if (BLL.BLL.Instance.IsExist(data.ComputerName))
                            {
                                BLL.BLL.Instance.Update(data);
                            }
                            else
                            {
                                BLL.BLL.Instance.Add(data);
                            }
                        }
                        catch (Exception)
                        {

                        }
                        //for mongoDB
                        try
                        {
                            mongoCollection = mongoDatabase.GetCollection<BsonDocument>("CI");

                            var document = data.ToBsonDocument();
                            var filter = Builders<BsonDocument>.Filter.Eq("_id", data.ComputerName);
                            var c = mongoCollection.Find(filter).FirstOrDefault();

                            if (c == null)
                            {
                                try
                                {
                                    mongoCollection.InsertOne(document);
                                }
                                catch (Exception)
                                {

                                }
                            }
                            else
                            {
                                try
                                {
                                    var CIInDB = BsonSerializer.Deserialize<CI>(c);
                                    var update = Builders<BsonDocument>.Update.Set("CPU_Usage", data.CPU_Usage)
                                        .Set("Max_Clock_Speed", data.Max_Clock_Speed)
                                        .Set("Num_Thread", data.Num_Thread)
                                        .Set("Num_Process", data.Num_Process)
                                        .Set("Disk_Usage", data.Disk_Usage)
                                        .Set("RAM_Usage", data.RAM_Usage)
                                        .Set("GPU_Usage", data.GPU_Usage)
                                        .Set("ActiveTime", CIInDB.ActiveTime + 1)
                                        .Set("IsOnline", CIInDB.IsOnline);
                                    mongoCollection.UpdateOne(filter, update);

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
            catch (Exception)
            {

            }
        }
    }
}
