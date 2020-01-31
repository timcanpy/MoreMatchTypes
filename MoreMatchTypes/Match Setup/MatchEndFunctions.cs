using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatchConfig
{
    public static class MatchEndFunctions
    {
        public static int GetLoser()
        {
            int loser = -1;
            Player plObj;

            //Determine which player lost
            for (int i = 0; i < 8; i++)
            {
                plObj = PlayerMan.inst.GetPlObj(i);
                if (!plObj)
                {
                    continue;
                }

                plObj.isKO = false;
                if (plObj.isLoseAndStop)
                {
                    loser = i;
                }
            }

            return loser;
        }
    }
}
