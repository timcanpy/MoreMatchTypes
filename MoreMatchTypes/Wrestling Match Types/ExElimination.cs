using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreMatchTypes.Wrestling_Match_Types
{
    class ExElimination
    {
    }
}

//private void InitPlayer()
//{
//global::MatchSetting matchSetting = global::GlobalWork.inst.MatchSetting;
//    for (int i = 0; i < 8; i++)
//{
//    global::Player plObj = global::PlayerMan.inst.GetPlObj(i);
//    if (plObj)
//    {
//        global::MatchWrestlerInfo matchWrestlerInfo = matchSetting.matchWrestlerInfo[i];
//        if (matchWrestlerInfo.entry)
//        {
//            plObj.HP = matchWrestlerInfo.HP;
//            plObj.SP = matchWrestlerInfo.SP;
//            plObj.HP_Neck = matchWrestlerInfo.HP_Neck;
//            plObj.HP_Arm = matchWrestlerInfo.HP_Arm;
//            plObj.HP_Waist = matchWrestlerInfo.HP_Waist;
//            plObj.HP_Leg = matchWrestlerInfo.HP_Leg;
//            plObj.royalRambleAppearTime = matchWrestlerInfo.royalRambleAppearTime;
//            plObj.WresParam = matchWrestlerInfo.param;
//            plObj.BP = 65535f;
//            plObj.WrsDP = 65535f;
//            plObj.PlPos.x = global::MatchData.PlayerInitialPosTbl[i].x;
//            plObj.PlPos.y = global::MatchData.PlayerInitialPosTbl[i].y;
//            plObj.PlPos.z = 1f;
//            plObj.downMoveTimes = 3;
//            plObj.Alignment = matchWrestlerInfo.alignment;
//            plObj.spSkillFlags |= global::MatchData.ConvSpSkillFlagTbl[(int)plObj.WresParam.specialSkill];
//            plObj.finishMove_Atk.Clear();
//            plObj.finishMove_Def[0].Clear();
//            plObj.finishMove_Def[1].Clear();
//            if (i < 4)
//            {
//                plObj.MyGroupPlIdx_Start = 0;
//                plObj.MyGroupPlIdx_End = 4;
//                plObj.EnemyGroupPlIdx_Start = 4;
//                plObj.EnemyGroupPlIdx_End = 8;
//                plObj.finishWewstlerID = matchSetting.matchWrestlerInfo[4].wrestlerID;
//            }
//            else
//            {
//                plObj.MyGroupPlIdx_Start = 4;
//                plObj.MyGroupPlIdx_End = 8;
//                plObj.EnemyGroupPlIdx_Start = 0;
//                plObj.EnemyGroupPlIdx_End = 4;
//                plObj.finishWewstlerID = matchSetting.matchWrestlerInfo[0].wrestlerID;
//            }
//        }
//    }
//}
//}
