using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScacchiP2P
{
    class WorkListener
    {
        DatiCondivisi Dati=DatiCondivisi.Istanza;
        public WorkListener() { }

        public void ProcThread()
        {
            int count = 0;
            string[] s;
            while (!Dati.Flag)
            {
                if (Dati.DatiRL.Count() > count)
                {
                    s = Dati.DatiRL[count].Split(';');
                    switch (s[0])
                    {
                        case "c":
                            
                            break;
                    }
                    count++;
                }
            }
        }
    }
}
