using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ScacchiP2P
{
    public class Listener
    {
        DatiCondivisi Dati = DatiCondivisi.Istanza;

        UdpClient Server = new UdpClient();
        IPEndPoint riceveEP = new IPEndPoint(IPAddress.Any, 42069);
        private byte[] dataReceived;
        public Listener() { }

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
