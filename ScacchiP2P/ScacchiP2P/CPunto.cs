namespace ScacchiP2P
{

    public class CPunto
    {
        private int _x;
        private int _y;
        bool _PosInCuiMangia;
        bool _PosInCuiMuove;
        public int x { get { return _x; } set { if (value >= 0 && value <= 7) _x = value; } }
        public int y { get { return _y; } set { if (value >= 0 && value <= 7) _y = value; } }

        public bool PosInCuiMangia { get { return _PosInCuiMangia; } set { _PosInCuiMangia = value; } }
        public bool PosInCuiMuove { get { return _PosInCuiMuove; } set { _PosInCuiMuove = value; } }

        public CPunto(int x, int y, bool PosInCuiMangia, bool PosInCuiMuove)
        {
            this.x = x;
            this.y = y;
            this.PosInCuiMangia = PosInCuiMangia;
            this.PosInCuiMuove = PosInCuiMuove;
        }
        public CPunto()
        {
            x = 0;
            y = 0;
        }
        public bool equal(CPunto p)
        {
            bool ris = true;
            if (x != p.x) ris = false;
            if (y != p.y) ris = false;
            return ris;
        }

    }
}
