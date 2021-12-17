using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScacchiP2P
{
    public class DatiCondivisi
    {
        private string PezzoGiocante_;//bianco o nero
        private Pezzo [,]Scacchieram_;
        public Pezzo[,] Scacchieram { get { return Scacchieram_; } set { Scacchieram_ = value; } }
        public string PezzoGiocante { get { return PezzoGiocante_; } set { PezzoGiocante_ = value; } }

        public DatiCondivisi()
        {
            Scacchieram = new Pezzo[9, 9];
            Scacchieram_ = new Pezzo[9, 9];
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    Scacchieram[x, y] = new Pezzo(x,y);
                }
            }
            

        }
    }
}
