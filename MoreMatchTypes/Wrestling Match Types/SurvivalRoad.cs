using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG;
using UnityEngine;
using MatchConfig;
using System.IO;

namespace MoreMatchTypes.Wrestling_Match_Types
{
    #region Access Modifiers
    [FieldAccess(Class = "MatchMain", Field = "InitMatch", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "MatchMain", Field = "EndMatch", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "MatchMain", Field = "InitRound", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "Referee", Field = "CheckMatchEnd", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "Menu_Result", Field = "Set_FinishSkill", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "Menu_SoundManager", Field = "MyMusic_Play", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "Menu_Title", Field = "Awake", Group = "MoreMatchTypes")]
    #endregion

    class SurvivalRoad
    {
        #region Variables
        private static bool isSurvival = false;

        /*Array to store match details
        [0] - Matches Remaining
        [1] - Continues Remaining
        [2] - Current Win Streak
        [3] - Highest Win Streak
        [4] - Total Losses
        [5] - Continues Used
        [6] - Higest Match Rating
        [7] - Total Match Rating
        [8] - Matches Played
        */
        private static int[] gameDetails;
        private static bool endRound = false;
        private static bool endMatch;
        private static bool isTag;
        private static bool isRandom;
        private static bool isRegen;
        private static int loserIndex;
        private static string[] teamNames;
        private static WresIDGroup[] currOpponents;
        private static List<WresIDGroup> teamList;
        private static List<WresIDGroup> waitingOpponents;
        private static List<WresIDGroup> usedOpponents;
        private static System.Random rnd = new System.Random();
        private static WresIDGroup playerEdit;
        private static WresIDGroup secondEdit;
        private static String opponentTeam;

        #endregion

        [Hook(TargetClass = "MatchMain", TargetMethod = "Awake", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void SetUpRound()
        {
            if (endRound)
            {
                SetupRound();
            }
        }
        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void SetMatchRules()
        {
            isSurvival = MoreMatchTypes_Form.moreMatchTypesForm.cb_survival.Checked;

            if (!isSurvival)
            {
                return;
            }

            if (!endRound)
            {
                L.D("Setting up match");
                SetupMatch();
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitRound", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        public static void StartRound(MatchMain m)
        {
            if (!isSurvival)
            {
                return;
            }

            if (loserIndex < 4 && loserIndex != -1)
            {
                //Refresh player & opponent characters for next match
                RefreshPlayer(0);
                RefreshPlayer(4);
                if (isTag)
                {
                    RefreshPlayer(1);
                    RefreshPlayer(5);
                }
            }

            //Resetting variables for the next match
            CheckSeconds();
            endRound = false;
            endMatch = false;
            loserIndex = -1;
        }

        [Hook(TargetClass = "Referee", TargetMethod = "CheckMatchEnd", InjectionLocation = 0, InjectFlags = HookInjectFlags.ModifyReturn, Group = "MoreMatchTypes")]
        public static bool CheckMatchEnd()
        {
            if (!isSurvival)
            {
                return false;
            }

            MatchMain main = MatchMain.inst;

            if (!main.isMatchEnd)
            {
                return false;
            }

            if (endRound || endMatch)
            {
                return true;
            }

            loserIndex = MatchEndFunctions.GetLoser();
            if (loserIndex == -1)
            {
                return false;
            }

            //Player lost, determine if any continues remain
            if (loserIndex < 4)
            {
                if (gameDetails[1] <= 0)
                {
                    EndMatch();
                    return false;
                }
                else
                {
                    EndRound();
                    return true;
                }
            }
            //Player won, determine if any matches remain
            else
            {
                if (gameDetails[0] <= 1)
                {
                    EndMatch();
                    return false;
                }
                else
                {
                    EndRound();
                    return true;
                }
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "EndMatch", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void ResetVariables()
        {
            if (!isSurvival)
            {
                return;
            }

            MatchMain main = MatchMain.inst;
            //Ensure that we reset all tracking variables
            if (endMatch || main.isInterruptedMatch)
            {
                endRound = false;
                MoreMatchTypes_Form.SurvivalRoadData.InProgress = false;
            }
        }

        [Hook(TargetClass = "Menu_Result", TargetMethod = "Set_FinishSkill", InjectionLocation = 0, InjectDirection = HookInjectDirection.After, InjectFlags = HookInjectFlags.PassParametersRef, Group = "MoreMatchTypes")]
        public static void SetResultScreenDisplay(ref string str)
        {
            if (!isSurvival)
            {
                return;
            }

            String result = CreateResult();
            str = result;
            UpdateProgress(result);
        }

        #region Helper Methods
        private static void SetTeamNames()
        {
            teamNames[0] = playerEdit.Name;
            if (secondEdit != null)
            {
                teamNames[0] += " & " + secondEdit.Name;
            }

            if (!MoreMatchTypes_Form.SurvivalRoadData.OpponentName.Equals(String.Empty))
            {
                teamNames[1] = MoreMatchTypes_Form.SurvivalRoadData.OpponentName.Trim();
            }
            else
            {
                teamNames[1] = "Red Team";
            }
        }
        private static void EndRound()
        {
            L.D("Ending Round");
            //Force game transition to the post match screen in order to reload wrestlers 
            MatchMain main = MatchMain.inst;
            main.isMatchEnd = true;
            MatchSetting settings = GlobalWork.GetInst().MatchSetting;
            main.isInterruptedMatch = false;

            //Determine if full HP regen rule is in effect
            if (isRegen)
            {
                RefreshPlayer(0);
                if (isTag)
                {
                    RefreshPlayer(1);
                }
            }

            //Ensure the 3 Game Rule Doesn't Force Match to End
            settings.MatchCnt = 0;
            settings.is3GameMatch = true;
            global::GlobalParam.is3GamesMatch = true;
            global::GlobalParam.m_MacthCount = 0;
            global::GlobalParam.flg_MacthForceEnd = false;
            //global::GlobalParam.flg_MacthCount = true;

            //Update Game Details
            if (!endRound)
            {
                UpdateDetails();
                endRound = true;
            }
        }
        private static void EndMatch()
        {
            //Ensure this isn't executed multiple times
            if (!endMatch)
            {
                L.D("Ending Match");
                MatchSetting settings = GlobalWork.GetInst().MatchSetting;

                //Ensure that we reset the 3 Game Match settings
                settings.is3GameMatch = false;
                global::GlobalParam.is3GamesMatch = false;
                global::GlobalParam.flg_MacthForceEnd = true;
                endRound = false;
                endMatch = true;
                UpdateDetails();
            }

        }
        private static void RefreshPlayer(int playerIndex)
        {
            try
            {
                Player plObj = PlayerMan.inst.GetPlObj(playerIndex);

                if ((!plObj))
                {
                    return;
                }

                //Ensure that player comes in fresh
                plObj.SetSP(65535f);
                plObj.SetHP(65535f);
                plObj.SetBP(65535f);
                plObj.AddHP_Arm(65535f);
                plObj.AddHP_Leg(65535f);
                plObj.AddHP_Neck(65535f);
                plObj.AddHP_Waist(65535f);

            }
            catch (Exception ex)
            {
                L.D("Refresh Player Error: " + ex.Message);
            }
        }
        private static WresIDGroup SelectOpponent(int index)
        {
            WresIDGroup opponent = null;
            int searchIndex;
            if (waitingOpponents.Count == 0)
            {
                InitializeLists();
            }

            //Handle selection for tag matches
            if (waitingOpponents.Count == 1 && isTag)
            {
                opponent = waitingOpponents[0];
                InitializeLists();
                currOpponents[index] = opponent;
                return opponent;
            }

            //Select opponent
            if (isRandom)
            {
                searchIndex = rnd.Next(waitingOpponents.Count - 1);
            }
            else
            {
                searchIndex = 0; //Get the next opponent in the list
            }

            if (isTag)
            {
                //Ensure that duplicate members are not selected
                if (index == 1)
                {
                    WresIDGroup firstOpp = currOpponents[0];
                    WresIDGroup secondOpp = waitingOpponents[searchIndex];
                    if (firstOpp.Equals(secondOpp))
                    {
                        foreach (WresIDGroup wrestler in waitingOpponents)
                        {
                            if (!firstOpp.Equals(wrestler))
                            {
                                currOpponents[index] = wrestler;
                                secondOpp = wrestler;
                                break;
                            }
                        }
                        return secondOpp;
                    }
                    else
                    {
                        currOpponents[index] = secondOpp;
                        return secondOpp;
                    }
                }
            }

            currOpponents[index] = waitingOpponents[searchIndex];
            return waitingOpponents[searchIndex];
        }
        private static void UpdateDetails()
        {
            //Update match played count
            gameDetails[8]++;

            if (loserIndex < 4)
            {
                gameDetails[1]--;
                gameDetails[2] = 0;
                gameDetails[4]++;
                if (!endMatch)
                {
                    gameDetails[5]++;
                }
            }
            else
            {
                gameDetails[0]--;
                gameDetails[2]++;

            }

            //Update win streak if applicable
            if (gameDetails[2] > gameDetails[3])
            {
                gameDetails[3] = gameDetails[2];
            }

        }
        private static void UpdateTeamMembers()
        {
            if (waitingOpponents.Count == 0)
            {
                InitializeLists();
                return;
            }

            WresIDGroup firstOpp = currOpponents[0];
            WresIDGroup secondOpp = null;
            if (isTag)
            {
                secondOpp = currOpponents[1];
            }

            try
            {
                //Move previous opponents to used list
                usedOpponents.Add(firstOpp);
                waitingOpponents.Remove(firstOpp);

                if (isTag)
                {
                    usedOpponents.Add(secondOpp);
                    waitingOpponents.Remove(secondOpp);
                }
            }
            catch (Exception ex)
            {
                L.D("Exception: " + ex.Message);
            }


        }
        private static void SetSecond(int index)
        {
            //Checking the tag partners to ensure they are set properly
            Player plObj;
            plObj = PlayerMan.inst.GetPlObj(index);
            if (!plObj)
            {
                return;
            }

            if (isTag)
            {
                plObj.isSecond = false;
            }
            else
            {
                plObj.isSecond = true;
            }

        }
        private static void CheckSeconds()
        {
            if (secondEdit == null)
            {
                return;
            }

            //Checking partners to determine if they should be seconds
            SetSecond(1);
            SetSecond(5);

        }
        private static void InitializeLists()
        {
            waitingOpponents.Clear();
            usedOpponents.Clear();

            if (teamList.Count == 0)
            {
                foreach (var wrestler in MoreMatchTypes_Form.SurvivalRoadData.Opponents)
                {
                    try
                    {
                        teamList.Add((WresIDGroup)wrestler);
                        waitingOpponents.Add((WresIDGroup)wrestler);
                    }
                    catch
                    { }
                }

            }

        }
        private static void SetupMatch()
        {
            MatchSetting settings = GlobalWork.inst.MatchSetting;

            isTag = MoreMatchTypes_Form.SurvivalRoadData.Tag;
            isRandom = MoreMatchTypes_Form.SurvivalRoadData.RandomSelect;
            playerEdit = MoreMatchTypes_Form.SurvivalRoadData.Wrestler;
            secondEdit = MoreMatchTypes_Form.SurvivalRoadData.Wrestler;
            opponentTeam = MoreMatchTypes_Form.SurvivalRoadData.OpponentName;
            isRegen = MoreMatchTypes_Form.SurvivalRoadData.RegainHP;

            //Create Initial Wrestler List
            waitingOpponents = new List<WresIDGroup>();
            usedOpponents = new List<WresIDGroup>();
            teamList = new List<WresIDGroup>();
            InitializeLists();
            currOpponents = new WresIDGroup[2]; //Only a maximum of two characters per team

            //Setting the initial opponents
            currOpponents[0] = MoreMatchTypes_Form.SurvivalRoadData.InitialOpponents[0];

            if (isTag)
            {
                currOpponents[1] = MoreMatchTypes_Form.SurvivalRoadData.InitialOpponents[1];
            }

            endRound = false;
            endMatch = false;
            loserIndex = -1;
            gameDetails = new int[9];
            teamNames = new String[2];
            SetTeamNames();

            //Setting game details
            gameDetails[0] = Convert.ToInt32(MoreMatchTypes_Form.SurvivalRoadData.Matches);
            gameDetails[1] = Convert.ToInt32(MoreMatchTypes_Form.SurvivalRoadData.Continues);
            settings.isSkipEntranceScene = true;
            settings.intrusionRate[0] = IntrusionRate.None;
            settings.intrusionRate[1] = IntrusionRate.None;
            global::GlobalParam.is3GamesMatch = true;
            global::GlobalParam.m_MacthCount = 0;
            settings.RoundNum = 0;
            MatchMain.inst.RoundCnt = 1;
        }
        private static void SetupRound()
        {
            L.D("Setting up round");
            MatchSetting settings = GlobalWork.inst.MatchSetting;
            MatchMain main = MatchMain.inst;
            main.RoundCnt = 1;
            settings.is3GameMatch = true;
            settings.RoundNum = 0;

            //Necessary for compatibility with Mods that change attire
            settings.isCarryOverHP = true;
            settings.MatchCnt = 1;

            //Update teams if the player won
            if (loserIndex > 3)
            {
                UpdateTeamMembers();
                WrestlerID wrestler = (WrestlerID)SelectOpponent(0).ID;
                settings = MatchConfiguration.AddPlayers(true, wrestler, 4, 0, false, 0, settings);

                //Adding new tag partner
                if (isTag)
                {
                    wrestler = (WrestlerID)SelectOpponent(1).ID;
                    settings = MatchConfiguration.AddPlayers(true, wrestler, 5, 0, false, 0, settings);
                }
            }
        }
        private static string CreateResult()
        {
            //Setting Match Evaluations
            String result = "";
            int currEval = MatchEvaluation.inst.EvaluateMatch();
            if (loserIndex > 3)
            {
                gameDetails[7] += currEval; //Only add the match rating for wins
            }
            if (currEval > gameDetails[6])
            {
                gameDetails[6] = currEval; //Replace the maximum match rating if applicable
            }

            try
            {
                if (endMatch)
                {
                    result += "Game Over\nMatches Played: " + gameDetails[8] + "\nContinues Used: " + gameDetails[5] + "\nHighest Win Streak: " + gameDetails[3];
                    result += "\nTotal Losses: " + gameDetails[4] + "\nHighest Match Rating: " + gameDetails[6] + "%";

                }
                else if (endRound)
                {
                    result += "\nMatches remaining: " + gameDetails[0] + "\nContinues remaining: " + gameDetails[1] + "\nCurrent Win Streak: " + gameDetails[2];
                    result += "\nHighest Match Rating: " + gameDetails[6] + "%";
                }

            }
            catch (Exception ex)
            {
                L.D("Results Screen Error: " + ex.Message);
            }

            return result;
        }
        private static void UpdateProgress(String result)
        {
            try
            {
                string info = "\nMatch #" + gameDetails[8] + "\n";
                string playerTeam = teamNames[0];
                String opponent = "";

                opponent = currOpponents[0].Name;

                if (isTag)
                {
                    opponent += " & " + currOpponents[1].Name;
                }

                opponent = teamNames[1] + " (" + opponent + ")";

                //Determine winner
                if (loserIndex > 3)
                {
                    info += playerTeam + " defeated " + opponent + ".";
                }
                else
                {
                    info += opponent + " defeated " + playerTeam + ".";
                }

                info += result;
                L.D(result);
                MoreMatchTypes_Form.SurvivalRoadData.MatchProgress += info + "\n\n";
            }
            catch (Exception ex)
            {
                L.D("UpdateProgressError: " + ex);
            }
           
        }

        #endregion
    }
}
