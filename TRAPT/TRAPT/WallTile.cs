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
    public class WallTile : Tile //Microsoft.Xna.Framework.GameComponent
    {
        

        public WallTile(Game game)
            : base(game)
        {
            // TODO: Construct any child components here

        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize(Vector2 position, string textureStr, int tileCount)
        {
            this.DrawOrder = 1000;
            //this.textureName = "walls";

            position.X = position.X * 128;
            position.Y = position.Y * 128;

            ////cheack around me
            ////int mapSize = TraptMain.tileLayer.mapWidth * TraptMain.tileLayer.mapHeight;
            //// create a checking rectangle above
            //Rectangle projection = this.Destination;
            //projection.Inflate(-16, 0);
            //projection.Y -= 32;
            ////move through map grid
            //for (int i = 0; i < TraptMain.tileLayer.mapWidth; i++)
            //{
            //    for (int j = 0; j < TraptMain.tileLayer.mapHeight; j++)
            //    {
            //        //if the projection hits another wall
            //        if (TraptMain.tileLayer.MapData[i, j] != null && projection.Intersects(TraptMain.tileLayer.MapData[i, j].Destination))
            //        {

            //        }
            //    }
            //}
            

            //this.Visible = true;

            base.Initialize(position, textureStr, tileCount);
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
    }
}
