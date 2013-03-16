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
}
