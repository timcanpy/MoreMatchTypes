using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatchConfig
{
    public class WresIDGroup
    {
        public string Name;

        public int ID;

        public int Group;

        public SubscribeItemInfo Info;

        public override string ToString()
        {
            return this.Name;
        }
    }
}
