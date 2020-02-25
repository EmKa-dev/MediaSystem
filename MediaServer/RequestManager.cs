using System;
using System.Text;
using TcpServerBaseLibrary.Core;
using TcpServerBaseLibrary.Interface;

namespace MediaSystem.MediaServer
{
    public class RequestManager : IMessageManager
    {
        private ILogger _logger;

        public RequestManager(ILogger logger)
        {
            _logger = logger;
        }

        public void HandleMessage(MessageObject obj)
        {
            _logger.Debug("Request received");

            SendFileData(obj);
        }

        private void SendFileData(MessageObject message)
        {
            string filename = Encoding.ASCII.GetString(message.CompleteData);

            LocalContentReader lcr = new LocalContentReader(ConfigReader.GetContentFolderPathFromConfig());

            Span<byte> b = lcr.GetSpecifiedFileData(filename);

            try
            {
                //TODO, make call async for sending large files.
                _logger.Debug($"sending requested file data:{filename}, {b.Length} bytes");
                message.UsedConnection.Send(b);

                _logger.Debug("Send successful");
            }
            catch (Exception)
            {
                _logger.Debug("Send unsuccessful");
                throw;
            }
        }
    }
}
