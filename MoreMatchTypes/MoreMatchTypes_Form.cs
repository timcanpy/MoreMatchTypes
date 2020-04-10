using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DG;
using System.Diagnostics;
using System.IO;
using MatchConfig;
using System.Linq;
using MoreMatchTypes.DataClasses;
using System.Reflection;
using MoreMatchTypes.Data_Classes;

namespace MoreMatchTypes
{
    public partial class MoreMatchTypes_Form : Form
    {
        #region Variables
        public static MoreMatchTypes_Form moreMatchTypesForm = null;
        private static List<String> promotionList = new List<string>();
        //public static List<uint> gameSpeed = new List<uint>();
        public static List<WresIDGroup> wrestlerList = new List<WresIDGroup>();
        private static String[] saveFileNames = new String[] { "SumoMoves.dat", "UWFIMoves.dat", "PancraseMoves.dat", "BoxingMoves.dat", "KickBoxingMoves.dat" };
        private static String[] saveFolderNames = new String[] { "./MatchTypeData/" };
        private static String sectionDivider = "|-------------------|";

        private static SurvivalRoadData survivalRoadData;
        public static SurvivalRoadData SurvivalRoadData { get => survivalRoadData; set => survivalRoadData = value; }

        private static ExEliminationData exEliminationData;
        public static ExEliminationData ExEliminationData { get => exEliminationData; set => exEliminationData = value; }

        private static String modPackName = "ModPack";
        public static bool modPackExists;

        #region Move Listing
        private static List<String> legalSumoMoves = new List<String>();
        private static List<String> illegalPancraseMoves = new List<String>();
        private static List<String> dqPancraseMoves = new List<String>();
        private static List<String> illegalUWFIMoves = new List<String>();
        private static List<String> dqUWFIMoves = new List<String>();
        private static List<String> dqBoxingMoves = new List<String>();
        private static List<String> dqKickboxingMoves = new List<String>();

        #endregion

        #endregion

        #region Initialization Methods
        public MoreMatchTypes_Form()
        {
            moreMatchTypesForm = this;
            InitializeComponent();
            FormClosing += MoreMatchTypes_FormClosing;
            tb_basic.LostFocus += tb_basic_LostFocus;
            tb_illegal.LostFocus += tb_illegal_LostFocus;
            tb_dq.LostFocus += tb_dq_LostFocus;

            //Determine whether UWFI and Pancrase matches are enabled
            //These rely on the ModPack.
            modPackExists = CheckModPackLoaded();

            cb_Pancrase.Visible = modPackExists;
            cb_uwfi.Visible = modPackExists;
            cb_Pancrase.Enabled = modPackExists;
            cb_uwfi.Enabled = modPackExists;

            //Remove Iron Man and First Blood, since the ModPack already has instances
            cb_IronManMatch.Visible = !modPackExists;
            cb_FirstBlood.Visible = !modPackExists;
            cb_IronManMatch.Enabled = !modPackExists;
            cb_FirstBlood.Enabled = !modPackExists;
        }
        public void MoreMatchTypes_Form_Load(object sender, EventArgs e)
        {
            #region Tooltips
            tt_normal.SetToolTip(cb_normalMatch, "Disables the More Match Types Mod.");
            #endregion

            #region Move Load
            LoadMoves();
            UpdateMoves();
            #endregion
        }

        #endregion

        #region Data Save
        private void SaveMoves()
        {
            //Sumo Moves
            String filePath = CheckSaveFile("Sumo");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (StreamWriter sw = File.AppendText(filePath))
            {
                foreach (String move in legalSumoMoves)
                {
                    if (move.Equals("\n") || move.Equals(""))
                    {
                        continue;
                    }
                    sw.WriteLine(move);
                }
            }

            //Pancrase Moves
            filePath = CheckSaveFile("Pancrase");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (StreamWriter sw = File.AppendText(filePath))
            {
                foreach (String move in illegalPancraseMoves)
                {
                    if (move.Equals("\n") || move.Equals(""))
                    {
                        continue;
                    }
                    sw.WriteLine(move);
                }

                sw.WriteLine(sectionDivider);

                foreach (String move in dqPancraseMoves)
                {
                    if (move.Equals("\n") || move.Equals(""))
                    {
                        continue;
                    }
                    sw.WriteLine(move);
                }
            }

            //UWFI Moves
            filePath = CheckSaveFile("UWFI");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (StreamWriter sw = File.AppendText(filePath))
            {
                foreach (String move in illegalUWFIMoves)
                {
                    if (move.Equals("\n") || move.Equals(""))
                    {
                        continue;
                    }
                    sw.WriteLine(move);
                }

                sw.WriteLine(sectionDivider);

                foreach (String move in dqUWFIMoves)
                {
                    if (move.Equals("\n") || move.Equals(""))
                    {
                        continue;
                    }
                    sw.WriteLine(move);
                }
            }

            //Boxing Moves
            filePath = CheckSaveFile("Boxing");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (StreamWriter sw = File.AppendText(filePath))
            {
                foreach (String move in dqBoxingMoves)
                {
                    if (move.Equals("\n") || move.Equals(""))
                    {
                        continue;
                    }
                    sw.WriteLine(move);
                }
            }

            //Boxing Moves
            filePath = CheckSaveFile("KickBoxing");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (StreamWriter sw = File.AppendText(filePath))
            {
                foreach (String move in dqKickboxingMoves)
                {
                    if (move.Equals("\n") || move.Equals(""))
                    {
                        continue;
                    }
                    sw.WriteLine(move);
                }
            }
        }
        private String CheckSaveFile(String dataType)
        {
            String path = CheckSaveFolder(dataType);

            switch (dataType)
            {
                case "Sumo":
                    path = path + saveFileNames[0];
                    break;
                case "UWFI":
                    path = path + saveFileNames[1];
                    break;
                case "Pancrase":
                    path = path + saveFileNames[2];
                    break;
                case "Boxing":
                    path = path + saveFileNames[3];
                    break;
                case "KickBoxing":
                    path = path + saveFileNames[4];
                    break;
                default:
                    path = path + saveFileNames[0];
                    break;
            }

            return path;

        }
        private String CheckSaveFolder(String dataType)
        {
            String folder = "";
            switch (dataType)
            {
                default:
                    folder = saveFolderNames[0];
                    break;
            }

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return folder;

        }

        #endregion

        #region Data Load
        private void LoadMoves()
        {
            String filePath = CheckSaveFile("Sumo");
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    legalSumoMoves.Clear();
                    var lines = File.ReadAllLines(filePath);
                    foreach (String move in lines)
                    {
                        legalSumoMoves.Add(move);
                    }
                }
            }

            filePath = CheckSaveFile("UWFI");
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    illegalUWFIMoves.Clear();
                    dqUWFIMoves.Clear();
                    var lines = File.ReadAllLines(filePath);
                    bool isDq = false;
                    foreach (String move in lines)
                    {
                        if (move.Equals(sectionDivider))
                        {
                            isDq = true;
                            continue;
                        }

                        if (!isDq)
                        {
                            illegalUWFIMoves.Add(move);
                        }
                        else
                        {
                            dqUWFIMoves.Add(move);
                        }
                    }
                }
            }

            filePath = CheckSaveFile("Pancrase");
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    illegalPancraseMoves.Clear();
                    dqPancraseMoves.Clear();
                    var lines = File.ReadAllLines(filePath);
                    bool isDq = false;
                    foreach (String move in lines)
                    {
                        if (move.Equals(sectionDivider))
                        {
                            isDq = true;
                            continue;
                        }

                        if (!isDq)
                        {
                            illegalPancraseMoves.Add(move);
                        }
                        else
                        {
                            dqPancraseMoves.Add(move);
                        }
                    }
                }
            }

            filePath = CheckSaveFile("Boxing");
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    dqBoxingMoves.Clear();
                    var lines = File.ReadAllLines(filePath);
                    foreach (String move in lines)
                    {
                        dqBoxingMoves.Add(move);
                    }
                }
            }

            filePath = CheckSaveFile("KickBoxing");
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    dqKickboxingMoves.Clear();
                    var lines = File.ReadAllLines(filePath);
                    foreach (String move in lines)
                    {
                        dqKickboxingMoves.Add(move);
                    }
                }
            }

        }

        #endregion

        #region General Window Methods
        private void MoreMatchTypes_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveMoves();
        }
        private void tb_basic_LostFocus(object sender, EventArgs e)
        {
            UpdateMoveList("Sumo");
        }
        private void tb_illegal_LostFocus(object sender, EventArgs e)
        {
            if (cb_uwfi.Checked)
            {
                UpdateMoveList("UWFI");
            }
            else if (cb_Pancrase.Checked)
            {
                UpdateMoveList("Pancrase");
            }
        }
        private void tb_dq_LostFocus(object sender, EventArgs e)
        {
            if (cb_uwfi.Checked)
            {
                UpdateMoveList("UWFI");
            }
            else if (cb_Pancrase.Checked)
            {
                UpdateMoveList("Pancrase");
            }
            else if (cb_boxing.Checked)
            {
                UpdateMoveList("Boxing");
            }
            else if (cb_kickboxing.Checked)
            {
                UpdateMoveList("Kickboxing");
            }

        }
        private void matchHelp_Click(object sender, EventArgs e)
        {
            if (cb_normalMatch.Checked)
            {
                MessageBox.Show("Run a normal match, without special rules.", "Normal Match", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Two teams battle in a traditional sumo match.\nThis match type only works with two fighters (no partners, no seconds).\nThe first fighter to fall to the mat loses.\nBasic Attacks determine which moves can be attempted, otherwise the clinch animation will be used.", "Sumo Match", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (cb_ttt.Checked)
            {
                MessageBox.Show("Take place in a timed tornado tag battle, where players join over the course of a match.\nThe first player joins after five minutes, then the next joins every two minutes afterwards.\nWhen all players have joined the match, pinfall or submission victories are possible.\nThe first team to score a victory wins the match.", "Timed Tornado Tag", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //Boxing
            if (cb_boxing.Checked)
            {
                MessageBox.Show("Boxing rules, with additional features for the referee and participants.", "Boxing Match", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //Kick Boxing
            if (cb_kickboxing.Checked)
            {
                MessageBox.Show("Kick-Boxing rules, with additional features for the referee and participants.", "Kick-Boxing Match", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //Lucha Tag
            if (cb_luchaTag.Checked)
            {
                MessageBox.Show("Lucha tag match, where players can enter the ring without tags (under certain conditions).", "Lucha Tag Match", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void isPost_CheckedChanged(object sender, EventArgs e)
        {
            if (removePosts.Checked)
            {
                removeRopes.Checked = true;
            }
        }
        private void matchConfig_Click(object sender, EventArgs e)
        {
            try
            {
                if (cb_exElim.Checked)
                {
                    if (SurvivalRoadData != null)
                    {
                        if (SurvivalRoadData.InProgress)
                        {
                            MessageBox.Show("Cannot set-up an Extended Elimination Match when a Survival Road session is in progress.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    if (EliminationForm.eliminationForm == null)
                    {
                        EliminationForm popUp = new EliminationForm();
                        popUp.Show();
                    }
                    else
                    {
                        EliminationForm.eliminationForm.Show();
                        EliminationForm.eliminationForm.Focus();
                    }
                }
                else if (cb_survival.Checked)
                {
                    if (SurvivalRoadForm.survivalForm == null)
                    {
                        SurvivalRoadForm popUp = new SurvivalRoadForm();
                        popUp.Show();
                    }
                    else
                    {
                        SurvivalRoadForm.survivalForm.Show();
                        SurvivalRoadForm.survivalForm.Focus();
                    }
                }
            }
            catch (Exception exception)
            {
                L.D("MatchConfigError: " + exception);
            }

        }
        private void tb_illegal_TextChanged(object sender, EventArgs e)
        {

        }

        #region Check Boxes
        private void cb_FirstBlood_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
            UncheckNormal();
            UncheckShoot();
        }
        private void cb_IronManMatch_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
            UncheckNormal();
            UncheckShoot();
        }
        private void cb_sumo_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
            UncheckNormal();
            UncheckWrestling();
            lbl_Basic.Visible = true;
            tb_basic.Visible = true;
            tb_basic.Clear();

            String moveList = "";
            foreach (String move in legalSumoMoves)
            {
                moveList += move + "\n";
            }
            tb_basic.Text = moveList;
            rulesTabControl.TabPages[0].Text = "Attack Rules (Sumo)";

            removePosts.Checked = true;
            removeRopes.Checked = true;
        }
        private void cb_uwfi_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
            UncheckNormal();
            UncheckWrestling();
            lbl_illegal.Visible = true;
            lbl_dq.Visible = true;
            tb_illegal.Visible = true;
            tb_dq.Visible = true;


            //Illegal Moves
            tb_illegal.Text = "";
            foreach (String move in illegalUWFIMoves)
            {
                tb_illegal.Text += move + "\n";
            }

            //DQ Moves
            tb_dq.Text = "";
            foreach (String move in dqUWFIMoves)
            {
                tb_dq.Text += move + "\n";
            }

            rulesTabControl.TabPages[0].Text = "Attack Rules (UWFI)";

            isAutoKo.Checked = true;
        }
        private void cb_Pancrase_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
            UncheckNormal();
            UncheckWrestling();
            pancraseLostPerDown.Visible = true;
            pancraseTotalPoints.Visible = true;
            downPointsLbl.Visible = true;
            totalPointsLbl.Visible = true;
            lbl_illegal.Visible = true;
            lbl_dq.Visible = true;
            tb_illegal.Visible = true;
            tb_dq.Visible = true;

            //Illegal Moves
            tb_illegal.Text = "";
            foreach (String move in illegalPancraseMoves)
            {
                tb_illegal.Text += move + "\n";
            }

            //DQ Moves
            tb_dq.Text = "";
            foreach (String move in dqPancraseMoves)
            {
                tb_dq.Text += move + "\n";
            }

            rulesTabControl.TabPages[0].Text = "Attack Rules (Pancrase)";

            isAutoKo.Checked = true;
        }
        private void cb_normalMatch_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
            UncheckShoot();
            UncheckWrestling();
        }
        private void cb_elimination_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
            UncheckNormal();
            UncheckShoot();
        }
        private void cb_boxing_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
            UncheckNormal();
            UncheckWrestling();
            lbl_dq.Visible = true;
            tb_dq.Visible = true;

            foreach (String move in dqBoxingMoves)
            {
                tb_dq.Text += move + "\n";
            }

            rulesTabControl.TabPages[0].Text = "Attack Rules (Boxing)";

            isAutoKo.Checked = true;
        }
        private void cb_kickboxing_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
            UncheckNormal();
            UncheckWrestling();
            lbl_dq.Visible = true;
            tb_dq.Visible = true;

            foreach (String move in dqKickboxingMoves)
            {
                tb_dq.Text += move + "\n";
            }

            rulesTabControl.TabPages[0].Text = "Attack Rules (Kick Boxing)";

            isAutoKo.Checked = true;
        }
        private void cb_ttt_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
            UncheckNormal();
            UncheckShoot();
        }
        private void cb_luchaTag_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
            UncheckNormal();
            UncheckShoot();
        }
        private void cb_exElim_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
            UncheckNormal();
            UncheckShoot();
            matchConfig.Visible = true;
        }
        private void cb_survival_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
            UncheckNormal();
            UncheckShoot();
            matchConfig.Visible = true;
        }

        #endregion

        #endregion

        #region Move Methods
        private void UpdateMoves()
        {
            List<String> dqMoves = new List<String>()
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
            if (legalSumoMoves.Count == 0)
            {
                legalSumoMoves = new List<String> { "Shoutei", "Face Slap B", "Chest Slap", "Knife - Edge Chop", "Koppo Style Shoutei", "Throat Chop", "Jigoku - Tsuki", "Elbow Butt" };
            }

            if (illegalPancraseMoves.Count == 0)
            {
                illegalPancraseMoves = illegalMoves;
            }

            if (dqPancraseMoves.Count == 0)
            {
                dqPancraseMoves = dqMoves;

                if (illegalUWFIMoves.Count == 0)
                {
                    illegalUWFIMoves = illegalMoves;
                }

                if (dqUWFIMoves.Count == 0)
                {
                    dqUWFIMoves = dqMoves;
                }

                if (dqBoxingMoves.Count == 0)
                {
                    dqBoxingMoves = dqMoves;
                }

                if (dqKickboxingMoves.Count == 0)
                {
                    dqKickboxingMoves = dqMoves;
                }

            }
        }

        #endregion

        #region Helper Methods
        private void UpdateMoveList(String matchType)
        {
            switch (matchType)
            {
                case "Sumo":
                    legalSumoMoves = tb_basic.Text.Split('\n').ToList();
                    break;
                case "Pancrase":
                    illegalPancraseMoves = tb_illegal.Text.Split('\n').ToList();
                    dqPancraseMoves = tb_dq.Text.Split('\n').ToList();
                    break;
                case "UWFI":
                    illegalUWFIMoves = tb_illegal.Text.Split('\n').ToList();
                    dqUWFIMoves = tb_dq.Text.Split('\n').ToList();
                    break;
                case "Boxing":
                    dqBoxingMoves = tb_dq.Text.Split('\n').ToList();
                    break;
                case "Kickboxing":
                    dqKickboxingMoves = tb_dq.Text.Split('\n').ToList();
                    break;
                default:
                    break;
            }
        }
        private void Clear()
        {
            //Hiding all customization items
            rulesTabControl.TabPages[0].Text = "Attack Rules (Not Available)";
            lbl_Basic.Visible = false;
            lbl_dq.Visible = false;
            lbl_illegal.Visible = false;
            tb_basic.Visible = false;
            tb_basic.Text = "";
            tb_illegal.Visible = false;
            tb_illegal.Text = "";
            tb_dq.Visible = false;
            tb_dq.Text = "";
            matchConfig.Visible = false;
            isAutoKo.Checked = false;
            removeRopes.Checked = false;
            removePosts.Checked = false;
            HideAllOptions();
            ShowOptions();
        }
        private void HideAllOptions()
        {
            cb_luchaFalls.Visible = false;
            cb_losersLeave.Visible = false;
        }
        private void ShowOptions()
        {
            if (cb_elimination.Checked)
            {
                cb_losersLeave.Visible = true;
            }
            
            if (cb_luchaTag.Checked)
            {
                cb_luchaFalls.Visible = true;
            }
        }
        private void UncheckNormal()
        {
            cb_normalMatch.Checked = false;
        }
        private void UncheckShoot()
        {
            cb_boxing.Checked = false;
            cb_kickboxing.Checked = false;
            cb_Pancrase.Checked = false;
            cb_uwfi.Checked = false;
            cb_sumo.Checked = false;
            isk1mma.Checked = false;
            pancraseLostPerDown.Visible = false;
            pancraseTotalPoints.Visible = false;
            downPointsLbl.Visible = false;
            totalPointsLbl.Visible = false;
        }
        private void UncheckWrestling()
        {
            cb_IronManMatch.Checked = false;
            cb_FirstBlood.Checked = false;
            cb_ttt.Checked = false;
            cb_luchaTag.Checked = false;
            cb_elimination.Checked = false;
            cb_exElim.Checked = false;
            cb_survival.Checked = false;

        }
        private bool CheckModPackLoaded()
        {
            bool exists = false;

            try
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    string name = assembly.GetName().Name;
                    L.D("Assembly:" + name);
                    if (name.Equals(modPackName))
                    {
                        exists = true;
                        break;
                    }
                }

            }
            catch (Exception e)
            {
                L.D("CheckModPackLoadedError: " + e);
            }

            return exists;

        }

        public void ResetModOptions()
        {
            if (cb_luchaTag.Checked || cb_elimination.Checked || cb_ttt.Checked)
            {
                UncheckWrestling();
                cb_normalMatch.Checked = true;
            }
        }
        #endregion

    }
}
