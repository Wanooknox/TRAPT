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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Cursor : Microsoft.Xna.Framework.GameComponent
    {
        //Tracking
        public Vector2 cursorPosition;

        //Drawing
        Texture2D cursorImg;

        public Cursor(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            //load the cursor image
            this.cursorImg = Game.Content.Load<Texture2D>("cursor");

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //update the cursor position
            MouseState ms = Mouse.GetState();
            this.cursorPosition.X = ms.X;
            this.cursorPosition.Y = ms.Y;

            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //draw the cursor
            spriteBatch.Draw(this.cursorImg, this.cursorPosition, Color.White);
        }
    }
}
