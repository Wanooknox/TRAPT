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
    public class LevelChanger : Structure
    {

        public LevelChanger(Game game)
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
            //level changer
            this.texture = Game.Content.Load<Texture2D>("exitDoor");
            this.destination = new Rectangle(14 * TraptMain.GRID_CELL_SIZE, 5, 128, 128);


            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            if (this.destination.Intersects(TraptMain.player.Destination))
                TraptMain.hud.ContextTip = "Press 'space bar' to enter next level"; 
            if (this.destination.Intersects(TraptMain.player.Destination) && ks.IsKeyDown(Keys.Space))
            {                
                TraptMain.nextlvl = "level2";
                TraptMain.currentGameState = GameState.Loading;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.destination, Color.White);
        }
    }
}
