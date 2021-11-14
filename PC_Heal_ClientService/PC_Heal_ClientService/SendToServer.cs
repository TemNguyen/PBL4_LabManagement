using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PC_Heal_ClientService
{
    public class SendToServer
    {

        public static async Task Send(CI computerInfo)
        {
            IPAddress serverIp = null;
            int serverPort = 0;

            string rootPath = AppDomain.CurrentDomain.BaseDirectory.ToString();
            string confPath = rootPath + @"service.conf";

            if (File.Exists(confPath))
            {
                var sr = new StreamReader(confPath);
                var line = await sr.ReadToEndAsync();
                string[] data = line.Split(new char[]
                {
                    '\r', '\n'
                });
                serverIp = IPAddress.Parse(data[0]);
                serverPort = Convert.ToInt32(data[2]);
                sr.Close();
            }
            else
            {
                //create new file
                using (StreamWriter sw = File.CreateText(confPath))
                {
                    await sw.WriteAsync("192.168.0.8\r\n5000");
                }
                //use default ip and port
                serverIp = IPAddress.Parse("192.168.0.8");
                serverPort = 5000;
            }


            TcpClient client = new TcpClient();
            await client.ConnectAsync(serverIp, serverPort);

            var stream = client.GetStream();
            var writer = new StreamWriter(stream) { AutoFlush = true };

            var serializer = new JsonSerializer();
            serializer.Serialize(writer, computerInfo);

        }
    }
}
