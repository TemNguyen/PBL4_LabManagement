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
using Newtonsoft.Json;

namespace PC_Heal_ServerService
{
    public partial class Service1 : ServiceBase
    {
        const int MAX_CONNECTION = 15;
        const int PORT_NUMBER = 5000;
        static TcpListener listener;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            listener = new TcpListener(IPAddress.Any, PORT_NUMBER);
            listener.Start();

            for(int i = 0; i < MAX_CONNECTION; i++)
            {
                new Thread(Processing).Start();
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

                    try
                    {
                        var stream = client.GetStream();
                        var reader = new StreamReader(stream);

                        var serializer = new JsonSerializer();
                        var data = serializer.Deserialize(reader, typeof(CI)) as CI;

                        if (BLL.BLL.Instance.IsExist(data.ComputerName))
                        {
                            BLL.BLL.Instance.Update(data);
                        }
                        else
                        {
                            BLL.BLL.Instance.Add(data);
                        }

                        stream.Close();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
