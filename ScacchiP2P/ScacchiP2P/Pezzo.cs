using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScacchiP2P
{
    public class Pezzo
    {
        string []alfabeto;
        int x;
        int y;
        public Pezzo(int x,int y)
        {
            this.x = x;
            this.y = y;
            alfabeto = new string[9];
            char v = 'a';
            for(int i = 0; i < 9; i++)
            {
                alfabeto[i] = v.ToString();
                v++;
            }
        }

        public override string ToString()
        {
            string s = alfabeto[x];
            s += (y + 1);
            return s;
        }
    }
}
