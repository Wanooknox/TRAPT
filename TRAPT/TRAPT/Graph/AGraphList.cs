using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
    public abstract class AGraphList<T> : AGraph<T>
        where T : IComparable<T>
    {
        protected List<List<IEdge<T>>> listAdjList;

        public AGraphList()
        {
            listAdjList = new List<List<IEdge<T>>>();
        }

        public override IEnumerable<IVertex<T>> EnumerateNeighbours(T data)
        {
            IVertex<T> v = GetVertex(data);
            List<IVertex<T>> neighbours = new List<IVertex<T>>();
            foreach (IEdge<T> e in listAdjList[v.Index])
            {
                neighbours.Add(e.To);
            }
            return neighbours;
        }

        protected override void AddVertexAdjustEdges(Vertex<T> v)
        {
            listAdjList.Add(new List<IEdge<T>>());
        }

        

        protected override void AddEdge(IEdge<T> e)
        {
            if (HasEdge(e.From.Data, e.To.Data))
            {
                throw new ApplicationException("Edge already exists");
            }
            listAdjList[e.From.Index].Add(e);
            numEdges++;
        }

        public override IEdge<T> GetEdge(T from, T to)
        {
            IVertex<T> vFrom = GetVertex(from);
            IVertex<T> vTo = GetVertex(to);
            if (!HasEdge(from, to))
            {
                throw new ApplicationException("Edge does not exist");
            }
            List<IEdge<T>> adjList = listAdjList[vFrom.Index];
            IEdge<T> found = null;
            for (int i = 0; i < adjList.Count && found == null; i++)
            {
                if (adjList[i].To.CompareTo(vTo) == 0)
                {
                    found = adjList[i];
                }
            }
            return found;
        }

        public override bool HasEdge(T from, T to)
        {
            IVertex<T> vFrom = GetVertex(from);
            IVertex<T> vTo = GetVertex(to);
            return (listAdjList[vFrom.Index].Contains(new Edge<T>(vFrom, vTo), new EdgeEqualityComparer<T>()));
        }

        

        protected override IEdge<T>[] GetAllEdges()
        {
            IEdge<T>[] edges = new IEdge<T>[numEdges];
            int i = 0;
            foreach (List<IEdge<T>> adjList in listAdjList)
            {
                foreach (IEdge<T> e in adjList)
                {
                    edges[i++] = e;
                }
            }
            return edges;
        }

        public override string ToString()
        {
            StringBuilder sbEdges = new StringBuilder("Edges:\n");
            for (int r = 0; r < listAdjList.Count; r++)
            {
                sbEdges.Append("Index " + r + ": ");
                bool commaAdded = false;
                foreach (IEdge<T> e in listAdjList[r])
                {
                    sbEdges.Append(e + ", ");
                    commaAdded = true;
                }
                if (commaAdded)
                {
                    sbEdges.Remove(sbEdges.Length - 2, 2);
                }
                sbEdges.Append("\n");
            }
            return base.ToString() + sbEdges;
        }

        #region Assignment Remove Edge and Vertex
        /// <summary>
        /// A method to remove a vertex from the graph. Also removes any associated edges.
        /// </summary>
        /// <param name="data">the vertex to remove</param>
        public override void RemoveVertex(T data)
        {
            //check if the vertex actually exists
            if (HasVertex(data))
            {
                //get the vertex to remove
                IVertex<T> toRemove = GetVertex(data);

                ////////////////COPY CURRENT EDGES AND VERTICES//////////////////
                //get a copy of all the edges - GetAllEdges was specially overridden in UGraphList
                IEdge<T>[] edges = GetAllEdges();
                //replace the edge List with the new blank one
                listAdjList = new List<List<IEdge<T>>>();

                //get a copy of all the current vertices
                IVertex<T>[] leftOverVert = new IVertex<T>[vertices.Count];
                vertices.CopyTo(leftOverVert);
                //blank the vertices List and Dictionary
                vertices = new List<IVertex<T>>();
                revLookup = new Dictionary<T, int>();

                ///////////////PROCESS EDGES AND VERTICES//////////////
                //loop through the vertices
                for (int i = 0; i < leftOverVert.Length; i++)
                {
                    //get the current vertex
                    IVertex<T> curr = leftOverVert[i];
                    //if the current vertex is NOT the one we intend to remove
                    if (curr.CompareTo(toRemove) != 0)
                    {
                        //re-add the vertex
                        AddVertex(curr.Data);
                    }
                }

                //re-add each of the edges that did not relate to the removed vertex.
                foreach (IEdge<T> e in edges)
                {
                    if (e.From.CompareTo(toRemove) != 0 && e.To.CompareTo(toRemove) != 0)
                    {
                        AddEdge(e.From.Data, e.To.Data, e.Weight);
                    }
                }
            }
        }

        /// <summary>
        /// a Method to remove an edge from the graph 
        /// </summary>
        /// <param name="from">the vertex from</param>
        /// <param name="to">the vertex to</param>
        public override void RemoveEdge(T from, T to)
        {
            //check if the edge actually exists
            if (HasEdge(from, to))
            {
                //if it does exist, delete it.
                listAdjList[GetVertex(from).Index].Remove(GetEdge(from, to));
                numEdges--;
            }
        }
        #endregion
    }
}
