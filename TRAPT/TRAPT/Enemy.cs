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
    /// This is a game component that implemets IUpdateable.
    /// </summary>
    public class Enemy : Agent
    {
        #region Class Variables
        //const Vector2 NORTH = new Vector2(0, -1);
        //Values responsible for drawing

        enum wallSide { UP, DOWN, LEFT, RIGHT, UNKNOWN };

        Vector2 spriteCenter;
        //Texture2D texture;
        //Rectangle destination;
        //Rectangle source;
        //Vector2 pos = Vector2.Zero;
        Vector2 velocity = Vector2.Zero;
        Vector2 velocityCap = new Vector2(5, 5);
        //float rotation;


        AI_ViewCone viewCone;                               //The viewcone keeps track of the viewCone AND the cone for melee range detection
        Path path;                                          //Data structure for an Agent's pathNodes ( DIFFERENT THAN THE TUTORIAL'S )
        Path anotherway;
        //Vector2 playerPosition;                             //The position of the human player
        Boolean lineOfSight;                                //Flag for L o S
        public bool followPath = false;

        bool isStuck = false;

        Line playerLineOfSight;
        Texture2D pixelTexture;

        protected PathNode currentNode = new PathNode(0, 0, 0);
        PathNode goalNode = new PathNode(0, 0, 0);
        bool dwelling = false;
        float acceleration = 0.01f;

        enum AIstate { DWELLING, SEARCHING, ATTACKING, PATHING };   //Enumerated type for keeping track of AI States
        AIstate currentState;

        Boolean instantDeath = false;
        int HP = 100;                                               //Hit poitns

        //// sprite shape
        int spriteStartX = 0; // X of top left corner of sprite 0. 
        int spriteStartY = 0; // Y of top left corner of sprite 0.
        int spriteWidth = 64;
        int spriteHeight = 64;

        Vector2 prevPos;

        private TimeSpan dwellTimeSpan = TimeSpan.Zero;
        #endregion

        /** Bug 2 Variables  **/
        public bool useBug2 = true;
        public static List<WallTile> obstacles;

        public static UGraphList<Cell> locationTracker;

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game"></param>
        public Enemy(Game game)
            : base(game)
        {
            path = new Path();
        }
        #endregion

        #region Set Methods
        public void setPath(Path p)
        {
            this.path = p;
        }

        public void addPathNode(PathNode p)
        {
            this.path.AddLast(p);
        }
        #endregion

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public void Initialize()
        {
            this.DrawOrder = 400;

            this.health = 10;

            pixelTexture = Game.Content.Load<Texture2D>("Pixel");
            viewCone = new AI_ViewCone(this.Game);
            this.texture = Game.Content.Load<Texture2D>("guardAnimation_New");
            this.position = path.getCurrent().getPosition();

            this.prevPos = this.position;

            this.source = new Rectangle(this.spriteStartX, this.spriteStartY, this.spriteWidth, this.spriteHeight);
            this.destination = source;
            this.destination.X = (int)this.position.X;
            this.destination.Y = (int)this.position.Y;

            playerLineOfSight = new Line(this.position, TraptMain.player.Position);
            currentNode = path.getNext();
            currentState = AIstate.PATHING;

            Array values = Enum.GetValues(typeof(WeaponType));
            Random random = new Random();
            WeaponType rndType = (WeaponType)values.GetValue(random.Next(values.Length));
            Weapon randWpn = new Weapon(Game);
            randWpn.Initialize(this.position, 30, rndType);
            randWpn.PickUp(true, this);
            // Console.WriteLine(this.Weapon);
            anotherway = new Path();

            //animation
            this.aniLength = 2;
            this.aniRate = 333;
            this.frameWidth = 64;

            //Intialize Bug2
            //InitializeBug2(position);

            base.Initialize();
        }

        /* private void InitializeBug2(Vector2 position)
         {
             bug2.Initialize(64/4, new Vector2(position.X, position.Y));
         }*/

        //this needs to be implemented....
        private wallSide getWallSide(WallTile w, Vector2 enemyPosition)
        {
            Vector2 wallCenter = new Vector2((w.Position.X + (128 / 2)), (w.Position.Y + (128 / 2)));
            wallSide returnSide = wallSide.UNKNOWN;

            if (enemyPosition.X <= wallCenter.X)
            {
                returnSide = wallSide.RIGHT;
            }
            else if (enemyPosition.X >= wallCenter.X)
            {
                returnSide = wallSide.LEFT;
            }
            else if (enemyPosition.Y <= wallCenter.Y)
            {
                returnSide = wallSide.UP;
            }
            else if (enemyPosition.Y >= wallCenter.Y)
            {
                returnSide = wallSide.DOWN;
            }
            return returnSide;
        }


        public void lookingAround(float tempRotation)
        {
            float upper = tempRotation + ((float)Math.PI / 6);
            float lower = tempRotation - ((float)Math.PI / 6);

            if (this.rotation < upper)
            {
                this.rotation += ((float)Math.PI / 36);
            }
        }

        //Working on this, viewCone methods not yet implemented
        /*private bool CheckLOS()
        {
            foreach ( WallTile w in obstacles ){
                if(this.playerLineOfSight.intersects(w.Destination)){
                    return false;
                }
            }
            return true;
        }*/

        private void CheckViewCone()
        {

            //This code was an attempt for checking line of site of enemy and player in the viewcone
            /* if (viewCone. && LineOfSightBroken(this.playerLineOfSight))
             {
                 Console.WriteLine("Resetting aiState");
                 currentState = AIstate.PATHING;
                 //Console.WriteLine("CurrentState: " + currentState.ToString());
             }
             else
             {
                 //Need to check if player is stealthed
                 if (TraptMain.player.power == Power.Shroud)
                 {
                     currentState = AIstate.PATHING;
                 }

                 else if (!viewCone.intersectsViewCone(TraptMain.player.Destination))
                 {
                     currentState = AIstate.PATHING;
                 }
             }   */

            if (viewCone.intersectsViewCone(TraptMain.player.Destination) && TraptMain.player.power == Power.Shroud)
            {
                currentState = AIstate.PATHING;
            }
            else if (LineOfSightBroken(playerLineOfSight) && viewCone.intersectsViewCone(TraptMain.player.Destination))
            {
                currentState = AIstate.PATHING;
            }
            else if (!LineOfSightBroken(playerLineOfSight) && viewCone.intersectsViewCone(TraptMain.player.Destination))
            {
                currentState = AIstate.SEARCHING;
            }

            //Need to get all of the tiles along the line of the sprite to the character, check if any of them are walls, if they are.
            //Set line of sight to false
        }

        public bool LineOfSightBroken(Line LoS)
        {
            /* bool result = false;

             //foreach neighboring cell around the position of the enemy
             foreach (IVertex<Cell> neighbour in TraptMain.locationTracker.EnumerateNeighbours(this.checkin))
             {
                 foreach (GameComponentRef obstacle in neighbour.Data)  //foreach gamecomponent obstacle
                 {
                     if (obstacle.item is WallTile) //if obstacle is a walltile
                     {
                         if (LoS.intersects(obstacle.item.Destination)) //if it line of sight intersects a wall tile 
                         {
                             result = true;
                         }
                     }
                 }
             }
             return result;*/

            List<WallTile> obstacles = this.GetNearest8WallTiles();
            foreach (WallTile w in obstacles)
            {
                if (playerLineOfSight.intersects(w.Destination))
                {
                    return true;
                }
            }
            return false;
        }

        private bool PlayerInsideEnemyViewCone()
        {
            if (viewCone.intersectsViewCone(TraptMain.player.Destination))
                return true;
            else
                return false;

        }

        private void CheckMeleeCone()
        {
            if (viewCone.intersectsMeleeCone(TraptMain.player.Destination) && (TraptMain.player.meleeDelay > TimeSpan.Zero))
            {
                this.HurtEnemy(Int32.MaxValue);
            }
        }

        public void TraversePath(GameTime gameTime)
        {

           // Console.WriteLine(currentNode);

            if (isStuck)
            {
                if (anotherway.Count() == 0)
                {
                    anotherway = GraphToPath(
                        (AGraph<PathNode>)TraptMain.tileLayer.TransitionGrid.ShortestWeightedPath(
                        new PathNode((int)position.X / 128, (int)position.Y / 128, 0),
                        new PathNode((int)currentNode.position.X / 128, (int)currentNode.position.Y / 128, 0)));
                }
                PathNode temp = anotherway.First.Value;
                anotherway.RemoveFirst();
                goalNode = currentNode;
                currentNode = new PathNode((int)temp.position.X * 128 +64, (int)temp.position.Y * 128 +64, 0);
                isStuck = false;
            }


            velocity = Vector2.Zero;
            float tempRotation = 0;
            if (Vector2.Distance(this.position, currentNode.getPosition()) < 25)
            {
                this.dwellTimeSpan = TimeSpan.FromSeconds(currentNode.getDwell());
                tempRotation = this.rotation;                
               
                if (anotherway.Count() <= 0)
                {
                    goalNode = path.goNext();
                }
                else
                {
                    PathNode temp = anotherway.First.Value;
                    anotherway.RemoveFirst();
                    //goalNode = currentNode;
                    goalNode = new PathNode((int)temp.position.X * 128 + 64, (int)temp.position.Y * 128 + 64, 0);
                }
                currentNode = goalNode;
            }
           
            if (dwellTimeSpan >= TimeSpan.Zero)
            {
                dwelling = true;
                this.lookingAround(tempRotation);
                dwellTimeSpan -= gameTime.ElapsedGameTime;
            }
            else
            {
                //Console.WriteLine("CurrentNode.X: " + currentNode.getPosition().X + " CurrentNode.y: " + currentNode.getPosition().Y);
                dwelling = false;
                float dx = currentNode.getPosition().X - this.position.X;
                float dy = currentNode.getPosition().Y - this.position.Y;
                rotation = (float)(Math.Atan2(dy, dx) + Math.PI / 2);



                /*  BUG2 implementation */

                //TraptMain.tileLayer.TransitionGrid.


                //create a list of the nearest 8 cells surrounding the enemy


                if (isStuck)
                {
                    
                    //velocity.Y = 0;
                    //velocity.X = 0;
                    //if (anotherway.Count() == 0)
                    //{
                    //    anotherway = GraphToPath(
                    //        TraptMain.tileLayer.TransitionGrid.ShortestWeightedPath(
                    //        new PathNode((int)position.X / 128, (int)position.Y / 128, 0), new PathNode((int)currentNode.position.X / 128, (int)currentNode.position.Y / 128, 0)));
                    //}
                    //PathNode temp = anotherway.First.Value;
                    //anotherway.RemoveFirst();
                    //currentNode = new PathNode((int)temp.position.X*128, (int)temp.position.Y*128, 0);
                }
                else
                {
                    velocity.Y = (float)(acceleration + Math.Cos(rotation + Math.PI));
                    velocity.X = (float)(acceleration + Math.Sin(rotation));
                }
            }
        }

        private Path GraphToPath(AGraph<PathNode> graph)
        {
            Path result = new Path();
            foreach (IVertex<PathNode> node in graph.EnumerateVertices(new PathNode((int)position.X / 128, (int)position.Y / 128, 0)))
            {
                result.AddLast(node.Data);
                //foreach gamecomponent obstacle
                //foreach (GameComponentRef obstacle in neighbour.Data)
                //{
                //    //if it is a wall tile add it to the list 
                //    if (obstacle.item is WallTile)
                //    {
                //        //add it to the list
                //        temp.Add((WallTile)obstacle.item);
                //    }
                //}
            }
            return result;
        }

        /*This method will get the 8 surrounding wall tiles for the enemy.*/
        private List<WallTile> GetNearest8WallTiles()
        {
            //create a temporary list of wall tiles to return
            List<WallTile> temp = new List<WallTile>();

            //foreach neighboring cell around the position of the enemy
            foreach (IVertex<Cell> neighbour in TraptMain.locationTracker.EnumerateNeighbours(this.checkin))
            {
                //foreach gamecomponent obstacle
                foreach (GameComponentRef obstacle in neighbour.Data)
                {
                    //if it is a wall tile add it to the list 
                    if (obstacle.item is WallTile)
                    {
                        //add it to the list
                        temp.Add((WallTile)obstacle.item);
                    }
                }
            }
            return temp;
        }

        public void SearchForPlayer(int playerX, int playerY)
        {
            //int timeInCone = 0;
            currentNode = new PathNode(playerX, playerY, 0);

            float dx = currentNode.getPosition().X - this.position.X;
            float dy = currentNode.getPosition().Y - this.position.Y;
            rotation = (float)(Math.Atan2(dy, dx) + Math.PI / 2);

            velocity.Y = (float)(acceleration + Math.Cos(rotation + Math.PI));
            velocity.X = (float)(acceleration + Math.Sin(rotation));
        }

        public void HurtEnemy(int damage)
        {
            this.health -= damage;
            if (this.health <= 0)
            {
                this.Weapon.Drop();
                this.ClearLocationCheckin();
                this.isDead = true;
                this.Dispose(true);
            }
            if (this.currentState != AIstate.SEARCHING
                && this.currentState != AIstate.ATTACKING)
            {
                this.currentState = AIstate.SEARCHING;
            }
        }

        public override void Update(GameTime gameTime)
        {
            obstacles = this.GetNearest8WallTiles();
            playerLineOfSight = new Line(this.position, TraptMain.player.Position);

            //check how far away the enemy is from the alien sprite
            float distBetweenThem = Vector2.Distance(this.position, TraptMain.player.Position);


            //this.destination.X = (int)this.position.X;
            //this.destination.Y = (int)this.position.Y;

            this.prevPos = this.position;

            //CheckViewCones and Line of sight before desiding which state should be active.
            //Still have not implemented updating of sprite animation, that will need to be switched around
            if (currentState == AIstate.PATHING)
            {
                TraversePath(gameTime);
                /**
                 * We need to implement some local path planning here if not on the path... Not quite sure how to do it.*/
            }
            /*  if (currentState == AIstate.DWELLING)
              {
                  Console.WriteLine("In dwelling state(me)");
                  this.dwellCounter = 5;
                  this.dwelling(path.getPrevious().getDwell());
                  //ADD WHAT TO DO WHEN DWELLING
                  //this.dwelling(path.getPrevious().getDwell());
                  //go to node and stay for some number of seconds (looking around) and start pathing again
              }   */
            if (currentState == AIstate.SEARCHING)
            {
                /* ADD WHAT TO DO WHEN SEARCHING FOR THE PLAYER
                 Go to the players coordinates unless line of sight is broken in the next tick/heartbeat
                 if Line of sight is broken, dwell at the last known location for X amount of time.
                 Return to the last node on the path and continue pathing... 
                 To get to the original path we will need to implement some kind of bug algorithm as shown in the lectures. Or other variations
                 local path planning*/

                //check if player line of sight has been broken by a wall object
                //CheckLineOfSight(playerLineOfSight);

                this.SearchForPlayer((int)TraptMain.player.Position.X, (int)TraptMain.player.Position.Y);

                if (distBetweenThem < 400) currentState = AIstate.ATTACKING;
            }
            if (currentState == AIstate.ATTACKING)
            {
                CheckViewCone();
                if (this.Weapon.WpnType == WeaponType.Shotgun)
                {
                    if (distBetweenThem < 100)
                    {
                        this.velocity = Vector2.Zero;
                    }
                }
                else if (this.Weapon.WpnType == WeaponType.SMG)
                {
                    if (distBetweenThem < 300)
                    {
                        this.velocity = Vector2.Zero;
                    }
                }
                this.Weapon.Shoot();
            }

            //Resolve collision
            int hitCount = imHitting.Count();
            for (int i = hitCount - 1; 0 <= i; i--)
            {
                this.Collide(imHitting[i]);
                imHitting.RemoveAt(i);
            }

            this.position.X += velocity.X;
            this.position.Y += velocity.Y;

            spriteCenter = new Vector2((this.source.Width / 2), (this.source.Height / 2));
            viewCone.Update(gameTime, rotation, this.position);
            this.CheckViewCone();
            this.CheckMeleeCone();

            base.Update(gameTime);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            playerLineOfSight.Draw(spriteBatch, pixelTexture);
            viewCone.Draw(spriteBatch);

            //this.destination = this.texture.Bounds;
            this.destination.X = (int)this.position.X;// -this.source.Width / 2;
            this.destination.Y = (int)this.position.Y;// -this.source.Height / 2;
            //this.source = texture.Bounds;            

            spriteBatch.Draw(this.texture, this.destination, this.source, Color.White,
                this.Rotation, // The rotation of the Sprite.  0 = facing up, Pi/2 = facing right
                spriteCenter,
                SpriteEffects.None, this.Depth);

            //re-center hit box
            this.destination.X = (int)this.position.X - this.source.Width / 2;
            this.destination.Y = (int)this.position.Y - this.source.Height / 2;
        }


        public override void Collide(EnvironmentObj that)
        {
            if (that is WallTile)
            {
                obstacles = GetNearest8WallTiles();

                //if there are wall tiles in the list of obstacles
                if (obstacles.Count != 0)
                {
                    isStuck = true;
                    foreach (WallTile w in obstacles)
                    {
                        if (this.getWallSide(w, this.position) == wallSide.LEFT)
                        {
                            this.position.X += 5;
                        }
                        else if (this.getWallSide(w, this.position) == wallSide.RIGHT)
                        {
                            this.position.X -= 5;
                        }
                        else if (this.getWallSide(w, this.position) == wallSide.UP)
                        {
                            this.position.Y += 5;
                        }
                        else if (this.getWallSide(w, this.position) == wallSide.DOWN)
                        {
                            this.position.Y -= 5;
                        }
                    }
                }
            }
            else if (that is Player)
            {
                this.velocity.X = 0;
                this.velocity.Y = 0;
            }
        }

        #region ToString
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
        #endregion
    }
}
