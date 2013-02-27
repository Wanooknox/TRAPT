using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
    internal class EdgeEqualityComparer<T> : IEqualityComparer<IEdge<T>>
        where T : IComparable<T>
    {
        public bool Equals(IEdge<T> x, IEdge<T> y)
        {
            return (x.From.CompareTo(y.From) == 0 && x.To.CompareTo(y.To) == 0);
        }

        public int GetHashCode(IEdge<T> obj)
        {
            throw new NotImplementedException();
        }
    }
}
