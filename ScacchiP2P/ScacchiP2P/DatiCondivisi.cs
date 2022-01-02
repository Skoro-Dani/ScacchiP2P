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
        //Connesso-> booleano che indica se sono connesso
        private static object LockConnesso = new object();
        private bool Connesso_;
        public bool Connesso { get { lock (LockConnesso) { return Connesso_; } } set { lock (LockConnesso) { Connesso_ = value; } } }
        //ARConnessione -> Asppetto la risposta dalla richiesta di connessio booleano che indica se si sta aspettanto una risposta alla nostra connessione
        private bool ARConnessione_;
        public bool ARConnessione { get { return ARConnessione_; } set { ARConnessione_ = value; } }
        //VConnesione -> vuole connetersi booleano che indica se qualcuno sta richiedendo una connessione
        private bool VConnesione_;
        public bool VConnesione { get { return VConnesione_; } set { VConnesione_ = value; } }
        //IpVConnesione-> indica l'IP di chi sta cercando la connessione
        private string IPVC_;
        public string IPVC { get { return IPVC_; } set { IPVC_ = value; } }
        //Mi salvo mainwindow cosi che tutti ne possano usufruire
        private static object LockWindow = new object();
        private MainWindow w_;
        public MainWindow w { get { lock (LockWindow) { return w_; } } set { lock (LockWindow) { w_ = value; } } }
        //PartitaStart -> Booleano che indica se la partita puo partire
        private static object LockPartitaStart = new object();
        private bool PartitaStart_;
        public bool PartitaStart { get { lock (LockPartitaStart) { return PartitaStart_; } } set { lock (LockPartitaStart) { PartitaStart_ = value; } } }
        //Nome Del giocatore che gioca
        private static object LockNome = new object();
        private string Nome_;
        public string Nome { get { lock (LockNome) { return Nome_; } } set { lock (LockNome) { Nome_ = value; } } }
        //Count contatore delle regole
        private static object LockCount = new object();
        private int Count_;
        public int Count { get { lock (LockCount) { return Count_; } } set { lock (LockCount) { Count_ = value; } } }




        private DatiCondivisi()
        {
            AzzeraDati();
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
        public string GetRWLpos(int pos)
        {
            lock (LockDatiRWL)
            {
                return DatiRWL_[pos];
            }
        }
        public string GetRLpos(int pos)
        {
            lock (LockDatiRL)
            {
                return DatiRL_[pos];
            }
        }
        public string GetDIpos(int pos)
        {
            lock (LockDatiDI)
            {
                return DatiDI_[pos];
            }
        }
        public void DeletePosRWL(int pos)
        {
            lock (LockDatiRWL)
            {
                DatiRWL_.RemoveAt(pos);
            }
        }
        public void DeletePosRL(int pos)
        {
            lock (LockDatiRL)
            {
                DatiRL_.RemoveAt(pos);
            }
        }
        public void DeletePosDI(int pos)
        {
            lock (LockDatiDI)
            {
                DatiDI_.RemoveAt(pos);
            }
        }
        public int GetLengthRWL()
        {
            lock (LockDatiRWL)
            {
                return DatiRWL_.Count;
            }
        }
        public int GetLengthRL()
        {
            lock (LockDatiRL)
            {
                return DatiRL_.Count;
            }
        }
        public int GetLengthDI()
        {
            lock (LockDatiDI)
            {
                return DatiDI.Count;
            }
        }
        public void AzzeraDati()
        {
            Connesso_ = false;
            ARConnessione = false;
            VConnesione = false;
            IP = "";
            IPVC = "";
            Flag = false;
            w = MainWindow.GetMainWindow();
            PartitaStart = false;
            Count = 0;
        }
    }
}
