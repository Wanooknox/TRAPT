using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
    public delegate void VisitorDelegate<T>(T data);

    public interface IGraph<T>
        where T : IComparable<T>
    {
        #region Properties
        int NumVertices { get; }
        int NumEdges { get; }
        #endregion

        #region Methods to work with Vertices
        void AddVertex(T data);
        bool HasVertex(T data);
        IVertex<T> GetVertex(T data);
        void RemoveVertex(T data);

        IEnumerable<IVertex<T>> EnumerateVertices();
        IEnumerable<IVertex<T>> EnumerateNeighbours(T data);
        #endregion

        #region MEthods to work with edges
        void AddEdge(T from, T to);
        void AddEdge(T from, T to, double weight);
        bool HasEdge(T from, T to);
        IEdge<T> GetEdge(T from, T to);
        void RemoveEdge(T from, T to);
        #endregion

        #region implementations of algorithms
        void DeptFirstTraversal(T data, VisitorDelegate<T> whatToDo);
        void BreadthFirstTraversal(T data, VisitorDelegate<T> whatToDo);

        IGraph<T> ShortestWeightedPath(T start, T end);
        IGraph<T> MinimumSpanningTree();
        #endregion

    }
}
