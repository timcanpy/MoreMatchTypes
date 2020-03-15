using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG;
using JetBrains.Annotations;
using MatchConfig;
using MoreMatchTypes.Data_Classes;

namespace MoreMatchTypes.Wrestling_Match_Types
{
    class ExElimination
    {
        #region Variables
        public static bool isExElim;
        public static String loserName;
        public static Queue<WresIDGroup> blueTeamReplacements;
        public static Queue<WresIDGroup> redTeamReplaements;
        public static Queue<int> blueOrderQueue;
        public static Queue<int> redOrderQueue;
        public static Queue<DefeatedPlayer> defeatedPlayers;
        public static int[] membersRemaining;
        public static int[] wins;
        public static EliminationUpdate eUpdate;
        public static bool endMatch;
        #endregion

        [Hook(TargetClass = "MatchMain", TargetMethod = "Awake", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void SetUpMatch()
        {
            isExElim = MoreMatchTypes_Form.moreMatchTypesForm.cb_exElim.Checked;
            endMatch = false;

            membersRemaining = new Int32[2];
            wins = new Int32[2];
            blueOrderQueue = new Queue<Int32>(new List<Int32> { 1, 2, 3 });
            redOrderQueue = new Queue<Int32>(new List<Int32> { 5, 6, 7 });
            defeatedPlayers = new Queue<DefeatedPlayer>();

            //We need to skip the first four members, as they are already in use.
            //Add blue members
            blueTeamReplacements = new Queue<WresIDGroup>();
            for (int i = 4; i < MoreMatchTypes_Form.ExEliminationData.BlueTeamMembers.Count; i++)
            {
                blueTeamReplacements.Enqueue(MoreMatchTypes_Form.ExEliminationData.BlueTeamMembers[i]);
            }

            //Add red members
            redTeamReplaements = new Queue<WresIDGroup>();
            for (int i = 4; i < MoreMatchTypes_Form.ExEliminationData.RedTeamMembers.Count; i++)
            {
                redTeamReplaements.Enqueue(MoreMatchTypes_Form.ExEliminationData.RedTeamMembers[i]);
            }

            //Add remaining team members.
            membersRemaining[0] = MoreMatchTypes_Form.ExEliminationData.BlueTeamMembers.Count ;
            membersRemaining[1] = MoreMatchTypes_Form.ExEliminationData.RedTeamMembers.Count;

            //Adding custom class for handling character switching
            eUpdate = MatchMain.inst.gameObject.GetComponent<EliminationUpdate>();
            if (eUpdate == null)
            {
                eUpdate = MatchMain.inst.gameObject.AddComponent<EliminationUpdate>();
            }

            eUpdate.Init();
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitRound", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None,
            Group = "MoreMatchTypes")]
        public static void SetUpPositions()
        {
            SetSeconds();
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "EndMatch", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void ResetVariables()
        {
            if (!isExElim)
            {
                return;
            }

            MatchMain main = MatchMain.inst;

            //Ensure that we reset all tracking variables
            if (main.isInterruptedMatch || main.isMatchEnd)
            {
                MoreMatchTypes_Form.ExEliminationData.InProgress = false;
            }

        }

        [Hook(TargetClass = "Announcer", TargetMethod = "Watch_Scuffle", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void OverrideScuffle()
        {
            //Ensure that the post-match gong is not triggered after an elimination
            if (!isExElim)
            {
                return;
            }
            Ring.inst.venueSetting.se_Scuffle = MatchSEEnum.NoUse;
        }

        [Hook(TargetClass = "Referee", TargetMethod = "CheckMatchEnd", InjectionLocation = 0,
            InjectFlags = HookInjectFlags.ModifyReturn, Group = "MoreMatchTypes")]
        public static bool CheckMatchEnd()
        {
            MatchMain main = MatchMain.inst;
            if (!main.isMatchEnd || endMatch)
            {
                return false;
            }

            Player plObj;
            CornerSide loserSide = CornerSide.Unknown;

            //Determine which player lost
            for (int i = 0; i < 8; i++)
            {
                plObj = PlayerMan.inst.GetPlObj(i);
                if (!plObj)
                {
                    continue;
                }

                if (plObj.isSecond || plObj.isIntruder)
                {
                    continue;
                }
                plObj.isKO = false;
                if (plObj.isLoseAndStop && (plObj.Zone == ZoneEnum.InRing || plObj.Zone == ZoneEnum.OutOfRing))
                {
                    L.D(DataBase.GetWrestlerFullName(plObj.WresParam) + " at index " + i + " has been eliminated.");

                    if (i < 3)
                    {
                        loserSide = CornerSide.Blue;
                    }
                    else
                    {
                        loserSide = CornerSide.Red;
                    }

                    loserName = DataBase.GetWrestlerFullName(plObj.WresParam);
                    SetLoserState(i);

                    //Add loser to the queue for replacement processing
                    defeatedPlayers.Enqueue(new DefeatedPlayer { player = plObj, side = loserSide });

                    bool continueMatch = UpdateTeam(loserSide);
                    main.isMatchEnd = !continueMatch;
                    endMatch = !continueMatch;
                    return continueMatch;
                }
                else
                {
                    continue;
                }
            }

            return false;
        }

        [Hook(TargetClass = "Menu_Result", TargetMethod = "Set_FinishSkill", InjectionLocation = 0,
            InjectDirection = HookInjectDirection.After, InjectFlags = HookInjectFlags.PassParametersRef,
            Group = "MoreMatchTypes")]
        public static void SetResultScreenDisplay(ref string str)
        {
            if (!isExElim)
            {
                return;
            }

            str = "\n" + MoreMatchTypes_Form.ExEliminationData.TeamNames[0] + ": " + wins[0] + " wins" +
                  "\n" + MoreMatchTypes_Form.ExEliminationData.TeamNames[1] + ": " + wins[1] + " wins" +
                  "\n";

            String result;

            if (wins[0] == wins[1])
            {
                result = "Draw";
            }
            else if (wins[0] > wins[1])
            {
                result = "Winner: " + MoreMatchTypes_Form.ExEliminationData.TeamNames[0];
            }
            else
            {
                result = "Winner: " + MoreMatchTypes_Form.ExEliminationData.TeamNames[1];
            }

            str += result;
        }

        #region Helper Methods
        public static void SetSeconds()
        {
            Player plObj;
            for (int i = 0; i < 8; i++)
            {
                //Skip starting positions
                if (i == 0 || i == 4)
                {
                    continue;
                }

                //Set anyone out of starting positions as a second.
                plObj = PlayerMan.inst.GetPlObj(i);
                if (!plObj)
                {
                    continue;
                }

                plObj.isSecond = true;
                plObj.Start_ForceControl(ForceCtrlEnum.SecondStanbdby);
                plObj.hasRight = false;
            }
        }
        public static bool UpdateTeam(CornerSide loserSide)
        {
            int membersLeft;
            int nextPlayer = -1;
            String teamName;

            Announcer.inst.PlayGong_Eliminated();

            if (loserSide == CornerSide.Blue)
            {
                wins[1]++;
                membersRemaining[0]--;

                if (membersRemaining[0] <= 0)
                {
                    return false;
                }

                membersLeft = membersRemaining[0];
                if (blueOrderQueue.Count != 0)
                    nextPlayer = blueOrderQueue.Dequeue();
                teamName = MoreMatchTypes_Form.ExEliminationData.TeamNames[0];

            }
            else
            {
                wins[0]++;
                membersRemaining[1]--;

                if (membersRemaining[1] <= 0)
                {
                    return false;
                }

                membersLeft = membersRemaining[1];
                if (redOrderQueue.Count != 0)
                    nextPlayer = redOrderQueue.Dequeue();
                teamName = MoreMatchTypes_Form.ExEliminationData.TeamNames[1];
            }

            AnnounceElimination(loserName, membersLeft, teamName);
            ActivateMember(nextPlayer, loserSide);
            return true;
        }
        public static void SetLoserState(int playerIndex)
        {
            Player plObj = PlayerMan.inst.GetPlObj(playerIndex);

            //Ensure that the loser leaves the ring.
            plObj.Start_ForceControl(ForceCtrlEnum.LoseAndExit);
            plObj.AddBP(1000f);
        }
        public static void ActivateMember(int playerIndex, CornerSide loserSide)
        {
            if (playerIndex == -1)
            {
                return;
            }

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
            plObj.SetPlayerController(PlayerControllerKind.AI);
            plObj.Start_ForceControl(global::ForceCtrlEnum.GoBackToRing);

            //Determine if this is a player controlled team
            MenuPadKind controller = MenuPadKind.COM;
            if (loserSide == CornerSide.Blue)
            {
                if (MoreMatchTypes_Form.ExEliminationData.ControlBlue)
                {
                    controller = MenuPadKind.Pad1;
                }
                else
                {
                    controller = MenuPadKind.COM;
                }
            }
            else
            {
                //Ensure that we cover scenarios for two human players
                if (MoreMatchTypes_Form.ExEliminationData.ControlBlue &&
                    MoreMatchTypes_Form.ExEliminationData.ControlRed)
                {
                    controller = MenuPadKind.Pad2;
                }
                else if (MoreMatchTypes_Form.ExEliminationData.ControlRed)
                {
                    controller = MenuPadKind.Pad1;
                }
                else
                {
                    controller = MenuPadKind.COM;
                }
            }

            MatchWrestlerInfo wrestler = GlobalWork.inst.MatchSetting.matchWrestlerInfo[playerIndex];
            GlobalParam.Set_WrestlerData(playerIndex, controller, wrestler.wrestlerID, false, wrestler.costume_no, 65535f, 65535f, 65535f, 65535f, 65535f, 65535f);
        }
        public static void AnnounceElimination(String eliminatedPlayer, int membersRemaining, String teamName)
        {
            DispNotification.inst.Show(eliminatedPlayer + " has been eliminated!\t" + teamName + " members remaining: " + membersRemaining, 300);
        }
        #endregion
    }
}
