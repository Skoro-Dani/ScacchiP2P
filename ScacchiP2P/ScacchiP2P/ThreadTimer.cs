using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ScacchiP2P
{
    public class ThreadTimer
    {
        DatiCondivisi Dati;
        MainWindow w;
        System.Timers.Timer TimerA;
        System.Timers.Timer TimerU;
        public bool Timer { get; set; }
        public ThreadTimer()
        {
            Dati = DatiCondivisi.Istanza;
            w = Dati.w;
        }
        
        public void ProcThread()
        {
            while (!Dati.Flag)
            {
                if(Timer==true)
                w.RefreshTimer(TimerA.ToString(), TimerU.ToString()); ;
                Thread.Sleep(1000);
            }
        }

        public void setTimer(int tempo)
        {
            TimerA = new System.Timers.Timer(tempo);
            TimerU = new System.Timers.Timer(tempo);
        }

        public void startTA()
        {
            TimerA.Start();
        }
        public void startTU()
        {
            TimerU.Start();
        }
        public void stopTA()
        {
            TimerA.Stop();
        }
        public void stopTU()
        {
            TimerU.Stop();
        }
    }
}
