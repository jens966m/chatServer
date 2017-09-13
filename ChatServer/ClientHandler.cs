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
        Auktion auktion;
        public TcpClient TcpClient { get { return newClient; } }

        public ClientHandler(TcpClient newClient, Auktion auktion)
        {
            this.newClient = newClient;
            nWS = newClient.GetStream();
            sR = new StreamReader(nWS);
            sW = new StreamWriter(nWS);
            this.auktion = auktion;
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
                string[] messageArray = newMessage.Split(',');
                    if (messageArray[0] == "chat")
                    {
                        Console.WriteLine(newMessage);
                        BroadCasting.SendToAll(newMessage);
                    }
                    else if (messageArray[0] == "bet")
                    {
                        if (auktion.done != true)
                        {
                            lock (auktion.lockObj)
                            {
                                if (int.Parse(messageArray[2]) > auktion.Bet)
                                {
                                    auktion.gavelCount = 0;
                                    auktion.Bet = int.Parse(messageArray[2]);
                                    auktion.LastBetter = messageArray[1];
                                    string message = "bet," + messageArray[1] + "," + messageArray[2];
                                    BroadCasting.SendToAll(message);
                                }
                                else
                                {
                                    sW.WriteLine("Error,Ugyldigt Bud");
                                    sW.Flush();

                                }

                            }
                        }
                        else
                        {
                            sW.WriteLine("gavel,Aktionen er slut");
                        }
                    }
                    else if (messageArray[0] == "StartUp")
                    {
                        string message = "StartUp," + auktion.Item + "," + auktion.LastBetter + "," + auktion.Bet + ","+ auktion.StartPrice;
                        sW.WriteLine(message);
                        sW.Flush();
                    }
                    else
                    {

                    }
                
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
