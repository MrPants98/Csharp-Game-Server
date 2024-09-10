using System.Net.WebSockets;
using System.Text;

namespace Unity_Game_Server
{
    internal class Program
    {
        private static GameServer server = new GameServer("localhost", 8080);
        private static PacketHandler packetHandler = new PacketHandler(server);

        static void Main(string[] args)
        {
            server.InitServer();
            packetHandler.InitPacketHandler();


            Console.ReadLine();
        }
    }
}