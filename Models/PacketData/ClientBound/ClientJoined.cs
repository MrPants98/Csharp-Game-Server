using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity_Game_Server.Models.PacketData.ClientBound
{
    public class ClientJoined
    {
        public string username { get; set; }
        public byte color { get; set; }
    }
}