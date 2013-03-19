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
    public enum WeaponType
    {
        SMG,
        Shotgun,
    }

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Weapon : Mover//Microsoft.Xna.Framework.GameComponent
    {
        
        #region Attributes
        //number of bullets
        private int ammo;
        //type of gun
        private WeaponType wpnType;
        //agent that is using the gun
        private Agent owner;
        //count down before next bullet can be shot.
        private TimeSpan delay = TimeSpan.Zero;
        private bool justPickedUp = false;
        private Random projectileStrayer;


        //private Texture2D texture;
        
        //private float rotation;
        //TACKING FIELDS
        //public Cell checkin;
        #endregion

        #region Properties
        public int Ammo
        {
            get { return ammo; }
            set { ammo = value; }
        }
        public WeaponType WpnType
        {
            get { return wpnType; }
            set { wpnType = value; }
        }

        public Agent Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        #endregion
        
        #region Methods
        public Weapon(Game game)
            : base(game)
        {
            //TraptMain.layers[1].Add(this);
            // TODO: Construct any child components here
        }
        

        
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public virtual void Initialize(Vector2 position, int ammo, WeaponType wpnType)
        {
            this.DrawOrder = 300;

            this.texture = Game.Content.Load<Texture2D>("guns");

            this.position = position;
            this.ammo = ammo;
            this.wpnType = wpnType;

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
                if (this.wpnType == WeaponType.SMG)
                {
                    //load the one floor view of the rifle
                    this.source = new Rectangle(0, 0, 64, 106);
                    this.destination = new Rectangle(0, 0, 64, 106);
                }
                else if (this.wpnType == WeaponType.Shotgun)
                {
                    //TODO: adjust numbers for the shotgun
                    //load the one floor view of the rifle
                    this.source = new Rectangle(0, 0, 64, 106);
                    this.destination = new Rectangle(0, 0, 64, 106);
                }
            }
            else // i DO have an ower
            {
                if (this.wpnType == WeaponType.SMG)
                {
                    // load the in hads view for the rife
                    this.source = new Rectangle(64, 0, 108 - 64, 106);
                    this.destination = new Rectangle(64, 0, 108 - 64, 106);
                }
                else if (this.wpnType == WeaponType.Shotgun)
                {
                    // load the in hads view for the rife
                    this.source = new Rectangle(64, 0, 108 - 64, 106);
                    this.destination = new Rectangle(64, 0, 108 - 64, 106);
                }
            }
        }

        /// <summary>
        /// Call to make the gun declare it's location
        /// </summary>
        /// <param name="set">true - declare loction, false - undeclare the location</param>
        public void CellPosition(bool set)
        {
            //if we need to set the location
            if (set)
            {
                // determine this object's cell position
                int cellX = (int)(this.position.X / 128);
                int cellY = (int)(this.position.Y / 128);

                IVertex<Cell> temp = TraptMain.locationTracker.GetVertex(new Cell(cellX, cellY));
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
                }
            }
        }

        /// <summary>
        /// Call to attach the gun to an agent (Player or Enemy)
        /// </summary>
        /// <param name="pickup">true - pick up the gun, false - drop the gun</param>
        /// <param name="owner">the agent to attach the gun to</param>
        public void PickUp(bool pickup, Agent owner)
        {
            if (pickup)
            {
                //if owner has no weapon, pick up
                if (owner.Weapon == null)
                {
                    this.Owner = owner;
                    this.Owner.Weapon = this;
                    //this.Owner.HasWeapon = true;
                    this.GetSprite();
                    this.CellPosition(false);
                    //this.delay = TimeSpan.FromMilliseconds(200);
                }
                else
                {
                    //force drop other gun
                    owner.Weapon.Drop();
                    //pick up this gun
                    this.Owner = owner;
                    this.Owner.Weapon = this;
                    //this.Owner.HasWeapon = true;
                    this.GetSprite();
                    this.CellPosition(false);
                }
                justPickedUp = true;
            }
            else //drop gun
            {
                //this.Owner.HasWeapon = false;
                this.Owner.Weapon = null;
                this.Owner = null;
                this.GetSprite();
                this.CellPosition(true);
            }
        }
        /// <summary>
        /// Call to make the gun shoot
        /// </summary>
        public void Shoot()
        {
            if (this.delay <= TimeSpan.Zero)
            {
                //MouseState ms = Mouse.GetState();

                //if button clicked and have enough ammo
                if (this.ammo > 0 )
                    //&& ((this.Owner is Player && ms.LeftButton == ButtonState.Pressed)
                    //|| (this.Owner is DummyEnemy && ((DummyEnemy)this.Owner).shooting)))
                {
                    if (this.wpnType == WeaponType.SMG)
                    {
                        Projectile bullet = new Projectile(Game);
                        bullet.Initialize(this.position, 20.0f, this.rotation, this.wpnType, ref this.projectileStrayer);
                        //100 millisecond delay
                        this.delay = TimeSpan.FromMilliseconds(100);
                    }
                    else if (this.wpnType == WeaponType.Shotgun)
                    {
                        //fire 10 projectiles at once for the shotgun.
                        for (int i = 0; i < 5; i++)
                        {
                            Projectile bullet = new Projectile(Game);
                            bullet.Initialize(this.position, 20.0f, this.rotation, this.wpnType, ref this.projectileStrayer);
                            //1500 millisecond delay
                            this.delay = TimeSpan.FromMilliseconds(1500);
                        }
                    }
                    //decrease ammo
                    this.ammo -= 1;
                }
            }
        }

        /// <summary>
        /// call to drop the gun
        /// </summary>
        public void Drop()
        {
            //if we can drop the gun
            if (this.delay <= TimeSpan.Zero)
            {
                //random angle
                this.rotation = (float)((new Random()).NextDouble() * (Math.PI * 2));
                //take away from owner
                this.Owner.Weapon = null;
                this.Owner = null;
                this.GetSprite();
                this.CellPosition(true);

                //this.SetOwner(false, new Agent(Game));
                //destroy gun
                //this.Dispose();
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            ks = Keyboard.GetState();
            //if i have an owner
            if (owner != null)
            {
                this.position.X = owner.Position.X;
                this.position.Y = owner.Position.Y;
                this.rotation = owner.Rotation;

                //if the shot delay is greater than zero
                if (this.delay > TimeSpan.Zero)
                {
                    //lower delay
                    this.delay -= gameTime.ElapsedGameTime;
                }

                if (!justPickedUp)
                {
                    if (this.owner is Player && !ks.IsKeyDown(Keys.R) && ksold.IsKeyDown(Keys.R))
                    {
                        this.Drop();
                    }
                }
                else { justPickedUp = false; }
                //TraptMain.ksold = ks;
            }

            //TODO: fix the location of the weapon object on every step, because it's an EnvironmentAgent
            ksold = ks;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            this.destination.X = (int)Math.Round(this.position.X);
            this.destination.Y = (int)Math.Round(this.position.Y);

            Vector2 origin = new Vector2(this.source.Width / 2, this.source.Height / 2);
            spriteBatch.Draw(this.texture, this.destination, this.source, Color.White,
                this.rotation, // The rotation of the Sprite.  0 = facing up, Pi/2 = facing right
                origin,
                SpriteEffects.None, 0);

            //recenter the hit box
            this.destination.X = (int)Math.Round(this.position.X - this.destination.Width / 2);
            this.destination.Y = (int)Math.Round(this.position.Y - this.destination.Height / 2);

        }
        #endregion
    }
}
