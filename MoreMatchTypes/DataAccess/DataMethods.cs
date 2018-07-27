using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreMatchTypes.DataAccess
{
    public class DataMethods
    {
        public static int UpdateSurvivalData(String Wrestler, int matches, int losses, int continues, int wins, int maxRating, int avgRating)
        {
            try
            {
                return 0;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }
    }
}
