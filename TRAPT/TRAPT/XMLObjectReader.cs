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
using System.Xml.Linq;


namespace TRAPT
{

    public class XMLObjectReader //: Microsoft.Xna.Framework.GameComponent
    {
        LinkedList<Enemy> agentList = new LinkedList<Enemy>();
        Enemy agent;

        public Game Game { get; set; }

        public LinkedList<Enemy> AgentList
        {
            get { return agentList; }
        }

        public XMLObjectReader(Game game)
        {
            Game = game;
        }


      
        public void populateEnemiesFromXML(string xmlName)
        {
            XDocument xDoc = new XDocument();
            //string filePath = "C:/Users/Jason/Documents/Visual Studio 2010/Projects/Trapt_AI_test/Trapt_AI_test/Trapt_AI_test/EnemiesToPopulate.xml";
            string filePath = Game.Content.RootDirectory + @"\"+ xmlName +".xml";
            xDoc = XDocument.Load(filePath);
            Enemy agent = null;

            //Creating a XDocument from a XML File
            var enemy = from i in xDoc.Descendants("Enemy") select i;                                  //Making a query
            foreach (var i in enemy)
            {
                Console.WriteLine("I am here");
                agent = new Enemy(Game);
                var nodeList = from node in i.Descendants("node") select node;
                foreach (var node in nodeList)
                {
                    int x = (int)node.Element("x") * TraptMain.GRID_CELL_SIZE;
                    int y = (int)node.Element("y") * TraptMain.GRID_CELL_SIZE;
                    int dwell = (int)node.Element("dwell");

                    PathNode tempNode = new PathNode(x, y, dwell);
                    agent.addPathNode(tempNode);
                }
                agent.Initialize();
                agentList.AddLast(agent);
            }

            Console.WriteLine(agentList.Count);
            foreach (Enemy a in agentList)
            {

                Console.WriteLine(a);
            }
        }
    }
}
