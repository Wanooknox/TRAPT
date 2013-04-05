using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TRAPT
{
    public class PathNode : IComparable<PathNode>
    {

        private int dwell;                                      //time the agent sits at the current node
        public Vector2 position = Vector2.Zero;


        public static int ARRIVED = 25;
        public static int WIDTH = 10;
        public static int HEIGHT = 10;

        private Dot debugDot;

        public PathNode(int xPos, int yPos, int dwell)
        {
            position.X = xPos;
            position.Y = yPos;
            this.dwell = dwell;

            //debugDot = new Dot(TraptMain.cursor.Game);
            //debugDot.Initialize(this.position);
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public int getDwell()
        {
            return dwell;
        }

        public override string ToString()
        {
            return "Position: ( " + position.X + " , " + position.Y + " ), Dwell Time: " + dwell;
        }

        public override int GetHashCode()
        {
            string temp = this.position.X + "," + this.position.Y + "";
            return temp.GetHashCode();
        }

        public override bool Equals(Object other)
        {
            bool result = false;
            if (other is PathNode)
            {
                result = this.position.X == ((PathNode)other).position.X && this.position.Y == ((PathNode)other).position.Y;
            }
            return result;
        }

        public int CompareTo(PathNode other)
        {
            int result = 1;
            //if the y is higher, we are greater
            if (this.position.Y > other.position.Y)
            {
                result = 1;
            }
            //if the y is lower we are lower
            else if (this.position.Y < other.position.Y)
            {
                result = -1;
            }
            //if y is equal, compare x
            else //if (this.Y == other.Y)
            {
                //if x is greater we are greater
                if (this.position.X > other.position.X)
                {
                    result = 1;
                }
                //if x is lower we are lower
                else if (this.position.X < other.position.X)
                {
                    result = -1;
                }
                //else we are identical.
                else //if (this.X == other.X)
                {
                    result = 0;
                }
            }

            return result;
        }
    }
}