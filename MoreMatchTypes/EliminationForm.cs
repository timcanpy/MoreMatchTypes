using DG;
using MatchConfig;
using ModPack;
using MoreMatchTypes.Data_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MoreMatchTypes
{
    public partial class EliminationForm : Form
    {
        #region Variables
        public static EliminationForm eliminationForm = null;
        public static List<String> promotionList = new List<string>();
        public static List<MatchConfig.WresIDGroup> wrestlerList = new List<MatchConfig.WresIDGroup>();
        #endregion

        #region Initialization Methods
        public EliminationForm()
        {
            InitializeComponent();
        }
        private void EliminationForm_Load(object sender, EventArgs e)
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
                LoadSettings();
            }
            catch (Exception ex)
            {
                L.D("EliminationForm_LoadError: " + ex);
            }
        }
        private void LoadPromotions()
        {
            promotionList.Clear();
            el_promotionList.Items.Clear();

            foreach (String promotion in MatchConfiguration.LoadPromotions())
            {
                el_promotionList.Items.Add(promotion);
                promotionList.Add(promotion);
            }

            if (el_promotionList.Items.Count > 0)
            {
                el_promotionList.SelectedIndex = 0;
            }
        }
        private void LoadEdits()
        {
            wrestlerList.Clear();
            el_resultList.Items.Clear();

            wrestlerList = MatchConfiguration.LoadWrestlers();

            foreach (MatchConfig.WresIDGroup wrestler in wrestlerList)
            {
                el_resultList.Items.Add(wrestler);
            }

            if (el_resultList.Items.Count > 0)
            {
                el_resultList.SelectedIndex = 0;
            }
        }
        private void LoadRings()
        {
            foreach (var ring in MatchConfiguration.LoadRings())
            {
                el_ringList.Items.Add(ring);
            }

            if (el_ringList.Items.Count > 0)
            {
                el_ringList.SelectedIndex = 0;
            }
        }
        private void LoadReferees()
        {
            foreach (RefereeInfo referee in MatchConfiguration.LoadReferees())
            {
                el_refereeList.Items.Add(referee);
            }

            if (el_refereeList.Items.Count > 0)
            {
                el_refereeList.SelectedIndex = 0;
            }
        }
        private void LoadThemes()
        {
            foreach (String theme in MatchConfiguration.LoadBGMs())
            {
                el_bgm.Items.Add(theme);
            }

            if (el_bgm.Items.Count > 0)
            {
                el_bgm.SelectedIndex = 0;
            }
        }
        private void LoadDifficulty()
        {
            foreach (String i in MatchConfiguration.LoadDifficulty())
            {
                el_difficulty.Items.Add(i);
            }

            if (el_difficulty.Items.Count > 0)
            {
                el_difficulty.SelectedIndex = 0;
            }
        }
        private void LoadGameSpeed()
        {
            foreach (uint speed in MatchConfiguration.LoadSpeed())
            {
                el_gameSpeed.Items.Add(speed);
            }

            if (el_gameSpeed.Items.Count > 0)
            {
                el_gameSpeed.SelectedIndex = 0;
            }
        }
        private void LoadVenues()
        {
            String[] venues = MatchConfiguration.LoadVenue();
            foreach (String venue in venues)
            {        
                el_venueList.Items.Add(venue);
            }

            if (el_venueList.Items.Count > 0)
            {
                el_venueList.SelectedIndex = 0;
            }
        }

        #endregion

        #region Team Management
        private void el_addBtn_Click(object sender, EventArgs e)
        {
            if (el_resultList.SelectedItem == null)
            {
                return;
            }

            MatchConfig.WresIDGroup wrestler = (MatchConfig.WresIDGroup)el_resultList.SelectedItem;

            if (rb_singleBlue.Checked)
            {
                if (!MatchConfiguration.ValidateWrestler((WrestlerID)wrestler.ID))
                {
                    ShowError(wrestler.Name + " includes DLC that you do not own.");
                    return;
                }

                if (!CheckDuplicates((MatchConfig.WresIDGroup)el_resultList.SelectedItem, SideCornerPostEnum.Left))
                {
                    el_blueList.Items.Add(el_resultList.SelectedItem);
                }
            }
            else if (rb_singleRed.Checked)
            {
                if (!MatchConfiguration.ValidateWrestler((WrestlerID)wrestler.ID))
                {
                    ShowError(wrestler.Name + " includes DLC that you do not own.");
                    return;
                }

                if (!CheckDuplicates((MatchConfig.WresIDGroup)el_resultList.SelectedItem, SideCornerPostEnum.Right))
                {
                    el_redList.Items.Add(el_resultList.SelectedItem);
                }
            }
            else if (rb_allBlue.Checked)
            {
                int invalidOptions = 0;
                for (int i = 0; i < el_resultList.Items.Count; i++)
                {
                    MatchConfig.WresIDGroup item = (MatchConfig.WresIDGroup)el_resultList.Items[i];
                    if (!MatchConfiguration.ValidateWrestler((WrestlerID)item.ID))
                    {
                        invalidOptions++;
                        continue;
                    }
                    if (!CheckDuplicates(item, SideCornerPostEnum.Left))
                    {
                        el_blueList.Items.Add(item);
                    }
                }

                if (invalidOptions > 0)
                {
                    ShowError(invalidOptions + " wrestlers include DLC that you do not own, and were skipped.");
                }
            }
            else if (rb_allRed.Checked)
            {
                int invalidOptions = 0;
                for (int i = 0; i < el_resultList.Items.Count; i++)
                {
                    MatchConfig.WresIDGroup item = (MatchConfig.WresIDGroup)el_resultList.Items[i];
                    if (!MatchConfiguration.ValidateWrestler((WrestlerID)item.ID))
                    {
                        invalidOptions++;
                        continue;
                    }
                    if (!CheckDuplicates(item, SideCornerPostEnum.Right))
                    {
                        el_redList.Items.Add(item);
                    }
                }

                if (invalidOptions > 0)
                {
                    ShowError(invalidOptions + " wrestlers include DLC that you do not own, and were skipped.");
                }
            }

            //Generate default team name
            List<String> wrestlers = new List<String>();

            if (rb_singleBlue.Checked || rb_allBlue.Checked)
            {
                if (el_blueTeamName.Text.Trim().Equals(String.Empty))
                {
                    foreach (MatchConfig.WresIDGroup item in el_blueList.Items)
                    {
                        wrestlers.Add(item.Name);
                    }

                    el_blueTeamName.Text = MatchConfiguration.GetTeamName(wrestlers, SideCornerPostEnum.Left);
                }
            }

            if (rb_singleRed.Checked || rb_allRed.Checked)
            {
                if (el_redTeamName.Text.Trim().Equals(String.Empty))
                {
                    foreach (MatchConfig.WresIDGroup item in el_redList.Items)
                    {
                        wrestlers.Add(item.Name);
                    }

                    el_redTeamName.Text = MatchConfiguration.GetTeamName(wrestlers, SideCornerPostEnum.Right);
                }
            }


        }
        private void el_removeOneBlue_Click(object sender, EventArgs e)
        {
            el_blueList.Items.Remove(el_blueList.SelectedItem);
        }
        private void el_removeAllBlue_Click(object sender, EventArgs e)
        {
            el_blueList.Items.Clear();
        }
        private void el_removeOneRed_Click(object sender, EventArgs e)
        {
            el_redList.Items.Remove(el_redList.SelectedItem);
        }
        private void el_removeAllRed_Click(object sender, EventArgs e)
        {
            el_redList.Items.Clear();
        }
        private void el_searchBtn_Click(object sender, EventArgs e)
        {
            LoadEditsFromPromotion();
            if (el_resultList.Items.Count > 0)
            {
                el_resultList.SelectedIndex = 0;
            }
        }
        private void el_promotionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            el_searchBtn_Click(sender, e);
        }
        private void el_refresh_Click(object sender, EventArgs e)
        {
            LoadPromotions();
            LoadEdits();
            LoadRings();
            LoadReferees();
        }
        #endregion

        #region Begin Match
        private void btn_matchStart_Click(object sender, EventArgs e)
        {
            if (!ValidateMatch())
            {
                return;
            }

            if (MoreMatchTypes_Form.ExEliminationData.InProgress)
            {
                ShowError("Please end the current Extended Elimination Match first.");
                return;
            }
            
            #region Variables
            MatchSetting settings = GlobalWork.GetInst().MatchSetting;
            WrestlerID wrestlerNo = WrestlerID.AbbieJones;
            bool isSecond = true;
            bool controlBoth = false;
            bool validEntry = false;
            int control = 0;
            #endregion

            //Adding initial teams
            try
            {
                settings = SetMatchConfig(settings);

                //Set-up Blue team
                for (int i = 0; i < 4; i++)
                {
                    if (i >= el_blueList.Items.Count)
                    {
                        break;
                    }

                    //Prepare wrestler
                    wrestlerNo = (WrestlerID)((MatchConfig.WresIDGroup)el_blueList.Items[i]).ID;
                    validEntry = true;
                    if (i == 0)
                    {
                        isSecond = false;

                        if (el_blueControl.Checked)
                        {
                            control = 1;
                        }
                        else
                        {
                            control = 0;
                        }
                    }
                    else
                    {
                        isSecond = true;
                    }

                    settings = MatchConfiguration.AddPlayers(validEntry, wrestlerNo, i, control, isSecond, 0, settings);
                }

                //Set-up Red team
                for (int i = 0; i < 4; i++)
                {
                    if (i >= el_redList.Items.Count)
                    {
                        break;
                    }

                    //Prepare wrestler
                    wrestlerNo = (WrestlerID)((MatchConfig.WresIDGroup)el_redList.Items[i]).ID;
                    validEntry = true;
                    if (i == 4)
                    {
                        isSecond = false;

                        if (el_redControl.Checked)
                        {
                            control = 1;
                        }
                        else
                        {
                            control = 0;
                        }
                    }
                    else
                    {
                        isSecond = true;
                    }

                    settings = MatchConfiguration.AddPlayers(validEntry, wrestlerNo, i+4, control, isSecond, 0, settings);
                }

                StartMatch();
            }
            catch (Exception ex)
            {
                ShowError("An error has occured.");
                L.D("ValidateExElimMatch: " + ex);
            }
        }
        private MatchSetting SetMatchConfig(MatchSetting settings)
        {
            ComboBox ringList = null;
            ComboBox refereeList = null;
            ComboBox arenaList = null;
            ComboBox difficultyList = null;
            ComboBox speedList = null;
            ComboBox bgmList = null;
            
            #region Match Setting Config

            //Setting controls
            ringList = this.el_ringList;
            refereeList = this.el_refereeList;
            arenaList = this.el_venueList;
            difficultyList = this.el_difficulty;
            speedList = this.el_gameSpeed;
            bgmList = this.el_bgm;
            
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

            if (GlobalParam.BattleRoyal_EntryWrestler == null)
            {
                GlobalParam.BattleRoyal_Initalize();
            }

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

            //Setting Match Rules
            settings.BattleRoyalKind = BattleRoyalKindEnum.Off;
            settings.VictoryCondition = VictoryConditionEnum.Count3;
            settings.MatchTime = 0;
            settings.ComLevel = difficultyList.SelectedIndex;
            settings.CriticalRate = CriticalRateEnum.Off;
            settings.isRopeCheck = true;
            settings.isElimination = false;
            settings.isLumberjack = false;
            settings.intrusionRate[0] = IntrusionRate.None;
            settings.intrusionRate[1] = IntrusionRate.None;
            settings.isTornadoBattle = false;
            settings.isCutPlay = false;
            settings.isDisableTimeCount = false;
            settings.isOutOfRingCount = true;
            settings.isOverTheTopRopeOn = false;
            settings.is3GameMatch = false;
            settings.isFoulCount = true;
            //settings.entranceSceneMode = EntranceSceneMode.EachWrestler;
            settings.isSkipEntranceScene = true;

            //Need to set a valid MatchBGM type  here, then override it on match start if necessary.
            if (el_bgm.SelectedIndex <= 2)
            {
                settings.matchBGM = MatchBGM.SpinningPanther;
            }
            else
            {
                settings.matchBGM = (MatchBGM)GetValidBGMID((String)el_bgm.SelectedItem);
            }

            GlobalParam.flg_CallDebugMenu = false;
            GlobalParam.befor_scene = "Scene_BattleSetting";
            GlobalParam.keep_scene = "Scene_BattleSetting";
            GlobalParam.next_scene = "";
            #endregion

            return settings;
        }

        #endregion

        #region Helper Methods
        private bool ValidateMatch()
        {
            bool isValid = true;

            if (!MoreMatchTypes_Form.moreMatchTypesForm.cb_exElim.Checked)
            {
                ShowError("The Extended Elimination option must be selected.");
                isValid = false;
            }

            if (el_blueList.Items.Count == 0 || el_redList.Items.Count == 0)
            {
                ShowError("Both teams must contain at least one member.");
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

            resultField = el_resultList;
            searchField = el_searchInput;
            promotionField = el_promotionList;

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
        private void StartMatch()
        {
            SaveExEliminationData(true);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Match");
            this.Hide();
        }
        #endregion

        private void formClose_Click(object sender, EventArgs e)
        {
            SaveExEliminationData(MoreMatchTypes_Form.ExEliminationData.InProgress);
            this.Hide();
        }

        private void SaveExEliminationData(bool inProgress)
        {
            if (!MoreMatchTypes_Form.ExEliminationData.InProgress)
            {
                ExEliminationData data = new ExEliminationData
                {
                    //Settings
                    Referee = (RefereeInfo)el_refereeList.SelectedItem,
                    Venue = (String)el_venueList.SelectedItem,
                    Ring = (RingInfo)el_ringList.SelectedItem,
                    Speed = (uint)el_gameSpeed.SelectedItem,
                    MatchBGM = (String)el_bgm.SelectedItem,
                    Difficulty = (String)el_difficulty.SelectedItem,
                    InProgress = inProgress,

                    //Player Options
                    ControlBlue = el_blueControl.Checked,
                    ControlRed = el_redControl.Checked,
                    TeamNames = new String[] { el_blueTeamName.Text, el_redTeamName.Text }
                };

                //Saving team members
                List<MatchConfig.WresIDGroup> members = new List<MatchConfig.WresIDGroup>();
                foreach (MatchConfig.WresIDGroup wrestler in el_blueList.Items)
                {
                    members.Add(wrestler);
                }
                data.BlueTeamMembers = members;

                members = new List<MatchConfig.WresIDGroup>();

                foreach (MatchConfig.WresIDGroup wrestler in el_redList.Items)
                {
                    members.Add(wrestler);
                }
                data.RedTeamMembers = members;

                MoreMatchTypes_Form.ExEliminationData = data;
            }
        }

        private void LoadSettings()
        {
            ExEliminationData data = MoreMatchTypes_Form.ExEliminationData;
            if (data == null)
            {
                MoreMatchTypes_Form.ExEliminationData = new ExEliminationData();
            }
            else
            {
                //Referee
                try
                {
                    el_refereeList.SelectedIndex =
                        data.Referee == null ? 0 : el_refereeList.FindString(data.Referee.Name);
                }
                catch
                {
                    el_refereeList.SelectedIndex = 0;
                }

                //Venue
                try
                {
                    el_venueList.SelectedIndex =
                        data.Venue == null ? 0 : el_venueList.FindString(data.Venue);
                }
                catch
                {
                    el_venueList.SelectedIndex = 0;
                }
              
                //Ring
                try
                {
                    el_ringList.SelectedIndex =
                        data.Ring == null ? 0 : el_ringList.FindString(data.Ring.Name);
                }
                catch
                {
                    el_ringList.SelectedIndex = 0;
                }
                
                //BGM
                try
                {
                    el_bgm.SelectedItem = data.MatchBGM;
                }
                catch
                {
                    el_bgm.SelectedIndex = 0;
                }

                el_gameSpeed.SelectedItem = data.Speed;
                el_difficulty.SelectedItem = data.Difficulty;

                el_blueControl.Checked = data.ControlBlue;
                el_redControl.Checked = data.ControlRed;
                el_blueTeamName.Text = data.TeamNames[0];
                el_redTeamName.Text = data.TeamNames[1];


                foreach (MatchConfig.WresIDGroup wrestler in data.BlueTeamMembers)
                {
                    el_blueList.Items.Add(wrestler);
                }

                foreach (MatchConfig.WresIDGroup wrestler in data.RedTeamMembers)
                {
                    el_redList.Items.Add(wrestler);
                }
            }
        }

        private bool CheckDuplicates(MatchConfig.WresIDGroup wrestler, SideCornerPostEnum side)
        {
            bool isDuplicate = false;

            if (side == SideCornerPostEnum.Left)
            {
                foreach (MatchConfig.WresIDGroup item in el_blueList.Items)
                {
                    if (wrestler == item)
                    {
                        isDuplicate = true;
                        break;
                    }
                }
            }
            else
            {
                foreach (MatchConfig.WresIDGroup item in el_redList.Items)
                {
                    if (wrestler == item)
                    {
                        isDuplicate = true;
                        break;
                    }
                }
            }

            return isDuplicate;
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
    }
}
