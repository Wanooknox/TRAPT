using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRAPT
{
    /// <summary>
    /// Class used to store a list of game objects
    /// </summary>
    public class Cell : List<GameComponentRef>, IComparable<Cell>//, IEnumerable<Cell>
    {
        //List<GameComponentRef> items;
        private int x;
        private int y;

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(Object other)
        {
            bool result = false;
            if (other is Cell)
            {
                result = this.X == ((Cell)other).X && this.Y == ((Cell)other).Y;
            }
            return result;
        }

        public override int GetHashCode()
        {
            string temp = this.X + "," + this.Y + "";
            return temp.GetHashCode();
        }

        public int CompareTo(Cell other)
        {
            int result = 1;
            //if the y is higher, we are greater
            if (this.Y > other.Y)
            {
                result = 1;
            }
                //if the y is lower we are lower
            else if (this.Y < other.Y)
            {
                result = -1;
            }
            //if y is equal, compare x
            else //if (this.Y == other.Y)
            {
                //if x is greater we are greater
                if (this.X > other.X)
                {
                    result = 1;
                }
                //if x is lower we are lower
                else if (this.X < other.X)
                {
                    result = -1;
                }
                //else we are identical.
                else //if (this.X == other.X)
                {
                    result = 0;
                }
            }

            return result;
        }

        //public new IEnumerator<Cell> GetEnumerator()
        //{
        //    //throw new NotImplementedException();
        //    return this.GetEnumerator();
        //}
    }
}
