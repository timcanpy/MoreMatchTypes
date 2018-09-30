using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DG;

namespace MoreMatchTypes
{
    #region Access Modifiers
    [FieldAccess(Class = "Referee", Field = "CheckMatchEnd", Group = "MoreMatchTypes"), FieldAccess(Class = "MatchMain", Field = "InitMatch", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "Menu_Result", Field = "Set_FinishSkill", Group = "MoreMatchTypes"), FieldAccess(Class = "MatchMain", Field = "ProcessMatchEnd_Draw", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "MatchMain", Field = "EndMatch", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "MatchMain", Field = "InitRound", Group = "MoreMatchTypes")]
    #endregion
    public class IronManMatch
    {
        #region Variables
        //Disabled for Battle Royal matches, therefore we will only have two teams to keep track of
        public static int[] wins = new int[2];
        public static string[] teamNames = new string[2];
        public static bool endMatch = false;
        public static MatchTime currMatchTime = null;
        public static bool isIronMan = false;
        #endregion

        #region Injection methods

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void SetMatchRules()
        {
            if (MoreMatchTypes_Form.form.cb_IronManMatch.Checked && GlobalWork.inst.MatchSetting.BattleRoyalKind == BattleRoyalKindEnum.Off)
            {
                isIronMan = true;
            }
            else
            {
                isIronMan = false;
            }

            if(!isIronMan)
            {
                return;
            }

            wins = new int[2];
            endMatch = false;
            currMatchTime = null;

            SetTeamNames();
            MoreMatchTypes_Form.form.Enabled = false;
        }

        [Hook(TargetClass = "Referee", TargetMethod = "CheckMatchEnd", InjectionLocation = 0, InjectFlags = HookInjectFlags.ModifyReturn, Group = "MoreMatchTypes")]
        public static bool SetMatchRestrictions()
        {
           if(!isIronMan)
            {
                return false;
            }
            else
            {
                MatchMain main = MatchMain.inst;

                //If a victory condition is met
                if (!main.isTimeUp && main.isMatchEnd)
                {
                    //Signal that the current round has ended
                    Referee matchRef = RefereeMan.inst.GetRefereeObj();
                    matchRef.PlDir = PlDirEnum.Left;
                    matchRef.ReqRefereeAnm(BasicSkillEnum.Refe_Stand_MatchEnd_Front_Left);
                    Announcer.inst.PlayGong_Eliminated();

                    int loser = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        Player plObj = PlayerMan.inst.GetPlObj(i);
                        if (!plObj)
                        {
                            continue;
                        }
                        if (plObj.isLoseAndStop)
                        {
                            loser = i;
                        }
                        if (plObj.isKO)
                        {
                            plObj.isKO = false;
                        }
                    }

                    //Record the last win and display the current score
                    if (loser <= 3)
                    { wins[1]++; }
                    else
                    { wins[0]++; }

                    //Prepare the next round
                    main.isMatchEnd = false;
                    main.isRoundEnd = true;
                    currMatchTime = new MatchTime
                    {
                        min = main.matchTime.min,
                        sec = main.matchTime.sec
                    };
                    main.isTimeCounting = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [Hook(TargetClass = "Menu_Result", TargetMethod = "Set_FinishSkill", InjectionLocation = 8, InjectDirection = HookInjectDirection.After, InjectFlags = HookInjectFlags.PassParametersVal | HookInjectFlags.PassLocals, LocalVarIds = new int[] { 1 }, Group = "MoreMatchTypes")]
        public static void SetResultScreenDisplay(ref UILabel finishText, string str)
        {
            if (isIronMan && endMatch)
            {
                string winResult = teamNames[0] + " : " + wins[0] + " win(s)\n" + teamNames[1] + " - " + wins[1] + " win(s)\n\n" + "Winner - " + (wins[0] > wins[1] ? teamNames[0] : teamNames[1]);
                string resultString = str.Replace("K.O.", "Iron Man Match\n\n" + winResult);
                finishText.text = resultString;
                endMatch = false;
                Array.Clear(wins, 0, wins.Length);
                Array.Clear(teamNames, 0, teamNames.Length);

            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "ProcessMatchEnd_Draw", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void SetVictoryConditions()
        {
            if (!isIronMan)
            { return; }
            else
            {
                Referee matchRef = RefereeMan.inst.GetRefereeObj();
                MatchMain main = MatchMain.inst;
                if (main.isTimeUp)
                {
                    endMatch = true;
                    main.isMatchEnd = true;
                    matchRef.PlDir = PlDirEnum.Left;
                    matchRef.ReqRefereeAnm(BasicSkillEnum.Refe_Stand_MatchEnd_Front_Left);
                    matchRef.State = RefeStateEnum.DeclareVictory;
                    Announcer.inst.PlayGong_MatchEnd();

                    //Determine the winner
                    PlayerMan p = PlayerMan.inst;

                    if (wins[0] > wins[1])
                    {
                        global::MatchEvaluation.inst.ResultType = global::MatchResultEnum.KO;
                        matchRef.SentenceLose(p.GetPlObj(4).PlIdx);
                        SetLosers(4, p);

                    }
                    else if (wins[0] < wins[1])
                    {
                        global::MatchEvaluation.inst.ResultType = global::MatchResultEnum.KO;
                        matchRef.SentenceLose(p.GetPlObj(0).PlIdx);
                        SetLosers(0, p);
                    }
                }
                else
                {
                    return;
                }
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "EndMatch", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void ResetWins()
        {
            if (isIronMan)
            {
                MoreMatchTypes_Form.form.Enabled = true;
                DG.Carlzilla.WentToDecision = false;
                currMatchTime = null;
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitRound", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        public static void UpdateMatchTimer(MatchMain m)
        {
            try
            {
                if (isIronMan && currMatchTime != null)
                {
                    DispNotification.inst.Show(teamNames[0] + " : " + wins[0] + "      " + teamNames[1] + " : " + wins[1], 300);
                    m.matchTime.Set(currMatchTime);

                    //Replenish wrestlers hp, stamina and spirit
                    for (int i = 0; i < 8; i++)
                    {
                        Player plObj = PlayerMan.inst.GetPlObj(i);
                        if (!plObj)
                        {
                            continue;
                        }
                        if (!plObj.isSecond && !plObj.isSleep)
                        {
                            float hp = plObj.HP;
                            float sp = plObj.SP;
                            float bp = plObj.BP;
                            float recoverVal = 65535f * UnityEngine.Random.Range(.15f, .3f);
                            float recoveryParam = (float)plObj.WresParam.hpRecovery;

                            hp = hp + recoverVal + (recoverVal * (recoveryParam) * .2f);
                            sp = sp + recoverVal + (recoverVal * (recoveryParam) * .2f);
                            bp = bp + recoverVal + (recoverVal * (recoveryParam) * .2f);

                            plObj.SetSP(sp);
                            plObj.SetHP(hp * .5f);
                            plObj.SetBP(bp * .75f);
                        }
                    }
                }
                else
                {
                    return;
                }
            }
            catch
            {
                return;
            }
        }

        [ControlPanel(Group = "MoreMatchTypes")]
        public static Form MSForm()
        {
            if (MoreMatchTypes_Form.form == null)
            {
                return new MoreMatchTypes_Form();
            }
            {
                return null;
            }
        }
        #endregion

        #region helper methods
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
        #endregion
    }
}
