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
    // a obstacle turned on/off by ObstacleSwitch
    public class Obstacle : Switch
    {
        int texPosisiton;

        public int TexPosistion
        {
            get { return texPosisiton; }
            set { texPosisiton = value; }
        }
        public Obstacle(Game game)
            : base(game)
        { 
        
        }

        public override void Initialize()
        {
            this.Depth = 500;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (!swStatus) // swStatus is changed from ObstacleSwitch-class
            {
                this.tileCount = 2; // obstacle is off
            }
            else
            {
                this.tileCount = texPosisiton; // obstacle is on
            }

            if (this.IsColliding(TraptMain.player) && this.swStatus)
            {
                // collision detection or do whatever fun there is to do
                TraptMain.player.Collide(this);
            }

            base.Update(gameTime);
        }
    }
}
