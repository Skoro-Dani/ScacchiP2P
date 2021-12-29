using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ScacchiP2P
{
    public class Writer
    {
        private DatiCondivisi Dati = DatiCondivisi.Istanza;
        UdpClient client = new UdpClient();
        private byte[] data;
        public Writer() { }


        public void ProcThread()
        {
            int count = 0;
            while (!Dati.Flag)
            {
                if (Dati.DatiDI.Count() > count)
                {
                    data = Encoding.ASCII.GetBytes(Dati.DatiDI[count]);
                    client.Send(data, data.Length, Dati.IP, 12345);
                    count++;
                }
            }
        }
    }
}

