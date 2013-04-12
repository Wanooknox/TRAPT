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


        protected AI_ViewCone viewCone;                               //The viewcone keeps track of the viewCone AND the cone for melee range detection
        protected Path path;                                          //Data structure for an Agent's pathNodes ( DIFFERENT THAN THE TUTORIAL'S )
        protected Path anotherway;
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
        int HP = 50;                                               //Hit poitns

        //// sprite shape
        int spriteStartX = 0; // X of top left corner of sprite 0. 
        int spriteStartY = 0; // Y of top left corner of sprite 0.
        int spriteWidth = 64;
        int spriteHeight = 64;

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

        Vector2 prevPos;

        private TimeSpan dwellTimeSpan = TimeSpan.Zero;
        #endregion

        /** Bug 2 Variables  **/
        public bool useBug2 = true;
        public static List<WallTile> obstacles;
        protected PathNode lastSafeNode;

        public static UGraphList<Cell> locationTracker;

        public static bool stopShooting;

        Line PositionToGoalNode;

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

        /// <summary>
        /// Initalization of enemy parameters
        /// </summary>
        public void Initialize()
        {
            this.DrawOrder = 400;

            this.health = 50;
            this.speed = 3.0f;

            pixelTexture = Game.Content.Load<Texture2D>("Pixel");
            viewCone = new AI_ViewCone(this.Game);
            this.texture = Game.Content.Load<Texture2D>(@"Characters\GuardSpriteSheet");
            this.position = path.getCurrent().getPosition();

            this.prevPos = this.position;

            this.source = new Rectangle(this.spriteStartX, this.spriteStartY, this.spriteWidth, this.spriteHeight);
            this.destination = source;
            this.destination.X = (int)this.position.X;
            this.destination.Y = (int)this.position.Y;

            //Circle for sound detection
            this.soundCircle = new Circle(this.position, 500);
            playerLineOfSight = new Line(this.position, TraptMain.player.Position);

            //Setting the initial state of the Enemy to pathing
            currentNode = path.getNext();
            currentState = AIstate.PATHING;

            //Initalizing the enemy's weapon to a random weapon type
            Array values = Enum.GetValues(typeof(WeaponType));
            WeaponType rndType = (WeaponType)values.GetValue(TraptMain.genRand.Next(values.Length));
            Weapon randWpn = new Weapon(Game);
            randWpn.Initialize(this.position, 30, rndType);
            randWpn.PickUp(true, this);
            
            //Initalization of map pathway
            anotherway = new Path();

            //animation
            this.aniRow = 1;
            this.aniLength = 2;
            this.aniRate = 333;
            this.frameWidth = 64;

            //Intialize Bug2
            //InitializeBug2(position);

            base.Initialize();
        }

        /// <summary>
        /// Checks to see which direction a wall is in relation to the enemy for use in collision detetion
        /// Horizontal walls
        /// </summary>
        /// <param name="w"></param>
        /// <param name="enemyPosition"></param>
        /// <returns></returns>
        private wallSide getHorizontalWallSide(WallTile w, Vector2 enemyPosition)
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
            return returnSide;
        }

        /// <summary>
        /// Checks to see which direction a wall is in relation to the enemy for use in collision detetion
        /// Vertical walls
        /// </summary>
        /// <param name="w"></param>
        /// <param name="enemyPosition"></param>
        /// <returns></returns>
        private wallSide getVerticalWallSide(WallTile w, Vector2 enemyPosition)
        {
            Vector2 wallCenter = new Vector2((w.Position.X + (128 / 2)), (w.Position.Y + (128 / 2)));
            wallSide returnSide = wallSide.UNKNOWN;
            if (enemyPosition.Y <= wallCenter.Y)
            {
                returnSide = wallSide.DOWN;
            }
            else if (enemyPosition.Y >= wallCenter.Y)
            {
                returnSide = wallSide.UP;
            }
            return returnSide;
        }

        /// <summary>
        /// Dictates a enemy's behavior when dwelling at a node.
        /// </summary>
        /// <param name="tempRotation"></param>
        /// Does not get called
        public void lookingAround(float tempRotation)
        {
            float upper = tempRotation + ((float)Math.PI / 6);
            float lower = tempRotation - ((float)Math.PI / 6);

            if (this.rotation < upper)
            {
                this.rotation += ((float)Math.PI / 36);
            }
        }

        /// <summary>
        /// Method to check the enemy's viewcone and dictate the enemy's actions given information from the viewcone
        /// </summary>
        private void CheckViewCone()
        {
            if (LineOfSightBroken(playerLineOfSight))
            {
                if (viewCone.intersectsViewCone(TraptMain.player.Destination))
                {
                    this.anotherway = new Path();
                }

                currentState = AIstate.PATHING;
            }
            else //line of sight NOT broken
            {
                if (viewCone.intersectsViewCone(TraptMain.player.Destination))
                {
                    if (TraptMain.player.power == Power.Shroud)
                    {
                        currentState = AIstate.PATHING;
                    }
                    else
                    {
                        currentState = AIstate.SEARCHING;
                    }
                }
            }
        }

        /// <summary>
        /// Checks to see if the enemy's line of sight is broken
        /// </summary>
        /// <param name="LoS">Retuns true if a wall is intersection the line, false otherwise</param>
        /// <returns></returns>
        public bool LineOfSightBroken(Line LoS)
        {
            foreach (WallTile w in obstacles)                       //going through each walltile 8 connected to enemy's current position
            {
                if (playerLineOfSight.intersects(w.Destination))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Method to check if the player is inside the viewcone
        /// </summary>
        /// <returns></returns>
        /// (Not really necisarry)....
        private bool PlayerInsideEnemyViewCone()
        {
            if (viewCone.intersectsViewCone(TraptMain.player.Destination))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Method to check if the player is inside the melee cone
        /// Dictates what happens if the player is in the melee cone and attacks
        /// </summary>
        private void CheckMeleeCone()
        {
            if (this.viewCone.intersectsMeleeCone(TraptMain.player.Destination) && (TraptMain.player.meleeDelay > TimeSpan.Zero))
            {
                this.HurtEnemy(Int32.MaxValue);
            }
        }

        /// <summary>
        /// Method that indicates what to do when an enemy has been dragged from their path and they collide with wall tiles
        /// </summary>
        private void OuttaHere()
        {
            if (isStuck)
            {
                try
                {
                    if (anotherway.Count() == 0)
                    {
                        anotherway = GraphToPath(                                                                       //Calculating Djikstras
                            (AGraph<PathNode>)TraptMain.tileLayer.TransitionGrid.ShortestWeightedPath(
                            new PathNode((int)position.X / 128, (int)position.Y / 128, 0),
                            new PathNode((int)currentNode.position.X / 128, (int)currentNode.position.Y / 128, 0)));
                    }
                    PathNode temp = anotherway.First.Value;
                    anotherway.RemoveFirst();
                    goalNode = currentNode;
                    currentNode = new PathNode((int)temp.position.X * 128 + 64, (int)temp.position.Y * 128 + 64, 0);
                    isStuck = false;
                }
                catch
                {
                    //do something to get out of wall.
                    this.position.X = lastSafeNode.position.X * TraptMain.GRID_CELL_SIZE + TraptMain.GRID_CELL_SIZE / 2;
                    this.position.Y = lastSafeNode.position.Y * TraptMain.GRID_CELL_SIZE + TraptMain.GRID_CELL_SIZE / 2;
                }
                finally
                {
                }
            }
        }

        public void TraversePath(GameTime gameTime)
        {
            velocity = Vector2.Zero;
            float tempRotation = 0;
            //are we close to the node?
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
                    goalNode = new PathNode((int)temp.position.X * 128 + 64, (int)temp.position.Y * 128 + 64, 0);
                }
                currentNode = goalNode;
            }

            //We've reached a node and now dwell
            if (dwellTimeSpan >= TimeSpan.Zero)
            {
                dwelling = true;
                //this.lookingAround(tempRotation);
                dwellTimeSpan -= gameTime.ElapsedGameTime;
            }
            else
            {
                //Move out towards the next node in the path
                dwelling = false;
                float dx = currentNode.getPosition().X - this.position.X;
                float dy = currentNode.getPosition().Y - this.position.Y;
                rotation = (float)(Math.Atan2(dy, dx) + Math.PI / 2);

                this.velocity.Y = (float)(this.speed * Math.Cos(this.rotation + Math.PI));
                this.velocity.X = (float)(this.speed * Math.Sin(this.rotation));
            }
        }

        /// <summary>
        /// Generates a path given the map's global pathway nodes
        /// </summary>
        /// <param name="graph"> Tiles </param>
        /// <returns> Path back to node </returns>
        private Path GraphToPath(AGraph<PathNode> graph)
        {
            Path result = new Path();
            foreach (IVertex<PathNode> node in graph.EnumerateVertices(new PathNode((int)position.X / 128, (int)position.Y / 128, 0)))
            {
                result.AddLast(node.Data);
            }
            return result;
        }

        /*This method will get the 8 surrounding wall tiles for the enemy.*/
        private List<WallTile> GetNearest8WallTiles()
        {
            //create a temporary list of wall tiles to return
            List<WallTile> temp = new List<WallTile>();

            //this cell
            foreach (GameComponentRef obstacle in this.checkin)
            {
                //if it is a wall tile add it to the list 
                if (obstacle.item is WallTile)
                {
                    //add it to the list
                    temp.Add((WallTile)obstacle.item);
                }
            }

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

        /// <summary>
        /// This method dictates the behavior of the AI when currentState is set to searching
        /// </summary>
        /// <param name="playerX">Player's position</param>
        /// <param name="playerY">Player's position</param>
        public void SearchForPlayer(int playerX, int playerY)
        {
            //Setting pathing to go for the player's location
            currentNode = new PathNode(playerX, playerY, 0);

            //getting the angle to where the player was last seen
            float dx = currentNode.getPosition().X - this.position.X;
            float dy = currentNode.getPosition().Y - this.position.Y;
            rotation = (float)(Math.Atan2(dy, dx) + Math.PI / 2);

            //Head towards the player
            this.velocity.Y = (float)(this.speed * Math.Cos(this.rotation + Math.PI));
            this.velocity.X = (float)(this.speed * Math.Sin(this.rotation));
        }

        /// <summary>
        /// Dictates how the AI takes damage and what to do when the AI dies
        /// </summary>
        /// <param name="damage"></param>
        public void HurtEnemy(int damage)
        {
            this.health -= damage;
            if (this.health <= 0)
            {
                this.isDead = true;
            }
            if (this.currentState != AIstate.SEARCHING
                && this.currentState != AIstate.ATTACKING
                && !this.isDead)
            {
                this.currentState = AIstate.SEARCHING;
            }
        }

        public override void Update(GameTime gameTime)
        {
            PathNode testSafeNode = new PathNode((int)this.position.X / 128, (int)this.position.Y / 128, 0);
            //if we are on a safe node
            if (TraptMain.tileLayer.TransitionGrid.HasVertex(testSafeNode)
                && !testSafeNode.Equals(lastSafeNode))
            {
                //save this node for corrections
                lastSafeNode = testSafeNode;
            }

            //update the sound circle
            this.soundCircle = new Circle(this.position, 500);


            this.prevPos = this.position;
            obstacles = this.GetNearest8WallTiles();
            playerLineOfSight = new Line(this.position, TraptMain.player.Position);
            this.PositionToGoalNode = new Line(currentNode.position, this.position);
            float distBetweenThem = Vector2.Distance(this.position, TraptMain.player.Position);

            this.OuttaHere();
           
            //Update actions given that the current state is pathing
            if (currentState == AIstate.PATHING)
            {
                TraversePath(gameTime);

                //if the circles are in collision and the player is shooting then change the state to searching
                if (this.soundCircle.Intersects(TraptMain.player.soundCircle) && TraptMain.player.isShooting &&
                    !LineOfSightBroken(playerLineOfSight) )
                {
                    currentState = AIstate.SEARCHING;
                }
            }
            //Update actions given the current state is searching
            if (currentState == AIstate.SEARCHING)
            {
                this.SearchForPlayer((int)TraptMain.player.Position.X, (int)TraptMain.player.Position.Y);

                //If at an appropriate distance, attack the player
                if (distBetweenThem < 400) currentState = AIstate.ATTACKING;
            }
            //Update actions given the current state is attacking
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
                if (!stopShooting)
                    this.Weapon.Shoot();
            }
            if (this.isDead)
            {
                this.velocity.X = 0;
                this.velocity.Y = 0;

                this.aniStart = 0;
                this.aniRow = 3;
                this.aniLength = 1;
                this.aniRate = 1400;

                //The folling is what to do if dead.
                //Object for death of an agent
                AgentDie zombie = new AgentDie(Game);
                zombie.Initialize(this.texture, 3300, this.rotation, this.position, this.source, this.destination);
                zombie.SetAnimationParams(this.aniStart, this.aniLength, this.aniRate, this.aniRow, this.frameWidth, this.frameHeight);

                //get rid of the enemy object
                this.Weapon.Drop();
                this.ClearLocationCheckin();
                this.Dispose(true);
            }

            //Resolve collision
            int hitCount = imHitting.Count();
            for (int i = hitCount - 1; 0 <= i; i--)
            {
                this.Collide(imHitting[i]);
                imHitting.RemoveAt(i);
            }//finish collision resolution


            //Update position
            this.position.X += velocity.X;
            this.position.Y += velocity.Y;

            //update enemy's parameters
            spriteCenter = new Vector2((this.source.Width / 2), (this.source.Height / 2));
            viewCone.Update(gameTime, rotation, this.position);
            this.CheckViewCone();
            if (currentState != AIstate.ATTACKING && currentState != AIstate.SEARCHING)
            {
                this.CheckMeleeCone();
            }

            base.Update(gameTime);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            this.destination.X = (int)this.position.X;
            this.destination.Y = (int)this.position.Y;       

            spriteBatch.Draw(this.texture, this.destination, this.source, Color.White,
                this.Rotation, // The rotation of the Sprite.  0 = facing up, Pi/2 = facing right
                spriteCenter,
                SpriteEffects.None, this.Depth);

            //re-center hit box
            this.destination.X = (int)this.position.X - this.source.Width / 2;
            this.destination.Y = (int)this.position.Y - this.source.Height / 2;
        }

        /// <summary>
        /// Returns the walltile object that the AI first came into contact with
        /// </summary>
        /// <returns></returns>
        public WallTile getFirstCollided()
        {
            foreach (WallTile w in obstacles)
            {
                if (PositionToGoalNode.intersects(w.Destination))
                {
                    return w;
                }
            }
            Console.WriteLine("returning NULL");
            return null;
        }

        /// <summary>
        /// Check to see if any collisions have occured
        /// </summary>
        /// <param name="that"></param>
        public override void Collide(EnvironmentObj that)
        {
            wallSide bar = wallSide.UNKNOWN;
            if (that is WallTile)
            {
                bar = this.getHorizontalWallSide((WallTile)that, this.prevPos);
                if (bar == wallSide.LEFT)
                {
                    this.position.X += this.speed;
                }
                else if (bar == wallSide.RIGHT)
                {
                    this.position.X -= this.speed;
                }
                bar = this.getVerticalWallSide((WallTile)that, this.prevPos);
                if (bar == wallSide.UP)
                {
                    this.position.Y += this.speed;
                }
                else if (bar == wallSide.DOWN)
                {
                    this.position.Y -= this.speed;
                }
                isStuck = true;
            }

            if (that is Player)
            {
                //stop the animation
                this.stopAnimation = true;

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
