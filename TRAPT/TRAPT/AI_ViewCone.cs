/**
 * Jason Carter
**/

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

    public class AI_ViewCone : Microsoft.Xna.Framework.GameComponent
    {
        
        //Circle viewConeCircle;
        Vector2 leftPoint;
        Vector2 leftMidPoint1, leftMidPoint2, leftMidPoint3, leftMidPoint4, leftMidPoint5, leftMidPoint6;
        Vector2 rightPoint;
        Vector2 rightMidPoint1, rightMidPoint2, rightMidPoint3, rightMidPoint4, rightMidPoint5, rightMidPoint6;
        Line leftSideViewCone;
        Line rightSideViewCone;
        Line frontLine;
        Line midLine1, midLine2, midLine3, midLine4, midLine5, midLine6;
        double viewConeLAngle;
        double viewConeRAngle;

        //Circle meleeRangeCircle;
        Vector2 leftPointBack;
        Vector2 rightPointBack;
        Line leftSideMeleeCone;
        Line rightSideMeleeCone;
        Line backLine;

        Texture2D lineTexture;

        Vector2 enemyPosition;

        const int vcRADIUS = 600;
        const int mcRADIUS = 50;

        public AI_ViewCone(Game game) : base(game)
        {
            lineTexture = game.Content.Load<Texture2D>("Pixel");
        }

        public virtual void  Initialize(float rotation, Vector2 center )
        {
            enemyPosition = center;
            float playerRotation = rotation + ((float)(3 * (Math.PI/2)));
            leftPoint = new Vector2((float)(((center.X + vcRADIUS) * Math.Cos(playerRotation - Math.PI/8))), (float) ((center.Y + vcRADIUS) * Math.Sin(playerRotation - Math.PI/8)));
            //leftPoint = new Vector2((float)(center.X), (float) (center.Y));
            rightPoint = new Vector2((float)((center.X + vcRADIUS) * Math.Cos(playerRotation + Math.PI/8)),  (float) ((center.Y + vcRADIUS) * Math.Sin(playerRotation + Math.PI/8)));
            rightPoint = new Vector2((float)((center.X + vcRADIUS) * Math.Cos(playerRotation)),  (float) ((center.Y + vcRADIUS) * Math.Sin(playerRotation)));
            leftSideViewCone = new Line(center, leftPoint);
            rightSideViewCone = new Line(center, rightPoint);
            frontLine = new Line(leftPoint, rightPoint);

            //Testing
            leftMidPoint1 = new Vector2((float)(((center.X + (vcRADIUS / 2)) * Math.Cos(playerRotation - Math.PI / 8))), (float)((center.Y + (vcRADIUS / 2)) * Math.Sin(playerRotation - Math.PI / 8)));
            rightMidPoint1 = new Vector2((float)((center.X + (vcRADIUS / 2)) * Math.Cos(playerRotation + Math.PI / 8)), (float)((center.Y + (vcRADIUS / 2)) * Math.Sin(playerRotation + Math.PI / 8)));
            midLine1 = new Line(leftMidPoint1, rightMidPoint1);


            leftMidPoint2 = new Vector2((float)(((center.X + (vcRADIUS / 2)) * Math.Cos(playerRotation - Math.PI / 8))), (float)((center.Y + (vcRADIUS / 2)) * Math.Sin(playerRotation - Math.PI / 8)));
            rightMidPoint2 = new Vector2((float)((center.X + (vcRADIUS / 2)) * Math.Cos(playerRotation + Math.PI / 8)), (float)((center.Y + (vcRADIUS / 2)) * Math.Sin(playerRotation + Math.PI / 8)));
            midLine2 = new Line(leftMidPoint2, rightMidPoint2);

            leftMidPoint3 = new Vector2((float)(((center.X + (vcRADIUS / 2)) * Math.Cos(playerRotation - Math.PI / 8))), (float)((center.Y + (vcRADIUS / 2)) * Math.Sin(playerRotation - Math.PI / 8)));
            rightMidPoint3 = new Vector2((float)((center.X + (vcRADIUS / 2)) * Math.Cos(playerRotation + Math.PI / 8)), (float)((center.Y + (vcRADIUS / 2)) * Math.Sin(playerRotation + Math.PI / 8)));
            midLine3 = new Line(leftMidPoint3, rightMidPoint3);

            leftMidPoint4 = new Vector2((float)(((center.X + (vcRADIUS / 2)) * Math.Cos(playerRotation - Math.PI / 8))), (float)((center.Y + (vcRADIUS / 2)) * Math.Sin(playerRotation - Math.PI / 8)));
            rightMidPoint4 = new Vector2((float)((center.X + (vcRADIUS / 2)) * Math.Cos(playerRotation + Math.PI / 8)), (float)((center.Y + (vcRADIUS / 2)) * Math.Sin(playerRotation + Math.PI / 8)));
            midLine4 = new Line(leftMidPoint4, rightMidPoint4);

            leftMidPoint5 = new Vector2((float)(((center.X + (vcRADIUS / 2)) * Math.Cos(playerRotation - Math.PI / 8))), (float)((center.Y + (vcRADIUS / 2)) * Math.Sin(playerRotation - Math.PI / 8)));
            rightMidPoint5 = new Vector2((float)((center.X + (vcRADIUS / 2)) * Math.Cos(playerRotation + Math.PI / 8)), (float)((center.Y + (vcRADIUS / 2)) * Math.Sin(playerRotation + Math.PI / 8)));
            midLine5 = new Line(leftMidPoint5, rightMidPoint5);

            leftMidPoint6 = new Vector2((float)(((center.X + (vcRADIUS / 2)) * Math.Cos(playerRotation - Math.PI / 8))), (float)((center.Y + (vcRADIUS / 2)) * Math.Sin(playerRotation - Math.PI / 8)));
            rightMidPoint6 = new Vector2((float)((center.X + (vcRADIUS / 2)) * Math.Cos(playerRotation + Math.PI / 8)), (float)((center.Y + (vcRADIUS / 2)) * Math.Sin(playerRotation + Math.PI / 8)));
            midLine6 = new Line(leftMidPoint6, rightMidPoint6);



            float dYLPoint = leftPoint.Y - center.Y;
            float dXLPoint = leftPoint.X - center.X;

            float dYRPoint = rightPoint.Y - center.Y;
            float dXRPoint = rightPoint.X - center.X;


            this.viewConeLAngle = Math.Atan2(dYLPoint, dXLPoint);
            this.viewConeRAngle = Math.Atan2(dYRPoint, dXRPoint);

           

           float backLineRotation = rotation + ((float)(Math.PI/2));
           leftPointBack = new Vector2((float)(center.X + mcRADIUS * Math.Cos(backLineRotation - Math.PI /4)), (float)((center.Y + mcRADIUS) * Math.Sin(backLineRotation - Math.PI /4)));
           rightPointBack = new Vector2((float)(center.X + mcRADIUS * Math.Cos(backLineRotation + Math.PI /4)), (float)((center.Y + mcRADIUS) * Math.Sin(backLineRotation + Math.PI /4)));
            leftSideMeleeCone = new Line(center, leftPointBack);
            rightSideMeleeCone = new Line(center, rightPointBack);
            backLine = new Line(leftPointBack, rightPointBack);

 	        base.Initialize();
        }

        /* 
         * Will need to implement changing the viewcone from the XML for ease of bug testing, and so that others who haven't looked at the code can
         * the viewCone/melee cone properties.
         **/
        public virtual void  Update(GameTime gameTime, float rotation, Vector2 center)
        {
            enemyPosition = center;



            //ViewCone book-keeping/Orientation/whatnot
            float playerRotation = rotation + ((float)(3 * (Math.PI / 2)));
            leftPoint = new Vector2((float)(center.X + (vcRADIUS * (Math.Cos(playerRotation - Math.PI / 10)))), (float)(center.Y + vcRADIUS * (Math.Sin(playerRotation - Math.PI / 10))));          //Attaining a point on a circle's circumfrance - LEFT SIDE
            rightPoint = new Vector2((float)(center.X + vcRADIUS * (Math.Cos(playerRotation + Math.PI / 10))), (float)(center.Y + vcRADIUS * (Math.Sin(playerRotation + Math.PI / 10))));           //Attaining a point on a circle's circumfrance - RIGHT SIDE
            leftSideViewCone = new Line(center, leftPoint);                 //Setting lines to make the sides of the viewcone, colision will happen with the lines themselves
            rightSideViewCone = new Line(center, rightPoint);
            frontLine = new Line(leftPoint, rightPoint);                    //Setting the line for the front of the viewcone.


            //Testing
            leftMidPoint1 = new Vector2((float)(center.X + ((vcRADIUS / 3) * (Math.Cos(playerRotation - Math.PI / 10)))), (float)(center.Y + (vcRADIUS / 3) * (Math.Sin(playerRotation - Math.PI / 10))));
            rightMidPoint1 = new Vector2((float)(center.X + (vcRADIUS / 3) * (Math.Cos(playerRotation + Math.PI / 10))), (float)(center.Y + (vcRADIUS / 3) * (Math.Sin(playerRotation + Math.PI / 10))));
            midLine1 = new Line(leftMidPoint1, rightMidPoint1);

            leftMidPoint2 = new Vector2((float)(center.X + ((vcRADIUS / 2.2) * (Math.Cos(playerRotation - Math.PI / 10)))), (float)(center.Y + (vcRADIUS/ 2.2) * (Math.Sin(playerRotation - Math.PI / 10))));
            rightMidPoint2 = new Vector2((float)(center.X + (vcRADIUS / 2.2) * (Math.Cos(playerRotation + Math.PI / 10))), (float)(center.Y + (vcRADIUS / 2.2) * (Math.Sin(playerRotation + Math.PI / 10))));
            midLine2 = new Line(leftMidPoint2, rightMidPoint2);

            leftMidPoint3 = new Vector2((float)(center.X + ((vcRADIUS / 1.8) * (Math.Cos(playerRotation - Math.PI / 10)))), (float)(center.Y + (vcRADIUS / 1.8) * (Math.Sin(playerRotation - Math.PI / 10))));
            rightMidPoint3 = new Vector2((float)(center.X + (vcRADIUS / 1.8) * (Math.Cos(playerRotation + Math.PI / 10))), (float)(center.Y + (vcRADIUS / 1.8) * (Math.Sin(playerRotation + Math.PI / 10))));
            midLine3 = new Line(leftMidPoint3, rightMidPoint3);

            leftMidPoint4 = new Vector2((float)(center.X + ((vcRADIUS / 1.5) * (Math.Cos(playerRotation - Math.PI / 10)))), (float)(center.Y + (vcRADIUS / 1.5) * (Math.Sin(playerRotation - Math.PI / 10))));
            rightMidPoint4 = new Vector2((float)(center.X + (vcRADIUS / 1.5) * (Math.Cos(playerRotation + Math.PI / 10))), (float)(center.Y + (vcRADIUS / 1.5) * (Math.Sin(playerRotation + Math.PI / 10))));
            midLine4 = new Line(leftMidPoint4, rightMidPoint4);

            leftMidPoint5 = new Vector2((float)(center.X + ((vcRADIUS / 1.3) * (Math.Cos(playerRotation - Math.PI / 10)))), (float)(center.Y + (vcRADIUS / 1.3) * (Math.Sin(playerRotation - Math.PI / 10))));
            rightMidPoint5 = new Vector2((float)(center.X + (vcRADIUS / 1.3) * (Math.Cos(playerRotation + Math.PI / 10))), (float)(center.Y + (vcRADIUS / 1.3) * (Math.Sin(playerRotation + Math.PI / 10))));
            midLine5 = new Line(leftMidPoint5, rightMidPoint5);

            leftMidPoint6 = new Vector2((float)(center.X + ((vcRADIUS / 1.15) * (Math.Cos(playerRotation - Math.PI / 10)))), (float)(center.Y + (vcRADIUS / 1.15) * (Math.Sin(playerRotation - Math.PI / 10))));
            rightMidPoint6 = new Vector2((float)(center.X + (vcRADIUS / 1.15) * (Math.Cos(playerRotation + Math.PI / 10))), (float)(center.Y + (vcRADIUS / 1.15) * (Math.Sin(playerRotation + Math.PI / 10))));
            midLine6 = new Line(leftMidPoint6, rightMidPoint6);




            float dYLPoint = leftPoint.Y - center.Y;            //Used in finding the angle of the left line
            float dXLPoint = leftPoint.X -center.X;

            float dYRPoint = rightPoint.Y - center.Y;           //Used in finding the angle of the right line
            float dXRPoint = rightPoint.X - center.X;



            this.viewConeLAngle = Math.Atan2(dYLPoint, dXLPoint);
            this.viewConeRAngle = Math.Atan2(dYRPoint, dXRPoint);

           // Console.WriteLine((viewConeLAngle - viewConeRAngle) * Math.PI);

            //Melee hitcone book-keeping/Orientation/whatnot
            float backLineRotation = rotation + ((float)(Math.PI / 2));
            leftPointBack = new Vector2((float)(center.X + 50 * Math.Cos(backLineRotation - Math.PI / 6)), (float)(center.Y + 50 * Math.Sin(backLineRotation - Math.PI / 6)));
            rightPointBack = new Vector2((float)(center.X + 50 * Math.Cos(backLineRotation + Math.PI / 6)), (float)(center.Y + 50 * Math.Sin(backLineRotation + Math.PI / 6)));
            leftSideMeleeCone = new Line(center, leftPointBack);
            rightSideMeleeCone = new Line(center, rightPointBack);
            backLine = new Line(leftPointBack, rightPointBack);

            //check for collision somehwere here.

 	        base.Update(gameTime);
        }



        //Working on this
        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerRectangle"></param>
        /// <returns></returns>
        

      
        /*
         * The parameter to this method needs to be changed to the player object.
         * From there we need to obtain the the bounding box. And do collision....
         * */
        public Boolean intersectsViewCone(Rectangle playerRectangle, Vector2 playerPosition)
        {
            float dX = playerPosition.Y - enemyPosition.Y;
            float dY = playerPosition.X - enemyPosition.X;
            //float dX = enemyPosition.Y - playerPosition.Y;
            //float dY = enemyPosition.X - playerPosition.X;


            double angle = (Math.Atan2(dY, dX) - Math.PI/2);


           //Console.WriteLine("Angle to player: " + angle);
           //Console.WriteLine("angle to leftSide of Cone: " + viewConeLAngle);
           //Console.WriteLine("angle to rightSide of Cone: " + viewConeRAngle);

            if (leftSideViewCone.intersects(playerRectangle) ||                     //Checking for intersection of an edge of the viewcone
                rightSideViewCone.intersects(playerRectangle)||
                frontLine.intersects(playerRectangle)        ||
                midLine1.intersects(playerRectangle)         || 
                midLine2.intersects(playerRectangle)         ||
                midLine3.intersects(playerRectangle)         ||
                midLine4.intersects(playerRectangle)         ||
                midLine5.intersects(playerRectangle)         ||
                midLine6.intersects(playerRectangle)
                )
            {

                return true;
            }           
            else
            {
                return false;
            }
        }

        /*
        * The parameter to this method needs to be changed to the player object.
        * From there we need to obtain the the bounding box. And do collision....
        **/
        public Boolean intersectsMeleeCone(Rectangle playerRectangle){
            if (leftSideMeleeCone.intersects(playerRectangle) ||
               rightSideMeleeCone.intersects(playerRectangle) ||
               backLine.intersects(playerRectangle))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    
        public void Draw(SpriteBatch spriteBatch){
            leftSideViewCone.Draw(spriteBatch, lineTexture);
            rightSideViewCone.Draw(spriteBatch, lineTexture);
            frontLine.Draw(spriteBatch, lineTexture);

            midLine1.Draw(spriteBatch, lineTexture);
            midLine2.Draw(spriteBatch, lineTexture);
            midLine3.Draw(spriteBatch, lineTexture);
            midLine4.Draw(spriteBatch, lineTexture);
            midLine5.Draw(spriteBatch, lineTexture);
            midLine6.Draw(spriteBatch, lineTexture);

            leftSideMeleeCone.Draw(spriteBatch, lineTexture);
            rightSideMeleeCone.Draw(spriteBatch, lineTexture);
            backLine.Draw(spriteBatch, lineTexture);
        }
    }
}
