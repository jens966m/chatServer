using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;
using System.Net.Sockets;


namespace ChatServer
{
    static class BroadCasting
    {
        static List<ClientHandler> clientList = new List<ClientHandler>();


        static public void AddToList(ClientHandler client)
        {

            clientList.Add(client);
        }

        static public void SendToAll(string messageToAll)
        {

            foreach (ClientHandler client in clientList)
            {
                client.SendToClient(messageToAll);
                

            }
            
        }

        static public void RemoveFromList(TcpClient clientToRemove)
        {
            clientList.Remove(clientList.Find(x => x.TcpClient.Client.RemoteEndPoint == clientToRemove.Client.RemoteEndPoint));

        }





    }
}
