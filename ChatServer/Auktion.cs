using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ChatServer
{
    public class Auktion
    {
        Thread auktionThread;
        public int Bet;
        public int StartPrice;
        public string LastBetter = "Nobody";
        public string Item;
        public object lockObj = new object();
        public int gavelCount = 0;
        public bool done = false;
        
        public Auktion()
        {
            Random rand = new Random();
            StartPrice = rand.Next(100, 200);
            Bet = 0;
            LastBetter = "Nobody";
            Item = "maleri";
            StartAuktion();
        }

        public void runningAuktion()
        {
            
            while (true)
            {
                
                if (Bet != 0)
                {
                    while (true)
                    {
                        Thread.Sleep(1000);
                        
                        lock (lockObj)
                        {
                            gavelCount++;
                            if (gavelCount == 10)
                            {
                                BroadCasting.SendToAll("Gavel,Første gang");
                            }
                            else if(gavelCount == 15)
                            {
                                BroadCasting.SendToAll("Gavel,Anden gang");
                            }
                            else if (gavelCount == 18)
                            {
                                BroadCasting.SendToAll("Gavel,Tredje gang");
                                BroadCasting.SendToAll("Gavel,Solgt til " + LastBetter + " for " + Bet + " kr");
                                done = true;
                            }
                        }
                        

                    }
                }
            }
        }
        void StartAuktion()
        {
            auktionThread = new Thread(runningAuktion);
            auktionThread.Start();
        }

    }
}
