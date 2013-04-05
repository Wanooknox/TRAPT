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
    public class Dot : EnvironmentObj//Microsoft.Xna.Framework.GameComponent
    {
        public Dot(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize(Vector2 position)
        {
            
            // TODO: Add your initialization code here
            this.position = position;// *128 + 64;
            this.position.X = position.X * 128 + 64;
            this.position.Y = position.Y * 128 + 64;

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

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Random temp = new Random();
            Vector2 tempPos = this.position;
            tempPos.X += (float)TraptMain.genRand.Next(32);
            spriteBatch.DrawString(TraptMain.font, "&", tempPos, Color.Lime);
        }
    }
}
