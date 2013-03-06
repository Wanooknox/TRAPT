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
    public class Weapon : EnvironmentObj//Microsoft.Xna.Framework.GameComponent
    {
        #region Attributes
        private int ammo;
        private string gunType;
        private EnvironmentObj owner;
        private Texture2D texture;
        //TACKING FIELDS
        public Cell checkin;
        #endregion

        #region Properties
        public int Ammo
        {
            get { return ammo; }
            set { ammo = value; }
        }
        public string GunType
        {
            get { return gunType; }
            set { gunType = value; }
        }
        public EnvironmentObj Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        #endregion
        
        #region Methods
        public Weapon(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }
        

        
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public virtual void Initialize(Vector2 position, int ammo, string gunType)
        {
            this.texture = Game.Content.Load<Texture2D>("guns");

            this.position = position;
            this.ammo = ammo;
            this.gunType = gunType;

            GetSprite();

            // determine this object's cell position
            int cellX = (int)(this.position.X / 128);
            int cellY = (int)(this.position.Y / 128);

            IVertex<Cell> temp = ((TraptMain)Game).locationTracker.GetVertex(new Cell(cellX, cellY));
            EnvironmentObj temp2 = this;

            //add this object to the determined cell
            temp.Data.Add(new GameComponentRef(ref temp2));
            this.checkin = temp.Data;

            base.Initialize();
        }

        /// <summary>
        /// used the guntype, and owner to determine what sprite to use
        /// </summary>
        public void GetSprite()
        {
            //if i don't have an owner
            if (owner == null)
            {
                if (this.gunType.Equals("rifle"))
                {
                    //load the one floor view of the rifle
                    this.source = new Rectangle(0, 0, 64, 106);
                    this.destination = new Rectangle(0, 0, 64, 106);

                }
            }
            else // i DO have an ower
            {
                if (this.gunType.Equals("rifle"))
                {
                    // load the in hads view for the rife
                    this.source = new Rectangle(64, 0, 108 - 64, 106);
                    this.destination = new Rectangle(64, 0, 108 - 64, 106);

                }
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //if i have an owner
            if (owner != null)
            {
                this.position.X = owner.Position.X;
                this.position.Y = owner.Position.Y;
            }

            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.destination.X = (int)Math.Round(this.position.X);
            this.destination.Y = (int)Math.Round(this.position.Y);

            Vector2 origin = new Vector2(this.source.Width / 2, this.source.Height / 2);
            spriteBatch.Draw(this.texture, this.destination, this.source, Color.White,
                0, // The rotation of the Sprite.  0 = facing up, Pi/2 = facing right
                origin,
                SpriteEffects.None, 0);
        }
        #endregion
    }
}
