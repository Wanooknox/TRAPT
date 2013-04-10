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
//using Microsoft.Xna.Framework.Net;
//using Microsoft.Xna.Framework.Storage;

namespace TRAPT
{
    public class HUD : Microsoft.Xna.Framework.GameComponent
    {
        //constants
        public const int MAX_HEALTH = 100;
        public const int MAX_ENERGY = 100;


        int health;
        int energy;
        int ammo;
        //int healthlost;

        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        public int Energy
        {
            get { return energy; }
            set { energy = value; }
        }
        public int Ammo
        {
          get { return ammo; }
          set { ammo = value; }
        }
        public string ContextTip { get; set; }

        Texture2D texH;
        Rectangle recH;
        Vector2   posH;

        Texture2D texE;
        Rectangle recE;
        Vector2 posE;

        //popup tips
        Texture2D texT;
        Rectangle recT;
        Vector2 posT;

        Texture2D backgrBars;

        SpriteFont ammoFont;
        //String ammoStr;
        Vector2 ammoPos;

        // energy timer and regen timers
        TimeSpan regenTimeE = TimeSpan.FromSeconds(0.1);
        TimeSpan regenTimeEleft;
        TimeSpan regenTimeH = TimeSpan.FromSeconds(0.1);
        TimeSpan regenTimeHleft;

        bool useEnergy;
        bool usedEnergy;
        bool isdead = false;
        bool isShooting = false;

        int recLeftPercent = 100;
        int timeLeftPercent = 100;

        public HUD(Game game) : base(game)
        {
            health = 100;
            energy = 100;
            ammo = 30;
            useEnergy = false;
        }


        // ------------------------------------
        public override void Initialize()
        {
            texH = Game.Content.Load<Texture2D>("Healthbar");
            //top right
            posH = new Vector2(Game.GraphicsDevice.Viewport.Width - texH.Width - 30, 55);
            //bottom right
            //posH = new Vector2(Game.GraphicsDevice.Viewport.Width - texH.Width, Game.GraphicsDevice.Viewport.Height - texH.Height);
            //new Vector2(30, 540);
            recH = new Rectangle(0, 0, texH.Width, texH.Height);

            texE = Game.Content.Load<Texture2D>("energybar");
            posE = new Vector2(Game.GraphicsDevice.Viewport.Width - texE.Width - 30, 80);
            //posE = new Vector2(30, 570);
            recE = new Rectangle(0, 0, texE.Width, texE.Height);

            texT = Game.Content.Load<Texture2D>("useIcon");
            posT = new Vector2(Game.GraphicsDevice.Viewport.Width/2, Game.GraphicsDevice.Viewport.Height/6);
            recT = new Rectangle(0, 0, texT.Width, texT.Height);
            this.ContextTip = "";

            ammoFont = Game.Content.Load<SpriteFont>("SpriteFont1");
            ammoPos = new Vector2(Game.GraphicsDevice.Viewport.Width - texH.Width - 30, 25);
            //ammoPos = new Vector2(30, 540-25);

            backgrBars = Game.Content.Load<Texture2D>("backgr_bars");

            health = 100;
            energy = 100;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rec = new Rectangle(0, 0, texH.Width + 20, texH.Height + 55);

            // background
            Color backgrColor = new Color(100,100,100,200);
            spriteBatch.Draw(backgrBars, new Vector2(posH.X - 10, posH.Y - 30), rec, backgrColor);
            
            // background for bars
            Rectangle backgrH = new Rectangle(0, 0, texH.Width, texH.Height);
            Rectangle backgrE = new Rectangle(0, 0, texE.Width, texE.Height);
            spriteBatch.Draw(texH, posH, backgrH, Color.Gray);
            spriteBatch.Draw(texE, posE, backgrE, Color.Black);

            // bars
            recH.Width = (int) ( ((this.health + 0.0) / (MAX_HEALTH + 0.0)) * texH.Width);
            recE.Width = (int) ( ((this.energy + 0.0) / (MAX_ENERGY + 0.0)) * texE.Width);
            spriteBatch.Draw(texH, posH, recH, Color.White);
            spriteBatch.Draw(texE, posE, recE, Color.White);
            
            // ammo
            Color textColor = Color.Yellow; 
            spriteBatch.DrawString(ammoFont, "Ammo: " + ammo, ammoPos, textColor);

            //context tip
            if (this.ContextTip != "") //if have context
            {
                // show context tip
                int tipWidth = texT.Width + this.ContextTip.Length * 10;

                posT.X = (Game.GraphicsDevice.Viewport.Width/2) - (tipWidth / 2);
                spriteBatch.Draw(texT, posT, recT, Color.White);
                spriteBatch.DrawString(ammoFont, this.ContextTip, new Vector2(posT.X + texT.Width + 10, posT.Y), Color.Blue);

                this.ContextTip = ""; //n context
            }


        }
        // -----------------------------------

        public void regenEnergy()
        {
            if (regenTimeEleft <= TimeSpan.Zero)
            {
                int recNewWidth = timeLeftPercent * texE.Width / 100;
                energy = timeLeftPercent;
                recE.Width = recNewWidth; 
                regenTimeEleft = TimeSpan.FromSeconds(0.1);
            }
        }

        public void regenHealth()
        { 
            if (regenTimeHleft <= TimeSpan.Zero)
            {
                health += 1;
                int newRecWidth = health * texH.Width / 100;
                recH.Width = newRecWidth;
                regenTimeHleft = TimeSpan.FromSeconds(0.1);
            }
        }

        public void looseAmmo(int looseA)
        {
            this.ammo -= looseA;
        }

    }
}
