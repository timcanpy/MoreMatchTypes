using MoreMatchTypes.Data_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatchConfig;
using System.IO;
using MoreMatchTypes.Data_Classes.Storage;
using Newtonsoft.Json;
using DG;

namespace MoreMatchTypes.DataClasses
{
    public class SurvivalRoadData : GeneralData
    {
        #region Variables
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

        //Required for tracking purposes after the first match begins
        private WresIDGroup[] initialOpponents;
        public WresIDGroup[] InitialOpponents { get => initialOpponents; set => initialOpponents = value; }
        #endregion

        public SurvivalRoadData()
        {
            InProgress = false;
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

        /*Array to store match details
  [0] - Matches Remaining
  [1] - Continues Remaining
  [2] - Current Win Streak
  [3] - Highest Win Streak
  [4] - Total Losses
  [5] - Continues Used
  [6] - Higest Match Rating
  [7] - Total Match Rating
  [8] - Matches Played
  */

        public bool SaveSurvivalData(int[] gameDetails)
        {
            try
            {
                SurvivalSaveData data = new SurvivalSaveData();
                data = new SurvivalSaveData { OwnerID = "" + gameDetails[8] + gameDetails[4] + gameDetails[5] + gameDetails[7], Date = DateTime.Now.ToString("dd-MM-yyyy hh:mm tt"), Details = matchProgress };

                if (Ring != null)
                {
                    data.Promotion = Ring.Name;
                }

                if (wrestler == null)
                {
                    data.Name = "";
                }
                else
                {
                    data.Name = wrestler.Name;
                }

                if (second != null)
                {
                    if (second.Name != null)
                    {
                        data.Name += " & " + second.Name;
                    }
                }

                data.ID = data.Name + data.Date + data.OwnerID;
                data.Record = gameDetails[8] + "\\" + gameDetails[4] + "\\" + gameDetails[5];
                data.MaxWinStreak = gameDetails[3];
                data.MaxRating = gameDetails[6];

                #region Create json information
                StringWriter stringWriter = new StringWriter();
                JsonTextWriter writer = new JsonTextWriter(stringWriter);

                writer.WriteStartObject();

                writer.WritePropertyName("id");
                writer.WriteValue(data.ID);

                writer.WritePropertyName("name");
                writer.WriteValue(data.Name);

                writer.WritePropertyName("promotion");
                writer.WriteValue(data.Promotion);

                writer.WritePropertyName("ownerId");
                writer.WriteValue(data.OwnerID);

                writer.WritePropertyName("date");
                writer.WriteValue(data.Date);

                writer.WritePropertyName("record");
                writer.WriteValue(data.Record);

                writer.WritePropertyName("maxWinStreak");
                writer.WriteValue(data.MaxWinStreak);

                writer.WritePropertyName("maxRating");
                writer.WriteValue(data.MaxRating);

                writer.WritePropertyName("details");
                writer.WriteStartArray();

                foreach (string detail in data.Details)
                {
                    writer.WriteValue(detail);
                }
                writer.WriteEndArray();

                writer.WriteEndObject();
                #endregion

                //Create Local File
                string fileName = wrestler.Name;
                if (second != null)
                {
                    if (second.Name != null)
                    {
                        fileName += "_" + second.Name;
                    }
                }

                fileName += "_" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-tt") + ".json";

                if (!Directory.Exists(reportFolder))
                {
                    Directory.CreateDirectory(reportFolder);
                }
                File.WriteAllText(Path.Combine(reportFolder, fileName), stringWriter.ToString());

                return true;
            }
            catch (Exception e)
            {
                L.D("SaveSurvivalDataError: " + e);
                return false;
            }
        }
    }
}
