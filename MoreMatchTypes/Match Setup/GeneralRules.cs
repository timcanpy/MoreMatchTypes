using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG;
using MoreMatchTypes.Match_Setup;
using UnityEngine;


namespace MoreMatchTypes.Match_Rules
{
    class GeneralRules
    {
        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void RingSetup()
        {
            if (MoreMatchTypes_Form.moreMatchTypesForm.cb_sumo.Checked)
            {
                MoreMatchTypes_Form.moreMatchTypesForm.removeRopes.Checked = true;
                MoreMatchTypes_Form.moreMatchTypesForm.removePosts.Checked = true;
            }

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

        #region Add UI Buttons - More Match Types Starts at 1000
        [Hook(TargetClass = "Menu_SceneManager", TargetMethod = ".ctor", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        public static void AddLuchaButton(Menu_SceneManager msm)
        {
            ModPack.ModButtonManager.AddButton("Lucha Tag Rules", "ルチャタグルール", "Players can enter the ring without tags (under certain conditions). Hold shift when selecting to set the 2/3 rule.", "プレーヤーはタグなしでリングに入ることができます（特定の条件下). 2/3ルールの設定を選択するときにShiftキーを押したままにする.", 1000, Menu_SceneManager.MainMenuBtnType.BTN_TYPE_CHANGE_SCENE, Menu_SceneManager.SELECT_SCENE.BATTLE_ONENIGHT_NORMAL, 0, ModPack.ModButtonManager.ButtonList.MatchMenu, typeof(MatchTypeHook), "SetLuchaRules", null, false, msm, "ResetRules", 0);
        }

        [Hook(TargetClass = "Menu_SceneManager", TargetMethod = ".ctor", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        public static void AddEliminationButton(Menu_SceneManager msm)
        {
            ModPack.ModButtonManager.AddButton("Elimination Rules", "撤廃規則", "Two teams participate in a gauntlet of 1v1 battles. Hold shift when selecting to force eliminated members to leave ringside.", "2つのチームが1対1のバトルに参加します。 排除されたメンバーを強制的にリングサイドから退出させる場合は、Shiftキーを押したままにします。", 1001, Menu_SceneManager.MainMenuBtnType.BTN_TYPE_CHANGE_SCENE, Menu_SceneManager.SELECT_SCENE.BATTLE_ONENIGHT_NORMAL, 1, ModPack.ModButtonManager.ButtonList.MatchMenu, typeof(MatchTypeHook), "SetEliminationRules", null, false, msm, "ResetRules", 0);
        }

        [Hook(TargetClass = "Menu_SceneManager", TargetMethod = ".ctor", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        public static void AddTTTButton(Menu_SceneManager msm)
        {
            ModPack.ModButtonManager.AddButton("Timed Tornado Tag Rules", "竜巻タグ（時限", "Tornado tag battle, where players join over the course of a match.", "トルネードタグバトル。プレイヤーは試合中に参加します", 1002, Menu_SceneManager.MainMenuBtnType.BTN_TYPE_CHANGE_SCENE, Menu_SceneManager.SELECT_SCENE.BATTLE_ONENIGHT_NORMAL, 2, ModPack.ModButtonManager.ButtonList.MatchMenu, typeof(MatchTypeHook), "SetTTTRules", null, false, msm, "ResetRules", 0);
        }
        #endregion
    }
}
