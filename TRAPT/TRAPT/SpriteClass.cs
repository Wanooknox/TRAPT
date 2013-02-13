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


namespace Midterm
{
    public class SpriteClass : Microsoft.Xna.Framework.GameComponent
    {
        //Constants used for comparison
        Vector2 NORTH = new Vector2(0, -1);


        //Position and Orientation
        Vector2 velocity;
        Vector2 pos;
        Vector2 direction;
        double rotation;

        //Drawing fields

        Texture2D texture;
        Rectangle destination;
        Nullable<Rectangle> source;
        Vector2 spriteCenter;

        float acceleration = 0.25f;
        float friction = 0.1f;

        //Controlls
        protected Keys forwards;
        protected Keys backwards;
        protected Keys strafeL;
        protected Keys strafeR;

        //Mouse

        MouseState mouse;
        Vector2 mouseVector;

        public SpriteClass(Game game) : base(game)
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

            Console.WriteLine("X: " + pos.X + " Y: " + pos.Y);
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
           

            texture = Game.Content.Load<Texture2D>("alien_middle");

            pos = position;
            rotation = theta;
            destination = new Rectangle((int)pos.X, (int)pos.Y, texture.Height, texture.Width);

            spriteCenter = new Vector2(pos.X + texture.Width / 2, pos.Y + texture.Height / 2);

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
            direction = mouseVector - pos;
            rotation = calculateAngle(direction, NORTH);
            //rotation = 0;
            //Console.WriteLine(rotation);

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
                //velocity.Y -= (float)(acceleration * Math.Cos(rotation +  Math.PI + (Math.PI)/2));
                //velocity.X -= (float)(acceleration * Math.Sin(rotation + (Math.PI/2)));

                velocity.X -= acceleration;
            }

            if (ks.IsKeyDown(strafeR))
            {
               // velocity.Y += (float)(acceleration * Math.Cos(rotation +  Math.PI + (Math.PI)/2));
               // velocity.X += (float)(acceleration * Math.Sin(rotation + (Math.PI/2)));

               velocity.X += (acceleration);
            }

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

            if (this.pos.X + this.velocity.X < 0 ||
               Game.GraphicsDevice.Viewport.Width < this.pos.X + this.velocity.X)
            {
                this.velocity.X = this.velocity.X * -0.5f;
            }
            if (this.pos.Y + this.velocity.Y < 0 ||
                Game.GraphicsDevice.Viewport.Height < this.pos.Y + this.velocity.Y)
            {
                this.velocity.Y = this.velocity.Y * -0.5f;
            }


      

            pos.Y += velocity.Y;
            pos.X += velocity.X;

             

            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        
            spriteCenter = new Vector2(texture.Width / 2,texture.Height / 2);

            this.destination.X = (int)Math.Round(this.pos.X - this.destination.Width / 2);
            this.destination.Y = (int)Math.Round(this.pos.Y - this.destination.Height / 2);


            //Console.WriteLine("X: " + pos.X + " Y: " + pos.Y);


            spriteBatch.Draw(texture, pos, null, Color.White, (float) rotation, spriteCenter, 1, SpriteEffects.None, 0.0f);
            //spriteBatch.Draw(texture, pos, Color.White);

        }
    }
}
