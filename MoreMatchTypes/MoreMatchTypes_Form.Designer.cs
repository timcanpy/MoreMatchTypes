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
            this.rulesTab = new System.Windows.Forms.TabPage();
            this.cb_losersLeave = new System.Windows.Forms.CheckBox();
            this.cb_membersWait = new System.Windows.Forms.CheckBox();
            this.matchHelp = new System.Windows.Forms.Button();
            this.tt_normal = new System.Windows.Forms.ToolTip(this.components);
            this.cb_ttt = new System.Windows.Forms.RadioButton();
            this.cb_boxing = new System.Windows.Forms.RadioButton();
            this.cb_kickboxing = new System.Windows.Forms.RadioButton();
            this.removePosts = new System.Windows.Forms.CheckBox();
            this.removeRopes = new System.Windows.Forms.CheckBox();
            this.isk1mma = new System.Windows.Forms.RadioButton();
            this.cb_luchaTag = new System.Windows.Forms.RadioButton();
            this.isAutoKo = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cb_survival = new System.Windows.Forms.RadioButton();
            this.cb_exElim = new System.Windows.Forms.RadioButton();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.cb_sumo = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cb_luchaFalls = new System.Windows.Forms.CheckBox();
            this.matchConfig = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.rulesTabControl.SuspendLayout();
            this.rulesTab.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cb_FirstBlood
            // 
            this.cb_FirstBlood.AutoSize = true;
            this.cb_FirstBlood.Location = new System.Drawing.Point(7, 56);
            this.cb_FirstBlood.Name = "cb_FirstBlood";
            this.cb_FirstBlood.Size = new System.Drawing.Size(80, 19);
            this.cb_FirstBlood.TabIndex = 0;
            this.cb_FirstBlood.Text = "First Blood";
            this.cb_FirstBlood.UseVisualStyleBackColor = true;
            this.cb_FirstBlood.CheckedChanged += new System.EventHandler(this.cb_FirstBlood_CheckedChanged);
            // 
            // cb_IronManMatch
            // 
            this.cb_IronManMatch.AutoSize = true;
            this.cb_IronManMatch.Location = new System.Drawing.Point(139, 56);
            this.cb_IronManMatch.Name = "cb_IronManMatch";
            this.cb_IronManMatch.Size = new System.Drawing.Size(67, 19);
            this.cb_IronManMatch.TabIndex = 1;
            this.cb_IronManMatch.Text = "Iron Man";
            this.cb_IronManMatch.UseVisualStyleBackColor = true;
            this.cb_IronManMatch.CheckedChanged += new System.EventHandler(this.cb_IronManMatch_CheckedChanged);
            // 
            // cb_uwfi
            // 
            this.cb_uwfi.AutoSize = true;
            this.cb_uwfi.Location = new System.Drawing.Point(199, 10);
            this.cb_uwfi.Name = "cb_uwfi";
            this.cb_uwfi.Size = new System.Drawing.Size(55, 19);
            this.cb_uwfi.TabIndex = 2;
            this.cb_uwfi.Text = "UWFI";
            this.cb_uwfi.UseVisualStyleBackColor = true;
            this.cb_uwfi.CheckedChanged += new System.EventHandler(this.cb_uwfi_CheckedChanged);
            // 
            // cb_normalMatch
            // 
            this.cb_normalMatch.AutoSize = true;
            this.cb_normalMatch.Checked = true;
            this.cb_normalMatch.Location = new System.Drawing.Point(10, 14);
            this.cb_normalMatch.Name = "cb_normalMatch";
            this.cb_normalMatch.Size = new System.Drawing.Size(93, 19);
            this.cb_normalMatch.TabIndex = 4;
            this.cb_normalMatch.TabStop = true;
            this.cb_normalMatch.Text = "Normal Match";
            this.cb_normalMatch.UseVisualStyleBackColor = true;
            this.cb_normalMatch.CheckedChanged += new System.EventHandler(this.cb_normalMatch_CheckedChanged);
            // 
            // cb_Pancrase
            // 
            this.cb_Pancrase.AutoSize = true;
            this.cb_Pancrase.Location = new System.Drawing.Point(112, 35);
            this.cb_Pancrase.Name = "cb_Pancrase";
            this.cb_Pancrase.Size = new System.Drawing.Size(71, 19);
            this.cb_Pancrase.TabIndex = 5;
            this.cb_Pancrase.Text = "Pancrase";
            this.cb_Pancrase.UseVisualStyleBackColor = true;
            this.cb_Pancrase.CheckedChanged += new System.EventHandler(this.cb_Pancrase_CheckedChanged);
            // 
            // tb_basic
            // 
            this.tb_basic.Location = new System.Drawing.Point(20, 43);
            this.tb_basic.Multiline = true;
            this.tb_basic.Name = "tb_basic";
            this.tb_basic.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_basic.Size = new System.Drawing.Size(275, 410);
            this.tb_basic.TabIndex = 6;
            this.tb_basic.Visible = false;
            // 
            // tb_illegal
            // 
            this.tb_illegal.Location = new System.Drawing.Point(329, 43);
            this.tb_illegal.Multiline = true;
            this.tb_illegal.Name = "tb_illegal";
            this.tb_illegal.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_illegal.Size = new System.Drawing.Size(326, 411);
            this.tb_illegal.TabIndex = 7;
            this.tb_illegal.Visible = false;
            this.tb_illegal.TextChanged += new System.EventHandler(this.tb_illegal_TextChanged);
            // 
            // tb_dq
            // 
            this.tb_dq.Location = new System.Drawing.Point(680, 43);
            this.tb_dq.Multiline = true;
            this.tb_dq.Name = "tb_dq";
            this.tb_dq.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_dq.Size = new System.Drawing.Size(269, 411);
            this.tb_dq.TabIndex = 8;
            this.tb_dq.Visible = false;
            // 
            // lbl_Basic
            // 
            this.lbl_Basic.AutoSize = true;
            this.lbl_Basic.Location = new System.Drawing.Point(17, 20);
            this.lbl_Basic.Name = "lbl_Basic";
            this.lbl_Basic.Size = new System.Drawing.Size(78, 15);
            this.lbl_Basic.TabIndex = 9;
            this.lbl_Basic.Text = "Basic Attacks";
            this.lbl_Basic.Visible = false;
            // 
            // lbl_illegal
            // 
            this.lbl_illegal.AutoSize = true;
            this.lbl_illegal.Location = new System.Drawing.Point(325, 20);
            this.lbl_illegal.Name = "lbl_illegal";
            this.lbl_illegal.Size = new System.Drawing.Size(79, 15);
            this.lbl_illegal.TabIndex = 10;
            this.lbl_illegal.Text = "Illegal Attacks";
            this.lbl_illegal.Visible = false;
            // 
            // lbl_dq
            // 
            this.lbl_dq.AutoSize = true;
            this.lbl_dq.Location = new System.Drawing.Point(677, 20);
            this.lbl_dq.Name = "lbl_dq";
            this.lbl_dq.Size = new System.Drawing.Size(66, 15);
            this.lbl_dq.TabIndex = 11;
            this.lbl_dq.Text = "DQ Attacks";
            this.lbl_dq.Visible = false;
            // 
            // cb_elimination
            // 
            this.cb_elimination.AutoSize = true;
            this.cb_elimination.Location = new System.Drawing.Point(139, 6);
            this.cb_elimination.Name = "cb_elimination";
            this.cb_elimination.Size = new System.Drawing.Size(80, 19);
            this.cb_elimination.TabIndex = 12;
            this.cb_elimination.Text = "Elimination";
            this.cb_elimination.UseVisualStyleBackColor = true;
            this.cb_elimination.CheckedChanged += new System.EventHandler(this.cb_elimination_CheckedChanged);
            // 
            // rulesTabControl
            // 
            this.rulesTabControl.Controls.Add(this.rulesTab);
            this.rulesTabControl.Location = new System.Drawing.Point(6, 227);
            this.rulesTabControl.Name = "rulesTabControl";
            this.rulesTabControl.SelectedIndex = 0;
            this.rulesTabControl.Size = new System.Drawing.Size(976, 545);
            this.rulesTabControl.TabIndex = 13;
            // 
            // rulesTab
            // 
            this.rulesTab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.rulesTab.Controls.Add(this.lbl_Basic);
            this.rulesTab.Controls.Add(this.tb_basic);
            this.rulesTab.Controls.Add(this.tb_illegal);
            this.rulesTab.Controls.Add(this.lbl_illegal);
            this.rulesTab.Controls.Add(this.lbl_dq);
            this.rulesTab.Controls.Add(this.tb_dq);
            this.rulesTab.Location = new System.Drawing.Point(4, 24);
            this.rulesTab.Name = "rulesTab";
            this.rulesTab.Padding = new System.Windows.Forms.Padding(3);
            this.rulesTab.Size = new System.Drawing.Size(968, 517);
            this.rulesTab.TabIndex = 0;
            this.rulesTab.Text = "Attack Rules (Not Available)";
            this.rulesTab.UseVisualStyleBackColor = true;
            // 
            // cb_losersLeave
            // 
            this.cb_losersLeave.AutoSize = true;
            this.cb_losersLeave.Checked = true;
            this.cb_losersLeave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_losersLeave.Enabled = false;
            this.cb_losersLeave.Location = new System.Drawing.Point(3, 107);
            this.cb_losersLeave.Name = "cb_losersLeave";
            this.cb_losersLeave.Size = new System.Drawing.Size(200, 19);
            this.cb_losersLeave.TabIndex = 3;
            this.cb_losersLeave.Text = "Losers Leave Ringside (Elimination)";
            this.cb_losersLeave.UseVisualStyleBackColor = true;
            this.cb_losersLeave.Visible = false;
            // 
            // cb_membersWait
            // 
            this.cb_membersWait.AutoSize = true;
            this.cb_membersWait.Checked = true;
            this.cb_membersWait.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_membersWait.Enabled = false;
            this.cb_membersWait.Location = new System.Drawing.Point(3, 83);
            this.cb_membersWait.Name = "cb_membersWait";
            this.cb_membersWait.Size = new System.Drawing.Size(221, 19);
            this.cb_membersWait.TabIndex = 2;
            this.cb_membersWait.Text = "Members Wait At Ringside (Elimination)";
            this.cb_membersWait.UseVisualStyleBackColor = true;
            this.cb_membersWait.Visible = false;
            // 
            // matchHelp
            // 
            this.matchHelp.Font = new System.Drawing.Font("Impact", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.matchHelp.Location = new System.Drawing.Point(168, 5);
            this.matchHelp.Name = "matchHelp";
            this.matchHelp.Size = new System.Drawing.Size(48, 29);
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
            // cb_ttt
            // 
            this.cb_ttt.AutoSize = true;
            this.cb_ttt.Location = new System.Drawing.Point(7, 7);
            this.cb_ttt.Name = "cb_ttt";
            this.cb_ttt.Size = new System.Drawing.Size(116, 19);
            this.cb_ttt.TabIndex = 19;
            this.cb_ttt.Text = "Timed Tornado Tag";
            this.cb_ttt.UseVisualStyleBackColor = true;
            this.cb_ttt.CheckedChanged += new System.EventHandler(this.cb_ttt_CheckedChanged);
            // 
            // cb_boxing
            // 
            this.cb_boxing.AutoSize = true;
            this.cb_boxing.Location = new System.Drawing.Point(6, 10);
            this.cb_boxing.Name = "cb_boxing";
            this.cb_boxing.Size = new System.Drawing.Size(60, 19);
            this.cb_boxing.TabIndex = 21;
            this.cb_boxing.Text = "Boxing";
            this.cb_boxing.UseVisualStyleBackColor = true;
            this.cb_boxing.CheckedChanged += new System.EventHandler(this.cb_boxing_CheckedChanged);
            // 
            // cb_kickboxing
            // 
            this.cb_kickboxing.AutoSize = true;
            this.cb_kickboxing.Location = new System.Drawing.Point(6, 35);
            this.cb_kickboxing.Name = "cb_kickboxing";
            this.cb_kickboxing.Size = new System.Drawing.Size(85, 19);
            this.cb_kickboxing.TabIndex = 22;
            this.cb_kickboxing.Text = "Kick Boxing";
            this.cb_kickboxing.UseVisualStyleBackColor = true;
            this.cb_kickboxing.CheckedChanged += new System.EventHandler(this.cb_kickboxing_CheckedChanged);
            // 
            // removePosts
            // 
            this.removePosts.AutoSize = true;
            this.removePosts.Location = new System.Drawing.Point(3, 59);
            this.removePosts.Name = "removePosts";
            this.removePosts.Size = new System.Drawing.Size(93, 19);
            this.removePosts.TabIndex = 27;
            this.removePosts.Text = "Remove Posts";
            this.removePosts.UseVisualStyleBackColor = true;
            this.removePosts.CheckedChanged += new System.EventHandler(this.isPost_CheckedChanged);
            // 
            // removeRopes
            // 
            this.removeRopes.AutoSize = true;
            this.removeRopes.Location = new System.Drawing.Point(3, 32);
            this.removeRopes.Name = "removeRopes";
            this.removeRopes.Size = new System.Drawing.Size(95, 19);
            this.removeRopes.TabIndex = 26;
            this.removeRopes.Text = "Remove Ropes";
            this.removeRopes.UseVisualStyleBackColor = true;
            // 
            // isk1mma
            // 
            this.isk1mma.AutoSize = true;
            this.isk1mma.Enabled = false;
            this.isk1mma.Location = new System.Drawing.Point(200, 36);
            this.isk1mma.Name = "isk1mma";
            this.isk1mma.Size = new System.Drawing.Size(132, 19);
            this.isk1mma.TabIndex = 28;
            this.isk1mma.Text = "K-1/MMA Mixed Rules";
            this.isk1mma.UseVisualStyleBackColor = true;
            this.isk1mma.Visible = false;
            // 
            // cb_luchaTag
            // 
            this.cb_luchaTag.AutoSize = true;
            this.cb_luchaTag.Location = new System.Drawing.Point(7, 31);
            this.cb_luchaTag.Name = "cb_luchaTag";
            this.cb_luchaTag.Size = new System.Drawing.Size(104, 19);
            this.cb_luchaTag.TabIndex = 29;
            this.cb_luchaTag.Text = "Lucha Tag Rules";
            this.cb_luchaTag.UseVisualStyleBackColor = true;
            this.cb_luchaTag.CheckedChanged += new System.EventHandler(this.cb_luchaTag_CheckedChanged);
            // 
            // isAutoKo
            // 
            this.isAutoKo.AutoSize = true;
            this.isAutoKo.Location = new System.Drawing.Point(3, 8);
            this.isAutoKo.Name = "isAutoKo";
            this.isAutoKo.Size = new System.Drawing.Size(105, 19);
            this.isAutoKo.TabIndex = 30;
            this.isAutoKo.Text = "KO When HP < 0";
            this.isAutoKo.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(6, 59);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(638, 158);
            this.tabControl1.TabIndex = 31;
            // 
            // tabPage2
            // 
            this.tabPage2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPage2.Controls.Add(this.cb_survival);
            this.tabPage2.Controls.Add(this.cb_exElim);
            this.tabPage2.Controls.Add(this.cb_IronManMatch);
            this.tabPage2.Controls.Add(this.cb_FirstBlood);
            this.tabPage2.Controls.Add(this.cb_luchaTag);
            this.tabPage2.Controls.Add(this.cb_ttt);
            this.tabPage2.Controls.Add(this.cb_elimination);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(630, 130);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Pro Wrestling Style";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cb_survival
            // 
            this.cb_survival.AutoSize = true;
            this.cb_survival.Location = new System.Drawing.Point(278, 6);
            this.cb_survival.Name = "cb_survival";
            this.cb_survival.Size = new System.Drawing.Size(92, 19);
            this.cb_survival.TabIndex = 31;
            this.cb_survival.Text = "Survival Road";
            this.cb_survival.UseVisualStyleBackColor = true;
            this.cb_survival.CheckedChanged += new System.EventHandler(this.cb_survival_CheckedChanged);
            // 
            // cb_exElim
            // 
            this.cb_exElim.AutoSize = true;
            this.cb_exElim.Location = new System.Drawing.Point(139, 31);
            this.cb_exElim.Name = "cb_exElim";
            this.cb_exElim.Size = new System.Drawing.Size(130, 19);
            this.cb_exElim.TabIndex = 30;
            this.cb_exElim.Text = "Extended Elimination";
            this.cb_exElim.UseVisualStyleBackColor = true;
            this.cb_exElim.CheckedChanged += new System.EventHandler(this.cb_exElim_CheckedChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPage3.Controls.Add(this.cb_sumo);
            this.tabPage3.Controls.Add(this.cb_uwfi);
            this.tabPage3.Controls.Add(this.cb_Pancrase);
            this.tabPage3.Controls.Add(this.isk1mma);
            this.tabPage3.Controls.Add(this.cb_boxing);
            this.tabPage3.Controls.Add(this.cb_kickboxing);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(630, 130);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "Shoot Style";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // cb_sumo
            // 
            this.cb_sumo.AutoSize = true;
            this.cb_sumo.Location = new System.Drawing.Point(112, 10);
            this.cb_sumo.Name = "cb_sumo";
            this.cb_sumo.Size = new System.Drawing.Size(54, 19);
            this.cb_sumo.TabIndex = 29;
            this.cb_sumo.Text = "Sumo";
            this.cb_sumo.UseVisualStyleBackColor = true;
            this.cb_sumo.CheckedChanged += new System.EventHandler(this.cb_sumo_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.cb_luchaFalls);
            this.panel1.Controls.Add(this.matchConfig);
            this.panel1.Controls.Add(this.removeRopes);
            this.panel1.Controls.Add(this.isAutoKo);
            this.panel1.Controls.Add(this.removePosts);
            this.panel1.Controls.Add(this.cb_losersLeave);
            this.panel1.Controls.Add(this.cb_membersWait);
            this.panel1.Location = new System.Drawing.Point(650, 83);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(328, 134);
            this.panel1.TabIndex = 32;
            // 
            // cb_luchaFalls
            // 
            this.cb_luchaFalls.AutoSize = true;
            this.cb_luchaFalls.Location = new System.Drawing.Point(3, 84);
            this.cb_luchaFalls.Name = "cb_luchaFalls";
            this.cb_luchaFalls.Size = new System.Drawing.Size(108, 19);
            this.cb_luchaFalls.TabIndex = 32;
            this.cb_luchaFalls.Text = "2/3 Falls (Lucha)";
            this.cb_luchaFalls.UseVisualStyleBackColor = true;
            this.cb_luchaFalls.Visible = false;
            // 
            // matchConfig
            // 
            this.matchConfig.Location = new System.Drawing.Point(209, 8);
            this.matchConfig.Name = "matchConfig";
            this.matchConfig.Size = new System.Drawing.Size(75, 23);
            this.matchConfig.TabIndex = 31;
            this.matchConfig.Text = "Setup";
            this.matchConfig.UseVisualStyleBackColor = true;
            this.matchConfig.Visible = false;
            this.matchConfig.Click += new System.EventHandler(this.matchConfig_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("NEWSFLASH", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(795, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 17);
            this.label1.TabIndex = 33;
            this.label1.Text = "Rules";
            // 
            // MoreMatchTypes_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(994, 778);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.matchHelp);
            this.Controls.Add(this.rulesTabControl);
            this.Controls.Add(this.cb_normalMatch);
            this.Font = new System.Drawing.Font("NEWSFLASH", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MoreMatchTypes_Form";
            this.Text = "More Match Types";
            this.Load += new System.EventHandler(this.MoreMatchTypes_Form_Load);
            this.rulesTabControl.ResumeLayout(false);
            this.rulesTab.ResumeLayout(false);
            this.rulesTab.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private System.Windows.Forms.TabPage rulesTab;
        public System.Windows.Forms.RadioButton cb_elimination;
        public System.Windows.Forms.CheckBox cb_membersWait;
        public System.Windows.Forms.CheckBox cb_losersLeave;
        public System.Windows.Forms.TabControl rulesTabControl;
        public System.Windows.Forms.ToolTip tt_normal;
        public System.Windows.Forms.Button matchHelp;
        public System.Windows.Forms.RadioButton cb_ttt;
        public System.Windows.Forms.RadioButton cb_boxing;
        public System.Windows.Forms.RadioButton cb_kickboxing;
        public System.Windows.Forms.CheckBox removePosts;
        public System.Windows.Forms.CheckBox removeRopes;
        public System.Windows.Forms.RadioButton isk1mma;
        public System.Windows.Forms.RadioButton cb_luchaTag;
        public System.Windows.Forms.CheckBox isAutoKo;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button matchConfig;
        public System.Windows.Forms.RadioButton cb_survival;
        public System.Windows.Forms.RadioButton cb_exElim;
        public System.Windows.Forms.RadioButton cb_sumo;
        public System.Windows.Forms.CheckBox cb_luchaFalls;
    }
}