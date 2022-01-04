namespace ScacchiP2P
{
    class ThreadGrafico
    {
        private Scacchiera sc = Scacchiera.Istanza;
        DatiCondivisi Dati = DatiCondivisi.Istanza;
        MainWindow w;
        public ThreadGrafico() { }


        public void ProcThread()
        {
            int count = 0;
            w = Dati.w;
            while (!Dati.Flag)
            {
                if (Dati.GetLengthRWL() > count)
                {
                    switch (Dati.GetRWLpos(count))
                    {
                        case "c":
                            break;
                    }
                    //Pulico il buffer di dati per alleggerire il programma
                    if (Dati.GetLengthRWL() > 10)
                    {
                        for (int i = 0; i < 10; i++)
                            Dati.DeletePosRWL(0);
                        count -= 10;
                    }
                }
            }
        }
    }
}
