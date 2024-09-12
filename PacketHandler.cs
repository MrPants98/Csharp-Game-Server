using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Unity_Game_Server.Models;
using Unity_Game_Server.Models.PacketData.ServerBound;

namespace Unity_Game_Server
{
    internal class PacketHandler
    {
        public async void InitPacketHandler()
        {
            Program.websocketServer.OnPacketReceived += ExtractPacketData;
            Console.WriteLine("Packet Handler Intialized");
        }

        private async void ExtractPacketData(byte[] packetData, WebSocket packetSender)
        {
            byte packetID = packetData[0];
            byte[] packetDataJSON = new byte[packetData.Length - 1];
            Array.Copy(packetData, 1, packetDataJSON, 0, packetDataJSON.Length);

            using (Stream stream = new MemoryStream(packetDataJSON))
            {
                switch (packetID)
                {
                    case 0:
                        ClientConnect connectPacket = await JsonSerializer.DeserializeAsync<ClientConnect>(stream);
                        await connectPacket.AddConnectedPlayer(packetSender);
                        break;
                }
            }
        }

        public async Task BroadcastPacket(byte[] packetData)
        {
            List<Task> broadcastTasks = new List<Task>();

            foreach (WebsocketClient client in Program.websocketServer.connectedWebsocketClients)
                if (client.socket.State == WebSocketState.Open)
                    broadcastTasks.Add(client.SendPacket(packetData));

            await Task.WhenAll(broadcastTasks);
        }
    }
}