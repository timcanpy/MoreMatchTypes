using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG;
using MatchConfig;
using MoreMatchTypes.Wrestling_Match_Types;
using UnityEngine;

namespace MoreMatchTypes.Data_Classes
{
    class EliminationUpdate : MonoBehaviour
    {
        private Player newPlayer;
        private VenueSetting venue;
        private DefeatedPlayer replacementPlayer;
        public void Init()
        {
            newPlayer = null;
            venue = Ring.inst.venueSetting;
            replacementPlayer = null;
        }

        private void Update()
        {
            if (MatchMain.inst.State == StateEnum.EntranceScene)
            {
                return;
            }

            if (!ExElimination.isExElim)
            {
                return;
            }

            if (ExElimination.defeatedPlayers.Count == 0 && newPlayer == null && replacementPlayer == null)
            {
                return;
            }

            //Repeat until this player is updated, then process the next
            if (newPlayer != null)
            {
                if (newPlayer.Zone == ZoneEnum.OutOfRing)
                {
                    newPlayer.hasRight = false;
                    newPlayer.isLoseAndStop = false;
                    L.D(DataBase.GetWrestlerFullName(newPlayer.WresParam) + " is now at ringside");
                    newPlayer = null;
                }
                else
                {
                    return;
                }
            }

            //Ensure that only one replacement is being processed at a time
            if (replacementPlayer == null)
            {
                if (ExElimination.defeatedPlayers.Count > 0)
                {
                    replacementPlayer = ExElimination.defeatedPlayers.Dequeue();

                    //Ensure that we have replacements remaining, before proceeding
                    if (replacementPlayer.side == CornerSide.Blue)
                    {
                        if (ExElimination.blueTeamReplacements.Count == 0)
                        {
                            L.D("Blue team is out of replacements");
                            replacementPlayer = null;
                            return;
                        }
                    }
                    else
                    {
                        if (ExElimination.redTeamReplaements.Count == 0)
                        {
                            L.D("Red team is out of replacements");
                            replacementPlayer = null;
                            return;
                        }
                    }

                    L.D(DataBase.GetWrestlerFullName(replacementPlayer.player.WresParam) + " has been queued for replacement");
                }
                else
                {
                    return;
                }
            }

            if (replacementPlayer.player.isSleep)
            {
                WresIDGroup nextMember;
                int index = replacementPlayer.player.PlIdx;

                //Updating remaining team members
                if (replacementPlayer.side == CornerSide.Blue)
                {
                    ExElimination.blueOrderQueue.Enqueue(index);
                    L.D(index + " has been queued for entry in the Blue Order queue.");
                    nextMember = ExElimination.blueTeamReplacements.Dequeue();
                }
                else
                {
                    ExElimination.redOrderQueue.Enqueue(index);
                    L.D(index + " has been queued for entry in the Red Order queue.");
                    nextMember = ExElimination.redTeamReplaements.Dequeue();
                }

                var group = replacementPlayer.player.Group;
                newPlayer = ActivatePlayer(replacementPlayer.player, nextMember);

                //Resetting the player controller AI
                newPlayer.plCont_AI = new PlayerController_AI(index);
                newPlayer.plCont_AI.Init(newPlayer);

                //Necessary to ensure the player is activated.
                newPlayer.plController.plIdx = index;
                newPlayer.SetPlayerController(PlayerControllerKind.AI);
                newPlayer.Start_ForceControl(ForceCtrlEnum.SecondStanbdby);

                newPlayer.isSecond = true;
                newPlayer.SecondPos = MatchData.SecondStandbyPosIdxTbl[newPlayer.PlIdx];
                newPlayer.Group = group;

                //Set the starting location
                newPlayer.Zone = ZoneEnum.OutOfRing;
                if (replacementPlayer.side == CornerSide.Blue)
                {
                    newPlayer.PlPos.x = MatchData.SecondStandbyPosTbl[0].x;
                    newPlayer.PlPos.y = MatchData.SecondStandbyPosTbl[0].y;
                }
                else
                {
                    newPlayer.PlPos.x = MatchData.SecondStandbyPosTbl[5].x;
                    newPlayer.PlPos.y = MatchData.SecondStandbyPosTbl[5].y;
                }


                L.D(DataBase.GetWrestlerFullName(replacementPlayer.player.WresParam) + " has been replaced by " +
                    DataBase.GetWrestlerFullName(newPlayer.WresParam));
                replacementPlayer = null;
            }

            #region Old code
            //int initialIndex;
            //if (ExElimination.loserSide == CornerSide.Blue)
            //{
            //    initialIndex = 0;
            //}
            //else
            //{
            //    initialIndex = 4;
            //}

            //Determine if a player is in position to be replaced.
            //for (int i = initialIndex; i < 8; i++)
            //{
            //Player plObj = PlayerMan.inst.GetPlObj(i);
            //if (!plObj)
            //{
            //    continue;
            //}

            //if (plObj.isSleep)
            ////{
            //    if (i < 4)
            //    {
            //        ExElimination.blueOrderQueue.Enqueue(i);
            //        nextMember = ExElimination.blueTeamMembers.Dequeue();

            //    }
            //    else
            //    {
            //        ExElimination.redOrderQueue.Enqueue(i);
            //        nextMember = ExElimination.redTeamMembers.Dequeue();
            //    }

            ////Replace with new player, and send him back to the ring.
            //var group = plObj.Group;
            //newPlayer = ActivatePlayer(plObj, nextMember);

            ////Resetting the player controller AI
            //newPlayer.forceControl = ForceCtrlEnum.None;
            //newPlayer.plCont_AI = new PlayerController_AI(i);
            //newPlayer.plCont_AI.Init(newPlayer);

            ////Necessary to ensure the player is activated.
            //newPlayer.plController.kind = PlayerControllerKind.AI;
            //newPlayer.plController.plIdx = i;

            //newPlayer.isIntruder = true;
            //newPlayer.intrusionTimer = 100;
            //newPlayer.SecondPos = MatchData.SecondStandbyPosIdxTbl[newPlayer.PlIdx];
            //newPlayer.Group = group;

            //newPlayer.SetPlayerController(PlayerControllerKind.AI);
            //newPlayer.Start_ForceControl(ForceCtrlEnum.SecondStanbdby);

            //}
            //}
            #endregion
        }

        private Player ActivatePlayer(Player plObj, WresIDGroup wrestler)
        {
            plObj.FormRen.DestroySprite();

            #region Variables
            int idx = plObj.PlIdx;
            MatchSetting matchSetting = GlobalWork.inst.MatchSetting;
            MatchWrestlerInfo info = MatchConfiguration.CreateWrestlerInfo(wrestler.ID);
            Player player = PlayerMan.inst.CreatePlayer(idx, null);
            WrestlerAppearanceData appearanceData = SaveData.inst.GetEditWrestlerData(info.wrestlerID).appearanceData;
            #endregion

            #region Setting wrestler appearance
            player.Init((WrestlerID)wrestler.ID, PadPort.AI, wrestler.Group);
            player.FormRen.InitTexture(appearanceData.costumeData[info.costume_no], null);
            player.FormRen.InitSprite(false);
            for (int i = 0; i < 5; i++)
            {
                Menu_SoundManager.Sound_ClipWrestlerVoice_List[idx, i] = null;
            }
            for (int j = 0; j < 5; j++)
            {
                Menu_SoundManager.Load_WrestlerVoice(idx, j, info.param.voiceType[j], info.param.voiceID[j]);
            }
            #endregion

            player.finishMove_Atk.Clear();
            player.finishMove_Def[0].Clear();
            player.finishMove_Def[1].Clear();

            if (idx < 4)
            {
                player.MyGroupPlIdx_Start = 0;
                player.MyGroupPlIdx_End = 4;
                player.EnemyGroupPlIdx_Start = 4;
                player.EnemyGroupPlIdx_End = 8;
                player.finishWewstlerID = matchSetting.matchWrestlerInfo[4].wrestlerID;
            }
            else
            {
                player.MyGroupPlIdx_Start = 4;
                player.MyGroupPlIdx_End = 8;
                player.EnemyGroupPlIdx_Start = 0;
                player.EnemyGroupPlIdx_End = 4;
                player.finishWewstlerID = matchSetting.matchWrestlerInfo[0].wrestlerID;
            }

            player.animator.ReqBasicAnm(BasicSkillEnum.Stand_Power_S, false, -1);
            player.animator.UpdateAnimation();
            player.animator.CurrentSkill = SkillDataMan.inst.GetSkillData_Standard(BasicSkillEnum.Stand_Power_S);
            player.animator.CurrentAnmIdx = 0;
            player.animator.AnmHostPlayer = player.PlIdx;
            player.ChangeState(PlStateEnum.Stand);
            player.animator.isAnmPause = false;

            #region Setting player to enter
            VenueSetting venueSetting = Ring.inst.venueSetting;
            player.moveSpeed = player.WresParam.walkSpeed;
            player.isLoseAndStop = false;
            player.isLose = false;
            player.isEntranceWalking = false;
            player.isEntrancePerformance = false;
            player.hasRight = true;
            player.SetSleep(false);
            player.TargetPlIdx = player.PlIdx;
            player.Group = idx;

            //player.PlPos.x = MatchData.PlayerInitialPosTbl[idx].x;
            //player.PlPos.y = MatchData.PlayerInitialPosTbl[idx].y;
            #endregion

            PlayerMan.inst.SetPlayer(idx, player);
            return player;
        }
    }
}
