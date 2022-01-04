using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace ScacchiP2P
{
    /// <summary>
    /// Logica di interazione per Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        DatiGiocatore dg;
        List<dgProv> lista = null;
        MD5 md5;
        public Login()
        {
            InitializeComponent();
            dg = DatiGiocatore.Istanza;
            deserialize();
            md5 = System.Security.Cryptography.MD5.Create();
        }

        private void deserialize()
        {
            XmlSerializer myXML = new XmlSerializer(typeof(List<dgProv>));
            StreamReader fIN = new StreamReader("utente.xml");
            lista = (List<dgProv>)myXML.Deserialize(fIN);
            fIN.Close();
        }
        private void serialize()
        {
            XmlSerializer myXML = new XmlSerializer(typeof(List<dgProv>));
            StreamWriter fOUT = new StreamWriter("utente.xml");
            myXML.Serialize(fOUT, lista);
            fOUT.Close();
        }

        private void bttn_login_Click(object sender, RoutedEventArgs e)
        {
            int pos=-1;
            bool ris = false;
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].Nome == TXT_username.Text)
                    if (lista[i].Password == CreateMD5Hash(TXT_password.Password))
                    {
                        pos = i;
                        ris = true;
                    }
            }
            if (ris)
            {
                dg.Nome = TXT_username.Text;
                if(pos!=-1)
                dg.Punti = lista[pos].Punti;
                serialize();
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Username o Password sbagliati", "errore");
            }
            azzera();
        }

        private void bttn_Registrati_Click(object sender, RoutedEventArgs e)
        {
            if (TXT_username.Text.Length > 0 && TXT_password.Password.Length > 0)
            {
                bool ris = false;
                for (int i = 0; i < lista.Count; i++)
                {
                    if (lista[i].Nome == TXT_username.Text) { ris = true; }
                }
                if (ris)
                {
                    MessageBox.Show("Utente gia esistente", "errore");
                }
                else
                {
                    lista.Add(new dgProv(TXT_username.Text, CreateMD5Hash(TXT_password.Password)));
                    MessageBox.Show("ti sei registrato, prova il login", "affermativo");
                }
            }
            else MessageBox.Show("Username e password non possono lasciati vuoti", "errore");
            azzera();
        }


        public string CreateMD5Hash(string input)
        {
            // Step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private void azzera()
        {
            TXT_password.Password = "";
            TXT_username.Text = "";
        }
    }
}
