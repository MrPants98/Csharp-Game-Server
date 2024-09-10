using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Unity_Game_Server
{
    internal class GameServer
    {
        public delegate void PacketReceivedHandler(string packet);

        public event PacketReceivedHandler OnPacketReceived;

        public string url { get; private set; }
        public UInt16 port { get; private set; }

        private HttpListener httpListener = new HttpListener();
        public List<WebSocket> connectedClients { get; private set; }

        public async void InitServer()
        {
            string uri = "http://" + url + ":" + port + "/";
            httpListener.Prefixes.Add(uri);
            httpListener.Start();
            Console.WriteLine($"Server started. Join with {uri}");

            while(true)
            {
                HttpListenerContext context = await httpListener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                    await HandleConnection(context);
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
        }

        private async Task HandleConnection(HttpListenerContext context)
        {
            HttpListenerWebSocketContext socketContext = await context.AcceptWebSocketAsync(null);
            WebSocket webSocket = socketContext.WebSocket;

            connectedClients.Add(webSocket);
            Console.WriteLine("New Client Connected");

            try
            {
                await ReceiveMessages(webSocket);
            } catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            } finally
            {
                connectedClients.Remove(webSocket);
                Console.WriteLine("A Client Disconnected");
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                webSocket.Dispose();
            }
        }

        private async Task ReceiveMessages(WebSocket webSocket)
        {
            byte[] buffer = new byte[1024 * 4];
            WebSocketReceiveResult result;

            while (webSocket.State == WebSocketState.Open)
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    OnPacketReceived?.Invoke(message);
                }
            }
        }

        public GameServer(string? _url, UInt16 _port)
        {
            url = _url;
            port = _port;
            connectedClients = new List<WebSocket>();
        }
    }
}