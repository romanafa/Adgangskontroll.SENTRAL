using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Adgangskontroll.SENTRAL.TCP_kortleser
{
    internal class TCP
    {
        private TcpListener _server;
        private bool _isRunning;

        public TCPServer(string ipAddress, int port)
        {
            _server = new TcpListener(IPAddress.Parse(ipAddress), port);
            _server.Start();
            _isRunning = true;
        }

        public void Start()
        {
            try
            {
                while (_isRunning)
                {
                    Console.WriteLine("Server is waiting for a connection...");
                    TcpClient client = _server.AcceptTcpClient();
                    Console.WriteLine("Client connected!");

                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Received: " + receivedData);

                    // Echo the received data back to the client
                    byte[] responseData = Encoding.ASCII.GetBytes("Received: " + receivedData);
                    stream.Write(responseData, 0, responseData.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            finally
            {
                stream.Close();
                client.Close();
            }
        }
    }
}
