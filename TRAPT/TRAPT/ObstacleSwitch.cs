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
    // Swich to turn on/off obstacles
    public class ObstacleSwitch : Switch
    {
        LinkedList<Obstacle> obstacles; // list of obstacles to change by this switch
        //TimeSpan delay = TimeSpan.Zero; // delay for the switch collision
        int texPosisiton;

        bool spaceIsDown = false;

        public int TexPosistion
        {
            get { return texPosisiton; }
            set { texPosisiton = value; }
        }

        public ObstacleSwitch(Game game)
            : base(game)
        { 
        
        }

        public LinkedList<Obstacle> Obstacles
        {
            get { return obstacles; }
            set { obstacles = value; }
        }

        public void Initialize(Vector2 posistion, string textureStr, int tileCount)
        {
            
            base.Initialize(posistion, textureStr, tileCount);
        }

        public override void Update(GameTime gameTime)
        {
            ks = Keyboard.GetState();
            gps = GamePad.GetState(PlayerIndex.One);

            if (this.IsColliding(TraptMain.player))
                TraptMain.hud.ContextTip = "Press 'Space Bar' to activate switch";

            if (this.IsColliding(TraptMain.player) 
                && (ks.IsKeyDown(Keys.Space) && !ksold.IsKeyDown(Keys.Space) 
                || gps.IsButtonDown(Buttons.A) && !gpsold.IsButtonDown(Buttons.A)) )// && delay >= TimeSpan.FromSeconds(2)
            {
                this.swStatus = !this.swStatus;
                //delay = TimeSpan.Zero;
               // spaceIsDown = false;
                Console.WriteLine("SpaceIsDown: " + spaceIsDown);
            }
            if (!this.swStatus)
            {
                this.tileCount = 1; // change the texture
                foreach (Obstacle obs in obstacles)
                {
                    obs.SwStatus = false; // turn off the obstacle(s)
                }
            }
            else
            {
                this.tileCount = 0;
                foreach (Obstacle obs in obstacles)
                {
                    obs.SwStatus = true; // turn on the obstacle(s)
                }
            }
            //delay += gameTime.ElapsedGameTime;


            //if (ks.IsKeyDown(Keys.Space))
            //{
            //    this.spaceIsDown = true;
            //}

            ksold = ks;
            gpsold = gps;

            base.Update(gameTime);


        }
    }
}
