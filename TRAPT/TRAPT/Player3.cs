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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Player3 : Microsoft.Xna.Framework.GameComponent
    {
        // PHYSICS FIELDS

        public Vector2 position;
        public Vector2 velocity;
        private float rotation;
        private float direction;

        float speed = 0f;
        private static float MAX_SPEED = 10f;
        private static float MIN_SPEED = 0f;
        float friction = 0.25f;
        float acceleration = 0.5f;
        float rotation_acceleration = 0.05f * (float)Math.PI;

        float spinCount = 0f;

        // CONTROLS

        // Set to protected so that subclasses can modify them.
        protected Keys up = Keys.W;
        protected Keys down = Keys.S;
        protected Keys left = Keys.A;
        protected Keys right = Keys.D;
        //protected Keys start = Keys.Space;
        //bool isMoving = false;


        // DRAWING FIELDS

        Texture2D texture;
        SpriteFont font;
        public Rectangle destination;
        public Rectangle source;
        Color color;
        //SoundEffect boing;



        // sprite shape
        int spriteStartX = 0; // X of top left corner of sprite 0. 
        int spriteStartY = 0; // Y of top left corner of sprite 0.
        int spriteWidth = 88;
        int spriteHeight = 88;

        public Player3(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public virtual void Initialize(Vector2 position, float rotation)
        {
            this.position = position;
            this.rotation = rotation;
            //this.direction = RandDir();

            //calculate a random sprite color
            Random randonGen = new Random();
            this.color = Color.FromNonPremultiplied(randonGen.Next(255), randonGen.Next(255), randonGen.Next(255), 255);

            this.destination = new Rectangle((int)this.position.X - this.spriteWidth / 2, (int)this.position.Y - this.spriteHeight / 2,
                this.spriteWidth, this.spriteHeight);
            this.source = new Rectangle(this.spriteStartX, this.spriteStartY, this.spriteWidth, this.spriteHeight);

            this.texture = Game.Content.Load<Texture2D>("face");
            this.font = Game.Content.Load<SpriteFont>("SpriteFont1");
            //this.boing = Game.Content.Load<SoundEffect>("boing");

            base.Initialize();
        }

        private float RandDir()
        {
            // set random rotation. random # from 0.0 - 1.0 * 2pi gets angle in radians
            return (float)((new Random()).NextDouble() * (2 * Math.PI));
        }

        private void StartMoving()
        {
            //change the direction to a random one.
            this.direction = RandDir();
            // calculate the velocity vector
            UpdateVelocity();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SpeedUp()
        {
            // as long as we are below the maximum speed
            if (this.speed < MAX_SPEED)
            {
                //add acceleration to the speed and recalculate the velocity vector
                this.speed += this.acceleration;
            }
            //UpdateVelocity();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SlowDown()
        {
            // as long as we are above the minimum speed
            if (this.speed > MIN_SPEED)
            {
                //subtract acceleration to the speed and recalculate the velocity vector
                this.speed -= this.acceleration;
                UpdateVelocity();
            }
        }

        /// <summary>
        /// re-calculate the velocity values based on the current speed and direction values.
        /// </summary>
        private void UpdateVelocity()
        {
            // do some fancy trig to find the right value for X and Y based onthe speed and direction
            this.velocity.Y = (float)(this.speed * Math.Cos(this.direction + Math.PI));
            this.velocity.X = (float)(this.speed * Math.Sin(this.direction));
        }

        /// <summary>
        /// re-calculate the direction angle from the current velocity and add a small offset
        /// </summary>
        private void UpdateDirection()
        {
            //calculate a new direction (in radians) based on the current velocity values
            this.direction = (float)Math.Atan2(this.velocity.X, -this.velocity.Y);
            //add a small ofset to differ the direction just a bit
            //this.direction += (float)((new Random()).NextDouble() * (Math.PI / 6));
        }


        private float MidAngle(float dir1, float dir2)
        {
            
            float finalDir;
            //if the difference in direction is less than 2 degrees
            if (Math.Abs(dir2 - dir1) < ((Math.PI * 2) / 360)
                || Math.Abs(dir1 - Math.PI) == dir2)
            {
                // enforce new direction
                finalDir = dir1;
            }
            else
            {
                //make sure dir2 is bigger
                if (dir1 > dir2)
                {
                    var temp = dir1;
                    dir1 = dir2;

                    dir2 = temp;
                }

                if (dir2 - dir1 > Math.PI)
                {
                    dir2 -= (float)(Math.PI * 2);
                }

                finalDir = (dir2 + dir1) / 2;
                if (finalDir < 0)
                {
                    finalDir += (float)(Math.PI * 2);
                }
            }

            //Console.WriteLine("" + finalDir);
            return finalDir;
        }

        private void MotionAdd(float dir, float speedAdd)
        {
            float midAngle;
            if (this.speed > MIN_SPEED)
            {
                midAngle = MidAngle(this.direction, dir);
            }
            else { midAngle = dir; }

            if (Math.Abs(this.velocity.Length()) < MAX_SPEED)
            {
                // do some fancy trig to find the right value for X and Y based on the speed and direction
                this.velocity.Y += (float)(speedAdd * Math.Cos(midAngle + Math.PI));
                this.velocity.X += (float)(speedAdd * Math.Sin(midAngle));

                this.speed += speedAdd;
                this.direction = midAngle;
            }
            else
            {
                this.speed = Math.Abs(this.velocity.Length());
                // do some fancy trig to find the right value for X and Y based on the speed and direction
                this.velocity.Y = (float)(this.speed * Math.Cos(midAngle + Math.PI));
                this.velocity.X = (float)(this.speed * Math.Sin(midAngle));

            }
            
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Moving update:
            KeyboardState ks = Keyboard.GetState();

            // Move faster or slower.
            if (ks.IsKeyDown(this.up))
            {
                //this.direction = (float)Math.PI * 2;
                //SpeedUp();

                //this.velocity.Y = -1 * (float)(this.speed);

                //this.velocity.Y -= (float)(this.acceleration);

                MotionAdd((float)(Math.PI * 2), this.acceleration);

            }
            else if (ks.IsKeyDown(this.down))
            {
                //this.direction = (float)Math.PI;
                //SpeedUp();

                //this.velocity.Y = (float)(this.speed);

                //this.velocity.Y += (float)(this.acceleration);

                MotionAdd((float)Math.PI, this.acceleration);
            }
            if (ks.IsKeyDown(this.left))
            {
                //this.direction = (float)(3 * Math.PI / 2);
                //SpeedUp();

                //this.velocity.X = -1 * (float)(this.speed);

                //this.velocity.X -= (float)(this.acceleration);

                MotionAdd((float)(3 * Math.PI / 2), this.acceleration);
            }
            else if (ks.IsKeyDown(this.right))
            {
                //this.direction = (float)Math.PI / 2;
                //SpeedUp();

                //this.velocity.X = (float)(this.speed);
                //this.velocity.X += (float)(this.acceleration);

                MotionAdd((float)(Math.PI / 2), this.acceleration);
            }


            // Include friction.
            // If our velocity (scalar magnitude of a vector = length of a vector) is greater than the effect of friction,
            // then friction should be applied in the opposite direction of the velocity.  
            if (Math.Abs(this.velocity.Length()) > this.friction)
            {
                this.velocity.X -= Math.Sign(this.velocity.X) * this.friction; // Whatever sign velocity is, 
                this.velocity.Y -= Math.Sign(this.velocity.Y) * this.friction; // apply friction in the opposite direction.
                this.speed -= this.friction;
            }
            else
            { // If our velocity is closer to zero than the effect of friction, we should just stop. 
                this.velocity.X = 0;
                this.velocity.Y = 0;
                this.speed = MIN_SPEED;
            }

            // Bounce off screen  
            if (this.position.X + this.velocity.X < this.spriteWidth
                || Game.GraphicsDevice.Viewport.Width < this.position.X + this.velocity.X)
            {
                //reverse the x velocity 
                //this.velocity.X = this.velocity.X * -1;
                this.velocity.X = 0;
                //calculate new direction angle from existing velocity
                //UpdateDirection();
                // start spinning and play boing!
                //this.spinCount = 3;
                //this.boing.Play();
            }
            if (this.position.Y + this.velocity.Y < this.spriteHeight
                || Game.GraphicsDevice.Viewport.Height < this.position.Y + this.velocity.Y)
            {

                //this.velocity.Y = this.velocity.Y * -1;
                this.velocity.Y = 0;
                //calculate new direction angle from existing velocity
                //UpdateDirection();
                // start spinning and play boing!
                //this.spinCount = 3;
                //this.boing.Play();
            }

            // Apply the velocity to the position.  
            this.position.Y += this.velocity.Y;
            this.position.X += this.velocity.X;
           

            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Basic destination rectangle updating from last time. 
            this.destination.X = (int)Math.Round(this.position.X - this.destination.Width / 2);
            this.destination.Y = (int)Math.Round(this.position.Y - this.destination.Height / 2);

            // Draw the player's texture.  
            // The origin is the point inside the source rectangle to rotate around.
            Vector2 origin = new Vector2(this.source.Width / 2, this.source.Height / 2);
            spriteBatch.Draw(this.texture, this.destination, this.source, this.color,
                // New parameters:
                this.rotation, // The rotation of the Sprite.  0 = facing up, Pi/2 = facing right
                origin,
                // Required, but, not important.
                SpriteEffects.None, 0);

            String debug = "Direction: " + this.direction * (180.0/Math.PI)
                + "\nVelocity: " + this.velocity
                + "\nSpeed: " + this.speed;

            spriteBatch.DrawString(this.font, debug, origin, Color.White);
        }

        public bool IsColliding(Player3 that)
        {
            return this.destination.Intersects(that.destination);
        }

        public void Collide(Player3 that)
        {
            // change the velocites
            Vector2 swapV = new Vector2(that.velocity.X, that.velocity.Y);
            that.velocity.X = this.velocity.X * -1;
            that.velocity.Y = this.velocity.Y * -1;
            this.velocity.X = swapV.X;
            this.velocity.Y = swapV.Y;
            //and input new motion
            this.UpdateDirection();
            that.UpdateDirection();
            //this.boing.Play();
            this.spinCount = 3;
            that.spinCount = 3;

        }

    }
}
