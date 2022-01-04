using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScacchiP2P
{
    [Serializable()]
    public class dgProv
    {
        //Dati Giocatore
        public string Nome { get; set; }
        public string Password { get; set; } 
        public int Punti { get; set; } 

        public dgProv(string nome, string pass)
        {
            Nome = nome;
            Password = pass;
            Punti = 0;
        }
        public dgProv()
        {
            AzzeraDati();
        }

        public void AzzeraDati()
        {
            Nome = "";
            Password = "";
            Punti = 0;
        }
    }
}
