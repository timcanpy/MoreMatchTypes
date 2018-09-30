using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG;
using UnityEngine;

namespace MoreMatchTypes.Wrestling_Match_Types
{
    [FieldAccess(Class = "MatchMain", Field = "InitMatch", Group = "MoreMatchTypes"), FieldAccess(Class = "Player", Field = "UpdatePlayer", Group = "MoreMatchTypes"), FieldAccess(Class = "MatchMain", Field = "InitRound", Group = "MoreMatchTypes")]

    class TimedElimination
    {
        public static bool isTimedElim = false;
        public static int nextEntry = 2;
        public static int minutePassed = 0;

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void SetMatchRules()
        {
            MatchSetting settings = GlobalWork.inst.MatchSetting;
            if (settings.arena == VenueEnum.LandMine_BarbedWire || settings.arena == VenueEnum.LandMine_FluorescentLamp || settings.arena == VenueEnum.Dodecagon || settings.BattleRoyalKind == BattleRoyalKindEnum.Off)
            {

            }
        }
    }
}
