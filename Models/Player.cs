namespace Unity_Game_Server.Models
{
    public class Player
    {
        public string username { get; private set; }
        public UInt64 uuid { get; private set; }
        public byte color { get; private set; }
        public WebsocketClient socketClient;


        public Player(string _username, UInt64 _uuid, byte _color, WebsocketClient _socketClient)
        {
            username = _username;
            uuid = _uuid;
            color = _color;
            socketClient = _socketClient;
        }
    }
}