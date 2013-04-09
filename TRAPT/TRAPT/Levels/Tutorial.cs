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
    public class Tutorial : Level
    {
        private string mapName = @"Maps\Tutorial";
        private string xmlName = @"AIFiles\TutorialObjects";

        Vector2 playerStart;        

        //Temp vars
        Weapon testShotGun;
        Weapon testSMGGun;



        public Tutorial(Game game)
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
            TraptMain.tileLayer.Initialize("tileSheet2", Game.Content.RootDirectory);
            TraptMain.tileLayer.OpenMap(mapName);
            

            //adjust the valid area for the camera
            TraptMain.camera.Limits = new Rectangle(0, 0, TraptMain.tileLayer.mapWidth * TraptMain.GRID_CELL_SIZE, TraptMain.tileLayer.mapHeight * TraptMain.GRID_CELL_SIZE);

            //initialize the player
            this.playerStart = new Vector2(2*128-64, 3*128-64);
            TraptMain.player = new Player(Game);
            TraptMain.player.Initialize(this.playerStart);

            //Create the AI 
            TraptMain.xmlReader.populateEnemiesFromXML(xmlName);

            //Setup two guns (one of each)
            Vector2 gun1Start = new Vector2(13*128-64, 3*128-64);
            this.testShotGun = new Weapon(Game);
            this.testShotGun.Initialize(gun1Start, 30, WeaponType.Shotgun);
            Vector2 gun2Start = new Vector2(14 * 128 - 64, 3 * 128 - 64);
            this.testSMGGun = new Weapon(Game);
            this.testSMGGun.Initialize(gun2Start, 50, WeaponType.SMG);

            //Level Changer 
            LevelChanger exit = new LevelChanger(Game);
            exit.Initialize(128, "exitDoors", 28 * TraptMain.GRID_CELL_SIZE-5, 9 * TraptMain.GRID_CELL_SIZE, TraptMain.GRID_CELL_SIZE, TraptMain.GRID_CELL_SIZE);

            TutorialGuides guide = new TutorialGuides(Game);
            guide.Initialize();
           

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            Vector2 playerCamPos = new Vector2(TraptMain.player.Position.X - Game.GraphicsDevice.Viewport.Width / 2, TraptMain.player.Position.Y - Game.GraphicsDevice.Viewport.Height / 2);
            Vector2 cursorCamPos = new Vector2(TraptMain.cursor.Position.X - Game.GraphicsDevice.Viewport.Width / 2, TraptMain.cursor.Position.Y - Game.GraphicsDevice.Viewport.Height / 2);
            TraptMain.camera.Position = new Vector2((cursorCamPos.X + playerCamPos.X) / 2, (cursorCamPos.Y + playerCamPos.Y) / 2);
            
            base.Update(gameTime);
        }
    }
}