
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
    class Bug2_v2
    {
        PathNode goalNode;
        List<WallTile> obstacles;
        WallTile firstWall;
        Line PositionToGoal;

        enum wallSide { UP, DOWN, LEFT, RIGHT, UNKNOWN };

        wallSide tempWallside;

        const int comparisonDistance = 64;


        public PathNode Update(PathNode goalNode, List<WallTile> obstacles, Vector2 enemyPosition)
        {
            PathNode returnNode = new PathNode(0, 0, 0);
            Vector2 potentialPos = Vector2.Zero;
            Vector2 otherPotentialPos = Vector2.Zero;
            this.goalNode = goalNode;
            this.obstacles = obstacles;
            this.PositionToGoal = new Line(goalNode.getPosition(), enemyPosition);

            if (this.getFirstCollided() != null)
            {
                firstWall = this.getFirstCollided();
                tempWallside = this.getWallSide(firstWall, enemyPosition);

                if (tempWallside == wallSide.DOWN)
                {
                    Console.Write("The wall is down");
                    potentialPos = new Vector2(enemyPosition.X + comparisonDistance, enemyPosition.Y);
                    otherPotentialPos= new Vector2(enemyPosition.X - comparisonDistance, enemyPosition.Y);
                   /* if (Vector2.Distance(potentialPos, goalNode.getPosition()) < Vector2.Distance(otherPotentialPos, goalNode.getPosition()))
                    {
                        returnNode = new PathNode((int)potentialPos.X, (int)potentialPos.Y, 0);
                    }
                    else
                    {
                        returnNode = new PathNode((int)otherPotentialPos.X, (int)otherPotentialPos.Y, 0);
                    }*/
                    returnNode = new PathNode((int)potentialPos.X, (int)potentialPos.Y, 0);
                    
                }
                else if (tempWallside == wallSide.UP)
                {
                    Console.Write("The wall is up");
                    potentialPos = new Vector2(enemyPosition.X + comparisonDistance, enemyPosition.Y);
                    otherPotentialPos = new Vector2(enemyPosition.X - comparisonDistance, enemyPosition.Y);
                   /* if (Vector2.Distance(potentialPos, goalNode.getPosition()) < Vector2.Distance(otherPotentialPos, goalNode.getPosition()))
                    {
                        returnNode = new PathNode((int)potentialPos.X, (int)potentialPos.Y, 0);
                    }
                    else
                    {
                        returnNode = new PathNode((int)otherPotentialPos.X, (int)otherPotentialPos.Y, 0);
                    }*/
                    returnNode = new PathNode((int)potentialPos.X, (int)potentialPos.Y, 0);
                }
                else if (tempWallside == wallSide.LEFT)
                {
                    Console.Write("The wall is to the left");
                    potentialPos = new Vector2(enemyPosition.X, enemyPosition.Y + comparisonDistance);
                    otherPotentialPos = new Vector2(enemyPosition.X, enemyPosition.Y - comparisonDistance);
                   /* if (Vector2.Distance(potentialPos, goalNode.getPosition()) < Vector2.Distance(otherPotentialPos, goalNode.getPosition()))
                    {
                        returnNode = new PathNode((int)potentialPos.X, (int)potentialPos.Y, 0);
                    }
                    else
                    {
                        returnNode = new PathNode((int)otherPotentialPos.X, (int)otherPotentialPos.Y, 0);
                    }*/
                    returnNode = new PathNode((int)potentialPos.X, (int)potentialPos.Y, 0);
                }
                else if (tempWallside == wallSide.RIGHT)
                {
                   /* Console.Write("The wall is to the right");
                    potentialPos = new Vector2(enemyPosition.X, enemyPosition.Y + comparisonDistance);
                    otherPotentialPos = new Vector2(enemyPosition.X, enemyPosition.Y - comparisonDistance);
                    /*if (Vector2.Distance(potentialPos, goalNode.getPosition()) < Vector2.Distance(otherPotentialPos, goalNode.getPosition()))
                    {
                        returnNode = new PathNode((int)potentialPos.X, (int)potentialPos.Y, 0);
                    }
                    else
                    {
                        returnNode = new PathNode((int)otherPotentialPos.X, (int)otherPotentialPos.Y, 0);
                    }
                    returnNode = new PathNode((int) enemyPosition.X, ((int) (enemyPosition.Y + comparisonDistance)), 0);*/
                   
                    returnNode = new PathNode((int)enemyPosition.X, (int)(enemyPosition.Y + 1000), 0);
                }
                return returnNode;
            }
            else
            {
                return goalNode;
                //return null;
            }
           
        }

        public WallTile getFirstCollided()
        {
            foreach( WallTile w in this.obstacles)
            {
                if (PositionToGoal.intersects(w.Destination))
                {
                    return w;
                }
            }
            return null;
        }

        private wallSide getWallSide(WallTile w, Vector2 enemyPosition)
        {
            Vector2 wallCenter = new Vector2((w.Position.X + (128 / 2)), (w.Position.Y + (128 / 2)));
            wallSide returnSide = wallSide.UNKNOWN;

            if (enemyPosition.X <= wallCenter.X)
            {
                returnSide = wallSide.RIGHT;
            }
            else if (enemyPosition.X >= wallCenter.X)
            {
                returnSide = wallSide.LEFT;
            }
            else if (enemyPosition.Y <= wallCenter.Y)
            {
                returnSide = wallSide.UP;
            }
            else if (enemyPosition.Y >= wallCenter.Y)
            {
                returnSide = wallSide.DOWN;
            }
            return returnSide;
        }
    }
}
