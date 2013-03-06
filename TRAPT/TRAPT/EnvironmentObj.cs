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
    public abstract class EnvironmentObj : Microsoft.Xna.Framework.DrawableGameComponent
    {
        //the coordinates of the obj
        protected Vector2 position;
        // destination is the rectangular location drawn within on screen
        // basically the area of pixels this object inhabits
        protected Rectangle destination;
        // the rectangle designating which part of the texture file to draw from
        protected Rectangle source;
        // the image file to load the sprite(s) from
        protected Texture2D texture;

        public List<EnvironmentObj> imHitting;

        public Vector2 Position
        {
            get { return position; }
            set { this.position = value; }
        }

        public Rectangle Destination
        {
            get { return this.destination; }
            set { this.destination = value; }
        }

        public EnvironmentObj(Game game)
            : base(game)
        {
            game.Components.Add(this);
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            imHitting = new List<EnvironmentObj>();

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //// Basic destination rectangle updating from last time. 
            //this.destination.X = (int)Math.Round(this.position.X - this.destination.Width / 2);
            //this.destination.Y = (int)Math.Round(this.position.Y - this.destination.Height / 2);

            //// Draw the player's texture.  
            //// The origin is the point inside the source rectangle to rotate around.
            //Vector2 origin = new Vector2(this.source.Width / 2, this.source.Height / 2);
            //spriteBatch.Draw(this.texture, this.destination, this.source, this.color,
            //    this.rotation, // The rotation of the Sprite.  0 = facing up, Pi/2 = facing right
            //    origin,
            //    SpriteEffects.None, 0);

            //String debug = "Direction: " + this.direction * (180.0 / Math.PI)
            //    + "\nVelocity: " + this.velocity
            //    + "\nSpeed: " + this.speed;

            //spriteBatch.DrawString(this.font, debug, origin, Color.White);
        }

        public virtual bool IsColliding(EnvironmentObj that)
        {
            return this.destination.Intersects(that.destination);
        }

        public virtual void Collide(EnvironmentObj that)
        {
            // change the velocites
            //Vector2 swapV = new Vector2(that.velocity.X, that.velocity.Y);
            //that.velocity.X = this.velocity.X * -1;
            //that.velocity.Y = this.velocity.Y * -1;
            //this.velocity.X = swapV.X;
            //this.velocity.Y = swapV.Y;
            ////and input new motion
            //this.UpdateDirection();
            //that.UpdateDirection();

        }
    }
}
