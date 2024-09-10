using System.Net.WebSockets;
using System.Text;

namespace Unity_Game_Server
{
    internal class Program
    {
        private static GameServer server = new GameServer("localhost", 8080);

        static void Main(string[] args)
        {
            server.InitServer();

            server.OnPacketReceived += PacketHandling;

            Console.ReadKey();
        }

        private static async void PacketHandling(string packet)
        {
            Console.WriteLine($"Packet Received: {packet}");
            await BroadcastPacket(packet);
        }

        private static async Task BroadcastPacket(string packet)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(packet);
            List<Task> broadcastTasks = new List<Task>();

            foreach (WebSocket client in server.connectedClients)
                if (client.State == WebSocketState.Open)
                    broadcastTasks.Add(client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None));

            await Task.WhenAll(broadcastTasks);
            Console.WriteLine($"Broadcasted packet: {packet}");
        }
    }
}