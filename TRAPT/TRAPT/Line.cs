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
    public class Line
    {

        float slope;
        float yIntercept;
        Vector2 point1;
        Vector2 point2;

        //Drawing stuff
        Color color = Color.Blue;

        public Line(Vector2 newPoint1, Vector2 newPoint2 )
        {
            if (newPoint1.X <= newPoint2.X)
            {
                point1 = newPoint1;
                point2 = newPoint2;
            }
            else
            {
                point1 = newPoint2;
                point2 = newPoint1;
            }

            slope = (point1.Y - point2.Y) / (point1.X - point2.X);
            yIntercept = point1.Y - slope * point1.X;
        }

        public float getSlope()
        {
            return slope;
        }

        public float getYIntercept()
        {
            return yIntercept;
        }

        public float CalculateY(float x)
        {
            return (slope * x + yIntercept);
        }

        public bool intersects(Line line)
        {
            if (slope == line.getSlope())
            {
                if (yIntercept == line.getYIntercept())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (float.IsInfinity(slope))
            {
                float y = line.getSlope() * point1.X + line.getYIntercept();
                float maxY = Math.Max(point1.Y, point2.Y);
                float minY = Math.Min(point1.Y, point2.Y);

                if (y >= minY && y <= maxY )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if(float.IsInfinity(line.getSlope()))
            {
                float y = slope * line.point1.X + yIntercept;
                float otherMaxY = Math.Max(line.point1.Y, line.point2.Y);
                float otherMinY = Math.Min(line.point1.Y, line.point2.Y);
                float maxY = Math.Max(point1.Y, point2.Y);
                float minY = Math.Min(point1.Y, point2.Y);

                if (y >= minY && y <= maxY && y >= otherMinY && y <= otherMaxY)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            float Xintersect = (yIntercept - line.getYIntercept()) / (line.getSlope() - slope);
            float lineMaxX = Math.Max(line.point1.X, line.point2.X);
            float lineMinX = Math.Min(line.point1.X, line.point2.X);
            float MaxX = Math.Max(point1.X, point2.X);
            float MinX = Math.Min(point1.X, point2.X);

            if (MinX <= Xintersect && Xintersect <= MaxX && lineMinX <= Xintersect && Xintersect <= lineMaxX)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool intersects(Rectangle rectangle)
        {
            Vector2 recPoint1, recPoint2, recPoint3, recPoint4;
            // Top left corner of the rectangle.
            recPoint1.X = rectangle.X;
            recPoint1.Y = rectangle.Y;

            // Top right corner of the rectangle
            recPoint2.X = rectangle.X + rectangle.Width;
            recPoint2.Y = rectangle.Y;

            // Bottom left corner
            recPoint3.X = rectangle.X;
            recPoint3.Y = rectangle.Y + rectangle.Height;

            // Bottom right corner
            recPoint4.X = rectangle.X + rectangle.Width;
            recPoint4.Y = rectangle.Y + rectangle.Height;

            // Define the four lines of the rectangle moving clockwise from the top left corner.
            // 1--2
            // |  |
            // 3--4
            Line recLine12 = new Line(recPoint1, recPoint2);
            Line recLine24 = new Line(recPoint2, recPoint4);
            Line recLine43 = new Line(recPoint4, recPoint3);
            Line recLine31 = new Line(recPoint3, recPoint1);
          
            if (this.intersects(recLine12) || this.intersects(recLine24) || this.intersects(recLine43) || this.intersects(recLine31))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixelTexture)
        {
            int distance = (int)Vector2.Distance(point1, point2);
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            
            spriteBatch.Draw(pixelTexture, new Rectangle((int)point1.X, (int)point1.Y, distance, 2), new Rectangle(0, 0, 1, 1), Color.White, angle, Vector2.Zero, SpriteEffects.None, 0.0f);

        }

    }
}
