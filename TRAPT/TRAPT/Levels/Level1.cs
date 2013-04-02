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
    public class Level1 : Level
    {
        private string mapName = "Level1Map";
        private string xmlName = "Level1Objects";

        Vector2 playerStart;

        //Temp vars
        Weapon testGun;

        public Level1(Game game)
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

            //WHY DO I HAVE TO DO THIS!?

            //Init camera
            //TraptMain.camera.Dispose(); ;
            //TraptMain.camera = new Camera(Game);
            //TraptMain.camera.Initialize(Game.GraphicsDevice.Viewport);
            ////camera.Limits = new Rectangle(0, 0, tileLayer.mapWidth * GRID_CELL_SIZE, tileLayer.mapHeight * GRID_CELL_SIZE);

            ////Init cursor
            //TraptMain.cursor.Dispose();
            //TraptMain.cursor = new Cursor(Game);
            //TraptMain.cursor.Initialize();
            //TraptMain.cursor.ChangeMouseMode("play");
            ////this.IsMouseVisible = true;

            //TraptMain.camera.Enabled = true;
            //TraptMain.cursor.Enabled = true;


            //////////////////

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
            this.playerStart = new Vector2(7424, 4224);
            TraptMain.player = new Player(Game);
            TraptMain.player.Initialize(this.playerStart);

            TraptMain.xmlReader.populateEnemiesFromXML(xmlName);

            //TEMP add a tester gun
            Vector2 gunStart = new Vector2((Game.GraphicsDevice.Viewport.Width / 4) * 3, (Game.GraphicsDevice.Viewport.Height / 4) * 3);
            this.testGun = new Weapon(Game);
            this.testGun.Initialize(gunStart, 200, WeaponType.SMG);


            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //WHY IS THIS UPDATE CALL SUDDENLY BREAKING THINGS!?
            //THE GAME SEEMS TO HAVE STARTED AUTO CALLING UPDATE ON ALL OBJECTS...
            //BUT ONLY ON NEW OBJECTS OR SOMETHING, BECAUSE WHEN YOU QUIT AND RESTART,
            //THE CAMERA AND CURSOR GET FUCKED UP AND NEED TO BE RESET AS WELL. UGH!
            //GameComponentCollection currComponentState = new GameComponentCollection();
            ////currComponentState.Concat(Game.Components);
            //for (int i = 0; i < Game.Components.Count(); i++)
            //{
            //    currComponentState.Add(Game.Components[i]);
            //}

            //foreach (GameComponent i in currComponentState)
            //{

            //    i.Update(gameTime);

            //}

            MouseState ms = Mouse.GetState();

            Vector2 playerCamPos = new Vector2(TraptMain.player.Position.X - Game.GraphicsDevice.Viewport.Width / 2, TraptMain.player.Position.Y - Game.GraphicsDevice.Viewport.Height / 2);
            Vector2 cursorCamPos = new Vector2(TraptMain.cursor.Position.X - Game.GraphicsDevice.Viewport.Width / 2, TraptMain.cursor.Position.Y - Game.GraphicsDevice.Viewport.Height / 2);
            TraptMain.camera.Position = new Vector2((cursorCamPos.X + playerCamPos.X) / 2, (cursorCamPos.Y + playerCamPos.Y) / 2);

            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.P))
            {
                //((TraptMain)Game).ChangeLevel("level2");
                TraptMain.nextlvl = "level2";
                TraptMain.currentGameState = GameState.Loading;
            }

            base.Update(gameTime);
        }
    }
}
