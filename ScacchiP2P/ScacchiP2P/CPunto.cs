namespace ScacchiP2P
{

    public class CPunto
    {
        private int _x;
        private int _y;
        public int x { get { return _x; } set { if (value >= 0 && value <= 7) _x = value; } }
        public int y { get { return _y; } set { if (value >= 0 && value <= 7) _y = value; } }

        public CPunto(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public CPunto()
        {
            x = 0;
            y = 0;
        }
        /*public void addx(int i)
        {
            this.x += i;
        }
        public void addy(int i)
        {
            this.y += i;
        }*/
    }
}
