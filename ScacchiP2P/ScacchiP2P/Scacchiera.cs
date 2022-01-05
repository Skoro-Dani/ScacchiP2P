using System.Collections.Generic;
using System.Threading;

namespace ScacchiP2P
{
    public class Scacchiera
    {
        ThreadTimer T = new ThreadTimer();
        Thread Ttimer;

        //istanza singleton
        private static Scacchiera istanza = null;
        private static object LockIstanza = new object();

        //Tipo di gioco Scacchi960 o standard
        private static object LockTipo = new object();
        private string TipoGioco_;
        public string TipoGioco { get { lock (LockTipo) { return TipoGioco_; } } set { lock (LockTipo) { TipoGioco_ = value; } } }

        //Matrice che replica la scacchiera
        private Pezzo[,] ScacchieraPezzi = new Pezzo[8, 8];
        //Colore->Colore che si sta giocando
        private static object LockColore = new object();
        private string Colore_;
        public string Colore { get { lock (LockColore) { return Colore_; } } set { lock (LockColore) { Colore_ = value; } } }
        //Tempo -> timer 
        private static object Locktempo = new object();
        private int tempo;
        private static object Locktimer = new object();
        private bool Timer_;
        public bool Timer { get { lock (Locktimer) { return Timer_; } } set { lock (Locktimer) { Timer_ = value; } } }
        //help-> booleano che indica se gli aiuti sono permessi
        private static object LockHelp = new object();
        private bool Help_;
        public bool Help { get { lock (LockHelp) { return Help_; } } set { lock (LockHelp) { Help_ = value; } } }
        //Punti-> booleano che indica se valgono i punti
        private static object LockPunti = new object();
        private bool Punti_;
        public bool Punti { get { lock (LockPunti) { return Punti_; } } set { lock (LockPunti) { Punti_ = value; } } }
        //MosseU->lista delle mosse attuate dall'utente
        private static object LockMossaBianco = new object();
        private List<string> MosseBianco;
        //MosseA->lista delle mosse attuate dall'avversario
        private static object LockMossaNero = new object();
        private List<string> MosseNero;
        private char[] alfabeto = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
        private IDictionary<char,int> alfabetorevers;
        //turno -> stringa che indica di chi è il turno
        private static object Lockturno = new object();
        private string Turno_;
        public string Turno { get { lock (Lockturno) { return Turno_; } } set { lock (Lockturno) { Turno_ = value; } } }
        private static object LockMossa = new object();
        private Scacchiera()
        {
            Colore = "";
            TipoGioco = "standard";
            tempo = 0;
            GeneraScacchiera();
            MosseBianco = new List<string>();
            MosseNero = new List<string>();
            alfabetorevers = new Dictionary<char, int>();
            for (int i = 0; i < 8; i++)
            {
                alfabetorevers[alfabeto[i]] = i;
            }

        }

        public static Scacchiera Istanza
        {
            get
            {
                lock (LockIstanza)
                {
                    if (istanza == null) istanza = new Scacchiera();

                    return istanza;
                }
            }
        }

        public void GeneraScacchiera()
        {
            if (TipoGioco.ToLower() == "standard")
            {
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        ScacchieraPezzi[x, y] = null;
                    }
                }
                for (int y = 0; y < 2; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        if (y == 1)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Pedone, Pezzo.inColore.Bianco);
                        else if (x == 0 || x == 7)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Torre, Pezzo.inColore.Bianco);
                        else if (x == 1 || x == 6)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Cavallo, Pezzo.inColore.Bianco);
                        else if (x == 2 || x == 5)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Alfiere, Pezzo.inColore.Bianco);
                        else if (x == 3)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Regina, Pezzo.inColore.Bianco);
                        else if (x == 4)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Re, Pezzo.inColore.Bianco);
                    }
                }
                for (int y = 6; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        if (y == 6)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Pedone, Pezzo.inColore.Nero);
                        else if (x == 0 || x == 7)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Torre, Pezzo.inColore.Nero);
                        else if (x == 1 || x == 6)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Cavallo, Pezzo.inColore.Nero);
                        else if (x == 2 || x == 5)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Alfiere, Pezzo.inColore.Nero);
                        else if (x == 3)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Regina, Pezzo.inColore.Nero);
                        else if (x == 4)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Re, Pezzo.inColore.Nero);
                    }
                }
            }
        }
        public Pezzo getPezzo(int x, int y)
        {
            return ScacchieraPezzi[x, y];
        }
        public void setTimer(int minTempo)
        {
            lock (Locktempo)
            {
                T.setTimer(minTempo);
                tempo = minTempo;
            }
        }
        public int getTimer()
        {
            lock (Locktempo)
            {
                return tempo;
            }
        }

        public void resetScacchiera()
        {
            GeneraScacchiera();
        }
        public void InvertiColore()
        {
            if (Colore == "bianco")
                Colore = "nero";
            else
                Colore = "bianco";
        }
        public void AddMosseU(string mossa)
        {
            lock (LockMossaBianco)
            {
                MosseBianco.Add(mossa);
            }
        }
        public void AddMosseA(string mossa)
        {
            lock (LockMossaNero)
            {
                MosseNero.Add(mossa);
            }
        }
        public List<string> GetListMosseU()
        {
            lock (LockMossaBianco)
            {
                return MosseBianco;
            }
        }
        public List<string> GetListMosseA()
        {
            lock (LockMossaNero)
            {
                return MosseNero;
            }
        }

        public void Mossa(string pos1, string pos2)
        {
            lock (Lockturno)
            {
                char[] s1 = new char[2];
                s1 = pos1.ToCharArray();
                Pezzo p1 = ScacchieraPezzi[alfabetorevers[s1[0]], s1[1]];
                char[] s2 = new char[2];
                s2 = pos1.ToCharArray();
                Pezzo p2 = ScacchieraPezzi[alfabetorevers[s1[0]], s1[1]];
                CPunto c1 = new CPunto(alfabetorevers[s1[0]], s1[1]);
                CPunto c2 = new CPunto(alfabetorevers[s2[0]], s2[1]);
                if (Turno == "bianco")
                {
                    if (p1.Colore == Pezzo.inColore.Bianco)
                    {
                        List<CPunto> posti = p1.DovePuoAndare(c1);
                        for (int i = 0; i < posti.Count; i++)
                        {
                            if (c2 == c1)
                            {
                                ScacchieraPezzi[alfabetorevers[s1[0]], s1[1]] = null;
                                ScacchieraPezzi[alfabetorevers[s2[0]], s2[1]] = p1;
                                if (Colore == Turno) AddMosseU(pos1 + "->" + pos2);
                                else AddMosseA(pos1 + "->" + pos2);
                            }
                        }
                    }
                }
                else if (Turno == "nero")
                {
                    if (p1.Colore == Pezzo.inColore.Nero)
                    {
                        List<CPunto> posti = p1.DovePuoAndare(c1);
                        for (int i = 0; i < posti.Count; i++)
                        {
                            if (c2 == c1)
                            {
                                ScacchieraPezzi[alfabetorevers[s1[0]], s1[1]] = null;
                                ScacchieraPezzi[alfabetorevers[s2[0]], s2[1]] = p1;
                                if (Colore == Turno) AddMosseU(pos1 + "->" + pos2);
                                else AddMosseA(pos1 + "->" + pos2);
                            }
                        }
                    }
                }
            }
            ControlloVittoria();
        }
        public void PartitaStart()
        {
            if(Timer==true)
            {
                if (Colore == "bianco")
                {
                    if (Turno == "bianco") T.startTU();
                    else T.startTA();
                }
                else
                {
                    if (Turno == "nero") T.startTA();
                    else T.startTU();
                }

            }
        }
        public void ControlloVittoria()
        {
            /*In Corso*/
        }

        public void Patta()
        {
            /*In Corso*/
        }
    }
}
