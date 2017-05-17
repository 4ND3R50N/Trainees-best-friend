using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteCode.Network;

namespace tbfContentManager.Classes
{
    abstract class Manager
    {
        abstract public int iUser_ID();
        abstract public ref SimpleNetwork_Client TCPClient();
        abstract public string sUser_name();
        abstract public List<string> sRoomData();


    }
}
