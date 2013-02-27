using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
    public interface IVertex<T> : IComparable<IVertex<T>>
        where T : IComparable<T>
    {
        int Index
        {
            get;
        }
        T Data
        {
            get;
        }
    }
}
