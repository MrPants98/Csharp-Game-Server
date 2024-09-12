namespace Unity_Game_Server
{
    internal class Program
    {
        public static WebsocketServer? websocketServer { get; private set; }
        public static GameServer? gameServer { get; private set; }
        public static PacketHandler packetHandler = new PacketHandler();

        static void Main(string[] args)
        {
            websocketServer = new WebsocketServer("localhost", 8080);
            gameServer = new GameServer();

            websocketServer.InitServer();
            packetHandler.InitPacketHandler();
            
            do
            {
                string? line = Console.ReadLine();
                if (line == "stop")
                    break;
            } while (true);
        }
    }
}