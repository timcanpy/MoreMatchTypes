using DG;
using MatchConfig;
using MoreMatchTypes.Data_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MoreMatchTypes.DataClasses;
using ModPack;

namespace MoreMatchTypes
{
    public partial class SurvivalRoadForm : Form
    {
        #region Variables
        public static SurvivalRoadForm survivalForm = null;
        public static List<String> promotionList = new List<string>();
        public static List<MatchConfig.WresIDGroup> wrestlerList = new List<MatchConfig.WresIDGroup>();
        MatchConfig.WresIDGroup[] initialOpponents = new MatchConfig.WresIDGroup[2];
        #endregion

        #region Initialization Methods
        public SurvivalRoadForm()
        {
            InitializeComponent();
        }
        private void SurvivalRoadForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadPromotions();
                LoadEdits();
                LoadRings();
                LoadReferees();
                LoadThemes();
                LoadDifficulty();
                LoadGameSpeed();
                LoadVenues();
                LoadContinues();
                LoadMatches();
                LoadMatchTypes();
                LoadSettings();
            }
            catch (Exception ex)
            {
                L.D("SurvivalRoadForm_LoadError: " + ex);
            }
        }
        private void LoadPromotions()
        {
            promotionList.Clear();
            sr_promotionList.Items.Clear();

            foreach (String promotion in MatchConfiguration.LoadPromotions())
            {
                sr_promotionList.Items.Add(promotion);
                promotionList.Add(promotion);
            }

            sr_promotionList.SelectedIndex = 0;
        }
        private void LoadEdits()
        {
            wrestlerList.Clear();
            sr_searchResult.Items.Clear();

            wrestlerList = MatchConfiguration.LoadWrestlers();

            foreach (MatchConfig.WresIDGroup wrestler in wrestlerList)
            {
                sr_searchResult.Items.Add(wrestler);
            }

            sr_promotionList.SelectedIndex = 0;
            sr_searchResult.SelectedIndex = 0;
        }
        private void LoadRings()
        {
            foreach (var ring in MatchConfiguration.LoadRings())
            {
                sr_ringList.Items.Add(ring);
            }

            sr_ringList.SelectedIndex = 0;
        }
        private void LoadReferees()
        {
            foreach (RefereeInfo referee in MatchConfiguration.LoadReferees())
            {
                sr_refereeList.Items.Add(referee);
            }

            sr_refereeList.SelectedIndex = 0;
        }
        private void LoadThemes()
        {
            foreach (String theme in MatchConfiguration.LoadBGMs())
            {
                sr_bgmList.Items.Add(theme);
            }

            sr_bgmList.SelectedIndex = 0;
        }
        private void LoadDifficulty()
        {
            foreach (String i in MatchConfiguration.LoadDifficulty())
            {
                sr_difficultyList.Items.Add(i);
            }

            sr_difficultyList.SelectedIndex = 0;
        }
        private void LoadGameSpeed()
        {
            foreach (uint speed in MatchConfiguration.LoadSpeed())
            {
                sr_speedList.Items.Add(speed);
            }
            sr_speedList.SelectedIndex = 0;
        }
        private void LoadVenues()
        {
            String[] venues = MatchConfiguration.LoadVenue();
            foreach (String venue in venues)
            {
                sr_venueList.Items.Add(venue);
            }

            sr_venueList.SelectedIndex = 0;
        }
        private void LoadContinues()
        {
            for (int i = 10; i >= 0; i--)
            {
                sr_continues.Items.Add(i);
            }

            sr_continues.SelectedIndex = sr_continues.Items.Count - 1;
        }
        private void LoadMatches()
        {
            for (int i = 100; i >= 1; i--)
            {
                sr_matches.Items.Add(i);
            }

            sr_matches.SelectedIndex = sr_matches.Items.Count - 1;
        }
        private void LoadMatchTypes()
        {
            sr_matchType.Items.Add("Normal");
            sr_matchType.Items.Add("Cage");
            sr_matchType.Items.Add("Barbed Wire");
            sr_matchType.Items.Add("Landmine");
            sr_matchType.Items.Add("SWA");
            sr_matchType.Items.Add("Gruesome");
            sr_matchType.Items.Add("S - 1");

            sr_matchType.SelectedIndex = 0;
        }
        private void LoadSettings()
        {
            SurvivalRoadData survivalRoadData = MoreMatchTypes_Form.SurvivalRoadData;
            if (survivalRoadData == null)
            {
                MoreMatchTypes_Form.SurvivalRoadData = new SurvivalRoadData();
            }
            else
            {
                //Referee
                try
                {
                    sr_refereeList.SelectedIndex = survivalRoadData.Referee == null
                        ? 0
                        : sr_refereeList.FindString(survivalRoadData.Referee.Name);
                }
                catch
                {
                    sr_refereeList.SelectedIndex = 0;
                }

                //Venue
                try
                {
                    sr_venueList.SelectedIndex =
                        survivalRoadData.Venue == null ? 0 : sr_venueList.FindString(survivalRoadData.Venue);
                }
                catch
                {
                    sr_venueList.SelectedIndex = 0;
                }

                //Ring
                try
                {
                    sr_ringList.SelectedIndex = survivalRoadData.Ring == null
                        ? 0
                        : sr_ringList.FindString(survivalRoadData.Ring.Name);
                }
                catch (Exception e)
                {
                    sr_ringList.SelectedIndex = 0;
                }

                //Bgm
                try
                {
                    sr_bgmList.SelectedItem = survivalRoadData.MatchBGM;
                }
                catch
                {
                    sr_bgmList.SelectedIndex = 0;
                }

                sr_speedList.SelectedItem = survivalRoadData.Speed;
                sr_difficultyList.SelectedItem = survivalRoadData.Difficulty;

                if (survivalRoadData.Wrestler != null)
                {
                    sr_wrestler.Items.Add(survivalRoadData.Wrestler);
                    sr_wrestler.SelectedIndex = 0;
                }
                if (survivalRoadData.Second != null)
                {
                    sr_second.Items.Add(survivalRoadData.Second);
                    sr_second.SelectedIndex = 0;
                }

                sr_continues.SelectedItem = survivalRoadData.Continues;
                sr_matches.SelectedItem = survivalRoadData.Matches;
                sr_regenHP.Checked = survivalRoadData.RegainHP;
                sr_single.Checked = survivalRoadData.Singles;
                sr_tag.Checked = survivalRoadData.Tag;
                sr_controlBoth.Checked = survivalRoadData.ControlBoth;
                sr_cutplay.Checked = survivalRoadData.CutPlay;
                sr_matchType.SelectedItem = survivalRoadData.MatchType;
                sr_simulate.Checked = survivalRoadData.Simulate;
                sr_simSecond.Checked = survivalRoadData.ControlSecond;
                sr_teamName.Text = survivalRoadData.OpponentName;
                sr_random.Checked = survivalRoadData.RandomSelect;

                foreach (MatchConfig.WresIDGroup opponent in survivalRoadData.Opponents)
                {
                    sr_teamList.Items.Add(opponent);
                }

                PopulateProgressBox(survivalRoadData.MatchProgress);
            }
        }

        #endregion

        #region Team Management
        private void sr_Add_Click(object sender, EventArgs e)
        {
            try
            {
                if (sr_searchResult.SelectedItem == null)
                {
                    return;
                }

                MatchConfig.WresIDGroup wrestler = (MatchConfig.WresIDGroup)sr_searchResult.SelectedItem;

                if (sr_addWrestler.Checked)
                {
                    if (!MatchConfiguration.ValidateWrestler((WrestlerID)wrestler.ID))
                    {
                        ShowError(wrestler.Name + " includes DLC that you do not own.");
                        return;
                    }

                    sr_wrestler.Items.Clear();
                    sr_wrestler.Items.Add(sr_searchResult.SelectedItem);
                    sr_wrestler.SelectedIndex = 0;
                }
                if (sr_addSecond.Checked)
                {
                    if (!MatchConfiguration.ValidateWrestler((WrestlerID)wrestler.ID))
                    {
                        ShowError(wrestler.Name + " includes DLC that you do not own.");
                        return;
                    }

                    sr_second.Items.Clear();
                    sr_second.Items.Add(sr_searchResult.SelectedItem);
                    sr_second.SelectedIndex = 0;
                }
                if (sr_addSingle.Checked)
                {
                    if (!MatchConfiguration.ValidateWrestler((WrestlerID)wrestler.ID))
                    {
                        ShowError(wrestler.Name + " includes DLC that you do not own.");
                        return;
                    }

                    sr_teamList.Items.Add(sr_searchResult.SelectedItem);
                }
                if (sr_addAll.Checked)
                {
                    int invalidOptions = 0;

                    foreach (MatchConfig.WresIDGroup item in sr_searchResult.Items)
                    {
                        if (sr_wrestler.Items.Count > 0)
                        {
                            if (!MatchConfiguration.ValidateWrestler((WrestlerID)item.ID))
                            {
                                invalidOptions++;
                                continue;
                            }
                            if (((MatchConfig.WresIDGroup)sr_wrestler.SelectedItem).Name.Equals(item.Name))
                            {
                               continue;
                            }
                            if (sr_second.Items.Count > 0)
                            {
                                if (((MatchConfig.WresIDGroup)sr_second.SelectedItem).Name.Equals(item.Name))
                                {
                                    continue;
                                }
                            }
                        }
                        sr_teamList.Items.Add(item);
                    }

                    if (invalidOptions > 0)
                    {
                        ShowError(invalidOptions + " wrestlers include DLC that you do not own, and were skipped.");
                    }
                }
            }
            catch (Exception exception)
            {
                L.D(exception.ToString());
            }

        }
        private void sr_Refresh_Click(object sender, EventArgs e)
        {
            LoadEditsFromPromotion();
        }
        private void sr_RemoveOne_Click(object sender, EventArgs e)
        {
            try
            {
                sr_teamList.Items.Remove(sr_teamList.SelectedItem);
            }
            catch (Exception exception)
            {
                L.D(exception.ToString());
            }
        }
        private void sr_removeAll_Click(object sender, EventArgs e)
        {
            sr_teamList.Items.Clear();
        }
        private void sr_Search_Click(object sender, EventArgs e)
        {
            LoadEditsFromPromotion();
            if (sr_searchResult.Items.Count > 0)
            {
                sr_searchResult.SelectedIndex = 0;
            }
        }
        private void sr_wrestlerClear_Click(object sender, EventArgs e)
        {
            sr_wrestler.Items.Clear();
        }
        private void sr_secondClear_Click(object sender, EventArgs e)
        {
            sr_second.Items.Clear();
        }
        private void sr_promotionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            sr_Search_Click(sender, e);
        }
        private void sr_randomWrestler_Click(object sender, EventArgs e)
        {
            if (sr_searchResult.Items.Count == 0)
            {
                return;
            }

            sr_wrestler.Items.Clear();
            sr_wrestler.Items.Add(sr_searchResult.Items[UnityEngine.Random.Range(0, sr_searchResult.Items.Count)]);
            sr_wrestler.SelectedIndex = 0;
        }
        private void sr_randomSecond_Click(object sender, EventArgs e)
        {
            if (sr_searchResult.Items.Count == 0)
            {
                return;
            }

            sr_second.Items.Clear();
            sr_second.Items.Add(sr_searchResult.Items[UnityEngine.Random.Range(0, sr_searchResult.Items.Count)]);
            sr_second.SelectedIndex = 0;
        }
        private void sr_promotionList_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            LoadEditsFromPromotion();
            if (sr_searchResult.Items.Count > 0)
            {
                sr_searchResult.SelectedIndex = 0;
            }
        }

        #endregion

        #region Match Management
        private void sr_matchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Setting general variables
            sr_controlBoth.Checked = false;
            sr_tag.Visible = true;
            sr_addSecond.Visible = true;
            string selectedType = sr_matchType.SelectedItem.ToString();
            //Ensuring that tornado tag only modes are accounted for
            if (selectedType.Equals("Normal"))
            {
                if (sr_tag.Checked)
                {
                    sr_controlBoth.Visible = true;
                    sr_cutplay.Visible = true;
                }
            }
            else
            {
                sr_controlBoth.Visible = false;
                sr_cutplay.Visible = false;
                sr_controlBoth.Checked = false;
                sr_cutplay.Checked = false;
            }

            //Ensuring that modes disallowing tag matches are accounted for.
            if (selectedType.Equals("SWA") || selectedType.Equals("S - 1"))
            {
                sr_addSecond.Checked = false;
                sr_addSecond.Visible = false;
                sr_addWrestler.Checked = true;
                sr_tag.Visible = false;
                sr_second.Items.Clear();
                sr_single.Checked = true;
            }
            else
            {
                sr_addSecond.Visible = true;
                sr_tag.Visible = true;
            }
        }
        private void sr_single_CheckedChanged(object sender, EventArgs e)
        {
            if (sr_single.Checked)
            {
                sr_controlBoth.Checked = false;
                sr_controlBoth.Visible = false;
                sr_cutplay.Visible = false;
                sr_cutplay.Checked = false;
            }
        }
        private void sr_tag_CheckedChanged(object sender, EventArgs e)
        {
            if (sr_tag.Checked && sr_matchType.SelectedIndex == 0)
            {
                sr_controlBoth.Visible = true;
                sr_cutplay.Visible = true;
            }
        }
        private void sr_resetMatches_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            sr_matches.SelectedIndex = sr_matches.Items.Count - 1;
        }
        private void sr_resetContinues_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            sr_continues.SelectedIndex = sr_continues.Items.Count - 1;
        }
        private void sr_simulate_CheckedChanged(object sender, EventArgs e)
        {
            if (sr_simulate.Checked)
            {
                sr_simSecond.Visible = true;
            }
            else
            {
                sr_simSecond.Visible = false;
                sr_simSecond.Checked = false;
            }
        }
        private void formClose_Click(object sender, EventArgs e)
        {
            SaveSurvivalRoadData(MoreMatchTypes_Form.SurvivalRoadData.InProgress);
            this.Hide();
        }
        private void sr_progressRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MoreMatchTypes_Form.SurvivalRoadData.InProgress)
            {
                PopulateProgressBox(MoreMatchTypes_Form.SurvivalRoadData.MatchProgress);
            }
        }
        #endregion

        #region Begin Match
        private void sr_start_Click(object sender, EventArgs e)
        {
            if (!ValidateMatch())
            {
                return;
            }

            if (MoreMatchTypes_Form.SurvivalRoadData.InProgress)
            {
                ShowError("Please end the current Survival Road session first.");
            }

            #region Variables
            MatchSetting settings = GlobalWork.GetInst().MatchSetting;
            MatchConfig.WresIDGroup player = (MatchConfig.WresIDGroup)sr_wrestler.SelectedItem;
            MatchConfig.WresIDGroup second = (MatchConfig.WresIDGroup)sr_second.SelectedItem;
            String selectedType = sr_matchType.SelectedItem.ToString();
            WrestlerID wrestlerNo = WrestlerID.AbbieJones;
            bool isSecond = true;
            bool controlBoth = false;
            bool validEntry = false;
            int control = 0;
            int oppCount = sr_teamList.Items.Count;
            #endregion

            try
            {
                settings = SetMatchConfig("Survival", settings);
                sr_progress.Clear();
                //Set-up player team
                for (int i = 0; i < 4; i++)
                {
                    //Handling the first player
                    if (i == 0)
                    {
                        wrestlerNo = (WrestlerID)player.ID;
                        isSecond = false;
                        control = 1;
                        validEntry = true;
                    }
                    else if (i == 1)
                    {
                        if (second != null)
                        {

                            validEntry = true;
                            wrestlerNo = (WrestlerID)second.ID;
                            if (sr_tag.Checked)
                            {
                                L.D("Setting tag options");
                                isSecond = false;
                                if (sr_controlBoth.Checked)
                                {
                                    control = 1;
                                }
                                else
                                {
                                    control = 0;
                                }
                            }
                            else if (!sr_tag.Checked)
                            {
                                isSecond = true;
                            }

                            //Ensure we handle cases where a second isn't allowed, and a tag match hasn't been selected.
                            if (!sr_tag.Checked && (!selectedType.Equals("Normal")))
                            {
                                validEntry = false;
                            }
                        }
                        else
                        {
                            validEntry = false;
                        }

                    }
                    else if (i > 1)
                    {
                        validEntry = false;
                    }

                    //Determine if this is a simulation
                    if (sr_simulate.Checked)
                    {
                        control = 0;
                        if (sr_simSecond.Checked && i == 1 && selectedType.Equals("Normal"))
                        {
                            control = 1;
                        }
                    }

                    settings = MatchConfiguration.AddPlayers(validEntry, wrestlerNo, i, control, isSecond, 0, settings);

                }

                //Set-up opponents
                MatchConfig.WresIDGroup opponent;
                int opponentCount = sr_teamList.Items.Count;
                for (int i = 4; i < 8; i++)
                {
                    if (i > 5)
                    {
                        validEntry = false;
                    }
                    else
                    {
                        validEntry = true;
                        int searchIndex;
                        //Get Random opponent
                        if (sr_random.Checked)
                        {
                            searchIndex = UnityEngine.Random.Range(0, opponentCount);
                        }
                        else
                        {
                            searchIndex = 0;
                        }

                        //Determine if we're creating a tag team or single competitor
                        if (opponentCount == 1)
                        {
                            opponent = (MatchConfig.WresIDGroup)sr_teamList.Items[searchIndex];
                            wrestlerNo = MatchConfiguration.GetWrestlerNo(opponent);
                            initialOpponents[0] = opponent;
                        }
                        else if (i == 4)
                        {
                            opponent = (MatchConfig.WresIDGroup)sr_teamList.Items[searchIndex];
                            wrestlerNo = MatchConfiguration.GetWrestlerNo(opponent);
                            initialOpponents[0] = opponent;
                        }
                        else if (sr_tag.Checked && i == 5)
                        {
                            L.D("Creating tag opponents");
                            opponent = (MatchConfig.WresIDGroup)sr_teamList.Items[searchIndex];
                            wrestlerNo = MatchConfiguration.GetWrestlerNo(opponent);

                            //Ensure that we aren't fielding duplicate wrestlers
                            while (initialOpponents[0].ID == (int)wrestlerNo)
                            {
                                L.D("Preventing duplicates");
                                searchIndex = UnityEngine.Random.Range(0, opponentCount);
                                opponent = (MatchConfig.WresIDGroup)sr_teamList.Items[searchIndex];
                                wrestlerNo = MatchConfiguration.GetWrestlerNo(opponent);
                            }

                            initialOpponents[1] = opponent;
                        }
                        else if (!sr_tag.Checked && i == 5)
                        {
                            validEntry = false;
                        }
                    }

                    settings = MatchConfiguration.AddPlayers(validEntry, wrestlerNo, i, 0, false, 0, settings);
                }

                StartMatch();
            }
            catch (Exception ex)
            {
                ShowError("An error has occured.");
                L.D("ValidateSurvivalMatchError: " + ex);
            }
        }

        #endregion

        #region Survival Road Controls
        //private void sr_continues_SelectedItemChanged(object sender, EventArgs e)
        //{

        //}

        //private void sr_matches_SelectedItemChanged(object sender, EventArgs e)
        //{

        //}
        #endregion

        #region Helper Methods
        private bool ValidateMatch()
        {
            bool isValid = true;

            if (MoreMatchTypes_Form.SurvivalRoadData.InProgress)
            {
                ShowError("Survival Road is currently in progress.");
                isValid = false;
            }
            if (!MoreMatchTypes_Form.moreMatchTypesForm.cb_survival.Checked)
            {
                ShowError("The Survival Road option must be selected.");
                isValid = false;
            }
            if (sr_wrestler.Items.Count == 0)
            {
                ShowError("A wrestler must be selected in order to play.");
                isValid = false;
            }
            if (sr_teamList.Items.Count == 0)
            {
                ShowError("At least one opponent must be selected in order to play single matches.");
                isValid = false;
            }
            if (sr_teamList.Items.Count < 2 && sr_tag.Checked)
            {
                ShowError("At least two opponents must be selected in order to play tag team matches.");
                isValid = false;
            }

            return isValid;
        }
        private void ShowError(String message)
        {
            ModPackDialog.Show(message, MessageBoxButtons.OK);
            //MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private int FindGroup(String groupName)
        {
            return promotionList.IndexOf(groupName);
        }
        private void LoadEditsFromPromotion()
        {
            String query = "";
            ComboBox resultField = null;
            TextBox searchField = null;
            ComboBox promotionField = null;

            resultField = sr_searchResult;
            searchField = sr_searchInput;
            promotionField = sr_promotionList;

            resultField.Items.Clear();

            //Find search terms
            query = searchField.Text;
            if (!query.TrimStart().TrimEnd().Equals(""))
            {
                foreach (MatchConfig.WresIDGroup wrestler in wrestlerList)
                {
                    if (query.ToLower().Equals(wrestler.Name.ToLower()) || wrestler.Name.ToLower().Contains(query.ToLower()))
                    {
                        resultField.Items.Add(wrestler);
                    }
                }
            }

            if (resultField.Items.Count > 0)
            {
                return;
            }
            foreach (MatchConfig.WresIDGroup current in wrestlerList)
            {
                try
                {
                    if (current.Group == FindGroup(promotionField.SelectedItem.ToString()))
                    {
                        resultField.Items.Add(current);
                    }
                }
                catch (Exception ex)
                {
                    L.D("Error: " + ex.Message);
                }
            }
        }
        private MatchSetting SetMatchConfig(String matchType, MatchSetting settings)
        {
            ComboBox ringList = null;
            ComboBox refereeList = null;
            ComboBox arenaList = null;
            ComboBox difficultyList = null;
            ComboBox speedList = null;
            ComboBox bgmList = null;

            //Setting controls
            ringList = this.sr_ringList;
            refereeList = this.sr_refereeList;
            arenaList = this.sr_venueList;
            difficultyList = this.sr_difficultyList;
            speedList = this.sr_speedList;
            bgmList = this.sr_bgmList;

            #region Match Setting Config
            GlobalParam.Erase_WrestlerResource();
            GlobalParam.Delete_BattleConfig();
            GlobalParam.Intalize_BattleMode();
            GlobalParam.Intalize_BattleConfig();
            GlobalParam.Load_ConfigData();
            GlobalParam.Set_BattleConfig_Value((GlobalParam.CFG_VAL)26, 0);
            GlobalParam.Set_MatchSetting_DefaultParam();

            GlobalParam.TitleMatch_BeltData = null;
            GlobalParam.m_BattleMode = GlobalParam.BattleMode.OneNightMatch;
            GlobalParam.flg_TitleMatch_Ready = false;
            GlobalParam.Set_MatchSetting_Wrestler(false);
            GlobalParam.Set_MatchSetting_Rule();
            GlobalParam.Init_WrestlerData();
            GlobalParam.Intalize_BattleMode();
            GlobalParam.Intalize_BattleConfig();

            //Set Ring
            try
            {
                RingInfo ring = (RingInfo)ringList.SelectedItem;

                switch (ring.SaveID)
                {
                    case -1:
                        settings.ringID = RingID.SWA;
                        break;
                    case -2:
                        settings.ringID = RingID.NJPW;
                        break;
                    case -3:
                        settings.ringID = RingID.Stardom;
                        break;
                    case -4:
                        settings.ringID = RingID.Takayamania;
                        break;
                    case -5:
                        settings.ringID = RingID.TakayamaniaEmpire;
                        break;
                    default:
                        settings.ringID = (RingID)ring.SaveID;
                        break;
                }
            }
            catch
            {
                L.D("Error Setting Ring");
                settings.ringID = RingID.SWA;
            }

            //Set Referee
            try
            {
                RefereeInfo referee = (RefereeInfo)refereeList.SelectedItem;
                switch (referee.SaveID)
                {
                    case -1:
                        settings.RefereeID = RefereeID.MrJudgement;
                        break;
                    case -2:
                        settings.RefereeID = RefereeID.RedShoesUnno;
                        break;
                    case -3:
                    default:
                        settings.RefereeID = (RefereeID)referee.SaveID;
                        break;

                }
            }
            catch
            {
                L.D("Error Setting Referee");
                settings.RefereeID = RefereeID.MrJudgement;
            }

            //Set Venue
            try
            {
                String venue = (String)arenaList.SelectedItem;
                switch (venue)
                {
                    case "Big Garden Arena":
                        settings.arena = VenueEnum.BigGardenArena;
                        break;
                    case "SCS Stadium":
                        settings.arena = VenueEnum.SCSStadium;
                        break;
                    case "Arena De Universo":
                        settings.arena = VenueEnum.ArenaDeUniverso;
                        break;
                    case "Spike Dome":
                        settings.arena = VenueEnum.SpikeDome;
                        break;
                    case "Yurakuen Hall":
                        settings.arena = VenueEnum.YurakuenHall;
                        break;
                    case "Dojo":
                        settings.arena = VenueEnum.Dojo;
                        break;
                    case "Takafumi City Gym":
                        settings.arena = VenueEnum.TakafumiCityGym;
                        break;
                    case "Sakae Outdoor Ring":
                        settings.arena = VenueEnum.SakaeOutdoorRing;
                        break;
                    case "USA Grand Dome":
                        settings.arena = VenueEnum.USAGrandDome;
                        break;
                    default:
                        settings.arena = VenueEnum.BigGardenArena;
                        break;
                }
            }
            catch
            {
                L.D("Error setting venue");
                settings.arena = VenueEnum.BigGardenArena;
            }

            //Set Game Speed
            try
            {
                settings.GameSpeed = (uint)speedList.SelectedItem;
            }
            catch
            {
                L.D("Error setting game speed");
                settings.GameSpeed = 100;
            }

            settings.BattleRoyalKind = BattleRoyalKindEnum.Off;
            settings.VictoryCondition = VictoryConditionEnum.Count3;
            settings.isOverTheTopRopeOn = false;
            settings.MatchTime = 0;
            settings.is3GameMatch = true;
            settings.isRopeCheck = true;
            settings.isElimination = false;
            settings.isLumberjack = false;
            settings.isTornadoBattle = false;
            settings.isCutPlay = false;
            settings.isDisableTimeCount = false;
            settings.isOutOfRingCount = true;
            settings.entranceSceneMode = EntranceSceneMode.EachCorner;

            settings.ComLevel = difficultyList.SelectedIndex;

            //Setting Custom Rules

            String type = sr_matchType.SelectedItem.ToString();
            if (type.Equals("Normal"))
            {
                settings.isFoulCount = true;
                settings.isCutPlay = sr_cutplay.Checked;
                settings.CriticalRate = CriticalRateEnum.Half;
            }
            if (type.Equals("Cage"))
            {
                settings.isFoulCount = false;
                settings.CriticalRate = CriticalRateEnum.Half;
                settings.arena = VenueEnum.Cage;
                settings.isTornadoBattle = true;
            }
            if (type.Equals("Barbed Wire"))
            {
                settings.isFoulCount = false;
                settings.CriticalRate = CriticalRateEnum.Normal;
                settings.arena = VenueEnum.BarbedWire;
                settings.isTornadoBattle = true;
            }
            if (type.Equals("Landmine"))
            {
                settings.isFoulCount = false;
                settings.CriticalRate = CriticalRateEnum.Normal;
                settings.arena = VenueEnum.LandMine_BarbedWire;
                settings.isTornadoBattle = true;
            }
            if (type.Equals("SWA"))
            {
                settings.isFoulCount = false;
                settings.CriticalRate = CriticalRateEnum.Normal;
                settings.is10CountKO = true;
            }
            if (type.Equals("Gruesome"))
            {
                settings.isFoulCount = false;
                settings.CriticalRate = CriticalRateEnum.Double;
                settings.arena = VenueEnum.Dodecagon;
                settings.isTornadoBattle = true;
            }
            if (type.Equals("S - 1"))
            {
                settings.isFoulCount = true;
                settings.CriticalRate = CriticalRateEnum.Double;
                settings.is10CountKO = true;
                settings.isS1Rule = true;
            }

            //Need to set a valid MatchBGM type  here, then override it on match start if necessary.
            try
            {
                if (sr_bgmList.SelectedIndex == 0)
                //{
                //    settings.matchBGM = MatchBGM.FireProWrestling;
                //}
                //else if (sr_bgmList.SelectedIndex == 1)
                //{
                //    settings.matchBGM = MatchBGM.SpinningPanther;
                //}
                //else if (sr_bgmList.SelectedIndex == 2)
                //{
                //    settings.matchBGM = MatchBGM.LonelyStage;
                //}
                //else
                //{
                //    settings.matchBGM = (MatchBGM)GetValidBGMID((String)sr_bgmList.SelectedItem);
                //}

                settings.matchBGM = MatchBGM.FireProWrestling;
            }
            catch (Exception e)
            {
                L.D("Error setting Match BGM");
                settings.matchBGM = MatchBGM.FireProWrestling;
            }

            GlobalParam.flg_CallDebugMenu = false;
            GlobalParam.befor_scene = "Scene_BattleSetting";
            GlobalParam.keep_scene = "Scene_BattleSetting";
            GlobalParam.next_scene = "";
            #endregion

            return settings;
        }
        private void StartMatch()
        {
            SaveSurvivalRoadData(true);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Match");
        }
        private void SaveSurvivalRoadData(bool inProgress)
        {
            //Save current data
            if (!MoreMatchTypes_Form.SurvivalRoadData.InProgress)
            {
                try
                {
                    SurvivalRoadData survivalRoadData = new SurvivalRoadData
                    {
                        //Settings
                        Referee = (RefereeInfo)sr_refereeList.SelectedItem,
                        Venue = (String)sr_venueList.SelectedItem,
                        Ring = (RingInfo)sr_ringList.SelectedItem,
                        Speed = (uint)sr_speedList.SelectedItem,
                        MatchBGM = (String)sr_bgmList.SelectedItem,
                        Difficulty = (String)sr_difficultyList.SelectedItem,
                        InProgress = inProgress,

                        //Player Options
                        Wrestler = sr_wrestler.Items.Count > 0 ? (MatchConfig.WresIDGroup)sr_wrestler.Items[0] : null,
                        Second = sr_second.Items.Count > 0 ? (MatchConfig.WresIDGroup)sr_second.Items[0] : null,
                        Continues = (int)sr_continues.SelectedItem,
                        Matches = (int)sr_matches.SelectedItem,
                        RegainHP = (bool)sr_regenHP.Checked,
                        Singles = (bool)sr_single.Checked,
                        Tag = (bool)sr_tag.Checked,
                        ControlBoth = (bool)sr_controlBoth.Checked,
                        CutPlay = (bool)sr_cutplay.Checked,
                        MatchType = (String)sr_matchType.SelectedItem,
                        Simulate = (bool)sr_simulate.Checked,
                        ControlSecond = (bool)sr_simSecond.Checked,
                        MatchProgress = new List<String>(),

                        //Opposing Team
                        OpponentName = sr_teamName.Text,
                        RandomSelect = sr_random.Checked,
                        Opponents = new List<MatchConfig.WresIDGroup>(),
                        InitialOpponents = initialOpponents
                    };

                    foreach (MatchConfig.WresIDGroup wrestler in sr_teamList.Items)
                    {
                        survivalRoadData.Opponents.Add(wrestler);
                    }

                    MoreMatchTypes_Form.SurvivalRoadData = survivalRoadData;
                }
                catch (Exception exception)
                {
                    L.D("SaveSurvivalDataException: " + exception);
                }
            }
        }
        private void PopulateProgressBox(List<String> progress)
        {
            sr_progress.Clear();
            foreach (String info in progress)
            {
                sr_progress.Text += info;
            }
        }

        private int GetValidBGMID(String name)
        {
            L.D("GetValidBGMID: " + name);
            for (int i = 0; i < MyMusic.FileList_Match.Count; i++)
            {
                if (MyMusic.FileList_Match[i].name.Equals(name))
                {
                    return i;
                }
            }

            return 0;
        }

        #endregion

        private void btn_import_Click(object sender, EventArgs e)
        {

        }

        private void btn_export_Click(object sender, EventArgs e)
        {

        }
    }
}
