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
        Vector2 rightPoint;
        Line leftSideViewCone;
        Line rightSideViewCone;
        Line frontLine;

        //Circle meleeRangeCircle;
        Vector2 leftPointBack;
        Vector2 rightPointBack;
        Line leftSideMeleeCone;
        Line rightSideMeleeCone;
        Line backLine;

        Texture2D lineTexture;

        const int vcRADIUS = 100;
        const int mcRADIUS = 50;

        public AI_ViewCone(Game game) : base(game)
        {
            lineTexture = game.Content.Load<Texture2D>("Pixel");
        }

        public virtual void  Initialize(float rotation, Vector2 center )
        {
            float playerRotation = rotation + ((float)(3 * (Math.PI/2)));
            leftPoint = new Vector2((float)(((center.X + vcRADIUS) * Math.Cos(playerRotation - Math.PI/4))), (float) ((center.Y + vcRADIUS) * Math.Sin(playerRotation - Math.PI/4)));
            rightPoint = new Vector2((float)((center.X + vcRADIUS) * Math.Cos(playerRotation + Math.PI / 4)), (float)((center.Y + vcRADIUS) * Math.Sin(playerRotation + Math.PI / 4)));
            leftSideViewCone = new Line(center, leftPoint);
            rightSideViewCone = new Line(center, rightPoint);
            frontLine = new Line(leftPoint, rightPoint);

           float backLineRotation = rotation + ((float)(Math.PI/2));
           leftPointBack = new Vector2((float)(center.X + mcRADIUS * Math.Cos(backLineRotation - Math.PI / 4)), (float)((center.Y + mcRADIUS) * Math.Sin(backLineRotation - Math.PI / 4)));
           rightPointBack = new Vector2((float)(center.X + mcRADIUS * Math.Cos(backLineRotation + Math.PI / 4)), (float)((center.Y + mcRADIUS) * Math.Sin(backLineRotation + Math.PI / 4)));
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
            //ViewCone book-keeping/Orientation/whatnot
            float playerRotation = rotation + ((float)(3 * (Math.PI / 2)));
            leftPoint = new Vector2((float)(center.X + (200 * (Math.Cos(playerRotation - Math.PI / 4)))), (float)(center.Y + 200 * (Math.Sin(playerRotation - Math.PI / 4))));          //Attaining a point on a circle's circumfrance - LEFT SIDE
            rightPoint = new Vector2((float)(center.X + 200 * (Math.Cos(playerRotation + Math.PI / 4))), (float)(center.Y + 200 * (Math.Sin(playerRotation + Math.PI / 4))));           //Attaining a point on a circle's circumfrance - RIGHT SIDE
            leftSideViewCone = new Line(center, leftPoint);                 //Setting lines to make the sides of the viewcone, colision will happen with the lines themselves
            rightSideViewCone = new Line(center, rightPoint);
            frontLine = new Line(leftPoint, rightPoint);                    //Setting the line for the front of the viewcone.

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
        public Boolean intersectsViewCone(Rectangle playerRectangle)
        {
            if(leftSideViewCone.intersects(playerRectangle) ||
              rightSideViewCone.intersects(playerRectangle) ||
              frontLine.intersects(playerRectangle))
            {
                return true;
            }
            else{
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

            leftSideMeleeCone.Draw(spriteBatch, lineTexture);
            rightSideMeleeCone.Draw(spriteBatch, lineTexture);
            backLine.Draw(spriteBatch, lineTexture);
        }
    }
}
