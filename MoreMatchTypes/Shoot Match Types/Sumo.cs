using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DG;
using MatchConfig;
using UnityEngine;


namespace MoreMatchTypes
{
    [FieldAccess(Class = "Player", Field = "UpdatePlayer", Group = "MoreMatchTypes"), FieldAccess(Class = "Player", Field = "SetGrappleResult", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "Menu_Result", Field = "Set_FinishSkill", Group = "MoreMatchTypes"), FieldAccess(Class = "MatchEvaluation", Field = "EvaluateSkill", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "MatchMain", Field = "InitRound", Group = "MoreMatchTypes"), FieldAccess(Class = "Player", Field = "UpdatePlayer", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "Referee", Field = "SentenceLose", Group = "MoreMatchTypes")]
    class Sumo
    {
        #region Variables
        public static List<string> basicAttacks;

        public static bool isSumo = false;
        public static string resultText;
        public static bool endMatch = false;

        #endregion

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void SetMatchRules()
        {
            MatchSetting settings = GlobalWork.inst.MatchSetting;
            isSumo = false;
            endMatch = false;

            if (settings.arena == VenueEnum.BarbedWire || settings.arena == VenueEnum.Cage || settings.arena == VenueEnum.LandMine_BarbedWire || settings.arena == VenueEnum.LandMine_FluorescentLamp)
            {
                return;
            }
            if (settings.BattleRoyalKind != BattleRoyalKindEnum.Off)
            {
                return;
            }

            if (!isOneOnOne())
            {
                return;
            }
            if (MoreMatchTypes_Form.moreMatchTypesForm.cb_sumo.Checked)
            {
                isSumo = true;
                settings.isReversalOff = true;
                settings.isOverTheTopRopeOn = true;
                settings.isPowerCompetitionOff = true;
                settings.isExchangeOfStriking = false;
                settings.isSkipEntranceScene = true;
                settings.MatchTime = 0;

                //Populate Basic Attacks
                String basicMoves = MoreMatchTypes_Form.moreMatchTypesForm.tb_basic.Text.TrimStart().TrimEnd();
                if (basicMoves != "")
                {
                    L.D("Create Sumo Basic Moves");
                    basicAttacks = CreateMoveList(basicMoves);
                }
                else
                {
                    basicAttacks = new List<string>()
                    {
                        "Shoutei",
                        "Face Slap B",
                        "Chest Slap",
                        "Knife-Edge Chop",
                        "Koppo Style Shoutei",
                        "Throat Chop",
                        "Jigoku-Tsuki"
                    };
                }
            }
            else
            {
                return;
            }

            resultText = "";
        }

        [Hook(TargetClass = "MatchEvaluation", TargetMethod = "EvaluateSkill", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassParametersVal, Group = "MoreMatchTypes")]
        public static void SetVictoryCondition(int plIDx, SkillData sd, SkillSlotAttr skillAttr)
        {
            if (!isSumo || sd.skillType == SkillTypeEnum.Performance)
            {
                return;
            }

            Player attacker = PlayerMan.inst.GetPlObj(plIDx);
            Player defender = PlayerMan.inst.GetPlObj(attacker.TargetPlIdx);
            Referee mRef = RefereeMan.inst.GetRefereeObj();

            if (basicAttacks.Contains(sd.skillName[1]) || (sd.anmType == SkillAnmTypeEnum.Single))
            {
                return;
            }

            float rngValue = UnityEngine.Random.Range(1, 3);

            //Handle back grapples
            if (attacker.animator.SkillSlotID == SkillSlotEnum.Back_X || attacker.animator.SkillSlotID == SkillSlotEnum.Back_A || attacker.animator.SkillSlotID == SkillSlotEnum.Back_B || attacker.animator.SkillSlotID == SkillSlotEnum.Back_B_UD || attacker.animator.SkillSlotID == SkillSlotEnum.Back_B_LR || attacker.animator.SkillSlotID == SkillSlotEnum.Back_XA)
            {
                if (rngValue == 1)
                {
                    attacker.animator.ReqBasicAnm(global::BasicSkillEnum.S1_Substitution_BackHold, true, defender.TargetPlIdx);
                }
                else
                {
                    defender.animator.ReqBasicAnm(global::BasicSkillEnum.S1_Substitution_BackRev, true, attacker.TargetPlIdx);
                }
                return;
            }

            //Handle Irish Whips
            if (attacker.animator.BasicSkillID == BasicSkillEnum.HammerThrough_D || attacker.animator.BasicSkillID == BasicSkillEnum.HammerThrough_S
                || attacker.animator.BasicSkillID == BasicSkillEnum.HammerThrough_U || attacker.animator.BasicSkillID == BasicSkillEnum.HammerThrough_Back_D
                || attacker.animator.BasicSkillID == BasicSkillEnum.HammerThrough_Back_S || attacker.animator.BasicSkillID == BasicSkillEnum.HammerThrough_Back_U
                || attacker.animator.BasicSkillID == BasicSkillEnum.HammerThrough_To_Runway)
            {
                attacker.animator.ReqBasicAnm(global::BasicSkillEnum.Grapple_Cut, true, defender.TargetPlIdx);
            }

            //Substitute animation on invalid move or if the defender isn't worn down enough
            if (!basicAttacks.Contains(sd.skillName[1]) && (defender.HP > (.25 * 65535f) && defender.SP > (.25 * 65535f) && defender.BP > (.25 * 65535f)))
            {
                attacker.ChangeState(global::PlStateEnum.NormalAnm);

                //Get animation
                if (rngValue == 1)
                {
                    attacker.animator.ReqBasicAnm(global::BasicSkillEnum.S1_Substitution_FrontHold, true, attacker.TargetPlIdx);
                }
                else
                {
                    defender.animator.ReqBasicAnm(global::BasicSkillEnum.PushAway_S, true, defender.TargetPlIdx);
                }
                attacker.SetGrappleResult(attacker.TargetPlIdx, true);
                attacker.SetGrappleResult(plIDx, false);
            }
        }

        [Hook(TargetClass = "Player", TargetMethod = "UpdatePlayer", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        public static void SumoLoseCondition(Player p)
        {
            if (!isSumo)
            {
                return;
            }

            //Ensure that we aren't checking during entrances
            if (MatchMain.inst.matchTime.min == 0 && MatchMain.inst.matchTime.sec == 0)
            {
                return;
            }

            if (MatchMain.inst.isMatchEnd)
            {
                return;
            }
            if (endMatch)
            {
                return;
            }

            Referee mRef = RefereeMan.inst.GetRefereeObj();
            Player attacker = PlayerMan.inst.GetPlObj(p.TargetPlIdx);

            //Determine if an illegal front headlock has been used
            if (attacker.animator.BasicSkillID == BasicSkillEnum.FrontHeadLock_ElbowStamp || attacker.animator.BasicSkillID == BasicSkillEnum.FrontHeadLock_BodyKneeLift)
            {
                mRef.PlDir = PlDirEnum.Left;
                mRef.State = RefeStateEnum.DeclareVictory;
                mRef.ReqRefereeAnm(BasicSkillEnum.Refe_Stand_MatchEnd_Front_Left);
                mRef.matchResult = MatchResultEnum.KO;
                attacker.isLoseAndStop = true;
                resultText = "Disqualification";
                mRef.SentenceLose(attacker.PlIdx);
                endMatch = true;
                return;
            }

            if (p.Zone == ZoneEnum.OutOfRing || p.Zone == ZoneEnum.SlopeRunway || p.Zone == ZoneEnum.Stage || p.Zone == ZoneEnum.StageEntrance || p.Zone == ZoneEnum.HorizontalRunway || p.Zone == ZoneEnum.OutsideCage)
            {
                L.D("Player Zone: " + p.Zone);
                mRef.PlDir = PlDirEnum.Left;
                mRef.ReqRefereeAnm(BasicSkillEnum.Refe_Stand_MatchEnd_Front_Left);
                mRef.State = RefeStateEnum.DeclareVictory;
                mRef.matchResult = MatchResultEnum.KO;
                p.isLoseAndStop = true;
                mRef.SentenceLose(p.PlIdx);
                resultText = "Ring Out Victory";
                endMatch = true;
                return;
            }
            if (p.State == PlStateEnum.Down_FaceDown || p.State == PlStateEnum.Down_FaceUp)
            {
                mRef.PlDir = PlDirEnum.Left;
                mRef.State = RefeStateEnum.DeclareVictory;
                mRef.ReqRefereeAnm(BasicSkillEnum.Refe_Stand_MatchEnd_Front_Left);
                mRef.matchResult = MatchResultEnum.KO;
                p.isLoseAndStop = true;
                resultText = "Knock Down Victory";
                mRef.SentenceLose(p.PlIdx);
                endMatch = true;
                return;
            }
        }

        //[Hook(TargetClass = "MatchMain", TargetMethod = "InitRound", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        //public static void PerformCeremony(MatchMain m)
        //{
        //    if (!isSumo)
        //    {
        //        return;
        //    }
        //    Player plObj = PlayerMan.inst.GetPlObj(0);
        //    plObj.TargetPlIdx = 4;
        //    plObj.animator.StartSlotAnm_Immediately(SkillSlotEnum.Performance_1, 0, true, 4);

        //    plObj = PlayerMan.inst.GetPlObj(4);
        //    plObj.TargetPlIdx = 0;
        //    plObj.animator.StartSlotAnm_Immediately(SkillSlotEnum.Performance_1, 0, true, 0);

        //}

        [Hook(TargetClass = "Menu_Result", TargetMethod = "Set_FinishSkill", InjectionLocation = 0, InjectDirection = HookInjectDirection.After, InjectFlags = HookInjectFlags.PassParametersRef, Group = "MoreMatchTypes")]
        public static void SetResultScreenDisplay(ref string str)
        {
            if (isSumo)
            {
                try
                {
                    if (!resultText.Equals(String.Empty))
                    {
                        //Get match time
                        string time = Regex.Split(str, Environment.NewLine)[0];
                        str = time + Environment.NewLine + resultText;
                    }
                }
                catch (IndexOutOfRangeException e)
                {}
                catch (Exception ex)
                {
                    L.D("SumoSetResultException: " + ex);
                }
                finally
                {
                    endMatch = false;
                }
            }
        }

        #region Helper Methods

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

        private static bool isOneOnOne()
        {
            return MatchConfiguration.GetPlayerCount() == 2;
        }
        #endregion
    }
}
