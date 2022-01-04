using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ScacchiP2P
{
    class DatiGiocatore
    {
        //istanza singleton
        private static DatiGiocatore istanza = null;
        private static object LockIstanza = new object();
        //Dati Giocatore
        private static object LockNome = new object();
        private string Nome_;
        public string Nome { get { lock (LockNome) { return Nome_; } } set { lock (LockNome) { Nome_ = value; } } }
        private static object LockPunti = new object();
        private int Punti_;
        public int Punti { get { lock (LockPunti) { return Punti_; } } set { lock (LockPunti) { Punti_ = value; } } }
        //seralizzazione
        public List<dgProv> lista = null;

        private DatiGiocatore()
        {
            AzzeraDati();
        }

        public static DatiGiocatore Istanza
        {
            get
            {
                lock (LockIstanza)
                {
                    if (istanza == null) istanza = new DatiGiocatore();
                    return istanza;
                }
            }
        }

        public void AzzeraDati()
        {
            Nome = "";
            Punti = 0;
        }

        public void deserialize()
        {
            XmlSerializer myXML = new XmlSerializer(typeof(List<dgProv>));
            StreamReader fIN = new StreamReader("utente.xml");
            lista = (List<dgProv>)myXML.Deserialize(fIN);
            fIN.Close();
        }
        public void serialize()
        {
            XmlSerializer myXML = new XmlSerializer(typeof(List<dgProv>));
            StreamWriter fOUT = new StreamWriter("utente.xml");
            myXML.Serialize(fOUT, lista);
            fOUT.Close();
        }
    }
}
