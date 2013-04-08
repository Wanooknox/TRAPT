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
    public class LevelChanger : Structure
    {


        public LevelChanger(Game game)
            : base(game) {   }



        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize(int frameSkip, string newTexture, int x, int y, int width, int height)
        {
            //level changer
            this.texture = Game.Content.Load<Texture2D>(newTexture);
            this.destination = new Rectangle( x, y, width, height );
            this.source = new Rectangle(frameSkip, 0, width, height);
            
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
                if (TraptMain.currentGameState == GameState.Tutorial)
                {
                    //Return to the main menu
                    TraptMain.player.Destroy();
                    TraptMain.nextlvl = "mainmenu";
                    TraptMain.nextGameState = GameState.MainMenu;
                    TraptMain.currentGameState = GameState.Loading;
                    TraptMain.cursor.ChangeMouseMode("menu");
                }
                else if( TraptMain.currentGameState == GameState.Playing)
                {
                    TraptMain.player.Destroy();
                    TraptMain.nextlvl = "level2";
                    TraptMain.currentGameState = GameState.Loading;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {          
            Vector2 origin = new Vector2(this.texture.Width/2, this.texture.Height/2);
            spriteBatch.Draw(this.texture, this.destination, this.source, Color.White);
        }
    }
}
