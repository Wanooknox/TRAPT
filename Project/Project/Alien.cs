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


namespace Project
{
    public class Alien : Microsoft.Xna.Framework.GameComponent
    {
        //Constants used for comparison
        Vector2 NORTH = new Vector2(0, -1);

        //testing
        public Vector2 screenPosition;

        //Position and Orientation
        public Vector2 velocity;
        public Vector2 alienPosition;
        public Vector2 direction;
        public double rotation;

        //Drawing fields

        public Texture2D texture;
        public Rectangle destination;
        //Nullable<Rectangle> source;
        public Vector2 spriteCenter;

        float acceleration = 0.05f;
        float friction = 0.01f;
        float maxSpeed = 1.0f;


        //Controlls
        protected Keys forwards;
        protected Keys backwards;
        protected Keys strafeL;
        protected Keys strafeR;

        //Mouse

        //MouseState mouse;
        Vector2 mouseVector;

        public Alien(Game game)
            : base(game)
        {

        }

        public double dotProduct(Vector2 v, Vector2 w)
        {
            return ((v.X * w.X) + (v.Y * w.Y));
        }

        public double getLength(Vector2 v)
        {
            return (Math.Sqrt(Math.Pow(v.X, 2) + Math.Pow(v.Y, 2)));
        }

        public double calculateAngle(Vector2 vec1, Vector2 vec2)
        {
            //Direction in radians
            double direction;

            Console.WriteLine("X: " + alienPosition.X + " Y: " + alienPosition.Y);
            if (vec1.X < 0)
            {
                direction = -1 * (Math.Acos(dotProduct(vec1, vec2) / (getLength(vec1) * getLength(vec2))));
            }
            else
            {
                direction = Math.Acos(dotProduct(vec1, vec2) / (getLength(vec1) * getLength(vec2)));
            }
            //direction = Math.Acos(dotProduct(mouseVector, NORTH));
            return direction;
        }


        public virtual void Initalize(Vector2 position, float theta)
        {
            texture = Game.Content.Load<Texture2D>("alien_with_tale");
            //texture = Game.Content.Load<Texture2D>("blueGuard");

            alienPosition = position;
            rotation = theta;
            destination = new Rectangle((int)alienPosition.X, (int)alienPosition.Y, texture.Height, texture.Width);

            spriteCenter = new Vector2(alienPosition.X + texture.Width / 2, alienPosition.Y + texture.Height / 2);

            forwards = Keys.W;
            backwards = Keys.S;
            strafeL = Keys.A;
            strafeR = Keys.D;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            MouseState ms = Mouse.GetState();
            mouseVector = new Vector2(ms.X, ms.Y);
            direction = mouseVector - alienPosition;
            rotation = calculateAngle(direction, NORTH);
            //rotation = 0;
            Console.WriteLine(velocity);

            if (ks.IsKeyDown(forwards))
            {
                velocity.Y += (float)(acceleration * Math.Cos(rotation + Math.PI));
                velocity.X += (float)(acceleration * Math.Sin(rotation));
            }

            if (ks.IsKeyDown(backwards))
            {
                velocity.Y -= (float)(acceleration * Math.Cos(rotation + Math.PI));
                velocity.X -= (float)(acceleration * Math.Sin(rotation));
            }

            if (ks.IsKeyDown(strafeL))
            {
                velocity.X -= acceleration;
            }

            if (ks.IsKeyDown(strafeR))
            {
                velocity.X += (acceleration);
            }


            //Apply friction
            if (Math.Abs(this.velocity.Length()) > this.friction)
            {
                this.velocity.X -= Math.Sign(this.velocity.X) * this.friction; // Whatever sign velocity is, 
                this.velocity.Y -= Math.Sign(this.velocity.Y) * this.friction; // apply friction in the opposite direction.

                // velocity.X += acceleration;
            }
            else
            { // If our velocity is closer to zero than the effect of friction, we should just stop. 
                this.velocity.X = 0;
                this.velocity.Y = 0;
            }


            //Check for collisions with walls
            //if (this.alienPosition.X + this.velocity.X < 32 ||
            //   Game.GraphicsDevice.Viewport.Width - 32 < this.alienPosition.X + this.velocity.X)
            //{
            //    this.velocity.X = this.velocity.X * -0.5f;
            //}
            //if (this.alienPosition.Y + this.velocity.Y < 32 ||
            //    Game.GraphicsDevice.Viewport.Height - 32 < this.alienPosition.Y + this.velocity.Y)
            //{
            //    this.velocity.Y = this.velocity.Y * -0.5f;
            //}


            alienPosition.Y += velocity.Y;
            alienPosition.X += velocity.X;
            screenPosition.Y = alienPosition.Y;
            screenPosition.X = alienPosition.X;


            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteCenter = new Vector2(texture.Width / 2, texture.Height / 2);

            this.destination.X = (int)Math.Round(this.alienPosition.X - this.destination.Width / 2);
            this.destination.Y = (int)Math.Round(this.alienPosition.Y - this.destination.Height / 2);


            //Console.WriteLine("X: " + pos.X + " Y: " + pos.Y);


            spriteBatch.Draw(texture, alienPosition, null, Color.White, (float)rotation, spriteCenter, 1, SpriteEffects.None, 0.0f);
            //spriteBatch.Draw(texture, pos, Color.White);

        }
    }
}
