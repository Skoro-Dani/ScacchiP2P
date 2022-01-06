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
        IPEndPoint riceveEP = new IPEndPoint(IPAddress.Any, 42069);
        private byte[] dataReceived;
        public Listener()
        {
            Dati = DatiCondivisi.Istanza;
        }

        public void ProcThread()
        {
            Server.Client.Bind(riceveEP);
            while (!Dati.Flag)
            {
                dataReceived = Server.Receive(ref riceveEP);
                string risposta = Encoding.ASCII.GetString(dataReceived);
                Dati.AddStringRL(risposta + ";" + riceveEP.Address);
                Console.WriteLine(risposta);
            }
        }
    }
}
