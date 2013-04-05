using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
    public class Edge<T> : IEdge<T>
        where T : IComparable<T>
    {
        #region Attributes
        private IVertex<T> from;
        private IVertex<T> to;
        private bool isWeighted;
        private double weight;
        #endregion

        #region Constructor
        public Edge(IVertex<T> from, IVertex<T> to)
            : this(from, to, double.PositiveInfinity, false)
        {
        }
        public Edge(IVertex<T> from, IVertex<T> to, double weight)
            : this(from, to, weight, true)
        {
        }

        private Edge(IVertex<T> from, IVertex<T> to, double weight, bool isWeighted)
        {
            this.from = from;
            this.to = to;
            this.weight = weight;
            this.isWeighted = isWeighted;
        }
        #endregion

        #region Properties
        public IVertex<T> From
        {
            get 
            {
                return from;
            }
        }

        public IVertex<T> To
        {
            get
            {
                return to;
            }
        }

        public double Weight
        {
            get
            {
                return weight;
            }
        }

        public bool IsWeighted
        {
            get
            {
                return isWeighted;
            }
        }
        #endregion

        public int CompareTo(IEdge<T> other)
        {
            int result = Weight.CompareTo(other.Weight);
            if (result == 0)
            {
                result = From.CompareTo(other.From);
                if (result == 0)
                {
                    result = To.CompareTo(other.To);
                }
            }
            return result;
        }

        public override string ToString()
        {
            return "[from=" + from + ",to=" + to + (isWeighted ? ",weight=" + weight : "") + "]";//base.ToString();
        }
    }
}
