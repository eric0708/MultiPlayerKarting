using System;
using System.Collections.Generic;
using System.Text;

namespace gameserver
{
    class Constants
    {
        public const string serverIPaddress = "127.0.0.1";
        public const int serverPort = 8888;
        public const int MaxPlayerNumber = 10;
        public const int TICKS_PER_SEC = 30;
        public const int MS_PER_TICK = 1000 / TICKS_PER_SEC;
    }
}
