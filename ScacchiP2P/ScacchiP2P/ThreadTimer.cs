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
        DatiCondivisi Dati = DatiCondivisi.Istanza;
        Scacchiera sc = Scacchiera.Istanza;
        MainWindow w;
        System.Timers.Timer TimerA;
        System.Timers.Timer TimerU;
        public ThreadTimer()
        {
            w = Dati.w;
        }
        
        public void ProcThread()
        {
            while (Dati.PartitaF == false && Dati.PartitaStart == true)
            {
                if(sc.Timer==true)
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
