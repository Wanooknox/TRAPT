using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Project
{
    class Camera
    {
        private Matrix transform;
        public Matrix Transform
        {
            get { return transform; }
        }


        private Vector2 centre;
        private Viewport viewport;
        private float zoom = 1;
        private float rotation = 0;

        public Camera(Viewport newViewport)
        {
            viewport = newViewport;
        }

        public float X
        {
            get { return centre.X; }
            set { centre.X = value; } 
        }

        public float Y
        {
            get { return centre.Y; }
            set { centre.Y = value; }
        }

        public float Zoom
        {
            get { return zoom; }
            set
            {
                zoom = value;
                if (zoom < 0.1f) zoom = 0.1f;
            }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }


        public void Update(Vector2 position)
        {
            centre = new Vector2(position.X, position.Y);
            transform = Matrix.CreateTranslation(new Vector3(-centre.X, -centre.Y,0)) *
                        Matrix.CreateRotationZ(Rotation) *
                        Matrix.CreateScale(new Vector3(Zoom, Zoom, 0))*
                        Matrix.CreateTranslation(new Vector3(viewport.Width/2, viewport.Height/2, 0));
        }

        //Vector2 position;
        //Matrix viewMatrix;
        //float scale = 1.0f;

        //public Matrix ViewMatrix
        //{
        //    get { return viewMatrix; }
        //}

        //public void Update(Vector2 playerPosition, Texture2D texture, Viewport view)
        //{
        //    position.X = ((playerPosition.X + texture.Width / 2) - (view.Width / 2) / scale);
        //    position.Y = ((playerPosition.Y + texture.Height / 2) - (view.Height / 2) / scale);

        //    if (position.X < 0) position.X = 0;
        //    if (position.Y < 0) position.Y = 0;

        //    if (Keyboard.GetState().IsKeyDown(Keys.Z))
        //        scale += 0.01f;
        //    else if (Keyboard.GetState().IsKeyDown(Keys.X))
        //        scale -= 0.01f;

        //    viewMatrix = Matrix.CreateTranslation(new Vector3(-position, 0)) *
        //        Matrix.CreateScale(scale);               
        //}
    }
}
