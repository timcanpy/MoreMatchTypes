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
    #endregion
    class Uwfi
    {
        #region Variables
        public static int[] points = new int[2];
        public static int[] foulCount = new int[2];
        public static string[] teamNames = new string[2];
        public static bool[] tdRecorded = new bool[8] { false, false, false, false, false, false, false, false };
        public static bool[] koRecorded = new bool[8] { false, false, false, false, false, false, false, false };
        public static bool fiveCount = false;
        public static bool isUwfi = false;
        public static bool ptEndMatch = false;
        public static int[] bloodState = new int[2];
        public static string resultText;
        public static List<String> illegalMoves;
        public static List<String> instantDQ;
        public static int bleedCeiling = 0;
        public static String refName = "";
        #endregion

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
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

            isUwfi = MoreMatchTypes_Form.moreMatchTypesForm.cb_uwfi.Checked;
            if (!isUwfi)
            {
                return;
            }

            //Populate Illegal Attacks
            String illegalString = MoreMatchTypes_Form.moreMatchTypesForm.tb_illegal.Text.TrimStart().TrimEnd();
            if (illegalString != "")
            {
                illegalMoves = CreateMoveList(illegalString);
            }
            else
            {
                List<String> illegalMoves = new List<String>()
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
                "Back Mount Punches"

            };
            }

            //Populate DQ Attacks
            String dqMoves = MoreMatchTypes_Form.moreMatchTypesForm.tb_dq.Text.TrimStart().TrimEnd();
            if (dqMoves != "")
            {
                instantDQ = CreateMoveList(dqMoves);
            }
            else
            {
                instantDQ = new List<String>()
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

            settings.isLumberjack = true;
            settings.isFoulCount = false;
            settings.isOutOfRingCount = false;
            settings.isCutPlay = false;
            settings.MatchTime = 0;
            settings.is10CountKO = true;
            settings.isExchangeOfStriking = false;

            tdRecorded = new bool[8] { false, false, false, false, false, false, false, false };
            koRecorded = new bool[8] { false, false, false, false, false, false, false, false };
            foulCount = new int[2];
            resultText = "";
            ptEndMatch = false;
            fiveCount = false;
            points[0] = 15;
            points[1] = 15;

            //Check for multi-man teams
            if (settings.matchWrestlerInfo[1].entry && !settings.matchWrestlerInfo[1].isSecond && !settings.matchWrestlerInfo[1].isSecond)
            {
                points[0] = 21;
            }

            if (settings.matchWrestlerInfo[5].entry && !settings.matchWrestlerInfo[5].isSecond && !settings.matchWrestlerInfo[5].isIntruder)
            {
                points[0] = 21;
            }

            SetTeamNames();

            Referee mref = RefereeMan.inst.GetRefereeObj();
            bleedCeiling = 5 - mref.RefePrm.interfereTime;
            refName = mref.RefePrm.name;
        }

        [Hook(TargetClass = "Player", TargetMethod = "UpdatePlayer", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        public static void TrackPlayer(Player plObj)
        {
            if (!isUwfi)
            {
                return;
            }

            if (plObj.State == PlStateEnum.S_KOCNT)
            {
                if (!koRecorded[plObj.PlIdx])
                {
                    if (plObj.PlIdx < 4)
                    {
                        points[0] -= 4;

                        points[0] = CheckPoints(points[0]);
                    }
                    else
                    {
                        points[1] -= 4;

                        points[1] = CheckPoints(points[1]);
                    }
                }

                DisplayScore("Down");

                koRecorded[plObj.PlIdx] = true;
                tdRecorded[plObj.PlIdx] = true;

                CheckForLoss();

            }
            else if (plObj.State == PlStateEnum.Down_FaceDown || plObj.State == PlStateEnum.Down_FaceUp)
            {
                if (!tdRecorded[plObj.PlIdx])
                {
                    if (plObj.DownTime > 300 && plObj.LastDamage >= 3000)
                    {
                        if (plObj.PlIdx < 4)
                        {
                            points[0] -= 3;

                            if (points[0] < 0)
                            {
                                points[0] = 0;
                            }
                        }
                        else
                        {
                            points[1] -= 3;

                            if (points[1] < 0)
                            {
                                points[1] = 0;
                            }
                        }


                        DisplayScore("Down");
                        tdRecorded[plObj.PlIdx] = true;

                        if (points[0] <= 0 && points[1] <= 0)
                        {
                            TriggerLoss(0);
                        }
                        if (points[0] <= 0)
                        {
                            TriggerLoss(1);
                        }
                        if (points[1] <= 0)
                        {
                            TriggerLoss(2);
                        }
                    }
                }
            }

            if (koRecorded[plObj.PlIdx])
            {
                if (plObj.State != PlStateEnum.S_KOCNT)
                {
                    koRecorded[plObj.PlIdx] = false;
                }
            }

            if (tdRecorded[plObj.PlIdx])
            {
                if (plObj.State < (PlStateEnum)6)
                {
                    tdRecorded[plObj.PlIdx] = false;
                }
            }
        }

        [Hook(TargetClass = "MatchEvaluation", TargetMethod = "EvaluateSkill", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassParametersVal, Group = "MoreMatchTypes")]
        public static void RemovePts(int plIDx, SkillData sd, SkillSlotAttr skillAttr)
        {
            if (!isUwfi)
            { return; }

            bool ptChange = false;

            if (sd.filteringType == SkillFilteringType.Performance)
            {
                return;
            }

            Player attacker = PlayerMan.inst.GetPlObj(plIDx);
            Player defender = PlayerMan.inst.GetPlObj(attacker.TargetPlIdx);

            if (instantDQ.Contains(sd.skillName[(int)global::SaveData.inst.optionSettings.language]))
            {

                if (plIDx < 4)
                {
                    points[0] -= 22;
                    points[0] = CheckPoints(points[0]);
                }
                else
                {
                    points[1] -= 22;
                    points[1] = CheckPoints(points[1]);
                }

                ptChange = true;
                DisplayScore("Disqualification");
                HandleFoul();
            }
            else if (illegalMoves.Contains(sd.skillName[(int)global::SaveData.inst.optionSettings.language]) || sd.flags == SkillData.SkillFlags.FoulTech )
            {

                if (plIDx < 4)
                {
                    foulCount[0]++;
                    points[0] -= foulCount[0];
                    points[0] = CheckPoints(points[0]);
                }
                else
                {
                    foulCount[1]++;
                    points[1] -= foulCount[1];
                    points[1] = CheckPoints(points[1]);
                }

                ptChange = true;
                DisplayScore("Illegal " + sd.skillName[(int)global::SaveData.inst.optionSettings.language]);
                HandleFoul();
            }
            else if (sd.filteringType == SkillFilteringType.Headbutt)
            {

                if (plIDx < 4)
                {
                    foulCount[0]++;
                    points[0] -= foulCount[0];
                    points[0] = CheckPoints(points[0]);
                }
                else
                {
                    foulCount[1]++;
                    points[1] -= foulCount[1];
                    points[1] = CheckPoints(points[1]);
                }

                ptChange = true;
                DisplayScore("Foul");
                HandleFoul();
            }
            else if (sd.filteringType == SkillFilteringType.Suplex)
            {
                if (plIDx < 4)
                {
                    points[1]--;
                    points[1] = CheckPoints(points[1]);
                }
                else
                {
                    points[0]--;
                    points[0] = CheckPoints(points[0]);
                }

                ptChange = true;
                if (sd.filteringType == SkillFilteringType.Suplex)
                {
                    DisplayScore("Suplex");
                }
            }

            if (!CheckForLoss())
            {
                //Suplexes are legal, and the match should continue
                if (sd.filteringType != SkillFilteringType.Suplex && ptChange)
                {
                    ForceCleanBreak();
                }
            }
        }

        [Hook(TargetClass = "Referee", TargetMethod = "Start_SubmissionCheck", InjectionLocation = 36, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        public static void RopeBreakPtLoss(Referee mRef)
        {
            if (!isUwfi)
            { return; }

            if (mRef.TargetPlIdx < 4)
            {
                points[0]--;
                CheckPoints(points[0]);
            }
            else
            {
                points[1]--;
                CheckPoints(points[1]);
            }

            DisplayScore("Rope Break");

            CheckForLoss();
        }

        [Hook(TargetClass = "Menu_Result", TargetMethod = "Set_FinishSkill", InjectionLocation = 0, InjectDirection = HookInjectDirection.After, InjectFlags = HookInjectFlags.PassParametersRef, Group = "MoreMatchTypes")]
        public static void SetResultScreenDisplay(ref string str)
        {
            if (!isUwfi)
            {
                return;
            }

            try
            {
                if (ptEndMatch && isUwfi)
                {
                    if (!resultText.Equals(String.Empty))
                    {
                        //Get match time
                        string time = Regex.Split(str, Environment.NewLine)[0];
                        str = time + Environment.NewLine + resultText;
                    }
                }
            }
            catch (IndexOutOfRangeException e)
            { }
            catch (Exception ex)
            {
                L.D("UwfiSetResultException: " + ex);
            }
            finally
            {
                ptEndMatch = false;
            }

        }

        [Hook(TargetClass = "Referee", TargetMethod = "CheckStartRefereeing", InjectionLocation = 336,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassParametersVal, ParamTypes = new Type[]
            {
                typeof(int)
            }, Group = "MoreMatchTypes")]
        public static void CheckForTKO_UWFI(int pl_idx)
        {
            if (isUwfi && pl_idx >= 0)
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

                if (point - 4 <= 0)
                {
                    GlobalWork.inst.MatchSetting.TKOCount = 1;
                    global::PlayerMan.inst.GetPlObj(pl_idx).TKO_Count = 1;
                }
            }
        }

        [Hook(TargetClass = "Player", TargetMethod = "Bleeding", InjectionLocation = 0, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        public static void CheckBleeding(Player p)
        {
            if (!isUwfi)
            {
                return;
            }
            int bloodLevel = global::MatchEvaluation.inst.PlResult[p.PlIdx].bledCnt;
            if (bloodLevel == bleedCeiling - 1)
            {
                MatchConfiguration.ShowCommentaryMessage(refName + " is watching " + DataBase.GetWrestlerFullName(p.WresParam) + " closely.");
            }
            else if (bloodLevel >= bleedCeiling)
            {
                MatchConfiguration.ShowCommentaryMessage(refName + " calls for a doctor stoppage!");
                Referee mRef = RefereeMan.inst.GetRefereeObj();
                mRef.PlDir = PlDirEnum.Left;
                mRef.State = RefeStateEnum.DeclareVictory;
                mRef.ReqRefereeAnm(BasicSkillEnum.Refe_Stand_MatchEnd_Front_Left);
                mRef.matchResult = MatchResultEnum.GiveUp;
                PlayerMan.inst.GetPlObj(p.PlIdx).isLoseAndStop = true;
                mRef.SentenceLose(p.PlIdx);
            }
        }

        #region Helper methods
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
        private static void TriggerLoss(int team)
        {
            #region Basic Skill Enum
            global::BasicSkillEnum[] RefAnmTbl = new global::BasicSkillEnum[56];
            RefAnmTbl[0] = global::BasicSkillEnum.HSTDH;
            RefAnmTbl[1] = global::BasicSkillEnum.FSTDH;
            RefAnmTbl[2] = global::BasicSkillEnum.BSTDH;
            RefAnmTbl[3] = global::BasicSkillEnum.HSTDM;
            RefAnmTbl[4] = global::BasicSkillEnum.FSTDM;
            RefAnmTbl[5] = global::BasicSkillEnum.BSTDM;
            RefAnmTbl[9] = global::BasicSkillEnum.Refe_Walk_S;
            RefAnmTbl[10] = global::BasicSkillEnum.Refe_Walk_D;
            RefAnmTbl[11] = global::BasicSkillEnum.Refe_Walk_U;
            RefAnmTbl[12] = global::BasicSkillEnum.HCNT_F;
            RefAnmTbl[13] = global::BasicSkillEnum.FCNT_F;
            RefAnmTbl[14] = global::BasicSkillEnum.BCNT_F;
            RefAnmTbl[15] = global::BasicSkillEnum.HCNT_S;
            RefAnmTbl[16] = global::BasicSkillEnum.FCNT_S;
            RefAnmTbl[17] = global::BasicSkillEnum.BCNT_S;
            RefAnmTbl[18] = global::BasicSkillEnum.HDOWN;
            RefAnmTbl[19] = global::BasicSkillEnum.FDOWN;
            RefAnmTbl[20] = global::BasicSkillEnum.BDOWN;
            RefAnmTbl[21] = global::BasicSkillEnum.FFALL1;
            RefAnmTbl[22] = global::BasicSkillEnum.BFALL1;
            RefAnmTbl[23] = global::BasicSkillEnum.FFALL2;
            RefAnmTbl[24] = global::BasicSkillEnum.BFALL2;
            RefAnmTbl[25] = global::BasicSkillEnum.FFALL3;
            RefAnmTbl[26] = global::BasicSkillEnum.BFALL3;
            RefAnmTbl[27] = global::BasicSkillEnum.FFALL4;
            RefAnmTbl[28] = global::BasicSkillEnum.BFALL4;
            RefAnmTbl[29] = global::BasicSkillEnum.Refe_StandUp_Front;
            RefAnmTbl[30] = global::BasicSkillEnum.Refe_StandUp_Back;
            RefAnmTbl[31] = global::BasicSkillEnum.FUPEND;
            RefAnmTbl[32] = global::BasicSkillEnum.BUPEND;
            RefAnmTbl[33] = global::BasicSkillEnum.FBREAK;
            RefAnmTbl[34] = global::BasicSkillEnum.BBREAK;
            RefAnmTbl[35] = global::BasicSkillEnum.Refe_CheckGiveUp_Front_Start;
            RefAnmTbl[36] = global::BasicSkillEnum.Refe_CheckGiveUp_Back_Start;
            RefAnmTbl[37] = global::BasicSkillEnum.Refe_CheckGiveUp_Front_Loop;
            RefAnmTbl[38] = global::BasicSkillEnum.Refe_CheckGiveUp_Back_Loop;
            RefAnmTbl[39] = global::BasicSkillEnum.GLFEND;
            RefAnmTbl[40] = global::BasicSkillEnum.GLBEND;
            RefAnmTbl[41] = global::BasicSkillEnum.GRFEND;
            RefAnmTbl[42] = global::BasicSkillEnum.GRBEND;
            RefAnmTbl[43] = global::BasicSkillEnum.Refe_Stand_MatchEnd_Front_Left;
            RefAnmTbl[44] = global::BasicSkillEnum.Refe_Stand_MatchEnd_Back_Left;
            RefAnmTbl[45] = global::BasicSkillEnum.Refe_Stand_MatchEnd_Front_Right;
            RefAnmTbl[46] = global::BasicSkillEnum.Refe_Stand_MatchEnd_Back_Right;
            RefAnmTbl[47] = global::BasicSkillEnum.FKOCHK;
            RefAnmTbl[48] = global::BasicSkillEnum.BKOCHK;
            RefAnmTbl[49] = global::BasicSkillEnum.FFALL5;
            RefAnmTbl[50] = global::BasicSkillEnum.BFALL5;
            RefAnmTbl[51] = global::BasicSkillEnum.ComplaintHandling_2;
            RefAnmTbl[52] = global::BasicSkillEnum.ComplaintHandling_1;
            RefAnmTbl[53] = global::BasicSkillEnum.CheckFoulLoop_2;
            RefAnmTbl[54] = global::BasicSkillEnum.CheckFoulLoop_1;
            #endregion

            Referee mref = RefereeMan.inst.GetRefereeObj();
            mref.PlDir = PlDirEnum.Left;
            mref.State = RefeStateEnum.DeclareVictory;
            mref.matchResult = MatchResultEnum.KO;
            mref.ReqRefereeAnm(RefAnmTbl[31 + MatchData.AnmOfsTbl_2Dir[(int)mref.PlDir]]);

            int loser = -1;
            switch (team)
            {
                case 0:
                    mref.SentenceLose(0);
                    mref.SentenceLose(4);
                    mref.matchResult = MatchResultEnum.KODraw;
                    resultText = "Draw";
                    break;
                case 1:
                    for (int i = 0; i < 4; i++)
                    {
                        if (PlayerMan.inst.GetPlObj(i).hasRight)
                        {
                            loser = i;
                            break;
                        }
                    }
                    mref.SentenceLose(loser);
                    PlayerMan.inst.GetPlObj(loser).isLoseAndStop = true;
                    ptEndMatch = true;
                    resultText = "T.K.O. By Point Loss";
                    break;

                case 2:
                    for (int i = 4; i < 7; i++)
                    {
                        if (PlayerMan.inst.GetPlObj(i).hasRight)
                        {
                            loser = i;
                            break;
                        }
                    }
                    mref.SentenceLose(loser);
                    PlayerMan.inst.GetPlObj(loser).isLoseAndStop = true;
                    ptEndMatch = true;
                    resultText = "T.K.O. By Point Loss";
                    break;
                default:
                    break;
            }
        }
        private static int CheckPoints(int points)
        {
            if (points < 0)
            {
                points = 0;
            }

            return points;
        }
        private static bool CheckForLoss()
        {
            bool isLoss = false;
            if (points[0] <= 0 && points[1] <= 0)
            {
                TriggerLoss(0);
                isLoss = true;
            }
            if (points[0] <= 0)
            {
                TriggerLoss(1);
                isLoss = true;
            }
            if (points[1] <= 0)
            {
                TriggerLoss(2);
                isLoss = true;
            }

            return isLoss;
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
        private static void HandleFoul()
        {
            Audience.inst.TensionDown();
        }
        private static void DisplayScore(String reason)
        {
            MatchConfiguration.ShowAnnouncement(reason + " -\t  " + teamNames[0] + ": " + points[0] + " points \t\t" + teamNames[1] + ": " + points[1] + " points");
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
