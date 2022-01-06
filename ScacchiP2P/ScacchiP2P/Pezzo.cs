namespace ScacchiP2P
{
    public class Pezzo
    {
        private InizialePezzo _Nome;
        private inColore _Colore;
        public InizialePezzo Nome
        {
            get { return _Nome; }
            set
            {
                _Nome = value;
            }
        }
        public inColore Colore
        {
            get { return _Colore; }
            set
            {
                _Colore = value;
            }
        }
        private string img_;
        public string img { get { return img_; } set { img_ = value; } }

        public Pezzo()
        {
            Nome = InizialePezzo.Vuoto;
            Colore = inColore.nulla;
        }

        public Pezzo(InizialePezzo Nome, inColore Colore)
        {
            this.Nome = Nome;
            this.Colore = Colore;
            img = "component/PNGPezzi/ReBiancoS.jpg";
        }
        public enum InizialePezzo
        {
            Pedone = 'P',
            Cavallo = 'C',
            Alfiere = 'B',
            Torre = 'R',
            Regina = 'Q',
            Re = 'K',
            Vuoto = ' ',
        }
        public enum inColore
        {
            Bianco = 'b',
            Nero = 'n',
            nulla = ' ',
        }


        /*public List<CPunto> DovePuoAndare(CPunto P)
        {
            int ix = 0;
            int iy = 0;
            int x, y;
            x = P.x;
            y = P.y;
            List<CPunto> ris = null;
            switch (Nome)
            {
                case InizialePezzo.Pedone:
                    //punti pedone
                    if (Colore == inColore.Bianco)
                    {
                        ris.Add(new CPunto(x, y + 1, false, true));
                        if (y == 2)
                        { ris.Add(new CPunto(x, y + 2, false, true)); }
                        ris.Add(new CPunto(x + 1, y + 1, true, false));
                        ris.Add(new CPunto(x - 1, y + 1, true, false));
                    }
                    if (Colore == inColore.Nero)
                    {
                        ris.Add(new CPunto(x, y - 1, false, true));
                        if (y == 7)
                        { ris.Add(new CPunto(x, y - 1, false, true)); }
                        ris.Add(new CPunto(x + 1, y - 1, true, false));
                        ris.Add(new CPunto(x - 1, y - 1, true, false));
                    }
                    break;
                case InizialePezzo.Cavallo:
                    //punti cavallo
                    ris.Add(new CPunto(x + 2, y + 1, true, true));
                    ris.Add(new CPunto(x + 2, y - 1, true, true));
                    ris.Add(new CPunto(x - 2, y + 1, true, true));
                    ris.Add(new CPunto(x - 2, y - 1, true, true));
                    ris.Add(new CPunto(x + 1, y + 2, true, true));
                    ris.Add(new CPunto(x - 1, y + 2, true, true));
                    ris.Add(new CPunto(x + 1, y - 2, true, true));
                    ris.Add(new CPunto(x - 1, y - 2, true, true));
                    break;
                case InizialePezzo.Alfiere:
                    //punti alfiere
                    ix = x;
                    iy = y;
                    while (ix < 8 && iy < 8)
                    {

                        ix++;
                        iy++;
                        ris.Add(new CPunto(ix, iy, true, true));
                    }
                    ix = x;
                    iy = y;
                    while (ix > -1 && iy > -1)
                    {

                        ix--;
                        iy--;
                        ris.Add(new CPunto(ix, iy, true, true));
                    }
                    ix = x;
                    iy = y;
                    while (ix > -1 && iy < 8)
                    {

                        ix--;
                        iy++;
                        ris.Add(new CPunto(ix, iy, true, true));
                    }
                    ix = x;
                    iy = y;
                    while (ix < 8 && iy > -1)
                    {

                        ix++;
                        iy--;
                        ris.Add(new CPunto(ix, iy, true, true));
                    }
                    break;
                case InizialePezzo.Torre:
                    //Punti torre
                    for (int i = 0; i < 8; i++)
                    {
                        if (i != x)
                            ris.Add(new CPunto(i, y, true, true));
                        if (i != y)
                            ris.Add(new CPunto(x, i, true, true));
                    }
                    break;
                case InizialePezzo.Regina:
                    //Punti Torre
                    for (int i = 0; i < 8; i++)
                    {
                        if (i != x)
                            ris.Add(new CPunto(i, y, true, true));
                        if (i != y)
                            ris.Add(new CPunto(x, i, true, true));
                    }
                    ix = x;
                    iy = y;
                    //punti Alfiere
                    while (ix < 8 && iy < 8)
                    {

                        ix++;
                        iy++;
                        ris.Add(new CPunto(ix, iy, true, true));
                    }
                    ix = x;
                    iy = y;
                    while (ix > -1 && iy > -1)
                    {

                        ix--;
                        iy--;
                        ris.Add(new CPunto(ix, iy, true, true));
                    }
                    ix = x;
                    iy = y;
                    while (ix > -1 && iy < 8)
                    {

                        ix--;
                        iy++;
                        ris.Add(new CPunto(ix, iy, true, true));
                    }
                    ix = x;
                    iy = y;
                    while (ix < 8 && iy > -1)
                    {

                        ix++;
                        iy--;
                        ris.Add(new CPunto(ix, iy, true, true));
                    }
                    break;
                case InizialePezzo.Re:
                    //Punti re
                    ris.Add(new CPunto(x, y + 1, true, true));
                    ris.Add(new CPunto(x + 1, y + 1, true, true));
                    ris.Add(new CPunto(x + 1, y, true, true));
                    ris.Add(new CPunto(x + 1, y - 1, true, true));
                    ris.Add(new CPunto(x, y - 1, true, true));
                    ris.Add(new CPunto(x - 1, y - 1, true, true));
                    ris.Add(new CPunto(x - 1, y, true, true));
                    ris.Add(new CPunto(x - 1, y + 1, true, true));
                    break;
            }

            return ris;
        }*/

    }
}
