using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PC_Heal_ClientService
{
    public class SendToServer
    {
        public static void Send(CI computer)
        {
            var serverIP = IPAddress.Parse("192.168.0.104");
            int serverPort = 5000;

            while (true)
            {
                var client = new TcpClient();
                client.Connect(serverIP, serverPort);
                var localEndpoint = client.Client.RemoteEndPoint as IPEndPoint;
                Console.WriteLine("Connecting to " + localEndpoint.Address + ":" + localEndpoint.Port);

                var stream = client.GetStream();
                var writer = new StreamWriter(stream) { AutoFlush = true };

                var computerInfo = computer;
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, computerInfo);
            }
        }
    }
}
