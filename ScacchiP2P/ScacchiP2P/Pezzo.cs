using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
            //img = Image.FromFile("D:\\GitHub\\ScacchiP2P\\ScacchiP2P\\ScacchiP2P\\PNGPezzi\\ReBiancoS.jpg");
            /*switch(Nome)
            {
                case InizialePezzo.Pedone:
                    if(Colore==inColore.Bianco)
                    {

                    }
                    else
                    {

                    }
                    break;
            }*/
        }
        public enum InizialePezzo
        {
            Pedone = 'P',
            Cavallo = 'C',
            Alfiere = 'B',
            Torre = 'R',
            Regina = 'Q',
            Re = 'K',
            Vuoto= ' ',
        }
        public enum inColore
        {
            Bianco = 'b',
            Nero = 'n',
            nulla = ' ',
        }


        public List<CPunto> DovePuoAndare(CPunto P)
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
                        ris.Add(new CPunto(x, y + 1));
                        if (y == 2)
                        { ris.Add(new CPunto(x, y + 2)); }
                        ris.Add(new CPunto(x + 1, y + 1));
                        ris.Add(new CPunto(x - 1, y + 1));
                    }
                    if (Colore == inColore.Nero)
                    {
                        ris.Add(new CPunto(x, y - 1));
                        if (y == 7)
                        { ris.Add(new CPunto(x, y - 1)); }
                        ris.Add(new CPunto(x + 1, y - 1));
                        ris.Add(new CPunto(x - 1, y - 1));
                    }
                    break;
                case InizialePezzo.Cavallo:
                    //punti cavallo
                    ris.Add(new CPunto(x + 2, y + 1));
                    ris.Add(new CPunto(x + 2, y - 1));
                    ris.Add(new CPunto(x - 2, y + 1));
                    ris.Add(new CPunto(x - 2, y - 1));
                    ris.Add(new CPunto(x + 1, y + 2));
                    ris.Add(new CPunto(x - 1, y + 2));
                    ris.Add(new CPunto(x + 1, y - 2));
                    ris.Add(new CPunto(x - 1, y - 2));
                    break;
                case InizialePezzo.Alfiere:
                    //punti alfiere
                    ix = x;
                    iy = y;
                    while (ix < 8 && iy<8)
                    {

                        ix++;
                        iy++;
                        ris.Add(new CPunto(ix, iy));
                    }
                    ix = x;
                    iy = y;
                    while (ix >-1 && iy >-1)
                    {

                        ix--;
                        iy--;
                        ris.Add(new CPunto(ix, iy));
                    }
                    ix = x;
                    iy = y;
                    while (ix > -1 && iy <8)
                    {

                        ix--;
                        iy++;
                        ris.Add(new CPunto(ix, iy));
                    }
                    ix = x;
                    iy = y;
                    while (ix <8 && iy >-1)
                    {

                        ix++;
                        iy--;
                        ris.Add(new CPunto(ix, iy));
                    }
                    break;
                case InizialePezzo.Torre:
                    //Punti torre
                    for (int i = 0; i < 8; i++)
                    {
                        if (i != x)
                            ris.Add(new CPunto(i, y));
                        if (i != y)
                            ris.Add(new CPunto(x, i));
                    }
                    break;
                case InizialePezzo.Regina:
                    //Punti Torre
                    for (int i = 0; i < 8; i++)
                    {
                        if (i != x)
                            ris.Add(new CPunto(i, y));
                        if (i != y)
                            ris.Add(new CPunto(x, i));
                    }
                    ix = x;
                    iy = y;
                    //punti Alfiere
                    while (ix < 8 && iy < 8)
                    {

                        ix++;
                        iy++;
                        ris.Add(new CPunto(ix, iy));
                    }
                    ix = x;
                    iy = y;
                    while (ix > -1 && iy > -1)
                    {

                        ix--;
                        iy--;
                        ris.Add(new CPunto(ix, iy));
                    }
                    ix = x;
                    iy = y;
                    while (ix > -1 && iy < 8)
                    {

                        ix--;
                        iy++;
                        ris.Add(new CPunto(ix, iy));
                    }
                    ix = x;
                    iy = y;
                    while (ix < 8 && iy > -1)
                    {

                        ix++;
                        iy--;
                        ris.Add(new CPunto(ix, iy));
                    }
                    break;
                case InizialePezzo.Re:
                    //Punti re
                    ris.Add(new CPunto(x, y + 1));
                    ris.Add(new CPunto(x + 1, y + 1));
                    ris.Add(new CPunto(x + 1, y));
                    ris.Add(new CPunto(x + 1, y - 1));
                    ris.Add(new CPunto(x, y - 1));
                    ris.Add(new CPunto(x - 1, y - 1));
                    ris.Add(new CPunto(x - 1, y));
                    ris.Add(new CPunto(x - 1, y + 1));
                    break;
            }

            return ris;
        }
    }
}
