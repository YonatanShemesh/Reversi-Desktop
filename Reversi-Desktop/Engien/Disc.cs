namespace Engien
{
    public class Disc
    {
        private eColors m_Color;
        private readonly Point r_Location;

        public Disc(eColors i_Color, Point i_Location)
        {
            m_Color = i_Color;
            r_Location = i_Location;
        }

        public void Flip()
        {
            if (m_Color == eColors.Black)
            {
                m_Color = eColors.White;
            }
            else
            {
                m_Color = eColors.Black;
            }
        }

        public int Xargument
        {
            get { return r_Location.x; }
        }

        public int Yargument
        {
            get { return r_Location.y; }
        }

        public eColors Color
        {
            get { return m_Color; }
        }

        public enum eColors
        {
            Black = 'x',
            White = 'o'
        }
    }
}
