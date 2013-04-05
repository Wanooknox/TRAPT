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
    //// A delegate type for hooking up change notifications.
    //public delegate void ChangedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public abstract class EnvironmentObj : Microsoft.Xna.Framework.DrawableGameComponent
    {
        //the coordinates of the obj
        protected Vector2 position;
        // destination is the rectangular location drawn within on screen
        // basically the area of pixels this object inhabits
        protected Rectangle destination;
        // the rectangle designating which part of the texture file to draw from
        protected Rectangle source;
        // the image file to load the sprite(s) from
        protected Texture2D texture;

        public KeyboardState ks, ksold;
        public MouseState ms, msold;
        public GamePadState gps, gpsold;

        public List<EnvironmentObj> imHitting;

        protected Texture2D pixelTexture;

        public Vector2 Position
        {
            get { return position; }
            set { this.position = value; }
        }

        public Rectangle Destination
        {
            get { return this.destination; }
            set { this.destination = value; }
        }

        protected float Depth
        {
            get
            {
                return 1.0f / this.DrawOrder;
            }
            set
            {
                this.DrawOrder = (int)value;
            }
        }

        public EnvironmentObj(Game game)
            : base(game)
        {
            game.Components.Add(this);
            //this.listener = new EventListener(this);
            
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            imHitting = new List<EnvironmentObj>();
            pixelTexture = Game.Content.Load<Texture2D>("Pixel");

            base.Initialize();
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

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public virtual bool IsColliding(EnvironmentObj that)
        {
            return this.destination.Intersects(that.destination);
        }

        public virtual void Collide(EnvironmentObj that)
        {
            
        }


    }

    //class EventListener
    //{
    //    private EnvironmentObj obj;

    //    public EventListener(EnvironmentObj obj)
    //    {
    //        this.obj = obj;
    //        // Add "ListChanged" to the Changed event on "List".
    //        this.obj.Disposed += new EventHandler<EventArgs>(EObjDispose);
    //            //.Changed += new ChangedEventHandler(ListChanged);
    //    }

    //    // This will be called whenever the list changes.
    //    private void EObjDispose(object sender, EventArgs e)
    //    {
    //        Console.WriteLine("Disposed of:" + this.obj);
    //        foreach (GameComponentCollection layer in TraptMain.layers)
    //        {
    //            if (layer.Contains(this.obj))
    //            {
    //                layer.Remove(this.obj);
    //                //layer[layer.IndexOf(this.obj)].Dispose();
    //            }
    //        }
    //    }

    //    public void Detach()
    //    {
    //        // Detach the event and delete the list
    //        this.obj.Disposed -= new EventHandler<EventArgs>(EObjDispose);
    //        this.obj = null;
    //    }
    //}
}
