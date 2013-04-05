using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TRAPT
{
    public class Circle
    {
        public Vector2 position;
        public float radius;

        public Circle()
        {
            position = Vector2.Zero;
        }

        public Circle(Vector2 newPosition, float newRadius)
        {
            position.X = newPosition.X;
            position.Y = newPosition.Y;
            radius = newRadius;
        }

        public void SetPosition(Vector2 newPosition)
        {
            position.X = newPosition.X;
            position.Y = newPosition.Y;
        }

        public void UpdatePosition(Vector2 position)
        {

        }

        public Vector2 GetPosition()
        {
            Vector2 returnPosition = new Vector2();
            returnPosition.X = position.X;
            returnPosition.Y = position.Y;
            return returnPosition;
        }

        public Boolean Intersects(Circle otherCircle)
        {
            double distance = Distance(otherCircle);
            if (distance < Math.Max(radius, otherCircle.radius))
                return true;
            else
                return false;
        }

        public Boolean Intersects(Rectangle otherRectangle)
        {
            double distance = Distance(otherRectangle);
            if (distance < radius)
                return true;
            else
                return false;
        }

        public double Distance(Rectangle otherRectangle)
        {
            return Math.Min(MinimumDistanceTop(otherRectangle), MinimumDistanceTop(otherRectangle));
        }

        private double MinimumDistanceTop(Rectangle otherRectangle)
        {
            Vector2 bottomLeft = new Vector2();
            bottomLeft.X = otherRectangle.X;
            bottomLeft.Y = otherRectangle.Y + otherRectangle.Height;
            double bottomLeftDistance = Distance(bottomLeft);

            Vector2 bottomRight = new Vector2();
            bottomRight.X = otherRectangle.X + otherRectangle.Width;
            bottomRight.Y = otherRectangle.Y + otherRectangle.Height;
            double bottomRightDistance = Distance(bottomRight);

            return Math.Min(bottomLeftDistance, bottomRightDistance);
        }

        private double MinimumDistanceBottom(Rectangle otherRectangle)
        {
            Vector2 topLeft = new Vector2();
            topLeft.X = otherRectangle.X;
            topLeft.Y = otherRectangle.Y;
            double topLeftDistance = Distance(topLeft);

            Vector2 topRight = new Vector2();
            topRight.X = otherRectangle.X + otherRectangle.Width;
            topRight.Y = otherRectangle.Y;
            double topRightDistance = Distance(topRight);

            return Math.Min(topLeftDistance, topRightDistance);
        }

        private double Distance(Vector2 otherPoint)
        {
            return Math.Sqrt(Math.Pow((position.X - otherPoint.X), 2) + Math.Pow((position.Y - otherPoint.Y), 2));
        }

        public double Distance(Circle otherCircle)
        {
            return Math.Sqrt(Math.Pow((position.X - otherCircle.position.X), 2) + Math.Pow((position.Y - otherCircle.position.Y), 2));
        }

        public void Draw(SpriteBatch spriteBatch, float rotation, Texture2D pixelTex)
        {
            Vector2 p1 = new Vector2((float)(((position.X + radius) * Math.Cos(rotation))), (float)((position.Y + radius) * Math.Sin(rotation)));
            Vector2 p2 = new Vector2((float)(((position.X + radius) * Math.Cos(rotation + Math.PI))), (float)((position.Y + radius) * Math.Sin(rotation + Math.PI)));
            Vector2 p3 = new Vector2((float)(((position.X + radius) * Math.Cos(rotation + Math.PI / 2))), (float)((position.Y + radius) * Math.Sin(rotation + Math.PI / 2)));
            Vector2 p4 = new Vector2((float)(((position.X + radius) * Math.Cos(rotation - Math.PI / 2))), (float)((position.Y + radius) * Math.Sin(rotation - Math.PI / 2)));

            Line l1 = new Line(position, p1);
            Line l2 = new Line(position, p2);
            Line l3 = new Line(position, p3);
            Line l4 = new Line(position, p4);

            l1.Draw(spriteBatch, pixelTex);
            l2.Draw(spriteBatch, pixelTex);
            l2.Draw(spriteBatch, pixelTex);
            l2.Draw(spriteBatch, pixelTex);
        }
    }
}
