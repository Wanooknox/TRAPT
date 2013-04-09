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

        LinkedList<Obstacle> barrierList = new LinkedList<Obstacle>();
        Obstacle barrier;

        LinkedList<ObstacleSwitch> switchList = new LinkedList<ObstacleSwitch>();
        ObstacleSwitch Switch;

        public Game Game { get; set; }

        public LinkedList<Enemy> AgentList
        {
            get { return agentList; }
        }

        public LinkedList<Obstacle> BarrierList
        {
            get { return barrierList; }
        }

        public LinkedList<ObstacleSwitch> SwitchList
        {
            get { return switchList; }
        }

        public XMLObjectReader(Game game)
        {
            Game = game;
        }

        public void placeSwitchAndBarrier(string xmlName)
        {
            string texStr = "Switches_3";
            string texStr1 = "Barriers_1";
            int texBarrierPos = 0;
            int texSwitchPos = 1;
            XDocument xDoc = new XDocument();
            string filePath = Game.Content.RootDirectory + @"\" + xmlName + ".xml";
            xDoc = XDocument.Load(filePath);

            Obstacle barrier = null;
            Vector2 posVec = new Vector2();
            var barriers = from b in xDoc.Descendants("Barrier") select b;
            foreach (var b in barriers)
            {
                barrier = new Obstacle(Game);
                int connection = (int)b.Element("connection");
                int texPosistion = (int)b.Element("texPosistion");
                var posistions = from pos in b.Descendants("posistion") select pos;
                foreach (var pos in posistions)
                {
                    int x = (int)pos.Element("x") * TraptMain.GRID_CELL_SIZE;
                    int y = (int)pos.Element("y") * TraptMain.GRID_CELL_SIZE;
                    posVec = new Vector2(x, y);
                }
                barrier.Initialize(posVec, texStr1, texBarrierPos);
                barrier.Connection = connection;
                barrier.TexPosistion = texPosistion;
                Console.WriteLine("Something");
                barrierList.AddLast(barrier);
                Console.WriteLine(barrierList);
                Console.WriteLine(barrierList.Count);
            }

            ObstacleSwitch Switch = null;
            var switches = from s in xDoc.Descendants("Switch") select s;
            foreach (var s in switches)
            {
                Switch = new ObstacleSwitch(Game);
                int connection = (int)s.Element("connection");
                var posistions = from pos in s.Descendants("posistion") select pos;
                foreach (var pos in posistions)
                {
                    int x = (int)pos.Element("x") * TraptMain.GRID_CELL_SIZE;
                    int y = (int)pos.Element("y") * TraptMain.GRID_CELL_SIZE;
                    Switch.Initialize(new Vector2(x, y), texStr, texSwitchPos);
                }
                LinkedList<Obstacle> obstaclesToChange = new LinkedList<Obstacle>();
                foreach (Obstacle obs in BarrierList)
                {
                    if (obs.Connection == connection)
                    {
                        obstaclesToChange.AddLast(obs);
                    }
                }
                Switch.Connection = connection;
                Switch.Obstacles = obstaclesToChange;
                switchList.AddLast(Switch);
            }
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
                    int x = (int)node.Element("x") * TraptMain.GRID_CELL_SIZE + (TraptMain.GRID_CELL_SIZE / 2);
                    int y = (int)node.Element("y") * TraptMain.GRID_CELL_SIZE + (TraptMain.GRID_CELL_SIZE / 2);
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
