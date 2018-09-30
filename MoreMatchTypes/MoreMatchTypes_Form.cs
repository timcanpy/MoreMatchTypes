using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DG;
using DG.DG;
using System.Diagnostics;
using System.IO;
using MatchConfig;
using System.Linq;

namespace MoreMatchTypes
{
    public partial class MoreMatchTypes_Form : Form
    {
        #region Variables
        public static MoreMatchTypes_Form form = null;
        public static List<String> promotionList = new List<string>();
        public static List<uint> gameSpeed = new List<uint>();
        public static List<WresIDGroup> wrestlerList = new List<WresIDGroup>();
        #endregion

        public MoreMatchTypes_Form()
        {
            form = this;
            InitializeComponent();
            rulesTabControl.TabPages.RemoveAt(2);
            rulesTabControl.TabPages.RemoveAt(1);

        }

        public void MoreMatchTypes_Form_Load(object sender, EventArgs e)
        {
            #region Shared Methods
            try
            {
                LoadOrgs();
                LoadSubs();
                LoadRings();
                LoadReferees();
                LoadThemes();
                LoadDifficulty();
                LoadGameSpeed();
                LoadVenues();
                #endregion
            }
            catch (Exception ex)
            {
                L.D("Load Match Type Execption: " + ex.Message);
            }
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
                MessageBox.Show("Take part in a gauntlet of matches.\nThe game will proceed until all matches are completed, or the player runs out of continues.\nEvery continue resets a player team's health and spirit.", "Survival Road", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            if (cb_timedElim.Checked)
            {
                MessageBox.Show("Take place in a timed tornado tag battle, where players join over the course of a match.\nThe first player joins after five minutes, then the next joins every two minutes afterwards.\nWhen all players have joined the match, pinfall or submission victories are possible.\nThe first team to score a victory wins the match.", "Timed Tornado Tag", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            tb_basic.Text = "Shoutei\nFace Slap B\nChest Slap\nKnife-Edge Chop\nKoppo Style Shoutei\nThroat Chop\nJigoku-Tsuki\nElbow Butt";
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
            promotionList.Clear();
            this.el_promotionList.Items.Clear();
            this.sr_promotionList.Items.Clear();

            foreach (String promotion in MatchConfiguration.LoadPromotions())
            {
                this.el_promotionList.Items.Add(promotion);
                this.sr_promotionList.Items.Add(promotion);
                promotionList.Add(promotion);
            }
        }

        private void LoadDifficulty()
        {
            //Set Difficulty Levels
            foreach (String i in MatchConfiguration.LoadDifficulty())
            {
                el_difficulty.Items.Add(i);
                sr_difficultyList.Items.Add(i);
            }

            el_difficulty.SelectedIndex = 8;
            sr_difficultyList.SelectedIndex = 8;
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
                        resultField.Items.Add(wrestler);
                    }
                }
            }

            if (resultField.Items.Count > 0)
            {
                return;
            }
            if (promotionField.SelectedItem.ToString().Contains("未登録"))
            {
                LoadSubs();
            }
            else
            {
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
        }

        private void LoadSubs()
        {
            wrestlerList.Clear();
            this.el_resultList.Items.Clear();
            this.sr_searchResult.Items.Clear();

            wrestlerList = MatchConfiguration.LoadWrestlers();

            foreach (WresIDGroup wrestler in wrestlerList)
            {
                this.el_resultList.Items.Add(wrestler);
                this.sr_searchResult.Items.Add(wrestler);
            }

            this.el_promotionList.SelectedIndex = 0;
            this.el_resultList.SelectedIndex = 0;

            this.sr_searchResult.SelectedIndex = 0;
            this.sr_promotionList.SelectedIndex = 0;
        }

        private void LoadRings()
        {
            foreach (String ring in MatchConfiguration.LoadRings())
            {
                el_ringList.Items.Add(ring);
                sr_ringList.Items.Add(ring);
            }

            el_ringList.SelectedIndex = 0;
            sr_ringList.SelectedIndex = 0;
        }

        private void LoadVenues()
        {
            String[] venues = MatchConfiguration.LoadVenue();
            foreach (String venue in venues)
            {
                el_venueList.Items.Add(venue);
                sr_venueList.Items.Add(venue);
            }

            el_venueList.SelectedIndex = 4;
            sr_venueList.SelectedIndex = 4;
        }

        private void LoadReferees()
        {
            foreach (String referee in MatchConfiguration.LoadReferees())
            {
                el_refereeList.Items.Add(referee);
                sr_refereeList.Items.Add(referee);
            }

            el_refereeList.SelectedIndex = 0;
            sr_refereeList.SelectedIndex = 0;
        }

        private void LoadThemes()
        {
            foreach (String theme in MatchConfiguration.LoadBGMs())
            {
                el_bgm.Items.Add(theme);
                sr_bgmList.Items.Add(theme);
            }

            el_bgm.SelectedIndex = 0;
            sr_bgmList.SelectedIndex = 0;

        }

        private void LoadGameSpeed()
        {
            try
            {
                foreach (uint speed in MatchConfiguration.LoadSpeed())
                {
                    el_gameSpeed.Items.Add(speed);
                    sr_speedList.Items.Add(speed);
                }
                el_gameSpeed.SelectedIndex = 1;
                sr_speedList.SelectedIndex = 1;
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

            try
            {
                settings = SetMatchConfig("Elimination", settings);

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

            #region Variables
            MatchSetting settings = GlobalWork.GetInst().MatchSetting;
            WresIDGroup player = (WresIDGroup)sr_wrestler.SelectedItem;
            WresIDGroup second = (WresIDGroup)sr_second.SelectedItem;
            String selectedType = sr_matchType.SelectedItem.ToString();
            WrestlerID wrestlerNo = WrestlerID.AbbieJones;
            bool isSecond = false;
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
                                L.D("Setting tag partner");
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
                    if (validEntry)
                    {
                        L.D("Wrestler Index: " + i + "\nIs Second: " + isSecond);
                    }

                }

                //Set-up opponents
                WresIDGroup opponent;
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

                        //Get Random opponent
                        int rngValue = UnityEngine.Random.Range(0, opponentCount - 1);

                        //Determine if we're creating a tag team or single competitor
                        if (opponentCount == 1)
                        {
                            wrestlerNo = MatchConfiguration.GetWrestlerNo((WresIDGroup)sr_teamList.Items[rngValue]);
                            sr_singleOpponent.Text = wrestlerNo.ToString();
                        }
                        else if (i == 4)
                        {
                            wrestlerNo = MatchConfiguration.GetWrestlerNo((WresIDGroup)sr_teamList.Items[rngValue]);
                            sr_singleOpponent.Text = wrestlerNo.ToString();
                        }
                        else if (i == 5)
                        {
                            wrestlerNo = MatchConfiguration.GetWrestlerNo((WresIDGroup)sr_teamList.Items[rngValue]);

                            //Ensure that we aren't fielding duplicate wrestlers
                            while (sr_singleOpponent.Text.Equals(wrestlerNo.ToString()))
                            {
                                rngValue = UnityEngine.Random.Range(0, opponentCount - 1);
                                wrestlerNo = MatchConfiguration.GetWrestlerNo((WresIDGroup)sr_teamList.Items[rngValue]);
                            }

                            sr_tagOpponent.Text = wrestlerNo.ToString();
                        }

                        if (sr_tag.Checked && i == 5)
                        {
                            validEntry = true;
                        }
                        else if (!sr_tag.Checked && i == 5)
                        {
                            validEntry = false;
                        }
                    }

                    settings = MatchConfiguration.AddPlayers(validEntry, wrestlerNo, i, 0, false, 0, settings);
                }

                StartMatch("Survival");
                MoreMatchTypes_Form.form.sr_start.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                sr_wrestler.Items.Clear();
                sr_wrestler.Items.Add(sr_searchResult.SelectedItem);
                sr_wrestler.SelectedIndex = 0;
            }
            if (sr_addSecond.Checked)
            {
                sr_second.Items.Clear();
                sr_second.Items.Add(sr_searchResult.SelectedItem);
                sr_second.SelectedIndex = 0;
            }
            if (sr_addSingle.Checked)
            {
                sr_teamList.Items.Add(sr_searchResult.SelectedItem);
            }
            if (sr_addAll.Checked)
            {
                foreach (WresIDGroup wrestler in sr_searchResult.Items)
                {
                    sr_teamList.Items.Add(wrestler);
                    L.D("Storing Wrestler " + wrestler.Name + ", " + wrestler.ID);
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

        private void sr_wrestlerClear_Click(object sender, EventArgs e)
        {
            sr_wrestler.Items.Clear();
        }

        private void sr_secondClear_Click(object sender, EventArgs e)
        {
            sr_second.Items.Clear();
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
                        ShowError("The Extended Elimination option must be selected.");
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

            //Setting controls
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

            //Set Ring
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

            //Set Referee
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

            //Set Game Speed
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
            settings.isOutOfRingCount = true;

            //Setting Custom Rules
            switch (matchType)
            {
                case "Elimination":
                    settings.isFoulCount = true;
                    settings.CriticalRate = CriticalRateEnum.Half;
                    break;
                case "Survival":
                    String type = sr_matchType.SelectedItem.ToString();
                    //settings.isOutOfRingCount = false;
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
                    break;
            }

            //Need to set a valid MatchBGM type  here, then override it on match start if necessary.
            if (el_bgm.SelectedIndex > 2)
            {
                settings.matchBGM = MatchBGM.SpinningPanther;
            }
            else
            {
                settings.matchBGM = (MatchBGM)el_bgm.SelectedIndex;
            }

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

        #endregion

        #region Helper Methods
        private void ShowError(String message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private int FindGroup(String groupName)
        {
            return promotionList.IndexOf(groupName);
        }

        #endregion

        private void sr_promotionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            sr_Search_Click(sender, e);
        }

        private void el_promotionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            el_searchBtn_Click(sender, e);
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
    }
}
