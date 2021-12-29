using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScacchiP2P
{
    public class DatiCondivisi
    {
        //istanza singleton
        private static DatiCondivisi istanza = null;
        private static object LockIstanza = new object();

        //attributi classe
        //Colore che si sta giocanto in questo momento
        private static object LockIColoreGiocante = new object();
        private string ColoreGiocante_;
        public string ColoreGiocante
        {
            get { lock (LockIColoreGiocante) { return ColoreGiocante_; } }
            set
            {
                lock (LockIColoreGiocante)
                {
                    ColoreGiocante_ = value;
                }
            }
        }
        //Scacchiera -> classe che contiene tutti i dati della scacchiera con le relative posizioni dei pezzi
        private static object LockIScacchiera = new object();
        private Scacchiera Scacchiera_;
        public Scacchiera Scacchiera
        {
            get { lock (LockIScacchiera) { return Scacchiera_; } }
            set
            {
                lock (LockIScacchiera)
                {
                    Scacchiera_ = value;
                }
            }
        }
        //lista Dati ricevuti dal listener
        private static object LockDatiRL = new object();
        private List<string> DatiRL_;
        public List<string> DatiRL { get { lock (LockDatiRL) { return DatiRL_; } } }
        //lista Dati Da inviare
        private static object LockDatiDI = new object();
        private List<string> DatiDI_;
        public List<string> DatiDI { get { lock (LockDatiDI) { return DatiDI_; } } }
        //lista Dati Ricevuti Dal WorkListener
        private static object LockDatiRWL = new object();
        private List<string> DatiRWL_;
        public List<string> DatiRWL { get { lock (LockDatiRWL) { return DatiRWL_; } } }
        //Flag programma finito
        private static object LockFlag = new object();
        private bool Flag_;
        public bool Flag { get { lock (LockFlag) { return Flag_; } }set { lock (LockFlag) { Flag_ = value; } } }
        //IpDestinatario
        private static object LockIP = new object();
        private string IP_;
        public string IP { get { lock (LockIP) { return IP_; } } set { lock (LockIP) { IP_ = value; } } }


        private DatiCondivisi()
        {
            ColoreGiocante = "bianco";
        }

        public static DatiCondivisi Istanza
        {
            get
            {
                lock (LockIstanza)
                {
                    if (istanza == null) istanza = new DatiCondivisi();
                    return istanza;
                }
            }
        }

        public void AddStringRL(string s)
        {
            lock (LockDatiRL)
            {
                DatiRL_.Add(s);
            }
        }
        public void AddStringDI(string s)
        {
            lock (LockDatiDI)
            {
                DatiDI_.Add(s);
            }
        }
        public void AddStringRWL(string s)
        {
            lock (LockDatiRWL)
            {
                DatiRWL_.Add(s);
            }
        }
    }
}
