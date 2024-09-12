using System.Net.WebSockets;

namespace Unity_Game_Server.Models.PacketData.ServerBound
{
    public class ClientConnect
    {
        public string username { get; set; }
        public UInt64 uuid { get; set; }
        public byte color { get; set; }

        public async Task AddConnectedPlayer(WebSocket clientJoiningSocket)
        {
            Player connectingPlayer = new Player(username, uuid, color, Program.websocketServer.connectedWebsocketClients.FirstOrDefault(sc => sc.socket == clientJoiningSocket));
            Program.gameServer?.connectedPlayers.Add(connectingPlayer);

            Console.WriteLine($"{connectingPlayer.username} has joined the game");

            await Program.gameServer.BroadcastClientJoin(uuid);
        }
    }
}
