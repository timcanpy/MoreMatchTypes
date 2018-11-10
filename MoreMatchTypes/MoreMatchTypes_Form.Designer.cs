namespace MoreMatchTypes
{
    partial class MoreMatchTypes_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cb_FirstBlood = new System.Windows.Forms.RadioButton();
            this.cb_IronManMatch = new System.Windows.Forms.RadioButton();
            this.cb_uwfi = new System.Windows.Forms.RadioButton();
            this.cb_normalMatch = new System.Windows.Forms.RadioButton();
            this.cb_Pancrase = new System.Windows.Forms.RadioButton();
            this.tb_basic = new System.Windows.Forms.TextBox();
            this.tb_illegal = new System.Windows.Forms.TextBox();
            this.tb_dq = new System.Windows.Forms.TextBox();
            this.lbl_Basic = new System.Windows.Forms.Label();
            this.lbl_illegal = new System.Windows.Forms.Label();
            this.lbl_dq = new System.Windows.Forms.Label();
            this.cb_elimination = new System.Windows.Forms.RadioButton();
            this.rulesTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cb_losersLeave = new System.Windows.Forms.CheckBox();
            this.cb_membersWait = new System.Windows.Forms.CheckBox();
            this.matchHelp = new System.Windows.Forms.Button();
            this.tt_normal = new System.Windows.Forms.ToolTip(this.components);
            this.sr_singleOpponent = new System.Windows.Forms.Label();
            this.sr_tagOpponent = new System.Windows.Forms.Label();
            this.cb_ttt = new System.Windows.Forms.RadioButton();
            this.cb_boxing = new System.Windows.Forms.RadioButton();
            this.cb_kickboxing = new System.Windows.Forms.RadioButton();
            this.rulesTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cb_FirstBlood
            // 
            this.cb_FirstBlood.AutoSize = true;
            this.cb_FirstBlood.Location = new System.Drawing.Point(23, 61);
            this.cb_FirstBlood.Name = "cb_FirstBlood";
            this.cb_FirstBlood.Size = new System.Drawing.Size(74, 17);
            this.cb_FirstBlood.TabIndex = 0;
            this.cb_FirstBlood.Text = "First Blood";
            this.cb_FirstBlood.UseVisualStyleBackColor = true;
            // 
            // cb_IronManMatch
            // 
            this.cb_IronManMatch.AutoSize = true;
            this.cb_IronManMatch.Location = new System.Drawing.Point(23, 38);
            this.cb_IronManMatch.Name = "cb_IronManMatch";
            this.cb_IronManMatch.Size = new System.Drawing.Size(67, 17);
            this.cb_IronManMatch.TabIndex = 1;
            this.cb_IronManMatch.Text = "Iron Man";
            this.cb_IronManMatch.UseVisualStyleBackColor = true;
            this.cb_IronManMatch.CheckedChanged += new System.EventHandler(this.cb_IronManMatch_CheckedChanged);
            // 
            // cb_uwfi
            // 
            this.cb_uwfi.AutoSize = true;
            this.cb_uwfi.Location = new System.Drawing.Point(185, 38);
            this.cb_uwfi.Name = "cb_uwfi";
            this.cb_uwfi.Size = new System.Drawing.Size(53, 17);
            this.cb_uwfi.TabIndex = 2;
            this.cb_uwfi.Text = "UWFI";
            this.cb_uwfi.UseVisualStyleBackColor = true;
            this.cb_uwfi.CheckedChanged += new System.EventHandler(this.cb_uwfi_CheckedChanged);
            // 
            // cb_normalMatch
            // 
            this.cb_normalMatch.AutoSize = true;
            this.cb_normalMatch.Checked = true;
            this.cb_normalMatch.Location = new System.Drawing.Point(23, 12);
            this.cb_normalMatch.Name = "cb_normalMatch";
            this.cb_normalMatch.Size = new System.Drawing.Size(91, 17);
            this.cb_normalMatch.TabIndex = 4;
            this.cb_normalMatch.TabStop = true;
            this.cb_normalMatch.Text = "Normal Match";
            this.cb_normalMatch.UseVisualStyleBackColor = true;
            this.cb_normalMatch.CheckedChanged += new System.EventHandler(this.cb_normalMatch_CheckedChanged);
            // 
            // cb_Pancrase
            // 
            this.cb_Pancrase.AutoSize = true;
            this.cb_Pancrase.Location = new System.Drawing.Point(185, 61);
            this.cb_Pancrase.Name = "cb_Pancrase";
            this.cb_Pancrase.Size = new System.Drawing.Size(70, 17);
            this.cb_Pancrase.TabIndex = 5;
            this.cb_Pancrase.TabStop = true;
            this.cb_Pancrase.Text = "Pancrase";
            this.cb_Pancrase.UseVisualStyleBackColor = true;
            this.cb_Pancrase.CheckedChanged += new System.EventHandler(this.cb_Pancrase_CheckedChanged);
            // 
            // tb_basic
            // 
            this.tb_basic.Location = new System.Drawing.Point(17, 37);
            this.tb_basic.Multiline = true;
            this.tb_basic.Name = "tb_basic";
            this.tb_basic.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_basic.Size = new System.Drawing.Size(236, 356);
            this.tb_basic.TabIndex = 6;
            this.tb_basic.Visible = false;
            // 
            // tb_illegal
            // 
            this.tb_illegal.Location = new System.Drawing.Point(282, 37);
            this.tb_illegal.Multiline = true;
            this.tb_illegal.Name = "tb_illegal";
            this.tb_illegal.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_illegal.Size = new System.Drawing.Size(280, 357);
            this.tb_illegal.TabIndex = 7;
            this.tb_illegal.Visible = false;
            this.tb_illegal.TextChanged += new System.EventHandler(this.tb_illegal_TextChanged);
            // 
            // tb_dq
            // 
            this.tb_dq.Location = new System.Drawing.Point(583, 37);
            this.tb_dq.Multiline = true;
            this.tb_dq.Name = "tb_dq";
            this.tb_dq.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_dq.Size = new System.Drawing.Size(231, 357);
            this.tb_dq.TabIndex = 8;
            this.tb_dq.Visible = false;
            // 
            // lbl_Basic
            // 
            this.lbl_Basic.AutoSize = true;
            this.lbl_Basic.Location = new System.Drawing.Point(15, 17);
            this.lbl_Basic.Name = "lbl_Basic";
            this.lbl_Basic.Size = new System.Drawing.Size(72, 13);
            this.lbl_Basic.TabIndex = 9;
            this.lbl_Basic.Text = "Basic Attacks";
            this.lbl_Basic.Visible = false;
            // 
            // lbl_illegal
            // 
            this.lbl_illegal.AutoSize = true;
            this.lbl_illegal.Location = new System.Drawing.Point(279, 17);
            this.lbl_illegal.Name = "lbl_illegal";
            this.lbl_illegal.Size = new System.Drawing.Size(73, 13);
            this.lbl_illegal.TabIndex = 10;
            this.lbl_illegal.Text = "Illegal Attacks";
            this.lbl_illegal.Visible = false;
            // 
            // lbl_dq
            // 
            this.lbl_dq.AutoSize = true;
            this.lbl_dq.Location = new System.Drawing.Point(580, 17);
            this.lbl_dq.Name = "lbl_dq";
            this.lbl_dq.Size = new System.Drawing.Size(62, 13);
            this.lbl_dq.TabIndex = 11;
            this.lbl_dq.Text = "DQ Attacks";
            this.lbl_dq.Visible = false;
            // 
            // cb_elimination
            // 
            this.cb_elimination.AutoSize = true;
            this.cb_elimination.Location = new System.Drawing.Point(572, 59);
            this.cb_elimination.Name = "cb_elimination";
            this.cb_elimination.Size = new System.Drawing.Size(111, 17);
            this.cb_elimination.TabIndex = 12;
            this.cb_elimination.TabStop = true;
            this.cb_elimination.Text = "Limited Elimination";
            this.cb_elimination.UseVisualStyleBackColor = true;
            this.cb_elimination.Visible = false;
            this.cb_elimination.CheckedChanged += new System.EventHandler(this.cb_elimination_CheckedChanged);
            // 
            // rulesTabControl
            // 
            this.rulesTabControl.Controls.Add(this.tabPage1);
            this.rulesTabControl.Location = new System.Drawing.Point(1, 145);
            this.rulesTabControl.Name = "rulesTabControl";
            this.rulesTabControl.SelectedIndex = 0;
            this.rulesTabControl.Size = new System.Drawing.Size(849, 472);
            this.rulesTabControl.TabIndex = 13;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lbl_Basic);
            this.tabPage1.Controls.Add(this.tb_basic);
            this.tabPage1.Controls.Add(this.tb_illegal);
            this.tabPage1.Controls.Add(this.lbl_illegal);
            this.tabPage1.Controls.Add(this.lbl_dq);
            this.tabPage1.Controls.Add(this.tb_dq);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(841, 446);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Attack Rules (UWFI, Pancrase, Boxing)";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // cb_losersLeave
            // 
            this.cb_losersLeave.AutoSize = true;
            this.cb_losersLeave.Checked = true;
            this.cb_losersLeave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_losersLeave.Enabled = false;
            this.cb_losersLeave.Location = new System.Drawing.Point(572, 36);
            this.cb_losersLeave.Name = "cb_losersLeave";
            this.cb_losersLeave.Size = new System.Drawing.Size(140, 17);
            this.cb_losersLeave.TabIndex = 3;
            this.cb_losersLeave.Text = "Losers Leave Ringside?";
            this.cb_losersLeave.UseVisualStyleBackColor = true;
            this.cb_losersLeave.Visible = false;
            // 
            // cb_membersWait
            // 
            this.cb_membersWait.AutoSize = true;
            this.cb_membersWait.Checked = true;
            this.cb_membersWait.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_membersWait.Enabled = false;
            this.cb_membersWait.Location = new System.Drawing.Point(572, 13);
            this.cb_membersWait.Name = "cb_membersWait";
            this.cb_membersWait.Size = new System.Drawing.Size(157, 17);
            this.cb_membersWait.TabIndex = 2;
            this.cb_membersWait.Text = "Members Wait At Ringside?";
            this.cb_membersWait.UseVisualStyleBackColor = true;
            this.cb_membersWait.Visible = false;
            // 
            // matchHelp
            // 
            this.matchHelp.Font = new System.Drawing.Font("Impact", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.matchHelp.Location = new System.Drawing.Point(144, 4);
            this.matchHelp.Name = "matchHelp";
            this.matchHelp.Size = new System.Drawing.Size(41, 25);
            this.matchHelp.TabIndex = 16;
            this.matchHelp.Text = "Help";
            this.matchHelp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.matchHelp.UseVisualStyleBackColor = true;
            this.matchHelp.Click += new System.EventHandler(this.matchHelp_Click);
            // 
            // tt_normal
            // 
            this.tt_normal.IsBalloon = true;
            this.tt_normal.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.tt_normal.ToolTipTitle = "Normal Match Info";
            // 
            // sr_singleOpponent
            // 
            this.sr_singleOpponent.AutoSize = true;
            this.sr_singleOpponent.Location = new System.Drawing.Point(618, 86);
            this.sr_singleOpponent.Name = "sr_singleOpponent";
            this.sr_singleOpponent.Size = new System.Drawing.Size(86, 13);
            this.sr_singleOpponent.TabIndex = 17;
            this.sr_singleOpponent.Text = "Single Opponent";
            this.sr_singleOpponent.Visible = false;
            // 
            // sr_tagOpponent
            // 
            this.sr_tagOpponent.AutoSize = true;
            this.sr_tagOpponent.Location = new System.Drawing.Point(618, 108);
            this.sr_tagOpponent.Name = "sr_tagOpponent";
            this.sr_tagOpponent.Size = new System.Drawing.Size(76, 13);
            this.sr_tagOpponent.TabIndex = 18;
            this.sr_tagOpponent.Text = "Tag Opponent";
            this.sr_tagOpponent.Visible = false;
            // 
            // cb_ttt
            // 
            this.cb_ttt.AutoSize = true;
            this.cb_ttt.Location = new System.Drawing.Point(23, 84);
            this.cb_ttt.Name = "cb_ttt";
            this.cb_ttt.Size = new System.Drawing.Size(119, 17);
            this.cb_ttt.TabIndex = 19;
            this.cb_ttt.TabStop = true;
            this.cb_ttt.Text = "Timed Tornado Tag";
            this.cb_ttt.UseVisualStyleBackColor = true;
            // 
            // cb_boxing
            // 
            this.cb_boxing.AutoSize = true;
            this.cb_boxing.Location = new System.Drawing.Point(185, 86);
            this.cb_boxing.Name = "cb_boxing";
            this.cb_boxing.Size = new System.Drawing.Size(57, 17);
            this.cb_boxing.TabIndex = 21;
            this.cb_boxing.TabStop = true;
            this.cb_boxing.Text = "Boxing";
            this.cb_boxing.UseVisualStyleBackColor = true;
            this.cb_boxing.CheckedChanged += new System.EventHandler(this.cb_boxing_CheckedChanged);
            // 
            // cb_kickboxing
            // 
            this.cb_kickboxing.AutoSize = true;
            this.cb_kickboxing.Location = new System.Drawing.Point(185, 110);
            this.cb_kickboxing.Name = "cb_kickboxing";
            this.cb_kickboxing.Size = new System.Drawing.Size(81, 17);
            this.cb_kickboxing.TabIndex = 22;
            this.cb_kickboxing.TabStop = true;
            this.cb_kickboxing.Text = "Kick Boxing";
            this.cb_kickboxing.UseVisualStyleBackColor = true;
            this.cb_kickboxing.CheckedChanged += new System.EventHandler(this.cb_kickboxing_CheckedChanged);
            // 
            // MoreMatchTypes_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 620);
            this.Controls.Add(this.cb_kickboxing);
            this.Controls.Add(this.cb_boxing);
            this.Controls.Add(this.cb_ttt);
            this.Controls.Add(this.sr_tagOpponent);
            this.Controls.Add(this.sr_singleOpponent);
            this.Controls.Add(this.matchHelp);
            this.Controls.Add(this.rulesTabControl);
            this.Controls.Add(this.cb_elimination);
            this.Controls.Add(this.cb_Pancrase);
            this.Controls.Add(this.cb_normalMatch);
            this.Controls.Add(this.cb_losersLeave);
            this.Controls.Add(this.cb_membersWait);
            this.Controls.Add(this.cb_uwfi);
            this.Controls.Add(this.cb_IronManMatch);
            this.Controls.Add(this.cb_FirstBlood);
            this.Name = "MoreMatchTypes_Form";
            this.Text = "More Match Types";
            this.Load += new System.EventHandler(this.MoreMatchTypes_Form_Load);
            this.rulesTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.RadioButton cb_FirstBlood;
        public System.Windows.Forms.RadioButton cb_IronManMatch;
        public System.Windows.Forms.RadioButton cb_uwfi;
        private System.Windows.Forms.RadioButton cb_normalMatch;
        public System.Windows.Forms.RadioButton cb_Pancrase;
        public System.Windows.Forms.TextBox tb_basic;
        public System.Windows.Forms.TextBox tb_illegal;
        public System.Windows.Forms.TextBox tb_dq;
        public System.Windows.Forms.Label lbl_Basic;
        public System.Windows.Forms.Label lbl_illegal;
        public System.Windows.Forms.Label lbl_dq;
        private System.Windows.Forms.TabPage tabPage1;
        public System.Windows.Forms.RadioButton cb_elimination;
        public System.Windows.Forms.CheckBox cb_membersWait;
        public System.Windows.Forms.CheckBox cb_losersLeave;
        public System.Windows.Forms.TabControl rulesTabControl;
        public System.Windows.Forms.ToolTip tt_normal;
        public System.Windows.Forms.Button matchHelp;
        public System.Windows.Forms.Label sr_singleOpponent;
        public System.Windows.Forms.Label sr_tagOpponent;
        public System.Windows.Forms.RadioButton cb_ttt;
        public System.Windows.Forms.RadioButton cb_boxing;
        public System.Windows.Forms.RadioButton cb_kickboxing;
    }
}