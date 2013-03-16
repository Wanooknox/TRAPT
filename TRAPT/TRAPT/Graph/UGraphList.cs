using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
    public class UGraphList<T> : AGraphList<T>
        where T : IComparable<T>
    {
        public UGraphList()
        {
            isDirected = false;
        }

        public override int NumEdges
        {
            get
            {
                return base.NumEdges / 2;
            }
        }

        public override void AddEdge(T from, T to)
        {
            base.AddEdge(from, to);
            base.AddEdge(to, from);
        }

        public override void AddEdge(T from, T to, double weight)
        {
            base.AddEdge(from, to, weight);
            base.AddEdge(to, from, weight);
        }

        #region Assignment Modified GetAllEdges() and RemoveEdge
        /// <summary>
        /// Overriden to sligtly adjust how the edges are obtained.
        /// </summary>
        /// <returns>a single dimentional array of the edges.</returns>
        protected override IEdge<T>[] GetAllEdges()
        {
            //the size of the array of edges must be half the number of edges
            //because we do not need any of the duplicate edges.
            IEdge<T>[] edges = new IEdge<T>[numEdges / 2];
            int i = 0;
            foreach (List<IEdge<T>> adjList in listAdjList)
            {
                foreach (IEdge<T> e in adjList)
                {
                    //get the edge that is a reverse of the current one
                    IEdge<T> opp = GetEdge(vertices[revLookup[e.To.Data]].Data, 
                        vertices[revLookup[e.From.Data]].Data);

                    if (!edges.Contains(opp))
                    {
                        edges[i++] = e;
                    }
                }
            }
            return edges;
        }

        /// <summary>
        /// Override of the Remove edge that removes the intended edge and it's duplicate.
        /// </summary>
        /// <param name="from">from vertex</param>
        /// <param name="to">to vertex</param>
        public override void RemoveEdge(T from, T to)
        {
            base.RemoveEdge(from, to);
            base.RemoveEdge(to, from);
        }
        #endregion
    }
}
