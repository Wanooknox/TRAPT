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

public class PathNode
{

    private int dwell;                                      //time the agent sits at the current node
    private Vector2 position = Vector2.Zero;


    public static int ARRIVED = 25;
    public static int WIDTH = 10;
    public static int HEIGHT = 10;

    

	public PathNode(int xPos, int yPos, int dwell )
	{
        position.X = xPos;
        position.Y = yPos;
        this.dwell = dwell;
	}

    public Vector2 getPosition()
    {
        return position;
    }

    public int getDwell()
    {
        return dwell;
    }

    public override string ToString()
    {
        return "Position: ( " + position.X + " , " + position.Y + " ), Dwell Time: " + dwell;
    }
}
