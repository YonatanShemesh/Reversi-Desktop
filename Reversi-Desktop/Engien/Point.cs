namespace Engien
{
    public class Point
    {
        private int m_X;
        private int m_Y;

        // <summary> Point Ctor </summary>  
        public Point(int i_x, int i_y)
        {
            m_X = i_x;
            m_Y = i_y;
        }
        //// <summary> Properties for setter getter for X data member </summary>   
        public int x
        {
            get { return m_X; }

            set { m_X = value; }
        }
        //// <summary> Properties for setter getter for Y data member </summary>   
        public int y
        {
            get { return m_Y; }
            set { m_Y = value; }
        }
    }
}
