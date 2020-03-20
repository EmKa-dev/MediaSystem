using MediaSystem.Communications;
using MediaSystem.DesktopClientWPF.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TcpServerBaseLibrary;

namespace MediaSystem.DesktopClientWPF.Models
{
    public class DownloadService : IDownloadService
    {
        private readonly string DownloadTempFolder;

        private ILogger _logger;

        public DownloadService(ILogger logger)
        {
            _logger = logger;
            DownloadTempFolder = GetTempFolder();
        }

        private string GetTempFolder()
        {
            var tmppath = Path.GetTempPath();

            var d = Directory.CreateDirectory(@$"{tmppath}\MediaSystemClient");

            return d.FullName;
        }

        public async Task<Uri> DownloadFileDataAsync(MediaFileInfo file, IPEndPoint iPEnd)
        {
            if (iPEnd == null)
            {
                return null;
            }

            string requestmessage = file.FileName;

            try
            {

                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                sock.Connect(iPEnd);

                //Build the header
                ApplicationProtocolHeader header = new ApplicationProtocolHeader
                {
                    Lenght = Encoding.ASCII.GetBytes(requestmessage).Length,

                    MessageTypeIdentifier = (int)MessageType.REQUEST
                };

                //Send the header
                sock.Send(header.WrapHeaderData());

                //Retrieve header acknowledegment
                byte[] headerbuffer = new byte[8];

                sock.Receive(headerbuffer);

                ApplicationProtocolHeader responseheader = new ApplicationProtocolHeader(headerbuffer);

                if (!responseheader.Equals(header))
                {
                    _logger.LogError("Failed to retrieve file data : Response header does not match");

                    return null;
                }

                sock.Send(Encoding.ASCII.GetBytes(requestmessage));

                List<byte> bytelist = new List<byte>();

                _logger.LogDebug($"Start downloading file into temp folder..");

                await Task.Run(() =>
                {
                    //Count check prevents the evaluation from returning false before the server had a chance to send anything
                    while (sock.Available > 0 || bytelist.Count == 0)
                    {
                        //Just for caution we clamp the max number of bytes we can download at a time.
                        byte[] by = new byte[sock.Available.Clamp(1, 4096)];
                        sock.Receive(by);
                        bytelist.AddRange(by);
                    }
                });

                File.WriteAllBytes($@"{DownloadTempFolder}\{file.FileName}", bytelist.ToArray());

                Uri uri = new Uri($@"{DownloadTempFolder}\{file.FileName}");

                sock.Shutdown(SocketShutdown.Both);
                sock.Close();

                return uri;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Uri DownloadFileData(MediaFileInfo file, IPEndPoint iPEnd)
        {
            if (iPEnd == null)
            {
                return null;
            }

            string requestmessage = file.FileName;

            try
            {

                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                sock.Connect(iPEnd);

                //Build the header
                ApplicationProtocolHeader header = new ApplicationProtocolHeader
                {
                    Lenght = Encoding.ASCII.GetBytes(requestmessage).Length,

                    MessageTypeIdentifier = (int)MessageType.REQUEST
                };

                //Send the header
                sock.Send(header.WrapHeaderData());

                //Retrieve header acknowledegment
                byte[] headerbuffer = new byte[8];

                sock.Receive(headerbuffer);

                ApplicationProtocolHeader responseheader = new ApplicationProtocolHeader(headerbuffer);

                if (!responseheader.Equals(header))
                {
                    _logger.LogError("Failed to retrieve file data : Response header does not match");
                    return null;
                }

                sock.Send(Encoding.ASCII.GetBytes(requestmessage));

                List<byte> bytelist = new List<byte>();

                _logger.LogDebug($"Start downloading file into temp folder..");

                //Count check prevents the evaluation from returning false before the server had a chance to send anything
                while (sock.Available > 0 || bytelist.Count == 0)
                {
                    //Just for caution we clamp the max number of bytes we can download at a time.
                    byte[] by = new byte[sock.Available.Clamp(1, 4096)];
                    sock.Receive(by);
                    bytelist.AddRange(by);
                }

                File.WriteAllBytes($@"{DownloadTempFolder}\{file.FileName}", bytelist.ToArray());

                Uri uri = new Uri($@"{DownloadTempFolder}\{file.FileName}");

                sock.Shutdown(SocketShutdown.Both);
                sock.Close();

                return uri;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
