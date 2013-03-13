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
    public class Cursor : Mover//Microsoft.Xna.Framework.GameComponent
    {
        //Tracking
        Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        //Drawing
        Texture2D cursorImg;

        public Cursor(Game game)
            : base(game)
        {
            //game.Components.Add(this);
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            this.DrawOrder = 9000;
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
            this.position.X = ms.X;
            this.position.Y = ms.Y;

            this.position = Vector2.Transform(this.position, Matrix.Invert(((TraptMain)Game).camera.GetViewMatrix()));
            

            base.Update(gameTime);
        }

        public Vector2 GetMouseInWorld()
        {
            MouseState ms = Mouse.GetState();
            return Vector2.Transform(
                new Vector2(ms.X, ms.Y), 
                Matrix.Invert(
                    ((TraptMain)Game).camera.GetViewMatrix()
                    )
                    );
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
            //draw the cursor
            spriteBatch.Draw(this.cursorImg, this.position, Color.White);
        }
    }
}
