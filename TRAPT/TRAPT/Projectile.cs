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
    public class Projectile : Agent//Microsoft.Xna.Framework.GameComponent
    {
        private string projectileType;
        private Random strayRandomizer;
        
        public Projectile(Game game)
            : base(game)
        {
            //((TraptMain)game).layers[1].Add(this);
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public virtual void Initialize(Vector2 position, float speed, float direction, string projectileType, ref Random strayRandomizer)
        {
            this.DrawOrder = 250;

            //image holding all the bullets
            this.texture = Game.Content.Load<Texture2D>("projectiles");

            //location and speed of the bullet
            this.position = position;
            this.speed = speed;

            //angle the bullet flies in.
            this.rotation = direction;
            this.direction = direction;

            this.projectileType = projectileType;
            this.strayRandomizer = strayRandomizer;

            GetSprite();
            CalculateVelocity();

            base.Initialize();
        }

        /// <summary>
        /// re-calculate the velocity values based on the current speed and direction values.
        /// </summary>
        private void CalculateVelocity()
        {
            double strayRange;
            switch (this.projectileType)
            {
                case "SMG":
                    strayRange = (Math.PI / 36);
                    break;
                case "shotgun":
                    strayRange = (Math.PI / 10);
                    break;
                default:
                    strayRange = (Math.PI / 360);
                    break;
            }

            //generate a random "stray" for the bullet
            //strayRandomizer = new Random();
            float stray = (float)(strayRandomizer.NextDouble() * strayRange);
            //if random int is 0, positive, else stray is negative
            stray = (strayRandomizer.Next(2) == 0) ? stray : -1 * stray;

            //alter the trajectory by +/- stray.
            this.rotation += stray;
            this.direction += stray;
            this.speed += stray;

            // do some fancy trig to find the right value for X and Y based onthe speed and direction
            this.velocity.Y = (float)(this.speed * Math.Cos(this.direction + Math.PI));
            this.velocity.X = (float)(this.speed * Math.Sin(this.direction));
        }

        /// <summary>
        /// used the projectileType, and owner to determine what sprite to use
        /// </summary>
        public void GetSprite()
        {
            //if a rifle shot.
            if (this.projectileType.Equals("SMG"))
            {
                //load the one floor view of the rifle
                this.source = new Rectangle(0, 0, 17, 48);
                this.destination = new Rectangle(0, 0, 17, 48);
            }
            else if (this.projectileType.Equals("shotgun"))
            {
                //TODO: adjust values for the shotgun shell.
                //load the one floor view of the rifle
                this.source = new Rectangle(0, 0, 17, 48);
                this.destination = new Rectangle(0, 0, 17, 48);
            }
          
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            this.position.X += this.velocity.X;
            this.position.Y += this.velocity.Y;

            //TODO: delete bullets outside of the game world.
            //if not in the game world.
            if (!((TraptMain)Game).IsInWorld(this.position))
            {
                this.Dispose(true);
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.destination.X = (int)Math.Round(this.position.X);
            this.destination.Y = (int)Math.Round(this.position.Y);

            Vector2 origin = new Vector2(this.source.Width / 2, this.source.Height / 2);
            spriteBatch.Draw(this.texture, this.destination, this.source, Color.White,
                this.rotation, // The rotation of the Sprite.  0 = facing up, Pi/2 = facing right
                origin,
                SpriteEffects.None, 0);
        }
    }
}
