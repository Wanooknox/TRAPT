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
    public enum Power
    {
        None,
        Shroud,
        Fortify,
    }

    public enum Barrier
    {
        None,
        Top,
        Bottom,
        Right, 
        Left,
        //TopRight,
        //TopLeft,
        //BottomRight,
        //BottomLeft,
    }

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Player : Agent//Microsoft.Xna.Framework.DrawableGameComponent
    {

        #region Variables
        // PHYSICS FIELDS
        private Vector2 prevPos, prevVel;
        //public Vector2 position;
        //public Vector2 velocity;
        //private float rotation;
        //private float direction;
        public bool isShooting;

        private Vector2 weapPos = new Vector2();
        public override Vector2 WeaponPosition
        {
            get
            {
                int offset = 25;
                weapPos.Y = (float)(offset * Math.Cos(this.rotation + Math.PI));
                weapPos.X = (float)(offset * Math.Sin(this.rotation));
                return new Vector2(this.position.X + weapPos.X, this.position.Y + weapPos.Y);
                //return this.position;
            }
        }

        //public override Vector2 WeaponOrigin
        //{ get { return new Vector2(-12,42); } }

        //float speed = 0f;
        private static float MAX_PLAYER_SPEED = 5f;
        private static float MIN_PLAYER_SPEED = 0f;

        //float friction = 0.25f;
        //float acceleration = 0.5f;

        //bool colliding = false;
        //EnvironmentObj collidingWith;
        public Power power = Power.None;
        private Barrier vBarrier = Barrier.None;
        private Barrier hBarrier = Barrier.None;

        //stats
        public int energy;
        TimeSpan healthDelay = TimeSpan.Zero;
        TimeSpan energyDelay = TimeSpan.Zero;
        TimeSpan healthRegenDelay = TimeSpan.Zero;
        TimeSpan energyRegenDelay = TimeSpan.Zero;
        

        // CONTROLS
        // Set to protected so that subclasses can modify them.
        protected Keys up = Keys.W;
        protected Keys down = Keys.S;
        protected Keys left = Keys.A;
        protected Keys right = Keys.D;
        // melee delay
        public TimeSpan meleeDelay = TimeSpan.Zero;
        //public bool doMelee = false;


        // DRAWING FIELDS
        Texture2D guideTex;
        SpriteFont font;
        //public Rectangle destination;
        //public Rectangle source;
        public Color color;
        
        // sprite shape
        int spriteStartX = 0; // X of top left corner of sprite 0. 
        int spriteStartY = 0; // Y of top left corner of sprite 0.
        int spriteWidth = 64;
        int spriteHeight = 64;
        #endregion

        /*Testing Bug2 for obstacles*/
        //public bool useBug2 = true;
        //public Bug2 bug2 = new Bug2();
        //public List<Obstacle> obstacles;

        public Player(Game game)
            : base(game)
        {
            //game.Components.Add(this);
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public virtual void Initialize(Vector2 position)
        {
            this.DrawOrder = 500;
            this.position = position;
            this.prevPos = position;
            this.rotation = 0;

            this.health = 100;
            this.energy = 100;

            //IntializeBug2(this.position);
            

            //calculate a random sprite color
            Random randonGen = new Random();
            this.color = Color.FromNonPremultiplied(randonGen.Next(255), randonGen.Next(255), randonGen.Next(255), 255);

            this.destination = new Rectangle((int)this.position.X - this.spriteWidth / 2, (int)this.position.Y - this.spriteHeight / 2,
                this.spriteWidth, this.spriteHeight);
            this.source = new Rectangle(this.spriteStartX, this.spriteStartY, this.spriteWidth, this.spriteHeight);

            this.texture = Game.Content.Load<Texture2D>(@"Characters\alienAnimation(New)");
            // font for printing debug info.
            this.font = Game.Content.Load<SpriteFont>("SpriteFont1");
            this.guideTex = Game.Content.Load<Texture2D>("tileguide");

            //animation
            this.aniStart = 0;
            this.aniLength = 9;
            this.aniRate = 167;
            this.frameWidth = 64;
            

            base.Initialize();
        }

        //private void InitializeBug2(Vector2 position)
        //{
        //    bug2.Initialize(this.destination.Height / 4, new Vector2(this.position.X, this.position.Y));
        //    //bug2.accerleration = Sumo.ACCELERATION_AMOUNT;
        //}

        /// <summary>
        /// 
        /// </summary>
        private void SpeedUp()
        {
            //speed = MAX_PLAYER_SPEED;
            // as long as we are below the maximum speed
            if (this.speed < MAX_PLAYER_SPEED)
            {
                //add acceleration to the speed and recalculate the velocity vector
                this.speed += this.acceleration;
            }
            //this.speed = MAX_PLAYER_SPEED;
        }
        
        // these are experimental methods that are not being used right now
        #region Not Used
        /// <summary>
        /// 
        /// </summary>
        private void SlowDown()
        {
            // as long as we are above the minimum speed
            if (this.speed > MIN_PLAYER_SPEED)
            {
                //subtract acceleration to the speed and recalculate the velocity vector
                this.speed -= this.acceleration;
                UpdateVelocity();
            }
        }

        /// <summary>
        /// re-calculate the velocity values based on the current speed and direction values.
        /// </summary>
        private void UpdateVelocity()
        {
            // do some fancy trig to find the right value for X and Y based onthe speed and direction
            this.velocity.Y = (float)(this.speed * Math.Cos(this.direction + Math.PI));
            this.velocity.X = (float)(this.speed * Math.Sin(this.direction));
        }

        /// <summary>
        /// re-calculate the direction angle from the current velocity and add a small offset
        /// </summary>
        private void UpdateDirection()
        {
            //calculate a new direction (in radians) based on the current velocity values
            this.direction = (float)Math.Atan2(this.velocity.X, -this.velocity.Y);
            //add a small ofset to differ the direction just a bit
            //this.direction += (float)((new Random()).NextDouble() * (Math.PI / 6));
        }

        private float MidAngle(float dir1, float dir2)
        {
            
            float finalDir;
            //if the difference in direction is less than 2 degrees
            if (Math.Abs(dir2 - dir1) < ((Math.PI * 2) / 360)
                || Math.Abs(dir1 - Math.PI) == dir2)
            {
                // enforce new direction
                finalDir = dir1;
            }
            else
            {
                //make sure dir2 is bigger
                if (dir1 > dir2)
                {
                    var temp = dir1;
                    dir1 = dir2;

                    dir2 = temp;
                }

                if (dir2 - dir1 > Math.PI)
                {
                    dir2 -= (float)(Math.PI * 2);
                }

                finalDir = (dir2 + dir1) / 2;
                if (finalDir < 0)
                {
                    finalDir += (float)(Math.PI * 2);
                }
            }

            //Console.WriteLine("" + finalDir);
            return finalDir;
        }

        private void MotionAdd(float dir, float speedAdd)
        {
            float midAngle;
            if (this.speed > MIN_PLAYER_SPEED)
            {
                midAngle = MidAngle(this.direction, dir);
            }
            else { midAngle = dir; }

            if (Math.Abs(this.velocity.Length()) < MAX_PLAYER_SPEED)
            {
                // do some fancy trig to find the right value for X and Y based on the speed and direction
                this.velocity.Y += (float)(speedAdd * Math.Cos(midAngle + Math.PI));
                this.velocity.X += (float)(speedAdd * Math.Sin(midAngle));

                this.speed += speedAdd;
                this.direction = midAngle;
            }
            else
            {
                this.speed = Math.Abs(this.velocity.Length());
                // do some fancy trig to find the right value for X and Y based on the speed and direction
                this.velocity.Y = (float)(this.speed * Math.Cos(midAngle + Math.PI));
                this.velocity.X = (float)(this.speed * Math.Sin(midAngle));

            }
        }
        #endregion

        private void LookToMouse()
        {
            Vector2 msInWorld = TraptMain.cursor.GetMouseInWorld();
            //calculate visual rotation angle to look toward the mouse position
            double delX = msInWorld.X - this.position.X;
            double delY = msInWorld.Y - this.position.Y;
            this.rotation = (float)(Math.Atan2(delY, delX) + (Math.PI / 2.0));
            //Console.WriteLine("Player angle change: " + delX + " " + delY);
        }
        #region HUD methods
        /// <summary>
        /// take of "damage" number of health points
        /// </summary>
        /// <param name="damage">how much health to lose.</param>
        public void HurtPlayer(int damage)
        {
            if (this.power == Power.Fortify)
            {
                damage = damage / 2;
            }
            this.health -= damage;
            this.healthRegenDelay = TimeSpan.FromMilliseconds(5000);
        }

        /// <summary>
        /// call to manage health: regen delay and regen
        /// </summary>
        private void ManageHealth(GameTime gameTime)
        {
            // less than full heath and no delay left before regen
            if (this.health < 100 && this.healthRegenDelay <= TimeSpan.Zero)
            {
                //if we are not yet regening
                //if (this.healthRegening == false)
                //{
                //    //set the delay BEFOR regen and go into regen mode
                //    this.healthRegenDelay = TimeSpan.FromMilliseconds(2000);
                //    healthRegening = true;
                //}
                //if health update delay < 0
                if (this.healthDelay <= TimeSpan.Zero)
                {
                    //increment health and set wait before next increment
                    this.health += 1;
                    this.healthDelay = TimeSpan.FromMilliseconds(25);
                }
            }
            //if full health
            if (this.health >= 100)
            {
                //stop over flow 
                this.health = 100;
            }
            //decrement delays and update hud value
            this.healthDelay -= gameTime.ElapsedGameTime;
            this.healthRegenDelay -= gameTime.ElapsedGameTime;
            TraptMain.hud.Health = this.health;
        }

        /// <summary>
        /// call to manage energy
        /// </summary>
        private void ManageEnergy(GameTime gameTime)
        {
            //if energy available
            if (this.energy > 0)
            {
                // and no delay to use energy
                if (this.energyDelay <= TimeSpan.Zero)
                {
                    //if in shroud
                    if (this.power == Power.Shroud)
                    {
                        this.energy -= 1;
                        this.energyDelay = TimeSpan.FromMilliseconds(25);
                        this.energyRegenDelay = TimeSpan.FromMilliseconds(3000);
                    }//else for fortify
                    else if (this.power == Power.Fortify)
                    {
                        this.energy -= 1;
                        this.energyDelay = TimeSpan.FromMilliseconds(100);
                        this.energyRegenDelay = TimeSpan.FromMilliseconds(3000);
                    }
                    else //else no power
                    {
                        //is no regen delay and less than full energy
                        if (this.energyRegenDelay <= TimeSpan.Zero && this.energy < 100)
                        {
                            this.energy += 1;
                            this.energyDelay = TimeSpan.FromMilliseconds(25);
                        }
                    }

                }
                //cap the energy max
                if (this.energy >= 100)
                {
                    this.energy = 100;
                }
            }
            else //no energy left
            {
                this.power = Power.None;
                if (this.energyRegenDelay <= TimeSpan.Zero && this.energy < 100)
                {
                    this.energy += 1;
                    this.energyDelay = TimeSpan.FromMilliseconds(25);
                }
            }
            //decrement delays
            this.energyDelay -= gameTime.ElapsedGameTime;
            this.energyRegenDelay -= gameTime.ElapsedGameTime;
            TraptMain.hud.Energy = this.energy;
        }

        private void ManagePowers()
        {
            if (!ks.IsKeyDown(Keys.Q) && ksold.IsKeyDown(Keys.Q)
                || !gps.IsButtonDown(Buttons.LeftShoulder) && gpsold.IsButtonDown(Buttons.LeftShoulder))
            {
                switch (this.power)
                {
                    case Power.None:
                        this.power = Power.Shroud;
                        break;
                    case Power.Fortify:
                        this.power = Power.Shroud;
                        break;
                    case Power.Shroud:
                        this.power = Power.None;
                        break;
                }

            }
            else if (!ks.IsKeyDown(Keys.E) && ksold.IsKeyDown(Keys.E)
                || !gps.IsButtonDown(Buttons.RightShoulder) && gpsold.IsButtonDown(Buttons.RightShoulder))
            {
                switch (this.power)
                {
                    case Power.None:
                        this.power = Power.Fortify;
                        break;
                    case Power.Shroud:
                        this.power = Power.Fortify;
                        break;
                    case Power.Fortify:
                        this.power = Power.None;
                        break;
                }
            }
        }

        private void ManageBarriers()
        {
            //barrier resolution  (Stops him from not to get caught in a wall)
            if (this.hBarrier == Barrier.Right)
            {
                if (this.velocity.X > 0)
                {
                    this.velocity.X = 0;
                    
                }
                else
                {
                    this.hBarrier = Barrier.None;
                }
            }
            else if (this.hBarrier == Barrier.Left)
            {

                if (this.velocity.X < 0)
                {
                    this.velocity.X = 0;
                }
                else
                {
                    this.hBarrier = Barrier.None;
                }
            }
            if (this.vBarrier == Barrier.Bottom)// && this.velocity.Y > 0)
            {
                //this.velocity.Y = 0; 
                if (this.velocity.Y > 0)
                {
                    this.velocity.Y = 0;
                }
                else
                {
                    this.vBarrier = Barrier.None;
                }
            }
            else if (this.vBarrier == Barrier.Top)// && this.velocity.Y < 0)
            {
                //this.velocity.Y = 0;
                if (this.velocity.Y < 0)
                {
                    this.velocity.Y = 0;
                }
                else
                {
                    this.vBarrier = Barrier.None;
                }
            }
        }
        #endregion
    
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            this.prevVel = Vector2.Zero + this.velocity;

            // Moving update:
            ks = Keyboard.GetState();
            ms = Mouse.GetState();
            gps = GamePad.GetState(PlayerIndex.One);
                    
            //Update sound circle
            this.soundCircle = new Circle(this.position, 500);

            // Move faster or slower.
            if (ks.IsKeyDown(this.up))
            {
                //increase the speed
                SpeedUp();
                //reverse on y axis at rate of speed
                this.velocity.Y = -1 * (float)(this.speed);
            }
            else if (ks.IsKeyDown(this.down))
            {
                //increase speed
                SpeedUp();
                //forward on y axis at rate of speed
                this.velocity.Y = (float)(this.speed);
            }
            if (ks.IsKeyDown(this.left))
            {
                //inccrease speed
                SpeedUp();
                //reverse on x axis at rate of speed
                this.velocity.X = -1 * (float)(this.speed);
            }
            else if (ks.IsKeyDown(this.right))
            {
                //increase speed
                SpeedUp();
                //forward on x axis at rate of speed
                this.velocity.X = (float)(this.speed);
            }


            if (TraptMain.useGamePad)
            {
                SpeedUp();
                this.velocity.X = gps.ThumbSticks.Left.X * (float)(this.speed);
                this.velocity.Y = -gps.ThumbSticks.Left.Y * this.speed;
            }

            this.ManagePowers();

            //TEMP HEALTH MODFIER
            //if (ks.IsKeyDown(Keys.T) && !ksold.IsKeyDown(Keys.T)) //key lifted
            //{
            //    this.HurtPlayer(10);
            //}

            //manage player health
            this.ManageHealth(gameTime);


            //Check if game over
            if (this.health <= 0)
            {
                if (this.Visible)
                {
                    this.velocity = Vector2.Zero;
                    if (this.Weapon != null)
                    {
                        this.Weapon.Dispose();
                    }
                    this.aniRow = 2;
                    this.aniLength = 1;
                    this.aniRate = 333;

                    this.isDead = true;
                    //make a deapth object
                    AgentDie zombie = new AgentDie(Game);
                    zombie.Initialize(this.texture, 3300, this.rotation, this.position, this.source, this.destination);
                    zombie.SetAnimationParams(this.aniStart, this.aniLength, this.aniRate, this.aniRow, this.frameWidth, this.frameHeight);

                    //get rid of the enemy object
                    //this.Weapon.Drop();
                    //this.ClearLocationCheckin();
                    //this.Dispose(true);
                    this.Visible = false;
                }
            }


            //update energy
            this.ManageEnergy(gameTime);

            this.LookToMouse();

            //if we have a gun and are clicking
            if (this.Weapon != null)
            {
                if (ms.LeftButton == ButtonState.Pressed || gps.Triggers.Right > 0)
                {
                    if (this.power == Power.Shroud)
                    {
                        this.power = Power.None;
                    }
                    this.Weapon.Shoot();
                    this.isShooting = true;
                }
                if( (ms.LeftButton == ButtonState.Released && !TraptMain.useGamePad) 
                    || (gps.Triggers.Right <= 0 && TraptMain.useGamePad))
                {
                    this.isShooting = false;
                }
                TraptMain.hud.Ammo = this.Weapon.Ammo;
            }
            else
            {
                TraptMain.hud.Ammo = 0;
            }

            //if not doing a melee and no wait time left
            if (this.meleeDelay <= TimeSpan.Zero)
                //if (!this.doMelee && this.meleeDelay <= TimeSpan.Zero)
            {
                this.aniRow = 0;
                this.aniStart = 0;
                this.aniLength = 9;
                if (this.Weapon != null) 
                    this.Weapon.Visible = true;
                if (ms.RightButton == ButtonState.Pressed && msold.RightButton == ButtonState.Released //button just lifted
                    || gps.Triggers.Left > 0 && !(gpsold.Triggers.Left > 0))
                {
                    //set a melee time to indicate performing a melee
                    this.meleeDelay = TimeSpan.FromMilliseconds(300);
                    this.aniRow = 1;
                    this.aniStart = 0;
                    this.aniLength = 0;
                    this.frameCount = 0;
                    if (this.Weapon != null) 
                        this.Weapon.Visible = false;
                }
            }
            else 
            {
                //decrement the melee delay
                this.meleeDelay -= gameTime.ElapsedGameTime;
            }

            //Change sripte animation when we have a gun
            if (this.Weapon != null)
            {
                this.aniRow = 1;
                this.aniLength = 9;
            }
            

            //////////////////////////////

            //Vector2 temp = Vector2.Transform(this.position, Matrix.Invert(((TraptMain)Game).camera.GetViewMatrix(new Vector2(0))));
            ////this.camera.Position = new Vector2(ms.X - temp.X, ms.Y - temp.Y);

            
            //double delX = ms.X - temp.X;//this.position.X;
            //double delY = ms.Y - temp.Y;//this.position.Y;
            //this.rotation = (float)(Math.Atan2(delY, delX) + (Math.PI / 2.0));
            //Console.WriteLine(delX + " " + delY);

            ////////////////////////
            
            // Include friction.
            // If our velocity (scalar magnitude of a vector = length of a vector) is greater than the effect of friction,
            // then friction should be applied in the opposite direction of the velocity.  
            if (Math.Abs(this.velocity.Length()) > this.friction)
            {
                this.velocity.X -= Math.Sign(this.velocity.X) * this.friction; // Whatever sign velocity is, 
                this.velocity.Y -= Math.Sign(this.velocity.Y) * this.friction; // apply friction in the opposite direction.
                //reduce the symbolic rate of speed along with velocity
                this.speed -= this.friction;
            }
            else
            { // If our velocity is closer to zero than the effect of friction, we should just stop. 
                this.velocity.X = MIN_PLAYER_SPEED;
                this.velocity.Y = MIN_PLAYER_SPEED;
                this.speed = MIN_PLAYER_SPEED;
            } 

            // stop at edge of screen  
            //if (this.position.X + this.velocity.X < this.spriteWidth
            //    || Game.GraphicsDevice.Viewport.Width < this.position.X + this.velocity.X)
            //{
            //    //halt the x velocity 
            //    this.velocity.X = 0;
            //}
            //if (this.position.Y + this.velocity.Y < this.spriteHeight
            //    || Game.GraphicsDevice.Viewport.Height < this.position.Y + this.velocity.Y)
            //{
            //    //halt y velocity
            //    this.velocity.Y = 0;
            //}

            //Enforce collision resolution
            //if (colliding)
            //{
            //    EnforceCollide(collidingWith);
            //}

            //if (!colliding)
            //{
            //    // Apply the velocity to the position.  
            //    this.position.Y += this.velocity.Y;
            //    this.position.X += this.velocity.X;
            //}
            //else 
            //{ 
            //    colliding = false;
            //    for (int i = 0; i < imHitting.Count(); i++)
            //    {
            //        this.Collide(imHitting[i]);
            //        imHitting.RemoveAt(i);
            //    }
            //}


            int hitCount = imHitting.Count();
            for (int i = hitCount - 1; 0 <= i; i--)
            {
                this.Collide(imHitting[i]);
                imHitting.RemoveAt(i);
            }

            //barrier resolution  (Stops him from not to get caught in a wall)
            ManageBarriers();


            // Apply the velocity to the position.
            this.prevPos = Vector2.Zero + this.position;
            this.position.Y += this.velocity.Y;
            this.position.X += this.velocity.X;

            //int hitCount = imHitting.Count();
            //for (int i = hitCount - 1; 0 <= i; i--)
            //{
            //    this.Collide(imHitting[i]);
            //    imHitting.RemoveAt(i);
            //}

            //this.hBarrier = Barrier.None;
            //this.vBarrier = Barrier.None;

            
            ksold = ks;
            msold = ms;
            gpsold = gps;

            base.Update(gameTime);
        }

        public void DrawShroud(SpriteBatch spriteBatch)
        {
            this.color = Color.Chartreuse;
            Random rand = new Random();
            this.color.A = (byte)rand.Next(50);

            //fray left
            this.destination.X = (int)Math.Round(this.position.X-rand.Next(3));
            this.destination.Y = (int)Math.Round(this.position.Y-rand.Next(3));

            // Draw the player's texture.  
            // The origin is the point inside the source rectangle to rotate around.
            Vector2 origin = new Vector2(this.source.Width / 2, this.source.Height / 2);
            spriteBatch.Draw(this.texture, this.destination, this.source, this.color,
                this.Rotation, // The rotation of the Sprite.  0 = facing up, Pi/2 = facing right
                origin,
                SpriteEffects.None, 0);

            //fray right
            this.destination.X = (int)Math.Round(this.position.X+rand.Next(3));
            this.destination.Y = (int)Math.Round(this.position.Y+rand.Next(3));

            // Draw the player's texture.  
            spriteBatch.Draw(this.texture, this.destination, this.source, this.color,
                this.Rotation, // The rotation of the Sprite.  0 = facing up, Pi/2 = facing right
                origin,
                SpriteEffects.None, 0);
        }

        public void DrawFortify(SpriteBatch spriteBatch)
        {
            //this.color = Color.;
            this.color.R = 50;
            this.color.G = 50;
            this.color.B = 50;
            Random rand = new Random();
            this.color.A = (byte)rand.Next(100,200);

            //fray left
            this.destination.X = (int)Math.Round(this.position.X+rand.Next(3));
            this.destination.Y = (int)Math.Round(this.position.Y+rand.Next(3));

            // Draw the player's texture.  
            // The origin is the point inside the source rectangle to rotate around.
            Vector2 origin = new Vector2(this.source.Width / 2, this.source.Height / 2);
            spriteBatch.Draw(this.texture, this.destination, this.source, this.color,
                this.Rotation, // The rotation of the Sprite.  0 = facing up, Pi/2 = facing right
                origin,
                SpriteEffects.None, 0);

            //fray right
            this.destination.X = (int)Math.Round(this.position.X + rand.Next(3));
            this.destination.Y = (int)Math.Round(this.position.Y + rand.Next(3));

            // Draw the player's texture.  
            spriteBatch.Draw(this.texture, this.destination, this.source, this.color,
                this.Rotation, // The rotation of the Sprite.  0 = facing up, Pi/2 = facing right
                origin,
                SpriteEffects.None, 0);
        }

        public void DrawNormal(SpriteBatch spriteBatch)
        {
            this.color = Color.Chartreuse;
            this.color.A = 255;

            this.destination.X = (int)Math.Round(this.position.X);
            this.destination.Y = (int)Math.Round(this.position.Y);

            // Draw the player's texture.  
            // The origin is the point inside the source rectangle to rotate around.
            Vector2 origin = new Vector2(this.source.Width / 2, this.source.Height / 2);
            spriteBatch.Draw(this.texture, this.destination, this.source, this.color,
                this.Rotation, // The rotation of the Sprite.  0 = facing up, Pi/2 = facing right
                origin,
                SpriteEffects.None, this.Depth);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //this.FPS = (gt.ElapsedGameTime.Milliseconds / 1000.0) * 100 *60;

            ////waste time
            //int n = 10;
            //for (int i = 0; i < n; i++)
            //{
            //    for (int j = 0; j < n; j++)
            //    {
            //        //do nothing
            //        Console.WriteLine("Waste Some Time " + i + " " + j);
            //    }
            //}

            // Basic destination rectangle updating from last time. 
            //this.DrawNormal(spriteBatch);
            //this.DrawShroud(spriteBatch);
            //this.DrawFortify(spriteBatch);

            switch (this.power)
            {
                case Power.None:
                    this.DrawNormal(spriteBatch);
                    break;
                case Power.Shroud:
                    this.DrawShroud(spriteBatch);
                    break;
                case Power.Fortify:
                    this.DrawFortify(spriteBatch);
                    break;
            }

            //TODO: Orgin is bad
            this.destination.X = (int)Math.Round(this.position.X - this.destination.Width / 2);
            this.destination.Y = (int)Math.Round(this.position.Y - this.destination.Height / 2);

            //reference for where the hitbox is.
            //spriteBatch.Draw(this.guideTex, this.Destination, Color.White);

            String debug = "Destination: " + this.Destination//this.direction * (180.0/Math.PI)
                + "\nVelocity: " + this.velocity
                + "\nSpeed: " + this.speed
                + "\nRotation: " + this.Rotation//.X + " " + this.Position.Y
                + "\nStuff: " + Game.Components.Count;
//                + "\nFPS: " + this.FPS;

            //spriteBatch.DrawString(this.font, debug, new Vector2(0), Color.White);
        }

        /// <summary>
        /// clear out the player object and all connected items
        /// </summary>
        public void Destroy()
        {
            //if have a weapon dispose of it
            if (this.Weapon != null)
            {
                this.Weapon.Dispose();
            }
            //dispose self
            this.Dispose();
        }

        public override bool IsColliding(EnvironmentObj that)
        {
            //Rectangle collidingBox = new Rectangle(this.destination.X - (this.destination.Width / 2), this.destination.Y - (this.destination.Height / 2), this.destination.Width * 2, this.destination.Y * 2);
            Rectangle collidingBox = this.destination;//.Inflate(32, 32);
            collidingBox.Inflate(32, 32);
            return collidingBox.Intersects(that.Destination);

           /* Rectangle temp = this.destination;

            temp.X = (int)Math.Round(this.position.X + this.velocity.X - this.destination.Width / 2);
            temp.Y = (int)Math.Round(this.position.Y + this.velocity.Y - this.destination.Height / 2);

            return temp.Intersects(that.Destination);*/

            //return this.destination.Intersects(that.Destination);
            //return true;
        }

        public override void Collide(EnvironmentObj that)
        {
            //this.destination.X -= (int)(Math.Sign(this.velocity.X) * Math.Abs(this.velocity.X));//(int)Math.Round(this.velocity.X);
            //this.destination.Y -= (int)Math.Round(this.velocity.Y);

            //Math.Sign(this.velocity.X) * this.friction;



            //this.position.X -= (int)(Math.Sign(this.velocity.X) * Math.Ceiling(Math.Abs(this.velocity.X)));
            //this.position.Y -= (int)(Math.Sign(this.velocity.Y) * Math.Ceiling(Math.Abs(this.velocity.Y)));

            //this.destination.X = (int)(Math.Sign(this.velocity.X) * Math.Abs(Math.Round(this.position.X - this.destination.Width / 2)));
            //this.destination.Y = (int)(Math.Sign(this.velocity.Y) * Math.Abs(Math.Round(this.position.Y - this.destination.Height / 2)));



            //// change the velocites
            //Vector2 swapV = new Vector2(that.velocity.X, that.velocity.Y);
            //that.velocity.X = this.velocity.X * -1;
            //that.velocity.Y = this.velocity.Y * -1;
            //this.velocity.X = swapV.X;
            //this.velocity.Y = swapV.Y;
            ////and input new motion
            //this.UpdateDirection();
            //that.UpdateDirection();

            //TODO: work on player object's collision resolution
            if (that is WallTile ||(that is Obstacle) && ((Obstacle)that).SwStatus)
            {
                //colliding = true;
                //throw new ApplicationException("hit wall!");

                //if (this.velocity.X > 0 || this.velocity.X < 0) //came from left
                //{
                //    this.position.X = this.prevPos.X; // -this.velocity.X;
                //    //this.velocity.X = 0;
                //}
                //if (this.velocity.Y > 0 || this.velocity.Y < 0)
                //{
                //    this.position.Y = this.prevPos.Y;// -this.velocity.Y;
                //    //this.velocity.Y = 0;
                //}

                //previous destionation rectangle on the y asix
                Rectangle prevDest = this.destination;
                prevDest.Y = (int)Math.Round(this.prevPos.Y - this.destination.Height / 2);

                //if (this.destination.Right > that.Destination.Left && this.destination.Right < that.Destination.Center.X) // object came from the left
                if (this.velocity.X > 0 && prevDest.Intersects(that.Destination)) // object came from the left
                {
                    //if position tracked by center of sprite, move position to be wall left - half my width
                    //this.position.X = that.Destination.Left - (this.destination.Width / 2)-1;
                    //if position tracked by top left corner, move position to be wall left - my width
                    //this.position.X = that.Destination.Left - (this.destination.Width) - 1;

                    //Kinda good
                    //this.position.X = prevPos.X;// -Math.Abs(prevVel.X);
                    this.position.X = prevPos.X - Math.Abs(velocity.X);

                    //barrier on the right side of me
                    this.hBarrier = Barrier.Right;

                    //this.velocity.X = 0;
                    //this.position.X = prevPos.X;
                }
                //else if (this.destination.Left < that.Destination.Right && this.destination.Left > that.Destination.Center.X) // object came from the right
                else if (this.velocity.X < 0 && prevDest.Intersects(that.Destination)) // object came from the right
                {
                    //this.position.X = that.Destination.Right + (this.destination.Width / 2) + 1;

                    //Kinda good
                    //this.position.X = prevPos.X;// +Math.Abs(prevVel.X);
                    this.position.X = prevPos.X +Math.Abs(velocity.X);

                    //barrier on the left side of me
                    this.hBarrier = Barrier.Left;

                    //this.velocity.X = 0;
                    //this.position.X = prevPos.X;                   
                }
                
                //previous destionation rectangle on the x asxis
                prevDest = this.destination;
                prevDest.X = (int)Math.Round(this.prevPos.X - this.destination.Width / 2);

                //if (this.destination.Bottom > that.Destination.Top && this.destination.Bottom < that.Destination.Center.Y) // object came from the top
                if (this.velocity.Y > 0 && prevDest.Intersects(that.Destination)) // object came from the top
                {
                    //this.position.Y = that.Destination.Top - (this.destination.Height/2)-1;

                    //Kinda good
                    //this.position.Y = prevPos.Y;// -Math.Abs(prevVel.Y);
                    this.position.Y = prevPos.Y - Math.Abs(velocity.Y);

                    //barrier on the bottom side of me
                    this.vBarrier = Barrier.Bottom;
                }
                //else if (this.destination.Top < that.Destination.Bottom && this.destination.Top > that.Destination.Center.Y) // object came from the top
                else if (this.velocity.Y < 0 && prevDest.Intersects(that.Destination)) // object came from the bottom
                {
                    //this.position.Y = that.Destination.Bottom + (this.destination.Height / 2) + 1;

                    //Kinda good
                    //this.position.Y = prevPos.Y;// +Math.Abs(prevVel.Y);
                    this.position.Y = prevPos.Y + Math.Abs(velocity.Y);

                    //barrier on the top side of me
                    this.vBarrier = Barrier.Top;
                }
                

                ////if horizontal collision
                //if (this.Destination.Left <= that.Destination.Right || this.Destination.Right >= that.Destination.Left)
                ////  if  (this.position.X + this.velocity.X < this.spriteWidth
                ////|| that.Destination.Width < this.position.X + this.velocity.X)
                //{
                //    //this.position.X -= this.Destination.Left - that.Destination.Right;
                //    this.velocity.X = 0;
                //}
                //if (this.Destination.Top <= that.Destination.Bottom || this.Destination.Bottom >= that.Destination.Top)
                //{
                //    //this.position.Y -= this.Destination.Bottom - that.Destination.Top;
                //    this.velocity.Y = 0;
                //}
                 
                
            }
            else if (that is Weapon)
            {
                if (((Weapon)that).Owner == null && this.Weapon == null)
                //if (TraptMain.ks.IsKeyDown(Keys.R) && !TraptMain.ksold.IsKeyDown(Keys.R))
                {
                    TraptMain.hud.ContextTip = ((Weapon)that).TipString();
                    //KeyboardState ks = Keyboard.GetState();
                    //if (((Weapon)that).Owner == null && this.Weapon == null)
                    if (!ks.IsKeyDown(Keys.F) && ksold.IsKeyDown(Keys.F)
                        || !gps.IsButtonDown(Buttons.Y) && gpsold.IsButtonDown(Buttons.Y))
                    {
                        ((Weapon)that).PickUp(true, this);
                        //this.Weapon = (Weapon)that;
                        Console.WriteLine("R key PRESSED");
                        //((Weapon)that).Owner = this;
                        //((Weapon)that).GetSprite();
                        //throw new ApplicationException("FUICK");
                    }
                    //TraptMain.ksold = ks;
                }
            }
            

        }
               
    }
}
