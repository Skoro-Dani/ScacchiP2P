using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ScacchiP2P
{
    public class Listener
    {
        DatiCondivisi Dati;

        UdpClient Server = new UdpClient();
        IPEndPoint riceveEP;
        private byte[] dataReceived;
        int porta;
        public Listener(int porta1)
        {
            porta = porta1;
            riceveEP = new IPEndPoint(IPAddress.Any, porta);
            Server.Client.Bind(riceveEP);
            Dati = DatiCondivisi.Istanza;
        }

        public void ProcThread()
        {
            
            Server.Client.ReceiveTimeout = 5000;
            while (!Dati.Flag)
            {
                try
                {
                    dataReceived = Server.Receive(ref riceveEP);
                    string risposta = Encoding.ASCII.GetString(dataReceived);
                    Dati.AddStringRL(risposta + ";" + riceveEP.Address);
                    Console.WriteLine(risposta);
                }
                catch (Exception e) { }
            }
        }
    }
}
