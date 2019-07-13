using DG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MoreMatchTypes.Shoot_Match_Types
{
    class K1MMA
    {
        public static bool isK1MMA;
        public static WrestlerAppearanceData[] origAppear = new WrestlerAppearanceData[8] { null, null, null, null, null, null, null, null };


        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue,
              InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void SetMatchRules()
        {
            isK1MMA = false;
            MatchSetting settings = GlobalWork.inst.MatchSetting;

            if (settings.arena == VenueEnum.BarbedWire || settings.arena == VenueEnum.Cage || settings.arena == VenueEnum.Dodecagon || settings.arena == VenueEnum.LandMine_BarbedWire || settings.arena == VenueEnum.LandMine_FluorescentLamp)
            {
                return;
            }
            if (settings.BattleRoyalKind != BattleRoyalKindEnum.Off)
            {
                return;
            }

            if (MoreMatchTypes_Form.form.isk1mma.Checked && IsOneOnOne())
            {
                isK1MMA = true;
                settings.VictoryCondition = VictoryConditionEnum.OnlyGiveUp;
                settings.TKOCount = 0;
                settings.is10CountKO = true;
                settings.isLumberjack = true;
                settings.isFoulCount = false;
                settings.isOutOfRingCount = false;
                settings.isS1Rule = true;

                MatchWrestlerInfo[] mwi = GlobalWork.inst.MatchSetting.matchWrestlerInfo;
                for (int i = 0; i < 8; i++)
                {
                    if (mwi[i] != null)
                    {
                        if (mwi[i].entry)
                        {
                            origAppear[i] = DataBase.GetAppearanceData(mwi[i].wrestlerID);
                        }
                    }
                }
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitRound", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MoreMatchTypes")]
        public static void StartRound(MatchMain m)
        {
            if (!isK1MMA)
            {
                return;
            }

            MatchSetting settings = GlobalWork.inst.MatchSetting;
            L.D("Round # " + m.RoundCnt);
            //MMA Rules
            if (m.RoundCnt % 2 == 0)
            {
                settings.isS1Rule = false;
                PrepareGear("mma");
            }
            //K1 Rules
            else
            {
                settings.isS1Rule = true;
                PrepareGear("k1");
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "EndMatch", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MoreMatchTypes")]
        public static void RestoreAppearanceData()
        {

            if (!isK1MMA)
            {
                return;
            }
            MatchWrestlerInfo[] mwi = GlobalWork.inst.MatchSetting.matchWrestlerInfo;
            for (int i = 0; i < 8; i++)
            {
                if (origAppear[i] != null)
                {
                    if (mwi[i] != null)
                    {
                        if (mwi[i].wrestlerID < WrestlerID.EditWrestlerIDTop)
                        {
                            PresetWrestlerData preset = PresetWrestlerDataMan.inst.GetPresetWrestlerData(mwi[i].wrestlerID);
                            preset.appearance.Set(origAppear[i]);
                        }
                        else
                        {
                            EditWrestlerData edit = SaveData.inst.GetEditWrestlerData(mwi[i].wrestlerID);
                            edit.appearanceData.Set(origAppear[i]);
                        }
                    }
                }
            }
        }

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

        private static void PrepareGear(String type)
        {
            String[] handParts;
            switch (type.ToLower())
            {
                case "mma":
                    for (int i = 0; i < 8; i++)
                    {
                        try
                        {
                            MatchWrestlerInfo mwi = GlobalWork.inst.MatchSetting.matchWrestlerInfo[i];
                            if (mwi != null)
                            {
                                CostumeData cd = new CostumeData();
                                cd.Set(DataBase.GetCostumeData(mwi.wrestlerID, mwi.costume_no));
                                cd = ClearHandLayers(cd);

                                cd.layerTex[7, 0] = origAppear[i].costumeData[0].layerTex[7, 0];
                                cd.layerTex[7, 1] = "m_ha_0001_m_1";
                                cd.layerTex[7, 2] = "m_ha_0002_m_1";
                                cd.layerTex[7, 3] = "m_ha_0004_m_1";

                                if (i < 4)
                                {
                                    cd.color[7, 1] = Color.blue;
                                    cd.color[7, 2] = Color.blue;
                                    cd.color[7, 3] = Color.blue;
                                }
                                else
                                {
                                    cd.color[7, 1] = Color.red;
                                    cd.color[7, 2] = Color.red;
                                    cd.color[7, 3] = Color.red;
                                }

                                Player plObj = PlayerMan.inst.GetPlObj(i);
                                plObj.FormRen.DestroySprite();
                                plObj.FormRen.InitTexture(cd);
                                plObj.FormRen.InitSprite();
                            }
                        }
                        catch (NullReferenceException e)
                        {
                        }

                    }
                    break;
                case "k1":
                    for (int i = 0; i < 8; i++)
                    {
                        try
                        {
                            MatchWrestlerInfo mwi = GlobalWork.inst.MatchSetting.matchWrestlerInfo[i];
                            if (mwi != null)
                            {
                                CostumeData cd = new CostumeData();
                                cd.Set(DataBase.GetCostumeData(mwi.wrestlerID, mwi.costume_no));
                                cd = ClearHandLayers(cd);

                                cd.layerTex[7, 0] = origAppear[i].costumeData[0].layerTex[7, 0];
                                cd.layerTex[7, 1] = "m_ha_0003_m_1";

                                if (i < 4)
                                {
                                    cd.color[7, 1] = Color.blue;
                                }
                                else
                                {
                                    cd.color[7, 1] = Color.red;
                                }

                                Player plObj = PlayerMan.inst.GetPlObj(i);
                                plObj.FormRen.DestroySprite();
                                plObj.FormRen.InitTexture(cd);
                                plObj.FormRen.InitSprite();
                            }
                        }
                        catch (NullReferenceException e)
                        {
                        }
                    }
                    break;
            }
        }

        private static CostumeData ClearHandLayers(CostumeData cd)
        {
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    cd.layerTex[7, i] = null;
                }
            }
            catch (NullReferenceException ex)
            {
            }

            return cd;
        }
    }
}
