using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG;
using UnityEngine;

namespace MoreMatchTypes
{

    #region Access Modifiers
    [FieldAccess(Class = "Player", Field = "UpdatePlayer", Group = "MoreMatchTypes"), FieldAccess(Class = "Referee", Field = "Start_SubmissionCheck", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "MatchMain", Field = "ExitMatch", Group = "MoreMatchTypes"), FieldAccess(Class = "MatchMain", Field = "CreatePlayers", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "Menu_Result", Field = "Set_FinishSkill", Group = "MoreMatchTypes"), FieldAccess(Class = "MatchEvaluation", Field = "EvaluateSkill", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "MatchMain", Field = "InitRound", Group = "MoreMatchTypes"), FieldAccess(Class = "MatchMain", Field = "Update_Match", Group = "MoreMatchTypes")]
    #endregion
    class Pancrase
    {

        #region variables
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
        public static MatchTime currMatchTime = null;
        public static float[] currentBP;
        #endregion


        [Hook(TargetClass = "MatchMain", TargetMethod = "CreatePlayers", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void SetMatchRules()
        {
            MatchSetting settings = GlobalWork.inst.MatchSetting;

            if (settings.arena == VenueEnum.BarbedWire || settings.arena == VenueEnum.Cage || settings.arena == VenueEnum.Dodecagon || settings.arena == VenueEnum.LandMine_BarbedWire || settings.arena == VenueEnum.LandMine_FluorescentLamp)
            {
                return;
            }
            if (settings.BattleRoyalKind != BattleRoyalKindEnum.Off)
            {
                return;
            }

            isPancrase = false;
            if (MoreMatchTypes_Form.form.cb_Pancrase.Checked && IsOneOnOne())
            {
                isPancrase = true;

                //Populate Illegal Attacks
                String illegalString = MoreMatchTypes_Form.form.tb_illegal.Text.TrimStart().TrimEnd();
                if (illegalString != "")
                {
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
                String dqMoves = MoreMatchTypes_Form.form.tb_dq.Text.TrimStart().TrimEnd();
                if (dqMoves != "")
                {
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
            if (MSCForm.Instance.checkBox7.Checked)
            {
                fiveCount = true;
                MSCForm.Instance.checkBox7.Checked = false;
                MSCForm.Instance.checkBox7.Enabled = false;
            }

            foulChecked = false;
            checkKo = false;
            ropeBreak = false;
            dqChecked = false;
            points = new int[2];
            points[0] = 5;
            points[1] = 5;
            teamNames = new string[2];
            SetTeamNames();
            settings.VictoryCondition = VictoryConditionEnum.OnlyGiveUp;
            settings.is10CountKO = true;
            settings.isLumberjack = true;
            settings.isFoulCount = false;
            settings.isOutOfRingCount = false;
            resultText = "";
            currMatchTime = null;
            currentBP = new float[2];
            endMatch = false;
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "Update_Match", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void CheckRoundEnd()
        {
            if (endMatch || !isPancrase)
            {
                return;
            }

            ////Determine if round should end
            //if(ropeBreak || foulChecked)
            //{
            //    EndRound();
            //}
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

                //Get fighters back into the ring, if anyone falls out of it.
                if (plObj.Zone == ZoneEnum.OutOfRing || plObj.Zone == ZoneEnum.SlopeRunway || plObj.Zone == ZoneEnum.Stage || plObj.Zone == ZoneEnum.StageEntrance || plObj.Zone == ZoneEnum.HorizontalRunway || plObj.Zone == ZoneEnum.OutsideCage)
                {
                    EndRound();
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

                    CheckMatchEnd("Disqualification");
                }


            }
        }

        [Hook(TargetClass = "MatchEvaluation", TargetMethod = "EvaluateSkill", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassParametersVal, Group = "MoreMatchTypes")]
        public static void RemovePts(int plIDx, SkillData sd, SkillSlotAttr skillAttr)
        {
            if (isPancrase)
            {

                if (illegalMoves.Contains(sd.skillName[1]))
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

        [Hook(TargetClass = "Menu_Result", TargetMethod = "Set_FinishSkill", InjectionLocation = 8, InjectDirection = HookInjectDirection.After, InjectFlags = HookInjectFlags.PassParametersVal | HookInjectFlags.PassLocals, LocalVarIds = new int[] { 1 }, Group = "MoreMatchTypes")]
        public static void SetResultScreenDisplay(ref UILabel finishText, string str)
        {
            if (endMatch && isPancrase)
            {
                if (!resultText.Equals(""))
                {
                    string resultString = str.Replace("K.O.", resultText).Replace("DRAW", resultText);
                    finishText.text = resultString;
                }
                endMatch = false;
            }
        }
        [Hook(TargetClass = "MatchMain", TargetMethod = "InitRound", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        public static void StartRound(MatchMain m)
        {
            if (isPancrase && currMatchTime != null)
            {
                m.matchTime.Set(currMatchTime);
                PlayerMan.inst.GetPlObj(0).BP = currentBP[0];
                PlayerMan.inst.GetPlObj(4).BP = currentBP[1];
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "ExitMatch", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void ResetFiveCount()
        {
            if (isPancrase)
            {
                MSCForm.Instance.checkBox7.Checked = fiveCount;
                MSCForm.Instance.checkBox7.Enabled = true;
            }
        }
        public static void CheckBleeding(Player pl)
        {
        }

        #region Helper Methods
        //Ensure that the match is One vs One
        private static bool IsOneOnOne()
        {
            bool isOneOnOne = true;
            for (int i = 0; i < 8; i++)
            {
                //These spots should hold the fighters, therefore we do not need to check
                if (i == 0 || i == 4)
                {
                    continue;
                }
                Player pl = PlayerMan.inst.GetPlObj(i);

                //Ignore if this spot is empty.
                if (!pl)
                {
                    continue;
                }

                //If the spot includes another fighter, this is not a One vs One match
                if(!pl.isSecond)
                {
                    isOneOnOne = false;
                    break;
                }
            }
            return isOneOnOne;
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
                EndRound();
            }

        }

        private static void EndRound()
        {
            //Prepare the next round
            currentBP[0] = PlayerMan.inst.GetPlObj(0).BP;
            currentBP[1] = PlayerMan.inst.GetPlObj(4).BP;

            PlayerMan.inst.GetPlObj(0).BP = 0;
            PlayerMan.inst.GetPlObj(4).BP = 0;
            MatchMain main = MatchMain.inst;

            Referee matchRef = RefereeMan.inst.GetRefereeObj();
            matchRef.PlDir = PlDirEnum.Left;
            matchRef.ReqRefereeAnm(BasicSkillEnum.Refe_Stand_MatchEnd_Front_Left);

            main.isMatchEnd = false;
            main.isRoundEnd = true;

            currMatchTime = new MatchTime
            {
                min = main.matchTime.min,
                sec = main.matchTime.sec
            };
            main.isTimeCounting = false;
            foulChecked = false;
            ropeBreak = false;
            checkKo = false;
            dqChecked = false;
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
                    if (!GetTeamName(wrestlers, out teamNames[0]))
                    {
                        teamNames[0] = "Blue Team";
                    }
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
                    if (!GetTeamName(wrestlers, out teamNames[1]))
                    {
                        teamNames[1] = "Red Team";
                    }
                }
            }
            catch
            {
                teamNames[0] = "Blue Team";
                teamNames[1] = "Red Team";
            }

        }

        public static bool GetTeamName(List<string> members, out string result)
        {
            result = string.Empty;

            foreach (Team t in DG.TagTeamCreatorForm.Teams)
            {
                if (Contains(members, t.Members))
                {
                    result = t.Name;
                    return true;
                }
            }

            return false;
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

        #endregion
    }
}

