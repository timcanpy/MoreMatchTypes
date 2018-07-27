using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG;
using DG.DG;

namespace MoreMatchTypes.Helper_Classes
{
    public static class MatchConfiguration
    {
        public static MatchSetting AddPlayers(bool entry, WrestlerID wrestlerNo, int slot, int control, bool isSecond, int costume, MatchSetting settings)
        {
            settings.matchWrestlerInfo[slot].entry = entry;

            if (entry)
            {
                settings.matchWrestlerInfo[slot].wrestlerID = wrestlerNo;
                settings.matchWrestlerInfo[slot].costume_no = costume;
                settings.matchWrestlerInfo[slot].alignment = WrestlerAlignmentEnum.Neutral;

                //Determine what the assigned pad should be
                switch (control)
                {
                    case 0:
                        settings.matchWrestlerInfo[slot].assignedPad = PadPort.AI;
                        break;
                    case 1:
                        settings.matchWrestlerInfo[slot].assignedPad = PadPort.Pad1;
                        break;

                    case 2:
                        settings.matchWrestlerInfo[slot].assignedPad = PadPort.Pad2;
                        break;

                    default:
                        settings.matchWrestlerInfo[slot].assignedPad = PadPort.AI;
                        break;
                }

                settings.matchWrestlerInfo[slot].HP = 65535f;
                settings.matchWrestlerInfo[slot].SP = 65535f;
                settings.matchWrestlerInfo[slot].HP_Neck = 65535f;
                settings.matchWrestlerInfo[slot].HP_Arm = 65535f;
                settings.matchWrestlerInfo[slot].HP_Waist = 65535f;
                settings.matchWrestlerInfo[slot].HP_Leg = 65535f;

                GlobalParam.Set_WrestlerData(slot, control, wrestlerNo, isSecond, costume, 65535f, 65535f, 65535f, 65535f, 65535f, 65535f);
            }
            else
            {
                settings.matchWrestlerInfo[slot].wrestlerID = global::WrestlerID.Invalid;
                GlobalParam.Set_WrestlerData(slot, -1, global::WrestlerID.Invalid, false, 0, 65535f, 65535f, 65535f, 65535f, 65535f, 65535f);
            }
            return settings;
        }

        public static WrestlerID GetWrestlerNo(String wrestlerData)
        {
            String[] wrestlerName = wrestlerData.Split(':');
            return (WrestlerID)Int32.Parse(wrestlerName[wrestlerName.Length - 1]);
        }

        public static String GetWrestlerName(String wrestlerData)
        {
            String[] wrestlerName = wrestlerData.Split(':');
            return wrestlerName[0];
        }
        
    }
}
