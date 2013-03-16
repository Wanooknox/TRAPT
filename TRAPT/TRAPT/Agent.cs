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
    public class Agent : Mover//Microsoft.Xna.Framework.GameComponent
    {
        public Weapon Weapon { get; set; }

        public Agent(Game game)
            : base(game)
        {
            //TraptMain.layers[1].Add(this);
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

            IVertex<Cell> temp = TraptMain.locationTracker.GetVertex(new Cell(cellX, cellY));
            EnvironmentObj temp2 = this;

            //add this object to the determined cell
            temp.Data.Add(new GameComponentRef(ref temp2));
            this.checkin = temp.Data;

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            // determine this object's cell position
            int cellX = (int)(this.position.X / 128);
            int cellY = (int)(this.position.Y / 128);

            IVertex<Cell> temp = TraptMain.locationTracker.GetVertex(new Cell(cellX, cellY));
            EnvironmentObj temp2 = this;
            if (this.checkin != null)
            {
                this.checkin.Remove(new GameComponentRef(ref temp2));
            }

            //add this object to the determined cell
            temp.Data.Add(new GameComponentRef(ref temp2));
            this.checkin = temp.Data;

            base.Update(gameTime);
        }
    }
}
