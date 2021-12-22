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
        public string PezzoGiocante { get { return PezzoGiocante_; } set { PezzoGiocante_ = value; } }

        public DatiCondivisi()
        {
            PezzoGiocante = "bianco";
        }
    }
}
