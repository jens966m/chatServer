using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Program MyProgram = new Program();
            MyProgram.Run();
        }

        public void Run()
        {

            TcpListener listener = new TcpListener(IPAddress.Any, 11000);
            listener.Start();
            TcpClient newClient;
            ClientHandler clientHandler;
            
            while (true)
            {
                newClient = listener.AcceptTcpClient();
                Console.WriteLine("Client Connected !");
                clientHandler = new ClientHandler(newClient);
                BroadCasting.AddToList(clientHandler);
                               

            }



        }
    }
}
