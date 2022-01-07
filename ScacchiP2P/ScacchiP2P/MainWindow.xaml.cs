using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ScacchiP2P
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DatiCondivisi Dati = DatiCondivisi.Istanza;
        private Scacchiera sc = Scacchiera.Istanza;
        private static MainWindow w;
        private DatiGiocatore dg = DatiGiocatore.Istanza;
        private Thread LT;
        private Thread WLT;
        private Thread WT;
        private Listener L;
        private WorkListener WL;
        private Writer W;
        private Login login;

        private List<Image> PosImg;
        private List<Image> PosDot;
        private List<CPunto> posdotP;
        private bool Selezionato=false;
        //costruttore
        public MainWindow()
        {
            InitializeComponent();
            w = this;
           // Dati = DatiCondivisi.Istanza;
            //dg = DatiGiocatore.Istanza;
            //sc = Scacchiera.Istanza;
            PosImg = new List<Image>();
            PosDot = new List<Image>();
            posdotP = new List<CPunto>();
            login = new Login();
            bool ris = (bool)login.ShowDialog();
            if (ris == false)
                this.Close();
            LBL_Nome.Content = dg.Nome + "->" + dg.Punti;

            L = new Listener();
            WL = new WorkListener();
            W = new Writer();

            LT = new Thread(new ThreadStart(L.ProcThread));
            WLT = new Thread(new ThreadStart(WL.ProcThread));
            WT = new Thread(new ThreadStart(W.ProcThread));

            LT.Start();
            WLT.Start();
            WT.Start();
            RefreshScacchiera();
        }

        //metodo che serve a riconoscere dove clicca l'utente
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
            if(sc.getPezzo(x,y-1)!=null)
            {
                posdotP=sc.GetPosizioni(new CPunto(x, y - 1,true,true), sc.getPezzo(x, y - 1));
                Selezionato = true;
            }
            RefreshScacchiera();
        }

        //aggiorna la grafica della scacchiera
        public void RefreshScacchiera()
        {
            if(PosImg!=null) PosImg.Clear();
            
            ScacchieraRet.Children.Clear();
            string image = "";
            int count = 0;
            for(int x=0;x<8;x++)
            {
                for(int y=0;y<8;y++)
                {
                    if(sc.getPezzo(x,y)!=null)
                    {
                        PosImg.Add(new Image());
                        count = PosImg.Count - 1;
                        image = "/ScacchiP2P;" + sc.getPezzo(x, y).img;
                        PosImg[count].Source = new BitmapImage(new Uri(image, UriKind.Relative));
                        PosImg[count].Width = ScacchieraRet.Width / 8;
                        PosImg[count].Height = ScacchieraRet.Height / 8;
                        Canvas.SetLeft(PosImg[count], x * ScacchieraRet.Width / 8);
                        Canvas.SetTop(PosImg[count], y * ScacchieraRet.Height / 8);
                        ScacchieraRet.Children.Add(PosImg[count]);
                    }
                }
            }
            if(Selezionato==true)
            {
                for(int i=0;i<posdotP.Count;i++)
                {
                    PosDot.Add(new Image());
                    count=PosDot.Count;
                    image = "/ScacchiP2P;" + "component/PNGScacchiera/Dot.png";
                    PosImg[count].Source = new BitmapImage(new Uri(image, UriKind.Relative));
                    PosImg[count].Width = ScacchieraRet.Width / 8;
                    PosImg[count].Height = ScacchieraRet.Height / 8;
                    Canvas.SetLeft(PosImg[count], posdotP[i].x * ScacchieraRet.Width / 8);
                    Canvas.SetTop(PosImg[count], posdotP[i].y * ScacchieraRet.Height / 8);
                    ScacchieraRet.Children.Add(PosImg[count]);
                }
            }
        }

        //get this
        public static MainWindow GetMainWindow()
        {
            return w;
        }

        //Metodo controllo che l'utente non faccia cose strane con le regole
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
        //invio Regole
        private void BTTN_inviaR_Click(object sender, RoutedEventArgs e)
        {
            if (RD_Amichevole.IsChecked == true)
            {
                Dati.AddStringDI("r;0");
            }
            else if (RD_Competitiva.IsChecked == true)
            {
                Dati.AddStringDI("r;1");
            }
            else
            {
                int tempo = -1;
                if (TXT_Tempo.Text != "")
                    int.TryParse(TXT_Tempo.Text, out tempo);
                string s = "r;2;";
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
        //invio Colore
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
        //richiesta Connesione
        private void BBTN_Connessione_Click(object sender, RoutedEventArgs e)
        {
            RefreshScacchiera();
            int ip1 = -1, ip2 = -1, ip3 = -1, ip4 = -1;
            bool i1=int.TryParse(TXT_IP_1.Text, out ip1);
            bool i2 = int.TryParse(TXT_IP_2.Text, out ip2);
            bool i3 = int.TryParse(TXT_IP_3.Text, out ip3);
            bool i4 = int.TryParse(TXT_IP_4.Text, out ip4);
            if (i1==true&&i2==true&i3==true&&i4==true)
            {
                Dati.IP = ip1 + "." + ip2 + "." + ip3 + "." + ip4;
                Dati.AddStringDI("c;" + dg.Nome);
                BBTN_Connessione.IsEnabled = false;
                Dati.ARConnessione = true;
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

        public void PattaRifutata()
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show("L'avversario ha rifiutato la Patta", "Patta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            });
        }

        //Metodo si e no per accettare una connesione
        public void RichiediConnessione(string s)
        {
            Dispatcher.Invoke(() =>
            {
                MessageBoxResult result = MessageBox.Show(s + " vuole connettersi.", "Richiesta Connessione", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Dati.Connesso = true;
                    Dati.IP = Dati.IPVC;
                    Dati.IPVC = "";
                    Dati.VConnesione = false;
                    Dati.ARConnessione = false;
                    Dati.AddStringDI("y;" + dg.Nome);
                }
                else
                {
                    Dati.AddStringDI("n;");
                }
            });
        }
        //avviso inizio Partita
        public void PartitaStart()
        {
            Dispatcher.Invoke(() =>
            {

                MessageBox.Show("La partita è iniziata", "Start", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            });
        }
        //arresa dell'avversario
        public void SurrenderDellavversario()
        {
            Dispatcher.Invoke(() =>
            {
                sc.ArresaMet(true);
                MessageBox.Show("L'avversario si è arreso", "Vittoria", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            });
        }
        //patta richiesta patta da parte dell'avversario
        public void Patta()
        {
            Dispatcher.Invoke(() =>
            {
                MessageBoxResult result = MessageBox.Show("L'avversario vuole patteggiare", "Patta", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    sc.Patta();
                    Dati.AddStringDI("y;m;");
                }
                else
                {
                    Dati.AddStringDI("n;m;");
                }
            });
        }
        //disconnesione dell'avversario
        public void Disconnessione()
        {
            Dispatcher.Invoke(() =>
            {
                Dati.AzzeraDati();
                MessageBox.Show("L'avversario si è disconnesso", "Disconnesione", MessageBoxButton.OK);

            });
        }

        //controllo della conessione da parte dell'utente
        public void ConnesioneA(bool a)
        {
            Dispatcher.Invoke(() =>
            {
                if (a)
                {
                    Dati.Connesso = true;
                    Dati.ARConnessione = false;
                    RD_bianco.IsEnabled = true;
                    RD_nero.IsEnabled = true;
                    BTTN_inviaSC.IsEnabled = true;
                    MessageBox.Show("L'avversario si è Connesso", "Connesione", MessageBoxButton.OK);
                }
                else
                {
                    Dati.AzzeraDati();
                    MessageBox.Show("L'avversario ha rifiutato la connesione", "Connesione", MessageBoxButton.OK);
                    TXT_IP_1.Text = "";
                    TXT_IP_2.Text = "";
                    TXT_IP_3.Text = "";
                    TXT_IP_4.Text = "";
                    RD_bianco.IsEnabled = false;
                    RD_nero.IsEnabled = false;
                    BTTN_inviaSC.IsEnabled = false;
                }

            });
        }
        //controllo delle regole da parte dell'utente
        public void RegoleA(bool a)
        {
            Dispatcher.Invoke(() =>
            {
                if (a)
                {
                    MessageBox.Show("L'avversario Ha Acccettato le regole", "Regole", MessageBoxButton.OK);
                    TAB_partita.IsEnabled = true;
                    TAB_partita.IsSelected = true;

                }
                else
                {
                    Dati.AzzeraDati();
                    MessageBox.Show("L'avversario ha rifiutato la connesione e di coneseguenza si sta disconnentendo", "Regole", MessageBoxButton.OK);
                }
            });
        }
        //metodo chiusura della finestra
        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            Dati.Flag = true;
            dg.deserialize();
            for (int i = 0; i < dg.lista.Count; i++)
                if (dg.lista[i].Nome == dg.Nome)
                {
                    dg.lista[i].Punti = dg.Punti;
                }
            dg.serialize();
        }
        //disconnesione da parte dell'utente
        private void BTTN_Disconnetiti_Click(object sender, RoutedEventArgs e)
        {
            Dati.AddStringDI("d;");
            Dati.AzzeraDati();
            disableAll();
            MessageBox.Show("Ti sei Disconessio", "Disconnesione", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        //disabilita tutti i controlli
        private void disableAll()
        {
            RD_Amichevole.IsEnabled = false;
            TAB_partita.IsEnabled = false;
            TAB_partita.IsSelected = false;
            TXT_IP_1.Text = "";
            TXT_IP_2.Text = "";
            TXT_IP_3.Text = "";
            TXT_IP_4.Text = "";
            RD_bianco.IsEnabled = false;
            RD_nero.IsEnabled = false;
            BTTN_inviaSC.IsEnabled = false;
            RD_bianco.IsEnabled = false;
            RD_nero.IsEnabled = false;
            BTTN_inviaSC.IsEnabled = false;
            BTTN_inviaR.IsEnabled = false;
            BBTN_Connessione.IsEnabled = true;
        }

        
        

        //aggiorna il timer
        public void RefreshTimer(string ValoreA, string ValoreU)
        {
            Dispatcher.Invoke(() =>
            {
                LBL_TimerA.Content = "Timer-> " + ValoreA;
                LBL_TimerU.Content = "Timer-> " + ValoreU;
            });
        }

        //metodo per evitare che l'utente faccia più mosse
        public void DisOAblSC(bool DisOAbl)
        {
            if (DisOAbl) ScacchieraRet.IsEnabled = true;
            else ScacchieraRet.IsEnabled = false;
        }

        public void Rivincita()
        {
            /* IN CORSO*/
        }

        private void Arresa(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Sicuro di voler Arrenderti", "Resa", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                sc.ArresaMet(false);
                Dati.AddStringDI("s;");
            }

        }

        private void Patta(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Sicuro di voler Patteggiare", "Patta", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Dati.AddStringDI("m;A0;A0;true;");
            }
        }

    }
}
