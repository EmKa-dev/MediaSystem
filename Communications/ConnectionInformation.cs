using System;

namespace MediaSystem.Communications
{
    [Serializable]
    public class ConnectionInformation
    {
        public string IPAddress { get; set; }
        public int Port { get; set; }
    }
}
