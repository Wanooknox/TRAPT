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
using Graph;

namespace TRAPT
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class EnvironmentStructures : EnvironmentObj //Microsoft.Xna.Framework.GameComponent
    {
        public EnvironmentStructures(Game game)
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
            // determine this object's cell position
            int cellX = (int)(this.position.X / 128);
            int cellY = (int)(this.position.Y / 128);

            IVertex<Cell> temp = ((TraptMain)Game).locationTracker.GetVertex(new Cell(cellX, cellY));
            EnvironmentObj temp2 = this;
            temp.Data.Add(new GameComponentRef(ref temp2));

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            

            base.Update(gameTime);
        }
    }
}
