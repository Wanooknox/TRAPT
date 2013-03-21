using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TRAPT
{
    /// <summary>
    /// Class to create a collection storable reference to a game component
    /// </summary>
    public class GameComponentRef : IComparable<GameComponentRef>
    {
        public EnvironmentObj item;

        public GameComponentRef(ref EnvironmentObj item)
        {
            this.item = item;
        }

        public override bool Equals(object obj)
        {
            bool result = false;
            if (obj is GameComponentRef)
            {
                result = this.item.Equals(((GameComponentRef)obj).item);
            }
            return result;
        }

        public int CompareTo(GameComponentRef obj)
        {
            //if this equals obj return 0 otherwise 1
            return (this.item.Equals(obj.item)) ? 0 : 1;
        }
    }
}
