using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ScacchiP2P
{
    public class Scacchiera
    {
        //istanza singleton
        private static Scacchiera istanza = null;
        private static object LockIstanza = new object();
        //Tipo di gioco Scacchi960 o standard
        private static object LockTipo = new object();
        private string TipoGioco_;
        public string TipoGioco { get { lock (LockTipo) { return TipoGioco_; } } set { lock (LockTipo) { TipoGioco_ = value; } } }
        //Matrice che replica la scacchiera
        private Pezzo[,] ScacchieraPezzi = new Pezzo[9, 9];
        //Colore->Colore che si sta giocando
        private static object LockColore = new object();
        private string Colore_;
        public string Colore { get { lock (LockColore) { return Colore_; } } set { lock (LockColore) { Colore_ = value; } } }
        //Tempo -> timer 
        private static object Locktempo = new object();
        private int tempo;
        //help-> booleano che indica se gli aiuti sono permessi
        private static object LockHelp = new object();
        private bool Help_;
        public bool Help { get { lock (LockHelp) { return Help_; } } set { lock (LockHelp) { Help_ = value; } } }
        //Punti-> booleano che indica se valgono i punti
        private static object LockPunti = new object();
        private bool Punti_;
        public bool Punti { get { lock (LockPunti) { return Punti_; } } set { lock (LockPunti) { Punti_ = value; } } }

        private Scacchiera()
        {
            Colore = "";
            TipoGioco = "standard";
            tempo = 0;
            GeneraScacchiera();
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
        public Pezzo getPezzo(int x,int y)
        {
            return ScacchieraPezzi[x, y];
        }
        public void setTimer(int minTempo)
        {
            lock (Locktempo)
            {
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
        public bool ControlloVittoria()
        {
            return false;
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

    }
}
