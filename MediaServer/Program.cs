using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TcpServerBaseLibrary.Core;
using TcpServerBaseLibrary.Interface;

namespace MediaSystem.MediaServer
{
    class Program
    {

        static ConsoleLogger logger = new ConsoleLogger();

        static Dictionary<int, IMessageManager> Pairs = new Dictionary<int, IMessageManager>()
            {
                { (int)Communications.MessageType.REQUEST, new RequestManager(logger) }
            };


        static UdpServer Udplistener = new UdpServer(logger, 8001);

        static void Main(string[] args)
        {
            
            Task.Run(() =>
            {
                //TODO: Add a timer of sorts so we can control how often and when this runs.
                while (true)
                {
                    if (!Udplistener.CurrentlyListening)
                    {
                        Udplistener.ListenForMessages();
                    }
                }
            });

            var Server = new TCPServer(logger, 8001, Pairs, 2);

            Server.Start();

            Console.ReadKey();
        }
    }
}
