using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DG;

namespace MoreMatchTypes
{
    #region Hooks
    [FieldAccess(Class = "MatchMain", Field = "CreatePlayers", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "MatchMain", Field = "EndMatch", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "MatchMain", Field = "InitRound", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "Referee", Field = "CheckMatchEnd", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "Announcer", Field = "Watch_Scuffle", Group = "MoreMatchTypes")]
    #endregion
    public class ExtendedElimination
    {

        #region Variables
        public static int[] wins = new int[2];
        public static string[] teamNames = new string[2];
        public static Queue<String> blueTeamMembers;
        public static Queue<String> redTeamMembers;
        public static int[] memberTrack;
        public static bool endRound = false;
        //public static bool nextMatch = false;
        public static int loserTrack;
        public static bool isExElimination;
        public static MatchTime currMatchTime;
        #endregion

        [Hook(TargetClass = "MatchMain", TargetMethod = "CreatePlayers", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void SetMatchRules()
        {
            //Ensure a valid elimination match can take place
            MatchSetting settings = GlobalWork.inst.MatchSetting;
            isExElimination = MoreMatchTypes_Form.form.cb_exElim.Checked;
            if (!isExElimination)
            {
                return;
            }

            //Check required to ensure that we can create new players on round end, without resetting our configuration
            //if (!endRound && !nextMatch)
            if(!endRound)
            {
                try
                {
                    teamNames = new string[2];
                    memberTrack = new int[2];
                    endRound = false;
                    loserTrack = 0;
                    currMatchTime = null;
                    settings.CriticalRate = CriticalRateEnum.Off;
                    settings.isOutOfRingCount = false;

                    SetTeamNames();
                    SetTeamMembers();

                }
                catch (Exception ex)
                {
                    L.D(ex.StackTrace);
                }
            }

            //if (nextMatch)
            if(endRound)
            {
                MatchMain main = MatchMain.inst;
                main.RoundCnt = 1;
                settings.is3GameMatch = false;
                settings.RoundNum = 0;
                settings.isSkipEntranceScene = true;
                settings.isCarryOverHP = true;
                settings.MatchCnt = 1;

                //Removing members from the losing team
                if (loserTrack == 0)
                {
                    blueTeamMembers.Dequeue();
                }
                else if (loserTrack == 1)
                {
                    redTeamMembers.Dequeue();
                }

                UpdateTeamMembers();
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitRound", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        public static void StartRound(MatchMain m)
        {
            if (!isExElimination)
            {
                return;
            }

            //Settings for the start of a match
            if (!endRound)
            {
                //Setting up team members to track; this will be decreased as a team loses.
                memberTrack[0] = 0;
                memberTrack[1] = 0;
                SetSeconds();
            }
            else
            {
                //nextMatch = false;
                endRound = false;
            }

            if (currMatchTime != null)
            {
                m.matchTime.Set(currMatchTime);
            }
        }

        //[Hook(TargetClass = "Referee", TargetMethod = "CheckMatchEnd_Draw", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        //public static void CheckCountOut(Referee matchRef)
        //{
        //    if (!isExElimination)
        //    {
        //        return;
        //    }

        //    MatchMain main = MatchMain.inst;
        //    if (main.isTimeUp)
        //    {
        //        return;
        //    }

        //    if (blueTeamMembers.Count == 1 && redTeamMembers.Count == 1)
        //    {
        //        return;
        //    }

        //    if (blueTeamMembers.Count == 1)
        //    {
        //        EndMatch(memberTrack[0]);
        //        return;
        //    }
        //    if (redTeamMembers.Count == 1)
        //    {
        //        EndMatch(memberTrack[0] + 4);
        //        return;
        //    }

        //    //Remove players from both sides
        //    Player plObj;
        //    plObj = PlayerMan.inst.GetPlObj(memberTrack[0]);
        //    DisplayElimination(DataBase.GetWrestlerFullName(plObj.WresParam), blueTeamMembers.Count - 1);

        //    plObj = PlayerMan.inst.GetPlObj(memberTrack[1]);
        //    DisplayElimination(DataBase.GetWrestlerFullName(plObj.WresParam), redTeamMembers.Count - 1);
        //    loserTrack = 2;
        //    EndRound();
        //}

        [Hook(TargetClass = "Referee", TargetMethod = "CheckMatchEnd", InjectionLocation = 0, InjectFlags = HookInjectFlags.ModifyReturn, Group = "MoreMatchTypes")]
        public static bool CheckMatchEnd()
        {
            if (!isExElimination)
            {
                return false;
            }
            MatchMain main = MatchMain.inst;
            if (!main.isMatchEnd)
            {
                return false;
            }
            int loser = -1;
            Player plObj;
            //Determine which player lost
            for (int i = 0; i < 8; i++)
            {
                plObj = PlayerMan.inst.GetPlObj(i);
                if (!plObj)
                {
                    continue;
                }

                plObj.isKO = false;
                if (plObj.isLoseAndStop)
                {
                    //Ensure that we ignore members that have already lost
                    if (i < 4 && i == memberTrack[0])
                    {
                        loserTrack = 0;
                        loser = i;
                        break;
                    }
                    else if (i > 3 && i == (memberTrack[1] + 4))
                    {
                        loserTrack = 1;
                        loser = i;
                        break;
                    }
                }
            }

            //There's an error if we reach this point
            if (loser == -1)
            {
                return false;
            }

            //Determine if the match has ended
            plObj = PlayerMan.inst.GetPlObj(loser);
            if (loserTrack == 0)
            {
                if (blueTeamMembers.Count == 1)
                {
                    memberTrack[0] = 1;
                    EndMatch();
                    return false;
                }
                else
                {
                    DisplayElimination(DataBase.GetWrestlerFullName(plObj.WresParam), blueTeamMembers.Count - 1);
                    EndRound();
                    return true;
                }
            }
            else if (loserTrack == 1)
            {
                if (redTeamMembers.Count == 1)
                {
                    memberTrack[1] = 1;
                    EndMatch();
                    return false;
                }
                else
                {
                    DisplayElimination(DataBase.GetWrestlerFullName(plObj.WresParam), redTeamMembers.Count - 1);
                    EndRound();
                    return true;
                }
            }
            else
            {
                //There's an error if we reach this point.
                return false;
            }
        }

        [Hook(TargetClass = "Announcer", TargetMethod = "Watch_Scuffle", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void OverrideScuffle()
        {
            //Ensure that the post-match gong is not triggered after an elimination
            if (!isExElimination)
            {
                return;
            }
            Ring.inst.venueSetting.se_Scuffle = MatchSEEnum.NoUse;
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "EndMatch", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void ResetVariables()
        {
            if (!isExElimination)
            {
                return;
            }

            try
            {
                L.D("Variables Reset Starting");
                MatchMain main = MatchMain.inst;
                //Ensure that we reset all tracking variables
                if (blueTeamMembers.Count <= 1 && memberTrack[0] > 0 || redTeamMembers.Count <= 1 && memberTrack[1] > 0 || main.isInterruptedMatch)
                {
                    currMatchTime = null;
                    endRound = false;
                    //nextMatch = false;
                    MoreMatchTypes_Form.form.btn_matchStart.Enabled = true;
                    L.D("Variables Reset Here");
                }
            }
            catch (Exception ex)
            {
                L.D("Error occured during variable reset; " + ex.Message);
                MoreMatchTypes_Form.form.btn_matchStart.Enabled = true;
            }
        }

        #region Helper Methods
        private static void SetTeamNames()
        {
            //Create Blue Team Name
            if (!MoreMatchTypes_Form.form.el_blueTeamName.Text.TrimStart().TrimEnd().Equals(""))
            {
                teamNames[0] = MoreMatchTypes_Form.form.el_blueTeamName.Text;
            }
            else
            {
                teamNames[0] = "Blue Team";
            }

            //Create Red Team Name
            if (!MoreMatchTypes_Form.form.el_blueTeamName.Text.TrimStart().TrimEnd().Equals(""))
            {
                teamNames[1] = MoreMatchTypes_Form.form.el_redTeamName.Text;
            }
            else
            {
                teamNames[1] = "Red Team";
            }
        }
        private static void SetTeamMembers()
        {
            blueTeamMembers = new Queue<string>();
            redTeamMembers = new Queue<string>();

            //Create Blue Team Member List
            foreach (String member in MoreMatchTypes_Form.form.el_blueList.Items)
            {
                blueTeamMembers.Enqueue(member);
            }

            //Create Red Team Member List
            foreach (String member in MoreMatchTypes_Form.form.el_redList.Items)
            {
                redTeamMembers.Enqueue(member);
            }
        }
        public static void EndMatch()
        {
            Referee mref = RefereeMan.inst.GetRefereeObj();
            mref.PlDir = PlDirEnum.Left;
            mref.State = RefeStateEnum.DeclareVictory;
            mref.ReqRefereeAnm(BasicSkillEnum.Refe_Stand_MatchEnd_Front_Left);
            MatchSetting settings = GlobalWork.GetInst().MatchSetting;

            //Ensure that we reset the 3 Game Match settings
            settings.is3GameMatch = false;
            global::GlobalParam.m_MacthCount = 3;
            global::GlobalParam.flg_MacthForceEnd = true;
            currMatchTime = null;
            endRound = false;
            //nextMatch = false;
            L.D("Match Ending Here");
        }
        public static void EndRound()
        {
            L.D("Ending Round? " + !endRound);
            //Override match end status
            MatchMain main = MatchMain.inst;
            main.isMatchEnd = false;

            //if(blueTeamMembers.Count <= 4 && redTeamMembers.Count <= 4)
            if (!endRound)
            {
                //Signal referee to end the round
                Referee matchRef = RefereeMan.inst.GetRefereeObj();
                matchRef.PlDir = PlDirEnum.Left;
                matchRef.ReqRefereeAnm(BasicSkillEnum.Refe_Stand_MatchEnd_Front_Left);
                //Announcer.inst.PlayGong_Eliminated();

                currMatchTime = new MatchTime
                {
                    min = main.matchTime.min,
                    sec = main.matchTime.sec
                };
                main.isTimeCounting = false;

                //Force game transition to the post match screen in order to reload wrestlers 
                main.isMatchEnd = true;
                MatchSetting settings = GlobalWork.GetInst().MatchSetting;
                settings.is3GameMatch = true;
                main.isInterruptedMatch = false;
                //nextMatch = true;

                //Ensure the 3 Game Rule Doesn't Force Match to End
                global::GlobalParam.m_MacthCount = 0;
                global::GlobalParam.flg_MacthForceEnd = false;
                global::GlobalParam.flg_MacthCount = true;

                if (loserTrack == 0)
                {
                    memberTrack[0]++;
                }
                else if (loserTrack == 1)
                {
                    memberTrack[1]++;
                }
            }
            endRound = true;

            #region Initial if condition to separate new wrestler load
            //Determine if we need a new round instance to update wrestler listing
            //if ((loserTrack == 0 && blueTeamMembers.Count > 4) || (loserTrack == 1 && redTeamMembers.Count > 4))
            //{
            //}
            #endregion
            #region Else Condition Implementing 4v4 Elimination Code
            //else
            //{
            //    try
            //    {
            //        main.isTimeCounting = true;
            //        //This section is executed when the losing team has 4 members or less remaining
            //        //Removing members from the losing team
            //        if (loserTrack == 0)
            //        {
            //            Player plObj = PlayerMan.inst.GetPlObj(memberTrack[0]);

            //            SetLoserState(memberTrack[0]);
            //            memberTrack[0]++;
            //            blueTeamMembers.Dequeue();

            //            ActivateMember(memberTrack[0]);
            //            L.D("Blue Members Remaining: " + blueTeamMembers.Count);
            //        }
            //        else if (loserTrack == 1)
            //        {
            //            Player plObj = PlayerMan.inst.GetPlObj(memberTrack[1] + 4);

            //            SetLoserState(memberTrack[1] + 4);
            //            memberTrack[1]++;
            //            redTeamMembers.Dequeue();

            //            ActivateMember(memberTrack[1] + 4);
            //            L.D("Red Members Remaining: " + redTeamMembers.Count);
            //        }
            //        SetSeconds();
            //        UpdateTeamMembers();
            //    }
            //    catch (Exception ex)
            //    {
            //        L.D("Error occurred during round end: " + ex.Message);
            //    }
            //}
            #endregion
        }
        public static void DisplayElimination(String wrestlerName, int membersRemaining)
        {
            DispNotification.inst.Show(wrestlerName + " has been eliminated!\t" + teamNames[loserTrack] + " members remaining: " + membersRemaining, 300);
        }
        public static void ActivateMember(int playerIndex)
        {
            Player plObj = PlayerMan.inst.GetPlObj(playerIndex);

            plObj.hasRight = true;
            plObj.isSecond = false;
            plObj.isSleep = false;

            //Ensure that player comes in fresh
            plObj.SetSP(65535f);
            plObj.SetHP(65535f);
            plObj.SetBP(65535f);

            //Determine how they enter the ring
            if (!MoreMatchTypes_Form.form.cb_membersWait.Checked)
            {
                plObj.isLose = false;
                plObj.Start_ForceControl(ForceCtrlEnum.GoBackToRing);
            }
        }
        public static void SetLoserState(int playerIndex)
        {
            Player plObj = PlayerMan.inst.GetPlObj(playerIndex);

            plObj.isKO = false;

            if (MoreMatchTypes_Form.form.cb_losersLeave.Checked)
            {
                plObj.Start_ForceControl(ForceCtrlEnum.LoseAndExit);
            }
            else
            {
                plObj.isSecond = true;

            }
        }
        public static void SetSeconds()
        {
            Player plObj;
            for (int i = 0; i < 8; i++)
            {
                //Skip the active positions
                if (i == memberTrack[0] || i == memberTrack[1] + 4)
                {
                    continue;
                }

                //Set anyone out of starting positions as a second.
                plObj = PlayerMan.inst.GetPlObj(i);
                if (!plObj)
                {
                    continue;
                }

                //Ensure losers are forced to leave ringside if applicable
                if (MoreMatchTypes_Form.form.cb_losersLeave.Checked && i < 4 && i < memberTrack[0])
                {
                    plObj.Start_ForceControl(ForceCtrlEnum.LoseAndExit);
                    continue;
                }

                if (MoreMatchTypes_Form.form.cb_losersLeave.Checked && i > 3 && i < memberTrack[1] + 4)
                {
                    plObj.Start_ForceControl(ForceCtrlEnum.LoseAndExit);
                    continue;
                }

                if (MoreMatchTypes_Form.form.cb_membersWait.Checked)
                {
                    plObj.isSecond = true;
                    plObj.Start_ForceControl(ForceCtrlEnum.SecondStanbdby);
                }
                else
                {
                    //Error - Player is not leaving ringside at match start.
                    plObj.Zone = ZoneEnum.StageEntrance;
                    plObj.isSecond = true;
                    plObj.Start_ForceControl(ForceCtrlEnum.LoseAndExit);
                }

                plObj.hasRight = false;
            }
        }
        public static void UpdateTeamMembers()
        {
            MatchSetting settings = GlobalWork.inst.MatchSetting;
            //Update Blue Team Members
            if (loserTrack == 0)
            {
                String[] wrestlers = blueTeamMembers.ToArray();

                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        settings.matchWrestlerInfo[i].entry = true;

                        String[] wrestlerName = wrestlers[i].Split(':');
                        settings.matchWrestlerInfo[i].wrestlerID = (WrestlerID)Int32.Parse(wrestlerName[wrestlerName.Length - 1]);
                        settings.matchWrestlerInfo[i].costume_no = 0;
                        settings.matchWrestlerInfo[i].alignment = WrestlerAlignmentEnum.Neutral;
                        settings.matchWrestlerInfo[i].assignedPad = PadPort.AI;

                        bool isSecond;
                        if (i == 0)
                        {
                            settings.matchWrestlerInfo[i].isSecond = false;
                            isSecond = false;
                        }
                        else
                        {
                            settings.matchWrestlerInfo[i].isSecond = true;
                            isSecond = true;
                        }
                        settings.matchWrestlerInfo[i].HP = 65535f;
                        settings.matchWrestlerInfo[i].SP = 65535f;
                        settings.matchWrestlerInfo[i].HP_Neck = 65535f;
                        settings.matchWrestlerInfo[i].HP_Arm = 65535f;
                        settings.matchWrestlerInfo[i].HP_Waist = 65535f;
                        settings.matchWrestlerInfo[i].HP_Leg = 65535f;

                        int playerControl = 0;
                        {
                            if (i == 0)
                            {
                                if (MoreMatchTypes_Form.form.el_blueControl.Checked)
                                {
                                    playerControl = 1;
                                }
                            }
                        }

                        if (i > blueTeamMembers.Count - 1)
                        {
                            settings.matchWrestlerInfo[i].entry = false;
                            settings.matchWrestlerInfo[i].wrestlerID = global::WrestlerID.Invalid;
                            GlobalParam.Set_WrestlerData(i, -1, global::WrestlerID.Invalid, false, 0, 65535f, 65535f, 65535f, 65535f, 65535f, 65535f);
                        }
                        else
                        {
                            GlobalParam.Set_WrestlerData(i, playerControl, settings.matchWrestlerInfo[i].wrestlerID, isSecond, 0, 65535f, 65535f, 65535f, 65535f, 65535f, 65535f);
                        }
                    }
                    catch
                    {
                        settings.matchWrestlerInfo[i].entry = false;
                        settings.matchWrestlerInfo[i].wrestlerID = global::WrestlerID.Invalid;
                        GlobalParam.Set_WrestlerData(i, -1, global::WrestlerID.Invalid, false, 0, 65535f, 65535f, 65535f, 65535f, 65535f, 65535f);
                    }
                }
            }

            //Update Red Team Members
            if (loserTrack == 1)
            {
                String[] wrestlers = redTeamMembers.ToArray();
                int spot = 4;

                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        settings.matchWrestlerInfo[spot].entry = true;
                        String[] wrestlerName = wrestlers[i].Split(':');
                        settings.matchWrestlerInfo[spot].wrestlerID = (WrestlerID)Int32.Parse(wrestlerName[wrestlerName.Length - 1]);
                        settings.matchWrestlerInfo[spot].costume_no = 0;
                        settings.matchWrestlerInfo[spot].alignment = WrestlerAlignmentEnum.Neutral;
                        settings.matchWrestlerInfo[spot].assignedPad = PadPort.AI;

                        bool isSecond;
                        if (i == 0)
                        {
                            settings.matchWrestlerInfo[spot].isSecond = false;
                            isSecond = false;
                        }
                        else
                        {
                            settings.matchWrestlerInfo[spot].isSecond = true;
                            isSecond = true;
                        }
                        settings.matchWrestlerInfo[spot].HP = 65535f;
                        settings.matchWrestlerInfo[spot].SP = 65535f;
                        settings.matchWrestlerInfo[spot].HP_Neck = 65535f;
                        settings.matchWrestlerInfo[spot].HP_Arm = 65535f;
                        settings.matchWrestlerInfo[spot].HP_Waist = 65535f;
                        settings.matchWrestlerInfo[spot].HP_Leg = 65535f;

                        int playerControl = 0;
                        {
                            if (i == 0)
                            {
                                if (MoreMatchTypes_Form.form.el_blueControl.Checked && MoreMatchTypes_Form.form.el_redControl.Checked)
                                {
                                    playerControl = 2;
                                }
                                else if (!MoreMatchTypes_Form.form.el_blueControl.Checked && MoreMatchTypes_Form.form.el_redControl.Checked)
                                {
                                    playerControl = 1;
                                }
                            }
                        }

                        if (i > redTeamMembers.Count - 1)
                        {
                            settings.matchWrestlerInfo[spot].entry = false;
                            settings.matchWrestlerInfo[spot].wrestlerID = global::WrestlerID.Invalid;
                            GlobalParam.Set_WrestlerData(spot, -1, global::WrestlerID.Invalid, false, 0, 65535f, 65535f, 65535f, 65535f, 65535f, 65535f);
                        }
                        else
                        {
                            GlobalParam.Set_WrestlerData(spot, playerControl, settings.matchWrestlerInfo[spot].wrestlerID, isSecond, 0, 65535f, 65535f, 65535f, 65535f, 65535f, 65535f);
                        }
                    }
                    catch
                    {
                        settings.matchWrestlerInfo[spot].entry = false;
                        settings.matchWrestlerInfo[spot].wrestlerID = global::WrestlerID.Invalid;
                        GlobalParam.Set_WrestlerData(spot, -1, global::WrestlerID.Invalid, false, 0, 65535f, 65535f, 65535f, 65535f, 65535f, 65535f);
                    }

                    spot++;
                }
            }
        }
        #endregion

    }
}
