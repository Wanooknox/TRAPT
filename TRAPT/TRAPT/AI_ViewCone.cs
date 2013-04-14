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

        //Points for lines on the view cone and melee cone
        Vector2 leftPoint;
        Vector2 rightPoint;
        Vector2 leftPointBack;
        Vector2 rightPointBack;
        Vector2 leftMidPoint;
        Vector2 rightMidPoint;
        Vector2 centerLeftPoint;
        Vector2 centerRightPoint;
        Vector2 backCenterPoint;

        //Lines for the view cone and melee cone
        Line leftSideViewCone;
        Line rightSideViewCone;
        Line frontLine;
        Line leftSideMeleeCone;
        Line rightSideMeleeCone;
        Line backLine;
        Line leftMidLine;
        Line centerLeftLine;
        Line centerRightLine;
        Line rightMidLine;

        //Angles corresponding to the viewcones left and right points ( based on the AI's rotation )
        double viewConeLAngle;
        double viewConeRAngle;

        //For Debugging purposes
        Texture2D lineTexture;

        //Position for the viewcone to be centered around
        Vector2 enemyPosition;

        //Constant values for the view cone's dimensions
        const int vcRADIUS = 450;       // Length of the Viewcone
        const int mcRADIUS = 50;        //Length of the Melee cone 

        public AI_ViewCone(Game game)
            : base(game)
        {
            lineTexture = game.Content.Load<Texture2D>("Pixel");
        }

        public virtual void Initialize(float rotation, Vector2 center)
        {
            enemyPosition = center;

            //Finding the point on a line given the AI's position and rotation
            float playerRotation = rotation + ((float)(3 * (Math.PI / 2)));
            leftPoint = new Vector2((float)(((center.X + vcRADIUS) * Math.Cos(playerRotation - Math.PI / 8))), (float)((center.Y + vcRADIUS) * Math.Sin(playerRotation - Math.PI / 8)));
            //??
            rightPoint = new Vector2((float)((center.X + vcRADIUS) * Math.Cos(playerRotation + Math.PI / 8)), (float)((center.Y + vcRADIUS) * Math.Sin(playerRotation + Math.PI / 8)));
            rightPoint = new Vector2((float)((center.X + vcRADIUS) * Math.Cos(playerRotation)), (float)((center.Y + vcRADIUS) * Math.Sin(playerRotation)));
            leftSideViewCone = new Line(center, leftPoint);
            rightSideViewCone = new Line(center, rightPoint);
            frontLine = new Line(leftPoint, rightPoint);
           
            //Finding the vector for the left side of the viewcone in relation to the enemy
            float dYLPoint = leftPoint.Y - center.Y;
            float dXLPoint = leftPoint.X - center.X;

            //Finding the vector for the right side of the viewcone in relation to the enemy
            float dYRPoint = rightPoint.Y - center.Y;
            float dXRPoint = rightPoint.X - center.X;

            //Calculating the angle
            this.viewConeLAngle = Math.Atan2(dYLPoint, dXLPoint);
            this.viewConeRAngle = Math.Atan2(dYRPoint, dXRPoint);


            //Calculations for the melee cone
            float backLineRotation = rotation + ((float)(Math.PI / 2));
            leftPointBack = new Vector2((float)(center.X + mcRADIUS * Math.Cos(backLineRotation - Math.PI / 4)), (float)((center.Y + mcRADIUS) * Math.Sin(backLineRotation - Math.PI / 4)));
            rightPointBack = new Vector2((float)(center.X + mcRADIUS * Math.Cos(backLineRotation + Math.PI / 4)), (float)((center.Y + mcRADIUS) * Math.Sin(backLineRotation + Math.PI / 4)));
            leftSideMeleeCone = new Line(center, leftPointBack);
            rightSideMeleeCone = new Line(center, rightPointBack);
            backLine = new Line(leftPointBack, rightPointBack);

            base.Initialize();
        }

        public virtual void Update(GameTime gameTime, float rotation, Vector2 center)
        {
            enemyPosition = center;



            //Updating the viewcones angle given the AI's rotation and position
            float playerRotation = rotation - ((float)(Math.PI / 2));
            leftPoint = new Vector2((float)(center.X + (vcRADIUS * (Math.Cos(playerRotation - Math.PI / 10)))), (float)(center.Y + vcRADIUS * (Math.Sin(playerRotation - Math.PI / 10))));          //Attaining a point on a circle's circumfrance - LEFT SIDE
            rightPoint = new Vector2((float)(center.X + vcRADIUS * (Math.Cos(playerRotation + Math.PI / 10))), (float)(center.Y + vcRADIUS * (Math.Sin(playerRotation + Math.PI / 10))));           //Attaining a point on a circle's circumfrance - RIGHT SIDE
            //Setting Lines
            leftSideViewCone = new Line(center, leftPoint);                 //Setting lines to make the sides of the viewcone, colision will happen with the lines themselves
            rightSideViewCone = new Line(center, rightPoint);
            frontLine = new Line(leftPoint, rightPoint);                    //Setting the line for the front of the viewcone.

            //Setting points for lines inside the viewcone
            leftMidPoint = new Vector2((float)(center.X + ((vcRADIUS) * (Math.Cos(playerRotation - Math.PI / 18)))), (float)(center.Y + (vcRADIUS) * (Math.Sin(playerRotation - Math.PI / 18))));
            centerLeftPoint = new Vector2((float)(center.X + (vcRADIUS) * (Math.Cos(playerRotation - Math.PI / 60))), (float)(center.Y + (vcRADIUS) * (Math.Sin(playerRotation - Math.PI / 60))));
            centerRightPoint = new Vector2((float)(center.X + (vcRADIUS) * (Math.Cos(playerRotation + Math.PI / 60))), (float)(center.Y + (vcRADIUS) * (Math.Sin(playerRotation + Math.PI / 60))));
            rightMidPoint = new Vector2((float)(center.X + (vcRADIUS) * (Math.Cos(playerRotation + Math.PI / 18))), (float)(center.Y + (vcRADIUS) * (Math.Sin(playerRotation + Math.PI / 18))));
            //Setting lines
            leftMidLine = new Line(center, leftMidPoint);
            rightMidLine = new Line(center, rightMidPoint);
            centerLeftLine = new Line(center, centerLeftPoint);
            centerRightLine = new Line(center, centerRightPoint);

            float dYLPoint = leftPoint.Y - center.Y;            //Used in finding the angle of the left line
            float dXLPoint = leftPoint.X - center.X;

            float dYRPoint = rightPoint.Y - center.Y;           //Used in finding the angle of the right line
            float dXRPoint = rightPoint.X - center.X;

            this.viewConeLAngle = Math.Atan2(dYLPoint, dXLPoint);   //Calculating the angles
            this.viewConeRAngle = Math.Atan2(dYRPoint, dXRPoint);

     

            //Book keeping for melee cone
            float backLineRotation = rotation + ((float)(Math.PI / 2));
            leftPointBack = new Vector2((float)(center.X + mcRADIUS * Math.Cos(backLineRotation - Math.PI / 6)), (float)(center.Y + mcRADIUS * Math.Sin(backLineRotation - Math.PI / 6)));
            rightPointBack = new Vector2((float)(center.X + mcRADIUS * Math.Cos(backLineRotation + Math.PI / 6)), (float)(center.Y + mcRADIUS * Math.Sin(backLineRotation + Math.PI / 6)));
            backCenterPoint = new Vector2((float)(center.X + mcRADIUS * Math.Cos(backLineRotation)), (float)(center.Y + mcRADIUS * Math.Sin(backLineRotation)));
            leftSideMeleeCone = new Line(center, leftPointBack);
            rightSideMeleeCone = new Line(center, rightPointBack);
            backLine = new Line(center, backCenterPoint);

            base.Update(gameTime);
        }



        /// <summary>
        /// Checks to see if the player is intersecting the viewcone
        /// </summary>
        /// <param name="playerRectangle">Players bounding box</param>
        /// <returns>True if in collision, false otherwise</returns>
        public Boolean intersectsViewCone(Rectangle playerRectangle)
        {
           

            if (leftSideViewCone.intersects(playerRectangle) ||                     //Checking for intersection with the viewcone
                rightSideViewCone.intersects(playerRectangle) ||
                leftMidLine.intersects(playerRectangle) ||
                centerLeftLine.intersects(playerRectangle) ||
                centerRightLine.intersects(playerRectangle) ||
                rightMidLine.intersects(playerRectangle)
                )
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check to see if the player is in the melee cone of the AI
        /// </summary>
        /// <param name="playerRectangle">Player bounding box</param>
        /// <returns>True if in collision, false otherwise</returns>
        public Boolean intersectsMeleeCone(Rectangle playerRectangle)
        {
            if (leftSideMeleeCone.intersects(playerRectangle) ||
               rightSideMeleeCone.intersects(playerRectangle))
            {
                return true;     
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Custom Draw method for debugging purposes.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            leftSideViewCone.Draw(spriteBatch, lineTexture);
            rightSideViewCone.Draw(spriteBatch, lineTexture);
           
            centerLeftLine.Draw(spriteBatch, lineTexture);
            centerRightLine.Draw(spriteBatch, lineTexture);
            leftMidLine.Draw(spriteBatch, lineTexture);
            rightMidLine.Draw(spriteBatch, lineTexture);

            leftSideMeleeCone.Draw(spriteBatch, lineTexture);
            rightSideMeleeCone.Draw(spriteBatch, lineTexture);
            backLine.Draw(spriteBatch, lineTexture);
        }
    }
}
