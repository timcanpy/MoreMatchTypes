using DG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatchConfig;
using UnityEngine;
using ModPack;

namespace MoreMatchTypes.Wrestling_Match_Types
{
    #region Access Modifiers
    [FieldAccess(Class = "Referee", Field = "Refereeing_OutOfRingCount", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "Referee", Field = "SetFree", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "Referee", Field = "Process_FallCount", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "Referee", Field = "Process_SubmissionCheck", Group = "MoreMatchTypes")]
    #endregion
    class LuchaTag
    {

        #region Variables
        public static bool isLuchaTag;
        public static int[] points;
        public static int playerCount;
        public static int countLimit;
        public static int modifier;
        public static bool is2Falls;
        public static String[] teamNames;
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

            if (playerCount < 4 || settings.BattleRoyalKind != BattleRoyalKindEnum.Off || !MoreMatchTypes_Form.moreMatchTypesForm.cb_luchaTag.Checked)
            {
                return;
            }
            else
            {
                countLimit = 0;
                is2Falls = MoreMatchTypes_Form.moreMatchTypesForm.cb_luchaFalls.Checked;
                isLuchaTag = true;
                settings.isOutOfRingCount = true;
                settings.isTornadoBattle = false;

                //Determine modifier for Lucha Tags
                if (playerCount > 4)
                {
                    modifier = 1;
                }

                //Both teams begin with one point; next fall wins.
                if (!is2Falls)
                {
                    points[0] = 1;
                    points[1] = 1;
                }

                teamNames = new String[2];
                SetTeamNames();
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

                //Checking the legal man still inside the ring
                Player legalBlue = PlayerMan.inst.GetPlObj(MatchConfiguration.GetLegalMan(CornerSide.Blue));
                Player legalRed = PlayerMan.inst.GetPlObj(MatchConfiguration.GetLegalMan(CornerSide.Red));
                Player legalInRing = IsInRing(legalBlue) ? legalBlue : legalRed;
                
                //New condition, if the legal man inside the ring is running then the auto-tag cannot be made
                if (((referee.RefeCount >= countLimit && legalInRing.State != PlStateEnum.Run && legalInRing.State != PlStateEnum.RopeRebound)
                     || NoLegalManInRing())
                    && GetPlayersOutsideRing() != playerCount)
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

        //[Hook(TargetClass = "Referee", TargetMethod = "Process_FallCount", InjectionLocation = 42,
        //    InjectFlags = HookInjectFlags.ModifyReturn | HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        //public static bool ProcessPin(Referee r)
        //{
        //    if (!isLuchaTag)
        //    {
        //        return false;
        //    }
        //    return CheckMatchEnd(r);
        //}

        //[Hook(TargetClass = "Referee", TargetMethod = "Process_SubmissionCheck", InjectionLocation = 10,
        //    InjectFlags = HookInjectFlags.ModifyReturn | HookInjectFlags.PassInvokingInstance, InjectDirection = HookInjectDirection.Before,
        //    Group = "MoreMatchTypes")]
        //public static bool ProcessSubmission(Referee r)
        //{
        //    Player plObj = global::PlayerMan.inst.GetPlObj(r.TargetPlIdx);
        //    if (!plObj)
        //    {
        //        return false ;
        //    }
        //    if (r.State != global::RefeStateEnum.CheckSubmission)
        //    {
        //        return false;
        //    }

        //    int disturbingPlayer = global::PlayerMan.inst.GetDisturbingPlayer();
        //    if (disturbingPlayer < 0 && plObj.isWannaGiveUp)
        //    {
        //        return CheckMatchEnd(r);
        //    }

        //    return false;
        //}

        //[Hook(TargetClass = "MatchMain", TargetMethod = "ProcessMatchEnd_Draw", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        //public static void SetVictoryConditions()
        //{
        //    if (!isLuchaTag)
        //    {
        //        return;
        //    }

        //    MatchMain main = MatchMain.inst;
        //    Referee matchRef = RefereeMan.inst.GetRefereeObj();
        //    if (main.isTimeUp)
        //    {
        //        if (points[0] == points[1])
        //        {
        //            return;
        //        }
        //        else
        //        {
        //            PlayerMan p = PlayerMan.inst;
        //            Announcer.inst.PlayGong_MatchEnd();

        //            if (points[0] > points[1])
        //            {
        //                matchRef.SentenceLose(p.GetPlObj(4).PlIdx);
        //                SetLosers(MatchConfiguration.GetLegalMan(CornerSide.Red), p);
        //            }
        //            else
        //            {
        //                matchRef.SentenceLose(p.GetPlObj(0).PlIdx);
        //                SetLosers(MatchConfiguration.GetLegalMan(CornerSide.Blue), p);
        //            }
        //        }
        //    }
        //}

        [Hook(TargetClass = "Menu_Result", TargetMethod = "Set_FinishSkill", InjectionLocation = 0, InjectDirection = HookInjectDirection.After, InjectFlags = HookInjectFlags.PassParametersRef, Group = "MoreMatchTypes")]
        public static void SetResultScreenDisplay(ref string str)
        {
            if (!isLuchaTag || !MatchMain.inst.isMatchEnd || str.Contains("K.O.") || !is2Falls)
            {
                return;
            }

            string result = teamNames[0] + ": " + points[0] + " points\n" + teamNames[1] + ": " + points[1] + " points\n\n";
            if (points[0] == points[1])
            {
                result += "Draw";
            }
            else
            {
                result += "Winner - " + (points[0] > points[1] ? teamNames[0] : teamNames[1]);
            }

            string resultString = "Lucha Tag Match\n\n" + result;
            str = resultString;
        }

        #region Helper Methods
        public static void SetTeamNames()
        {
            PlayerMan p = PlayerMan.inst;
            int playerCount = MatchConfiguration.GetPlayerCount();

            //Set-up if only two wrestlers exist
            if (playerCount == 2)
            {
                teamNames[0] = DataBase.GetWrestlerFullName(PlayerMan.inst.GetPlObj(0).WresParam);
                teamNames[1] = DataBase.GetWrestlerFullName(PlayerMan.inst.GetPlObj(4).WresParam);
                return;
            }

            //Set-up for if multi-man teams exist
            try
            {
                //Get Team One Members
                List<String> wrestlers = new List<String>();
                wrestlers.Clear();

                for (int i = 0; i < 4; i++)
                {
                    Player plObj = PlayerMan.inst.GetPlObj(i);
                    if (!plObj)
                    {
                        continue;
                    }
                    if (!plObj.isSecond && !plObj.isSleep && !plObj.isIntruder)
                    {
                        wrestlers.Add(DataBase.GetWrestlerFullName(plObj.WresParam));
                    }
                }

                //Determine if a team is necessary
                if (wrestlers.Count == 1)
                {
                    teamNames[0] = wrestlers[0];
                }
                else
                {
                    teamNames[0] = MatchConfiguration.GetTeamName(wrestlers, SideCornerPostEnum.Left);
                }

                //Get Team Two Members
                wrestlers.Clear();

                for (int i = 4; i < 8; i++)
                {
                    Player plObj = PlayerMan.inst.GetPlObj(i);
                    if (!plObj)
                    {
                        continue;
                    }
                    if (!plObj.isSecond && !plObj.isSleep && !plObj.isIntruder)
                    {
                        wrestlers.Add(DataBase.GetWrestlerFullName(plObj.WresParam));
                    }
                }

                //Determine if a team is necessary
                if (wrestlers.Count == 1)
                {
                    teamNames[1] = wrestlers[0];
                }
                else
                {
                    teamNames[1] = MatchConfiguration.GetTeamName(wrestlers, SideCornerPostEnum.Right);
                }
            }
            catch
            {
                teamNames[0] = "Blue Team";
                teamNames[1] = "Red Team";
            }

        }
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
                points[1] = 2;
                return false;
            }
            else if (loser == 4)
            {
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
            }
            else if (loser > 4)
            {
                points[0]++;
            }

            if (points[0] >= 2 || points[1] >= 2)
            {
                return false;
            }

            //Signal that the current round has ended
            Announcer.inst.PlayGong_Eliminated();
            r.SetFree();

            PlayerMan.inst.GetPlObj(loser).isLoseAndStop = false;

            SetLegalMen("blue");
            SetLegalMen("red");

            MatchConfiguration.ShowAnnouncement("Score -\t" + teamNames[0] + ": " + points[0] + "\t\t" + teamNames[1] + ": " + points[1], 300);
            return true;
        }
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
            Player blueMan = PlayerMan.inst.GetPlObj(MatchConfiguration.GetLegalMan(CornerSide.Blue));
            Player redMan = PlayerMan.inst.GetPlObj(MatchConfiguration.GetLegalMan(CornerSide.Red));

            //Process Blue Corner
            if (!IsInRing(blueMan) || forceSwitch.Equals("blue"))
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
            if (!IsInRing(redMan) || forceSwitch.Equals("red"))
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
            return MatchConfiguration.GetPlayerCount();
        }
        private static int GetPlayersOutsideRing()
        {
            int result = 0;
            for (int i = 0; i < 8; i++)
            {
                Player pl = PlayerMan.inst.GetPlObj(i);

                //Ignore if this spot is empty.
                if (!pl)
                {
                    continue;
                }

                if (pl.isSecond || pl.isIntruder)
                {
                    continue;
                }

                if (!IsInRing(pl))
                {
                    result++;
                }
            }

            return result;
        }
        #endregion
    }
}
