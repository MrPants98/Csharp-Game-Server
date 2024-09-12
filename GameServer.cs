using System.Text;
using System.Text.Json;
using Unity_Game_Server.Models;
using Unity_Game_Server.Models.PacketData.ClientBound;

namespace Unity_Game_Server
{
    public class GameServer
    {
        public List<Player> connectedPlayers = new List<Player>();

        public async Task BroadcastClientJoin(UInt64 uuid)
        {
            Player playerJoined = connectedPlayers.Find(p => p.uuid == uuid);
            string packetDataJSON = JsonSerializer.Serialize(new ClientJoined { username = playerJoined.username, color = playerJoined.color });

            byte[] packetDataBuffer = Encoding.UTF8.GetBytes(packetDataJSON);
            byte[] buffer = new byte[packetDataBuffer.Length + 1];
            buffer[0] = 0x01;
            Array.Copy(packetDataBuffer, 0, buffer, 1, packetDataBuffer.Length);

            await Program.packetHandler.BroadcastPacket(buffer);
        }
    }
}
