using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
    public class DGraphList<T> : AGraphList<T>
        where T : IComparable<T>
    {
        public DGraphList()
        {
            isDirected = true;
        }

    }
}
