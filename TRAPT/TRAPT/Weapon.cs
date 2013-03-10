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
<<<<<<< HEAD
    public class Weapon : EnvironmentObj//Microsoft.Xna.Framework.GameComponent
    {
        #region Attributes
        private int ammo;
        private string gunType;
        private EnvironmentObj owner;
        private Texture2D texture;
=======
    public class Weapon : Mover//Microsoft.Xna.Framework.GameComponent
    {
        #region Attributes
        //number of bullets
        private int ammo;
        //type of gun
        private string gunType;
        //agent that is using the gun
        private Agent owner;
        //count down before next bullet can be shot.
        private TimeSpan delay = TimeSpan.Zero;
        private Random projectileStrayer;


        //private Texture2D texture;
        
        //private float rotation;
>>>>>>> Weapons and better collisions
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
<<<<<<< HEAD
        public EnvironmentObj Owner
=======
        public Agent Owner
>>>>>>> Weapons and better collisions
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
<<<<<<< HEAD
=======
            this.DrawOrder = 300;

>>>>>>> Weapons and better collisions
            this.texture = Game.Content.Load<Texture2D>("guns");

            this.position = position;
            this.ammo = ammo;
            this.gunType = gunType;

<<<<<<< HEAD
            GetSprite();

            // determine this object's cell position
            int cellX = (int)(this.position.X / 128);
            int cellY = (int)(this.position.Y / 128);

            IVertex<Cell> temp = ((TraptMain)Game).locationTracker.GetVertex(new Cell(cellX, cellY));
            EnvironmentObj temp2 = this;

            //add this object to the determined cell
            temp.Data.Add(new GameComponentRef(ref temp2));
            this.checkin = temp.Data;
=======
            this.rotation = 0;

            this.projectileStrayer = new Random();

            GetSprite();
            CellPosition(true);

            //// determine this object's cell position
            //int cellX = (int)(this.position.X / 128);
            //int cellY = (int)(this.position.Y / 128);

            //IVertex<Cell> temp = ((TraptMain)Game).locationTracker.GetVertex(new Cell(cellX, cellY));
            //EnvironmentObj temp2 = this;

            ////add this object to the determined cell
            //temp.Data.Add(new GameComponentRef(ref temp2));
            //this.checkin = temp.Data;
>>>>>>> Weapons and better collisions

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
<<<<<<< HEAD
                if (this.gunType.Equals("rifle"))
=======
                if (this.gunType.Equals("SMG"))
>>>>>>> Weapons and better collisions
                {
                    //load the one floor view of the rifle
                    this.source = new Rectangle(0, 0, 64, 106);
                    this.destination = new Rectangle(0, 0, 64, 106);
<<<<<<< HEAD

=======
                }
                else if (this.gunType.Equals("shotgun"))
                {
                    //TODO: adjust numbers for the shotgun
                    //load the one floor view of the rifle
                    this.source = new Rectangle(0, 0, 64, 106);
                    this.destination = new Rectangle(0, 0, 64, 106);
>>>>>>> Weapons and better collisions
                }
            }
            else // i DO have an ower
            {
<<<<<<< HEAD
                if (this.gunType.Equals("rifle"))
=======
                if (this.gunType.Equals("SMG"))
                {
                    // load the in hads view for the rife
                    this.source = new Rectangle(64, 0, 108 - 64, 106);
                    this.destination = new Rectangle(64, 0, 108 - 64, 106);
                }
                else if (this.gunType.Equals("shotgun"))
>>>>>>> Weapons and better collisions
                {
                    // load the in hads view for the rife
                    this.source = new Rectangle(64, 0, 108 - 64, 106);
                    this.destination = new Rectangle(64, 0, 108 - 64, 106);
<<<<<<< HEAD

=======
                }
            }
        }

        public void CellPosition(bool set)
        {
            //if we need to set the location
            if (set)
            {
                // determine this object's cell position
                int cellX = (int)(this.position.X / 128);
                int cellY = (int)(this.position.Y / 128);

                IVertex<Cell> temp = ((TraptMain)Game).locationTracker.GetVertex(new Cell(cellX, cellY));
                EnvironmentObj temp2 = this;
                if (this.checkin != null)
                {
                    this.checkin.Remove(new GameComponentRef(ref temp2));
                }

                //add this object to the determined cell
                temp.Data.Add(new GameComponentRef(ref temp2));
                this.checkin = temp.Data;
            }
            else //else we need to unset the location.
            {
                if (this.checkin != null)
                {
                    EnvironmentObj temp2 = this;
                    this.checkin.Remove(new GameComponentRef(ref temp2));
>>>>>>> Weapons and better collisions
                }
            }
        }

<<<<<<< HEAD
=======
        public void SetOwner(bool pickup, Agent owner)
        {
            if (pickup)
            {
                this.Owner = owner;
                this.GetSprite();
                this.CellPosition(false);
            }
            else //drop gun
            {
                this.Owner = null;
                this.GetSprite();
                this.CellPosition(true);
            }
        }

>>>>>>> Weapons and better collisions
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
<<<<<<< HEAD
            }

            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
=======
                this.rotation = owner.Rotation;

                MouseState ms = Mouse.GetState();
                //if the shot delay is zero or less
                if (this.delay <= TimeSpan.Zero)
                {
                    //if button clicked and have enough ammo
                    if (ms.LeftButton == ButtonState.Pressed && this.ammo > 0)
                    {
                        if (this.gunType.Equals("SMG"))
                        {
                            Projectile bullet = new Projectile(Game);
                            bullet.Initialize(this.position, 10.0f, this.rotation, this.gunType, ref this.projectileStrayer);
                            //100 millisecond delay
                            this.delay = TimeSpan.FromMilliseconds(100);
                        }
                        else if (this.gunType.Equals("shotgun"))
                        {
                            //fire 10 projectiles at once for the shotgun.
                            for (int i = 0; i < 10; i++)
                            {
                                Projectile bullet = new Projectile(Game);
                                bullet.Initialize(this.position, 10.0f, this.rotation, this.gunType, ref this.projectileStrayer);
                                //1500 millisecond delay
                                this.delay = TimeSpan.FromMilliseconds(1500);
                            }
                        }
                        //decrease ammo
                        this.ammo -= 1;
                    }
                }
                else //lower the shot delay
                {
                    //current delay time minus the time since last update call.
                    this.delay -= gameTime.ElapsedGameTime;
                }
            }

            //TODO: fix the location of the weapon object on every step, because it's an EnvironmentAgent

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
>>>>>>> Weapons and better collisions
        {
            this.destination.X = (int)Math.Round(this.position.X);
            this.destination.Y = (int)Math.Round(this.position.Y);

            Vector2 origin = new Vector2(this.source.Width / 2, this.source.Height / 2);
            spriteBatch.Draw(this.texture, this.destination, this.source, Color.White,
<<<<<<< HEAD
                0, // The rotation of the Sprite.  0 = facing up, Pi/2 = facing right
=======
                this.rotation, // The rotation of the Sprite.  0 = facing up, Pi/2 = facing right
>>>>>>> Weapons and better collisions
                origin,
                SpriteEffects.None, 0);
        }
        #endregion
    }
}
