using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
    public class Vertex<T> : IVertex<T>
        where T : IComparable<T>
    {
        #region Attributes
        private int index;
        private T data;
        #endregion

        #region Constructor
        public Vertex(int index, T data)
        {
            this.index = index;
            this.data = data;
        }
        #endregion

        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
            }
        }

        public T Data
        {
            get 
            {
                return data;
            }
        }

        public int CompareTo(IVertex<T> other)
        {
            return Index.CompareTo(other.Index);
        }
        public override string ToString()
        {
            return "[index=" + index + ",data=" + data + "]";
        }
    }
}
