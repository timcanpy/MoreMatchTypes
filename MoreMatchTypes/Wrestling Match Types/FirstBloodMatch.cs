﻿using DG;
using MatchConfig;
using System;
using System.Windows.Forms;
using UnityEngine;

namespace MoreMatchTypes
{
    [FieldAccess(Class = "Player", Field = "Bleeding", Group = "MoreMatchTypes"), FieldAccess(Class = "Menu_Result", Field = "Set_FinishSkill", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "MatchMain", Field = "InitMatch", Group = "MoreMatchTypes")]
    public class FirstBloodMatch
    {
        #region Variables
        public static int[] bloodMeter = new int[8];
        public static bool endMatch = false;
        public static bool isFirstBlood = false;
        #endregion

        #region Injection Methods
        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void SetMatchRules()
        {
            

            if (MoreMatchTypes_Form.moreMatchTypesForm.cb_FirstBlood.Checked && GlobalWork.inst.MatchSetting.BattleRoyalKind == BattleRoyalKindEnum.Off)
            {
                isFirstBlood = true;
            }
            else
            {
                isFirstBlood = false;
            }

            if (!isFirstBlood)
            {
                return;
            }

            //Setting rules to Exhibition, Single Fall Matches only
            MatchSetting setting = GlobalWork.inst.MatchSetting;
            setting.VictoryCondition = VictoryConditionEnum.OnlyEscape;
            setting.is10CountKO = false;
            setting.isOutOfRingCount = false;
            setting.isFoulCount = false;
            setting.isElimination = false;
            setting.isTornadoBattle = true;
            setting.MatchTime = 0;
            setting.CriticalRate = CriticalRateEnum.Off;
            MoreMatchTypes_Form.moreMatchTypesForm.Enabled = false;

            bloodMeter = new int[8];
            endMatch = false;


        }

        [Hook(TargetClass = "Player", TargetMethod = "Bleeding", InjectionLocation = 0, InjectFlags = HookInjectFlags.PassInvokingInstance | HookInjectFlags.ModifyReturn, Group = "MoreMatchTypes")]
        public static bool SetMatchRestrictions(Player matchPlayer)
        {
            //Disable for Battle Royal Matches
            if (!isFirstBlood)
            {
                return false;
            }
            else
            {
                //Get the player reference targetting the CURRENT player (matchPlayer) being modified.
                Player playerObj = PlayerMan.inst.GetPlObj(matchPlayer.TargetPlIdx);
                if (playerObj == null)
                { return false; }

                //Get the attacker's current skill and add it to the sum of bleeding damage done to the receiving (matchPlayer).
                SkillData currentSkill = playerObj.animator.CurrentSkill;
                if (currentSkill != null)
                {
                    bloodMeter[matchPlayer.PlIdx] += currentSkill.bleedingRate;

                    string defender = DataBase.GetWrestlerFullName(matchPlayer.WresParam);
                    string attacker = DataBase.GetWrestlerFullName(playerObj.WresParam);

                    if (bloodMeter[matchPlayer.PlIdx] <= 100)
                    {
                        MatchConfiguration.ShowCommentaryMessage(attacker + " is trying to bust open " + defender + ".");
                    }
                    else if (bloodMeter[matchPlayer.PlIdx] <= 200)
                    {
                        MatchConfiguration.ShowCommentaryMessage(attacker + " is really working over " + defender + "!");
                    }
                    else
                    {
                        MatchConfiguration.ShowCommentaryMessage(attacker + " is trying to put " + defender + " in the hospital! Such savagery!");
                    }

                }

                //Disable bleeding if match time has not passed the given value
                if (MatchMain.inst.matchTime.min < UnityEngine.Random.Range(8, 12))
                { return true; }

                if (bloodMeter[matchPlayer.PlIdx] >= 300)
                { return false; }
                else
                { return true; }
            }
        }

        [Hook(TargetClass = "Menu_Result", TargetMethod = "Set_FinishSkill", InjectionLocation = 0, InjectDirection = HookInjectDirection.After, InjectFlags = HookInjectFlags.PassParametersRef, Group = "MoreMatchTypes")]
        public static void SetResultScreenDisplay(ref string str)
        {
            if (isFirstBlood)
            {
                string resultString = str.Replace("K.O.", "First Blood");
                str = resultString;
                endMatch = false;
            }
        }

        [Hook(TargetClass = "Player", TargetMethod = "Bleeding", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        public static void SetVictoryConditions(Player matchPlayer)
        {
            //Ensure method is only run when the option is selected and the match type is not a Battle Royal
            if (!isFirstBlood)
            { return; }

            if (matchPlayer.isBleeding && MoreMatchTypes_Form.moreMatchTypesForm.cb_FirstBlood.Checked)
            {
                endMatch = true;
                GlobalWork.inst.MatchSetting.VictoryCondition = VictoryConditionEnum.Count3;
                Referee matchRef = RefereeMan.inst.GetRefereeObj();
                matchRef.PlDir = PlDirEnum.Left;
                matchRef.ReqRefereeAnm(BasicSkillEnum.Refe_Stand_MatchEnd_Front_Left);
                matchRef.State = RefeStateEnum.DeclareVictory;
                matchRef.matchResult = MatchResultEnum.KO;
                matchRef.SentenceLose(matchPlayer.PlIdx);
            }

        }
        #endregion

        #region helper methods
        [Hook(TargetClass = "MatchMain", TargetMethod = "EndMatch", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void ResetBloodMeter()
        {
            Array.Clear(bloodMeter, 0, bloodMeter.Length);
            MoreMatchTypes_Form.moreMatchTypesForm.Enabled = true;
        }


        #endregion
    }
}
