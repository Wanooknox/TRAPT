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
    public enum AdjoinType
    {
        None,
        Wall,
        Floor,
    }

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public abstract class Tile : Structure//Microsoft.Xna.Framework.GameComponent
    {

        //adjoing sides
        protected AdjoinType adjTop = AdjoinType.None;
        public AdjoinType adjBottom = AdjoinType.None;
        protected AdjoinType adjLeft = AdjoinType.None;
        public AdjoinType adjRight = AdjoinType.None;

        //protected string textureName;
        protected bool textureSet = false;


        // A "frame" is one frame of the animation; a box around the player within the spirte map. 
        int tileCount = 0; // Which frame we are.  Values = {0, 1, 2}
        int tileRow = 0;
        int tileSkipX = 128; // How much to move the frame in X when we increment a frame--X distance between top left corners.
        int tileStartX = 0; // X of top left corner of frame 0. 
        int tileStartY = 0; // Y of top left corner of frame 0.
        protected int tileWidth = 128; // X of right minus X of left. 
        protected int tileHeight = 128; // Y of bottom minus Y of top.

        public Tile(Game game)
            : base(game)
        {
            //TraptMain.layers[0].Add(this);
            // TODO: Construct any child components here
        }

        
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public virtual void Initialize(Vector2 position, string textureStr, int tileCount, int tileRow)
        {
            //position of the tile
            this.Position = position;

            //calculate the dest rectangle
            this.destination = new Rectangle(
                (int)this.Position.X,// - this.tileWidth / 2,
                (int)this.Position.Y,// - this.tileHeight / 2,
                this.tileWidth, this.tileHeight);

            //determine what tile to draw
            this.tileCount = tileCount;
            this.tileRow = tileRow;
            this.source = new Rectangle(this.tileStartX + this.tileSkipX * this.tileCount, this.tileStartY + this.tileSkipX * this.tileRow, this.tileWidth, this.tileHeight);

            //load the tilesheet
            this.texture = Game.Content.Load<Texture2D>(textureStr);

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if (!textureSet)
            {
                //need to make some adjustments for a wall tile
                if (this is WallTile)
                {
                    /////////////OUTSIDE CORNERS
                    if (this.adjTop == AdjoinType.Wall //&& this.adjBottom == AdjoinType.None
                        && this.adjLeft == AdjoinType.Wall )//&& this.adjRight == AdjoinType.None)
                    {
                        //bottom right outside corner
                        tileCount = 6;
                    }
                    else if (this.adjTop == AdjoinType.Wall /*&& this.adjBottom == AdjoinType.None*/
                       /* && this.adjLeft == AdjoinType.None */&& this.adjRight == AdjoinType.Wall)
                    {
                        //bottom left outside corner
                        tileCount = 7;
                    }
                    else if (/*this.adjTop == AdjoinType.None &&*/ this.adjBottom == AdjoinType.Wall
                        && this.adjLeft == AdjoinType.Wall )//&& this.adjRight == AdjoinType.None)
                    {
                        //top right outside corner
                        tileCount = 5;
                    }
                    else if (/*this.adjTop == AdjoinType.None &&*/ this.adjBottom == AdjoinType.Wall
                       /* && this.adjLeft == AdjoinType.None */&& this.adjRight == AdjoinType.Wall)
                    {
                        //top left outside corner
                        tileCount = 4;
                    }

                    //////VERTICLE SIDES
                    else if (this.adjTop == AdjoinType.Wall && this.adjBottom == AdjoinType.Wall
                        /*&& this.adjLeft == AdjoinType.None */&& this.adjRight == AdjoinType.Floor)
                    {
                        //left vert
                        tileCount = 8;
                    }
                    else if (this.adjTop == AdjoinType.Wall && this.adjBottom == AdjoinType.Wall
                        && this.adjLeft == AdjoinType.Floor /*&& this.adjRight == AdjoinType.None*/)
                    {
                        //right vert
                        tileCount = 9;
                    }

                    ///////////HORIZONTAL SIDES
                    else if (/*this.adjTop == AdjoinType.None && */this.adjBottom == AdjoinType.Floor
                        && this.adjLeft == AdjoinType.Wall && this.adjRight == AdjoinType.Wall)
                    {
                        //top hor
                        tileCount = 10;
                    }
                    else if (this.adjTop == AdjoinType.Floor //&& this.adjBottom == AdjoinType.None
                        && this.adjLeft == AdjoinType.Wall && this.adjRight == AdjoinType.Wall)
                    {
                        //bottom hor
                        tileCount = 11;
                    }

                    //////////INSIDE CORNERS
                    if (this.adjTop == AdjoinType.Floor && this.adjBottom == AdjoinType.Wall
                        && this.adjLeft == AdjoinType.Floor && this.adjRight == AdjoinType.Wall)
                    {
                        //top left inside corner
                        tileCount = 3;
                    }
                    else if (this.adjTop == AdjoinType.Floor && this.adjBottom == AdjoinType.Wall
                        && this.adjLeft == AdjoinType.Wall && this.adjRight == AdjoinType.Floor)
                    {
                        //top right inside corner
                        tileCount = 2;
                    }
                    else if (this.adjTop == AdjoinType.Wall && this.adjBottom == AdjoinType.Floor
                        && this.adjLeft == AdjoinType.Floor && this.adjRight == AdjoinType.Wall)
                    {
                        //bottom left inside corner
                        tileCount = 1;
                    }
                    else if (this.adjTop == AdjoinType.Wall && this.adjBottom == AdjoinType.Floor
                        && this.adjLeft == AdjoinType.Wall && this.adjRight == AdjoinType.Floor)
                    {
                        //bottom right inside corner
                        tileCount = 0;
                    }
                }

                this.source = new Rectangle(this.tileStartX + this.tileSkipX * this.tileCount, this.tileStartY + this.tileSkipX * this.tileRow, this.tileWidth, this.tileHeight);
                textureSet = true;
            }

            base.Update(gameTime);
        }

        public void CheckAroundMe()
        {
            //check around me

            // create a checking rectangle above
            Rectangle projectTop = this.Destination;
            projectTop.Inflate(-32, 0);
            projectTop.Y -= 64;
            //create checking rectangle to the left
            Rectangle projectLeft = this.Destination;
            projectLeft.Inflate(0, -32);
            projectLeft.X -= 64;

            //move through map grid
            for (int i = 0; i < TraptMain.tileLayer.mapWidth; i++)
            {
                for (int j = 0; j < TraptMain.tileLayer.mapHeight; j++)
                {
                    //if the projection hits another wall
                    if (TraptMain.tileLayer.MapData[i, j] != null && !(this.SameAs(TraptMain.tileLayer.MapData[i, j])))
                    {
                        Tile cheque = TraptMain.tileLayer.MapData[i, j];
                        if (projectTop.Intersects(cheque.Destination))
                        {
                            this.adjTop = (cheque is WallTile) ? AdjoinType.Wall : AdjoinType.Floor;//AdjoinType.Wall;
                            cheque.adjBottom = (this is WallTile) ? AdjoinType.Wall : AdjoinType.Floor;//AdjoinType.Wall;
                            //if (cheque is WallTile)
                            //{
                            //    //my top: wall and his bottom: wall
                            //    this.adjTop = AdjoinType.Wall;
                            //    cheque.adjBottom = (this is WallTile) ? AdjoinType.Wall : AdjoinType.Floor;//AdjoinType.Wall;
                            //}
                            //else if (cheque is FloorTile)
                            //{
                            //    //my top: floor and his bottom: wall
                            //    this.adjTop = AdjoinType.Floor;
                            //    cheque.adjBottom = (this is WallTile) ? AdjoinType.Wall : AdjoinType.Floor;//AdjoinType.Wall;
                            //}
                        }
                        if (projectLeft.Intersects(cheque.Destination))
                        {
                            this.adjLeft = (cheque is WallTile) ? AdjoinType.Wall : AdjoinType.Floor;//AdjoinType.Wall;
                            cheque.adjRight = (this is WallTile) ? AdjoinType.Wall : AdjoinType.Floor;//AdjoinType.Wall;
                            //if (cheque is WallTile)
                            //{
                            //    this.adjLeft = AdjoinType.Wall;
                            //    cheque.adjRight = (this is WallTile) ? AdjoinType.Wall : AdjoinType.Floor;//AdjoinType.Wall;
                            //}
                            //else if (cheque is FloorTile)
                            //{
                            //    this.adjLeft = AdjoinType.Floor;
                            //    cheque.adjRight = (this is WallTile) ? AdjoinType.Wall : AdjoinType.Floor; //AdjoinType.Wall;
                            //}
                        }


                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.Destination, this.source, Color.White);

        }

        public bool SameAs(Object other)
        {
            bool result = false;
            if (other is Tile)
            {
                result = (this.position.X == ((Tile)other).position.X && this.position.Y == ((Tile)other).position.Y);
            }
            return result;
        }
    }
}
