using DG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatchConfig;
using UnityEngine;
using ModPack;

namespace MoreMatchTypes.Wrestling_Match_Types
{
    #region Access Modifiers
    [FieldAccess(Class = "Referee", Field = "Refereeing_OutOfRingCount", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "Referee", Field = "SetFree", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "Referee", Field = "Process_FallCount", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "Referee", Field = "Process_SubmissionCheck", Group = "MoreMatchTypes")]
    #endregion
    class LuchaTag
    {

        #region Variables
        public static bool isLuchaTag;
        public static int[] points;
        public static int playerCount;
        public static int countLimit;
        public static int modifier;
        public static bool is2Falls;
        public static String[] teamNames;
        #endregion

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void SetMatchRules()
        {
            isLuchaTag = false;
            points = new int[2];
            MatchSetting settings = GlobalWork.inst.MatchSetting;
            playerCount = GetPlayerCount();
            modifier = 0;

            if (playerCount < 4 || settings.BattleRoyalKind != BattleRoyalKindEnum.Off || !MoreMatchTypes_Form.moreMatchTypesForm.cb_luchaTag.Checked)
            {
                return;
            }
            else
            {
                countLimit = 0;
                is2Falls = MoreMatchTypes_Form.moreMatchTypesForm.cb_luchaFalls.Checked;
                isLuchaTag = true;
                settings.isOutOfRingCount = true;
                settings.isTornadoBattle = false;

                //Determine modifier for Lucha Tags
                if (playerCount > 4)
                {
                    modifier = 1;
                }

                //Both teams begin with one point; next fall wins.
                if (!is2Falls)
                {
                    points[0] = 1;
                    points[1] = 1;
                }

                teamNames = new String[2];
                SetTeamNames();
            }

        }

        [Hook(TargetClass = "Referee", TargetMethod = "Refereeing_OutOfRingCount", InjectionLocation = 0,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        public static void CheckFreeTag(Referee referee)
        {
            if (!isLuchaTag || referee.RefeCount == 20)
            {
                return;
            }

            try
            {
                //Randomized for variance.
                if (countLimit == -1)
                {
                    switch (referee.RefePrm.outCount)
                    {
                        case 0:
                        case 1:
                            countLimit = 4;
                            break;
                        case 2:
                            countLimit = 5;
                            break;
                        case 3:
                        case 4:
                            countLimit = 6;
                            break;
                    }

                    //Subtract modifier from the final value
                    countLimit = countLimit - modifier < 0 ? countLimit : countLimit - modifier;
                    countLimit = UnityEngine.Random.Range(0, countLimit);
                }

                if ((referee.RefeCount >= countLimit || NoLegalManInRing()) && PlayerMan.inst.GetPlayerNum_OutOfRingCount() != playerCount)
                {
                    countLimit = -1;
                    SetLegalMen("");
                }
            }
            catch (Exception e)
            {
                L.D("Lucha Error: " + e);
            }
        }

        [Hook(TargetClass = "Referee", TargetMethod = "Process_FallCount", InjectionLocation = 42,
            InjectFlags = HookInjectFlags.ModifyReturn | HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        public static bool ProcessPin(Referee r)
        {
            return CheckMatchEnd(r);
        }

        [Hook(TargetClass = "Referee", TargetMethod = "Process_SubmissionCheck", InjectionLocation = 25,
            InjectFlags = HookInjectFlags.ModifyReturn | HookInjectFlags.PassInvokingInstance,
            Group = "MoreMatchTypes")]
        public static bool ProcessSubmission(Referee r)
        {
            int disturbingPlayer = global::PlayerMan.inst.GetDisturbingPlayer();
            Player plObj = global::PlayerMan.inst.GetPlObj(r.TargetPlIdx);
            if (disturbingPlayer < 0 && plObj.isWannaGiveUp)
            {
                return CheckMatchEnd(r);
            }

            return false;
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "ProcessMatchEnd_Draw", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void SetVictoryConditions()
        {
            if (!isLuchaTag)
            {
                return;
            }

            MatchMain main = MatchMain.inst;
            Referee matchRef = RefereeMan.inst.GetRefereeObj();
            if (main.isTimeUp)
            {
                if (points[0] == points[1])
                {
                    return;
                }
                else
                {
                    PlayerMan p = PlayerMan.inst;
                    Announcer.inst.PlayGong_MatchEnd();

                    if (points[0] > points[1])
                    {
                        matchRef.SentenceLose(p.GetPlObj(4).PlIdx);
                        SetLosers(GetLegalMan("red"), p);
                    }
                    else
                    {
                        matchRef.SentenceLose(p.GetPlObj(0).PlIdx);
                        SetLosers(GetLegalMan("blue"), p);
                    }
                }
            }
        }

        [Hook(TargetClass = "Menu_Result", TargetMethod = "Set_FinishSkill", InjectionLocation = 0, InjectDirection = HookInjectDirection.After, InjectFlags = HookInjectFlags.PassParametersRef, Group = "MoreMatchTypes")]
        public static void SetResultScreenDisplay(ref string str)
        {
            if (!isLuchaTag || !MatchMain.inst.isMatchEnd || str.Contains("K.O.") || !is2Falls)
            {
                return;
            }

            string result = teamNames[0] + ": " + points[0] + " points\n" + teamNames[1] + ": " + points[1] + " points\n\n";
            if (points[0] == points[1])
            {
                result += "Draw";
            }
            else
            {
                result += "Winner - " + (points[0] > points[1] ? teamNames[0] : teamNames[1]);
            }

            string resultString = "Lucha Tag Match\n\n" + result;
            str = resultString;
        }

        #region UI Methods

        [Hook(TargetClass = "Menu_SceneManager", TargetMethod = ".ctor", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance,
            Group = "MoreMatchTypes")]
        public static void AddLuchaButton(Menu_SceneManager manager)
        {
            //ModButtonManager.AddButton("Lucha Tag", "ラッチャー・タグ", "Take part in a Lucha Libre Tag match.", "Lucha Libreの試合に参加する", 200, Menu_SceneManager.MainMenuBtnType.BTN_TYPE_CHANGE_SCENE, Menu_SceneManager.SELECT_SCENE.BATTLE_ONENIGHT_NORMAL);
        }
        #endregion

        #region Helper Methods
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
                    teamNames[0] = GetTeamName(wrestlers);
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
                    teamNames[1] = GetTeamName(wrestlers);
                }
            }
            catch
            {
                teamNames[0] = "Blue Team";
                teamNames[1] = "Red Team";
            }

        }
        public static String GetTeamName(List<String> wrestlers)
        {
            List<string> list = new List<string>(wrestlers);
            foreach (Team current in ModPack.ModPack.Teams)
            {
                bool flag = list.Count == 1;
                if (flag)
                {
                    break;
                }
                bool flag2 = Contains(list, current.Members);
                if (flag2)
                {
                    list.Add(current.Name);
                    foreach (string current2 in current.Members)
                    {
                        list.Remove(current2);
                    }
                }
            }
            int count = list.Count;
            int num = count;
            string result;
            if (num != 1)
            {
                if (num != 2)
                {
                    string text = string.Join(", ", list.ToArray());
                    text = text.Insert(text.LastIndexOf(",") + 2, "& ");
                    result = text;
                }
                else
                {
                    result = list[0] + " & " + list[1];
                }
            }
            else
            {
                result = list[0];
            }
            return result;
        }
        public static bool Contains(List<string> champs, List<string> members)
        {
            bool flag = champs.Count <= members.Count;
            bool result;
            if (flag)
            {
                foreach (string current in champs)
                {
                    bool flag2 = !members.Contains(current);
                    if (flag2)
                    {
                        result = false;
                        return result;
                    }
                }
            }
            else
            {
                foreach (string current2 in members)
                {
                    bool flag3 = !champs.Contains(current2);
                    if (flag3)
                    {
                        result = false;
                        return result;
                    }
                }
            }
            result = true;
            return result;
        }
        public static bool CheckMatchEnd(Referee r)
        {
            if (!isLuchaTag)
            {
                L.D("Not lucha tag rules");
                return false;
            }

            //Get the loser
            int loser = r.TargetPlIdx;
            //Determine if a captain has lost
            if (loser == 0)
            {
                L.D("Blue Captain lost");
                points[1] = 2;
                return false;
            }
            else if (loser == 4)
            {
                L.D("Red Captain lost");
                points[0] = 2;
                return false;
            }

            //Knocked out losers automatically end the match
            if (PlayerMan.inst.GetPlObj(loser).isKO)
            {
                if (loser < 4)
                {
                    points[1] = 2;
                }
                else
                {
                    points[0] = 2;
                }

                L.D("Player knocked out");
                return false;
            }

            if (loser < 4)
            {
                points[1]++;
                L.D("Blue loses a point");
            }
            else if (loser > 4)
            {
                points[0]++;
                L.D("Red loses a point");
            }

            if (points[0] >= 2 || points[1] >= 2)
            {
                L.D("Point total exceeded");
                return false;
            }

            //Signal that the current round has ended
            Announcer.inst.PlayGong_Eliminated();
            r.SetFree();

            PlayerMan.inst.GetPlObj(loser).isLoseAndStop = false;

            SetLegalMen("blue");
            SetLegalMen("red");

            DispNotification.inst.Show("Score -\t" + teamNames[0] + ": " + points[0] + "\t\t" + teamNames[1] + ": " + points[1], 300);
            return true;
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
        private static bool NoLegalManInRing()
        {
            bool result = true;
            for (int i = 0; i < 8; i++)
            {
                Player pl = PlayerMan.inst.GetPlObj(i);

                //Ignore if this spot is empty.
                if (!pl)
                {
                    continue;
                }

                if (!pl.hasRight)
                {
                    continue;
                }
                else
                {
                    if (IsInRing(pl))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }
        private static bool IsInRing(Player pl)
        {
            return pl.Zone == ZoneEnum.Apron || pl.Zone == ZoneEnum.OnCornerPost || pl.Zone == ZoneEnum.InRing;
        }
        private static void SetLegalMen(String forceSwitch)
        {
            Player blueMan = PlayerMan.inst.GetPlObj(GetLegalMan("blue"));
            Player redMan = PlayerMan.inst.GetPlObj(GetLegalMan("red"));

            //Process Blue Corner
            if (!IsInRing(blueMan) || forceSwitch.Equals("blue"))
            {
                Player nextPlayer = PlayerMan.inst.GetPlObj(GetNextTag("blue", blueMan.PlIdx));

                blueMan.hasRight = false;
                nextPlayer.hasRight = true;
                blueMan.TagStandbyPos = nextPlayer.TagStandbyPos;
                nextPlayer.isTagPartnerStandby = false;
                nextPlayer.Start_ForceControl(ForceCtrlEnum.GoBackToRing);
                blueMan.Start_ForceControl(ForceCtrlEnum.GoBackToApron);
            }

            //Process Red Corner
            if (!IsInRing(redMan) || forceSwitch.Equals("red"))
            {
                Player nextPlayer = PlayerMan.inst.GetPlObj(GetNextTag("red", redMan.PlIdx));

                redMan.hasRight = false;
                nextPlayer.hasRight = true;
                redMan.TagStandbyPos = nextPlayer.TagStandbyPos;
                nextPlayer.isTagPartnerStandby = false;
                nextPlayer.Start_ForceControl(ForceCtrlEnum.GoBackToRing);
                redMan.Start_ForceControl(ForceCtrlEnum.GoBackToApron);
            }
        }
        private static int GetLegalMan(String corner)
        {
            int legalMan = 0;
            switch (corner.ToLower())
            {
                case "blue":
                    for (int i = 0; i < 4; i++)
                    {
                        Player pl = PlayerMan.inst.GetPlObj(i);

                        //Ignore if this spot is empty.
                        if (!pl)
                        {
                            continue;
                        }

                        if (pl.hasRight)
                        {
                            legalMan = pl.PlIdx;
                            break;
                        }
                    }

                    break;

                case "red":
                    {
                        for (int i = 4; i < 8; i++)
                        {
                            Player pl = PlayerMan.inst.GetPlObj(i);

                            //Ignore if this spot is empty.
                            if (!pl)
                            {
                                continue;
                            }

                            if (pl.hasRight)
                            {
                                legalMan = pl.PlIdx;
                                break;
                            }
                        }

                        break;
                    }
            }

            return legalMan;
        }
        private static int GetNextTag(String corner, int legalMan)
        {
            switch (corner.ToLower())
            {
                case "blue":
                    for (int i = 0; i < 4; i++)
                    {
                        if (i == legalMan)
                        {
                            continue;
                        }

                        Player pl = PlayerMan.inst.GetPlObj(i);

                        //Ignore if this spot is empty.
                        if (!pl)
                        {
                            continue;
                        }

                        if (pl.hasRight)
                        {
                            continue;
                        }
                        else if (IsInRing(pl))
                        {
                            legalMan = i;
                            break;
                        }
                    }
                    break;
                case "red":
                    for (int i = 4; i < 8; i++)
                    {
                        if (i == legalMan)
                        {
                            continue;
                        }

                        Player pl = PlayerMan.inst.GetPlObj(i);

                        //Ignore if this spot is empty.
                        if (!pl)
                        {
                            continue;
                        }

                        if (pl.hasRight)
                        {
                            continue;
                        }
                        else if (IsInRing(pl))
                        {
                            legalMan = i;
                            break;
                        }
                    }
                    break;
            }

            return legalMan;
        }
        private static int GetPlayerCount()
        {
            return MatchConfiguration.GetPlayerCount();
        }
        #endregion
    }
}
