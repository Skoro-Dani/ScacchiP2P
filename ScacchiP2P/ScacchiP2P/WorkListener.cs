namespace ScacchiP2P
{
    class WorkListener
    {
        DatiCondivisi Dati;
        Scacchiera sc;
        MainWindow w;
        public WorkListener()
        {
            Dati = DatiCondivisi.Istanza;
            sc = Scacchiera.Istanza;
            w = Dati.w;
        }

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
                            if (!Dati.Connesso)
                                if (!Dati.ARConnessione)
                               {
                                    Dati.VConnesione = true;
                                    Dati.IPVC = s[2];
                                    w.RichiediConnessione(s[1]);
                               }
                            break;
                        case "m":
                            if (Dati.Connesso)
                            {
                                if (s[4] == "true")
                                {
                                    w.Patta();
                                }
                                else
                                {
                                    sc.Mossa(s[1], s[2]);
                                }
                            }
                            break;
                        case "ms":
                            if (Dati.Connesso)
                            {
                                Dati.PartitaStart = true;
                                w.PartitaStart();
                            }
                            break;
                        case "s":
                            if (Dati.Connesso)
                            {
                                w.SurrenderDellavversario();
                            }
                            break;
                        case "d":
                            if (Dati.Connesso)
                            {
                                w.Disconnessione();
                            }
                            break;
                        case "sc":
                            if (Dati.Connesso)
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
                                switch (s[2])
                                {
                                    case "0":
                                        sc.Help = true;
                                        sc.Timer = false;
                                        sc.Punti = false;
                                        break;
                                    case "1":
                                        sc.Help = false;
                                        sc.Timer = true;
                                        sc.Punti = true;
                                        sc.setTimer(10000);
                                        break;
                                    case "2":
                                        sc.Punti = false;
                                        int tempo = 0;
                                        int.TryParse(s[2], out tempo);
                                        if (tempo != 0) { sc.setTimer(tempo); sc.Timer = true; }
                                        else sc.Timer = false;
                                        if (s[3] == "true") sc.Help = true;
                                        else sc.Help = false;
                                        if (s[4] == "standard") sc.TipoGioco = "standard";
                                        else sc.TipoGioco = "scacchi960";
                                        break;
                                }
                            }
                            break;
                        case "fp":
                            if (Dati.Connesso)
                            {
                                sc.ControlloVittoria();
                            }
                            break;
                        case "a":
                            if (Dati.Connesso)
                            {
                                Dati.ResaA = true;
                            }
                            break;
                        case "y":
                            switch (s[1])
                            {
                                case "c":
                                    w.ConnesioneA(true);
                                    break;
                                case "r":
                                    w.RegoleA(true);
                                    break;
                            }
                            break;
                        case "n":
                            switch (s[1])
                            {
                                case "c":
                                    w.ConnesioneA(false);
                                    break;
                                case "r":
                                    w.RegoleA(false);
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
