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
    public class AgentDie : Agent //Microsoft.Xna.Framework.GameComponent
    {
        private TimeSpan life;
        private Vector2 spriteCenter;

        public AgentDie(Game game)
            : base(game)
        {
            TraptMain.layers[1].Add(this);
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize(Texture2D texture, int life, float rotation, Vector2 position, Rectangle source, Rectangle destination)
        {
            this.Depth = 200;
            this.texture = texture;            
            this.life = TimeSpan.FromMilliseconds(life);

            //drawing stuff
            this.rotation = rotation;
            this.position = position;
            this.source = source;
            this.destination = destination;

            base.Initialize();
        }

        public void SetAnimationParams(int aniStart, int aniLength, int aniRate, int aniRow, int frameWidth, int frameHeight)
        {
            this.frameCount = -1; // Which frame we are.  Values = {0, 1, 2}
            this.aniStart = aniStart; // the index of the first frame
            this.aniLength = aniLength; // the count of the frame on which to wrap on
            this.aniRate = aniRate; // # milliseconds between frames.
            this.aniRow = aniRow; // the row in the source to pull frames from
            this.frameWidth = frameWidth; // how wide a frame is
            this.frameHeight = frameHeight; // how tall a frame is.
            this.isLoop = false;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //if death object should stick around
            if (this.life >= TimeSpan.Zero)
            {
                //decrement life time
                this.life -= gameTime.ElapsedGameTime;
            }
            else //death object should be gone
            {
                this.Dispose();
            }

            spriteCenter = new Vector2((this.source.Width / 2), (this.source.Height / 2));
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.destination.X = (int)this.position.X;// -this.source.Width / 2;
            this.destination.Y = (int)this.position.Y;// -this.source.Height / 2;
            //this.source = texture.Bounds;            

            spriteBatch.Draw(this.texture, this.destination, this.source, Color.White,
                this.Rotation, // The rotation of the Sprite.  0 = facing up, Pi/2 = facing right
                spriteCenter,
                SpriteEffects.None, this.Depth);

            //re-center hit box
            this.destination.X = (int)this.position.X - this.source.Width / 2;
            this.destination.Y = (int)this.position.Y - this.source.Height / 2;
        }
    }
}
