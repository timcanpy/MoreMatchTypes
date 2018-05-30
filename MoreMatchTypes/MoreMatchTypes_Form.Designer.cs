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
            this.SuspendLayout();
            // 
            // cb_FirstBlood
            // 
            this.cb_FirstBlood.AutoSize = true;
            this.cb_FirstBlood.Location = new System.Drawing.Point(23, 111);
            this.cb_FirstBlood.Name = "cb_FirstBlood";
            this.cb_FirstBlood.Size = new System.Drawing.Size(74, 17);
            this.cb_FirstBlood.TabIndex = 0;
            this.cb_FirstBlood.Text = "First Blood";
            this.cb_FirstBlood.UseVisualStyleBackColor = true;
            // 
            // cb_IronManMatch
            // 
            this.cb_IronManMatch.AutoSize = true;
            this.cb_IronManMatch.Location = new System.Drawing.Point(23, 37);
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
            this.cb_uwfi.Location = new System.Drawing.Point(23, 61);
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
            this.cb_sumo.Location = new System.Drawing.Point(23, 85);
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
            this.cb_Pancrase.Location = new System.Drawing.Point(154, 13);
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
            this.tb_basic.Location = new System.Drawing.Point(23, 170);
            this.tb_basic.Multiline = true;
            this.tb_basic.Name = "tb_basic";
            this.tb_basic.Size = new System.Drawing.Size(236, 356);
            this.tb_basic.TabIndex = 6;
            this.tb_basic.Visible = false;
            // 
            // tb_illegal
            // 
            this.tb_illegal.Location = new System.Drawing.Point(294, 170);
            this.tb_illegal.Multiline = true;
            this.tb_illegal.Name = "tb_illegal";
            this.tb_illegal.Size = new System.Drawing.Size(280, 356);
            this.tb_illegal.TabIndex = 7;
            this.tb_illegal.Visible = false;
            this.tb_illegal.TextChanged += new System.EventHandler(this.tb_illegal_TextChanged);
            // 
            // tb_dq
            // 
            this.tb_dq.Location = new System.Drawing.Point(609, 169);
            this.tb_dq.Multiline = true;
            this.tb_dq.Name = "tb_dq";
            this.tb_dq.Size = new System.Drawing.Size(231, 357);
            this.tb_dq.TabIndex = 8;
            this.tb_dq.Visible = false;
            // 
            // lbl_Basic
            // 
            this.lbl_Basic.AutoSize = true;
            this.lbl_Basic.Location = new System.Drawing.Point(25, 154);
            this.lbl_Basic.Name = "lbl_Basic";
            this.lbl_Basic.Size = new System.Drawing.Size(72, 13);
            this.lbl_Basic.TabIndex = 9;
            this.lbl_Basic.Text = "Basic Attacks";
            this.lbl_Basic.Visible = false;
            // 
            // lbl_illegal
            // 
            this.lbl_illegal.AutoSize = true;
            this.lbl_illegal.Location = new System.Drawing.Point(291, 154);
            this.lbl_illegal.Name = "lbl_illegal";
            this.lbl_illegal.Size = new System.Drawing.Size(73, 13);
            this.lbl_illegal.TabIndex = 10;
            this.lbl_illegal.Text = "Illegal Attacks";
            this.lbl_illegal.Visible = false;
            // 
            // lbl_dq
            // 
            this.lbl_dq.AutoSize = true;
            this.lbl_dq.Location = new System.Drawing.Point(606, 154);
            this.lbl_dq.Name = "lbl_dq";
            this.lbl_dq.Size = new System.Drawing.Size(62, 13);
            this.lbl_dq.TabIndex = 11;
            this.lbl_dq.Text = "DQ Attacks";
            this.lbl_dq.Visible = false;
            // 
            // MoreMatchTypes_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 568);
            this.Controls.Add(this.lbl_dq);
            this.Controls.Add(this.lbl_illegal);
            this.Controls.Add(this.lbl_Basic);
            this.Controls.Add(this.tb_dq);
            this.Controls.Add(this.tb_illegal);
            this.Controls.Add(this.tb_basic);
            this.Controls.Add(this.cb_Pancrase);
            this.Controls.Add(this.cb_normalMatch);
            this.Controls.Add(this.cb_sumo);
            this.Controls.Add(this.cb_uwfi);
            this.Controls.Add(this.cb_IronManMatch);
            this.Controls.Add(this.cb_FirstBlood);
            this.Name = "MoreMatchTypes_Form";
            this.Text = "More Match Types";
            this.Load += new System.EventHandler(this.MoreMatchTypes_Form_Load);
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
    }
}