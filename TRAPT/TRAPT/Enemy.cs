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
    public class Enemy : Agent 
    {
        //const Vector2 NORTH = new Vector2(0, -1);
        //Values responsible for drawing

        Vector2 spriteCenter;
        Texture2D texture;
        Rectangle destination;
        Rectangle source;
        //Vector2 pos = Vector2.Zero;
        Vector2 velocity = Vector2.Zero;
        Vector2 velocityCap = new Vector2(5, 5);
        //float rotation;

        AI_ViewCone viewCone;                               //The viewcone keeps track of the viewCone AND the cone for melee range detection
        Path path;                                          //Data structure for an Agent's pathNodes ( DIFFERENT THAN THE TUTORIAL'S )
        Vector2 playerPosition;                             //The position of the human player
        Boolean lineOfSight;                                //Flag for L o S

        Line playerLineOfSight;
        Texture2D pixelTexture;

        private PathNode currentNode = new PathNode(0, 0, 0);
        private int dwellCounter;                           //For keeping track of the time the AI has spent at a node

        float acceleration = 0.01f;

        enum AIstate { DWELLING, SEARCHING, ATTACKING, PATHING };   //Enumerated type for keeping track of AI States
        AIstate currentState;

        Boolean instantDeath = false;
        int HP = 100;                                               //Hit poitns
        

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game"></param>
        public Enemy(Game game)
            : base(game)
        {
            path = new Path();
        }

        /// <summary>
        /// ToString to check the Nodes of a specific agent
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            String temp = "Agent's Nodes: \n";
            foreach (PathNode p in path)
            {
                temp += p.ToString() + "\n";
            }
            return temp;
        }

        public void setPath(Path p)
        {
            this.path = p;
        }

        public void addPathNode(PathNode p)
        {
            this.path.AddLast(p);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public void Initialize()
        {
            this.DrawOrder = 400;

            pixelTexture = Game.Content.Load<Texture2D>("Pixel");
            viewCone = new AI_ViewCone(this.Game);
            texture = Game.Content.Load<Texture2D>("blueGuard");
            this.position = path.getCurrent().getPosition();
            playerLineOfSight = new Line(this.position, TraptMain.player.Position);
            currentNode = path.getNext();
            dwellCounter = 0;
            currentState = AIstate.PATHING;

            Array values = Enum.GetValues(typeof(WeaponType));
            Random random = new Random();
            WeaponType rndType = (WeaponType)values.GetValue(random.Next(values.Length));
            Weapon randWpn = new Weapon(Game);
            randWpn.Initialize(this.position, 30, rndType);
            randWpn.PickUp(true, this);
            Console.WriteLine(this.Weapon);
            

            base.Initialize();
        }

        //Working on this, viewCone methods not yet implemented
        
        private void checkViewCone()
        {
           // if(this.inLineOfSight() && intersectsViewCone(TraptMain.player.Destination))
            //Need to check if player is stealthed
            if ( viewCone.intersectsViewCone(TraptMain.player.Destination, TraptMain.player.Position))
            {
                currentState = AIstate.SEARCHING;
            }

            if (!viewCone.intersectsViewCone(TraptMain.player.Destination, TraptMain.player.Position))
            {
                currentState = AIstate.PATHING;
            }
      
            //Need to get all of the tiles along the line of the sprite to the character, check if any of them are walls, if they are.
            //Set line of sight to false
        }

        public void traversePath(GameTime gameTime)
        {
           double currentTime = gameTime.TotalGameTime.Seconds;
           velocity = Vector2.Zero;
           if (Vector2.Distance(this.position, currentNode.getPosition()) < 25)
           {
               dwellCounter = 0;
               currentNode = path.goNext();
           }
           
           float dx = currentNode.getPosition().X - this.position.X;
           float dy = currentNode.getPosition().Y - this.position.Y;
           rotation = (float)(Math.Atan2(dy, dx) + Math.PI / 2);

           velocity.Y = (float)(acceleration + Math.Cos(rotation + Math.PI));
           velocity.X = (float)(acceleration + Math.Sin(rotation));
          
        }

        public void searchForPlayer(int playerX, int playerY)
        {
            int timeInCone = 0;
            currentNode = new PathNode(playerX, playerY, 0);

            float dx = currentNode.getPosition().X - this.position.X;
            float dy = currentNode.getPosition().Y - this.position.Y;
            rotation = (float)(Math.Atan2(dy, dx) + Math.PI / 2);

            velocity.Y = (float)(acceleration + Math.Cos(rotation + Math.PI));
            velocity.X = (float)(acceleration + Math.Sin(rotation));

            

        }

        public void dwelling(int dwellHeartBeat)
        {
            if (dwellCounter <= dwellHeartBeat)
            {
                dwellCounter++;
            }
            if (dwellCounter > dwellHeartBeat)
            {
                dwellCounter = 0;
                currentState = AIstate.PATHING;
            }
        }

        public void firingWeapon()
        {
            if(Vector2.Distance(this.position, TraptMain.player.Position) < 250)
            {
                this.Weapon.Shoot();
            }

        }

        public override void Update(GameTime gameTime)
        {
            playerLineOfSight = new Line(this.position, TraptMain.player.Position);
           

            //checkLineOfSight();
            //CheckViewCones and Line of sight before desiding which state should be active.
            //Still have not implemented updating of sprite animation, that will need to be switched around
            if (currentState == AIstate.PATHING)
            {
                traversePath(gameTime);
                /**
                 * We need to implement some local path planning here if not on the path... Not quite sure how to do it.*/
                   
            }
            if (currentState == AIstate.DWELLING)
            {
                //ADD WHAT TO DO WHEN DWELLING
                //this.dwelling(path.getPrevious().getDwell());
            }
            if (currentState == AIstate.ATTACKING)
            {
                //this.firingWeapon();
                //ADD WHAT TO DO WHEN ATTACKING
                //Set a range in which the AI should fire it's weapon
            }
            if (currentState == AIstate.SEARCHING)
            {
                this.searchForPlayer((int) TraptMain.player.Position.X, (int)TraptMain.player.Position.Y);
             /* ADD WHAT TO DO WHEN SEARCHING FOR THE PLAYER
                Go to the players coordinates unless line of sight is broken in the next tick/heartbeat
                if Line of sight is broken, dwell at the last known location for X amount of time.
                Return to the last node on the path and continue pathing... 
                To get to the original path we will need to implement some kind of bug algorithm as shown in the lectures. Or other variations
                local path planning*/
            }

            this.position.X += velocity.X;
            this.position.Y += velocity.Y;
            
            spriteCenter = new Vector2((this.texture.Width / 2),(this.texture.Height / 2));
            viewCone.Update(gameTime, rotation, this.position);
            this.checkViewCone();
            Console.WriteLine(currentState);
            //this.Weapon.Shoot();
            
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            playerLineOfSight.Draw(spriteBatch, pixelTexture);
            viewCone.Draw(spriteBatch);
            //spriteBatch.Draw(texture, this.position, null, Color.White, (float)rotation, spriteCenter, 1, SpriteEffects.None, 0.0f);

            this.destination = this.texture.Bounds;
            this.destination.X = (int)this.position.X;
            this.destination.Y = (int)this.position.Y;
            this.source = this.texture.Bounds;
            spriteBatch.Draw(this.texture, this.destination, this.source, Color.White, 
                this.Rotation, // The rotation of the Sprite.  0 = facing up, Pi/2 = facing right
                spriteCenter,
                SpriteEffects.None, this.Depth);
        }
    }
}
