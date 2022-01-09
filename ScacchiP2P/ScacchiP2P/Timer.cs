using System;
using System.Threading;
using System.Timers;

namespace ScacchiP2P
{
    public class Timer
    {
        DatiCondivisi Dati;
        MainWindow w;
        System.Timers.Timer TimerA;
        System.Timers.Timer TimerU;
        public bool Timer_ { get; set; }
        
        public Timer()
        {
            Dati = DatiCondivisi.Istanza;
            w = MainWindow.GetMainWindow();

            TimerA = new System.Timers.Timer();
            TimerU= new System.Timers.Timer();

            TimerA.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            TimerU.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            w=MainWindow.GetMainWindow();
            Console.WriteLine("Ciao");
            w.RefreshTimer(TimerA.Interval.ToString(), TimerU.Interval.ToString());
        }

        public void ProcThread()
        {
            while (Dati.Flag == false)
            {
                if (Timer_ == true)
                {
                    /*Console.WriteLine("sono dentro");
                    w.RefreshTimer(TimerA.ToString(), TimerU.ToString()); ;
                    Thread.Sleep(1000);*/
                }
            }
        }

        public void setTimer(int tempo)
        {
            TimerA.Interval = tempo;
            TimerU.Interval = tempo;
            Timer_ = true;
        }

        public void startTA()
        {
            TimerA.Enabled = true;
        }
        public void startTU()
        {
            TimerU.Enabled = true;
        }
        public void stopTA()
        {
            //TimerA.Enabled = false;
        }
        public void stopTU()
        {
            //TimerU.Enabled = false;
        }
    }
}
