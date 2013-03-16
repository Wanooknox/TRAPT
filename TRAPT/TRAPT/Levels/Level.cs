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
    public class Level : Microsoft.Xna.Framework.GameComponent
    {
        private EventListener listener;

        public Level(Game game)
            : base(game)
        {
            //this.listener = new EventListener(this);
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            ((TraptMain)Game).ConstructLayers();

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

        public void Destory()
        {
            int count = Game.Components.Count();
            //for all game components
            for (int i = count-1; 0 <= i ; i--)
            {
                //if not a crucial component
                if (!(Game.Components[i] is Player 
                    || (Game.Components[i] is Weapon && ((Weapon)Game.Components[i]).Owner is Player)
                    || Game.Components[i] is Cursor 
                    || Game.Components[i] is Camera))
                {
                    //destroy it
                    ((GameComponent)Game.Components[i]).Dispose();
                }
            }
            //dispose of self
            this.Dispose();
        }
    }

    class EventListener
    {
        private Level obj;

        public EventListener(Level obj)
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
