using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG;
using MatchConfig;
using UnityEngine;

//when you modify a return on a method that returns something other than void you always need to out XXX that thing

namespace MoreMatchTypes.Shoot_Match_Types
{
    [FieldAccess(Class = "MatchMain", Field = "ProceedNextRound", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "Player", Field = "IsS1Waza", Group = "MoreMatchTypes")]
    class Boxing
    {
        #region Variables
        public static bool isBoxing = false;
        public static int foulCeiling = 0;
        public static int bleedCeiling = 0;
        public static int[] foulCount = new int[2];
        public static bool isFoul = false;
        public static string refName = "";
        public static List<String> dqAttacks = new List<String>();
        public static bool isDown = false;
        #endregion

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void SetMatchRules()
        {
            MatchSetting settings = GlobalWork.inst.MatchSetting;
            isBoxing = false;

            if (MoreMatchTypes_Form.moreMatchTypesForm.cb_boxing.Checked || MoreMatchTypes_Form.moreMatchTypesForm.cb_kickboxing.Checked && IsOneOnOne())
            {
                isBoxing = true;
                isDown = false;
                foulCount = new int[2];
                settings.isS1Rule = true;
                settings.isExchangeOfStriking = false;
                settings.isPowerCompetitionOff = true;

                String dqMoves = MoreMatchTypes_Form.moreMatchTypesForm.tb_dq.Text.TrimStart().TrimEnd();
                if (dqMoves.Trim() != "")
                {
                    dqAttacks = CreateMoveList(dqMoves);
                }
                else
                {
                    dqAttacks = new List<string>
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
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitRound", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void StartRound()
        {
            if (!isBoxing)
            {
                return;
            }
            Referee mref = RefereeMan.inst.GetRefereeObj();
            foulCeiling = 5 - mref.RefePrm.foulCount;
            bleedCeiling = 5 - mref.RefePrm.interfereTime;
            refName = mref.RefePrm.name;
            isFoul = false;
            isDown = false;
            L.D("Foul Ceiling: " + foulCeiling);
            L.D("Bleed Ceiling: " + bleedCeiling);
        }

        [Hook(TargetClass = "Player", TargetMethod = "IsS1Waza", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance | HookInjectFlags.PassParametersVal | HookInjectFlags.ModifyReturn, Group = "MoreMatchTypes")]
        public static bool EnableFoulCheck(Player p, out bool result, SkillSlotEnum slot)
        {
            if (!isBoxing)
            {
                result = false;

            }
            p.animator.ReqSlotAnm(slot, true, p.TargetPlIdx, true);
            result = true;
            return result;
        }

        [Hook(TargetClass = "MatchEvaluation", TargetMethod = "EvaluateSkill", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassParametersVal, Group = "MoreMatchTypes")]
        public static void FoulOpponent(int plIDx, SkillData sd, SkillSlotAttr skillAttr)
        {
            if (!isBoxing)
            {
                return;
            }

            Player attacker = PlayerMan.inst.GetPlObj(plIDx);
            Player defender = PlayerMan.inst.GetPlObj(attacker.TargetPlIdx);
            if (attacker.lastSkillHit = false)
            {
                return;
            }
            //Determine if attacker fouls opponent
            if (sd.filteringType != SkillFilteringType.Punch && sd.filteringType != SkillFilteringType.Chop && sd.filteringType != SkillFilteringType.Performance && sd.filteringType != SkillFilteringType.Thrust)
            {
                //Allow kicks if this is a kickboxing match
                if (MoreMatchTypes_Form.moreMatchTypesForm.cb_kickboxing.Checked)
                {
                    if (sd.filteringType == SkillFilteringType.Kick)
                    {
                        return;
                    }
                }

                //Increase bleeding rate on headbutts and elbows
                if (sd.filteringType == SkillFilteringType.Headbutt || sd.filteringType == SkillFilteringType.Elbow)
                {
                    if (sd.bleedingRate < 10)
                    {
                        sd.bleedingRate = 10;
                    }
                }
                int discretion = attacker.WresParam.aiParam.discreation;

                if (sd.anmType != SkillAnmTypeEnum.HitBranch_Single || sd.anmType != SkillAnmTypeEnum.HitBranch_Pair)
                {
                    discretion = 0;
                }
                if (UnityEngine.Random.Range(1, 100) >= discretion)
                {
                    //Execute the foul move
                    isFoul = true;
                    int foulIncrement = 1;
                    int currFouls = 0;

                    if (dqAttacks.Contains(sd.skillName[(int)SaveData.inst.optionSettings.language].ToLower()) || sd.flags == SkillData.SkillFlags.FoulTech)
                    {
                        foulIncrement = foulCeiling + 1;
                    }
               
                    //Determine if defender resists illegal blows
                    if (sd.flags == SkillData.SkillFlags.FoulTech)
                    {
                        ResistBlow(defender);
                    }

                    //Determine if referee processes the foul
                    if (plIDx < 4)
                    {
                        currFouls = foulCount[0];
                    }
                    else
                    {
                        currFouls = foulCount[1];
                    }

                    float foulValue = foulCeiling * (currFouls + 1);

                    if (sd.filteringType != SkillFilteringType.Elbow || sd.filteringType != SkillFilteringType.Headbutt)
                    {
                        foulValue = foulCeiling * (currFouls + 2);
                    }
                    L.D("Foul Value: " + foulValue);
                    if (UnityEngine.Random.Range(1, 50) > foulValue)
                    {
                        if (plIDx < 4)
                        {
                            foulCount[0] += foulIncrement;
                            if (foulCount[0] > foulCeiling)
                            {
                                CallDQ(plIDx);
                            }
                            else
                            {
                                CallFoul(plIDx, sd.skillName[(int)SaveData.inst.optionSettings.language]);

                                //Ensure that foul move does not continue
                                defender.animator.isReqAnmLoopEnd = true;
                                defender.SetDownTime(0);
                            }
                        }
                        else
                        {
                            foulCount[1] += foulIncrement;
                            if (foulCount[1] > foulCeiling)
                            {
                                CallDQ(plIDx);
                            }
                            else
                            {
                                CallFoul(plIDx, sd.skillName[(int)SaveData.inst.optionSettings.language]);

                                //Ensure that foul move does not continue
                                defender.animator.isReqAnmLoopEnd = true;
                                defender.SetDownTime(0);
                            }
                        }
                    }
                    Audience.inst.PlayCheerVoice(CheerVoiceEnum.BOOING, 1);
                    Audience.inst.TensionDown();

                }
                else
                {
                    //Clinch instead of committing a foul
                    attacker.animator.ReqBasicAnm(global::BasicSkillEnum.S1_Substitution_FrontHold, true, attacker.TargetPlIdx);
                    attacker.SetGrappleResult(attacker.TargetPlIdx, true);
                    attacker.SetGrappleResult(plIDx, false);
                }
            }

        }

        [Hook(TargetClass = "Player", TargetMethod = "Bleeding", InjectionLocation = 0, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        public static void CheckBleeding(Player p)
        {
            if (!isBoxing)
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

        [Hook(TargetClass = "MatchMain", TargetMethod = "ProceedNextRound", InjectionLocation = 0, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void HandleRoundEnd()
        {
            if (isBoxing)
            {
                ForceCleanBreak();
            }
        }

        [Hook(TargetClass = "Player", TargetMethod = "UpdatePlayer", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        public static void ProcessTaunt(Player plObj)
        {
            if (!isBoxing)
            {
                return;
            }

            Player defender = PlayerMan.inst.GetPlObj(plObj.TargetPlIdx);

            //Handle edge cases where both fighers are down
            if (defender.State != PlStateEnum.S_KOCNT && plObj.State != PlStateEnum.S_KOCNT)
            {
                isDown = false;
                return;
            }

            if (defender.State == PlStateEnum.S_KOCNT && !isDown)
            {
                isDown = true;
                int showmanship = plObj.WresParam.aiParam.personalTraits;
                if (UnityEngine.Random.Range(1, 100) < showmanship)
                {
                    ExecuteTaunt(plObj);
                }
            }
        }

        #region Helper Methods
        private static bool IsOneOnOne()
        {
            return MatchConfiguration.GetPlayerCount() == 2;
        }

        private static void CallFoul(int plIdx, String move)
        {
            //Determine if this is a no contest or disqualification based on defender's status
            Player defender = PlayerMan.inst.GetPlObj(plIdx);
            if (defender.isKO)
            {
                //Determine the attacker
                int foulIndex = 0;
                if (plIdx > 3)
                {
                    foulIndex = 1;
                }
                if (UnityEngine.Random.Range(0, 3) > foulCeiling - foulCount[foulIndex])
                {
                    CallDQ(plIdx);
                }
                else
                {
                    CallNoContest();
                }
            }
            MatchConfiguration.ShowCommentaryMessage("Foul! " + DataBase.GetWrestlerFullName(PlayerMan.inst.GetPlObj(plIdx).WresParam) + " is warned for usage of an illegal " + move + ".");

            ForceCleanBreak();
        }

        private static void CallDQ(int plIdx)
        {
            Referee mRef = RefereeMan.inst.GetRefereeObj();
            mRef.PlDir = PlDirEnum.Left;
            mRef.State = RefeStateEnum.DeclareVictory;
            mRef.ReqRefereeAnm(BasicSkillEnum.Refe_Stand_MatchEnd_Front_Left);
            mRef.matchResult = MatchResultEnum.Foul;
            PlayerMan.inst.GetPlObj(plIdx).isLoseAndStop = true;
            mRef.SentenceLose(plIdx);
        }

        private static void CallNoContest()
        {
            Referee mRef = RefereeMan.inst.GetRefereeObj();
            mRef.PlDir = PlDirEnum.Left;
            mRef.State = RefeStateEnum.DeclareVictory;
            mRef.ReqRefereeAnm(BasicSkillEnum.Refe_Stand_MatchEnd_Front_Left);
            mRef.matchResult = MatchResultEnum.RingOutDraw;
            for (int i = 0; i < 8; i++)
            {
                Player pl = PlayerMan.inst.GetPlObj(i);
                if (!pl)
                {
                    continue;
                }
                pl.Start_ForceControl(global::ForceCtrlEnum.GoBackToApron);
            }
        }

        private static void ResistBlow(Player pl)
        {
            //Get current spirit
            float spirit = pl.SP;
            if (spirit > 65535f / 2)
            {
                spirit = 1;
            }
            else if (spirit > 65535f / 4)
            {
                spirit = 0;
            }
            else
            {
                spirit = -1;
            }

            if (UnityEngine.Random.Range(1, 5) > 5 + spirit)
            {
                pl.isKO = true;
            }
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

            MatchSetting settings = GlobalWork.inst.MatchSetting;
            ////Do not perform at the start of a round
            //if (main.matchTime.sec == 0 && main.matchTime.min % settings.MatchTime == 0)
            //{
            //    return;
            //}
            Referee mRef = RefereeMan.inst.GetRefereeObj();
            global::MatchSEPlayer.inst.PlayRefereeVoice(global::RefeVoiceEnum.Break);
            mRef.State = global::RefeStateEnum.CallBeforeMatch_1;
            mRef.ReqRefereeAnm(global::BasicSkillEnum.ROUNDF);
            mRef.UpdateRefereeAnm();
        }
        //private static void ForceCleanBreak()
        //{
        //    for (int i = 0; i < 8; i++)
        //    {
        //        Player pl = PlayerMan.inst.GetPlObj(i);
        //        if (!pl)
        //        {
        //            continue;
        //        }
        //        //Force Submission Breaks
        //        if (pl.isSubmissionAtk)
        //        {
        //            pl.plController.padPush = PadBtnEnum.Atk_M;
        //        }
        //        pl.Start_ForceControl(global::ForceCtrlEnum.WaitMatchStart);

        //    }

        //    //Do not perform at the start of a match.
        //    MatchMain main = MatchMain.inst;
        //    if (main.matchTime.min == 0 && main.matchTime.sec == 0)
        //    {
        //        return;
        //    }

        //    MatchSetting settings = GlobalWork.inst.MatchSetting;
        //    //Do not perform at the start of a round
        //    if (main.matchTime.sec == 0 && main.matchTime.min % settings.MatchTime == 0)
        //    {
        //        return;
        //    }
        //    Referee mRef = RefereeMan.inst.GetRefereeObj();
        //    global::MatchSEPlayer.inst.PlayRefereeVoice(global::RefeVoiceEnum.Break);
        //    mRef.State = global::RefeStateEnum.CallBeforeMatch_1;
        //    mRef.ReqRefereeAnm(global::BasicSkillEnum.ROUNDF);
        //    mRef.UpdateRefereeAnm();
        //}

        private static void ExecuteTaunt(Player plObj)
        {
            int randomNum = UnityEngine.Random.Range(1, 4);
            if (randomNum == 1)
            {
                plObj.animator.StartSlotAnm_Immediately(SkillSlotEnum.Performance_1, 0, true, plObj.PlIdx);
            }
            else if (randomNum == 2)
            {
                plObj.animator.StartSlotAnm_Immediately(SkillSlotEnum.Performance_2, 0, true, plObj.PlIdx);
            }
            else if (randomNum == 3)
            {
                plObj.animator.StartSlotAnm_Immediately(SkillSlotEnum.Performance_3, 0, true, plObj.PlIdx);
            }
            else
            {
                plObj.animator.StartSlotAnm_Immediately(SkillSlotEnum.Performance_4, 0, true, plObj.PlIdx);
            }
            plObj.AddSP(plObj.WresParam.aiParam.personalTraits);
        }

        private static List<String> CreateMoveList(String moveList)
        {
            char[] separators = new char[4] { ',', '|', ';', '\n' };

            List<string> modifiedList;
            modifiedList = moveList.Split(separators).ToList();

            foreach (String move in modifiedList)
            {
                move.TrimStart().TrimEnd().ToLower();
            }
            return modifiedList;
        }
        #endregion

    }
}
