using System;
using System.Threading;

namespace ScacchiP2P
{
    public class Timer
    {
        DatiCondivisi Dati;
        MainWindow w;
        public bool Timer_ { get; set; }

        private int minutiU;
        private int minutiA;

        private int secondiU;
        private int secondiA;
        private bool boolU;
        private bool boolA;

        public int minutiU_ { get { return minutiU; } }
        public int minutiA_ { get { return minutiA; } }
        public int tempoU_ { get { return secondiU; } }
        public int tempoA_ { get { return secondiA; } }
        public Timer()
        {
            Dati = DatiCondivisi.Istanza;
            w = MainWindow.GetMainWindow();
        }

        public void ProcThread()
        {
            while (Dati.Flag == false)
            {
                if (Timer_ == true)
                {
                    if (boolA == true)
                    {
                        secondiA--;
                        if (secondiA<0)
                        {
                            minutiA--;
                            secondiA = 60;
                            
                        }
                        
                    }
                    else if (boolU == true)
                    {
                        secondiU--;
                        if (secondiU < 0)
                        {
                            secondiU = 60;
                            minutiU--;
                        }
                        
                    }

                    w.RefreshTimer(minutiA+"."+secondiA.ToString(), minutiU + "." + secondiU.ToString());
                }
                Thread.Sleep(1000);
            }
        }

        public void setTimer(int tempo)
        {
            w = MainWindow.GetMainWindow();
            minutiA = tempo;
            minutiU = tempo;
            secondiU = 0;
            secondiA = 0;
        }

        public void startTA()
        {
            boolA = true;
        }
        public void startTU()
        {
            boolU = true;
        }

        public void stopTA()
        {
            boolA = false;
        }
        public void stopTU()
        {
            boolU = false;
        }

    }
}
