using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG;
using UnityEngine;


namespace MoreMatchTypes
{
    [FieldAccess(Class = "MatchMain", Field = "CreatePlayers", Group = "MoreMatchTypes"), FieldAccess(Class = "Player", Field = "UpdatePlayer", Group = "MoreMatchTypes"), FieldAccess(Class = "MatchMain", Field = "InitRound", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "PlayerController_AI", Field = "Process_CageDeathMatch", Group = "MoreMatchTypes")]
    class TimedTornadoTag
    {
        public static bool isTTT = false;
        public static String changeFlag = "blue";
        public static int minutePassed = 0;
        public static Queue<Player> blueTeam;
        public static Queue<Player> redTeam;
        public static CriticalRateEnum critRate;
        public static bool outOfRingCount;

        [Hook(TargetClass = "MatchMain", TargetMethod = "CreatePlayers", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void SetMatchRules()
        {
            MatchSetting settings = GlobalWork.inst.MatchSetting;

            if (settings.arena == VenueEnum.BarbedWire || settings.arena == VenueEnum.Dodecagon || settings.BattleRoyalKind != BattleRoyalKindEnum.Off || IsOneOnOne() || settings.isS1Rule)
            {
                return;
            }

            if (MoreMatchTypes_Form.form.cb_ttt.Checked)
            {
                isTTT = true;
                changeFlag = "blue";
                minutePassed = 0;
                InitTeams();
                settings.isTornadoBattle = true;
                settings.isCutPlay = false;
                critRate = settings.CriticalRate;
                outOfRingCount = settings.isOutOfRingCount;
                settings.VictoryCondition = VictoryConditionEnum.OnlyEscape;
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitRound", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void StartMatch()
        {
            if (!isTTT)
            {
                return;
            }
            try
            {
                foreach (Player pl in blueTeam)
                {
                    pl.Zone = ZoneEnum.Apron;
                    pl.hasRight = false;
                }

                foreach (Player pl in redTeam)
                {
                    pl.Zone = ZoneEnum.Apron;
                    pl.hasRight = false;
                }
            }
            catch (Exception ex)
            {
                L.D("Start error: " + ex.Message);
            }
        }

        [Hook(TargetClass = "Player", TargetMethod = "UpdatePlayer", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void CheckTime()
        {
            if (!isTTT)
            {
                return;
            }

            if (blueTeam.Count == 0 && redTeam.Count == 0)
            {
                return;
            }

            MatchMain main = MatchMain.inst;

            if (main.matchTime.min == 15 && main.matchTime.sec == 0 && minutePassed < main.matchTime.min)
            {
                SendInMember();

            }
            else if (main.matchTime.min == 13 && main.matchTime.sec == 0 && minutePassed < main.matchTime.min)
            {
                SendInMember();
            }
            else if (main.matchTime.min == 11 && main.matchTime.sec == 0 && minutePassed < main.matchTime.min)
            {
                SendInMember();
            }
            else if (main.matchTime.min == 9 && main.matchTime.sec == 0 && minutePassed < main.matchTime.min)
            {
                SendInMember();
            }
            else if (main.matchTime.min == 7 && main.matchTime.sec == 0 && minutePassed < main.matchTime.min)
            {
                SendInMember();
            }
            else if (main.matchTime.min == 5 & main.matchTime.sec == 0 && minutePassed < main.matchTime.min)
            {
                SendInMember();
            }

        }

        [Hook(TargetClass = "PlayerController_AI", TargetMethod = "Process_CageDeathMatch", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.ModifyReturn, Group = "MoreMatchTypes")]
        public static bool PreventEarlyFallCount(out bool result)
        {
            result = false;
            if (!isTTT)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

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
                if (!pl.isSecond)
                {
                    isOneOnOne = false;
                    break;
                }
            }
            return isOneOnOne;
        }
        private static void InitTeams()
        {
            blueTeam = new Queue<Player>();
            redTeam = new Queue<Player>();
            for (int i = 0; i < 4; i++)
            {
                Player pl = PlayerMan.inst.GetPlObj(i);

                //Ignore if this spot is empty.
                if (!pl)
                {
                    continue;
                }
                if (i != 0)
                {

                    blueTeam.Enqueue(pl);
                }
            }

            for (int i = 4; i < 8; i++)
            {
                Player pl = PlayerMan.inst.GetPlObj(i);

                //Ignore if this spot is empty.
                if (!pl)
                {
                    continue;
                }

                if (i != 4)
                {
                    redTeam.Enqueue(pl);
                }
            }

        }
        private static void SendInMember()
        {
            if (changeFlag.Equals("blue"))
            {
                if (blueTeam.Count == 0 && redTeam.Count != 0)
                {
                    changeFlag = "red";
                    SendInMember();
                }
                else if (blueTeam.Count != 0)
                {
                    Player pl = blueTeam.Dequeue();
                    pl.hasRight = true;
                    pl.Start_ForceControl(ForceCtrlEnum.GoBackToRing);
                }
            }
            else if (changeFlag.Equals("red"))
            {
                if (redTeam.Count == 0 && blueTeam.Count != 0)
                {
                    changeFlag = "blue";
                    SendInMember();
                }
                else if (redTeam.Count != 0)
                {
                    Player pl = redTeam.Dequeue();
                    pl.hasRight = true;
                    pl.Start_ForceControl(ForceCtrlEnum.GoBackToRing);
                }
            }

            SwitchFlag();

            Announcer.inst.PlayGong_Eliminated();
            minutePassed = MatchMain.inst.matchTime.min;

            //Determine if match rules should change
            if (blueTeam.Count == 0 && redTeam.Count == 0)
            {
                Announcer.inst.PlayGong_MatchStart();
                GlobalWork.inst.MatchSetting.VictoryCondition = VictoryConditionEnum.Count3;
                GlobalWork.inst.MatchSetting.isOutOfRingCount = outOfRingCount;
                GlobalWork.inst.MatchSetting.CriticalRate = critRate;
                DispNotification.inst.Show("Pinfall victories are now possible!", 180);
            }
        }
        private static void SwitchFlag()
        {
            if (changeFlag.Equals("blue"))
            {
                changeFlag = "red";
            }
            else
            {
                changeFlag = "blue";
            }
        }
    }
}
