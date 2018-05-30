namespace Interfaces
{
    public interface IMatchType
    {
        //Method Declaration
        bool SetMatchRestrictions(Player p);
        void SetVictoryConditions(Player p);
        void SetResultScreenDisplay(string str, UILabel finishText);
        void SetMatchRules();
    }
}
