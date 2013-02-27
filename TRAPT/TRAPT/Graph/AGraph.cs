using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
    public abstract class AGraph<T> : IGraph<T>
        where T : IComparable<T>
    {
        #region Attributes
        protected List<IVertex<T>> vertices;
        protected Dictionary<T, int> revLookup;

        protected int numEdges;

        protected bool isDirected;
        protected bool isWeighted;
        #endregion

        #region Constructor
        public AGraph()
        {
            vertices = new List<IVertex<T>>();
            revLookup = new Dictionary<T, int>();
            numEdges = 0;
        }
        #endregion

        public int NumVertices
        {
            get
            {
                return vertices.Count;
            }
        }

        public virtual int NumEdges
        {
            get
            {
                return numEdges;
            }
        }

        public void AddVertex(T data)
        {
            if (HasVertex(data))
            {
                throw new ApplicationException("Vertex Exists");
            }
            Vertex<T> v = new Vertex<T>(NumVertices, data);
            vertices.Add(v);
            revLookup.Add(data, v.Index);
            //do something about the edges
            AddVertexAdjustEdges(v);
        }

        protected abstract void AddVertexAdjustEdges(Vertex<T> v);

        public bool HasVertex(T data)
        {
            return revLookup.ContainsKey(data);
        }

        public IVertex<T> GetVertex(T data)
        {
            if (!HasVertex(data))
            {
                throw new ApplicationException("No such vertex");
            }
            int index = revLookup[data];
            return vertices[index];
        }



        public IEnumerable<IVertex<T>> EnumerateVertices()
        {
            return vertices;
        }

        public abstract IEnumerable<IVertex<T>> EnumerateNeighbours(T data);

        public virtual void AddEdge(T from, T to)
        {
            if (numEdges == 0)
            {
                isWeighted = false;
            }
            else if (isWeighted)
            {
                throw new ApplicationException("Can't add unweighted edge to weighted graph");
            }
            Edge<T> e = new Edge<T>(GetVertex(from), GetVertex(to));
            AddEdge(e);
        }

        public virtual void AddEdge(T from, T to, double weight)
        {
            if (numEdges == 0)
            {
                isWeighted = true;
            }
            else if (!isWeighted)
            {
                throw new ApplicationException("Can't add weighted edge to unweighted graph");
            }
            Edge<T> e = new Edge<T>(GetVertex(from), GetVertex(to), weight);
            AddEdge(e);
        }

        protected abstract void AddEdge(IEdge<T> e);

        public abstract bool HasEdge(T from, T to);

        public abstract IEdge<T> GetEdge(T from, T to);

        public void DeptFirstTraversal(T start, VisitorDelegate<T> whatToDo)
        {
            IVertex<T> vStart = GetVertex(start);
            VertexVisitTracker<T>[] tracker = CreateVisitTracker();
            RecDFT(vStart, whatToDo, tracker);
        }

        private void RecDFT(IVertex<T> current, VisitorDelegate<T> whatToDo, VertexVisitTracker<T>[] tracker)
        {
            //http://library.books24x7.com/book/id_3949/viewer.asp?bookid=3949&chunkid=0470533945
            //Visit( v )
            whatToDo(current.Data);
            //Mark( v )
            tracker[current.Index].marked = true;
            //for every neighbor w of v
            foreach (IVertex<T> w in EnumerateNeighbours(current.Data))
            {
                //if w is not marked then
                if (!tracker[w.Index].marked)
                {
                    //DepthFirstTraversal(G, w)
                    RecDFT(w, whatToDo, tracker);
                }
            }

        }

        private VertexVisitTracker<T>[] CreateVisitTracker()
        {
            VertexVisitTracker<T>[] tracker =
                new VertexVisitTracker<T>[vertices.Count];
            for (int i = 0; i < tracker.Length; i++)
            {
                tracker[i] = new VertexVisitTracker<T>(vertices[i]);
            }
            return tracker;
        }

        #region Assignment Breadth First Traversal
        /// <summary>
        /// A method to perform a breadth first traversl on the graph
        /// </summary>
        /// <param name="start">the starting vertex</param>
        /// <param name="whatToDo">delegate to porcess the vertex.</param>
        public void BreadthFirstTraversal(T start, VisitorDelegate<T> whatToDo)
        {
            IVertex<T> vStart = GetVertex(start);
            VertexVisitTracker<T>[] tracker = CreateVisitTracker();
            //create a queue to hold unvisited vertices
            Queue<IVertex<T>> toVisit = new Queue<IVertex<T>>();
            RecBFT(vStart, whatToDo, toVisit, tracker);

        }
        /// <summary>
        /// A Recursive method to preform the breadth first search
        /// </summary>
        /// <param name="vCurr">the current vertex</param>
        /// <param name="whatToDo">delegate to process the vertex</param>
        /// <param name="toVisit">a queue of any vertexes left to process</param>
        /// <param name="tracker">tracks which vertexes are processed</param>
        private void RecBFT(IVertex<T> vCurr, VisitorDelegate<T> whatToDo,
            Queue<IVertex<T>> toVisit, VertexVisitTracker<T>[] tracker)
        {
            //process the current vertex
            whatToDo(vCurr.Data);
            //mark the current
            tracker[vCurr.Index].marked = true;

            //add all unmarked neighbours to the queue
            foreach (IVertex<T> w in EnumerateNeighbours(vCurr.Data))
            {
                //if w is not marked and not yet in the queue
                if (!tracker[w.Index].marked && !toVisit.Contains(w))
                {
                    //add the w to the queue
                    toVisit.Enqueue(w);
                }
            }
            //if there is something in the queue, recurse into the next vertex.
            if (toVisit.Count > 0)
            {
                RecBFT(toVisit.Dequeue(), whatToDo, toVisit, tracker);
            }
        }
        #endregion

        public IGraph<T> ShortestWeightedPath(T start, T end)
        {
            //set up steps
            IVertex<T> vStart = GetVertex(start);
            IVertex<T> vEnd = GetVertex(end);

            VertexPathTracker<T>[] tracker = CreatePathTracker();

            List<VertexPathTracker<T>> toDoList = CreatePathToDoList(tracker);

            // set the starting vertex dist to 0
            tracker[vStart.Index].distance = 0;
            VertexPathTracker<T> current;
            //loop while the todolist is not empty
            while (toDoList.Count > 0)
            {
                //sort the list
                toDoList.Sort();
                //pull off the lowest distance as the current vertex
                current = toDoList[0];
                toDoList.RemoveAt(0);
                //mark current as visited
                current.visited = true;
                // for each neighbor of current
                foreach (IVertex<T> w in EnumerateNeighbours(current.vertex.Data))
                {
                    //if neighbor is not visited
                    if (!tracker[w.Index].visited)
                    {
                        //calculate the proposed distance
                        double proposed = current.distance + GetEdge(current.vertex.Data, w.Data).Weight;
                        //if the proposed is les than the neighbours recorded distance
                        if (proposed < tracker[w.Index].distance)
                        {
                            //update the nieghbour distance
                            tracker[w.Index].distance = proposed;
                            tracker[w.Index].previous = current.vertex;
                        }
                    }
                }
            }
            //build a new graph with the path
            IGraph<T> result = (IGraph<T>)GetType().Assembly.CreateInstance(GetType().FullName);
            //start at the end of the path
            current = tracker[vEnd.Index];
            //add the vertex to the result
            result.AddVertex(current.vertex.Data);
            //loop while current's previous is not null
            while (current.previous != null)
            {
                //add previous to the graph
                result.AddVertex(current.previous.Data);
                //add the endge from previous to current to the result
                result.AddEdge(current.previous.Data, current.vertex.Data,
                    GetEdge(current.previous.Data, current.vertex.Data).Weight);
                //make previous the new current
                current = tracker[current.previous.Index];
            }

            return result;

        }

        private List<VertexPathTracker<T>> CreatePathToDoList(VertexPathTracker<T>[] tracker)
        {
            List<VertexPathTracker<T>> toDoList = new List<VertexPathTracker<T>>();
            foreach (VertexPathTracker<T> vpt in tracker)
            {
                toDoList.Add(vpt);
            }
            return toDoList;
        }

        private VertexPathTracker<T>[] CreatePathTracker()
        {
            VertexPathTracker<T>[] tracker = new VertexPathTracker<T>[vertices.Count];
            for (int i = 0; i < tracker.Length; i++)
            {
                tracker[i] = new VertexPathTracker<T>(vertices[i]);
            }
            return tracker;
        }

        public IGraph<T> MinimumSpanningTree()
        {
            //create the forrest -  one graph ofor each vertex
            AGraph<T>[] forest = CreateForest();
            //get a list off all edges in this graph
            IEdge<T>[] edges = GetAllEdges();
            //sort the edges
            Array.Sort(edges);

            int currEdge = 0;

            //while there is more than one edge in our forrest
            //and there edges to process

            while (forest.Length > 1 && currEdge < edges.Length)
            {
                //grab the lowest weighted edge
                IEdge<T> e = edges[currEdge++];
                //look in the forest to find where e.From is and e.To is. 
                //If they are in different graphs, then merge the two graphs together.
                int treeFrom = FindTree(forest, e.From.Data);
                int treeTo = FindTree(forest, e.To.Data);
                if (treeFrom != treeTo)
                {
                    //take all vertices and edges from tree to and add thm to tree from.
                    MergeTrees(forest, treeFrom, treeTo);
                    //add the edge to connect th two graphs together
                    forest[treeFrom].AddEdge(e.From.Data, e.To.Data, e.Weight);
                    //get rid of the old treeTo from the forest
                    forest = CutTree(forest, treeTo);
                }
            }
            //returns the only remaining Graph as the MST
            return forest[0];
        }

        private AGraph<T>[] CutTree(AGraph<T>[] forest, int treeTo)
        {
            //alocate a smaller forest
            AGraph<T>[] newForest = new AGraph<T>[forest.Length - 1];
            //copy the old ofrest into the new forest except for the graph at  treeTo
            Array.Copy(forest, 0, newForest, 0, treeTo);
            Array.Copy(forest, treeTo + 1, newForest, treeTo, newForest.Length - treeTo);
            //return new forest
            return newForest;
        }

        private void MergeTrees(AGraph<T>[] forest, int treeFrom, int treeTo)
        {
            //loop for all vertices in forest[treeTo] and add themm to forest[treeFrom]
            foreach (IVertex<T> v in forest[treeTo].EnumerateVertices())
            {
                forest[treeFrom].AddVertex(v.Data);
            }
            //loop for all edges in forest[treeTo] and add themm to forest[treeFrom]
            foreach (IEdge<T> e in forest[treeTo].GetAllEdges())
            {
                //have to avoid adding a duplicate edge
                //since we deal with an undirected graph, there will be 
                //an edge A-B and and aedge B-A, but trying to add both will cause an exception
                if (!forest[treeFrom].HasEdge(e.From.Data, e.To.Data))
                {
                    forest[treeFrom].AddEdge(e.From.Data, e.To.Data, e.Weight);
                }
            }
        }

        private int FindTree(AGraph<T>[] forest, T data)
        {
            int foundTree = -1;
            for (int i = 0; i < forest.Length && foundTree < 0; i++)
            {
                if (forest[i].HasVertex(data))
                {
                    foundTree = i;
                }
            }
            return foundTree;
        }

        protected abstract IEdge<T>[] GetAllEdges();

        private AGraph<T>[] CreateForest()
        {
            //allocate an array of graphs
            AGraph<T>[] forest = new AGraph<T>[vertices.Count];
            //for each vertex, create a graph and add the vertex to it
            foreach (IVertex<T> v in EnumerateVertices())
            {
                //creates a new graph using reflection
                forest[v.Index] = (AGraph<T>)GetType().Assembly.CreateInstance(GetType().FullName);
                //add current vertext to the new graph
                forest[v.Index].AddVertex(v.Data);
            }
            return forest;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            //loop through each vertex and add to the result
            foreach (IVertex<T> v in EnumerateVertices())
            {
                result.Append(v + ", ");
            }
            // take off the last comma
            if (vertices.Count > 0)
            {
                result.Remove(result.Length - 2, 2);
            }
            //return the result
            return GetType().Name + "\nvertices: " + result + "\n";
        }

        #region Assignment Remove Edge and Vertex
        public abstract void RemoveVertex(T data);
        public abstract void RemoveEdge(T from, T to);
        #endregion

    }
}
