using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScacchiP2P
{
    public class ControllaVittoria
    {
        DatiCondivisi Dati;
        Scacchiera sc;

        public ControllaVittoria()
        {
            Dati = DatiCondivisi.Istanza;
            sc = Scacchiera.Istanza;
        }

        public void ProcThread()
        {
            while(Dati.Flag==false)
            {
                if (sc.Pstart)
                    sc.ControlloVittoria(sc.ScacchieraPezzi);
                Thread.Sleep(1000);
            }
        }

    }
}
