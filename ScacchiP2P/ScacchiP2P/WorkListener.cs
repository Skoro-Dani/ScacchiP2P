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
        Scacchiera sc = Scacchiera.Istanza;
        public WorkListener() { }

        public void ProcThread()
        {
            int count = 0;
            string[] s;
            while (!Dati.Flag)
            {
                if (Dati.GetLengthRL() > count)
                {
                    s = Dati.DatiRL[count].Split(';');
                    switch (s[0])
                    {
                        case "c":
                            if(!Dati.Connesso)
                                if(!Dati.ARConnessione)
                                {
                                    Dati.VConnesione = true;
                                    Dati.IPVC = s[2];
                                    Dati.AddStringRWL("c;" + s[1]);
                                }
                            break;
                        case "m":
                            if (Dati.Connesso)
                            {
                                Dati.AddStringRWL(Dati.DatiRL[count]);
                            }
                            break;
                        case "d":
                            if (Dati.Connesso)
                            {
                                Dati.AzzeraDati();
                            }
                            break;
                        case "sc":
                            if(Dati.Connesso)
                            {
                                if (s[1] == "bianco")
                                    sc.Colore = "nero";
                                else
                                    sc.Colore = "bianco";
                            }
                            break;
                        case "r":
                            if (Dati.Connesso)
                            {
                                if (int.Parse(s[1]) < 5)
                                {
                                    switch (s[2])
                                    {
                                        case "0":
                                            Dati.AddStringRWL("r;" + s[1] + ";0;");
                                            break;
                                        case "1":
                                            Dati.AddStringRWL("r;" + s[1] + ";1;");
                                            break;
                                        case "2":
                                            Dati.AddStringRWL("r;" + s[1] + ";2;"+s[3]+";"+s[4]+";"+s[5]);
                                            break;
                                    }
                                }
                                else
                                {
                                    Dati.AddStringDI("d;");
                                    Dati.AddStringRWL("d;La Connessione verrà chiusa perchè i due partecipanti non riuscivano a mettersi d'accordo sulle regole");
                                }
                            }
                            break;
                        case "fp":
                            Dati.AddStringRWL("fp");
                            break;
                        case "a":
                            Dati.AddStringRWL("a");
                            break;
                        case "y":
                            switch(s[1])
                            {
                                case "c":
                                    Dati.IP = Dati.IPVC;
                                    Dati.Connesso = true;
                                    Dati.ARConnessione = false;
                                    Dati.IPVC = "";
                                    Dati.VConnesione = false;
                                    Dati.AddStringRWL("c;" + s[2] + ";Ha accettato la tua partita");
                                    break;
                                case "r":
                                    Dati.AddStringRWL("r;y;Le regole sono state accettate");
                                    break;
                                case "a":
                                    sc.InvertiColore();
                                    sc.resetScacchiera();
                                    Dati.PartitaStart = true;
                                    break;
                            }
                            break;
                        case "n":
                            switch (s[1])
                            {
                                case "c":
                                    Dati.AzzeraDati();
                                    Dati.AddStringRWL("c;Ha Rifiutato la tua partita");
                                    break;
                                case "r":
                                    if (s[2] == "l")
                                    {
                                        //???? da chiarire
                                    }
                                    else if (s[2] == "d")
                                        Dati.AddStringRL("d;" + Dati.IP);
                                    break;
                                case "a":
                                    Dati.AddStringRL("d;" + Dati.IP);
                                    break;
                            }
                            break;
                    }
                    count++;

                    //Pulico il buffer di dati per alleggerire il programma
                    if (Dati.GetLengthRL() > 10)
                    {
                        for (int i = 0; i < 10; i++)
                            Dati.DeletePosRL(0);
                        count -= 10;
                    }
                }
            }
        }
    }
}
