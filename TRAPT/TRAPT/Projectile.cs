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
    public class Projectile : Agent//Microsoft.Xna.Framework.GameComponent
    {
        private Agent owner;

        private WeaponType projectileType;
        private Random strayRandomizer;

        private SoundEffect shotSound;

        private Texture2D collider;
        private Rectangle collidingBox;

        //projectile lifespan
        private float life;
        private float age = 0;
        
        public Projectile(Game game)
            : base(game)
        {
            TraptMain.layers[1].Add(this);
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public virtual void Initialize(Agent owner, Vector2 position, float speed, float direction, WeaponType projectileType, ref Random strayRandomizer)
        {
            this.DrawOrder = 250;

            this.owner = owner;

            //image holding all the bullets
            this.texture = Game.Content.Load<Texture2D>("projectiles");
            this.collider = Game.Content.Load<Texture2D>("bulletcollider");


            //angle the bullet flies in.
            this.rotation = direction;
            this.direction = direction;

            //location and speed of the bullet
            this.position = position;

            //this.position.Y += (float)(42 * Math.Cos(this.rotation + Math.PI));
            //this.position.X += (float)(-12 * Math.Sin(this.rotation));
            //return new Vector2(this.position.X + weapPos.X, this.position.Y + weapPos.Y);

            this.speed = (float)(speed + strayRandomizer.NextDouble());

            this.projectileType = projectileType;
            this.strayRandomizer = strayRandomizer;

            GetSprite();
            CalculateVelocity();

            //determine projectile life and sound
            switch (this.projectileType)
            {
                case WeaponType.SMG:
                    this.life = 700;//1920;
                    this.shotSound = Game.Content.Load<SoundEffect>(@"Sound\SMG");
                    break;
                case WeaponType.Shotgun:
                    this.life = 500;//360;
                    this.shotSound = Game.Content.Load<SoundEffect>(@"Sound\shotgun");
                    break;
                default:
                    this.life = 10;
                    break;
            }
            this.shotSound.Play(0.4f,0.0f,0.0f);

            base.Initialize();
        }

        /// <summary>
        /// re-calculate the velocity values based on the current speed and direction values.
        /// </summary>
        private void CalculateVelocity()
        {
            double strayRange;
            switch (this.projectileType)
            {
                case WeaponType.SMG:
                    strayRange = (Math.PI / 36);
                    break;
                case WeaponType.Shotgun:
                    strayRange = (Math.PI / 18);
                    break;
                default:
                    strayRange = (Math.PI / 360);
                    break;
            }

            //generate a random "stray" for the bullet
            //strayRandomizer = new Random();
            float stray = (float)(strayRandomizer.NextDouble() * strayRange);
            //if random int is 0, positive, else stray is negative
            stray = (strayRandomizer.Next(2) == 0) ? stray : -1 * stray;

            //alter the trajectory by +/- stray.
            this.rotation += stray;
            this.direction += stray;
            this.speed += stray;

            // do some fancy trig to find the right value for X and Y based onthe speed and direction
            this.velocity.Y = (float)(this.speed * Math.Cos(this.direction + Math.PI));
            this.velocity.X = (float)(this.speed * Math.Sin(this.direction));
        }

        /// <summary>
        /// used the projectileType, and owner to determine what sprite to use
        /// </summary>
        public void GetSprite()
        {
            //if a rifle shot.
            if (this.projectileType == WeaponType.SMG)
            {
                //load the one floor view of the rifle
                this.source = new Rectangle(0, 0, 17, 48);
                this.destination = new Rectangle(0, 0, 17, 48);
            }
            else if (this.projectileType == WeaponType.Shotgun)
            {
                //TODO: adjust values for the shotgun shell.
                //load the one floor view of the rifle
                this.source = new Rectangle(0, 0, 17, 48);
                this.destination = new Rectangle(0, 0, 17, 48);
            }
          
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            this.position.X += this.velocity.X;
            this.position.Y += this.velocity.Y;

            //deal with collisions
            int hitCount = imHitting.Count();
            for (int i = hitCount - 1; 0 <= i; i--)
            {
                this.Collide(imHitting[i]);
                imHitting.RemoveAt(i);
            }

            //deal with lifespan
            if (this.age <= this.life) //if still young
            {
                this.age += this.velocity.Length(); //age
            }
            else //else projectile should die
            {
                this.Dispose();
            }

            //if not in the game world.
            if (!((TraptMain)Game).IsInWorld(this.position))
            {
                this.Dispose(true);
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.destination.X = (int)Math.Round(this.position.X);
            this.destination.Y = (int)Math.Round(this.position.Y);

            Vector2 origin = new Vector2(this.source.Width / 2, this.source.Height / 2);
            spriteBatch.Draw(this.texture, this.destination, this.source, Color.White,
                this.rotation, // The rotation of the Sprite.  0 = facing up, Pi/2 = facing right
                //this.owner.WeaponOrigin,
                origin,
                SpriteEffects.None, this.Depth);

            //spriteBatch.Draw(this.collider, this.destination, this.source, Color.White,
            //    this.rotation, // The rotation of the Sprite.  0 = facing up, Pi/2 = facing right
            //    this.owner.WeaponOrigin,
            //    SpriteEffects.None, this.Depth);

            //Line hbleft = new Line(new Vector2(collidingBox.Left, collidingBox.Top), new Vector2(collidingBox.Left, collidingBox.Bottom));
            //hbleft.Draw(spriteBatch, pixelTexture);
            //Line hbtop = new Line(new Vector2(collidingBox.Left, collidingBox.Top), new Vector2(collidingBox.Right, collidingBox.Top));
            //hbtop.Draw(spriteBatch, pixelTexture);
        }

        public override bool IsColliding(EnvironmentObj that)
        {
            collidingBox = this.destination;//.Inflate(32, 32);
            //collidingBox.Inflate(-8, -24);
            collidingBox.Width = 1;
            collidingBox.Height = 1;

            //Vector2 pt = new Vector2(collidingBox.X, collidingBox.Y);
            //pt = Vector2.Transform(pt,
            //    Matrix.Invert(
            //    Matrix.CreateRotationX(this.rotation)
            //    )
            //    );
            //collidingBox.X = (int)pt.X;
            //collidingBox.Y = (int)pt.Y;
            return collidingBox.Intersects(that.Destination);


            //pt = Vector2.Transform(pt,
            //    Matrix.CreateTranslation(new Vector3(this.owner.WeaponOrigin, 0))
            //    );

            //Vector2.Transform(new Vector2(collidingBox.X, collidingBox.Y),
            //    Matrix.Invert(
            //    Matrix.CreateRotationZ(this.rotation)
            //    )
            //    );
        }

        public override void Collide(EnvironmentObj that)
        {
            //TODO: work on player object's collision resolution
            if (that is WallTile)
            {
                this.Dispose(true);
            }
            else if (that is Player && !(owner is Player))
            {
                ((Player)that).HurtPlayer(2);
                this.Dispose(true);
            }
            else if (that is Enemy && !(owner is Enemy))
            {
                ((Enemy)that).HurtEnemy(2);
                this.Dispose(true);
            }
        }
    }
}
