using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;

namespace Unity_Game_Server.Models
{
    public class WebsocketClient
    {
        public WebSocket socket { get; private set; }
        public UInt16 clientID { get; private set; }

        public async Task SendPacket(byte[] packetBuffer)
        {
            await socket.SendAsync(new ArraySegment<byte>(packetBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public WebsocketClient(WebSocket _socket, UInt16 _clientID)
        {
            socket = _socket;
            clientID = _clientID;
        }
    }
}