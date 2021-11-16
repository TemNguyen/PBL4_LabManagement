using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Threading;
using System.Timers;


namespace PC_Heal_ServerService
{
    public partial class Service1 : ServiceBase
    {
        static TcpListener _listener;
        private System.Timers.Timer _timer = null;

        private static Dictionary<string, TcpClient> _currentClients = new Dictionary<string, TcpClient>();
        private static HashSet<string> _totalClients = new HashSet<string>();

        public Service1()
        {
            InitializeComponent();
            BLL.MongoDb.Instance.Initiation();
        }

        protected override void OnStart(string[] args)
        {
            _timer = new System.Timers.Timer();
            //check is socket dispose every 10s
            _timer.Interval = 10000;
            _timer.Elapsed += Timer_Elapsed;
            _timer.Enabled = true;
            //start new thread to avoid starting state on service.
            int portNumber = GetPortNumber();
            Thread thread = new Thread(() => StartListening(portNumber));
            thread.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (var client in _totalClients)
            {
                if (IsClientDispose(_currentClients[client]))
                {
                    BLL.MongoDb.Instance.UpdateComputerState(client);
                }
            }
        }

        private void StartListening(int portNumber)
        {
            _listener = new TcpListener(IPAddress.Any, portNumber);
            const int backLog = 100;
            _listener.Start(backLog);

            while (true)
            {
                try
                {
                    var client = _listener.AcceptTcpClient();

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
                    //multi thread
                    new Thread(SaveData).Start(client);
                }
                catch (SocketException)
                {
                    //ignore
                }
                catch (Exception)
                {
                    //ignore
                }
            }
        }

        static void SaveData(object state)
        {
            var client = state as TcpClient;
            try
            {
                if (client != null)
                {
                    var stream = client.GetStream();
                    var reader = new StreamReader(stream);

                    var serializer = new JsonSerializer();
                    var data = serializer.Deserialize(reader, typeof(CI)) as CI;

                    BLL.MongoDb.Instance.Save(data);

                    stream.Close();
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        static bool IsClientDispose(TcpClient client)
        {
            var s = client.Client;
            bool part1, part2;
            try
            {
                const int timeOut = 1000;
                part1 = s.Poll(timeOut, SelectMode.SelectRead);
                part2 = (s.Available == 0);
            }
            catch (Exception)
            {
                return true;
            }
            if (part1 && part2)
                return true;
            else
                return false;
        }

        private int GetPortNumber()
        {
            int portNumber = 0;
            string rootPath = AppDomain.CurrentDomain.BaseDirectory.ToString();
            string confPath = rootPath + @"service.conf";

            try
            {
                if (File.Exists(confPath))
                {
                    using (StreamReader sr = new StreamReader(confPath))
                    {
                        portNumber = Convert.ToInt32(sr.ReadToEnd());
                    }
                }
                else
                {
                    //create new file
                    using (StreamWriter sw = File.CreateText(confPath))
                    {
                        sw.Write("5000");
                    }
                    //use default port
                    portNumber = 5000;
                }
            }
            catch (Exception)
            {
                //ignore
            }

            return portNumber;
        }
    }
}
