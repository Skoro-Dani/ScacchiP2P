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

        private int tempoU;
        private int tempoA;
        private bool boolU;
        private bool boolA;

        public int minutiU_ { get { return minutiU; } }
        public int minutiA_ { get { return minutiA; } }
        public int tempoU_ { get { return tempoU; } }
        public int tempoA_ { get { return tempoA; } }
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
                        
                        if(tempoA<0)
                        {
                            tempoA = 60;
                            minutiA--;
                        }
                        tempoA--;
                    }
                    else if (boolU == true)
                    {
                        
                        if (tempoU < 0)
                        {
                            tempoU = 60;
                            tempoU--;
                        }
                        tempoU--;
                    }

                    w.RefreshTimer(minutiA+"."+tempoA.ToString(), minutiU + "." + tempoU.ToString());
                }
                Thread.Sleep(1000);
            }
        }

        public void setTimer(int tempo)
        {
            w = MainWindow.GetMainWindow();
            minutiA = tempo;
            minutiU = tempo;
            tempoU = 0;
            tempoA = 0;
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
