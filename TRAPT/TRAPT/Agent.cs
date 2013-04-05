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

        public virtual Vector2 WeaponPosition 
        {
            get
            {
                return this.position;
            }
        }
        //public virtual Vector2 WeaponOrigin
        //{ get { return new Vector2(32); } }

        // stats
        public int health;
        public bool isDead = false;

        public Circle soundCircle;       


        public bool stopAnimation;
        
        // A "frame" is one frame of the animation; a box around the player within the spirte map. 
        protected int frameCount = 0; // Which frame we are.  Values = {0, 1, 2}
        protected int aniStart = 0; // the index of the first frame
        protected int aniLength = 0; // the count of the frame on which to wrap on
        protected int aniRate = 16; // # milliseconds between frames.
        protected int aniRow = 0; // the row in the source to pull frames from
        protected int frameWidth = 64; // how wide a frame is
        protected int frameHeight = 64; // how tall a frame is.
        protected bool isLoop = true;

        // Keep a counter, to count the number of ticks since the last change of animation frame.
        //int animationCount; // How many ticks since the last frame change.
        //int animationMax = 10; // How many ticks to change frame after. 
        TimeSpan animationTimer = TimeSpan.Zero;

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

            this.isLoop = true;

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            if (!isDead)
            {
                try
                {
                    UpdateCheckin();
                }
                catch
                {
                    //TODO: Move actor back in world somehow
                }

                //this.UpdateAnimation(gameTime);             //Changed this
            }

            this.UpdateAnimation(gameTime);

            base.Update(gameTime);
        }

        public void UpdateCheckin()
        {
            // determine this object's cell position
            int cellX = (int)(this.position.X / 128);
            int cellY = (int)(this.position.Y / 128);

            IVertex<Cell> temp = TraptMain.locationTracker.GetVertex(new Cell(cellX, cellY));
            EnvironmentObj temp2 = this;
            ClearLocationCheckin();

            //add this object to the determined cell
            temp.Data.Add(new GameComponentRef(ref temp2));
            this.checkin = temp.Data;
        }

        public void ClearLocationCheckin()
        {
            EnvironmentObj temp = this;
            if (this.checkin != null)
            {
                this.checkin.Remove(new GameComponentRef(ref temp));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateAnimation(GameTime gameTime)
        {
            //if (!stopAnimation)
            //{
                //if need new frame
                if (this.animationTimer <= TimeSpan.Zero)
                {
                    // if frames left in loop
                    if (this.frameCount < this.aniLength)
                    {
                        //move to next frame
                        this.frameCount++;
                    }
                    else //else last frame
                    {
                        if (isLoop)
                        {
                            //loop "back to one"
                            this.frameCount = 0;
                        }
                    }
                    this.animationTimer = TimeSpan.FromMilliseconds(this.aniRate);
                }
                this.animationTimer -= gameTime.ElapsedGameTime;
            //}
            //else
            //{
            //    this.frameCount = 1;   //middle sprite with his feet underneath
            //}

            // Update the source rectangle, based on where in the animation we are.  
            this.source.X = (this.aniStart + this.frameCount) * this.frameWidth;
            this.source.Y = this.aniRow * this.frameHeight;
        }

    }
}
