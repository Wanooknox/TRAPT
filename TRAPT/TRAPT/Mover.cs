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
    // A delegate type for hooking up change notifications.
    public delegate void ChangedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Mover : EnvironmentObj//Microsoft.Xna.Framework.GameComponent
    {
        #region Attributes

        //PHYSICS FIELDS
        public Vector2 velocity;
        protected float rotation;
        protected float direction;

        protected float speed = 0f;
        protected float friction = 0.25f;
        protected float acceleration = 0.5f;

        //TACKING FIELDS
        public Cell checkin;
        private EventListener listener;

        #endregion

        #region Properties
        /// <summary>
        /// gets the objects rotation.
        /// </summary>
        public float Rotation
        {
            get { return rotation; }
            //set { rotation = value; }
        }
        #endregion

        public Mover(Game game)
            : base(game)
        {
            this.listener = new EventListener(this);
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

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
    }

    class EventListener
    {
        private EnvironmentObj obj;

        public EventListener(EnvironmentObj obj)
        {
            this.obj = obj;
            // Add "ListChanged" to the Changed event on "List".
            this.obj.Disposed += new EventHandler<EventArgs>(EObjDispose);
            //.Changed += new ChangedEventHandler(ListChanged);
        }

        // This will be called whenever the list changes.
        private void EObjDispose(object sender, EventArgs e)
        {
            Console.WriteLine("Disposed of:" + this.obj);
            foreach (GameComponentCollection layer in TraptMain.layers)
            {
                if (layer.Contains(this.obj))
                {
                    layer.Remove(this.obj);
                    //layer[layer.IndexOf(this.obj)].Dispose();
                }
            }
        }

        public void Detach()
        {
            // Detach the event and delete the list
            this.obj.Disposed -= new EventHandler<EventArgs>(EObjDispose);
            this.obj = null;
        }
    }
}
