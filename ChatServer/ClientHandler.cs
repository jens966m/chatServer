using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChatServer
{
    public class ClientHandler
    {
        Thread clientThread;
        TcpClient newClient;
        NetworkStream nWS;
        StreamReader sR;
        StreamWriter sW;
        public TcpClient TcpClient { get { return newClient; } }

        public ClientHandler(TcpClient newClient)
        {
            this.newClient = newClient;
            nWS = newClient.GetStream();
            sR = new StreamReader(nWS);
            sW = new StreamWriter(nWS);

            StartClient();
        }

        public void HandleClient()
        {
            string newMessage;
            while (true)
            {
                try
                {
                newMessage = sR.ReadLine();
                Console.WriteLine(newMessage);
                BroadCasting.SendToAll(newMessage);
                }

                catch (Exception e)
                {
                 
                    Console.WriteLine("Client disconnected");
                    Console.WriteLine(e.Message);
                    BroadCasting.RemoveFromList(newClient);

                    clientThread.Abort();
                    
                    
                    
                }




            }

        }

        public void StartClient()
        {
            clientThread = new Thread(HandleClient);
            clientThread.Start();
            
        }

        public void SendToClient(string toClient)
        {
            sW.WriteLine(toClient);
            sW.Flush();

        }

    }
}
