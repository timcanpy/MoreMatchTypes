using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG;
using UnityEngine;
using MoreMatchTypes.DataAccess;
using MatchConfig;
using System.IO;

namespace MoreMatchTypes.Wrestling_Match_Types
{
    #region Access Modifiers
    [FieldAccess(Class = "MatchMain", Field = "CreatePlayers", Group = "MoreMatchTypes")]
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
        private static String second;
        private static bool endMatch = false;
        private static bool endGame;
        private static bool isTag;
        private static bool isReverse;
        private static int loserTrack;
        private static string[] teamNames;
        private static WresIDGroup[] currOpponents;
        private static List<WresIDGroup> teamList;
        private static List<WresIDGroup> waitingOpponents;
        private static List<WresIDGroup> usedOpponents;
        private static System.Random rnd = new System.Random();
        private static String playerEdit;
        private static String secondEdit;
        private static String opponentTeam;

        #endregion

        [Hook(TargetClass = "MatchMain", TargetMethod = "CreatePlayers", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void SetMatchRules()
        {
            isSurvival = MoreMatchTypes_Form.form.cb_survival.Checked;

            if (!isSurvival)
            {
                return;
            }

            if (!endMatch)
            {
                InitializeSettings();
            }
            else
            {
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

            if (loserTrack < 4 && loserTrack != -1)
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
            endMatch = false;
            endGame = false;
            loserTrack = -1;
        }

        [Hook(TargetClass = "Menu_SoundManager", TargetMethod = "MyMusic_Play", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void UpdateMusic()
        {
            if (!isSurvival)
            {
                return;
            }

            //Force change the Match BGM; this implementation allows the theme to be changed before each new match
            string matchBGM = "";
            string bgmPath = System.IO.Directory.GetCurrentDirectory() + @"\BGM";
            try
            {
                matchBGM = bgmPath + @"\" + MoreMatchTypes_Form.form.sr_bgmList.SelectedItem.ToString();
                global::Menu_SoundManager.MyMusic_SelectFile_Match = matchBGM;
                L.D("Match BGM Path: " + matchBGM);
            }
            catch (Exception ex)
            {
                L.D("Change Music Exception: " + ex.Message + "\nMatch Bgm: " + matchBGM);
            }

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

            if (endMatch || endGame)
            {
                return true;
            }

            loserTrack = MatchEndFunctions.GetLoser();
            if (loserTrack == -1)
            {
                return false;
            }

            //Player lost, determine if any continues remain
            if (loserTrack < 4)
            {
                if (gameDetails[1] <= 0)
                {
                    EndGame();
                    L.D("Continues: " + gameDetails[1]);
                    return false;
                }
                else
                {
                    EndMatch();
                    return true;
                }
            }
            //Player won, determine if any matches remain
            else
            {
                if (gameDetails[0] <= 1)
                {
                    L.D("Matches: " + gameDetails[0]);
                    EndGame();
                    return false;
                }
                else
                {
                    EndMatch();
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
            if (endGame || main.isInterruptedMatch)
            {
                endMatch = false;
                MoreMatchTypes_Form.form.sr_start.Enabled = true;
            }
        }

        [Hook(TargetClass = "Menu_Result", TargetMethod = "Set_FinishSkill", InjectionLocation = 8, InjectDirection = HookInjectDirection.After, InjectFlags = HookInjectFlags.PassParametersVal | HookInjectFlags.PassLocals, LocalVarIds = new int[] { 1 }, Group = "MoreMatchTypes")]
        public static void SetResultScreenDisplay(ref UILabel finishText, string str)
        {
            if (!isSurvival)
            {
                return;
            }

            String result = CreateResult();
            finishText.text = "Survival Road\n" + result;
            UpdateProgress(result);
        }

        #region Helper Methods
        private static void SetTeamNames()
        {
            teamNames[0] = "Blue Team";
            teamNames[1] = "Red Team";

            if (!MoreMatchTypes_Form.form.sr_teamName.Text.Trim().Equals(String.Empty))
            {
                teamNames[1] = MoreMatchTypes_Form.form.sr_teamName.Text.Trim();
            }
        }
        private static void EndMatch()
        {
            //Force game transition to the post match screen in order to reload wrestlers 
            MatchMain main = MatchMain.inst;
            main.isMatchEnd = true;
            MatchSetting settings = GlobalWork.GetInst().MatchSetting;
            settings.is3GameMatch = true;
            main.isInterruptedMatch = false;


            //Ensure the 3 Game Rule Doesn't Force Match to End
            global::GlobalParam.m_MacthCount = 0;
            global::GlobalParam.flg_MacthForceEnd = false;
            global::GlobalParam.flg_MacthCount = true;

            //Update Game Details
            if (!endMatch)
            {
                L.D("Ending Match");
                UpdateDetails();
                endMatch = true;
            }
        }
        private static void EndGame()
        {
            //Ensure this isn't executed multiple times
            if (!endGame)
            {
                Referee mref = RefereeMan.inst.GetRefereeObj();
                mref.PlDir = PlDirEnum.Left;
                mref.State = RefeStateEnum.DeclareVictory;
                mref.ReqRefereeAnm(BasicSkillEnum.Refe_Stand_MatchEnd_Front_Left);
                MatchSetting settings = GlobalWork.GetInst().MatchSetting;

                //Ensure that we reset the 3 Game Match settings
                settings.is3GameMatch = false;
                global::GlobalParam.flg_MacthForceEnd = true;
                endMatch = false;
                endGame = true;
                L.D("Game Ending Here");
                UpdateDetails();

                L.D("DB Write Result: " + DataMethods.UpdateSurvivalData(playerEdit, gameDetails[8], gameDetails[4], gameDetails[5], gameDetails[3], gameDetails[6], gameDetails[7]));
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

                L.D("Refreshed Player: " + playerIndex);
            }
            catch (Exception ex)
            {
                L.D("Refresh Player Error: " + ex.Message);
            }
        }
        private static WresIDGroup SelectOpponent(int index)
        {
            WresIDGroup opponent = null;
            int randomIndex;
            if (waitingOpponents.Count == 0)
            {
                L.D("Resetting at zero lists");
                InitializeLists();
            }

            //Handle selection for tag matches
            if (waitingOpponents.Count == 1 && isTag)
            {
                L.D("Resetting lists for tag teams");
                opponent = waitingOpponents[0];
                InitializeLists();
                currOpponents[index] = opponent;
                return opponent;
            }

            //Select random opponent
            randomIndex = rnd.Next(waitingOpponents.Count - 1);

            if (isTag)
            {
                //Ensure that duplicate members are not selected
                if (index == 1)
                {
                    L.D("Checking for duplicates");
                    L.D("Waiting Opponents: " + waitingOpponents.Count);
                    L.D("First Opponent: " + currOpponents[0]);
                    L.D("Second Opponent: " + waitingOpponents[randomIndex]);
                    WresIDGroup firstOpp = currOpponents[0];
                    WresIDGroup secondOpp = waitingOpponents[randomIndex];
                    if (firstOpp.Equals(secondOpp))
                    {
                        L.D("Looking for new opponent");
                        foreach (WresIDGroup wrestler in waitingOpponents)
                        {
                            if (!firstOpp.Equals(wrestler))
                            {
                                currOpponents[index] = wrestler;
                                secondOpp = wrestler;
                                break;
                            }
                        }
                        L.D(firstOpp + " vs " + secondOpp);
                        L.D("Returning " + waitingOpponents[randomIndex] + " at index" + index);
                        return secondOpp;
                    }
                    else
                    {
                        L.D("Returning " + waitingOpponents[randomIndex] + " at index" + index);
                        currOpponents[index] = secondOpp;
                        return secondOpp;
                    }
                }
            }

            L.D("Returning " + waitingOpponents[randomIndex] + " at index" + index);
            currOpponents[index] = waitingOpponents[randomIndex];
            return waitingOpponents[randomIndex];
        }
        private static void UpdateDetails()
        {
            //Update match played count
            gameDetails[8]++;

            if (loserTrack < 4)
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
                L.D("Waiting Opponents Before: " + waitingOpponents.Count);
                L.D("Removed wrestler: " + firstOpp);
                waitingOpponents.Remove(firstOpp);
                L.D("Waiting Opponent After: " + waitingOpponents.Count);

                if (isTag)
                {
                    usedOpponents.Add(secondOpp);
                    L.D("Removed tag wrestler: " + secondOpp);
                    waitingOpponents.Remove(secondOpp);
                    L.D("Waiting Opponent Count: " + waitingOpponents.Count);
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
            if (second.Trim().Equals(string.Empty))
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
                L.D("Initializing team list");

                foreach (var wrestler in MoreMatchTypes_Form.form.sr_teamList.Items)
                {
                    try
                    {
                        teamList.Add((WresIDGroup)wrestler);
                        waitingOpponents.Add((WresIDGroup)wrestler);
                    }
                    catch
                    { }
                }

                L.D("Team List Count: " + teamList.Count());

                L.D("Waiting Opponent List Count: " + waitingOpponents.Count());
            }

        }
        private static void InitializeSettings()
        {

            isTag = MoreMatchTypes_Form.form.sr_tag.Checked;
            isReverse = MoreMatchTypes_Form.form.sr_reverse.Checked;
            playerEdit = MoreMatchTypes_Form.form.sr_wrestler.Text;
            secondEdit = MoreMatchTypes_Form.form.sr_second.Text;
            opponentTeam = MoreMatchTypes_Form.form.sr_teamName.Text;

            //Create Initial Wrestler List
            waitingOpponents = new List<WresIDGroup>();
            usedOpponents = new List<WresIDGroup>();
            teamList = new List<WresIDGroup>();
            InitializeLists();
            currOpponents = new WresIDGroup[2];

            //Adding single wrestler
            int.TryParse(MoreMatchTypes_Form.form.sr_singleOpponent.Text, out int singleID);
            currOpponents[0] = MatchConfiguration.GetWrestlerData(singleID, teamList);

            //Adding tag wrestler
            if (isTag)
            {
                int.TryParse(MoreMatchTypes_Form.form.sr_tagOpponent.Text, out int tagID);
                currOpponents[1] = MatchConfiguration.GetWrestlerData(tagID, teamList);
            }

            endMatch = false;
            endGame = false;
            loserTrack = -1;
            gameDetails = new int[9];
            teamNames = new String[2];
            SetTeamNames();

            //Setting game details
            gameDetails[0] = Convert.ToInt32(MoreMatchTypes_Form.form.sr_matches.Text);
            gameDetails[1] = Convert.ToInt32(MoreMatchTypes_Form.form.sr_continues.Text);
            second = MoreMatchTypes_Form.form.sr_second.Text;
        }
        private static void SetupMatch()
        {
            //Setting up the next match
            MatchSetting settings = GlobalWork.inst.MatchSetting;
            MatchMain main = MatchMain.inst;
            main.RoundCnt = 1;
            settings.is3GameMatch = false;
            settings.RoundNum = 0;
            settings.isSkipEntranceScene = true;
            settings.isCarryOverHP = true;
            settings.MatchCnt = 1;

            L.D("Recent loser: " + loserTrack);
            //Update teams if the player won
            if (loserTrack > 3)
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
            if (loserTrack > 3)
            {
                gameDetails[7] += currEval; //Only add the match rating for wins
            }
            if (currEval > gameDetails[6])
            {
                gameDetails[6] = currEval; //Replace the maximum match rating if applicable
            }

            try
            {
                if (endGame)
                {
                    result += "Game Over\nMatches Played: " + gameDetails[8] + "\nContinues Used: " + gameDetails[5] + "\nHighest Win Streak: " + gameDetails[3];
                    result += "\nTotal Losses: " + gameDetails[4] + "\nHighest Match Rating: " + gameDetails[6] + "%\nAverage Match Rating: " + gameDetails[7] / (gameDetails[8] - gameDetails[4]) + "%";

                }
                else if (endMatch)
                {
                    result += "\nMatches remaining: " + gameDetails[0] + "\nContinues remaining: " + gameDetails[1] + "\nCurrent Win Streak: " + gameDetails[2];
                    result += "\nHighest Match Rating: " + gameDetails[6] + "%\nAverage Match Rating: " + gameDetails[7] / (gameDetails[8] - gameDetails[4]) + "%";
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
            string info = "\nMatch #" + gameDetails[8] + "\n";
            string playerTeam = MatchConfiguration.GetWrestlerName(playerEdit);
            String opponent = opponentTeam;
            if (opponent.Equals(""))
            {
                opponent = currOpponents[0].ToString();
            }

            if (isTag)
            {
                playerTeam += " & " + MatchConfiguration.GetWrestlerName(secondEdit);
                if (opponentTeam.Equals(""))
                {
                    opponent += currOpponents[1];
                }
            }

            //Determine winner
            if (loserTrack > 3)
            {
                info += playerTeam + " defeated " + opponent + ".";
            }
            else
            {
                info += opponent + " defeated " + playerTeam + ".";
            }

            info += result;
            MoreMatchTypes_Form.form.sr_progress.Text += info + "\n\n";
        }

        #endregion
    }
}
