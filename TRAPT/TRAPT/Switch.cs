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
    public class Switch : Structure
    {


        // A "frame" is one frame of the animation; a box around the player within the spirte map. 
        public int tileCount = 0; // Which frame we are.  Values = {0, 1, 2}
        int tileSkipX = 128; // How much to move the frame in X when we increment a frame--X distance between top left corners.
        int tileStartX = 0; // X of top left corner of frame 0. 
        int tileStartY = 0; // Y of top left corner of frame 0.
        int tileWidth = 128; // X of right minus X of left. 
        int tileHeight = 128; // Y of bottom minus Y of top.

        // switch
        public bool swStatus = true; // switch is on
        public Vector2 refPlayerVec = new Vector2();
        public int connection; // from XML file, how the switch is connected to the desired object we want to change.

        public int Connection
        {
            get { return connection; }
            set { connection = value; }
        }


        public bool SwStatus
        {
            get { return swStatus;}
            set { swStatus = value;}
        }

        public Switch(Game game)
            : base(game)
        { 
            
        }

        public void Initialize(Vector2 position, string textureStr, int tileCount)
        {
            this.Depth = 100;
            //this.DrawOrder = 0;
            //position of the tile
            this.Position = position;

            //calculate the dest rectangle
            this.destination = new Rectangle(
                (int)this.Position.X,// - this.tileWidth / 2,
                (int)this.Position.Y,// - this.tileHeight / 2,
                this.tileWidth, this.tileHeight);

            //determine what tile to draw
            this.tileCount = tileCount;
            this.source = new Rectangle(this.tileStartX + this.tileSkipX * this.tileCount, this.tileStartY, this.tileWidth, this.tileHeight);

            //load the tilesheet
            this.texture = Game.Content.Load<Texture2D>(textureStr);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.source = new Rectangle(this.tileStartX + this.tileSkipX * this.tileCount, this.tileStartY, this.tileWidth, this.tileHeight);
            spriteBatch.Draw(this.texture, this.Destination, this.source, Color.White,0,Vector2.Zero,SpriteEffects.None,this.Depth);
            //base.Draw(spriteBatch);
        }
    }
}
