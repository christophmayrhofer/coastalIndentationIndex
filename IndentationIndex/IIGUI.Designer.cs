namespace IndentationIndex
{
    partial class IIGUI
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
            this.btn_val = new System.Windows.Forms.Button();
            this.trkbar_seg = new System.Windows.Forms.TrackBar();
            this.txtbox_seg = new System.Windows.Forms.TextBox();
            this.txtbox_min = new System.Windows.Forms.TextBox();
            this.txtbox_max = new System.Windows.Forms.TextBox();
            this.chkbox_cat = new System.Windows.Forms.CheckBox();
            this.chkbox_con = new System.Windows.Forms.CheckBox();
            this.btn_analyze = new System.Windows.Forms.Button();
            this.combox_unit = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.trkbar_seg)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_val
            // 
            this.btn_val.Location = new System.Drawing.Point(12, 114);
            this.btn_val.Name = "btn_val";
            this.btn_val.Size = new System.Drawing.Size(114, 23);
            this.btn_val.TabIndex = 0;
            this.btn_val.Text = "Save II Values";
            this.btn_val.UseVisualStyleBackColor = true;
            this.btn_val.Click += new System.EventHandler(this.button1_Click);
            // 
            // trkbar_seg
            // 
            this.trkbar_seg.Location = new System.Drawing.Point(47, 48);
            this.trkbar_seg.Name = "trkbar_seg";
            this.trkbar_seg.Size = new System.Drawing.Size(158, 45);
            this.trkbar_seg.TabIndex = 1;
            this.trkbar_seg.ValueChanged += new System.EventHandler(this.trkbar_seg_ValueChanged);
            // 
            // txtbox_seg
            // 
            this.txtbox_seg.Location = new System.Drawing.Point(73, 22);
            this.txtbox_seg.Name = "txtbox_seg";
            this.txtbox_seg.Size = new System.Drawing.Size(53, 20);
            this.txtbox_seg.TabIndex = 2;
            this.txtbox_seg.Text = "100";
            this.txtbox_seg.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtbox_seg_KeyDown);
            // 
            // txtbox_min
            // 
            this.txtbox_min.Location = new System.Drawing.Point(12, 48);
            this.txtbox_min.Name = "txtbox_min";
            this.txtbox_min.Size = new System.Drawing.Size(28, 20);
            this.txtbox_min.TabIndex = 3;
            this.txtbox_min.Text = "0";
            this.txtbox_min.TextChanged += new System.EventHandler(this.txtbox_min_TextChanged);
            // 
            // txtbox_max
            // 
            this.txtbox_max.Location = new System.Drawing.Point(211, 48);
            this.txtbox_max.Name = "txtbox_max";
            this.txtbox_max.Size = new System.Drawing.Size(42, 20);
            this.txtbox_max.TabIndex = 4;
            this.txtbox_max.Text = "1000";
            this.txtbox_max.TextChanged += new System.EventHandler(this.txtbox_max_TextChanged);
            // 
            // chkbox_cat
            // 
            this.chkbox_cat.AutoSize = true;
            this.chkbox_cat.Checked = true;
            this.chkbox_cat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbox_cat.Location = new System.Drawing.Point(12, 89);
            this.chkbox_cat.Name = "chkbox_cat";
            this.chkbox_cat.Size = new System.Drawing.Size(114, 17);
            this.chkbox_cat.TabIndex = 5;
            this.chkbox_cat.Text = "Display II Category";
            this.chkbox_cat.UseVisualStyleBackColor = true;
            this.chkbox_cat.CheckedChanged += new System.EventHandler(this.chkbox_cat_CheckedChanged);
            // 
            // chkbox_con
            // 
            this.chkbox_con.AutoSize = true;
            this.chkbox_con.Location = new System.Drawing.Point(132, 89);
            this.chkbox_con.Name = "chkbox_con";
            this.chkbox_con.Size = new System.Drawing.Size(122, 17);
            this.chkbox_con.TabIndex = 6;
            this.chkbox_con.Text = "Display Construction";
            this.chkbox_con.UseVisualStyleBackColor = true;
            this.chkbox_con.CheckedChanged += new System.EventHandler(this.chkbox_con_CheckedChanged);
            // 
            // btn_analyze
            // 
            this.btn_analyze.Location = new System.Drawing.Point(132, 114);
            this.btn_analyze.Name = "btn_analyze";
            this.btn_analyze.Size = new System.Drawing.Size(121, 23);
            this.btn_analyze.TabIndex = 7;
            this.btn_analyze.Text = "Analyze";
            this.btn_analyze.UseVisualStyleBackColor = true;
            this.btn_analyze.Click += new System.EventHandler(this.btn_analyze_Click);
            // 
            // combox_unit
            // 
            this.combox_unit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.combox_unit.FormattingEnabled = true;
            this.combox_unit.Items.AddRange(new object[] {
            "m",
            "km",
            "ft",
            "mi"});
            this.combox_unit.Location = new System.Drawing.Point(132, 22);
            this.combox_unit.Name = "combox_unit";
            this.combox_unit.Size = new System.Drawing.Size(50, 21);
            this.combox_unit.TabIndex = 8;
            this.combox_unit.SelectedIndexChanged += new System.EventHandler(this.combox_unit_SelectedIndexChanged);
            // 
            // IIGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 159);
            this.Controls.Add(this.combox_unit);
            this.Controls.Add(this.btn_analyze);
            this.Controls.Add(this.chkbox_con);
            this.Controls.Add(this.chkbox_cat);
            this.Controls.Add(this.txtbox_max);
            this.Controls.Add(this.txtbox_min);
            this.Controls.Add(this.txtbox_seg);
            this.Controls.Add(this.btn_val);
            this.Controls.Add(this.trkbar_seg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IIGUI";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "IndentationIndex";
            ((System.ComponentModel.ISupportInitialize)(this.trkbar_seg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_val;
        private System.Windows.Forms.TrackBar trkbar_seg;
        private System.Windows.Forms.TextBox txtbox_seg;
        private System.Windows.Forms.TextBox txtbox_min;
        private System.Windows.Forms.TextBox txtbox_max;
        private System.Windows.Forms.CheckBox chkbox_cat;
        private System.Windows.Forms.CheckBox chkbox_con;
        private System.Windows.Forms.Button btn_analyze;
        private System.Windows.Forms.ComboBox combox_unit;
    }
}