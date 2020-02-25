using MediaSystem.Communications;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TcpServerBaseLibrary.Interface;

namespace MediaSystem.MediaServer
{
    /// <summary>
    /// Got the whole responsibility of listening to incoming udp-requests and returning connectioninfo
    /// </summary>
    public class UdpServer
    {
        private UdpClient _udpClient;

        private ILogger _logger;

        private readonly LocalContentReader _Lcr = new LocalContentReader(ConfigReader.GetContentFolderPathFromConfig());

        public bool CurrentlyListening { get; private set; } = false;

        public UdpServer(ILogger logger, int port)
        {
            _logger = logger;
            _udpClient = new UdpClient(port, AddressFamily.InterNetwork);
        }

        public void ListenForMessages()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0000);

            _logger.Info("Starts listening for datagram");

            this.CurrentlyListening = true;

            IAsyncResult r = _udpClient.BeginReceive(new AsyncCallback(EndRec), _udpClient);

            void EndRec(IAsyncResult ar)
            {
                byte[] m = _udpClient.EndReceive(ar, ref ep);

                string res = Encoding.ASCII.GetString(m);

                if (res == "!Hello")
                {
                    _logger.Debug("!Hello message received, notifying presence with a response");

                    SendPresenceResponseAsync(ep);
                }

                if (res == "!Device")
                {
                    _logger.Debug("!Device message received, Sending device (serailized) data as datagram");

                    SendDeviceInfoAsync(ep);
                }

                CurrentlyListening = false;
            }
        }

        private async void SendPresenceResponseAsync(IPEndPoint iPEnd)
        {
            byte[] d = Encoding.ASCII.GetBytes("!IAmHere");

            await _udpClient.SendAsync(d, d.Length, iPEnd);
        }

        private async void SendDeviceInfoAsync(IPEndPoint iPEnd)
        {
            DeviceSerializer ser = new DeviceSerializer();

            //TODO: Detect IP and port
            Device device = new Device
            {
                MediaType = _Lcr.MediaType,

                ConnectionInfo = new ConnectionInformation
                {
                    IPAddress = IPHelper.GetLocalIPv4(System.Net.NetworkInformation.NetworkInterfaceType.Ethernet),
                    Port = 8001
                },
           
                CoverInfo = new CoverInformation
                (
                    _Lcr.MediaType.ToString(),
                    _Lcr.NumberOfFiles,
                    _Lcr.GetCoverImageData()
                ),

                MediaFiles = _Lcr.GetContentInfo()
            };

            //Serialize a device object
            byte[] datatosend = ser.Serialize(device);

            _logger.Debug($"Sending device data : {datatosend.Length} bytes");

            //Because of the limitations presented by using a udp solution, we need to enforce a max size for the cover-image.
            //Trying to send too large images is unreliable and will likely fail.
            await _udpClient.SendAsync(datatosend, datatosend.Length, iPEnd);
        }
    }
}
