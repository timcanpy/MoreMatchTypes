using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DG;

namespace MoreMatchTypes
{
    [FieldAccess(Class = "MatchMain", Field = "InitMatch", Group = "MoreMatchTypes"), FieldAccess(Class = "MatchMain", Field = "InitRound", Group = "MoreMatchTypes"), FieldAccess(Class = "Referee", Field = "CheckMatchEnd", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "PlayerForcedController", Field = "FoceControl_SecondStandby", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "PlayerForcedController", Field = "FoceControl_LoseAndExit_ToStage", Group = "MoreMatchTypes")]
    public class Elimination
    {
        #region Variables
        //Disabled for Battle Royal matches, therefore we will only have two teams to keep track of
        public static int[] wins = new int[2];
        public static string[] teamNames = new string[2];
        public static Queue<String> blueTeamMembers;
        public static Queue<String> redTeamMembers;
        public static int[] memberTrack;
        public static MatchTime currMatchTime = null;
        public static bool endRound;
        public static int loserTrack;
        public static bool isElimination;
        public static MenuPadKind[] padControls;
        #endregion


        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void SetMatchRules()
        {
            isElimination = false;
            padControls = new MenuPadKind[8];

            //Ensure a valid elimination match can take place
            MatchSetting settings = GlobalWork.inst.MatchSetting;
            if (settings.BattleRoyalKind != BattleRoyalKindEnum.Off || settings.isS1Rule)
            {
                return;
            }
            if (settings.arena == VenueEnum.BarbedWire || settings.arena == VenueEnum.Cage || settings.arena == VenueEnum.Dodecagon || settings.arena == VenueEnum.LandMine_BarbedWire || settings.arena == VenueEnum.LandMine_FluorescentLamp)
            {
                return;
            }


            isElimination = MoreMatchTypes_Form.form.cb_elimination.Checked;
            if (!isElimination)
            {
                return;
            }

            //Set variables for the match type
            currMatchTime = null;
            teamNames = new string[2];
            memberTrack = new int[2];
            endRound = false;
            loserTrack = 0;
            settings.CriticalRate = CriticalRateEnum.Half;
            SetTeamNames();
            SetTeamMembers();
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitRound", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        public static void StartRound(MatchMain m)
        {
            if (!isElimination)
            {
                return;
            }

            //Settings for the start of a match
            if (!endRound)
            {
                //Setting up team members to track; this will be decreased as a team loses.
                memberTrack[0] = 0;
                memberTrack[1] = 0;
            }
            else
            {
                endRound = false;

                //Removing members from the losing team
                if (loserTrack == 0)
                {
                    Player plObj = PlayerMan.inst.GetPlObj(memberTrack[0]);

                    SetLoserState(memberTrack[0]);
                    memberTrack[0]++;
                    blueTeamMembers.Dequeue();

                    ActivateMember(memberTrack[0]);
                }
                else if (loserTrack == 1)
                {
                    Player plObj = PlayerMan.inst.GetPlObj(memberTrack[1] + 4);

                    SetLoserState(memberTrack[1] + 4);
                    memberTrack[1]++;
                    redTeamMembers.Dequeue();

                    ActivateMember(memberTrack[1] + 4);
                }
                //Handle double count outs
                else if (loserTrack == 2)
                {
                    Player plObj;

                    //Blue Team
                    plObj = PlayerMan.inst.GetPlObj(memberTrack[0]);

                    SetLoserState(memberTrack[0]);
                    memberTrack[0]++;
                    blueTeamMembers.Dequeue();

                    ActivateMember(memberTrack[0]);

                    //Red Team
                    plObj = PlayerMan.inst.GetPlObj(memberTrack[1] + 4);

                    SetLoserState(memberTrack[1] + 4);
                    memberTrack[1]++;
                    redTeamMembers.Dequeue();

                    ActivateMember(memberTrack[1] + 4);
                }
                m.matchTime.Set(currMatchTime);
            }

            SetSeconds();
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "ProcessMatchEnd_Draw", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void CheckCountOut()
        {
            if (!isElimination)
            {
                return;
            }

            MatchMain main = MatchMain.inst;
            if (main.isTimeUp)
            {
                return;
            }

            if (blueTeamMembers.Count == 1 && redTeamMembers.Count == 1)
            {
                return;
            }

            if (blueTeamMembers.Count == 1)
            {
                EndMatch(memberTrack[0]);
                return;
            }
            if (redTeamMembers.Count == 1)
            {
                EndMatch(memberTrack[0] + 4);
                return;
            }

            //Remove players from both sides
            Player plObj;
            plObj = PlayerMan.inst.GetPlObj(memberTrack[0]);
            DisplayElimination(DataBase.GetWrestlerFullName(plObj.WresParam), blueTeamMembers.Count - 1);

            plObj = PlayerMan.inst.GetPlObj(memberTrack[1]);
            DisplayElimination(DataBase.GetWrestlerFullName(plObj.WresParam), redTeamMembers.Count - 1);
            loserTrack = 2;
            EndRound();
        }

        [Hook(TargetClass = "Referee", TargetMethod = "CheckMatchEnd", InjectionLocation = 0, InjectFlags = HookInjectFlags.ModifyReturn, Group = "MoreMatchTypes")]
        public static bool CheckMatchEnd()
        {
            if (!isElimination)
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
                    //EndMatch(-1);
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
                    //EndMatch(-1);
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
            if (!isElimination)
            {
                return;
            }
            Ring.inst.venueSetting.se_Scuffle = MatchSEEnum.NoUse;
        }

        #region Helper Methods

        public static void EndMatch(int loserIndex)
        {
            Referee mref = RefereeMan.inst.GetRefereeObj();
            mref.PlDir = PlDirEnum.Left;
            mref.State = RefeStateEnum.DeclareVictory;
            mref.ReqRefereeAnm(BasicSkillEnum.Refe_Stand_MatchEnd_Front_Left);

            //Ignore if all players have been eliminated
            if (loserIndex != -1)
            {
                mref.SentenceLose(loserIndex);
                PlayerMan.inst.GetPlObj(loserIndex).isLoseAndStop = true;
                mref.matchResult = MatchResultEnum.RingOut;
            }
        }

        public static void EndRound()
        {
            MatchMain main = MatchMain.inst;
            main.isMatchEnd = false;

            //Signal referee to end the round
            Referee matchRef = RefereeMan.inst.GetRefereeObj();
            matchRef.PlDir = PlDirEnum.Left;
            matchRef.ReqRefereeAnm(BasicSkillEnum.Refe_Stand_MatchEnd_Front_Left);
            Announcer.inst.PlayGong_Eliminated();

            //Removing members from the losing team
            if (loserTrack == 0)
            {
                Player plObj = PlayerMan.inst.GetPlObj(memberTrack[0]);

                SetLoserState(memberTrack[0]);
                memberTrack[0]++;
                blueTeamMembers.Dequeue();

                ActivateMember(memberTrack[0]);
            }
            else if (loserTrack == 1)
            {
                Player plObj = PlayerMan.inst.GetPlObj(memberTrack[1] + 4);

                SetLoserState(memberTrack[1] + 4);
                memberTrack[1]++;
                redTeamMembers.Dequeue();

                ActivateMember(memberTrack[1] + 4);
            }
            //Handle double count outs
            else if (loserTrack == 2)
            {
                Player plObj;

                //Blue Team
                plObj = PlayerMan.inst.GetPlObj(memberTrack[0]);

                SetLoserState(memberTrack[0]);
                memberTrack[0]++;
                blueTeamMembers.Dequeue();

                ActivateMember(memberTrack[0]);

                //Red Team
                plObj = PlayerMan.inst.GetPlObj(memberTrack[1] + 4);

                SetLoserState(memberTrack[1] + 4);
                memberTrack[1]++;
                redTeamMembers.Dequeue();

                ActivateMember(memberTrack[1] + 4);
            }

            SetSeconds();
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
            plObj.isLose = false;
            plObj.Start_ForceControl(global::ForceCtrlEnum.GoBackToRing);

            MatchWrestlerInfo wrestler = GlobalWork.inst.MatchSetting.matchWrestlerInfo[playerIndex];
            GlobalParam.Set_WrestlerData(playerIndex, padControls[playerIndex], wrestler.wrestlerID, false, wrestler.costume_no, 65535f, 65535f, 65535f, 65535f, 65535f, 65535f);
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

        public static void SetTeamNames()
        {
            PlayerMan p = PlayerMan.inst;
            int playerCount = p.GetPlayerNum();

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
                    if (!plObj.isSecond && !plObj.isSleep)
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
                    teamNames[0] = "Blue Team";
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
                    if (!plObj.isSecond && !plObj.isSleep)
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
                    teamNames[1] = "Red Team";
                }
            }
            catch
            {
                teamNames[0] = "Blue Team";
                teamNames[1] = "Red Team";
            }
        }

        public static void SetTeamMembers()
        {
            blueTeamMembers = new Queue<string>();
            redTeamMembers = new Queue<string>();

            PlayerMan p = PlayerMan.inst;
            //Set the team members
            for (int i = 0; i < 8; i++)
            {
                Player plObj = PlayerMan.inst.GetPlObj(i);
                if (!plObj)
                {
                    continue;
                }

                if (i < 4)
                {
                    blueTeamMembers.Enqueue(DataBase.GetWrestlerFullName(plObj.WresParam));
                }
                else
                {
                    redTeamMembers.Enqueue(DataBase.GetWrestlerFullName(plObj.WresParam));
                }
            }
        }

        public static void SetPadControls()
        {
            for (int i = 0; i < 8; i++)
            {
                Player player = PlayerMan.inst.GetPlObj(i);
                if (!player)
                {
                    continue;
                }

                switch (player.plCont_Pad.port)
                {
                    case PadPort.Pad1: padControls[i] = MenuPadKind.Pad1;
                        break;
                    case PadPort.Pad2:
                        padControls[i] = MenuPadKind.Pad2;
                        break;
                    case PadPort.Pad3:
                        padControls[i] = MenuPadKind.Pad3;
                        break;
                    case PadPort.Pad4:
                        padControls[i] = MenuPadKind.Pad4;
                        break;
                    case PadPort.Pad5:
                        padControls[i] = MenuPadKind.Pad5;
                        break;
                    case PadPort.Pad6:
                        padControls[i] = MenuPadKind.Pad6;
                        break;
                    case PadPort.Pad7:
                        padControls[i] = MenuPadKind.Pad7;
                        break;
                    case PadPort.Pad8:
                        padControls[i] = MenuPadKind.Pad8;
                        break;
                    case PadPort.AI:
                    default:
                        padControls[i] = MenuPadKind.COM;
                        break;
                }
            }
        }

        public static void SetLoserState(int playerIndex)
        {
            Player plObj = PlayerMan.inst.GetPlObj(playerIndex);

            //plObj.isKO = false;

            if (MoreMatchTypes_Form.form.cb_losersLeave.Checked)
            {
                plObj.Start_ForceControl(ForceCtrlEnum.LoseAndExit);
            }
            else
            {
                plObj.isSecond = true;

            }
        }
        
        public static bool Contains(List<string> thisTeam, List<string> tMembers)
        {
            foreach (string w in thisTeam)
            {
                if (!tMembers.Contains(w))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
    }

}
