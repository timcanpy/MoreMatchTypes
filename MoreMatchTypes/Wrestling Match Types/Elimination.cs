using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DG;
using MatchConfig;
using ModPack;
using UnityEngine;
using MatchConfig;

namespace MoreMatchTypes
{
    [FieldAccess(Class = "MatchMain", Field = "InitMatch", Group = "MoreMatchTypes"), FieldAccess(Class = "MatchMain", Field = "InitRound", Group = "MoreMatchTypes"), FieldAccess(Class = "Referee", Field = "CheckMatchEnd", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "PlayerForcedController", Field = "FoceControl_SecondStandby", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "PlayerForcedController", Field = "FoceControl_LoseAndExit_ToStage", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "Referee", Field = "ProcesskMatchEnd_Normal", Group = "MoreMatchTypes")]
    public class Elimination
    {
        #region Variables
        //Disabled for Battle Royal matches, therefore we will only have two teams to keep track of
        public static int[] wins = new int[2];
        public static string[] teamNames = new string[2];
        public static Queue<String> blueTeamMembers;
        public static Queue<String> redTeamMembers;
        public static bool endMatch;
        public static int[] memberTrack;
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


            isElimination = MoreMatchTypes_Form.moreMatchTypesForm.cb_elimination.Checked;
            if (!isElimination)
            {
                return;
            }

            //Set variables for the match type
            endMatch = false;
            teamNames = new string[2];
            memberTrack = new int[2];
            memberTrack[0] = 0;
            memberTrack[1] = 0;
            loserTrack = -1;
            SetTeamNames();
            SetTeamMembers();
            SetPadControls();
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitRound", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        public static void StartRound(MatchMain m)
        {
            if (!isElimination)
            {
                return;
            }
            #region Old Code
            //Settings for the start of a match
            //if (!endRound)
            //{
            //    //Setting up team members to track; this will be decreased as a team loses.
            //    memberTrack[0] = 0;
            //    memberTrack[1] = 0;
            //}
            //else
            //{
            //    endRound = false;

            //    //Removing members from the losing team
            //    if (loserTrack == 0)
            //    {
            //        Player plObj = PlayerMan.inst.GetPlObj(memberTrack[0]);

            //        SetLoserState(memberTrack[0]);
            //        memberTrack[0]++;
            //        blueTeamMembers.Dequeue();

            //        ActivateMember(memberTrack[0]);
            //    }
            //    else if (loserTrack == 1)
            //    {
            //        Player plObj = PlayerMan.inst.GetPlObj(memberTrack[1] + 4);

            //        SetLoserState(memberTrack[1] + 4);
            //        memberTrack[1]++;
            //        redTeamMembers.Dequeue();

            //        ActivateMember(memberTrack[1] + 4);
            //    }
            //    //Handle double count outs
            //    else if (loserTrack == 2)
            //    {
            //        Player plObj;

            //        //Blue Team
            //        plObj = PlayerMan.inst.GetPlObj(memberTrack[0]);

            //        SetLoserState(memberTrack[0]);
            //        memberTrack[0]++;
            //        blueTeamMembers.Dequeue();

            //        ActivateMember(memberTrack[0]);

            //        //Red Team
            //        plObj = PlayerMan.inst.GetPlObj(memberTrack[1] + 4);

            //        SetLoserState(memberTrack[1] + 4);
            //        memberTrack[1]++;
            //        redTeamMembers.Dequeue();

            //        ActivateMember(memberTrack[1] + 4);
            //    }
            //}
            #endregion
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
                //Determine who has more points
                if (wins[0] > wins[1])
                {
                    //Red Team Loses
                    EndMatch(MatchConfiguration.GetLegalMan(CornerSide.Red));
                }
                else
                {
                    //Blue Team Loses
                    EndMatch(MatchConfiguration.GetLegalMan(CornerSide.Blue));
                }

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

        [Hook(TargetClass = "Referee", TargetMethod = "ProcesskMatchEnd_Normal", InjectionLocation = 0, InjectFlags = HookInjectFlags.ModifyReturn, Group = "MoreMatchTypes")]
        public static bool CheckMatchEnd()
        {
            if (!isElimination)
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
                if (plObj.isLose)
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
                    endMatch = true;
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
                    endMatch = true;
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
                L.D("EliminationError: Reached point where loser track is zero");
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
            //Announcer.inst.PlayGong_Eliminated();

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
            MatchConfiguration.ShowAnnouncement(wrestlerName + " has been eliminated!\t" + teamNames[loserTrack] + " members remaining: " + membersRemaining, 300);
        }
        public static void ActivateMember(int playerIndex)
        {
            Player plObj = PlayerMan.inst.GetPlObj(playerIndex);
            L.D("Next member state is " + plObj.State);

            //Ensure that edits immediately enter a ready state.
            if (plObj.State != PlStateEnum.Stand && plObj.State != PlStateEnum.Performance && plObj.Zone == ZoneEnum.OutOfRing)
            {
                plObj.ChangeState(PlStateEnum.Stand);
            }

            plObj.hasRight = true;
            plObj.isSecond = false;
            plObj.isSleep = false;

            //Ensure that player comes in fresh
            plObj.SetSP(65535f);
            plObj.SetHP(65535f);
            plObj.SetBP(65535f);

            //Determine ukemi bonus; based on player's spot on the team.
            switch (playerIndex)
            {
                case 1:
                case 5:
                    plObj.UkeRecoveryPoint += 3840;
                    break;
                case 2:
                case 6:
                    plObj.UkeRecoveryPoint += 7680;
                    break;
                case 3:
                case 7:
                    plObj.UkeRecoveryPoint += 15360;
                    break;
                default:
                    break;
            }

            //Determine how they enter the ring
            plObj.isLose = false;
            plObj.SetPlayerController(PlayerControllerKind.AI);
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

                if (plObj.isIntruder)
                {
                    continue;
                }

                //Force players to leave ring side
                if (MoreMatchTypes_Form.moreMatchTypesForm.cb_losersLeave.Checked && i < 4 && i < memberTrack[0])
                {
                    plObj.Start_ForceControl(ForceCtrlEnum.LoseAndExit);
                    continue;
                }

                if (MoreMatchTypes_Form.moreMatchTypesForm.cb_losersLeave.Checked && i > 3 && i < memberTrack[1] + 4)
                {
                    plObj.Start_ForceControl(ForceCtrlEnum.LoseAndExit);
                    continue;
                }

                plObj.isSecond = true;
                plObj.Start_ForceControl(ForceCtrlEnum.SecondStanbdby);
                plObj.hasRight = false;
            }
        }
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
        public static void SetTeamMembers()
        {
            blueTeamMembers = new Queue<string>();
            redTeamMembers = new Queue<string>();

            PlayerMan p = PlayerMan.inst;
            MatchSetting settings = GlobalWork.inst.MatchSetting;

            //Set the team members
            for (int i = 0; i < 8; i++)
            {
                Player plObj = PlayerMan.inst.GetPlObj(i);
                if (!plObj)
                {
                    continue;
                }

                if (plObj.isIntruder)
                {
                    continue;
                }

                //This match type cannot include Seconds, so we override the initial setting here.
                settings.matchWrestlerInfo[i].isSecond = false;

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

                try
                {
                    if (player.plController.kind == PlayerControllerKind.AI)
                    {
                        padControls[i] = MenuPadKind.COM;
                    }
                    else
                    {
                        switch (player.plCont_Pad.port)
                        {
                            case PadPort.Pad1:
                                padControls[i] = MenuPadKind.Pad1;
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
                catch (Exception e)
                {
                    L.D("Error on index " + i);
                    L.D("SetPadControlError:" + e);
                    padControls[i] = MenuPadKind.COM;
                }

            }
        }
        public static void SetLoserState(int playerIndex)
        {
            Player plObj = PlayerMan.inst.GetPlObj(playerIndex);

            //Ensure that the loser leaves the ring.
            plObj.Start_ForceControl(ForceCtrlEnum.LoseAndExit);
            plObj.AddBP(1000f);
        }
        #endregion
    }

}
