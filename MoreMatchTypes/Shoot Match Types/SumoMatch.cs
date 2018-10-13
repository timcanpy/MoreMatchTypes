//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using DG;
//using UnityEngine;


//namespace MoreMatchTypes
//{
//    [FieldAccess(Class = "Player", Field = "UpdatePlayer", Group = "MoreMatchTypes"), FieldAccess(Class = "Player", Field = "SetGrappleResult", Group = "MoreMatchTypes")]
//    [FieldAccess(Class = "Menu_Result", Field = "Set_FinishSkill", Group = "MoreMatchTypes"), FieldAccess(Class = "MatchEvaluation", Field = "EvaluateSkill", Group = "MoreMatchTypes")]
//    [FieldAccess(Class = "MatchMain", Field = "InitRound", Group = "MoreMatchTypes"), FieldAccess(Class = "Player", Field = "UpdatePlayer", Group = "MoreMatchTypes")]
//    [FieldAccess(Class = "Referee", Field = "SentenceLose", Group = "MoreMatchTypes")]
//    class SumoMatch
//    {
//        #region Variables
//        public static List<string> basicAttacks;

//        public static bool isSumo = false;
//        public static string resultText;
//        public static bool endMatch = false;

//        #endregion

//        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
//        public static void SetMatchRules()
//        {
//            MatchSetting settings = GlobalWork.inst.MatchSetting;

//            if (settings.arena == VenueEnum.BarbedWire || settings.arena == VenueEnum.Cage || settings.arena == VenueEnum.LandMine_BarbedWire || settings.arena == VenueEnum.LandMine_FluorescentLamp)
//            {
//                return;
//            }
//            if (settings.BattleRoyalKind != BattleRoyalKindEnum.Off)
//            {
//                return;
//            }

//            isSumo = false;
//            endMatch = false;
//            if (MoreMatchTypes_Form.form.cb_sumo.Checked)
//            {
//                isSumo = true;
//                settings.isReversalOff = true;
//                settings.isOverTheTopRopeOn = true;
//                settings.isPowerCompetitionOff = true;
//                settings.MatchTime = 0;

//                //Populate Basic Attacks
//                String basicMoves = MoreMatchTypes_Form.form.tb_basic.Text.TrimStart().TrimEnd();
//                if (basicMoves != "")
//                {
//                    basicAttacks = CreateMoveList(basicMoves);
//                }
//                else
//                {
//                    basicAttacks = new List<string>()
//                    {
//                        "Shoutei",
//                        "Face Slap B",
//                        "Chest Slap",
//                        "Knife-Edge Chop",
//                        "Koppo Style Shoutei",
//                        "Throat Chop",
//                        "Jigoku-Tsuki"
//                    };
//                }
//            }
//            else
//            {
//                return;
//            }

//            resultText = "";
//        }

//        [Hook(TargetClass = "MatchEvaluation", TargetMethod = "EvaluateSkill", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassParametersVal, Group = "MoreMatchTypes")]
//        public static void SetVictoryCondition(int plIDx, SkillData sd, SkillSlotAttr skillAttr)
//        {
//            if (!isSumo)
//            {
//                return;
//            }

//            Player attacker = PlayerMan.inst.GetPlObj(plIDx);
//            Player defender = PlayerMan.inst.GetPlObj(attacker.TargetPlIdx);
//            Referee mRef = RefereeMan.inst.GetRefereeObj();

//            if (basicAttacks.Contains(sd.skillName[1]) || (sd.anmType == SkillAnmTypeEnum.Single))
//            {
//                return;
//            }

//            float rngValue = UnityEngine.Random.Range(1, 2);

//            //Handle back grapples
//            if (attacker.animator.SkillSlotID == SkillSlotEnum.Back_X || attacker.animator.SkillSlotID == SkillSlotEnum.Back_A || attacker.animator.SkillSlotID == SkillSlotEnum.Back_B || attacker.animator.SkillSlotID == SkillSlotEnum.Back_B_UD || attacker.animator.SkillSlotID == SkillSlotEnum.Back_B_LR || attacker.animator.SkillSlotID == SkillSlotEnum.Back_XA)
//            {
//                if (rngValue == 1)
//                {
//                    attacker.animator.ReqBasicAnm(global::BasicSkillEnum.S1_Substitution_BackHold, true, defender.TargetPlIdx);
//                }
//                else
//                {
//                    defender.animator.ReqBasicAnm(global::BasicSkillEnum.S1_Substitution_BackRev, true, attacker.TargetPlIdx);
//                }
//                return;
//            }


//            //Substitute animation on invalid move or if the defender isn't worn down enough
//            if (!basicAttacks.Contains(sd.skillName[1]) && (defender.HP > (.25 * 65535f) && defender.SP > (.25 * 65535f) && defender.BP > (.25 * 65535f)))
//            {
//                attacker.ChangeState(global::PlStateEnum.NormalAnm);

//                //Get animation
//                if(rngValue == 1)
//                {
//                    attacker.animator.ReqBasicAnm(global::BasicSkillEnum.S1_Substitution_FrontHold, true, defender.TargetPlIdx);
//                }
//                else
//                {
//                    defender.animator.ReqBasicAnm(global::BasicSkillEnum.PushAway_S, true, attacker.TargetPlIdx);
//                }
//                attacker.SetGrappleResult(attacker.TargetPlIdx, true);
//                attacker.SetGrappleResult(plIDx, false);
//            }
//            else
//            {
//                mRef.PlDir = PlDirEnum.Left;
//                mRef.State = RefeStateEnum.DeclareVictory;
//                mRef.ReqRefereeAnm(BasicSkillEnum.Refe_Stand_MatchEnd_Front_Left);
//                mRef.matchResult = MatchResultEnum.KO;
//                defender.isLoseAndStop = true;
//                mRef.SentenceLose(defender.PlIdx);
//                endMatch = true;
//            }


//        }

//        [Hook(TargetClass = "Player", TargetMethod = "UpdatePlayer", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
//        public static void SumoLoseCondition(Player p)
//        {
//            if (!isSumo)
//            {
//                return;
//            }
//            if (MatchMain.inst.isMatchEnd)
//            {
//                return;
//            }
//            if (endMatch)
//            {
//                return;
//            }

//            Referee mRef = RefereeMan.inst.GetRefereeObj();
//            Player attacker = PlayerMan.inst.GetPlObj(p.TargetPlIdx);

//            //Determine if an illegal front headlock has been used
//            if (attacker.animator.BasicSkillID == BasicSkillEnum.FrontHeadLock_ElbowStamp || attacker.animator.BasicSkillID == BasicSkillEnum.FrontHeadLock_BodyKneeLift)
//            {
//                mRef.PlDir = PlDirEnum.Left;
//                mRef.State = RefeStateEnum.DeclareVictory;
//                mRef.ReqRefereeAnm(BasicSkillEnum.Refe_Stand_MatchEnd_Front_Left);
//                mRef.matchResult = MatchResultEnum.KO;
//                attacker.isLoseAndStop = true;
//                resultText = "D.Q.";
//                mRef.SentenceLose(attacker.PlIdx);
//                endMatch = true;
//                return;
//            }

//            if (p.Zone == ZoneEnum.OutOfRing || p.Zone == ZoneEnum.SlopeRunway || p.Zone == ZoneEnum.Stage || p.Zone == ZoneEnum.StageEntrance || p.Zone == ZoneEnum.HorizontalRunway || p.Zone == ZoneEnum.OutsideCage)
//            {
//                mRef.PlDir = PlDirEnum.Left;
//                mRef.ReqRefereeAnm(BasicSkillEnum.Refe_Stand_MatchEnd_Front_Left);
//                mRef.State = RefeStateEnum.DeclareVictory;
//                mRef.matchResult = MatchResultEnum.KO;
//                p.isLoseAndStop = true;
//                mRef.SentenceLose(p.PlIdx);
//                resultText = "Ring Out Victory";
//                endMatch = true;
//                return;
//            }
//            if (p.State == PlStateEnum.Down_FaceDown || p.State == PlStateEnum.Down_FaceUp)
//            {
//                mRef.PlDir = PlDirEnum.Left;
//                mRef.State = RefeStateEnum.DeclareVictory;
//                mRef.ReqRefereeAnm(BasicSkillEnum.Refe_Stand_MatchEnd_Front_Left);
//                mRef.matchResult = MatchResultEnum.KO;
//                p.isLoseAndStop = true;
//                mRef.SentenceLose(p.PlIdx);
//                endMatch = true;
//                return;
//            }
//        }

//        [Hook(TargetClass = "MatchMain", TargetMethod = "InitRound", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
//        public static void PerformCeremony(MatchMain m)
//        {
//            if (!isSumo)
//            {
//                return;
//            }
//            for (int i = 0; i < 8; i++)
//            {
//                Player plObj = PlayerMan.inst.GetPlObj(i);
//                if (!plObj)
//                {
//                    continue;
//                }

//                plObj.animator.StartSlotAnm_Immediately(SkillSlotEnum.Performance_1, 0, true, i);
//            }
//        }

//        [Hook(TargetClass = "Menu_Result", TargetMethod = "Set_FinishSkill", InjectionLocation = 8, InjectDirection = HookInjectDirection.After, InjectFlags = HookInjectFlags.PassParametersVal | HookInjectFlags.PassLocals, LocalVarIds = new int[] { 1 }, Group = "MoreMatchTypes")]
//        public static void SetResultScreenDisplay(ref UILabel finishText, string str)
//        {
//            if (isSumo)
//            {
//                if (resultText.Equals(""))
//                {
//                    resultText = "Knock Down Victory";
//                }
//                string resultString = str.Replace("K.O.", resultText).Replace("DRAW", resultText);
//                finishText.text = resultString;
//                endMatch = false;
//            }
//        }

//        [Hook(TargetClass = "MatchMain", TargetMethod = "Awake", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
//        public static void SumoRingSetup()
//        {
//            if (!isSumo)
//            {
//                return;
//            }

//            List<string> unwantedComponents = new List<string>();
//            unwantedComponents.Add("Shadow_Bottom");
//            unwantedComponents.Add("Shadow_Middle");
//            unwantedComponents.Add("Shadow_Top");
//            unwantedComponents.Add("Rope_Bottom");
//            unwantedComponents.Add("Rope_Middle");
//            unwantedComponents.Add("Rope_Top");
//            unwantedComponents.Add("Prefab_Rope(Clone)");
//            unwantedComponents.Add("Corner_Shadow");
//            unwantedComponents.Add("TurnBuckle_West_A");
//            unwantedComponents.Add("TurnBuckle_West_B");
//            unwantedComponents.Add("TurnBuckle_West_C");
//            unwantedComponents.Add("TurnBuckle_East_A");
//            unwantedComponents.Add("TurnBuckle_East_B");
//            unwantedComponents.Add("TurnBuckle_East_C");
//            unwantedComponents.Add("TurnBuckle_North_A");
//            unwantedComponents.Add("TurnBuckle_North_B");
//            unwantedComponents.Add("TurnBuckle_North_C");
//            unwantedComponents.Add("TurnBuckle_South_A");
//            unwantedComponents.Add("TurnBuckle_South_B");
//            unwantedComponents.Add("TurnBuckle_South_C");
//            unwantedComponents.Add("TurnBuckle_ALL");
//            unwantedComponents.Add("Post_West");
//            unwantedComponents.Add("Post_North");
//            unwantedComponents.Add("Post_South");
//            unwantedComponents.Add("Post_East");

//            foreach (UnityEngine.Component c in GameObject.FindObjectsOfType<UnityEngine.Component>())
//            {
//                if (unwantedComponents.Contains(c.name))
//                {
//                    c.gameObject.SetActive(false);
//                }
//            }
//        }

//        #region Helper Methods

//        private static List<String> CreateMoveList(String moveList)
//        {
//            char[] separators = new char[4] { ',', '|', ';', '\n' };

//            List<string> modifiedList;
//            modifiedList = moveList.Split(separators).ToList();

//            foreach(String move in modifiedList)
//            {
//                move.TrimStart().TrimEnd();
//            }
//            return modifiedList;
//        }
//        #endregion
//    }
//}
