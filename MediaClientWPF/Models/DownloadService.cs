using MediaSystem.Communications;
using MediaSystem.DesktopClientWPF.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TcpServerBaseLibrary;

namespace MediaSystem.DesktopClientWPF.Models
{
    public class DownloadService : IDownloadService, IDisposable
    {
        private readonly string _downloadTempFolder;

        private ILogger _logger;

        public DownloadService(ILogger logger)
        {
            _logger = logger;
            _downloadTempFolder = GetTempFolder();
        }

        private string GetTempFolder()
        {
            var tmppath = Path.GetTempPath();

            var d = Directory.CreateDirectory(@$"{tmppath}\MediaSystemClient");

            return d.FullName;
        }

        public Uri DownloadFileData(MediaFileInfo file, IPEndPoint iPEnd)
        {
            Uri uri;

            if (TryCheckTempFolderForFile(file.FileName, out uri))
            {
                return uri;
            }

            if (iPEnd == null)
            {
                return null;
            }

            string requestmessage = file.FileName;

            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {

                sock.Connect(iPEnd);

                //Send header and receive response, according to protocol
                if (!IsMessageHeaderAccepted(requestmessage, sock))
                {
                    _logger.LogError($"Failed to retrieve file data for \"{requestmessage}\" : Response header does not match");
                    return null;
                }

                _logger.LogDebug($"Start downloading file into temp folder..");

                List<byte> bytelist = RequestAndReceiveFileData(requestmessage ,sock);

                sock.Shutdown(SocketShutdown.Both);
                sock.Close();

                //Write new file to temp-folder and get uri
                File.WriteAllBytes($@"{_downloadTempFolder}\{file.FileName}", bytelist.ToArray());

                uri = new Uri($@"{_downloadTempFolder}\{file.FileName}");

                return uri;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<byte> RequestAndReceiveFileData(string requestmessage, Socket sock)
        {
            sock.Send(Encoding.ASCII.GetBytes(requestmessage));

            List<byte> bytelist = new List<byte>();

            //Count check prevents the evaluation from returning false before the server had a chance to send anything
            while (sock.Available > 0 || bytelist.Count == 0)
            {
                //Just for caution we clamp the max number of bytes we can download at a time.
                byte[] by = new byte[sock.Available.Clamp(1, 4096)];
                sock.Receive(by);
                bytelist.AddRange(by);
            }

            return bytelist;
        }

        private bool IsMessageHeaderAccepted(string requestmessage, Socket sock)
        {
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
                return false;
            }

            return true;
        }

        private bool TryCheckTempFolderForFile(string filename, out Uri uri)
        {
            if (File.Exists($@"{_downloadTempFolder}\{filename}"))
            {
                _logger.LogDebug($"\"{filename}\" already exists in temp-folder, no download needed");

                uri = new Uri($@"{_downloadTempFolder}\{filename}");
                return true;
            }

            uri = null;
            return false;
        }

        public void Dispose()
        {
            foreach (var file in Directory.GetFiles(_downloadTempFolder))
            {
                File.Delete(file);
            }

            _logger.LogDebug("Temp-files deleted");
        }
    }
}
