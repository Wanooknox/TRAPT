using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
    public class DGraphMatrix<T> : AGraphMatrix<T>
        where T : IComparable<T>
    {
        public DGraphMatrix()
        {
            isDirected = true;
        }

    }
}
