using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace MediaSystem.MediaServer
{
    public static class IPHelper
    {
        //Code courtesy of "Compman2408", https://stackoverflow.com/questions/6803073/get-local-ip-address
        public static string GetLocalIPv4(NetworkInterfaceType _type)
        {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            return output;
        }
    }
}
