using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScacchiP2P
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DatiCondivisi Dati;
        Scacchiera sc;
        private static MainWindow w;
        public MainWindow()
        {
            InitializeComponent();
            w = this;
            Dati = DatiCondivisi.Istanza;
            sc = Scacchiera.Istanza;
        }

        private void Click(object sender, MouseButtonEventArgs e)
        {
            //e.GetPosition((IInputElement)sender)
            char[] a = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
            int x = (int)e.GetPosition((IInputElement)sender).X / (int)(ScacchieraRet.Width / 8);
            int y = (int)e.GetPosition((IInputElement)sender).Y / (int)(ScacchieraRet.Height / 8);

            if (sc.Colore == "bianco")
            {
                y -= 7;
                if (y < 0)
                {
                    y *= -1;

                }
            }
            else
            {
                x -= 7;
                if (x < 0)
                {
                    x *= -1;

                }
            }
            y += 1;
            MessageBox.Show(a[x] + "" + y);
        }

        public static MainWindow GetMainWindow()
        {
            return w;
        }

        private void ControlliPartita(object sender, RoutedEventArgs e)
        {
            if (RD_Amichevole.IsChecked == true || RD_Competitiva.IsChecked == true)
            {
                TXT_Tempo.IsEnabled = false;
                RD_helpn.IsEnabled = false;
                RD_helps.IsEnabled = false;
                RD_standard.IsEnabled = false;
                RD_Scacchi960.IsEnabled = false;
            }
            else if (RD_Personalizzata.IsChecked == true)
            {
                TXT_Tempo.IsEnabled = true;
                RD_helpn.IsEnabled = true;
                RD_helps.IsEnabled = true;
                RD_standard.IsEnabled = true;
                RD_Scacchi960.IsEnabled = true;
            }
        }

        private void BTTN_inviaR_Click(object sender, RoutedEventArgs e)
        {
            if (RD_Amichevole.IsChecked == true)
            {
                Dati.AddStringDI("r;" + Dati.Count + ";0");
            }
            else if (RD_Competitiva.IsChecked == true)
            {
                Dati.AddStringDI("r;" + Dati.Count + ";1");
            }
            else
            {
                int tempo = -1;
                if (TXT_Tempo.Text != "")
                    int.TryParse(TXT_Tempo.Text, out tempo);
                string s = "r;" + Dati.Count + ";2;";
                if (tempo != -1)
                    s += tempo + ";";
                else
                {
                    MessageBox.Show("il tempo può essere espresso solo in numeri", "errore", MessageBoxButton.OK, MessageBoxImage.Error);
                    TXT_Tempo.Text = "";
                }
                if (RD_helps.IsChecked == true)
                    s += "true;";
                else
                    s += "false;";
                if (RD_standard.IsChecked == true)
                    s += "standard;";
                else
                    s += "scacchi960;";
            }
        }

        private void BTTN_inviaSC_Click(object sender, RoutedEventArgs e)
        {
            if (RD_bianco.IsChecked == true)
            {
                Dati.AddStringDI("sc;bianco");
            }
            else
            {
                Dati.AddStringDI("sc;nero");
            }
            RD_Amichevole.IsEnabled = true;
            RD_Competitiva.IsEnabled = true;
            RD_Personalizzata.IsEnabled = true;
            BTTN_inviaSC.IsEnabled = false;
            BTTN_inviaR.IsEnabled = true;
        }

        private void BBTN_Connessione_Click(object sender, RoutedEventArgs e)
        {
            int ip1 = -1, ip2 = -1, ip3 = -1, ip4 = -1;
            int.TryParse(TXT_IP_1.Text, out ip1);
            int.TryParse(TXT_IP_2.Text, out ip2);
            int.TryParse(TXT_IP_3.Text, out ip3);
            int.TryParse(TXT_IP_4.Text, out ip4);
            if (ip1 != -1 && ip2 != -1 && ip3 != -1 && ip4 != -1)
            {
                Dati.IP = ip1 + "." + ip2 + "." + ip3 + "." + ip4;
                Dati.AddStringDI("c;" + Dati.Nome);
                RD_bianco.IsEnabled = true;
                RD_nero.IsEnabled = true;
                BTTN_inviaSC.IsEnabled = true;
                BBTN_Connessione.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("L'indirizzo ip è composto da soli numeri, ritenti per favore", "Errore inserimento", MessageBoxButton.OK, MessageBoxImage.Error);
                TXT_IP_1.Text = "";
                TXT_IP_2.Text = "";
                TXT_IP_3.Text = "";
                TXT_IP_4.Text = "";
            }
        }

    }
}
