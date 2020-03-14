using MediaSystem.Communications;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MediaSystem.DesktopClientWPF.Models
{
    //Responsibilities:
    //Detect running mediaservers
    //Request info from those detected servers
    //Raise event

    //Points to ponder
    //If there are several servers running, we will get multiple hits
    //We can know where to send the request by listening to returned datagrams

    /// <summary>
    /// Class responsible for detecting media devices on the local network
    /// </summary>
    public class DetectionService : IServerScanner
    {
        public event Action<DeviceInfo> DeviceDetected;

        private UdpClient _UDPClient = new UdpClient();

        /// <summary>
        /// True if broadcasts are being sent, false if not.
        /// </summary>
        public bool Enabled { get; set; } = true;

        public DetectionService()
        {
            //Wait 3 seconds for each receive call to finish, otherwise consider it failed
            _UDPClient.Client.ReceiveTimeout = 3000;
            _UDPClient.Ttl = 2;
        }

        /// <summary>
        /// Broadcasts a request to find available servers, if found, request info and raise DeviceDetected event
        /// </summary>
        public void ScanForDevice()
        {
            IPEndPoint newEP = new IPEndPoint(IPAddress.Broadcast, 8001);

            SessionLogger.LogEvent("Sending a broadcast ");

            byte[] q = Encoding.ASCII.GetBytes("!Hello");

            try
            {
                _UDPClient.Send(q, q.Length, newEP);

                try
                {
                    var response = Encoding.ASCII.GetString(_UDPClient.Receive(ref newEP));

                    if (response == "!IAmHere")
                    {
                        GetDeviceInfoAndFireEvent(newEP);
                    }
                }
                catch (SocketException)
                {
                    SessionLogger.LogEvent("No luck, try again");
                    return;
                }

            }
            catch (Exception e)
            {
                SessionLogger.LogEvent(e.Message);
                return;
            }
        }

        private void GetDeviceInfoAndFireEvent(IPEndPoint iPEnd)
        {

            DeviceSerializer ser = new DeviceSerializer();

            if (_UDPClient.Available <= 0 && Enabled)
            {
                try
                {
                    byte[] devicedata = SendAndReceiveRequest(iPEnd, "!Device");

                    NotifyDeviceDetected(ser.Deserialize(devicedata));
                }
                catch (Exception)
                {
                    SessionLogger.LogEvent("Failed to retrieve devicedata");
                    return;
                }
            }
        }

        private byte[] SendAndReceiveRequest(IPEndPoint iPEnd, string request)
        {
            byte[] q = Encoding.ASCII.GetBytes(request);

            _UDPClient.Send(q, q.Length, iPEnd);

             return _UDPClient.Receive(ref iPEnd);
        }

        private void NotifyDeviceDetected(DeviceInfo info)
        {
            this.DeviceDetected?.Invoke(info);
        }
    }
}