using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DG;
using DG.DG;
using System.Diagnostics;
using System.IO;
using MoreMatchTypes.Helper_Classes;

namespace MoreMatchTypes
{
    public partial class MoreMatchTypes_Form : Form
    {
        #region Variables
        public static MoreMatchTypes_Form form = null;
        public static List<uint> gameSpeed = new List<uint>();
        public static List<String> venues = new List<String>();
        public static List<WresIDGroup> wrestlerList = new List<WresIDGroup>();
        #endregion

        public MoreMatchTypes_Form()
        {
            form = this;
            InitializeComponent();

        }

        public void MoreMatchTypes_Form_Load(object sender, EventArgs e)
        {
            #region Shared Methods
            LoadOrgs();
            LoadSubs();
            LoadRings();
            LoadVenues();
            LoadReferees();
            LoadThemes();
            LoadDifficulty();
            LoadGameSpeed();
            #endregion

            #region Survival Road Methods
            LoadContinues();
            LoadMatches();
            LoadMatchTypes();
            #endregion

            #region Tooltips
            tt_normal.SetToolTip(cb_normalMatch, "Disables the More Match Types Mod.");
            #endregion
        }

        #region General Window Methods

        private void matchHelp_Click(object sender, EventArgs e)
        {
            if (cb_normalMatch.Checked)
            {
                MessageBox.Show("Disables the More Match Types mod.", "Normal Match", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (cb_IronManMatch.Checked)
            {
                MessageBox.Show("Score victories over a specified period of time.\nThe team with a greater number of victories is the winner.", "Iron Man Match", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (cb_FirstBlood.Checked)
            {
                MessageBox.Show("Win the match by forcing your opponent to bleed.\nThe team with a member that bleeds first is the loser.", "First Blood Match", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (cb_elimination.Checked)
            {
                MessageBox.Show("Two teams participate in a gauntlet of 1v1 battles.\nThe first team to run out of members is the loser.", "Elimination Match", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (cb_exElim.Checked)
            {
                MessageBox.Show("Two teams participate in a gauntlet of 1v1 battles.\nThe first team to run out of members is the loser.\nThis version allows an unlimited number of members per team.", "Elimination Match", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (cb_sumo.Checked)
            {
                MessageBox.Show("Two teams battle in a traditional sumo match.\nA team loses when any member falls to the mat.\nBasic Attacks determine which moves can be attempted, otherwise the clinch animation will be used.", "Sumo Match", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (cb_uwfi.Checked)
            {
                MessageBox.Show("Take part in a classic UWFI style match.\nTeams have a certain number of points that are reduced over the course of a match.\nThe first team to run out of points, or submit to the opponent is the loser.\nIllegal Attacks determine which moves will cost points based on legality.\nDQ Attacks determine which moves will cause a team to lose immediately.", "UWFI Match", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (cb_Pancrase.Checked)
            {
                MessageBox.Show("Take part in a classic Pancrase style match.\nThis match type only works with two fighters (no partners, no seconds).\nEach fighter has a certain number of points that are reduced over the course of a match.\nThe first fighter to run out of points, or submit to the opponent is the loser.\nIllegal Attacks determine which moves will cost points based on legality.\nDQ Attacks determine which moves will cause a team to lose immediately.", "Pancrase Match", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (cb_survival.Checked)
            {

            }

        }

        private void cb_FirstBlood_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
        }

        private void cb_IronManMatch_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
        }

        private void cb_sumo_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
            lbl_Basic.Visible = true;
            tb_basic.Visible = true;

            tb_basic.Text = "Shoutei\nFace Slap B\nChest Slap\nKnife-Edge Chop\nKoppo Style Shoutei\nThroat Chop\nJigoku-Tsuki";
            rulesTabControl.SelectedIndex = 0;
        }

        private void cb_uwfi_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
            lbl_illegal.Visible = true;
            lbl_dq.Visible = true;
            tb_illegal.Visible = true;
            tb_dq.Visible = true;

            List<String> illegalMoves = new List<String>()
            {
                "Knuckle Arrow",
                "Knuckle Pat",
                "Elbow to the Crown",
                "Elbow Stamp",
                "Elbow Stamp (Neck)",
                "Elbow Stamp (Arm)",
                "Elbow Stamp (Leg)",
                "Stomping (Face)",
                "Stomping (Neck)",
                "Clap Kick",
                "Thumbing to the Eyes",
                "Thumbing to the Eyes B",
                "Face Raking",
                "Choke Attack",
                "Cobra Claw",
                "Headbutt",
                "Headbutt Rush",
                "Jumping Headbutt",
                "Leg-Lift Headbutt Rush",
                "No-Touch Headbutt",
                "Enzui Headbutt",
                "Manhattan Drop",
                "Manhattan Drop B",
                "Mount Headbutt",
                "Mount Knuckle Arrow",
                "Corner Headbutt Rush",
                "Rope Trailing",
                "Guillotine Whip",
                "Corner Strike Rush",
                "Mount Punches",
                "Back Mount Punches"

            };
            List<String> instantDQ = new List<String>()
            {
                "Giant Steel Knuckles",
                "Brass Knuckle Punch",
                "Weapon Attack",
                "Scythe Attack",
                "Bite",
                "Testicular Claw",
                "Chair's Illusion",
                "Low Blow",
                "Lip Lock",
                "Back Low Blow",
                "Groin Head Drop",
                "Groin Knee Stamp",
                "Groin Stomping",
                "Ategai",
                "Bronco Buster",
                "Mist",
                "Big Fire"
            };

            //Illegal Moves
            tb_illegal.Text = "";
            foreach (String move in illegalMoves)
            {
                tb_illegal.Text += move + "\n";
            }

            tb_illegal.Text = tb_illegal.Text.Remove(tb_illegal.Text.Length - 1);

            //DQ Moves
            tb_dq.Text = "";
            foreach (String move in instantDQ)
            {
                tb_dq.Text += move + "\n";
            }

            tb_dq.Text = tb_dq.Text.Remove(tb_dq.Text.Length - 1);
            rulesTabControl.SelectedIndex = 0;

        }

        private void cb_Pancrase_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
            lbl_illegal.Visible = true;
            lbl_dq.Visible = true;
            tb_illegal.Visible = true;
            tb_dq.Visible = true;

            //Illegal Moves
            tb_illegal.Text = "Knuckle Arrow\nKnuckle Pat\nElbow to the Crown\nElbow Stamp\nElbow Stamp (Neck)\nElbow Stamp (Arm)\nElbow Stamp (Leg)\nStomping (Face)\nStomping (Neck)\nClap Kick\n";
            tb_illegal.Text += "Thumbing to the Eyes\nThumbing to the Eyes B\nFace Raking\nChoke Attack\nCobra Claw\nHeadbutt\nHeadbutt Rush\nJumping Headbutt\nLeg-Lift Headbutt Rush\nNo-Touch Headbutt\n";
            tb_illegal.Text += "Enzui Headbutt\nManhattan Drop\nManhattan Drop B\nMount Headbutt\nMount Knuckle Arrow\nCorner Headbutt Rush\nRope Trailing\nGuillotine Whip\nCorner Strike Rush\n";
            tb_illegal.Text += "Mount Punches\nBack Mount Punches";

            //DQ Moves
            tb_dq.Text = "Giant Steel Knuckles\nBrass Knuckle Punch\nWeapon Attack\nScythe Attack\nBite\nTesticular Claw\nChair's Illusion\nLow Blow\nLip Lock\nBack Low Blow\nGroin Head Drop\n";
            tb_dq.Text += "Groin Knee Stamp\nGroin Stomping\nAtegai\nBronco Buster\nMist\nBig Fire";

            rulesTabControl.SelectedIndex = 0;

        }

        private void tb_illegal_TextChanged(object sender, EventArgs e)
        {

        }

        private void cb_normalMatch_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
        }

        private void cb_elimination_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
        }

        private void cb_exElim_CheckedChanged(object sender, EventArgs e)
        {
            rulesTabControl.SelectedIndex = 1;
        }

        private void cb_survival_CheckedChanged(object sender, EventArgs e)
        {
            rulesTabControl.SelectedIndex = 2;
        }

        public void Clear()
        {
            //Hiding all customization items
            lbl_Basic.Visible = false;
            lbl_dq.Visible = false;
            lbl_illegal.Visible = false;
            tb_basic.Visible = false;
            tb_basic.Text = "";
            tb_illegal.Visible = false;
            tb_illegal.Text = "";
            tb_dq.Visible = false;
            tb_dq.Text = "";
        }
        #endregion

        #region Rule Setup Methods
        public void LoadOrgs()
        {
            //Elimination
            this.el_promotionList.Items.Clear();
            this.el_promotionList.Items.Add("ALL");

            //Survival Road
            this.sr_promotionList.Items.Clear();
            this.sr_promotionList.Items.Add("ALL");

            foreach (GroupInfo current in SaveData.GetInst().groupList)
            {
                string longName = SaveData.GetInst().organizationList[current.organizationID].longName;
                this.el_promotionList.Items.Add(longName + " : " + current.longName);
                this.sr_promotionList.Items.Add(longName + " : " + current.longName);

            }
        }

        private void LoadDifficulty()
        {
            //Set Difficulty Levels
            for (int i = 1; i <= 10; i++)
            {
                el_difficulty.Items.Add(i);
                sr_difficultyList.Items.Add(i);
            }

            el_difficulty.SelectedIndex = 0;
            sr_difficultyList.SelectedIndex = 0;
        }

        private void LoadSubsFromOrg(String matchType)
        {
            String query = "";
            ComboBox resultField = null;
            TextBox searchField = null;
            ComboBox promotionField = null;

            switch (matchType)
            {
                case "Elimination":
                    resultField = el_resultList;
                    searchField = el_searchInput;
                    promotionField = el_promotionList;
                    break;

                case "Survival":
                    resultField = sr_searchResult;
                    searchField = sr_searchInput;
                    promotionField = sr_promotionList;
                    break;

            }

            resultField.Items.Clear();

            //Find search terms
            query = searchField.Text;
            if (!query.TrimStart().TrimEnd().Equals(""))
            {
                foreach (WresIDGroup wrestler in wrestlerList)
                {
                    if (query.ToLower().Equals(wrestler.Name.ToLower()) || wrestler.Name.ToLower().Contains(query.ToLower()))
                    {
                        resultField.Items.Add(wrestler.Name + ":" + wrestler.ID);
                    }
                }
            }

            if (resultField.Items.Count > 0)
            {
                return;
            }

            if (promotionField.SelectedIndex == 0)
            {
                this.LoadSubs();
            }
            else
            {
                foreach (WresIDGroup current in wrestlerList)
                {
                    if (current.Group == promotionField.SelectedIndex - 1)
                    {
                        resultField.Items.Add(current.Name + ":" + current.ID);
                    }
                }
            }
        }

        private void LoadSubs()
        {
            wrestlerList.Clear();
            this.el_resultList.Items.Clear();
            this.sr_searchResult.Items.Clear();

            foreach (EditWrestlerData current in SaveData.inst.editWrestlerData)
            {
                WresIDGroup wresIDGroup = new WresIDGroup
                {
                    Name = DataBase.GetWrestlerFullName(current.wrestlerParam),
                    ID = 10000 + SaveData.inst.editWrestlerData.IndexOf(current),
                    Group = current.wrestlerParam.groupID
                };
                wrestlerList.Add(wresIDGroup);
                this.el_resultList.Items.Add(wresIDGroup.Name + ":" + wresIDGroup.ID);
                this.sr_searchResult.Items.Add(wresIDGroup.Name + ":" + wresIDGroup.ID);
            }

            this.el_promotionList.SelectedIndex = 0;
            this.el_resultList.SelectedIndex = 0;

            this.sr_searchResult.SelectedIndex = 0;
            this.sr_promotionList.SelectedIndex = 0;
        }

        private void LoadRings()
        {
            el_ringList.Items.Add("SWA");
            sr_ringList.Items.Add("SWA");
            foreach (RingData current in SaveData.GetInst().editRingData)
            {
                el_ringList.Items.Add(current.name);
                sr_ringList.Items.Add(current.name);
            }

            el_ringList.SelectedIndex = 0;
            sr_ringList.SelectedIndex = 0;
        }

        private void LoadVenues()
        {
            venues.Add("Big Garden Arena");
            venues.Add("SCS Stadium");
            venues.Add("Arena De Universo");
            venues.Add("Spike Dome");
            venues.Add("Yurakuen Hall");
            venues.Add("Dojo");

            foreach (String venue in venues)
            {
                el_venueList.Items.Add(venue);
                sr_venueList.Items.Add(venue);
            }

            el_venueList.SelectedIndex = 0;
            sr_venueList.SelectedIndex = 0;
        }

        private void LoadReferees()
        {
            el_refereeList.Items.Add("Mr Judgement");
            sr_refereeList.Items.Add("Mr Judgement");

            foreach (RefereeData current in SaveData.GetInst().editRefereeData)
            {
                IDObject referee = new IDObject(current.Prm.name, current.referee_id);
                el_refereeList.Items.Add(current.Prm.name);
                sr_refereeList.Items.Add(current.Prm.name);
            }

            el_refereeList.SelectedIndex = 0;
            sr_refereeList.SelectedIndex = 0;
        }

        private void LoadThemes()
        {
            el_bgm.Items.Add("Fire Pro Wrestling 2017");
            el_bgm.Items.Add("Spinning Panther 2017");
            el_bgm.Items.Add("Lonely Stage 2017");

            //Survival Road
            sr_bgmList.Items.Add("Fire Pro Wrestling 2017");
            sr_bgmList.Items.Add("Spinning Panther 2017");
            sr_bgmList.Items.Add("Lonely Stage 2017");

            string currentPath = System.IO.Directory.GetCurrentDirectory();

            try
            {
                IEnumerable<String> themes;
                themes = Directory.GetFiles(currentPath + @"\BGM");
                foreach (String theme in themes)
                {
                    el_bgm.Items.Add(theme.Replace(currentPath + @"\BGM", "").Replace(@"\", ""));
                    sr_bgmList.Items.Add(theme.Replace(currentPath + @"\BGM", "").Replace(@"\", ""));
                }

                el_bgm.SelectedIndex = 0;
                sr_bgmList.SelectedIndex = 0;
            }
            catch
            {

            }

        }

        private void LoadGameSpeed()
        {
            try
            {
                //Set Game Speeds
                gameSpeed.Add(100);
                gameSpeed.Add(125);
                gameSpeed.Add(150);
                gameSpeed.Add(175);
                gameSpeed.Add(200);
                gameSpeed.Add(300);
                gameSpeed.Add(400);
                gameSpeed.Add(800);
                gameSpeed.Add(1000);

                foreach (uint speed in gameSpeed)
                {
                    el_gameSpeed.Items.Add(speed);
                    sr_speedList.Items.Add(speed);
                }
                el_gameSpeed.SelectedIndex = 0;
                sr_speedList.SelectedIndex = 0;
            }
            catch
            { }
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
        #endregion

        #region Extended Elimination Controls
        private void btn_matchStart_Click(object sender, EventArgs e)
        {
            if (!ValidateMatch("Elimination"))
            {
                return;
            }

            #region Variables
            int blueTeamCount = el_blueList.Items.Count;
            int redTeamCount = el_redList.Items.Count;
            bool isSecond;
            MatchSetting settings = GlobalWork.GetInst().MatchSetting;
            #endregion

            settings = SetMatchConfig("Elimination", settings);

            try
            {
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
                    String[] wrestlerName = el_blueList.Items[i].ToString().Split(':');
                    WrestlerID wrestlerNo = (WrestlerID)Int32.Parse(wrestlerName[wrestlerName.Length - 1]);
                    if (i == 0)
                    {
                        isSecond = false;
                    }
                    else
                    {
                        isSecond = true;
                    }
                    settings = AddPlayers(true, wrestlerNo, i, 0, isSecond, 0, settings);
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
                    String[] wrestlerName = el_redList.Items[i].ToString().Split(':');
                    WrestlerID wrestlerNo = (WrestlerID)Int32.Parse(wrestlerName[wrestlerName.Length - 1]);
                    if (i == 0)
                    {
                        isSecond = false;
                    }
                    else
                    {
                        isSecond = true;
                    }
                    settings = AddPlayers(true, wrestlerNo, i + 4, 0, isSecond, 0, settings);
                }
                #endregion

                StartMatch("Elimination");
                btn_matchStart.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btn_matchStart.Enabled = true;
            }

        }
        private void el_refresh_Click(object sender, EventArgs e)
        {
            LoadOrgs();
            LoadSubs();
            LoadRings();
            LoadReferees();
        }
        private void el_addBtn_Click(object sender, EventArgs e)
        {
            if (rb_singleBlue.Checked)
            {
                el_blueList.Items.Add(el_resultList.SelectedItem);
            }
            else if (rb_singleRed.Checked)
            {
                el_redList.Items.Add(el_resultList.SelectedItem);
            }
            else if (rb_allBlue.Checked)
            {
                for (int i = 0; i < el_resultList.Items.Count; i++)
                {
                    el_blueList.Items.Add(el_resultList.Items[i]);
                }
            }
            else if (rb_allRed.Checked)
            {
                for (int i = 0; i < el_resultList.Items.Count; i++)
                {
                    el_redList.Items.Add(el_resultList.Items[i]);
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
            LoadSubsFromOrg("Elimination");
            el_resultList.SelectedIndex = 0;
        }
        #endregion

        #region Survival Road Controls
        private void sr_start_Click(object sender, EventArgs e)
        {
            if (!ValidateMatch("Survival"))
            {
                return;
            }
        }

        private void sr_Search_Click(object sender, EventArgs e)
        {
            LoadSubsFromOrg("Survival");
            sr_searchResult.SelectedIndex = 0;
        }

        private void sr_Add_Click(object sender, EventArgs e)
        {
            if (sr_addWrestler.Checked)
            {
                sr_wrestler.Text = sr_searchResult.SelectedItem.ToString();
            }
            if (sr_addSecond.Checked)
            {
                sr_second.Text = sr_searchResult.SelectedItem.ToString();
            }
            if (sr_addSingle.Checked)
            {
                sr_teamList.Items.Add(sr_searchResult.SelectedItem.ToString());
            }
            if (sr_addAll.Checked)
            {
                foreach (String wrestler in sr_searchResult.Items)
                {
                    sr_teamList.Items.Add(wrestler);
                }
            }
        }

        private void sr_Refresh_Click(object sender, EventArgs e)
        {

        }

        private void sr_RemoveOne_Click(object sender, EventArgs e)
        {
            sr_teamList.Items.Remove(sr_teamList.SelectedItem);
        }

        private void sr_removeAll_Click(object sender, EventArgs e)
        {
            sr_teamList.Items.Clear();
        }

        private void sr_continues_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void sr_matches_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void sr_matchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Setting general variables
            sr_controlBoth.Checked = false;
            sr_tag.Visible = true;
            sr_addSecond.Visible = true;

            //Ensuring that tornado tag only modes are accounted for
            if (sr_matchType.SelectedIndex == 0)
            {
                if (sr_tag.Checked)
                {
                    sr_controlBoth.Visible = true;
                }
            }
            else
            {
                sr_controlBoth.Visible = false;
            }

            //Ensuring that modes disallowing tag matches are accounted for.
            if (sr_matchType.SelectedIndex == 4 | sr_matchType.SelectedIndex == 6)
            {
                sr_addSecond.Checked = false;
                sr_addSecond.Visible = false;
                sr_addWrestler.Checked = true;
                sr_tag.Visible = false;
                sr_second.Clear();
                sr_single.Checked = true;
            }

        }

        private void sr_single_CheckedChanged(object sender, EventArgs e)
        {
            if (sr_single.Checked)
            {
                sr_controlBoth.Checked = false;
                sr_controlBoth.Visible = false;
            }
        }

        private void sr_tag_CheckedChanged(object sender, EventArgs e)
        {
            if (sr_tag.Checked && sr_matchType.SelectedIndex == 0)
            {
                sr_controlBoth.Visible = true;
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

        private void sr_wrestlerClear_Click(object sender, EventArgs e)
        {
            sr_wrestler.Clear();
        }

        private void sr_secondClear_Click(object sender, EventArgs e)
        {
            sr_second.Clear();
        }
        #endregion

        #region Shared Execution Methods
        private bool ValidateMatch(String matchType)
        {
            bool isValid = true;

            switch (matchType)
            {
                case "Elimination":
                    if (!cb_exElim.Checked)
                    {
                        MessageBox.Show("The Extended Elimination option must be selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        isValid = false;
                    }

                    if (el_blueList.Items.Count == 0 || el_redList.Items.Count == 0)
                    {
                        MessageBox.Show("Both teams must contain at least one member.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        isValid = false;
                    }
                    break;
                case "Survival":
                    if (!cb_survival.Checked)
                    {
                        MessageBox.Show("The Survival Road option must be selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        isValid = false;
                    }
                    break;
            }

            return isValid;
        }

        private void StartMatch(String matchType)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Match");
        }

        private MatchSetting SetMatchConfig(String matchType, MatchSetting settings)
        {
            ComboBox ringList = null;
            ComboBox refereeList = null;
            ComboBox arenaList = null;
            ComboBox difficultyList = null;
            ComboBox speedList = null;
            ComboBox bgmList = null;

            switch (matchType)
            {
                case "Elimination":
                    ringList = this.el_ringList;
                    refereeList = this.el_refereeList;
                    arenaList = this.el_venueList;
                    difficultyList = this.el_difficulty;
                    speedList = this.el_gameSpeed;
                    bgmList = this.el_bgm;
                    break;
                case "Survival":
                    ringList = this.sr_ringList;
                    refereeList = this.sr_refereeList;
                    arenaList = this.sr_venueList;
                    difficultyList = this.sr_difficultyList;
                    speedList = this.sr_speedList;
                    bgmList = this.sr_bgmList;
                    break;
            }

            #region Match Setting Config
            GlobalParam.Delete_BattleConfig();
            GlobalParam.Intalize_BattleMode();
            GlobalParam.Intalize_BattleConfig();
            GlobalParam.Load_ConfigData();
            GlobalParam.Set_BattleConfig_Value(26, 0);
            GlobalParam.Set_MatchSetting_DefaultParam();
            GlobalParam.TitleMatch_BeltData = null;
            GlobalParam.m_BattleMode = GlobalParam.BattleMode.OneNightMatch;
            GlobalParam.m_BattleRule = GlobalParam.BattleRule.Normal;
            GlobalParam.flg_TitleMatch_Ready = false;
            GlobalParam.Set_MatchSetting_Wrestler(false);
            GlobalParam.Set_MatchSetting_Rule();
            GlobalParam.Init_WrestlerData();
            GlobalParam.Intalize_BattleMode();
            GlobalParam.Intalize_BattleConfig();

            try
            {
                settings.ringID = ringList.SelectedIndex;
                if (settings.ringID != 0)
                {
                    settings.ringID = 10000 + settings.ringID - 1;
                }
            }

            catch
            {
                settings.ringID = 0;
            }

            try
            {
                settings.RefereeID = refereeList.SelectedIndex;
                if (settings.RefereeID != 0)
                {
                    settings.RefereeID = 10000 + settings.RefereeID - 1;
                }
            }
            catch
            {
                settings.RefereeID = 0;
            }

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
                    default:
                        settings.arena = VenueEnum.BigGardenArena;
                        break;
                }

            }
            catch
            {
                settings.arena = VenueEnum.Dojo;
            }

            settings.BattleRoyalKind = BattleRoyalKindEnum.Off;
            settings.VictoryCondition = VictoryConditionEnum.Count3;
            settings.isOverTheTopRopeOn = false;
            settings.MatchTime = 0;
            settings.is3GameMatch = false;
            settings.ComLevel = difficultyList.SelectedIndex;

            try
            {
                settings.GameSpeed = (uint)speedList.SelectedItem;
            }
            catch
            {
                settings.GameSpeed = 100;
            }

            settings.isRopeCheck = true;
            settings.isElimination = false;
            settings.isLumberjack = false;
            settings.isTornadoBattle = false;
            settings.isCutPlay = false;
            settings.isDisableTimeCount = false;
            settings.isFoulCount = true;
            settings.CriticalRate = CriticalRateEnum.Off;

            //Need to set a valid MatchBGM type  here, then override it on match start if necessary.
            //if (el_bgm.SelectedIndex > 2)
            //{
            //    settings.matchBGM = MatchBGM.SpinningPanther;
            //}
            //else
            //{
            //    settings.matchBGM = (MatchBGM)el_bgm.SelectedIndex;
            //}

            settings.matchBGM = MatchBGM.SpinningPanther;

            settings.isSkipEntranceScene = true;
            settings.entranceSceenMode = EntranceSceneMode.EachCorner;
            settings.isPlayDemo = false;
            GlobalParam.flg_CallDebugMenu = false;
            GlobalParam.befor_scene = "Scene_BattleSetting";
            GlobalParam.keep_scene = "Scene_BattleSetting";
            GlobalParam.next_scene = "";
            #endregion

            return settings;
        }

        private MatchSetting AddPlayers(bool entry, WrestlerID wrestlerNo, int slot, int control, bool isSecond, int costume, MatchSetting settings)
        {
            settings.matchWrestlerInfo[slot].entry = entry;

            if (entry)
            {
                settings.matchWrestlerInfo[slot].wrestlerID = wrestlerNo;
                settings.matchWrestlerInfo[slot].costume_no = costume;
                settings.matchWrestlerInfo[slot].alignment = WrestlerAlignmentEnum.Neutral;

                //Determine what the assigned pad should be
                switch (control)
                {
                    case 0:
                        settings.matchWrestlerInfo[slot].assignedPad = PadPort.AI;
                        break;
                    case 1:
                        settings.matchWrestlerInfo[slot].assignedPad = PadPort.Pad1;
                        break;

                    case 2:
                        settings.matchWrestlerInfo[slot].assignedPad = PadPort.Pad2;
                        break;

                    default:
                        settings.matchWrestlerInfo[slot].assignedPad = PadPort.AI;
                        break;
                }

                settings.matchWrestlerInfo[slot].HP = 65535f;
                settings.matchWrestlerInfo[slot].SP = 65535f;
                settings.matchWrestlerInfo[slot].HP_Neck = 65535f;
                settings.matchWrestlerInfo[slot].HP_Arm = 65535f;
                settings.matchWrestlerInfo[slot].HP_Waist = 65535f;
                settings.matchWrestlerInfo[slot].HP_Leg = 65535f;

                GlobalParam.Set_WrestlerData(slot, control, wrestlerNo, isSecond, costume, 65535f, 65535f, 65535f, 65535f, 65535f, 65535f);
            }
            else
            {
                settings.matchWrestlerInfo[slot].wrestlerID = global::WrestlerID.Invalid;
                GlobalParam.Set_WrestlerData(slot, -1, global::WrestlerID.Invalid, false, 0, 65535f, 65535f, 65535f, 65535f, 65535f, 65535f);
            }
            return settings;
        }
        #endregion

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }


        private void tt_normal_Popup(object sender, PopupEventArgs e)
        {

        }

    }
}
