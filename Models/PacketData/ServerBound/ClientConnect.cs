using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Unity_Game_Server.Models.PacketData.ServerBound
{
    public class ClientConnect
    {
        public required string username;
        public UInt64 uuid;
        public byte color;

        public void AddConnectedPlayer(WebSocket clientJoiningSocket)
        {
            Player connectingPlayer = new Player(username, uuid, color, Program.websocketServer.connectedWebsocketClients.FirstOrDefault(sc => sc.socket == clientJoiningSocket));
            Program.gameServer?.connectedPlayers.Add(connectingPlayer);

            Console.WriteLine($"{username} has joined the game");
        }
    }
}
