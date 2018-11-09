using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DG;
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
        private static String[] saveFileNames = new String[] { "SumoMoves.dat", "UWFIMoves.dat", "PancraseMoves.dat", "BoxingMoves.dat", "KickBoxingMoves.dat" };
        private static String[] saveFolderNames = new String[] { "./MatchTypeData/" };
        private static String sectionDivider = "|-------------------|";

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

        public MoreMatchTypes_Form()
        {
            form = this;
            InitializeComponent();
            FormClosing += MoreMatchTypes_FormClosing;
            tb_basic.LostFocus += tb_basic_LostFocus;
            tb_illegal.LostFocus += tb_illegal_LostFocus;
            tb_dq.LostFocus += tb_dq_LostFocus;
            rulesTabControl.TabPages.RemoveAt(2);
            rulesTabControl.TabPages.RemoveAt(1);
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

        #region Data Save
        private void SaveMoves()
        {
            //Pancrase Moves
            String filePath = CheckSaveFile("Pancrase");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (StreamWriter sw = File.AppendText(filePath))
            {
                foreach (String move in illegalPancraseMoves)
                {
                    if (!move.Equals("\n") && !move.Equals(""))
                    {
                        sw.WriteLine(move);
                    }
                }

                sw.WriteLine(sectionDivider);

                foreach (String move in dqPancraseMoves)
                {
                    if (!move.Equals("\n") && !move.Equals(""))
                    {
                        sw.WriteLine(move);
                    }
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
                    if (!move.Equals("\n") && !move.Equals(""))
                    {
                        sw.WriteLine(move);
                    }
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
                    if (!move.Equals("\n") && !move.Equals(""))
                    {
                        sw.WriteLine(move);
                    }
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



        private void MoreMatchTypes_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveMoves();
        }

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
            if (cb_uwfi.Checked)
            {
                MessageBox.Show("Take part in a classic UWFI style match.\nTeams have a certain number of points that are reduced over the course of a match.\nThe first team to run out of points, or submit to the opponent is the loser.\nIllegal Attacks determine which moves will cost points based on legality.\nDQ Attacks determine which moves will cause a team to lose immediately.", "UWFI Match", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (cb_Pancrase.Checked)
            {
                MessageBox.Show("Take part in a classic Pancrase style match.\nThis match type only works with two fighters (no partners, no seconds).\nEach fighter has a certain number of points that are reduced over the course of a match.\nThe first fighter to run out of points, or submit to the opponent is the loser.\nIllegal Attacks determine which moves will cost points based on legality.\nDQ Attacks determine which moves will cause a team to lose immediately.", "Pancrase Match", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            tb_basic.Clear();

            String moveList = "";
            foreach (String move in legalSumoMoves)
            {
                moveList += move + "\n";
            }
            tb_basic.Text = moveList;
            rulesTabControl.SelectedIndex = 0;
        }

        private void cb_uwfi_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
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

        private void cb_boxing_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
            lbl_dq.Visible = true;
            tb_dq.Visible = true;

            foreach (String move in dqBoxingMoves)
            {
                tb_dq.Text += move + "\n";
            }

            rulesTabControl.SelectedIndex = 0;
        }

        private void cb_kickboxing_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
            lbl_dq.Visible = true;
            tb_dq.Visible = true;

            foreach (String move in dqKickboxingMoves)
            {
                tb_dq.Text += move + "\n";
            }

            rulesTabControl.SelectedIndex = 0;
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

        #region Shared Execution Methods

        private void StartMatch(String matchType)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Match");
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

        #endregion

        #region Boxing Methods


        #endregion
    }
}
