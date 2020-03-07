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

namespace MoreMatchTypes
{
    public partial class EliminationForm : Form
    {
        #region Variables
        public static EliminationForm eliminationForm = null;
        public static List<String> promotionList = new List<string>();
        public static List<WresIDGroup> wrestlerList = new List<WresIDGroup>();
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

            foreach (WresIDGroup wrestler in wrestlerList)
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
            if (rb_singleBlue.Checked)
            {
                if (!CheckDuplicates((WresIDGroup)el_resultList.SelectedItem, SideCornerPostEnum.Left))
                {
                    el_blueList.Items.Add(el_resultList.SelectedItem);
                }
            }
            else if (rb_singleRed.Checked)
            {
                if (!CheckDuplicates((WresIDGroup)el_resultList.SelectedItem, SideCornerPostEnum.Right))
                {
                    el_redList.Items.Add(el_resultList.SelectedItem);
                }
            }
            else if (rb_allBlue.Checked)
            {
                for (int i = 0; i < el_resultList.Items.Count; i++)
                {
                    if (!CheckDuplicates((WresIDGroup)el_resultList.Items[i], SideCornerPostEnum.Left))
                    {
                        el_blueList.Items.Add(el_resultList.Items[i]);
                    }
                }
            }
            else if (rb_allRed.Checked)
            {
                for (int i = 0; i < el_resultList.Items.Count; i++)
                {
                    if (!CheckDuplicates((WresIDGroup)el_resultList.Items[i], SideCornerPostEnum.Right))
                    {
                        el_redList.Items.Add(el_resultList.Items[i]);
                    }
                }
            }

            //Generate default team name
            List<String> wrestlers = new List<String>();

            if (rb_singleBlue.Checked || rb_allBlue.Checked)
            {
                if (el_blueTeamName.Text.Trim().Equals(String.Empty))
                {
                    foreach (WresIDGroup item in el_blueList.Items)
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
                    foreach (WresIDGroup item in el_redList.Items)
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
            }

            #region Variables
            int blueTeamCount = el_blueList.Items.Count;
            int redTeamCount = el_redList.Items.Count;
            bool isSecond;
            MatchSetting settings = GlobalWork.GetInst().MatchSetting;
            #endregion

            try
            {
                settings = SetMatchConfig(settings);

                #region Create Wrestlers
                //Set the initial blue team members
                int searchCount;
                if (blueTeamCount > 3)
                {
                    searchCount = 4;
                }
                else
                {
                    searchCount = blueTeamCount;
                }

                for (int i = 0; i < searchCount; i++)
                {
                    WrestlerID wrestlerNo = MatchConfiguration.GetWrestlerNo((WresIDGroup)el_blueList.Items[i]);
                    if (i == 0)
                    {
                        isSecond = false;
                    }
                    else
                    {
                        isSecond = true;
                    }
                    settings = MatchConfiguration.AddPlayers(true, wrestlerNo, i, 0, isSecond, 0, settings);
                }

                //Set the initial red team members
                if (redTeamCount > 3)
                {
                    searchCount = 4;
                }
                else
                {
                    searchCount = redTeamCount;
                }

                for (int i = 0; i < searchCount; i++)
                {
                    WrestlerID wrestlerNo = MatchConfiguration.GetWrestlerNo((WresIDGroup)el_redList.Items[i]);
                    if (i == 0)
                    {
                        isSecond = false;
                    }
                    else
                    {
                        isSecond = true;
                    }
                    settings = MatchConfiguration.AddPlayers(true, wrestlerNo, i + 4, 0, isSecond, 0, settings);
                }
                #endregion

                StartMatch();
                btn_matchStart.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btn_matchStart.Enabled = true;
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

            //Setting controls
            ringList = this.el_ringList;
            refereeList = this.el_refereeList;
            arenaList = this.el_venueList;
            difficultyList = this.el_difficulty;
            speedList = this.el_gameSpeed;
            bgmList = this.el_bgm;

            #region Match Setting Config
            GlobalParam.Delete_BattleConfig();
            GlobalParam.Load_ConfigData();
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

                if (ring.SaveID == -1)
                {
                    settings.ringID = RingID.SWA;
                }
                else
                {
                    settings.ringID = (RingID)ring.SaveID;
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
                settings.RefereeID = (RefereeID)referee.SaveID;
                if (settings.RefereeID == RefereeID.Invalid)
                {
                    settings.RefereeID = RefereeID.MrJudgement;
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
            settings.isTornadoBattle = false;
            settings.isCutPlay = false;
            settings.isDisableTimeCount = false;
            settings.isOutOfRingCount = true;
            settings.isOverTheTopRopeOn = false;
            settings.is3GameMatch = false;
            settings.isFoulCount = true;

            //Need to set a valid MatchBGM type  here, then override it on match start if necessary.
            if (el_bgm.SelectedIndex > 2)
            {
                settings.matchBGM = MatchBGM.SpinningPanther;
            }
            else
            {
                settings.matchBGM = (MatchBGM)el_bgm.SelectedIndex;
            }

            //settings.isSkipEntranceScene = true;
            settings.entranceSceneMode = EntranceSceneMode.EachCorner;
            //settings.isPlayDemo = false;
            //GlobalParam.flg_CallDebugMenu = false;
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
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                foreach (WresIDGroup wrestler in wrestlerList)
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
            foreach (WresIDGroup current in wrestlerList)
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
            UnityEngine.SceneManagement.SceneManager.LoadScene("Match");
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
                List<WresIDGroup> members = new List<WresIDGroup>();
                foreach (WresIDGroup wrestler in el_blueList.Items)
                {
                    members.Add(wrestler);
                }
                data.BlueTeamMembers = members;

                members = new List<WresIDGroup>();

                foreach (WresIDGroup wrestler in el_redList.Items)
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
                el_refereeList.SelectedIndex =
                    data.Referee == null ? 0 : el_refereeList.FindString(data.Referee.Name);

                el_venueList.SelectedIndex =
                    data.Venue == null ? 0 : el_venueList.FindString(data.Venue);

                el_ringList.SelectedIndex =
                    data.Ring == null ? 0 : el_ringList.FindString(data.Ring.Name);

                el_gameSpeed.SelectedItem = data.Speed;
                el_bgm.SelectedItem = data.MatchBGM;
                el_difficulty.SelectedItem = data.Difficulty;

                el_blueControl.Checked = data.ControlBlue;
                el_redControl.Checked = data.ControlRed;
                el_blueTeamName.Text = data.TeamNames[0];
                el_redTeamName.Text = data.TeamNames[1];


                foreach (WresIDGroup wrestler in data.BlueTeamMembers)
                {
                    el_blueList.Items.Add(wrestler);
                }

                foreach (WresIDGroup wrestler in data.RedTeamMembers)
                {
                    el_redList.Items.Add(wrestler);
                }
            }
        }

        private bool CheckDuplicates(WresIDGroup wrestler, SideCornerPostEnum side)
        {
            bool isDuplicate = false;

            if (side == SideCornerPostEnum.Left)
            {
                foreach (WresIDGroup item in el_blueList.Items)
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
                foreach (WresIDGroup item in el_redList.Items)
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
    }
}
