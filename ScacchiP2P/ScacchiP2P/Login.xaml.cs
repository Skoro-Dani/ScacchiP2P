using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace ScacchiP2P
{
    /// <summary>
    /// Logica di interazione per Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        DatiGiocatore dg;
        MD5 md5;
        public Login()
        {
            InitializeComponent();
            dg = DatiGiocatore.Istanza;
            dg.deserialize();
            md5 = System.Security.Cryptography.MD5.Create();
        }

        private void bttn_login_Click(object sender, RoutedEventArgs e)
        {
            int pos = -1;
            bool ris = false;
            for (int i = 0; i < dg.lista.Count; i++)
            {
                if (dg.lista[i].Nome == TXT_username.Text)
                    if (dg.lista[i].Password == CreateMD5Hash(TXT_password.Password))
                    {
                        pos = i;
                        ris = true;
                    }
            }
            if (ris)
            {
                dg.Nome = TXT_username.Text;
                if (pos != -1)
                    dg.Punti = dg.lista[pos].Punti;
                dg.serialize();
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
                for (int i = 0; i < dg.lista.Count; i++)
                {
                    if (dg.lista[i].Nome == TXT_username.Text) { ris = true; }
                }
                if (ris)
                {
                    MessageBox.Show("Utente gia esistente", "errore");
                }
                else
                {
                    dg.lista.Add(new dgProv(TXT_username.Text, CreateMD5Hash(TXT_password.Password)));
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
