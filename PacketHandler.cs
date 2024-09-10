using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Unity_Game_Server.Models;

namespace Unity_Game_Server
{
    internal class PacketHandler
    {
        private GameServer server;

        public void InitPacketHandler()
        {
            server.OnPacketReceived += PacketHandling;
        }

        private async void PacketHandling(byte[] packetData)
        {
            await BroadcastPacket(packetData);
        }

        private async Task BroadcastPacket(byte[] packetData)
        {
            List<Task> broadcastTasks = new List<Task>();

            foreach (Client client in server.connectedClients)
                if (client.socket.State == WebSocketState.Open)
                    broadcastTasks.Add(client.SendPacket(packetData));

            await Task.WhenAll(broadcastTasks);
        }

        public PacketHandler(GameServer _server)
        {
            server = _server;
        }
    }
}