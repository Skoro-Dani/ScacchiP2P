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
        DatiCondivisi Dati = DatiCondivisi.Istanza;
        public MainWindow()
        {
            InitializeComponent();
            Dati.ColoreGiocante = "bianco";
        }

        private void Click(object sender, MouseButtonEventArgs e)
        {
            //e.GetPosition((IInputElement)sender)
            char[] a = { 'A', 'B', 'C', 'D', 'E', 'F', 'G','H' };
            int x = (int)e.GetPosition((IInputElement)sender).X / (int)(Scacchiera.Width/8);
            int y = (int)e.GetPosition((IInputElement)sender).Y/ (int)(Scacchiera.Height/8);

            if (Dati.ColoreGiocante.Equals("bianco"))
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
            MessageBox.Show(a[x]+""+ y);
        }
    }
}
