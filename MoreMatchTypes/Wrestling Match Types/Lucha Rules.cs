using DG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MoreMatchTypes.Wrestling_Match_Types
{
    #region Access Modifiers
    [FieldAccess(Class = "Referee", Field = "Refereeing_OutOfRingCount", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "Referee", Field = "SetFree", Group = "MoreMatchTypes")]
    #endregion
    class LuchaTag
    {

        #region Variables

        public static bool isLuchaTag;
        public static int[] points;
        public static int playerCount;
        public static int countLimit;
        public static int modifier;
        #endregion

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void SetMatchRules()
        {
            isLuchaTag = false;
            points = new int[2];
            MatchSetting settings = GlobalWork.inst.MatchSetting;
            playerCount = GetPlayerCount();
            modifier = 0;

            if (playerCount < 4 || settings.BattleRoyalKind != BattleRoyalKindEnum.Off || !MoreMatchTypes_Form.form.cb_luchaTag.Checked)
            {
                return;
            }
            else
            {
                countLimit = 0;
                isLuchaTag = true;
                settings.isOutOfRingCount = true;
                settings.isTornadoBattle = false;

                //Determine modifier for Lucha Tags
                if (playerCount > 4)
                {
                    modifier = 1;
                }
            }

        }

        [Hook(TargetClass = "Referee", TargetMethod = "Refereeing_OutOfRingCount", InjectionLocation = 0,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        public static void CheckFreeTag(Referee referee)
        {
            if (!isLuchaTag || referee.RefeCount == 20)
            {
                return;
            }

            try
            {
                //Randomized for variance.
                if (countLimit == -1)
                {
                    switch (referee.RefePrm.outCount)
                    {
                        case 0:
                        case 1:
                            countLimit = 4;
                            break;
                        case 2:
                            countLimit = 5;
                            break;
                        case 3:
                        case 4:
                            countLimit = 6;
                            break;
                    }

                    //Subtract modifier from the final value
                    countLimit = countLimit - modifier < 0 ? countLimit : countLimit - modifier;
                    countLimit = UnityEngine.Random.Range(0, countLimit);
                }

                if ((referee.RefeCount >= countLimit || NoLegalManInRing()) && PlayerMan.inst.GetPlayerNum_OutOfRingCount() != playerCount)
                {
                    countLimit = -1;
                    SetLegalMen("");
                }
            }
            catch (Exception e)
            {
                L.D("Lucha Error: " + e);
            }
        }

        [Hook(TargetClass = "Referee", TargetMethod = "Process_FallCount", InjectionLocation = 42,
            InjectFlags = HookInjectFlags.ModifyReturn | HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        public static bool CheckMatchEnd(Referee r)
        {
            if (!isLuchaTag)
            {
                return false;
            }

            //Get the loser
            int loser = r.TargetPlIdx;
            //Determine if a captain has lost
            if (loser == 0)
            {
                L.D("Blue Captain lost");
                points[1] = 2;
                return false;
            }
            else if (loser == 4)
            {
                L.D("Red Captain lost");
                points[0] = 2;
                return false;
            }

            //Knocked out losers automatically end the match
            if (PlayerMan.inst.GetPlObj(loser).isKO)
            {
                if (loser < 4)
                {
                    points[1] = 2;
                }
                else
                {
                    points[0] = 2;
                }
                return false;
            }

            if (loser < 4)
            {
                points[1]++;
                L.D("Blue loses a point");
            }
            else if (loser > 4)
            {
                points[0]++;
                L.D("Red loses a point");
            }

            if (points[0] >= 2 || points[1] >= 2)
            {
                return false;
            }

            //Signal that the current round has ended
            Announcer.inst.PlayGong_Eliminated();
            r.SetFree();

            PlayerMan.inst.GetPlObj(loser).isLoseAndStop = false;

            if (loser < 4)
            {
                SetLegalMen("blue");
            }
            else
            {
                SetLegalMen("red");
            }

            DispNotification.inst.Show("Blue Team : " + points[0] + "      Red Team : " + points[1], 300);
            return true;
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "ProcessMatchEnd_Draw", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void SetVictoryConditions()
        {
            if (!isLuchaTag)
            {
                return;
            }

            MatchMain main = MatchMain.inst;
            Referee matchRef = RefereeMan.inst.GetRefereeObj();
            if (main.isTimeUp)
            {
                if (points[0] == points[1])
                {
                    return;
                }
                else
                {
                    PlayerMan p = PlayerMan.inst;
                    Announcer.inst.PlayGong_MatchEnd();

                    if (points[0] > points[1])
                    {
                        matchRef.SentenceLose(p.GetPlObj(4).PlIdx);
                        SetLosers(4, p);
                    }
                    else
                    {
                        matchRef.SentenceLose(p.GetPlObj(0).PlIdx);
                        SetLosers(0, p);
                    }
                }
            }
        }

        [Hook(TargetClass = "Menu_Result", TargetMethod = "Set_FinishSkill", InjectionLocation = 8, InjectDirection = HookInjectDirection.After, InjectFlags = HookInjectFlags.PassParametersVal | HookInjectFlags.PassLocals, LocalVarIds = new int[] { 1 }, Group = "MoreMatchTypes")]
        public static void SetResultScreenDisplay(ref UILabel finishText, string str)
        {
            if (!isLuchaTag || !MatchMain.inst.isMatchEnd || finishText.text.Contains("K.O."))
            {
                return;
            }

            string result = "Blue Team: " + points[0] + " points\nRed Team: " + points[1] + " points\n\n";
            if (points[0] == points[1])
            {
                result += "Draw";
            }
            else
            {
                result += "Winner - " + (points[0] > points[1] ? "Blue Team" : "Red Team");
            }

            string resultString = "Lucha Tag Match\n\n" + result;
            finishText.text = resultString;
        }

        #region Helper Methods
        public static void SetLosers(int startIndex, PlayerMan p)
        {
            try
            {
                for (int i = startIndex; i < startIndex + 4; i++)
                {
                    if (p.GetPlObj(i))
                    {
                        p.GetPlObj(i).isLoseAndStop = true;
                    }
                }
            }
            catch
            {
                return;
            }

        }
        private static bool NoLegalManInRing()
        {
            bool result = true;
            for (int i = 0; i < 8; i++)
            {
                Player pl = PlayerMan.inst.GetPlObj(i);

                //Ignore if this spot is empty.
                if (!pl)
                {
                    continue;
                }

                if (!pl.hasRight)
                {
                    continue;
                }
                else
                {
                    if (IsInRing(pl))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }
        private static bool IsInRing(Player pl)
        {
            return pl.Zone == ZoneEnum.Apron || pl.Zone == ZoneEnum.OnCornerPost || pl.Zone == ZoneEnum.InRing;
        }
        private static void SetLegalMen(String forceSwitch)
        {
            Player blueMan = PlayerMan.inst.GetPlObj(GetLegalMan("blue"));
            Player redMan = PlayerMan.inst.GetPlObj(GetLegalMan("red"));

            //Process Blue Corner
            if (!IsInRing(blueMan) && !forceSwitch.Equals("blue"))
            {
                Player nextPlayer = PlayerMan.inst.GetPlObj(GetNextTag("blue", blueMan.PlIdx));

                blueMan.hasRight = false;
                nextPlayer.hasRight = true;
                blueMan.TagStandbyPos = nextPlayer.TagStandbyPos;
                nextPlayer.isTagPartnerStandby = false;
                nextPlayer.Start_ForceControl(ForceCtrlEnum.GoBackToRing);
                blueMan.Start_ForceControl(ForceCtrlEnum.GoBackToApron);
            }

            //Process Red Corner
            if (!IsInRing(redMan) && !forceSwitch.Equals("red"))
            {
                Player nextPlayer = PlayerMan.inst.GetPlObj(GetNextTag("red", redMan.PlIdx));

                redMan.hasRight = false;
                nextPlayer.hasRight = true;
                redMan.TagStandbyPos = nextPlayer.TagStandbyPos;
                nextPlayer.isTagPartnerStandby = false;
                nextPlayer.Start_ForceControl(ForceCtrlEnum.GoBackToRing);
                redMan.Start_ForceControl(ForceCtrlEnum.GoBackToApron);
            }
        }
        private static int GetLegalMan(String corner)
        {
            int legalMan = 0;
            switch (corner.ToLower())
            {
                case "blue":
                    for (int i = 0; i < 4; i++)
                    {
                        Player pl = PlayerMan.inst.GetPlObj(i);

                        //Ignore if this spot is empty.
                        if (!pl)
                        {
                            continue;
                        }

                        if (pl.hasRight)
                        {
                            legalMan = pl.PlIdx;
                            break;
                        }
                    }

                    break;

                case "red":
                    {
                        for (int i = 4; i < 8; i++)
                        {
                            Player pl = PlayerMan.inst.GetPlObj(i);

                            //Ignore if this spot is empty.
                            if (!pl)
                            {
                                continue;
                            }

                            if (pl.hasRight)
                            {
                                legalMan = pl.PlIdx;
                                break;
                            }
                        }

                        break;
                    }
            }

            return legalMan;
        }
        private static int GetNextTag(String corner, int legalMan)
        {
            switch (corner.ToLower())
            {
                case "blue":
                    for (int i = 0; i < 4; i++)
                    {
                        if (i == legalMan)
                        {
                            continue;
                        }

                        Player pl = PlayerMan.inst.GetPlObj(i);

                        //Ignore if this spot is empty.
                        if (!pl)
                        {
                            continue;
                        }

                        if (pl.hasRight)
                        {
                            continue;
                        }
                        else if (IsInRing(pl))
                        {
                            legalMan = i;
                            break;
                        }
                    }
                    break;
                case "red":
                    for (int i = 4; i < 8; i++)
                    {
                        if (i == legalMan)
                        {
                            continue;
                        }

                        Player pl = PlayerMan.inst.GetPlObj(i);

                        //Ignore if this spot is empty.
                        if (!pl)
                        {
                            continue;
                        }

                        if (pl.hasRight)
                        {
                            continue;
                        }
                        else if (IsInRing(pl))
                        {
                            legalMan = i;
                            break;
                        }
                    }
                    break;
            }

            return legalMan;
        }
        private static int GetPlayerCount()
        {
            int count = 0;

            for (int i = 0; i < 8; i++)
            {
                Player pl = PlayerMan.inst.GetPlObj(i);

                //Ignore if this spot is empty.
                if (!pl)
                {
                    continue;
                }

                if (!pl.isSecond)
                {
                    count++;
                }
            }

            return count;
        }
        #endregion
    }
}
