using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TRAPT
{
    class Button
    {
        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;

        Color color = new Color(255, 255, 255, 255);

        public Vector2 size;
        public bool isClicked;
        private MouseState msold;


        public Button(Texture2D newTexture, GraphicsDevice graphics)
        {
            texture = newTexture;
            
            size = new Vector2(graphics.Viewport.Width / 3, graphics.Viewport.Height / 20);
        }

        public void Update(MouseState mouse)
        {
            GamePadState gps = GamePad.GetState(PlayerIndex.One);
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);

            //Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);
            Rectangle mouseRectangle = new Rectangle((int)TraptMain.cursor.Position.X, (int)TraptMain.cursor.Position.Y, 1, 1);

            if (mouseRectangle.Intersects(rectangle))
            {
                this.color = Color.Red;
                if (mouse.LeftButton == ButtonState.Pressed && msold.LeftButton == ButtonState.Released
                    || gps.IsButtonDown(Buttons.A))
                {
                    isClicked = true;
                }
            }
            if (!mouseRectangle.Intersects(rectangle))
            {
                this.color = Color.White;
            }

            msold = mouse;
        }

        public void setPosition(Vector2 newPosition)
        {
            position = newPosition;
        }

        public void setSize(int x, int y)
        {
            size = new Vector2(x, y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, color);
        }
    }
}
