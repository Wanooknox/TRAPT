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
    public class Tile : EnvironmentStructures//Microsoft.Xna.Framework.GameComponent
    {
        

        protected string textureName;


        // A "frame" is one frame of the animation; a box around the player within the spirte map. 
        int tileCount = 0; // Which frame we are.  Values = {0, 1, 2}
        int tileSkipX = 128; // How much to move the frame in X when we increment a frame--X distance between top left corners.
        int tileStartX = 0; // X of top left corner of frame 0. 
        int tileStartY = 0; // Y of top left corner of frame 0.
        int tileWidth = 128; // X of right minus X of left. 
        int tileHeight = 128; // Y of bottom minus Y of top.

        public Tile(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public virtual void Initialize(Vector2 position, int tileCount)
        {
            this.Position = position;

            this.destination = new Rectangle(
                (int)this.Position.X,// - this.tileWidth / 2,
                (int)this.Position.Y,// - this.tileHeight / 2,
                this.tileWidth, this.tileHeight);

            this.tileCount = tileCount;
            this.source = new Rectangle(this.tileStartX + this.tileSkipX * this.tileCount, this.tileStartY, this.tileWidth, this.tileHeight);

            this.texture = Game.Content.Load<Texture2D>(this.textureName);


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

            // Update the source rectangle, based on where in the animation we are.  
            //this.source.X = this.frameStartX + this.frameSkipX * this.frameCount;

            // Draw the player's texture.  
            // The origin is the point inside the source rectangle to rotate around.
            //Vector2 origin = new Vector2(this.source.Width / 2, this.source.Height / 2);
            //spriteBatch.Draw(this.texture, this.Area, this.source, Color.White,
            //    // New parameters:
            //    this.rotation, // The rotation of the Sprite.  0 = facing up, Pi/2 = facing right
            //    origin,
            //    // Required, but, not important.
            //    SpriteEffects.None, 0);
            spriteBatch.Draw(this.texture, this.Destination, this.source, Color.White);

        }
    }
}
