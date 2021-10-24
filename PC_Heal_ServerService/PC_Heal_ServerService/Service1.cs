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
        const int MAX_CONNECTION = 15;
        const int PORT_NUMBER = 5000;
        static TcpListener listener;

        System.Timers.Timer timer = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new System.Timers.Timer();
            timer.Interval = 2000;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            try
            {
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
                for (int i = 0; i < MAX_CONNECTION; i++)
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

                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
