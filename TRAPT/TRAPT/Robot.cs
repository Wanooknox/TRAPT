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
    public class Robot : Enemy
    {
        bool isStatSet = false;

        public Robot(Game game)
            : base(game)
        { }

        public virtual void Intialize()
        {
            base.Initialize();
        }
       
        public override void Update(GameTime gameTime)
        {
            Console.WriteLine("Robot is being updated");
            if( !isStatSet )
            {
                this.viewCone = new AI_ViewCone(this.Game);
                this.anotherway = new Path();
                this.texture = Game.Content.Load<Texture2D>(@"Characters\robot");
                this.health = 100;
                this.speed = 6.0f;
                this.isStatSet = true;
            }

            base.Update(gameTime);
        }
    }
}
