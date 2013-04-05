using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
    public class AGraphMatrix<T> : AGraph<T>
        where T : IComparable<T>
    {
        #region Attributes
        protected IEdge<T>[,] matrix;

        #endregion

        #region constructor
        public AGraphMatrix()
        {
            matrix = new IEdge<T>[0, 0];
        }
        #endregion

        protected override void AddVertexAdjustEdges(Vertex<T> v)
        {
            //store the old matrix
            IEdge<T>[,] oldMatrix = matrix;
            //make a new matrix with extra row and column
            matrix = new IEdge<T>[NumVertices, NumVertices];
            //copy the edges from the old matrix to the new one
            for (int r = 0; r < oldMatrix.GetLength(0); r++)
            {
                for (int c = 0; c < oldMatrix.GetLength(1); c++)
                {
                    matrix[r, c] = oldMatrix[r, c];
                }
            }
        }

        protected override void AddEdge(IEdge<T> e)
        {
            if (matrix[e.From.Index, e.To.Index] != null)
            {
                throw new ApplicationException("Edge already exists");
            }
            matrix[e.From.Index, e.To.Index] = e;
            numEdges++;
        }


        public override bool HasEdge(T from, T to)
        {
            //IVertex<T> vFrom = GetVertex(from);
            //IVertex<T> vTo = GetVertex(to);
            //IEdge<T> e = matrix[vFrom.Index, vTo.Index];
            //return e != null;
            return matrix[GetVertex(from).Index, GetVertex(to).Index] != null;
        }

        public override IEdge<T> GetEdge(T from, T to)
        {
            if (!HasEdge(from, to))
            {
                throw new ApplicationException("No such edge");
            }
            return matrix[GetVertex(from).Index, GetVertex(to).Index];
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder("Edge matrix:\n");
            //loop for each row and column
            for (int r = 0; r < matrix.GetLength(0); r++)
            {
                result.Append("row " + r);
                for (int c = 0; c < matrix.GetLength(1); c++)
                {
                    result.Append(", " +
                        (matrix[r, c] == null ? "[ NULL ]" : matrix[r, c].ToString()));
                }
                result.Append("\n");
            }
            return base.ToString() + result; ;
        }

        public override IEnumerable<IVertex<T>> EnumerateNeighbours(T data)
        {
            IVertex<T> v = GetVertex(data);
            //make a list
            List<IVertex<T>> neighbours = new List<IVertex<T>>();
            //loop through the row associated with the vertex
            for (int c = 0; c < matrix.GetLength(1); c++)
            {
                //if there is an edge in the column
                if (matrix[v.Index, c] != null)
                {
                    //add the "to vertex to the list
                    neighbours.Add(matrix[v.Index, c].To);
                }
            }
            //return the list
            return neighbours;
        }

        protected override IEdge<T>[] GetAllEdges()
        {
            IEdge<T>[] edges = new IEdge<T>[numEdges];
            int i = 0;
            //loop through each ro and column
            for (int r = 0; r < matrix.GetLength(0); r++)
            {
                for (int c = 0; c < matrix.GetLength(1); c++)
                {
                    IEdge<T> e = matrix[r, c];
                    if (e != null)
                    {
                        edges[i++] = e;
                    }
                }
            }
            return edges;
        }

        #region Assignment Remove Edge and Vertex
        /// <summary>
        /// A method to remove a vertex from the graphs. Also removes all corresponding 
        /// edges and adjusts the size of the matrix accordingly
        /// </summary>
        /// <param name="data">the vertex to remove</param>
        public override void RemoveVertex(T data)
        {
            //check if the vertex actually exists
            if (HasVertex(data))
            {
                //get the vertex to remove
                IVertex<T> toRemove = GetVertex(data);

                //create a blank temp list and dictionary to hold the vertices
                List<IVertex<T>> leftOverVert = new List<IVertex<T>>();
                Dictionary<T, int> leftLookup = new Dictionary<T, int>();
                //loop through the vertices
                for (int i = 0; i < vertices.Count; i++)
                {
                    //get the current vertex
                    IVertex<T> curr = vertices[i];
                    //if the current vertex is NOT the one we intend to remove
                    if (curr.CompareTo(toRemove) != 0)
                    {
                        //get a new index value for the current vertex
                        //if it has an index greater than the removed vertex, 
                        //the index will decrement by 1 - other wise no change
                        int newIndex = (curr.Index > toRemove.Index) ? i - 1 : i;
                        //create a vertex with the new index and current data
                        IVertex<T> add = new Vertex<T>(newIndex, curr.Data);
                        //add it to the list and dictionary
                        leftOverVert.Add(add);
                        leftLookup.Add(curr.Data, add.Index);
                    }
                }
                //once all vertices to be preserved are in the temp list and dictionary
                //replace the current list and dictionary
                vertices = leftOverVert;
                revLookup = leftLookup;

                //make a new matrix, smaller by 1 level
                IEdge<T>[,] newMatrix = new IEdge<T>[matrix.GetLength(0) - 1, matrix.GetLength(1) - 1];
                //get a copy of all the edges - GetAllEdges was specially overridden in UGraphMatrix
                IEdge<T>[] edges = GetAllEdges();
                //replace the matrix with the new blank one
                matrix = newMatrix;
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
        /// A Method to remove an edge from the graph
        /// </summary>
        /// <param name="from">the edge point from</param>
        /// <param name="to">the edge point to</param>
        public override void RemoveEdge(T from, T to)
        {
            //check if the edge actually exists
            if (HasEdge(from, to))
            {
                //if it does exist, delete it.
                matrix[GetVertex(from).Index, GetVertex(to).Index] = null;
                numEdges--;
            }
        }
        #endregion
    }
}
