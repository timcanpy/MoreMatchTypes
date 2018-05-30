using System;
using System.Collections;

namespace NewMatchTypes
{
    public interface IMatchType
    {
        //Method Declaration
        public void GetPlayerList();
        public void GetMatchRules();
        public void SetMatchRestrictions(Player p);
        public void SetVictoryCondtions(IEnumerable<T> settings);


    }
}
