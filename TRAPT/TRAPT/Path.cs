using System;
using System.Collections.Generic;
using System.Linq;

namespace TRAPT
{
    /**
     * This class is pretty straight forward.
     * Keeps track of nodes in the list, also has circular properties to deal with
     * what to do when at the end of the path ( could be implemented elsewhere, just more clean in here )
     * */
    public class Path : LinkedList<PathNode>
    {
        int current = 0;
        int previous;
        int next;

        /// <summary>
        /// Constructor
        /// </summary>
        public Path()
        {
            previous = current - 1;
            next = current + 1;
        }
        /// <summary>
        /// Go to the next node, set other nodes accordingly
        /// </summary>
        /// <returns></returns>
        public PathNode goNext()
        {
            previous = current;
            current = next;
            next = previous + 1;

            //Checking for the end of the list
            if(next == this.Count){
                next = 0;
            }

            return this.ElementAt(next);
        }

        /// <summary>
        /// Go to the previous node.
        /// </summary>
        /// <returns></returns>
        public PathNode goPrevious()
        {
            next = current;
            current = previous;
            previous = next - 1;

            //Checking for the start of the list, ie bounds.
            if (previous < 0)
            {
                previous = this.Count;
            }

            return this.ElementAt(current);
        }

        /// <summary>
        /// Getter for the currentNode
        /// </summary>
        /// <returns></returns>
        public PathNode getCurrent()
        {
            return this.ElementAt(current);
        }

        /// <summary>
        /// Only for the purpose of retrieving the next node without having to change the AI's currentNode.
        /// </summary>
        /// <returns></returns>
        public PathNode getNext()
        {
            return this.ElementAt(next);
        }

        /// <summary>
        /// Only for the purpose of retrieving the previous node without having to change the AI's currentNode
        /// </summary>
        /// <returns></returns>
        public PathNode getPrevious()
        {
            return this.ElementAt(previous);
        }
    }
}