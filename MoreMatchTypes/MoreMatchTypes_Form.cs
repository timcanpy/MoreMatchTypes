using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DG;
using DG.DG;
using System.Diagnostics;
using System.IO;

namespace MoreMatchTypes
{
    public partial class MoreMatchTypes_Form : Form
    {
        public static MoreMatchTypes_Form form = null;
        public static List<uint> gameSpeed = new List<uint>();
        public static List<String> venues = new List<String>();

        public MoreMatchTypes_Form()
        {
            form = this;
            InitializeComponent();

        }

        public static List<WresIDGroup> wrestlerList = new List<WresIDGroup>();

        public void MoreMatchTypes_Form_Load(object sender, EventArgs e)
        {
            LoadOrgs();
            LoadSubs();
            LoadRings();
            LoadVenues();
            LoadReferees();
            LoadThemes();
            LoadDifficulty();
            LoadGameSpeed();
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

        private void el_searchBtn_Click(object sender, EventArgs e)
        {
            LoadSubsFromOrg();
            el_resultList.SelectedIndex = 0;
        }

        #region Rule Functions
        public void LoadOrgs()
        {
            //Elimination
            this.el_promotionList.Items.Clear();
            this.el_promotionList.Items.Add("ALL");

            //Survival Road
            
            foreach (GroupInfo current in SaveData.GetInst().groupList)
            {
                string longName = SaveData.GetInst().organizationList[current.organizationID].longName;
                this.el_promotionList.Items.Add(longName + " : " + current.longName);

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
        }

        private void LoadSubsFromOrg()
        {
            this.el_resultList.Items.Clear();

            //Find search terms
            String query = el_searchInput.Text;
            if (!query.TrimStart().TrimEnd().Equals(""))
            {
                foreach (WresIDGroup wrestler in wrestlerList)
                {
                    if (query.ToLower().Equals(wrestler.Name.ToLower()) || wrestler.Name.ToLower().Contains(query.ToLower()))
                    {
                        el_resultList.Items.Add(wrestler.Name + ":" + wrestler.ID);
                    }
                }
            }

            if (el_resultList.Items.Count > 0)
            {
                return;
            }

            if (this.el_promotionList.SelectedIndex == 0)
            {
                this.LoadSubs();
            }
            else
            {
                foreach (WresIDGroup current in wrestlerList)
                {
                    if (current.Group == this.el_promotionList.SelectedIndex - 1)
                    {
                        this.el_resultList.Items.Add(current.Name + ":" + current.ID);
                    }
                }
            }
        }

        private void LoadSubs()
        {
            wrestlerList.Clear();
            this.el_resultList.Items.Clear();

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
            }

            this.el_promotionList.SelectedIndex = 0;
            this.el_resultList.SelectedIndex = 0;
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

            foreach(String venue in venues)
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
        #endregion

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

        private void btn_matchStart_Click(object sender, EventArgs e)
        {
            if (!cb_exElim.Checked)
            {
                MessageBox.Show("The Extended Elimination option must be selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (el_blueList.Items.Count == 0 || el_redList.Items.Count == 0)
            {
                MessageBox.Show("Both teams must contain at least one member.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int blueTeamCount = el_blueList.Items.Count;
            int redTeamCount = el_redList.Items.Count;

            try
            {
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

                MatchSetting settings = GlobalWork.GetInst().MatchSetting;
                
                try
                {
                    settings.ringID = MoreMatchTypes_Form.form.el_ringList.SelectedIndex;
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
                    settings.RefereeID = MoreMatchTypes_Form.form.el_refereeList.SelectedIndex;
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
                    String venue = (String)MoreMatchTypes_Form.form.el_venueList.SelectedItem;
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
                settings.ComLevel = MoreMatchTypes_Form.form.el_difficulty.SelectedIndex;

                try
                {
                    settings.GameSpeed = (uint)MoreMatchTypes_Form.form.el_gameSpeed.SelectedItem;
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
                if (el_bgm.SelectedIndex > 3)
                {
                    settings.matchBGM = MatchBGM.FireProWrestling;
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
                    settings.matchWrestlerInfo[i].entry = true;
                    String[] wrestlerName = el_blueList.Items[i].ToString().Split(':');
                    settings.matchWrestlerInfo[i].wrestlerID = (WrestlerID)Int32.Parse(wrestlerName[wrestlerName.Length - 1]);
                    settings.matchWrestlerInfo[i].costume_no = 0;
                    settings.matchWrestlerInfo[i].alignment = WrestlerAlignmentEnum.Neutral;
                    settings.matchWrestlerInfo[i].assignedPad = PadPort.AI;

                    bool isSecond;
                    if (i == 0)
                    {
                        settings.matchWrestlerInfo[i].isSecond = false;
                        isSecond = false;
                    }
                    else
                    {
                        settings.matchWrestlerInfo[i].isSecond = true;
                        isSecond = true;
                    }
                    settings.matchWrestlerInfo[i].HP = 65535f;
                    settings.matchWrestlerInfo[i].SP = 65535f;
                    settings.matchWrestlerInfo[i].HP_Neck = 65535f;
                    settings.matchWrestlerInfo[i].HP_Arm = 65535f;
                    settings.matchWrestlerInfo[i].HP_Waist = 65535f;
                    settings.matchWrestlerInfo[i].HP_Leg = 65535f;

                    int playerControl = 0;
                    {
                        if (i == 0)
                        {
                            if (MoreMatchTypes_Form.form.el_blueControl.Checked)
                            {
                                playerControl = 1;
                            }
                        }
                    }

                    GlobalParam.Set_WrestlerData(i, playerControl, settings.matchWrestlerInfo[i].wrestlerID, isSecond, 0, 65535f, 65535f, 65535f, 65535f, 65535f, 65535f);
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
                    settings.matchWrestlerInfo[i + 4].entry = true;
                    String[] wrestlerName = el_redList.Items[i].ToString().Split(':');
                    settings.matchWrestlerInfo[i + 4].wrestlerID = (WrestlerID)Int32.Parse(wrestlerName[wrestlerName.Length - 1]);
                    settings.matchWrestlerInfo[i + 4].costume_no = 0;
                    settings.matchWrestlerInfo[i + 4].alignment = WrestlerAlignmentEnum.Neutral;
                    settings.matchWrestlerInfo[i + 4].assignedPad = PadPort.AI;

                    bool isSecond;
                    if (i == 0)
                    {
                        settings.matchWrestlerInfo[i + 4].isSecond = false;
                        isSecond = false;
                    }
                    else
                    {
                        settings.matchWrestlerInfo[i + 4].isSecond = true;
                        isSecond = true;
                    }
                    settings.matchWrestlerInfo[i + 4].HP = 65535f;
                    settings.matchWrestlerInfo[i + 4].SP = 65535f;
                    settings.matchWrestlerInfo[i + 4].HP_Neck = 65535f;
                    settings.matchWrestlerInfo[i + 4].HP_Arm = 65535f;
                    settings.matchWrestlerInfo[i + 4].HP_Waist = 65535f;
                    settings.matchWrestlerInfo[i + 4].HP_Leg = 65535f;

                    int playerControl = 0;
                    {
                        if (i == 0)
                        {
                            if (MoreMatchTypes_Form.form.el_blueControl.Checked && MoreMatchTypes_Form.form.el_redControl.Checked)
                            {
                                playerControl = 2;
                            }
                            else if (!MoreMatchTypes_Form.form.el_blueControl.Checked && MoreMatchTypes_Form.form.el_redControl.Checked)
                            {
                                playerControl = 1;
                            }
                        }
                    }

                    GlobalParam.Set_WrestlerData(i + 4, playerControl, settings.matchWrestlerInfo[i + 4].wrestlerID, isSecond, 0, 65535f, 65535f, 65535f, 65535f, 65535f, 65535f);

                }
                #endregion

                UnityEngine.SceneManagement.SceneManager.LoadScene("Match");
                btn_matchStart.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btn_matchStart.Enabled = true;
            }

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }
    }

    public class WresIDGroup
    {
        public string Name;

        public int ID;

        public int Group;

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class IDObject
    {
        public string Name;

        public int ID;

        public IDObject(String name, int id)
        {
            Name = name;
            ID = id;
        }
    }
}
