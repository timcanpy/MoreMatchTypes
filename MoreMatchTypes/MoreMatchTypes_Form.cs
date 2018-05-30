using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MoreMatchTypes
{
    public partial class MoreMatchTypes_Form : Form
    {
        public static MoreMatchTypes_Form form = null;

        public MoreMatchTypes_Form()
        {
            form = this;
            InitializeComponent();
        }


        public void MoreMatchTypes_Form_Load(object sender, EventArgs e)
        {
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
    }
}
