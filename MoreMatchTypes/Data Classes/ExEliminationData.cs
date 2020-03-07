using MoreMatchTypes.Data_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatchConfig;
using System.IO;

namespace MoreMatchTypes.Data_Classes
{
    public class ExEliminationData : GeneralData
    {

        private List<WresIDGroup> blueTeamMembers;

        public List<WresIDGroup> BlueTeamMembers
        {
            get { return blueTeamMembers; }
            set { blueTeamMembers = value; }
        }

        private List<WresIDGroup> redTeamMembers;

        public List<WresIDGroup> RedTeamMembers
        {
            get { return redTeamMembers; }
            set { redTeamMembers = value; }
        }

        private String[] teamNames;

        public String[] TeamNames
        {
            get { return teamNames; }
            set { teamNames = value; }
        }

        private bool controlBlue;

        public bool ControlBlue
        {
            get { return controlBlue; }
            set { controlBlue = value; }
        }

        private bool controlRedBoolean;

        public bool ControlRed
        {
            get { return controlRedBoolean; }
            set { controlRedBoolean = value; }
        }



        public ExEliminationData()
        {
            InProgress = false;
            blueTeamMembers = new List<WresIDGroup>();
            redTeamMembers = new List<WresIDGroup>();
            teamNames = new String[2];
        }
    }
}
