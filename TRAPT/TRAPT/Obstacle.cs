//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;


//namespace TRAPT
//{
//    public class Obstacle : Structure
//    {
//        // Stores the position of the obstacle in the play area
//        public Rectangle position;
//        private int bugOffset = -1;

//        public Obstacle(Game game)
//            : base(game)
//        {
//            // TODO: Construct any child components here
//        }

       
//        public override void Initialize(Rectangle newPosition, int offSet)
//        {
//            this.position = newPosition;
//            this.bugOffset = offSet;

//            base.Initialize();
//        }

//        public override void Update(GameTime gameTime)
//        {
            

//            base.Update(gameTime);
//        }

//        public static void Draw(List<Obstacle> obstacles, SpriteBatch spriteBatch, Texture2D obstacleTexture)
//        {
//            // Draw obstacles
//            for (int i = 0; i < obstacles.Count; i++)
//            {
//                spriteBatch.Draw(obstacleTexture, obstacles[i].position, Color.White);
//            }
//        }
//    }
//}
