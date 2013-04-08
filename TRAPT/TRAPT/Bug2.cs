//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace TRAPT
//{
//    public class Bug2
//    {
//        public float accerleration = 1.0f;
//        public Vector2 velocity = Vector2.Zero;
//        float rotation = 0;
//        public Vector2 position = Vector2.Zero;
//        public Vector2 destination = Vector2.Zero;
//        // Flags whether the bug is moving around an object.
//        bool circumventing = false;
//        // The rectangle the bug collided with. 
//        private Rectangle collisionRectangle = new Rectangle();
//        private Vector2 collisionPoint = Vector2.Zero;
//        private List<Vector2> circumPath = new List<Vector2>();
//        private int pathStep = 0;
//        // The line between the bug and the goal.
//        private Line mLine;
//        public Rectangle AABB;

//        public Bug2()
//        {
            
//        }

//        public void Initialize(int size, Vector2 newPosition)
//        {
//            position = newPosition;
//            AABB = new Rectangle((int)Math.Round(newPosition.X), (int)Math.Round(newPosition.Y), size, size);
//        }

//        public void MoveTowardsDestination(PathNode currentNode, List<Obstacle> obstacles)
//        {
//            destination = currentNode.position;
//            /* When the bug is not traveling around an obstacle, have it figure out it's goal and then have it head in a straight line towards the goal. */
//            mLine = new Line(currentNode.position, position);

//            // Move towards the new destination. 
//            //Move(new Vector2(destination.location.X + destination.texture.Width / 2, destination.location.Y + destination.texture.Height / 2));
//            Move(currentNode.position);
//            AABB.X = (int)Math.Round(position.X) - AABB.Width / 2;
//            AABB.Y = (int)Math.Round(position.Y) - AABB.Height / 2;

//            bool collision = false;

//            for (int i = 0; i < obstacles.Count; i++)
//            {
//                if (AABB.Intersects(obstacles[i].position))
//                {
//                    collision = true;
//                    collisionRectangle = new Rectangle(obstacles[i].position.X - AABB.Width / 2, obstacles[i].position.Y - AABB.Height / 2, obstacles[i].position.Width + AABB.Height, obstacles[i].position.Height + AABB.Height);
//                }
//            }

//            /* If we collide with an obstacle we should start circumventing it i.e. traveling around it's perimeter. */
//            if (collision)
//            {
//                circumventing = true;
//                BackUp();
//            }
//        }

//        public void CreateCircumventPath()
//        {
//            // We haven't defined a path yet to get around the obstacle. 
//            pathStep = 0;
//            // Since we aren't already circumventing the object we need to find the closest corner. 
//            Vector2 topLeft = new Vector2(collisionRectangle.X, collisionRectangle.Y);
//            Vector2 topRight = new Vector2(collisionRectangle.X + collisionRectangle.Width, collisionRectangle.Y);
//            Vector2 bottomLeft = new Vector2(collisionRectangle.X, collisionRectangle.Y + collisionRectangle.Height);
//            Vector2 bottomRight = new Vector2(collisionRectangle.X + collisionRectangle.Width, collisionRectangle.Y + collisionRectangle.Height);

//            List<Vector2> corners = new List<Vector2>();
//            corners.Add(topLeft);
//            corners.Add(topRight);
//            corners.Add(bottomLeft);
//            corners.Add(bottomRight);
//            // The closest corner will be the starting point in the path to circumvent the object.
//            Vector2 closestCorner = new Vector2(float.MaxValue, float.MaxValue);
//            for (int i = 0; i < corners.Count; i++)
//            {
//                if (Vector2.Distance(corners[i], position) < Vector2.Distance(closestCorner, position))
//                {
//                    closestCorner = corners[i];
//                }
//            }

//            circumPath.Add(closestCorner);

//            // The second closest point will be the last point we will navigate in our path.
//            Vector2 farthestCorner = new Vector2(float.MaxValue, float.MaxValue);
//            for (int i = 0; i < corners.Count; i++)
//            {
//                if (corners[i] != closestCorner && Vector2.Distance(corners[i], position) < Vector2.Distance(farthestCorner, position))
//                {
//                    farthestCorner = corners[i];
//                }
//            }


//            // The third closest point will be the second point we will navigate in our path.
//            Vector2 secondCorner = new Vector2(float.MaxValue, float.MaxValue);
//            for (int i = 0; i < corners.Count; i++)
//            {
//                if (corners[i] != closestCorner && corners[i] != farthestCorner && Vector2.Distance(corners[i], position) < Vector2.Distance(secondCorner, position))
//                {
//                    secondCorner = corners[i];
//                }
//            }
//            circumPath.Add(secondCorner);

//            // The farthest point will be the third point we will navigate in our path.
//            Vector2 thirdCorner = new Vector2(float.MaxValue, float.MaxValue);
//            for (int i = 0; i < corners.Count; i++)
//            {

//                if (corners[i] != closestCorner && corners[i] != secondCorner && corners[i] != farthestCorner)
//                {

//                    thirdCorner = corners[i];
//                }
//            }
//            circumPath.Add(thirdCorner);

//            circumPath.Add(farthestCorner);

//        }

//        public void FollowCircumventPath()
//        {
//            if (Vector2.Distance(position, circumPath[pathStep]) < accerleration)
//            {
//                /* If the distance between the bug and the corner it is heading towards is less than the acceleration amount, 
//                 * then we need to start heading towards the next corner in the path. */
//                if (pathStep < circumPath.Count - 1)
//                {
//                    pathStep++;
//                }
//                else
//                {
//                    pathStep = 0;
//                }
//            }

//            if (Vector2.Distance(position, new Vector2(position.X, mLine.CalculateY(position.X))) < accerleration)
//            {// We have reached the m line on the other side of the obstacle so we can continue traveling towards the goal.
//                circumventing = false;
//                circumPath = new List<Vector2>();
//                collisionPoint = new Vector2();
//            }

//            else
//            { // We are still circumventing the obstacle, so update the bug's position.
//                Move(circumPath[pathStep]);
//            }
//        }

//        public void CircumventObstacle()
//        {
//            if (circumPath.Count == 0)
//            {
//                CreateCircumventPath();
//                Move(circumPath[pathStep]);
//                Move(circumPath[pathStep]);
//            }

//            // Otherwise we have the path already and so we follow it. 
//            else
//            {
//                FollowCircumventPath();
//            }
//        }

//        // Determine if we need a new position, then move towards the position.
//        public Vector2 Update(PathNode currentNode, List<Obstacle> obstacles)
//        {
//            if (!circumventing)
//            {
//                MoveTowardsDestination(currentNode, obstacles);
//            }

//           /* Here we are trying to circumvent the obstacle so start following it's edge. */
//            else
//            {
//                CircumventObstacle();
//            }

//            return position;
//        }

//        // Updates the bug's position by heading towards the destination.
//        public void Move(Vector2 newDestination)
//        {
//            destination = newDestination;

//            if (Math.Abs(position.X - destination.X) < accerleration && Math.Abs(position.Y - destination.Y) < accerleration)
//            {// The bug has reached the destination so stop moving.
//                velocity.X = 0;
//                velocity.Y = 0;
//            }
//            else
//            {// The bug is heading towards the goal.
//                Vector2 baseVelocity = Vector2.Zero;
//                baseVelocity.X = accerleration;
//                baseVelocity.Y = 0;

//                rotation = (float)Math.Atan2(destination.Y - position.Y, destination.X - position.X);

//                velocity.X = (baseVelocity.X * (float)Math.Cos(rotation)) - (baseVelocity.Y * (float)Math.Sin(rotation));
//                velocity.Y = (baseVelocity.X * (float)Math.Sin(rotation)) + (baseVelocity.Y * (float)Math.Cos(rotation));

//                position.X = position.X + velocity.X;
//                position.Y = position.Y + velocity.Y;
//            }

//        }

//        // The bug steps back from the direction it was heading in the case of a collision.
//        public void BackUp()
//        {
//            position = position - velocity;
//            position -= velocity;
//        }

//        //public void Draw(SpriteBatch spriteBatch)
//        //{
//        //    if(mLine != null)
//        //        mLine.Draw(spriteBatch);
//        //}
//    }    
//}
