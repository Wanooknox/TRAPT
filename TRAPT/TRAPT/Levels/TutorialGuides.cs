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


namespace TRAPT.Levels
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class TutorialGuides : EnvironmentObj//Microsoft.Xna.Framework.DrawableGameComponent
    {
        //guide images
        Texture2D moveGuide;
        Texture2D gunGuide;
        Texture2D powerGuide;
        Texture2D meleeGuide;
        //guide positions
        Vector2 movePos;
        Vector2 gunPos;
        Vector2 powerPos;
        Vector2 meleePos;

        public TutorialGuides(Game game)
            : base(game)
        {
            //TraptMain.layers[0].Add(this);
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            this.Depth = 10;

            this.moveGuide = Game.Content.Load<Texture2D>(@"Tutorial/MoveGuide");
            this.gunGuide = Game.Content.Load<Texture2D>(@"Tutorial/GunGuide");
            this.powerGuide = Game.Content.Load<Texture2D>(@"Tutorial/PowerGuide");
            this.meleeGuide = Game.Content.Load<Texture2D>(@"Tutorial/MeleeGuide");

            this.movePos = new Vector2(3 * 128, 1 * 128+32);
            this.gunPos = new Vector2(11 * 128, 1 * 128);
            this.powerPos = new Vector2(4 * 128, 8 * 128);
            this.meleePos = new Vector2(12 * 128, 9 * 128);

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
            spriteBatch.Draw(this.moveGuide, this.movePos, this.moveGuide.Bounds, Color.White, 0, 
                Vector2.Zero, 1, SpriteEffects.None, this.Depth);
            spriteBatch.Draw(this.gunGuide, this.gunPos, this.gunGuide.Bounds, Color.White, 0,
                Vector2.Zero, 1, SpriteEffects.None, this.Depth);
            spriteBatch.Draw(this.powerGuide, this.powerPos, this.powerGuide.Bounds, Color.White, 0,
                Vector2.Zero, 1, SpriteEffects.None, this.Depth);
            spriteBatch.Draw(this.meleeGuide, this.meleePos, this.meleeGuide.Bounds, Color.White, 0,
                Vector2.Zero, 1, SpriteEffects.None, this.Depth);

            //spriteBatch.Draw(moveGuide, movePos, Color.White);
            //spriteBatch.Draw(gunGuide, gunPos, Color.White);
            //spriteBatch.Draw(powerGuide, powerPos, Color.White);
            //spriteBatch.Draw(meleeGuide, meleePos, Color.White);
        }
    }
}
