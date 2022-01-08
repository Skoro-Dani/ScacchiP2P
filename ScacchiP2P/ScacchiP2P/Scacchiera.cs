using System;
using System.Collections.Generic;
using System.Threading;

namespace ScacchiP2P
{
    public class Scacchiera
    {
        //Dichiarazione dati Neccesari
        ThreadTimer T;
        DatiCondivisi Dati;
        MainWindow w;
        DatiGiocatore dg;
        private Thread TimerT;
        /*
         * 
         */
        //istanza singleton
        private static Scacchiera istanza = null;
        private static object LockIstanza = new object();
        /*
         * 
         */
        //Matrice che replica la scacchiera
        private static object Locksc = new object();
        private Pezzo[,] sc_;
        public Pezzo[,] ScacchieraPezzi { get { lock (Locksc) { return sc_; } } set { lock (Locksc) { sc_ = value; } } }
        /*
         * 
         */
        //Colore->Colore che si sta giocando
        private static object LockColore = new object();
        private string Colore_;
        public string Colore { get { lock (LockColore) { return Colore_; } } set { lock (LockColore) { Colore_ = value; } } }
        /*Regole
         * 
         * 
         */
        //Tipo di gioco Scacchi960 o standard
        private static object LockTipo = new object();
        private string TipoGioco_;
        public string TipoGioco { get { lock (LockTipo) { return TipoGioco_; } } set { lock (LockTipo) { TipoGioco_ = value; } } }
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
        /*
         * 
         * 
         */
        //MosseU->lista delle mosse attuate dall'utente
        private static object LockMossaBianco = new object();
        private List<string> MosseBianco;
        //MosseA->lista delle mosse attuate dall'avversario
        private static object LockMossaNero = new object();
        private List<string> MosseNero;
        /*
         * 
         */
        //alfabeto
        private char[] alfabeto = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
        private IDictionary<char, int> alfabetorevers;
        /*
         * 
         */
        //turno -> bool che indica se è il turno dell'avversario
        private static object Lockturno = new object();
        private bool TurnoAvv_;
        public bool TurnoAvv { get { lock (Lockturno) { return TurnoAvv_; } } set { lock (Lockturno) { TurnoAvv_ = value; } } }
        /*
         * 
         */
        //lock
        private static object LockMossa = new object();
        /*
         * VittoriaAvv-> Booleano che indica chi ha vinto
         */
        private bool VittoriaAvv { get; set; }
        /*
        * Patta->Indica se si vuole la patta
        */
        private bool APatta { get; set; }
        /*
        * Arresa->Indica se si vuole la resa
        */
        private bool Arresa { get; set; }
        /*
        * Pstart->Indica se la partita è iniziata
        */
        private static object LockPstart = new object();
        private bool Pstart_;
        public bool Pstart { get { lock (LockPstart) { return Pstart_; } } set { lock (LockPstart) { Pstart_ = value; } } }

        private Scacchiera()
        {
            T = new ThreadTimer();
            TimerT = new Thread(new ThreadStart(T.ProcThread));
            TimerT.Start();
            sc_ = new Pezzo[8, 8];
            Arresa = false;
            APatta = false;
            Colore = "bianco";
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

            Dati = DatiCondivisi.Istanza;
            w = Dati.w;
            w = MainWindow.GetMainWindow();
        }
        public void setWindow(MainWindow w)
        {
            this.w = w;
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
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    ScacchieraPezzi[x, y] = null;
                }
            }
            if (TipoGioco.ToLower() == "standard")
            {
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
            /*else
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        if (y == 1)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Pedone, Pezzo.inColore.Bianco);
                        else if (x == 0 || x == 5)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Alfiere, Pezzo.inColore.Bianco);
                        else if (x == 1)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Regina, Pezzo.inColore.Bianco);
                        else if (x == 2 || x == 4)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Torre, Pezzo.inColore.Bianco);
                        else if (x == 3)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Re, Pezzo.inColore.Bianco);
                        else if (x == 6 || x == 7)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Cavallo, Pezzo.inColore.Bianco);
                    }
                }
                for (int y = 6; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        if (y == 6)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Pedone, Pezzo.inColore.Nero);
                        else if (x == 0 || x == 5)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Alfiere, Pezzo.inColore.Nero);
                        else if (x == 1)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Regina, Pezzo.inColore.Nero);
                        else if (x == 2 || x == 4)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Torre, Pezzo.inColore.Nero);
                        else if (x == 3)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Re, Pezzo.inColore.Nero);
                        else if (x == 6 || x == 7)
                            ScacchieraPezzi[x, y] = new Pezzo(Pezzo.InizialePezzo.Cavallo, Pezzo.inColore.Nero);
                    }
                }
            }*/
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

        //metodo per muovere un pezzo
        public void Mossa(string pos1, string pos2)
        {
            lock (LockMossa)
            {
                //dichiarazione variabili
                Pezzo p1, p2;
                char[] s1, s2;
                //prendo il primo pezzo
                s1 = new char[2];
                s1 = pos1.ToCharArray();
                int po1, po2;
                po1 = alfabetorevers[s1[0]];
                po2 = int.Parse(s1[1].ToString());
                p1 = ScacchieraPezzi[po1,po2];

                //controllo il secondo pezzo
                s2 = new char[2];
                s2 = pos2.ToCharArray();
                int pob1, pob2;
                pob1 = alfabetorevers[s1[0]];
                pob2 = int.Parse(s2[1].ToString());

                if (ScacchieraPezzi[pob1,pob2 ] != null)
                    p2 = ScacchieraPezzi[pob1, pob2];
                else p2 = null;

                //posizione poezzo 1
                CPunto c1 = new CPunto(po1, po2, false, true);

                //posizione pezzo 2
                CPunto c2 = new CPunto(pob1, pob2, false, true);

                //lista dei posti in cui puo andare c1
                List<CPunto> posti = GetPosizioni(c1, p1);
                for (int i = 0; i < posti.Count; i++)
                {
                    if (p2 == null)
                    {
                        if (c2.equal(posti[i])==true && posti[i].PosInCuiMuove == true)
                        {
                            ScacchieraPezzi[po1,po2] = null;
                            ScacchieraPezzi[pob1, pob2] = p1;
                            InvertiTurno(pos1, pos2);
                        }
                    }
                    else
                    {
                        if (c2.equal(posti[i]) && posti[i].PosInCuiMangia == true)
                        {
                            ScacchieraPezzi[po1,po2] = null;
                            ScacchieraPezzi[pob1, pob2] = p1;
                            InvertiTurno(pos1, pos2);
                        }
                    }
                }

            }
        }

        //metodo che restituisce tutte le posizioni in cui puo andare un pezzo
        private List<CPunto> GetPosizioneEffetive(CPunto Punto, Pezzo P)
        {
            List<CPunto> Pos = new List<CPunto>();

            switch (P.Nome)
            {
                case Pezzo.InizialePezzo.Cavallo:
                    Pos = GetPosCavallo(Punto, P);
                    break;
                case Pezzo.InizialePezzo.Torre:
                    Pos = GetPosTorre(Punto, P);
                    break;
                case Pezzo.InizialePezzo.Pedone:
                    Pos = GetPosPedone(Punto, P);
                    break;
                case Pezzo.InizialePezzo.Alfiere:
                    Pos = GetPosAlfiere(Punto, P);
                    break;
                case Pezzo.InizialePezzo.Regina:
                    Pos = GetPosRegina(Punto, P);
                    break;
                case Pezzo.InizialePezzo.Re:
                    Pos = GetPosRe(Punto, P);

                    break;
            }

            return Pos;
        }

        //metodo in cui restituisce tutte le posizioni dove mangiano i pezzi avversari
        private List<CPunto> PosInCuiSiMangiaNemico(Pezzo[,] Sc, Pezzo.inColore ColoreNemico)
        {
            List<CPunto> pos = new List<CPunto>();
            List<CPunto> posProv = new List<CPunto>();
            //Ciclo che percorre tutta la scacchiera
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    //controllo che non sia vuoto
                    if (Sc[x, y] != null)
                    {
                        //Guardo se è il nemico
                        if (Sc[x, y].Colore == ColoreNemico)
                        {
                            //prendo le posizioni effettive
                            posProv.AddRange(GetPosizioneEffetive(new CPunto(x, y, true, true), Sc[x, y]));
                            //posProv = GetPosizioneEffetive(new CPunto(x, y, true, true), Sc[x, y]);
                            //
                            /*for (int i = 0; i < posProv.Count; i++)
                            {
                                if (posProv[i].PosInCuiMangia == true)
                                    pos.Add(posProv[i]);
                            }*/
                        }
                    }
                }
            }
            //Ciclo che toglie tutte le posizioni in cui non si mangia
            for (int i = 0; i < posProv.Count; i++)
            {
                if (posProv[i].PosInCuiMangia == true)
                    pos.Add(posProv[i]);
            }
            return pos;
        }

        //metodo il quale restituisce un booleano per capire se una determinata mossa causerà scacco
        private bool CausaScacco(CPunto Pos1, CPunto Pos2, Pezzo P, Pezzo[,] sc)
        {
            bool Scacco = false;
            List<CPunto> PosDovePuoAndare = GetPosizioneEffetive(Pos1, P);
            List<CPunto> PosDoveMangiano;
            //Posizione Re
            CPunto posRe = new CPunto(-1, -1, true, true);
            //Ciclo che percorre tutta la scacchiera
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (sc[x, y] != null)
                        //Controllo che il pezzo si un re e che il colore sia uguale al pezzo
                        if (sc[x, y].Nome == Pezzo.InizialePezzo.Re && sc[x, y].Colore == P.Colore)
                        {
                            posRe.x = x;
                            posRe.y = y;
                        }
                }
            }
            //decido chi sarà il nemico a seconda del colore del pezzo
            if (P.Colore == Pezzo.inColore.Bianco)
                PosDoveMangiano = PosInCuiSiMangiaNemico(sc, Pezzo.inColore.Nero);
            else
                PosDoveMangiano = PosInCuiSiMangiaNemico(sc, Pezzo.inColore.Bianco);
            //ciclo che percorre tutte le posizioni dove mangiano
            for (int i = 0; i < PosDoveMangiano.Count; i++)
            {
                if (sc[PosDoveMangiano[i].x, PosDoveMangiano[i].y] == sc[posRe.x, posRe.y])
                    Scacco = true;
            }

            return Scacco;
        }

        //metodo pubblico per ottenere le posizioni
        public List<CPunto> GetPosizioni(CPunto Punto, Pezzo P)
        {
            List<CPunto> pos = new List<CPunto>();
            List<CPunto> posProv = GetPosizioneEffetive(Punto, P);

            for (int i = 0; i < posProv.Count; i++)
            {
                if (CausaScacco(Punto, posProv[i], P, ScacchieraPezzi) == false)
                    pos.Add(posProv[i]);
            }

            return pos;
        }
        public void PartitaStart()
        {
            if (Colore == "bianco") TurnoAvv = false;
            else TurnoAvv = true;

            if (Timer)
            {
                T.Timer = true;

                if (TurnoAvv) T.startTA();
                else T.startTU();
            }
            else T.Timer = false;
            GeneraScacchiera();
            Pstart = true;
        }
        public void ControlloVittoria(Pezzo[,] sc)
        {
            bool risControlloScaccoN = true;
            bool risControlloScaccoB = true;

            List<CPunto> pos;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (sc[x, y] != null)
                    {
                        //controllo per i neri
                        if (sc[x, y].Colore == Pezzo.inColore.Nero)
                        {
                            pos = GetPosizioni(new CPunto(x, y, true, true), sc[x, y]);
                            if (pos.Count > 0)
                                risControlloScaccoN = false;
                            //controllo per i Bianchi
                        }
                        else if (sc[x, y].Colore == Pezzo.inColore.Bianco)
                        {
                            pos = GetPosizioni(new CPunto(x, y, true, true), sc[x, y]);
                            if (pos.Count > 0)
                                risControlloScaccoB = false;
                        }
                    }

                }
            }

            if (risControlloScaccoB == true)
            {
                if (Colore == "bianco") VittoriaAvv = true;
                else VittoriaAvv = false;
                FinePartita();
            }
            if (risControlloScaccoN == true)
            {
                if (Colore == "nero") VittoriaAvv = true;
                else VittoriaAvv = false;
                FinePartita();
            }

        }

        public void Patta()
        {
            APatta = true;
            FinePartita();
        }

        public void FinePartita()
        {
            if (Punti == true)
            {
                if (APatta != true)
                {
                    if (Arresa != true)
                    {
                        if (VittoriaAvv == false) dg.Punti += 100;
                        else dg.Punti -= 100;
                    }
                    else dg.Punti -= 100;
                }
            }
            if (Timer == true)
            {
                T.stopTA();
                T.startTU();
                T.setTimer(tempo);
            }
            GeneraScacchiera();
            APatta = false;
            Arresa = false;
            Pstart = false;
            w.Rivincita();
        }

        public void ArresaMet(bool ArresaAvve)
        {
            if (ArresaAvve == false)
                Arresa = true;
            else Arresa = false;
            FinePartita();
        }
        private void InvertiTurno(string pos1, string pos2)
        {
            if (TurnoAvv == false)
            {
                AddMosseU(pos1 + "->" + pos2);
                TurnoAvv = true;
                if (Timer == true)
                {
                    T.stopTU();
                    T.startTA();
                }
                w.DisOrAnb(false);
            }
            else
            {
                AddMosseA(pos1 + "->" + pos2);
                TurnoAvv = false;
                if (Timer == true)
                {
                    T.stopTA();
                    T.startTU();
                }
                w.DisOrAnb(true);
            }

        }

        private List<CPunto> GetPosCavallo(CPunto Punto, Pezzo P)
        {
            List<CPunto> Pos = new List<CPunto>();

            //A destra in alto
            try
            {
                if (ScacchieraPezzi[Punto.x + 2, Punto.y + 1] != null)
                {
                    if (ScacchieraPezzi[Punto.x + 2, Punto.y + 1].Colore == P.Colore)
                        Pos.Add(new CPunto(Punto.x + 2, Punto.y + 1, true, true));
                }
                else Pos.Add(new CPunto(Punto.x + 2, Punto.y + 1, true, true));
            }
            catch (Exception e) { Console.WriteLine("Pos Non Valida in Cavallo (A destra in alto)"); }
            //A destra in basso
            try
            {
                if (ScacchieraPezzi[Punto.x + 2, Punto.y - 1] != null)
                {
                    if (ScacchieraPezzi[Punto.x + 2, Punto.y - 1].Colore == P.Colore)
                        Pos.Add(new CPunto(Punto.x + 2, Punto.y - 1, true, true));
                }
                else Pos.Add(new CPunto(Punto.x + 2, Punto.y - 1, true, true));
            }
            catch (Exception e) { Console.WriteLine("Pos Non Valida in Cavallo (A destra in basso)"); }
            //A sinistra in Alto
            try
            {
                if (ScacchieraPezzi[Punto.x - 2, Punto.y + 1] != null)
                {
                    if (ScacchieraPezzi[Punto.x - 2, Punto.y + 1].Colore == P.Colore)
                        Pos.Add(new CPunto(Punto.x - 2, Punto.y + 1, true, true));
                }
                else Pos.Add(new CPunto(Punto.x - 2, Punto.y + 1, true, true));
            }
            catch (Exception e) { Console.WriteLine("Pos Non Valida in Cavallo (A sinistra in alto)"); }
            //A sinistra in basso
            try
            {
                if (ScacchieraPezzi[Punto.x - 2, Punto.y - 1] != null)
                {
                    if (ScacchieraPezzi[Punto.x - 2, Punto.y - 1].Colore == P.Colore)
                        Pos.Add(new CPunto(Punto.x - 2, Punto.y - 1, true, true));
                }
                else Pos.Add(new CPunto(Punto.x - 2, Punto.y - 1, true, true));
            }
            catch (Exception e) { Console.WriteLine("Pos Non Valida in Cavallo (A destra in basso)"); }
            //In alto a destra
            try
            {
                if (ScacchieraPezzi[Punto.x + 1, Punto.y + 2] != null)
                {
                    if (ScacchieraPezzi[Punto.x + 1, Punto.y + 2].Colore == P.Colore)
                        Pos.Add(new CPunto(Punto.x + 1, Punto.y + 2, true, true));
                }
                else Pos.Add(new CPunto(Punto.x + 1, Punto.y + 2, true, true));
            }
            catch (Exception e) { Console.WriteLine("Pos Non Valida in Cavallo (In Alto a destra)"); }
            { }
            //In alto a sinistra
            try
            {
                if (ScacchieraPezzi[Punto.x - 1, Punto.y + 2] != null)
                {
                    if (ScacchieraPezzi[Punto.x - 1, Punto.y + 2].Colore == P.Colore)
                        Pos.Add(new CPunto(Punto.x - 1, Punto.y + 2, true, true));
                }
                else Pos.Add(new CPunto(Punto.x - 1, Punto.y + 2, true, true));
            }
            catch (Exception e) { Console.WriteLine("Pos Non Valida in Cavallo (In Alto a sinistra)"); }
            //In basso a destra
            try
            {
                if (ScacchieraPezzi[Punto.x + 1, Punto.y - 2] != null)
                {
                    if (ScacchieraPezzi[Punto.x + 1, Punto.y - 2].Colore == P.Colore)
                        Pos.Add(new CPunto(Punto.x + 1, Punto.y - 2, true, true));
                }
                else Pos.Add(new CPunto(Punto.x + 1, Punto.y - 2, true, true));
            }
            catch (Exception e) { Console.WriteLine("Pos Non Valida in Cavallo (In basso a destra)"); }
            //In basso a sinistra
            try
            {
                if (ScacchieraPezzi[Punto.x - 1, Punto.y - 2] != null)
                {
                    if (ScacchieraPezzi[Punto.x - 1, Punto.y - 2].Colore == P.Colore)
                        Pos.Add(new CPunto(Punto.x - 1, Punto.y - 2, true, true));
                }
                else Pos.Add(new CPunto(Punto.x - 1, Punto.y - 2, true, true));
            }
            catch (Exception e) { Console.WriteLine("Pos Non Valida in Cavallo (In basso a sinistra)"); }


            return Pos;
        }
        private List<CPunto> GetPosTorre(CPunto Punto, Pezzo P)
        {
            int count = 0;
            List<CPunto> Pos = new List<CPunto>();
            //Controllo Torre a destra
            count = 0;
            for (int x = Punto.x + 1; x < 8; x++)
            {
                try
                {
                    if (ScacchieraPezzi[x, Punto.y] != null)
                    {
                        if (count == 0)
                        {
                            if (ScacchieraPezzi[x, Punto.y].Colore != P.Colore)
                                Pos.Add(new CPunto(x, Punto.y, true, true));
                            count++;
                        }
                    }
                    else
                    {
                        if (count == 0)
                            Pos.Add(new CPunto(x, Punto.y, true, true));
                    }
                }
                catch (Exception e) { Console.WriteLine("Pos Non Valida in Torre (A destra)"); }
            }
            //Controllo della torre a Sinistra
            count = 0;
            for (int x = Punto.x - 1; x > 0; x--)
            {
                try
                {
                    if (ScacchieraPezzi[x, Punto.y] != null)
                    {
                        if (count == 0)
                        {
                            if (ScacchieraPezzi[x, Punto.y].Colore != P.Colore)
                                Pos.Add(new CPunto(x, Punto.y, true, true));
                            count++;
                        }
                    }
                    else
                    {
                        if (count == 0)
                            Pos.Add(new CPunto(x, Punto.y, true, true));
                    }
                }
                catch (Exception e) { Console.WriteLine("Pos Non Valida in Torre (A sinistra)"); }
            }
            //Controllo della torre in alto
            count = 0;
            for (int y = Punto.y + 1; y < 8; y++)
            {
                try
                {
                    if (ScacchieraPezzi[Punto.x, y] != null)
                    {
                        if (count == 0)
                        {
                            if (ScacchieraPezzi[Punto.x, y].Colore != P.Colore)
                                Pos.Add(new CPunto(Punto.x, y, true, true));
                            count++;
                        }
                    }
                    else
                    {
                        if (count == 0)
                            Pos.Add(new CPunto(Punto.x, y, true, true));
                    }
                }
                catch (Exception e) { Console.WriteLine("Pos Non Valida in Torre (In alto)"); }
            }
            //controllo della torre in basso
            count = 0;
            for (int y = Punto.y - 1; y > 0; y--)
            {
                try
                {
                    if (ScacchieraPezzi[Punto.x, y] != null)
                    {
                        if (count == 0)
                        {
                            if (ScacchieraPezzi[Punto.x, y].Colore != P.Colore)
                                Pos.Add(new CPunto(Punto.x, y, true, true));
                            count++;
                        }
                    }
                    else
                    {
                        if (count == 0)
                            Pos.Add(new CPunto(Punto.x, y, true, true));
                    }
                }
                catch (Exception e) { Console.WriteLine("Pos Non Valida in Torre (In basso)"); }
            }
            return Pos;
        }
        private List<CPunto> GetPosPedone(CPunto Punto, Pezzo P)
        {
            List<CPunto> Pos = new List<CPunto>();
            //Se bianco il pedone si muove in un modo
            if (P.Colore == Pezzo.inColore.Bianco)
            {
                //avanti con il pedone
                try
                {
                    if (ScacchieraPezzi[Punto.x, Punto.y + 1] == null)
                    {
                        Pos.Add(new CPunto(Punto.x, Punto.y + 1, false, true));
                        if (ScacchieraPezzi[Punto.x, Punto.y + 2] == null)
                            Pos.Add(new CPunto(Punto.x, Punto.y + 2, false, true));
                    }
                }
                catch (Exception e) { Console.WriteLine("Pos Non Valida in pedone (Avanti Bianco)"); }
                //dove mangia il pedone
                try
                {
                    if (ScacchieraPezzi[Punto.x + 1, Punto.y + 1] != null)
                    {
                        if (P.Colore != ScacchieraPezzi[Punto.x + 1, Punto.y + 1].Colore)
                            Pos.Add(new CPunto(Punto.x + 1, Punto.y + 1, true, false));
                    }
                }
                catch (Exception e) { Console.WriteLine("Pos Non Valida in pedone (Mangia Destra Bianco)"); }
                try
                {
                    if (ScacchieraPezzi[Punto.x - 1, Punto.y + 1] != null)
                    {
                        if (P.Colore != ScacchieraPezzi[Punto.x - 1, Punto.y + 1].Colore)
                            Pos.Add(new CPunto(Punto.x - 1, Punto.y + 1, true, false));
                    }
                }
                catch (Exception e) { Console.WriteLine("Pos Non Valida in pedone (Mangia Sinistra Bianco)"); }
            }
            //Se nero il pedone si muove in un modo
            else
            {
                try
                {
                    if (ScacchieraPezzi[Punto.x, Punto.y - 1] == null)
                    {
                        //avanti con il pedone
                        Pos.Add(new CPunto(Punto.x, Punto.y - 1, false, true));
                        if ((ScacchieraPezzi[Punto.x, Punto.y - 2] == null))
                            Pos.Add(new CPunto(Punto.x, Punto.y - 2, false, true));
                    }
                }
                catch (Exception e) { Console.WriteLine("Pos Non Valida in pedone (Avanti Nero)"); }
                //dove mangia il pedone
                try
                {
                    if (ScacchieraPezzi[Punto.x - 1, Punto.y - 1] != null)
                    {
                        if (P.Colore != ScacchieraPezzi[Punto.x - 1, Punto.y - 1].Colore)
                            Pos.Add(new CPunto(Punto.x - 1, Punto.y - 1, true, false));
                    }
                }
                catch (Exception e) { Console.WriteLine("Pos Non Valida in pedone (Mangia Destro Nero)"); }
                try
                {
                    if (ScacchieraPezzi[Punto.x + 1, Punto.y - 1] != null)
                    {
                        if (P.Colore != ScacchieraPezzi[Punto.x + 1, Punto.y - 1].Colore)
                            Pos.Add(new CPunto(Punto.x + 1, Punto.y - 1, true, false));
                    }
                }
                catch (Exception e) { Console.WriteLine("Pos Non Valida in pedone (Mangia Sinistro Nero)"); }
            }
            return Pos;
        }
        private List<CPunto> GetPosAlfiere(CPunto Punto, Pezzo P)
        {
            List<CPunto> Pos = new List<CPunto>();
            int ix = 0, iy = 0;
            int count = 0;
            //punti alfiere alto a destra
            ix = Punto.x;
            iy = Punto.y;
            count = 0;
            while (ix < 8 && iy < 8)
            {
                try
                {
                    ix++;
                    iy++;
                    if (ScacchieraPezzi[ix, iy] != null)
                    {
                        if (P.Colore != ScacchieraPezzi[ix, iy].Colore)
                        {
                            if (count == 0)
                            {
                                Pos.Add(new CPunto(ix, iy, true, true));
                            }
                        }
                            count++;

                    }
                    else if (count == 0)
                    {
                        Pos.Add(new CPunto(ix, iy, true, true));
                    }
                }
                catch (Exception e)
                { }
            }
            //punti alfiere in basso a sinistra
            ix = Punto.x;
            iy = Punto.y;
            count = 0;
            while (ix > -1 && iy > -1)
            {
                try
                {
                    ix--;
                    iy--;
                    if (ScacchieraPezzi[ix, iy] != null)
                    {
                        if (P.Colore != ScacchieraPezzi[ix, iy].Colore)
                        {
                            if (count == 0)
                            {
                                Pos.Add(new CPunto(ix, iy, true, true));
                            }
                        }
                            count++;

                    }
                    else if (count == 0)
                    {
                        Pos.Add(new CPunto(ix, iy, true, true));
                    }
                }
                catch (Exception e)
                { }
            }
            //punti alfiere in alto a sinistra
            ix = Punto.x;
            iy = Punto.y;
            count = 0;
            while (ix > -1 && iy < 8)
            {
                try
                {
                    ix--;
                    iy++;
                    if (ScacchieraPezzi[ix, iy] != null)
                    {
                        if (P.Colore != ScacchieraPezzi[ix, iy].Colore)
                        {
                            if (count == 0)
                            {
                                Pos.Add(new CPunto(ix, iy, true, true));
                            }
                        }
                            count++;

                    }
                    else if (count == 0)
                    {
                        Pos.Add(new CPunto(ix, iy, true, true));
                    }
                }
                catch (Exception e)
                { }
            }
            //punti alfiere in basso a destra
            ix = Punto.x;
            iy = Punto.y;
            count = 0;
            while (ix < 8 && iy > -1)
            {
                try
                {
                    ix++;
                    iy--;
                    if (ScacchieraPezzi[ix, iy] != null)
                    {
                        if (P.Colore != ScacchieraPezzi[ix, iy].Colore)
                        {
                            if (count == 0)
                            {
                                Pos.Add(new CPunto(ix, iy, true, true));
                            }
                        }
                            count++;

                    }
                    else if (count == 0)
                    {
                        Pos.Add(new CPunto(ix, iy, true, true));
                    }
                }
                catch (Exception e)
                { }
            }
            return Pos;
        }
        private List<CPunto> GetPosRegina(CPunto Punto, Pezzo P)
        {
            List<CPunto> posAlfiere = GetPosAlfiere(Punto, P);
            List<CPunto> posTorre = GetPosTorre(Punto, P);
            List<CPunto> Pos = new List<CPunto>();
            Pos.AddRange(posTorre);
            Pos.AddRange(posAlfiere);

            return Pos;
        }
        private List<CPunto> GetPosRe(CPunto Punto, Pezzo P)
        {
            List<CPunto> Pos = new List<CPunto>();
            //Punti re
            //controllo che non viene mangiato
            //In avanti
            try
            {
                if (ScacchieraPezzi[Punto.x, Punto.y + 1] == null)
                {
                    Pos.Add(new CPunto(Punto.x, Punto.y + 1, true, true));
                }
                else if (ScacchieraPezzi[Punto.x, Punto.y + 1].Colore != P.Colore)
                {
                    Pos.Add(new CPunto(Punto.x, Punto.y + 1, true, true));
                }
            }
            catch (Exception e) { Console.WriteLine("Pos Non Valida in Re (Avanti)"); }
            //Diagonale avanti destra
            try
            {
                if (ScacchieraPezzi[Punto.x + 1, Punto.y + 1] == null)
                {
                    Pos.Add(new CPunto(Punto.x + 1, Punto.y + 1, true, true));
                }
                else if (ScacchieraPezzi[Punto.x + 1, Punto.y + 1].Colore != P.Colore)
                {
                    Pos.Add(new CPunto(Punto.x + 1, Punto.y + 1, true, true));
                }
            }
            catch (Exception e) { Console.WriteLine("Pos Non Valida in Re (Diagonale avanti destra)"); }
            //A destra
            try
            {
                if (ScacchieraPezzi[Punto.x + 1, Punto.y] == null)
                {
                    Pos.Add(new CPunto(Punto.x + 1, Punto.y, true, true));
                }
                else if (ScacchieraPezzi[Punto.x + 1, Punto.y].Colore != P.Colore)
                {
                    Pos.Add(new CPunto(Punto.x + 1, Punto.y, true, true));
                }
            }
            catch (Exception e) { Console.WriteLine("Pos Non Valida in Re (A destra)"); }
            //Diagonale indietro a destro
            try
            {
                if (ScacchieraPezzi[Punto.x + 1, Punto.y - 1] == null)
                {
                    Pos.Add(new CPunto(Punto.x + 1, Punto.y - 1, true, true));
                }
                else if (ScacchieraPezzi[Punto.x + 1, Punto.y - 1].Colore != P.Colore)
                {
                    Pos.Add(new CPunto(Punto.x + 1, Punto.y - 1, true, true));
                }
            }
            catch (Exception e) { Console.WriteLine("Pos Non Valida in Re (Diagonale indietro destra)"); }
            //Indietro
            try
            {
                if (ScacchieraPezzi[Punto.x, Punto.y - 1] == null)
                {
                    Pos.Add(new CPunto(Punto.x, Punto.y - 1, true, true));
                }
                else if (ScacchieraPezzi[Punto.x, Punto.y - 1].Colore != P.Colore)
                {
                    Pos.Add(new CPunto(Punto.x, Punto.y - 1, true, true));
                }
            }
            catch (Exception e) { Console.WriteLine("Pos Non Valida in Re (Indietro)"); }
            //Diagonale indietro sinistra
            try
            {
                if (ScacchieraPezzi[Punto.x - 1, Punto.y - 1] == null)
                {
                    Pos.Add(new CPunto(Punto.x - 1, Punto.y - 1, true, true));
                }
                else if (ScacchieraPezzi[Punto.x - 1, Punto.y - 1].Colore != P.Colore)
                {
                    Pos.Add(new CPunto(Punto.x - 1, Punto.y - 1, true, true));
                }
            }
            catch (Exception e) { Console.WriteLine("Pos Non Valida in Re (Diagonale indietro sinistra)"); }
            //A sinistra
            try
            {
                if (ScacchieraPezzi[Punto.x - 1, Punto.y] == null)
                {
                    Pos.Add(new CPunto(Punto.x - 1, Punto.y, true, true));
                }
                else if (ScacchieraPezzi[Punto.x - 1, Punto.y].Colore != P.Colore)
                {
                    Pos.Add(new CPunto(Punto.x - 1, Punto.y, true, true));
                }
            }
            catch (Exception e) { Console.WriteLine("Pos Non Valida in Re (A sinistra)"); }
            //Diagonale avanti a sinistra
            try
            {
                if (ScacchieraPezzi[Punto.x - 1, Punto.y + 1] == null)
                {
                    Pos.Add(new CPunto(Punto.x - 1, Punto.y + 1, true, true));
                }
                else if (ScacchieraPezzi[Punto.x - 1, Punto.y + 1].Colore != P.Colore)
                {
                    Pos.Add(new CPunto(Punto.x - 1, Punto.y + 1, true, true));
                }
            }
            catch (Exception e) { Console.WriteLine("Pos Non Valida in Re (Diagonale avanti a sinistra)"); }
            return Pos;
        }
    }
}