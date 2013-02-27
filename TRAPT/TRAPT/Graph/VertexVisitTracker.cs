using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
    internal class VertexVisitTracker<T> 
        where T : IComparable<T>
    {
        internal bool marked;
        internal IVertex<T> vertex;

        public VertexVisitTracker(IVertex<T> v)
        {
            this.vertex = v;
            marked = false;
        }
    }
}
