using System;
using System.Net;
using System.Net.Sockets;
using Utility;

namespace Airbrake.Net.Sockets.SocketException
{
    class Program
    {
        static void Main(string[] args)
        {
            // Working example, localhost with default port 80.
            ConnectToSocket("127.0.0.1");

            // Invalid example, localhost with inactive port 4444.
            ConnectToSocket("127.0.0.1:4444");

            // Invalid example, Google DNS with default port 80.
            ConnectToSocket("8.8.8.8");
        }

        private static IPEndPoint ParseIPEndPoint(string server)
        {
            Uri uri;
            // Try to generate a valid URI from server.
            if (Uri.TryCreate(server, UriKind.Absolute, out uri) || 
                Uri.TryCreate(String.Concat("http://", server), UriKind.Absolute, out uri))
            {
                // Create new IPEndPoint with parsed Uri arguments.
                return new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port);
            }
            // If Uri creation fails, invalid server parameter was passed.
            throw new FormatException($"Invalid server provided to ParseIPEndPoint of: {server}");
        }

        private static void ConnectToSocket(string uri)
        {
            try
            {
                // Create IPEndPoint.
                IPEndPoint endPoint = ParseIPEndPoint(uri);
                // Create socket from EndPoint arguments.
                var socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // Attempt connection.
                socket.Connect(endPoint);
                // Confirm connection with output.
                Logging.Log($"Connection established to: {endPoint.Address}:{endPoint.Port}");
                // Close connection.
                socket.Close();
            }
            catch (System.Net.Sockets.SocketException exception)
            {
                Logging.Log(exception);
            }
        }
    }
}
