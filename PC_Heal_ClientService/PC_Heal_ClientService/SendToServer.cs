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
        //private static ManualResetEvent connectDone = new ManualResetEvent(false);
        //private static ManualResetEvent sendDone = new ManualResetEvent(false);

        public static async Task Send(CI computerInfo)
        {
            var serverIp = IPAddress.Parse("192.168.0.104");
            var serverPort = 5000;

            TcpClient client = new TcpClient();
            await client.ConnectAsync(serverIp, serverPort);

            var stream = client.GetStream();
            var writer = new StreamWriter(stream) { AutoFlush = true };

            var serializer = new JsonSerializer();
            serializer.Serialize(writer, computerInfo);

        }
    }
}
