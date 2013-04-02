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
    public class Level2 : Level
    {
        private string mapName = "newMap";

        Vector2 playerStart;

        //Temp vars
        Weapon testGun;

        public Level2(Game game)
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
            //get the size of the map
            TraptMain.tileLayer.ReadMapDimensions(mapName);

            //populate the location tracker
            TraptMain.PopulateGraph();

            //load the map
            TraptMain.tileLayer.Initialize("mapTiles", Game.Content.RootDirectory);
            //this.tileLayer.Initialize(Content.Load<Texture2D>("spriteSheet"), Content.RootDirectory);
            TraptMain.tileLayer.OpenMap(mapName);

            //adjust the valid area for the camera
            TraptMain.camera.Limits = new Rectangle(0, 0, TraptMain.tileLayer.mapWidth * TraptMain.GRID_CELL_SIZE, TraptMain.tileLayer.mapHeight * TraptMain.GRID_CELL_SIZE);

            //initialize the player
            //this.playerStart = new Vector2(450, 300);
            //TraptMain.player = new Player(Game);
            //TraptMain.player.Initialize(this.playerStart);
            TraptMain.player.Position = new Vector2(450, 300);

            //TEMP add a tester gun
            Vector2 gunStart = new Vector2((Game.GraphicsDevice.Viewport.Width / 4) * 3, (Game.GraphicsDevice.Viewport.Height / 4) * 3);
            this.testGun = new Weapon(Game);
            this.testGun.Initialize(gunStart, 30, WeaponType.Shotgun);


            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //GameComponentCollection currComponentState = new GameComponentCollection();
            //currComponentState.Concat(Game.Components);
            //foreach (GameComponent i in currComponentState)
            //{

            //    i.Update(gameTime);

            //}

            MouseState ms = Mouse.GetState();

            Vector2 playerCamPos = new Vector2(TraptMain.player.Position.X - Game.GraphicsDevice.Viewport.Width / 2, TraptMain.player.Position.Y - Game.GraphicsDevice.Viewport.Height / 2);
            Vector2 cursorCamPos = new Vector2(TraptMain.cursor.Position.X - Game.GraphicsDevice.Viewport.Width / 2, TraptMain.cursor.Position.Y - Game.GraphicsDevice.Viewport.Height / 2);
            TraptMain.camera.Position = new Vector2((cursorCamPos.X + playerCamPos.X) / 2, (cursorCamPos.Y + playerCamPos.Y) / 2);

            


            base.Update(gameTime);
        }
    }
}