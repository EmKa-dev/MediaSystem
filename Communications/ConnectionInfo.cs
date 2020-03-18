using System;

namespace MediaSystem.Communications
{
    [Serializable]
    public class ConnectionInfo
    {
        public string IPAddress { get; set; }
        public int Port { get; set; }
    }
}
