using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DG;
using MatchConfig;
using ModPack;
using UnityEngine;

namespace MoreMatchTypes
{

    #region Access Modifiers
    [FieldAccess(Class = "Player", Field = "UpdatePlayer", Group = "MoreMatchTypes"), FieldAccess(Class = "Referee", Field = "Start_SubmissionCheck", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "MatchMain", Field = "ExitMatch", Group = "MoreMatchTypes"), FieldAccess(Class = "MatchMain", Field = "InitMatch", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "Menu_Result", Field = "Set_FinishSkill", Group = "MoreMatchTypes"), FieldAccess(Class = "MatchEvaluation", Field = "EvaluateSkill", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "MatchMain", Field = "InitRound", Group = "MoreMatchTypes"), FieldAccess(Class = "MatchMain", Field = "Update_Match", Group = "MoreMatchTypes"), FieldAccess(Class = "Player", Field = "SetGrappleResult", Group = "MoreMatchTypes")]
    #endregion
    class Pancrase
    {

        #region Variables
        public static List<String> illegalMoves = new List<String>();
        public static List<String> instantDQ;
        public static bool isPancrase;
        public static bool endMatch;
        public static bool fiveCount;
        public static bool foulChecked;
        public static bool dqChecked;
        public static bool ropeBreak;
        public static bool checkKo;
        public static int[] points;
        public static string resultText;
        public static string[] teamNames;
        #endregion

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void SetMatchRules()
        {
            MatchSetting settings = GlobalWork.inst.MatchSetting;
            isPancrase = false;

            if (settings.arena == VenueEnum.BarbedWire || settings.arena == VenueEnum.Cage || settings.arena == VenueEnum.Dodecagon || settings.arena == VenueEnum.LandMine_BarbedWire || settings.arena == VenueEnum.LandMine_FluorescentLamp)
            {
                return;
            }
            if (settings.BattleRoyalKind != BattleRoyalKindEnum.Off)
            {
                return;
            }

            if (MoreMatchTypes_Form.moreMatchTypesForm.cb_Pancrase.Checked && IsOneOnOne())
            {
                isPancrase = true;

                //Populate Illegal Attacks
                String illegalString = MoreMatchTypes_Form.moreMatchTypesForm.tb_illegal.Text.TrimStart().TrimEnd();
                if (illegalString != "")
                {
                    L.D("Create Pancrase Illegal Moves");
                    illegalMoves = CreateMoveList(illegalString);
                }
                else
                {
                    illegalMoves = new List<string>()
                      {
                        "Knuckle Arrow",
                        "Knuckle Pat",
                        "Elbow to the Crown",
                        "Elbow Stamp",
                        "Elbow Stamp (Neck)",
                        "Elbow Stamp (Arm)",
                        "Elbow Stamp (Leg)",
                        "Stomping (Face)",
                        "Stomping (Neck)",
                        "Clap Kick",
                        "Thumbing to the Eyes",
                        "Thumbing to the Eyes B",
                        "Face Raking",
                        "Choke Attack",
                        "Cobra Claw",
                        "Headbutt",
                        "Headbutt Rush",
                        "Jumping Headbutt",
                        "Leg-Lift Headbutt Rush",
                        "No-Touch Headbutt",
                        "Enzui Headbutt",
                        "Manhattan Drop",
                        "Manhattan Drop B",
                        "Mount Headbutt",
                        "Mount Knuckle Arrow",
                        "Corner Headbutt Rush",
                        "Rope Trailing",
                        "Guillotine Whip",
                        "Corner Strike Rush",
                        "Mount Punches",
                        "Back Mount Punches",
                        "Reverse Hanging Punch",
                        "Straight",
                        "Jab",
                        "American Jab",
                        "American Hook",
                        "Combination 3",
                        "Dash Straight",
                        "Hook",
                        "Russian Hook",
                        "Uppercut",
                        "Uraken",
                        "Superman Punch",
                        "American Punch Rush",
                        "Dazed Texas Jab",
                        "Discus Punch",
                        "Dynamite Punch Rush",
                        "Face Straight",
                        "Force Straight Punch",
                        "Force Straight Punch B",
                        "Grapple Punch Rush",
                        "Headlock Punches",
                        "Mach Punch Rush",
                        "Russian Hook Rush",
                        "Seikentsuki Punch",
                        "Seikentsuki Punch Rush",
                        "Short Uppercut",
                        "Spirit Punch Rush",
                        "Texas Jab",
                        "Wrist Punch",
                        "Bionic Elbow",
                        "Final Rolling Elbow",
                        "Face Punch",
                        "Face Punch Rush",
                        "Jumping Fist Drop",
                        "Bite",
                        "Cobra Claw",
                        "Guillotine Choke",
                        "Back-Mount Punches",
                        "Corner Punch Rush",
                        "Mount Knuckle Arrow",
                        "Leg Clip Punch",
                        "Punch",
                        "Elbow"
                    };
                }

                //Populate DQ Attacks
                String dqMoves = MoreMatchTypes_Form.moreMatchTypesForm.tb_dq.Text.TrimStart().TrimEnd();
                if (dqMoves != "")
                {
                    L.D("Create Pancrase DQ Moves");
                    instantDQ = CreateMoveList(dqMoves);
                }
                else
                {
                    instantDQ = new List<string>()
                          {
                            "Giant Steel Knuckles",
                            "Brass Knuckle Punch",
                            "Weapon Attack",
                            "Scythe Attack",
                            "Bite",
                            "Testicular Claw",
                            "Chair's Illusion",
                            "Low Blow",
                            "Lip Lock",
                            "Back Low Blow",
                            "Groin Head Drop",
                            "Groin Knee Stamp",
                            "Groin Stomping",
                            "Ategai",
                            "Bronco Buster",
                            "Mist",
                            "Big Fire"
                        };

                }
            }
            if (!isPancrase)
            {
                return;
            }

            settings.VictoryCondition = VictoryConditionEnum.OnlyGiveUp;
            settings.isLumberjack = true;
            settings.isFoulCount = false;
            settings.isOutOfRingCount = false;
            settings.is10CountKO = true;
            settings.MatchTime = 0;

            resultText = "";
            foulChecked = false;
            checkKo = false;
            ropeBreak = false;
            dqChecked = false;
            endMatch = false;
            points = new int[2];
            teamNames = new string[2];
            points[0] = 5;
            points[1] = 5;

            SetTeamNames();
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "Update_Match", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void CheckRoundEnd()
        {
            if (endMatch || !isPancrase)
            {
                return;
            }
        }

        [Hook(TargetClass = "Referee", TargetMethod = "Start_SubmissionCheck", InjectionLocation = 36, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        public static void RopeBreakPtLoss(Referee mRef)
        {
            if (isPancrase)
            {

                if (mRef.TargetPlIdx < 4)
                {
                    points[0]--;
                    ropeBreak = true;
                }
                else
                {
                    points[1]--;
                    ropeBreak = true;
                }

                if (ropeBreak)
                {
                    CheckMatchEnd("Rope Break");
                }
            }
        }

        [Hook(TargetClass = "Player", TargetMethod = "UpdatePlayer", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        public static void TrackPlayer(Player plObj)
        {
            if (isPancrase)
            {
                //Point Deduction for Knock Downs
                if (plObj.State == PlStateEnum.S_KOCNT && !checkKo)
                {
                    if (plObj.PlIdx < 4)
                    {
                        points[0]--;
                        checkKo = true;
                    }
                    else
                    {
                        points[1]--;
                        checkKo = true;
                    }
                }

                //Ensure that all players are standing before the check continues
                if (checkKo && !MatchMain.inst.isMatchEnd)
                {
                    Player p1 = PlayerMan.inst.GetPlObj(0);
                    Player p2 = PlayerMan.inst.GetPlObj(4);
                    if (p1.State != PlStateEnum.S_KOCNT && p2.State != PlStateEnum.S_KOCNT)
                    {
                        CheckMatchEnd("Down");
                    }
                }

                //Disqualify wrestlers for climbing the turnbuckles.
                if (plObj.Zone == ZoneEnum.OnCornerPost)
                {
                    if (plObj.PlIdx < 4)
                    {
                        points[0] = 0;
                        checkKo = true;
                    }
                    else
                    {
                        points[1] = 0;
                        checkKo = true;
                    }

                    HandleFoul();
                    CheckMatchEnd("Disqualification");
                }


            }
        }

        [Hook(TargetClass = "MatchEvaluation", TargetMethod = "EvaluateSkill", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassParametersVal, Group = "MoreMatchTypes")]
        public static void RemovePts(int plIDx, SkillData sd, SkillSlotAttr skillAttr)
        {
            if (isPancrase)
            {

                if (instantDQ.Contains(sd.skillName[1]))
                {

                    if (plIDx < 4)
                    {
                        points[0] = 0;
                        dqChecked = true;
                    }
                    else
                    {
                        points[1] = 0;
                        dqChecked = true;
                    }
                }
                else if (illegalMoves.Contains(sd.skillName[1]))
                {
                    if (plIDx < 4)
                    {
                        points[0]--;
                        foulChecked = true;
                    }
                    else
                    {
                        points[1]--;
                        foulChecked = true;
                    }

                }
                
                if (foulChecked)
                {
                    PlayerMan.inst.GetPlObj(plIDx).animator.isReqAnmLoopEnd = true;
                    CheckMatchEnd("Foul");
                }
                if (dqChecked)
                {
                    CheckMatchEnd("Disqualification");
                }
            }
        }

        [Hook(TargetClass = "Menu_Result", TargetMethod = "Set_FinishSkill", InjectionLocation = 0, InjectDirection = HookInjectDirection.After, InjectFlags = HookInjectFlags.PassParametersRef, Group = "MoreMatchTypes")]
        public static void SetResultScreenDisplay(ref string str)
        {
            try
            {
                if (endMatch && isPancrase)
                {
                    if (!resultText.Equals(String.Empty))
                    {
                        //Get match time
                        string time = Regex.Split(str, Environment.NewLine)[0];
                        str = time + Environment.NewLine + resultText;
                    }
                }
            }
            catch (IndexOutOfRangeException)
            { }
            catch (Exception ex)
            {
                L.D("PancraseSetResultException: " + ex);
            }
            finally
            {
                endMatch = false;
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "EndMatch", InjectionLocation = 0,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void CleanUp()
        {
            if (!isPancrase)
            {
                return;
            }
        }

        [Hook(TargetClass = "Referee", TargetMethod = "CheckStartRefereeing", InjectionLocation = 335,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassParametersVal, ParamTypes = new Type[]
            {
                typeof(int)
            }, Group = "MoreMatchTypes")]
        public static void CheckForTKO_Pancrase(int pl_idx)
        {
            if (isPancrase)
            {
                int point = 0;

                if (pl_idx < 4)
                {
                    point = points[0];
                }
                else
                {
                    point = points[1];
                }

                if (point - 1 <= 0)
                {
                    GlobalWork.inst.MatchSetting.TKOCount = 1;
                    global::PlayerMan.inst.GetPlObj(pl_idx).TKO_Count = 1;
                }
            }
        }

        #region Helper Methods
        //Ensure that the match is One vs One
        private static bool IsOneOnOne()
        {
            return MatchConfiguration.GetPlayerCount() == 2;
        }
        private static void TriggerLoss(int team, string reason)
        {
            DispNotification.inst.Show(reason + " -\t" + teamNames[0] + ": " + points[0] + " points \t\t" + teamNames[1] + ": " + points[1] + " points", 300);
            Referee mref = RefereeMan.inst.GetRefereeObj();
            mref.PlDir = PlDirEnum.Left;
            mref.State = RefeStateEnum.DeclareVictory;
            mref.matchResult = MatchResultEnum.KO;
            mref.ReqRefereeAnm(BasicSkillEnum.Refe_Stand_MatchEnd_Front_Left);
            resultText = "T.K.O. Due to Point Loss";
            PlayerMan.inst.GetPlObj(team).isLoseAndStop = true;
            endMatch = true;
            mref.SentenceLose(team);
        }
        private static void CheckMatchEnd(string reason)
        {
            if (endMatch)
            {
                return;
            }

            if (points[0] <= 0)
            {
                TriggerLoss(0, reason);
            }
            else if (points[1] <= 0)
            {
                TriggerLoss(4, reason);
            }
            else
            {
                DispNotification.inst.Show(reason + " -\t" + teamNames[0] + ": " + points[0] + " points \t\t" + teamNames[1] + ": " + points[1] + " points", 300);
                ForceCleanBreak(); //Allows the round to continue
                ResumeAfterBreak();
            }

        }
        private static void ResumeAfterBreak()
        {
            foulChecked = false;
            ropeBreak = false;
            checkKo = false;
            dqChecked = false;
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
        private static List<String> CreateMoveList(String moveList)
        {
            char[] separators = new char[4] { ',', '|', ';', '\n' };

            List<string> modifiedList;
            modifiedList = moveList.Split(separators).ToList();

            foreach (String move in modifiedList)
            {
                move.TrimStart().TrimEnd();
            }
            return modifiedList;
        }
        public static void HandleFoul()
        {
            Audience.inst.TensionDown();
        }
        private static void ForceCleanBreak()
        {
            for (int i = 0; i < 8; i++)
            {
                Player pl = PlayerMan.inst.GetPlObj(i);
                if (!pl)
                {
                    continue;
                }

                pl.DownTime = 0;

                //Force Submission Breaks
                if (pl.isSubmissionAtk)
                {
                    L.D("Player releases submission");
                    pl.plCont_AI.padPush = PadBtnEnum.Atk_M;
                };

                if (!pl.State.ToString().Contains("Down") && !pl.isSubmissionAtk && !pl.isSubmissionDef)
                {
                    pl.Start_ForceControl(global::ForceCtrlEnum.WaitMatchStart);
                }
            }

            //Do not perform at the start of a match.
            MatchMain main = MatchMain.inst;
            if (main.matchTime.min == 0 && main.matchTime.sec == 0)
            {
                return;
            }

            Referee mRef = RefereeMan.inst.GetRefereeObj();
            global::MatchSEPlayer.inst.PlayRefereeVoice(global::RefeVoiceEnum.Break);
            mRef.State = global::RefeStateEnum.CallBeforeMatch_1;
            mRef.ReqRefereeAnm(global::BasicSkillEnum.ROUNDF);
            mRef.UpdateRefereeAnm();
        }

        #endregion
    }
}

