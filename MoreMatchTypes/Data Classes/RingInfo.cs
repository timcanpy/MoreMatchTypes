using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreMatchTypes.Data_Classes
{
    public class RingInfo
    {
        private int saveID;

        public int SaveID
        {
            get { return saveID; }
            set { saveID = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public override String ToString()
        {
            return Name;
        }
    }
}
