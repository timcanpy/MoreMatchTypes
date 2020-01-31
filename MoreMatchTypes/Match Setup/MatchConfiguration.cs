using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DG;
using MoreMatchTypes.Data_Classes;

namespace MatchConfig
{
    public static class MatchConfiguration
    {
        //Ensure we aren't fielding duplicates
        public static String singleOpponent = "";
        public static String tagOpponent = "";

        public static MatchSetting AddPlayers(bool entry, WrestlerID wrestlerNo, int slot, int control, bool isSecond, int costume, MatchSetting settings)
        {
            settings.matchWrestlerInfo[slot].entry = entry;

            if (entry)
            {
                settings.matchWrestlerInfo[slot].wrestlerID = wrestlerNo;
                settings.matchWrestlerInfo[slot].costume_no = costume;
                settings.matchWrestlerInfo[slot].alignment = WrestlerAlignmentEnum.Neutral;

                //Determine what the assigned pad should be
                MenuPadKind controller;
                switch (control)
                {
                    case 0:
                        settings.matchWrestlerInfo[slot].assignedPad = PadPort.AI;
                        controller = MenuPadKind.COM;
                        break;
                    case 1:
                        settings.matchWrestlerInfo[slot].assignedPad = PadPort.Pad1;
                        controller = MenuPadKind.Pad1;
                        break;

                    case 2:
                        settings.matchWrestlerInfo[slot].assignedPad = PadPort.Pad2;
                        controller = MenuPadKind.Pad2;
                        break;

                    default:
                        settings.matchWrestlerInfo[slot].assignedPad = PadPort.AI;
                        controller = MenuPadKind.COM;
                        break;
                }

                settings.matchWrestlerInfo[slot].HP = 65535f;
                settings.matchWrestlerInfo[slot].SP = 65535f;
                settings.matchWrestlerInfo[slot].HP_Neck = 65535f;
                settings.matchWrestlerInfo[slot].HP_Arm = 65535f;
                settings.matchWrestlerInfo[slot].HP_Waist = 65535f;
                settings.matchWrestlerInfo[slot].HP_Leg = 65535f;


                GlobalParam.Set_WrestlerData(slot, controller, wrestlerNo, isSecond, costume, 65535f, 65535f, 65535f, 65535f, 65535f, 65535f);
            }
            else
            {
                settings.matchWrestlerInfo[slot].wrestlerID = global::WrestlerID.Invalid;
                GlobalParam.Set_WrestlerData(slot, MenuPadKind.Invalid, global::WrestlerID.Invalid, false, 0, 65535f, 65535f, 65535f, 65535f, 65535f, 65535f);
            }
            return settings;
        }

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
            List<String> venues = new List<String>
            {
                "Big Garden Arena",
                "SCS Stadium",
                "Arena De Universo",
                "Spike Dome",
                "Yurakuen Hall",
                 "Dojo"
        };

            //Ensure that the new venues only show up for users that own Entrance Craft
            if (SaveData.inst.IsDLCInstalled(DLCEnum.EntranceCreate))
            {
                venues.Add("Takafumi City Gym");
                venues.Add("Sakae Outdoor Ring");
                venues.Add("USA Grand Dome");
            }

            return venues.ToArray();
        }

        public static List<RingInfo> LoadRings()
        {
            List<RingInfo> rings = new List<RingInfo>
            {
                new RingInfo
                {
                    SaveID = -1,
                    Name = "SWA"
                }
            };
            foreach (RingData ring in SaveData.GetInst().editRingData)
            {
                RingInfo ringInfo = new RingInfo();
                ringInfo.SaveID = (int)ring.editRingID;
                ringInfo.Name = ring.name;
                rings.Add(ringInfo);
            }
            return rings;
        }

        public static List<RefereeInfo> LoadReferees()
        {
            List<RefereeInfo> referees = new List<RefereeInfo>();
            foreach (RefereeData referee in SaveData.GetInst().editRefereeData)
            {
                RefereeInfo refereeInfo = new RefereeInfo
                {
                    SaveID = (int)referee.editRefereeID,
                    Name = referee.Prm.name
                };
                referees.Add(refereeInfo);
            }

            //Ensure that referee list is never empty
            if (referees.Count == 0)
            {
                referees.Add(new RefereeInfo
                {
                    SaveID = -1,
                    Name = "Mr. Judgement"
                });
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
                    ID = (Int32)current.editWrestlerID,
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

        public static int GetPlayerCount()
        {
            int players = 0;
            MatchSetting matchSetting = global::GlobalWork.inst.MatchSetting;
            for (int i = 0; i < 8; i++)
            {
                Player plObj = PlayerMan.inst.GetPlObj(i);

                if (!plObj)
                {
                    continue;
                }

                if (matchSetting.matchWrestlerInfo[plObj.PlIdx].isSecond || matchSetting.matchWrestlerInfo[plObj.PlIdx].isIntruder)
                {
                    continue;
                }

                players++;
            }

            return players;
        }

    }
}
