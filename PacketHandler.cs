using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Unity_Game_Server.Models;
using Unity_Game_Server.Models.PacketData.ServerBound;

namespace Unity_Game_Server
{
    internal class PacketHandler
    {
        public void InitPacketHandler()
        {
            Program.websocketServer.OnPacketReceived += ExtractPacketData;
        }

        private async void ExtractPacketData(byte[] packetData, WebSocket packetSender)
        {
            byte packetID = packetData[0];
            switch (packetID)
            {
                case 0:
                    
                    _ = Task.Run(() =>
                    {
                        ClientConnect connectPacket = JsonSerializer.Deserialize<ClientConnect>(Encoding.UTF8.GetString(packetData, 1, packetData.Length - 1));
                        connectPacket.AddConnectedPlayer(packetSender);
                    });
                    break;
            }
        }

        private async Task BroadcastPacket(byte[] packetData)
        {
            List<Task> broadcastTasks = new List<Task>();

            foreach (WebsocketClient client in Program.websocketServer.connectedWebsocketClients)
                if (client.socket.State == WebSocketState.Open)
                    broadcastTasks.Add(client.SendPacket(packetData));

            await Task.WhenAll(broadcastTasks);
        }
    }
}