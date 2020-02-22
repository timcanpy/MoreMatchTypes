using MoreMatchTypes.Data_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatchConfig;
using System.IO;

namespace MoreMatchTypes.DataClasses
{
    public class SurvivalRoadData : GeneralData
    {
        #region Variables
        //private RefereeInfo referee;
        //public RefereeInfo Referee { get => referee; set => referee = value; }

        //private String venue;
        //public String Venue { get => venue; set => venue = value; }

        //private RingInfo ring;
        //public RingInfo Ring { get => ring; set => ring = value; }

        //private uint speed;
        //public uint Speed { get => speed; set => speed = value; }

        //private String matchBGM;
        //public String MatchBGM { get => matchBGM; set => matchBGM = value; }

        //private String difficulty;
        //public String Difficulty { get => difficulty; set => difficulty = value; }

        private WresIDGroup wrestler;
        public WresIDGroup Wrestler { get => wrestler; set => wrestler = value; }

        private WresIDGroup second;
        public WresIDGroup Second { get => second; set => second = value; }

        private int continues;
        public int Continues { get => continues; set => continues = value; }

        private int matches;
        public int Matches { get => matches; set => matches = value; }

        private bool regainHP;
        public bool RegainHP { get => regainHP; set => regainHP = value; }

        private bool singles;
        public bool Singles { get => singles; set => singles = value; }

        private bool tag;
        public bool Tag { get => tag; set => tag = value; }

        private bool controlBoth;
        public bool ControlBoth { get => controlBoth; set => controlBoth = value; }

        private bool cutPlay;
        public bool CutPlay { get => cutPlay; set => cutPlay = value; }

        private String matchType;
        public string MatchType { get => matchType; set => matchType = value; }

        private bool simulate;
        public bool Simulate { get => simulate; set => simulate = value; }

        private bool controlSecond;
        public bool ControlSecond { get => controlSecond; set => controlSecond = value; }

        private List<String> matchProgress;
        public List<String> MatchProgress { get => matchProgress; set => matchProgress = value; }

        private String opponentName;
        public String OpponentName { get => opponentName; set => opponentName = value; }

        private bool randomSelect;
        public bool RandomSelect { get => randomSelect; set => randomSelect = value; }

        private List<WresIDGroup> opponents;
        public List<WresIDGroup> Opponents { get => opponents; set => opponents = value; }

        private bool inProgress;
        public bool InProgress { get => inProgress; set => inProgress = value; }

        //Required for tracking purposes after the first match begins
        private WresIDGroup[] initialOpponents;
        public WresIDGroup[] InitialOpponents { get => initialOpponents; set => initialOpponents = value; }
        #endregion

        public SurvivalRoadData()
        {
            inProgress = false;
            matchProgress = new List<String>();
            initialOpponents = new WresIDGroup[2];
        }

        public bool UpdateSurvivalData(String info)
        {
            try
            {
                matchProgress.Add(info);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SaveSurvivalData()
        {
            try
            {
                //Create file name
                string fileName = wrestler.Name;
                if (second != null)
                {
                    fileName += "_" + second.Name;
                }

                fileName += "_" + DateTime.Today.ToString("dd-MM-yyyy") + ".txt";

                Directory.CreateDirectory(reportFolder);
                File.WriteAllLines(Path.Combine(reportFolder, fileName), matchProgress.ToArray());

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
