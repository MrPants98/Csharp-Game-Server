using System.Net.WebSockets;
using System.Text;

namespace Unity_Game_Server
{
    internal class Program
    {
        public static WebsocketServer? websocketServer { get; private set; }
        public static GameServer? gameServer { get; private set; }
        private static PacketHandler packetHandler = new PacketHandler();

        static void Main(string[] args)
        {
            websocketServer = new WebsocketServer("localhost", 8080);
            gameServer = new GameServer();

            websocketServer.InitServer();
            packetHandler.InitPacketHandler();


            Console.ReadLine();
        }
    }
}