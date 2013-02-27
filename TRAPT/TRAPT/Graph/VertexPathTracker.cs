using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
    internal class VertexPathTracker<T> : IComparable<VertexPathTracker<T>>
        where T : IComparable<T>
    {
        internal IVertex<T> vertex;
        internal double distance;
        internal IVertex<T> previous;
        internal bool visited;

        public VertexPathTracker(IVertex<T> vertex)
        {
            this.vertex = vertex;
            distance = double.PositiveInfinity;
            previous = null;
            visited = false;
        }

        #region Methods

        #endregion



        public int CompareTo(VertexPathTracker<T> other)
        {
            return distance.CompareTo(other.distance);
        }
    }
}
