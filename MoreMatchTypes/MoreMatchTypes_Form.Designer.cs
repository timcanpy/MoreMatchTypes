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
            this.cb_FirstBlood = new System.Windows.Forms.RadioButton();
            this.cb_IronManMatch = new System.Windows.Forms.RadioButton();
            this.cb_uwfi = new System.Windows.Forms.RadioButton();
            this.cb_sumo = new System.Windows.Forms.RadioButton();
            this.cb_normalMatch = new System.Windows.Forms.RadioButton();
            this.cb_Pancrase = new System.Windows.Forms.RadioButton();
            this.tb_basic = new System.Windows.Forms.TextBox();
            this.tb_illegal = new System.Windows.Forms.TextBox();
            this.tb_dq = new System.Windows.Forms.TextBox();
            this.lbl_Basic = new System.Windows.Forms.Label();
            this.lbl_illegal = new System.Windows.Forms.Label();
            this.lbl_dq = new System.Windows.Forms.Label();
            this.cb_elimination = new System.Windows.Forms.RadioButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cb_losersLeave = new System.Windows.Forms.CheckBox();
            this.cb_membersWait = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.cb_bellSave = new System.Windows.Forms.CheckBox();
            this.cb_stopFight = new System.Windows.Forms.CheckBox();
            this.cb_docStop = new System.Windows.Forms.CheckBox();
            this.cb_roundBreak = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.el_searchInput = new System.Windows.Forms.TextBox();
            this.el_searchBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.el_promotionList = new System.Windows.Forms.ComboBox();
            this.el_resultList = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rb_singleBlue = new System.Windows.Forms.RadioButton();
            this.rb_singleRed = new System.Windows.Forms.RadioButton();
            this.rb_allBlue = new System.Windows.Forms.RadioButton();
            this.rb_allRed = new System.Windows.Forms.RadioButton();
            this.el_refresh = new System.Windows.Forms.Button();
            this.el_removeAllBlue = new System.Windows.Forms.Button();
            this.el_addBtn = new System.Windows.Forms.Button();
            this.el_blueList = new System.Windows.Forms.ListBox();
            this.el_removeOneBlue = new System.Windows.Forms.Button();
            this.el_redList = new System.Windows.Forms.ListBox();
            this.el_removeOneRed = new System.Windows.Forms.Button();
            this.el_removeAllRed = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.cb_uwfi.Location = new System.Drawing.Point(153, 61);
            this.cb_uwfi.Name = "cb_uwfi";
            this.cb_uwfi.Size = new System.Drawing.Size(53, 17);
            this.cb_uwfi.TabIndex = 2;
            this.cb_uwfi.Text = "UWFI";
            this.cb_uwfi.UseVisualStyleBackColor = true;
            this.cb_uwfi.CheckedChanged += new System.EventHandler(this.cb_uwfi_CheckedChanged);
            // 
            // cb_sumo
            // 
            this.cb_sumo.AutoSize = true;
            this.cb_sumo.Location = new System.Drawing.Point(153, 38);
            this.cb_sumo.Name = "cb_sumo";
            this.cb_sumo.Size = new System.Drawing.Size(52, 17);
            this.cb_sumo.TabIndex = 3;
            this.cb_sumo.Text = "Sumo";
            this.cb_sumo.UseVisualStyleBackColor = true;
            this.cb_sumo.CheckedChanged += new System.EventHandler(this.cb_sumo_CheckedChanged);
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
            this.cb_Pancrase.Location = new System.Drawing.Point(153, 84);
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
            this.cb_elimination.Location = new System.Drawing.Point(22, 84);
            this.cb_elimination.Name = "cb_elimination";
            this.cb_elimination.Size = new System.Drawing.Size(75, 17);
            this.cb_elimination.TabIndex = 12;
            this.cb_elimination.TabStop = true;
            this.cb_elimination.Text = "Elimination";
            this.cb_elimination.UseVisualStyleBackColor = true;
            this.cb_elimination.CheckedChanged += new System.EventHandler(this.cb_elimination_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(2, 145);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(849, 422);
            this.tabControl1.TabIndex = 13;
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
            this.tabPage1.Size = new System.Drawing.Size(841, 396);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Attack Rules";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.cb_losersLeave);
            this.tabPage2.Controls.Add(this.cb_membersWait);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(841, 396);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Elimination Rules";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cb_losersLeave
            // 
            this.cb_losersLeave.AutoSize = true;
            this.cb_losersLeave.Checked = true;
            this.cb_losersLeave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_losersLeave.Enabled = false;
            this.cb_losersLeave.Location = new System.Drawing.Point(6, 6);
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
            this.cb_membersWait.Location = new System.Drawing.Point(147, 6);
            this.cb_membersWait.Name = "cb_membersWait";
            this.cb_membersWait.Size = new System.Drawing.Size(157, 17);
            this.cb_membersWait.TabIndex = 2;
            this.cb_membersWait.Text = "Members Wait At Ringside?";
            this.cb_membersWait.UseVisualStyleBackColor = true;
            this.cb_membersWait.Visible = false;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.cb_bellSave);
            this.tabPage3.Controls.Add(this.cb_stopFight);
            this.tabPage3.Controls.Add(this.cb_docStop);
            this.tabPage3.Controls.Add(this.cb_roundBreak);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(841, 396);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Referee Options";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // cb_bellSave
            // 
            this.cb_bellSave.AutoSize = true;
            this.cb_bellSave.Location = new System.Drawing.Point(16, 87);
            this.cb_bellSave.Name = "cb_bellSave";
            this.cb_bellSave.Size = new System.Drawing.Size(207, 17);
            this.cb_bellSave.TabIndex = 3;
            this.cb_bellSave.Text = "Count continues after the round ends?";
            this.cb_bellSave.UseVisualStyleBackColor = true;
            // 
            // cb_stopFight
            // 
            this.cb_stopFight.AutoSize = true;
            this.cb_stopFight.Location = new System.Drawing.Point(17, 64);
            this.cb_stopFight.Name = "cb_stopFight";
            this.cb_stopFight.Size = new System.Drawing.Size(238, 17);
            this.cb_stopFight.TabIndex = 2;
            this.cb_stopFight.Text = "End Match if Fighter Fails to Defend Himself?";
            this.cb_stopFight.UseVisualStyleBackColor = true;
            // 
            // cb_docStop
            // 
            this.cb_docStop.AutoSize = true;
            this.cb_docStop.Location = new System.Drawing.Point(17, 41);
            this.cb_docStop.Name = "cb_docStop";
            this.cb_docStop.Size = new System.Drawing.Size(122, 17);
            this.cb_docStop.TabIndex = 1;
            this.cb_docStop.Text = "Check Fighter Cuts?";
            this.cb_docStop.UseVisualStyleBackColor = true;
            // 
            // cb_roundBreak
            // 
            this.cb_roundBreak.AutoSize = true;
            this.cb_roundBreak.Location = new System.Drawing.Point(17, 18);
            this.cb_roundBreak.Name = "cb_roundBreak";
            this.cb_roundBreak.Size = new System.Drawing.Size(125, 17);
            this.cb_roundBreak.TabIndex = 0;
            this.cb_roundBreak.Text = "Force Round Break?";
            this.cb_roundBreak.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.el_removeOneBlue);
            this.groupBox1.Controls.Add(this.el_removeAllBlue);
            this.groupBox1.Controls.Add(this.el_blueList);
            this.groupBox1.Location = new System.Drawing.Point(6, 194);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 195);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Blue Team Set-Up";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.el_removeAllRed);
            this.groupBox2.Controls.Add(this.el_removeOneRed);
            this.groupBox2.Controls.Add(this.el_redList);
            this.groupBox2.Location = new System.Drawing.Point(415, 194);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(419, 195);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Red Team Set-Up";
            // 
            // el_searchInput
            // 
            this.el_searchInput.Location = new System.Drawing.Point(6, 27);
            this.el_searchInput.Name = "el_searchInput";
            this.el_searchInput.Size = new System.Drawing.Size(336, 20);
            this.el_searchInput.TabIndex = 0;
            // 
            // el_searchBtn
            // 
            this.el_searchBtn.Location = new System.Drawing.Point(741, 27);
            this.el_searchBtn.Name = "el_searchBtn";
            this.el_searchBtn.Size = new System.Drawing.Size(75, 23);
            this.el_searchBtn.TabIndex = 1;
            this.el_searchBtn.Text = "Search";
            this.el_searchBtn.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Wrestler Search";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(358, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Promotion List";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.el_addBtn);
            this.panel1.Controls.Add(this.el_refresh);
            this.panel1.Controls.Add(this.rb_allRed);
            this.panel1.Controls.Add(this.rb_allBlue);
            this.panel1.Controls.Add(this.rb_singleRed);
            this.panel1.Controls.Add(this.rb_singleBlue);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.el_resultList);
            this.panel1.Controls.Add(this.el_promotionList);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.el_searchBtn);
            this.panel1.Controls.Add(this.el_searchInput);
            this.panel1.Location = new System.Drawing.Point(3, 47);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(828, 119);
            this.panel1.TabIndex = 6;
            // 
            // el_promotionList
            // 
            this.el_promotionList.FormattingEnabled = true;
            this.el_promotionList.Location = new System.Drawing.Point(361, 27);
            this.el_promotionList.Name = "el_promotionList";
            this.el_promotionList.Size = new System.Drawing.Size(374, 21);
            this.el_promotionList.TabIndex = 4;
            // 
            // el_resultList
            // 
            this.el_resultList.FormattingEnabled = true;
            this.el_resultList.Location = new System.Drawing.Point(6, 72);
            this.el_resultList.Name = "el_resultList";
            this.el_resultList.Size = new System.Drawing.Size(336, 21);
            this.el_resultList.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Search Results";
            // 
            // rb_singleBlue
            // 
            this.rb_singleBlue.AutoSize = true;
            this.rb_singleBlue.Location = new System.Drawing.Point(361, 56);
            this.rb_singleBlue.Name = "rb_singleBlue";
            this.rb_singleBlue.Size = new System.Drawing.Size(169, 17);
            this.rb_singleBlue.TabIndex = 7;
            this.rb_singleBlue.TabStop = true;
            this.rb_singleBlue.Text = "Add Single Result (Blue Team)";
            this.rb_singleBlue.UseVisualStyleBackColor = true;
            // 
            // rb_singleRed
            // 
            this.rb_singleRed.AutoSize = true;
            this.rb_singleRed.Location = new System.Drawing.Point(362, 79);
            this.rb_singleRed.Name = "rb_singleRed";
            this.rb_singleRed.Size = new System.Drawing.Size(168, 17);
            this.rb_singleRed.TabIndex = 8;
            this.rb_singleRed.TabStop = true;
            this.rb_singleRed.Text = "Add Single Result (Red Team)";
            this.rb_singleRed.UseVisualStyleBackColor = true;
            // 
            // rb_allBlue
            // 
            this.rb_allBlue.AutoSize = true;
            this.rb_allBlue.Location = new System.Drawing.Point(536, 56);
            this.rb_allBlue.Name = "rb_allBlue";
            this.rb_allBlue.Size = new System.Drawing.Size(156, 17);
            this.rb_allBlue.TabIndex = 9;
            this.rb_allBlue.TabStop = true;
            this.rb_allBlue.Text = "Add All Results (Blue Team)";
            this.rb_allBlue.UseVisualStyleBackColor = true;
            // 
            // rb_allRed
            // 
            this.rb_allRed.AutoSize = true;
            this.rb_allRed.Location = new System.Drawing.Point(536, 80);
            this.rb_allRed.Name = "rb_allRed";
            this.rb_allRed.Size = new System.Drawing.Size(155, 17);
            this.rb_allRed.TabIndex = 10;
            this.rb_allRed.TabStop = true;
            this.rb_allRed.Text = "Add All Results (Red Team)";
            this.rb_allRed.UseVisualStyleBackColor = true;
            // 
            // el_refresh
            // 
            this.el_refresh.Location = new System.Drawing.Point(741, 57);
            this.el_refresh.Name = "el_refresh";
            this.el_refresh.Size = new System.Drawing.Size(75, 23);
            this.el_refresh.TabIndex = 11;
            this.el_refresh.Text = "Refresh";
            this.el_refresh.UseVisualStyleBackColor = true;
            // 
            // el_removeAllBlue
            // 
            this.el_removeAllBlue.Location = new System.Drawing.Point(218, 172);
            this.el_removeAllBlue.Name = "el_removeAllBlue";
            this.el_removeAllBlue.Size = new System.Drawing.Size(160, 23);
            this.el_removeAllBlue.TabIndex = 12;
            this.el_removeAllBlue.Text = "Remove All";
            this.el_removeAllBlue.UseVisualStyleBackColor = true;
            // 
            // el_addBtn
            // 
            this.el_addBtn.Location = new System.Drawing.Point(741, 87);
            this.el_addBtn.Name = "el_addBtn";
            this.el_addBtn.Size = new System.Drawing.Size(75, 23);
            this.el_addBtn.TabIndex = 13;
            this.el_addBtn.Text = "Add";
            this.el_addBtn.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.el_addBtn.UseVisualStyleBackColor = true;
            // 
            // el_blueList
            // 
            this.el_blueList.FormattingEnabled = true;
            this.el_blueList.Location = new System.Drawing.Point(7, 20);
            this.el_blueList.Name = "el_blueList";
            this.el_blueList.ScrollAlwaysVisible = true;
            this.el_blueList.Size = new System.Drawing.Size(371, 147);
            this.el_blueList.TabIndex = 0;
            // 
            // el_removeOneBlue
            // 
            this.el_removeOneBlue.Location = new System.Drawing.Point(11, 172);
            this.el_removeOneBlue.Name = "el_removeOneBlue";
            this.el_removeOneBlue.Size = new System.Drawing.Size(130, 23);
            this.el_removeOneBlue.TabIndex = 1;
            this.el_removeOneBlue.Text = "Remove Selected";
            this.el_removeOneBlue.UseVisualStyleBackColor = true;
            this.el_removeOneBlue.Click += new System.EventHandler(this.button5_Click);
            // 
            // el_redList
            // 
            this.el_redList.FormattingEnabled = true;
            this.el_redList.Location = new System.Drawing.Point(-2, 20);
            this.el_redList.Name = "el_redList";
            this.el_redList.ScrollAlwaysVisible = true;
            this.el_redList.Size = new System.Drawing.Size(415, 147);
            this.el_redList.TabIndex = 0;
            // 
            // el_removeOneRed
            // 
            this.el_removeOneRed.Location = new System.Drawing.Point(6, 172);
            this.el_removeOneRed.Name = "el_removeOneRed";
            this.el_removeOneRed.Size = new System.Drawing.Size(161, 23);
            this.el_removeOneRed.TabIndex = 1;
            this.el_removeOneRed.Text = "Remove Selected";
            this.el_removeOneRed.UseVisualStyleBackColor = true;
            // 
            // el_removeAllRed
            // 
            this.el_removeAllRed.Location = new System.Drawing.Point(241, 172);
            this.el_removeAllRed.Name = "el_removeAllRed";
            this.el_removeAllRed.Size = new System.Drawing.Size(163, 23);
            this.el_removeAllRed.TabIndex = 2;
            this.el_removeAllRed.Text = "Remove All";
            this.el_removeAllRed.UseVisualStyleBackColor = true;
            // 
            // MoreMatchTypes_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 596);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cb_elimination);
            this.Controls.Add(this.cb_Pancrase);
            this.Controls.Add(this.cb_normalMatch);
            this.Controls.Add(this.cb_sumo);
            this.Controls.Add(this.cb_uwfi);
            this.Controls.Add(this.cb_IronManMatch);
            this.Controls.Add(this.cb_FirstBlood);
            this.Name = "MoreMatchTypes_Form";
            this.Text = "More Match Types";
            this.Load += new System.EventHandler(this.MoreMatchTypes_Form_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.RadioButton cb_FirstBlood;
        public System.Windows.Forms.RadioButton cb_IronManMatch;
        public System.Windows.Forms.RadioButton cb_uwfi;
        public System.Windows.Forms.RadioButton cb_sumo;
        private System.Windows.Forms.RadioButton cb_normalMatch;
        public System.Windows.Forms.RadioButton cb_Pancrase;
        public System.Windows.Forms.TextBox tb_basic;
        public System.Windows.Forms.TextBox tb_illegal;
        public System.Windows.Forms.TextBox tb_dq;
        public System.Windows.Forms.Label lbl_Basic;
        public System.Windows.Forms.Label lbl_illegal;
        public System.Windows.Forms.Label lbl_dq;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        public System.Windows.Forms.RadioButton cb_elimination;
        public System.Windows.Forms.CheckBox cb_membersWait;
        public System.Windows.Forms.CheckBox cb_losersLeave;
        public System.Windows.Forms.CheckBox cb_stopFight;
        public System.Windows.Forms.CheckBox cb_docStop;
        public System.Windows.Forms.CheckBox cb_roundBreak;
        public System.Windows.Forms.CheckBox cb_bellSave;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button el_searchBtn;
        private System.Windows.Forms.TextBox el_searchInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox el_resultList;
        private System.Windows.Forms.ComboBox el_promotionList;
        private System.Windows.Forms.Button el_addBtn;
        private System.Windows.Forms.Button el_removeAllBlue;
        private System.Windows.Forms.Button el_refresh;
        private System.Windows.Forms.RadioButton rb_allRed;
        private System.Windows.Forms.RadioButton rb_allBlue;
        private System.Windows.Forms.RadioButton rb_singleRed;
        private System.Windows.Forms.RadioButton rb_singleBlue;
        private System.Windows.Forms.ListBox el_blueList;
        private System.Windows.Forms.Button el_removeOneBlue;
        private System.Windows.Forms.Button el_removeAllRed;
        private System.Windows.Forms.Button el_removeOneRed;
        private System.Windows.Forms.ListBox el_redList;
    }
}