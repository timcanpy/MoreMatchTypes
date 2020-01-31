﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG;
using UnityEngine;


namespace MoreMatchTypes.Match_Rules
{
    class GeneralRules
    {
        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void RingSetup()
        {
            List<string> unwantedComponents = new List<string>();
            MatchSetting settings = GlobalWork.inst.MatchSetting;
            if (MoreMatchTypes_Form.moreMatchTypesForm.removePosts.Checked)
            {
                MoreMatchTypes_Form.moreMatchTypesForm.removeRopes.Checked = true;
            }

            if (MoreMatchTypes_Form.moreMatchTypesForm.removeRopes.Checked)
            {
                unwantedComponents.Add("Shadow_Bottom");
                unwantedComponents.Add("Shadow_Middle");
                unwantedComponents.Add("Shadow_Top");
                unwantedComponents.Add("Rope_Bottom");
                unwantedComponents.Add("Rope_Middle");
                unwantedComponents.Add("Rope_Top");
                unwantedComponents.Add("Prefab_Rope(Clone)");
            }

            if (MoreMatchTypes_Form.moreMatchTypesForm.removePosts.Checked)
            {
                unwantedComponents.Add("CornerShadow");
                unwantedComponents.Add("TurnBuckle_West_A");
                unwantedComponents.Add("TurnBuckle_West_B");
                unwantedComponents.Add("TurnBuckle_West_C");
                unwantedComponents.Add("TurnBuckle_East_A");
                unwantedComponents.Add("TurnBuckle_East_B");
                unwantedComponents.Add("TurnBuckle_East_C");
                unwantedComponents.Add("TurnBuckle_North_A");
                unwantedComponents.Add("TurnBuckle_North_B");
                unwantedComponents.Add("TurnBuckle_North_C");
                unwantedComponents.Add("TurnBuckle_South_A");
                unwantedComponents.Add("TurnBuckle_South_B");
                unwantedComponents.Add("TurnBuckle_South_C");
                unwantedComponents.Add("TurnBuckle_ALL");
                unwantedComponents.Add("Post_West");
                unwantedComponents.Add("Post_North");
                unwantedComponents.Add("Post_South");
                unwantedComponents.Add("Post_East");

                //Remove deathmatch items
                unwantedComponents.Add("WoodPlate_South");
                unwantedComponents.Add("WoodPlate_North");
                unwantedComponents.Add("WoodPlate_East");
                unwantedComponents.Add("WoodPlate_West");
            }

            foreach (UnityEngine.Component c in GameObject.FindObjectsOfType<UnityEngine.Component>())
            {
                //L.D("Component: " + c.ToString());
                if (unwantedComponents.Contains(c.name))
                {
                    c.gameObject.SetActive(false);
                }
            }
        }

       
        [Hook(TargetClass = "Player", TargetMethod = "ProcessRunningCollision_InRing_Diagonal", InjectionLocation = 0,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.ModifyReturn | HookInjectFlags.PassInvokingInstance,
            Group = "MoreMatchTypes")]
        public static bool RemovePostClash(Player p)
        {
            if (global::Ring.inst.venueSetting.ringKind == RingKind.Octagon ||
                !MoreMatchTypes_Form.moreMatchTypesForm.removePosts.Checked)
            {
                return false;
            }
            else
            {
                p.ChangeState(global::PlStateEnum.AttackDamage);
                p.animator.ReqBasicAnm(global::BasicSkillEnum.Oops_D + global::MatchData.AnmOfsTbl_2Dir[(int)p.PlDir], false, -1);
                return true;
            }
        }

        [Hook(TargetClass = "Player", TargetMethod = "ProcessRunningCollision_InRing_ParallelRope", InjectionLocation = 0,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.ModifyReturn | HookInjectFlags.PassInvokingInstance,
            Group = "MoreMatchTypes")]
        public static bool RemoveRopeClash(Player p)
        {
            if (global::Ring.inst.venueSetting.ringKind == RingKind.Octagon ||
                !MoreMatchTypes_Form.moreMatchTypesForm.removeRopes.Checked)
            {
                return false;
            }
            else
            {
                p.ChangeState(global::PlStateEnum.AttackDamage);
                p.animator.ReqBasicAnm(global::BasicSkillEnum.Oops_D + global::MatchData.AnmOfsTbl_2Dir[(int)p.PlDir], false, -1);
                return true;
            }
        }

        [Hook(TargetClass = "MatchMisc", TargetMethod = "ApplyDamage", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassParametersVal,
            Group = "MoreMatchTypes")]
        public static void CheckAutoKO(int atk_pl_idx, int def_pl_idx)
        {
            if (MoreMatchTypes_Form.moreMatchTypesForm.isAutoKo.Checked)
            {
                Player defender = PlayerMan.inst.GetPlObj(def_pl_idx);
                if (defender.HP <= 0)
                {
                    defender.isKO = true;
                }
            }
        }
    }
}