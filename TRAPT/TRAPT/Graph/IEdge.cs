using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
    public interface IEdge<T> : IComparable<IEdge<T>>
        where T : IComparable<T>
    {
        IVertex<T> From
        {
            get;
        }
        IVertex<T> To
        {
            get;
        }
        double Weight
        {
            get;
        }
        bool IsWeighted
        {
            get;
        }
    }
}
