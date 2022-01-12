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
        private Thread CVT;
        private Listener L;
        private WorkListener WL;
        private Writer W;
        private ControllaVittoria CV;
        private Login login;

        private List<Image> PosImg;
        private List<Image> PosDot;
        private List<CPunto> posdotP;

        private CPunto punto1 = new CPunto(0, 0, true, true);
        private Pezzo p;
        private bool Selezionato = false;
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
            Dati.w = w;

            RefreshScacchiera();
            disableAll();
            start();
        }

        //metodo che serve a riconoscere dove clicca l'utente
        private void Click(object sender, MouseButtonEventArgs e)
        {
            //e.GetPosition((IInputElement)sender)
            char[] a = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
            int x = (int)e.GetPosition((IInputElement)sender).X / (int)(ScacchieraRet.Width / 8);
            int y = (int)e.GetPosition((IInputElement)sender).Y / (int)(ScacchieraRet.Height / 8);
            string pos1, pos2;
            bool trovato = false;
            CPunto punto2 = new CPunto(-1, -1, true, true);
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
            if (Selezionato == false)
            {
                if (sc.getPezzo(x, y) != null)
                {
                    if (sc.getPezzo(x, y).Colore.ToString().ToLower() == sc.Colore.ToLower())
                    {
                        punto1 = new CPunto(x, y, true, true);
                        p = sc.getPezzo(x, y);
                        posdotP = sc.GetPosizioni(new CPunto(x, y, true, true), sc.getPezzo(x, y));
                        Selezionato = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < posdotP.Count; i++)
                {
                    if (x == posdotP[i].x && y == posdotP[i].y)
                    {
                        trovato = true;
                        punto2 = posdotP[i];
                    }
                }
                if (trovato == true)
                {
                    pos1 = a[punto1.x] + (punto1.y + 1).ToString();
                    pos2 = a[punto2.x] + (punto2.y + 1).ToString();
                    sc.Mossa(pos1, pos2);
                    Selezionato = false;
                    Dati.AddStringDI("m;" + a[punto1.x] + (punto1.y + 1) + ";" + a[punto2.x] + (punto2.y + 1) + ";" + sc.getPezzo(punto2.x, punto2.y).Nome.ToString().Substring(0, 1) + ";" + "false");
                }
                else
                {
                    Selezionato = false;
                    p = null;
                    punto1 = null;
                }
            }
            RefreshScacchiera();
        }
        public void DisOrAnb(bool DisOrAbl)
        {
            Dispatcher.Invoke(() =>
            {
                if (DisOrAbl == true) ScacchieraRet.IsEnabled = true;
                else ScacchieraRet.IsEnabled = false;
            });
        }

        //aggiorna la grafica della scacchiera
        public void RefreshScacchiera()
        {
            Dispatcher.Invoke(() =>
            {
                if (PosImg != null) PosImg.Clear();

                ScacchieraRet.Children.Clear();
                string image = "";
                int count = 0;
                int ix = 0, iy = 0;
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        if (sc.getPezzo(x, y) != null)
                        {

                            PosImg.Add(new Image());
                            count = PosImg.Count - 1;
                            image = "/ScacchiP2P;" + sc.getPezzo(x, y).img;
                            PosImg[count].Source = new BitmapImage(new Uri(image, UriKind.Relative));

                            PosImg[count].Width = ScacchieraRet.Width / 8;
                            PosImg[count].Height = ScacchieraRet.Height / 8;
                            if (sc.Colore == "bianco")
                            {
                                ix = x;
                                iy = y - 7;
                                if (iy < 0)
                                {
                                    iy *= -1;
                                }
                            }
                            else
                            {
                                iy = y;
                                ix = x - 7;
                                if (ix < 0)
                                {
                                    ix *= -1;
                                }
                            }

                            Canvas.SetLeft(PosImg[count], ix * ScacchieraRet.Width / 8);
                            Canvas.SetTop(PosImg[count], iy * ScacchieraRet.Height / 8);
                            ScacchieraRet.Children.Add(PosImg[count]);
                        }
                    }
                }
                if (Selezionato == true)
                {
                    for (int i = 0; i < posdotP.Count; i++)
                    {
                        PosDot.Add(new Image());
                        count = PosDot.Count - 1;
                        image = "/ScacchiP2P;" + "component/PNGScacchiera/Dot.png";
                        PosDot[count].Source = new BitmapImage(new Uri(image, UriKind.Relative));
                        PosDot[count].Width = ScacchieraRet.Width / 8;
                        PosDot[count].Height = ScacchieraRet.Height / 8;
                        if (sc.Colore == "bianco")
                        {
                            ix = posdotP[i].x;
                            iy = posdotP[i].y - 7;
                            if (iy < 0)
                            {
                                iy *= -1;


                            }
                        }
                        else
                        {
                            iy = posdotP[i].y;
                            ix = posdotP[i].x - 7;
                            if (ix < 0)
                            {
                                ix *= -1;

                            }

                        }
                        Canvas.SetLeft(PosDot[count], ix * ScacchieraRet.Width / 8);
                        Canvas.SetTop(PosDot[count], iy * ScacchieraRet.Height / 8);
                        ScacchieraRet.Children.Add(PosDot[count]);
                    }
                }
                if (sc.TurnoAvv == false) ScacchieraRet.IsEnabled = true;
                else ScacchieraRet.IsEnabled = false;
                List<string> ut = sc.GetListMosseU();
                List<string> av = sc.GetListMosseA();
                string sb = "";
                string sn = "";
                if (sc.Colore.ToLower() == "bianco")
                {
                    for (int i = 0; i < ut.Count; i++)
                        sb += ut[i] + "\r\n";
                    for (int i = 0; i < av.Count; i++)
                        sn += av[i] + "\r\n";
                }
                else
                {
                    for (int i = 0; i < ut.Count; i++)
                        sn += ut[i] + "\r\n";
                    for (int i = 0; i < av.Count; i++)
                        sb += av[i] + "\r\n";
                }
                ListBianco.Content = sb;
                ListNero.Content = sn;
            });
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
                Tempo_list.IsEnabled = false;
                RD_helpn.IsEnabled = false;
                RD_helps.IsEnabled = false;
                RD_standard.IsEnabled = false;
                RD_Scacchi960_.Visibility = Visibility.Hidden;
                RD_standard.Visibility = Visibility.Hidden;
                lblh.Visibility = Visibility.Hidden;
                lbls.Visibility = Visibility.Hidden;
                lblt.Visibility = Visibility.Hidden;
                Tempo_list.Visibility = Visibility.Hidden;
                RD_helpn.Visibility = Visibility.Hidden;
                RD_helps.Visibility = Visibility.Hidden;
                //RD_Scacchi960.IsEnabled = false;
            }
            else if (RD_Personalizzata.IsChecked == true)
            {
                Tempo_list.IsEnabled = true;
                RD_helpn.IsEnabled = true;
                RD_helps.IsEnabled = true;
                RD_standard.IsEnabled = true;
                RD_Scacchi960_.Visibility = Visibility.Visible;
                RD_standard.Visibility = Visibility.Visible;
                lblh.Visibility = Visibility.Visible;
                lbls.Visibility = Visibility.Visible;
                lblt.Visibility = Visibility.Visible;
                Tempo_list.Visibility = Visibility.Visible;
                RD_helpn.Visibility = Visibility.Visible;
                RD_helps.Visibility = Visibility.Visible;
                //RD_Scacchi960.IsEnabled = true;

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
                string tempos = (string)Tempo_list.SelectedItem;
                string s = "r;2;";
                s += tempos + ";";
                if (RD_helps.IsChecked == true)
                    s += "true;";
                else
                    s += "false;";
                if (RD_standard.IsChecked == true)
                    s += "standard;";
                else
                    s += "scacchi960;";
                Dati.AddStringDI(s);
            }
        }

        //richiesta Connesione
        private void BBTN_Connessione_Click(object sender, RoutedEventArgs e)
        {
            RefreshScacchiera();
            int ip = 0;
            bool ipb = true;
            string[] ip1 = TXT_IP.Text.Split('.');
            if (ip1.Length == 4)
            {
                for (int i = 0; i < 4; i++)
                    ipb = int.TryParse(ip1[i], out ip);
            }

            if (ipb == true || TXT_IP.Text == "localhost")
            {
                Dati.IP = TXT_IP.Text;
                Dati.AddStringDI("c;" + dg.Nome);
                BBTN_Connessione.IsEnabled = false;
                Dati.ARConnessione = true;
            }
            else
            {
                MessageBox.Show("L'indirizzo ip è composto da soli numeri, ritenti per favore", "Errore inserimento", MessageBoxButton.OK, MessageBoxImage.Error);
                TXT_IP.Text = "";
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
                    Dati.AddStringDI("y;c;" + dg.Nome);
                }
                else
                {
                    Dati.AddStringDI("n;c;");
                }
            });
        }
        //Medoto per accettare o rifiutare le regole
        public void RichiediRegole(string s)
        {
            Dispatcher.Invoke(() =>
            {
                string[] r = s.Split(';');
                string domanda = "";
                switch (r[1])
                {
                    case "0":
                        domanda = "L'avversario vuole fare l'amichevole";
                        break;
                    case "1":
                        domanda = "L'avversario vuole fare una competitiva";
                        break;
                    case "2":
                        domanda = "L'avversario vuole fare una partita con queste regole: \r\n" +
                            "tempo-> " + r[2] + "\r\n" +
                            "Help-> " + r[3] + "\r\n" +
                            "TipoScacchi->" + r[4];
                        break;
                }
                MessageBoxResult result = MessageBox.Show(domanda, "Regole", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Dati.AddStringDI("y;r;");
                    TAB_partita.Visibility = Visibility.Visible;
                    TAB_partita.IsEnabled = true;
                    TAB_partita.IsSelected = true;
                }
                else
                {
                    Dati.AddStringDI("n;r;d;");
                    TAB_NPartita.IsSelected = true;
                    disableAll();
                    Dati.AddStringRL("d;");
                }
            });
        }
        //avviso inizio Partita
        public void PartitaStart()
        {
            Dispatcher.Invoke(() =>
            {
                RefreshScacchiera();
                MessageBox.Show("La partita è iniziata", "Start", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                disableAll();
                TAB_partita.Visibility = Visibility.Visible;
                TAB_partita.IsEnabled = true;
                TAB_partita.IsSelected = true;
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
                disableAll();
                Dati.AzzeraDati();
                MessageBox.Show("L'avversario si è disconnesso", "Disconnesione", MessageBoxButton.OK);
                TAB_NPartita.IsSelected = true;
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
                    BTTN_ScNero.Visibility = Visibility.Visible;
                    Bttn_SCBianco.Visibility = Visibility.Visible;
                    lblsc.Visibility = Visibility.Visible;
                    MessageBox.Show("L'avversario si è Connesso", "Connesione", MessageBoxButton.OK);
                }
                else
                {
                    Dati.AzzeraDati();
                    MessageBox.Show("L'avversario ha rifiutato la connesione", "Connesione", MessageBoxButton.OK);
                    TXT_IP.Text = "";
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
                    TAB_partita.Visibility = Visibility.Visible;
                    TAB_partita.IsEnabled = true;
                    TAB_partita.IsSelected = true;
                    Dati.AddStringDI("ms;");
                    sc.PartitaStart();
                    PartitaStart();
                }
                else
                {
                    Dati.AzzeraDati();
                    Dati.AddStringRL("d;");
                    MessageBox.Show("L'avversario ha rifiutato la connesione e di coneseguenza si sta disconnentendo", "Regole", MessageBoxButton.OK);
                    disableAll();
                }
            });
        }
        //metodo chiusura della finestra
        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            Dati.AddStringDI("d;");
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
            sc.AzzeraDati();
            disableAll();
            MessageBox.Show("Ti sei Disconessio", "Disconnesione", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            Dati.AzzeraDati();
        }
        //disabilita tutti i controlli
        private void disableAll()
        {
            TAB_NPartita.IsSelected = true;
            BBTN_Connessione.IsEnabled = true;
            RD_Amichevole.IsEnabled = false;
            TAB_partita.IsEnabled = false;
            TAB_partita.IsSelected = false;
            TXT_IP.Text = "";
            BTTN_ScNero.Visibility = Visibility.Hidden;
            Bttn_SCBianco.Visibility = Visibility.Hidden;
            img_b.Visibility = Visibility.Hidden;
            img_n.Visibility = Visibility.Hidden;
            BTTN_inviaR.IsEnabled = false;
            BBTN_Connessione.IsEnabled = true;
            RD_Amichevole.Visibility = Visibility.Hidden; ;
            RD_Competitiva.Visibility = Visibility.Hidden;
            BTTN_inviaR.Visibility = Visibility.Hidden;
            Tempo_list.Visibility = Visibility.Hidden;
            RD_helpn.Visibility = Visibility.Hidden;
            RD_helps.Visibility = Visibility.Hidden;
            lblh.Visibility = Visibility.Hidden;
            lblR.Visibility = Visibility.Hidden;
            lbls.Visibility = Visibility.Hidden;
            lblt.Visibility = Visibility.Hidden;
            RD_Personalizzata.Visibility = Visibility.Hidden;
            RD_Scacchi960_.Visibility = Visibility.Hidden;
            RD_standard.Visibility = Visibility.Hidden;
            lblsc.Visibility = Visibility.Hidden;
            TAB_partita.Visibility = Visibility.Hidden;
            TAB_Rivincita.Visibility = Visibility.Hidden;
            LBL_Risultato.Content = "";
            BBTN_Rivincitas.IsEnabled = true;
            BBTN_Rivincitan.IsEnabled = true;
            LBL_avversarioRiv.Content = "Aspettando la risposta dell'avversario";
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
        public void Rivincita(bool vittoriaAvversario)
        {
            Dispatcher.Invoke(() =>
            {
                TAB_Rivincita.Visibility = Visibility.Visible;
                TAB_Rivincita.IsSelected = true;
                TAB_Rivincita.IsEnabled = true;
                TAB_partita.IsEnabled = false;
                BBTN_Rivincitas.IsEnabled = true;
                BBTN_Rivincitan.IsEnabled = true;
                string mess = "";
                if (sc.APatta == false)
                {
                    if (vittoriaAvversario == true)
                        mess = "Ritenta sarai più fortunato :( Hai perso!!!";
                    else if (vittoriaAvversario == false)
                        mess = "Congratulazioni!!!!! Hai vinto!!!";
                }
                else mess = "Sad story:( avete fatto patta";
                LBL_Risultato.Content = mess;
            });
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
                Dati.AddStringDI("m;A0;A0;R;true;");
            }
        }
        private void BTTN_collegati_Click(object sender, RoutedEventArgs e)
        {
            start();
        }
        private void Bttn_SCBianco_Click(object sender, RoutedEventArgs e)
        {
            Dati.AddStringDI("sc;bianco");
            Bttn_SCBianco.Visibility = Visibility.Hidden;
            BTTN_ScNero.Visibility = Visibility.Hidden;
            img_b.Visibility = Visibility.Visible;
            sc.Colore = "bianco";
            VisibleRegole();
        }
        private void BTTN_ScNero_Click(object sender, RoutedEventArgs e)
        {
            Dati.AddStringDI("sc;nero");
            Bttn_SCBianco.Visibility = Visibility.Hidden;
            BTTN_ScNero.Visibility = Visibility.Hidden;
            img_n.Visibility = Visibility.Visible;
            sc.Colore = "nero";
            VisibleRegole();
        }
        private void VisibleRegole()
        {
            //le rendo visibili
            RD_Amichevole.Visibility = Visibility.Visible; ;
            RD_Competitiva.Visibility = Visibility.Visible;
            BTTN_inviaR.Visibility = Visibility.Visible;

            lblR.Visibility = Visibility.Visible;
            RD_Personalizzata.Visibility = Visibility.Visible;
            RD_Amichevole.IsEnabled = true;
            RD_Competitiva.IsEnabled = true;
            RD_Personalizzata.IsEnabled = true;
            BTTN_inviaR.IsEnabled = true;
        }
        private void BBTN_Rivincitas_Click(object sender, RoutedEventArgs e)
        {
            Dati.AddStringDI("a;");
            sc.controllorivincita(true);
            BBTN_Rivincitas.IsEnabled = false;
            BBTN_Rivincitan.IsEnabled = false;
        }
        public void rivincita()
        {
            Dispatcher.Invoke(() =>
            {
                LBL_avversarioRiv.Content = "L'avversario vuole fare la rivincita";
                sc.controllorivincita(true);
            });
        }
        private void start()
        {
            BTTN_collegati.IsEnabled = false;
            L = new Listener(int.Parse(TXT_portaascolto.Text));
            WL = new WorkListener();
            W = new Writer(int.Parse(TXT_portascrittura.Text));
            CV = new ControllaVittoria();

            CVT = new Thread(new ThreadStart(CV.ProcThread));
            LT = new Thread(new ThreadStart(L.ProcThread));
            WLT = new Thread(new ThreadStart(WL.ProcThread));
            WT = new Thread(new ThreadStart(W.ProcThread));

            sc.setWindow(this);
            LT.Start();
            WLT.Start();
            WT.Start();
            CVT.Start();
        }

    }
}
