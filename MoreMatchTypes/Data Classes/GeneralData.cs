using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreMatchTypes.Data_Classes
{
    public abstract class GeneralData
    {
        private RefereeInfo referee;
        public RefereeInfo Referee { get => referee; set => referee = value; }

        private String venue;
        public String Venue { get => venue; set => venue = value; }

        private RingInfo ring;
        public RingInfo Ring { get => ring; set => ring = value; }

        private uint speed;
        public uint Speed { get => speed; set => speed = value; }

        private String matchBGM;
        public String MatchBGM { get => matchBGM; set => matchBGM = value; }

        private String difficulty;
        public String Difficulty { get => difficulty; set => difficulty = value; }

        protected String reportFolder = "./EGOData/Reports";
    }
}
