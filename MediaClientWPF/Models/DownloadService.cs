﻿using MediaSystem.Communications;
using MediaSystem.DesktopClientWPF.Extensions;
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
    public static class DownloadService
    {
        public static readonly string DownloadTempFolder = CreateTempFolder();

        private static string CreateTempFolder()
        {
            var d = Directory.CreateDirectory(@".\DownloadTemp");

            //Clean directory
            foreach (var file in Directory.EnumerateFiles(d.FullName))
            {
                File.Delete(file);
            }

            return d.FullName;
        }

        public static async Task<Uri> DownloadFileDataFromServerAsync(MediaFileInfo file, IPEndPoint iPEnd)
        {
            if (iPEnd == null)
            {
                SessionLogger.LogEvent("Can't download data, no connection info provided. (If testing with mocks, this is expected)");
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
                    SessionLogger.LogEvent("Failed to retrieve file data : Response header does not match");
                    return null;
                }

                sock.Send(Encoding.ASCII.GetBytes(requestmessage));

                List<byte> bytelist = new List<byte>();

                SessionLogger.LogEvent($"Start downloading file into temp folder..");

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

        public static Uri DownloadFileDataFromServerSync(MediaFileInfo file, IPEndPoint iPEnd)
        {
            if (iPEnd == null)
            {
                SessionLogger.LogEvent("Can't download data, no connection info provided. (If testing with mocks, this is expected)");
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
                    SessionLogger.LogEvent("Failed to retrieve file data : Response header does not match");
                    return null;
                }

                sock.Send(Encoding.ASCII.GetBytes(requestmessage));

                List<byte> bytelist = new List<byte>();

                SessionLogger.LogEvent($"Start downloading file into temp folder..");

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