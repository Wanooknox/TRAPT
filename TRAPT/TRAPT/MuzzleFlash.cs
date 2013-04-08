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
    public class MuzzleFlash : Agent
    {
        private TimeSpan life;
        private Vector2 spriteCenter;

        public MuzzleFlash(Game game)
            : base(game)
        {
            TraptMain.layers[1].Add(this);
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize(Vector2 position, float rotation) //Rectangle source, Rectangle destination)
        {
            this.Depth = 900;
            this.texture = Game.Content.Load<Texture2D>("muzzleflash");
            this.life = TimeSpan.FromMilliseconds(50);

            //drawing stuff
            this.rotation = rotation + (float)Math.PI/4;
            this.position = position;
            //this.position.X += 32;
            //this.position.Y += 32;
            this.source = new Rectangle(0, 0, 32, 32);
            this.destination = this.source;

            //animation stuff
            this.frameCount = -1; // Which frame we are.  Values = {0, 1, 2}
            this.aniStart = 0; // the index of the first frame
            this.aniLength = 4; // the count of the frame on which to wrap on
            this.aniRate = 10; // # milliseconds between frames.
            this.aniRow = 0; // the row in the source to pull frames from
            this.frameWidth = 32; // how wide a frame is
            this.frameHeight = 32; // how tall a frame is.
            this.isLoop = false;

            //spriteCenter = new Vector2((this.source.Width ), (this.source.Height ));
            spriteCenter = new Vector2(48, 48);

            base.Initialize();
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

            //spriteCenter = new Vector2((this.source.Width / 2), (this.source.Height / 2));
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
            //this.destination.X = (int)this.position.X - this.source.Width / 2;
            //this.destination.Y = (int)this.position.Y - this.source.Height / 2;
        }
    }
}
