﻿using System.Net.Sockets;
using System.Text;

namespace ScacchiP2P
{
    public class Writer
    {
        private DatiCondivisi Dati;
        UdpClient client = new UdpClient();
        private byte[] data;
        public Writer()
        {
            Dati = DatiCondivisi.Istanza;
        }


        public void ProcThread()
        {
            int count = 0;
            while (Dati.Flag==false)
            {
                if (Dati.GetLengthDI() > count)
                {
                    data = Encoding.ASCII.GetBytes(Dati.DatiDI[count]);
                    client.Send(data, data.Length, Dati.IP, 42069);
                    count++;
                    //Pulico il buffer di dati per alleggerire il programma
                    if (Dati.GetLengthDI() > 10)
                    {
                        for (int i = 0; i < 10; i++)
                            Dati.DeletePosDI(0);
                        count -= 10;
                    }
                }
            }
        }
    }
}

