namespace stend
{
    partial class LirProgForm
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
            this.label6 = new System.Windows.Forms.Label();
            this.intSetPanel = new System.Windows.Forms.Panel();
            this.BitUpDown = new System.Windows.Forms.NumericUpDown();
            this.NetUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ProgBtn = new System.Windows.Forms.Button();
            this.intSetPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(3, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(268, 73);
            this.label6.Text = "Enter the interface settings. Then, insert a special plug into the \"Output\" conne" +
                "ctor. Turn off, then turn on the interface power. Click the Write button";
            // 
            // intSetPanel
            // 
            this.intSetPanel.Controls.Add(this.BitUpDown);
            this.intSetPanel.Controls.Add(this.NetUpDown);
            this.intSetPanel.Controls.Add(this.label4);
            this.intSetPanel.Controls.Add(this.label1);
            this.intSetPanel.Controls.Add(this.label2);
            this.intSetPanel.Location = new System.Drawing.Point(3, 89);
            this.intSetPanel.Name = "intSetPanel";
            this.intSetPanel.Size = new System.Drawing.Size(262, 113);
            // 
            // BitUpDown
            // 
            this.BitUpDown.Location = new System.Drawing.Point(148, 69);
            this.BitUpDown.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.BitUpDown.Name = "BitUpDown";
            this.BitUpDown.Size = new System.Drawing.Size(56, 24);
            this.BitUpDown.TabIndex = 26;
            this.BitUpDown.ValueChanged += new System.EventHandler(this.BitUpDown_ValueChanged);
            // 
            // NetUpDown
            // 
            this.NetUpDown.Location = new System.Drawing.Point(148, 33);
            this.NetUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NetUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NetUpDown.Name = "NetUpDown";
            this.NetUpDown.Size = new System.Drawing.Size(56, 24);
            this.NetUpDown.TabIndex = 25;
            this.NetUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NetUpDown.ValueChanged += new System.EventHandler(this.NetUpDown_ValueChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 20);
            this.label4.Text = "Sensor bit depth:";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular);
            this.label1.ForeColor = System.Drawing.Color.Indigo;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 20);
            this.label1.Text = "Interface settings";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 20);
            this.label2.Text = "Network address:";
            // 
            // ProgBtn
            // 
            this.ProgBtn.Location = new System.Drawing.Point(3, 220);
            this.ProgBtn.Name = "ProgBtn";
            this.ProgBtn.Size = new System.Drawing.Size(88, 38);
            this.ProgBtn.TabIndex = 4;
            this.ProgBtn.Text = "Write";
            this.ProgBtn.Click += new System.EventHandler(this.ProgButton_Click);
            // 
            // LirProgForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(271, 268);
            this.Controls.Add(this.ProgBtn);
            this.Controls.Add(this.intSetPanel);
            this.Controls.Add(this.label6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LirProgForm";
            this.Text = "Lir-916 setting";
            this.intSetPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel intSetPanel;
        private System.Windows.Forms.NumericUpDown BitUpDown;
        private System.Windows.Forms.NumericUpDown NetUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ProgBtn;
    }
}