using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DG;

namespace MatchConfig
{
    public static class MatchConfiguration
    {
        //public static MatchSetting AddPlayers(bool entry, WrestlerID wrestlerNo, int slot, int control, bool isSecond, int costume, MatchSetting settings)
        //{
        //    settings.matchWrestlerInfo[slot].entry = entry;

        //    if (entry)
        //    {
        //        settings.matchWrestlerInfo[slot].wrestlerID = wrestlerNo;
        //        settings.matchWrestlerInfo[slot].costume_no = costume;
        //        settings.matchWrestlerInfo[slot].alignment = WrestlerAlignmentEnum.Neutral;

        //        //Determine what the assigned pad should be
        //        switch (control)
        //        {
        //            case 0:
        //                settings.matchWrestlerInfo[slot].assignedPad = PadPort.AI;
        //                break;
        //            case 1:
        //                settings.matchWrestlerInfo[slot].assignedPad = PadPort.Pad1;
        //                break;

        //            case 2:
        //                settings.matchWrestlerInfo[slot].assignedPad = PadPort.Pad2;
        //                break;

        //            default:
        //                settings.matchWrestlerInfo[slot].assignedPad = PadPort.AI;
        //                break;
        //        }

        //        settings.matchWrestlerInfo[slot].HP = 65535f;
        //        settings.matchWrestlerInfo[slot].SP = 65535f;
        //        settings.matchWrestlerInfo[slot].HP_Neck = 65535f;
        //        settings.matchWrestlerInfo[slot].HP_Arm = 65535f;
        //        settings.matchWrestlerInfo[slot].HP_Waist = 65535f;
        //        settings.matchWrestlerInfo[slot].HP_Leg = 65535f;

        //        GlobalParam.Set_WrestlerData(slot, control, wrestlerNo, isSecond, costume, 65535f, 65535f, 65535f, 65535f, 65535f, 65535f);
        //    }
        //    else
        //    {
        //        settings.matchWrestlerInfo[slot].wrestlerID = global::WrestlerID.Invalid;
        //        GlobalParam.Set_WrestlerData(slot, -1, global::WrestlerID.Invalid, false, 0, 65535f, 65535f, 65535f, 65535f, 65535f, 65535f);
        //    }
        //    return settings;
        //}

        public static WrestlerID GetWrestlerNo(WresIDGroup wrestler)
        {
            return (WrestlerID)wrestler.ID;
        }

        public static WresIDGroup GetWrestlerData(int id, List<WresIDGroup> wrestlerList)
        {
            WresIDGroup wrestlerData = null;
            foreach (WresIDGroup wrestler in wrestlerList)
            {
                if (wrestler.ID == id)
                {
                    L.D("Returning wrestler " + wrestler.Name);
                    wrestlerData = wrestler;
                    break;
                }
            }
            return wrestlerData;
        }

        public static String GetWrestlerName(String wrestlerData)
        {
            String[] wrestlerName = wrestlerData.Split(':');
            return wrestlerName[0];
        }

        public static uint[] LoadSpeed()
        {
            uint[] speeds = new uint[9];
            speeds[0] = 100;
            speeds[1] = 125;
            speeds[2] = 150;
            speeds[3] = 175;
            speeds[4] = 200;
            speeds[5] = 300;
            speeds[6] = 400;
            speeds[7] = 800;
            speeds[8] = 1000;
            return speeds;
        }

        public static String[] LoadDifficulty()
        {
            String[] levels = new String[10];

            for (int i = 1; i <= 10; i++)
            {
                levels[i - 1] = i.ToString();
            }

            return levels;
        }

        public static String[] LoadVenue()
        {
            String[] venues = new String[6];
            venues[0] = "Big Garden Arena";
            venues[1] = "SCS Stadium";
            venues[2] = "Arena De Universo";
            venues[3] = "Spike Dome";
            venues[4] = "Yurakuen Hall";
            venues[5] = "Dojo";

            return venues;
        }

        public static List<String> LoadRings()
        {
            List<String> rings = new List<String>
            {
                "SWA"
            };
            foreach (RingData ring in SaveData.GetInst().editRingData)
            {
                rings.Add(ring.name);
            }
            return rings;
        }

        public static List<String> LoadReferees()
        {
            List<String> referees = new List<String>
            {
                "Mr Judgement"
            };
            foreach (RefereeData referee in SaveData.GetInst().editRefereeData)
            {
                referees.Add(referee.Prm.name);
            }
            return referees;
        }

        public static List<String> LoadBGMs()
        {
            List<String> bgms = new List<String>
            {
                "Fire Pro Wrestling 2017",
                "Spinning Panther 2017",
                "Lonely Stage 2017"
            };

            string currentPath = System.IO.Directory.GetCurrentDirectory();

            IEnumerable<String> themes;
            themes = Directory.GetFiles(currentPath + @"\BGM");
            foreach (String theme in themes)
            {
                bgms.Add(theme.Replace(currentPath + @"\BGM", "").Replace(@"\", ""));
            }
            return bgms;
        }

        public static List<WresIDGroup> LoadWrestlers()
        {
            List<WresIDGroup> wrestlers = new List<WresIDGroup>();

            foreach (EditWrestlerData current in SaveData.inst.editWrestlerData)
            {
                WresIDGroup wresIDGroup = new WresIDGroup
                {
                    Name = DataBase.GetWrestlerFullName(current.wrestlerParam),
                    ID = (Int32)WrestlerID.EditWrestlerIDTop + SaveData.inst.editWrestlerData.IndexOf(current),
                    Group = current.wrestlerParam.groupID
                };
                wrestlers.Add(wresIDGroup);
            }
            return wrestlers;
        }

        public static List<String> LoadPromotions()
        {
            List<String> promotions = new List<String>();
            foreach (GroupInfo current in SaveData.GetInst().groupList)
            {
                string longName = SaveData.GetInst().organizationList[current.organizationID].longName;
                promotions.Add(longName + " : " + current.longName);
            }
            return promotions;
        }

        public static List<WresIDGroup> LoadWrestlersFromPromotion(List<WresIDGroup> wrestlerList, String promotionName, List<String> promotionList)
        {
            List<WresIDGroup> wrestlers = new List<WresIDGroup>();
            return wrestlers;
        }


    }
}
