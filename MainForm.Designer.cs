namespace stend
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.SettingsMenu = new System.Windows.Forms.MenuItem();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.ControlPage = new System.Windows.Forms.TabPage();
            this.ConEnCheck = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.SpdLabel = new System.Windows.Forms.Label();
            this.MovLabel = new System.Windows.Forms.Label();
            this.ComprLabel = new System.Windows.Forms.Label();
            this.StrLabel = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.StrainLoadLabel = new System.Windows.Forms.Label();
            this.SpdText = new System.Windows.Forms.TextBox();
            this.MovingText = new System.Windows.Forms.TextBox();
            this.StrText = new System.Windows.Forms.TextBox();
            this.ComprText = new System.Windows.Forms.TextBox();
            this.StrainLoadText = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.LogPage = new System.Windows.Forms.TabPage();
            this.LogList = new System.Windows.Forms.ListBox();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.MainTabControl.SuspendLayout();
            this.ControlPage.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.LogPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            // 
            // menuItem1
            // 
            this.menuItem1.MenuItems.Add(this.SettingsMenu);
            this.menuItem1.Text = "Options";
            // 
            // SettingsMenu
            // 
            this.SettingsMenu.Text = "Hardware settings";
            this.SettingsMenu.Click += new System.EventHandler(this.SettingsMenu_Click);
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.ControlPage);
            this.MainTabControl.Controls.Add(this.LogPage);
            this.MainTabControl.Location = new System.Drawing.Point(0, 44);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(769, 349);
            this.MainTabControl.TabIndex = 0;
            // 
            // ControlPage
            // 
            this.ControlPage.Controls.Add(this.ConEnCheck);
            this.ControlPage.Controls.Add(this.panel3);
            this.ControlPage.Controls.Add(this.panel2);
            this.ControlPage.Controls.Add(this.panel1);
            this.ControlPage.Location = new System.Drawing.Point(4, 25);
            this.ControlPage.Name = "ControlPage";
            this.ControlPage.Size = new System.Drawing.Size(761, 320);
            this.ControlPage.Text = "Control";
            // 
            // ConEnCheck
            // 
            this.ConEnCheck.Location = new System.Drawing.Point(3, 13);
            this.ConEnCheck.Name = "ConEnCheck";
            this.ConEnCheck.Size = new System.Drawing.Size(119, 20);
            this.ConEnCheck.TabIndex = 3;
            this.ConEnCheck.Text = "Enable control";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.SpdLabel);
            this.panel3.Controls.Add(this.MovLabel);
            this.panel3.Controls.Add(this.ComprLabel);
            this.panel3.Controls.Add(this.StrLabel);
            this.panel3.Controls.Add(this.button7);
            this.panel3.Controls.Add(this.StrainLoadLabel);
            this.panel3.Controls.Add(this.SpdText);
            this.panel3.Controls.Add(this.MovingText);
            this.panel3.Controls.Add(this.StrText);
            this.panel3.Controls.Add(this.ComprText);
            this.panel3.Controls.Add(this.StrainLoadText);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Location = new System.Drawing.Point(439, 39);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(320, 272);
            // 
            // SpdLabel
            // 
            this.SpdLabel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular);
            this.SpdLabel.Location = new System.Drawing.Point(194, 200);
            this.SpdLabel.Name = "SpdLabel";
            this.SpdLabel.Size = new System.Drawing.Size(37, 20);
            this.SpdLabel.Text = "n/a";
            // 
            // MovLabel
            // 
            this.MovLabel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular);
            this.MovLabel.Location = new System.Drawing.Point(194, 165);
            this.MovLabel.Name = "MovLabel";
            this.MovLabel.Size = new System.Drawing.Size(37, 20);
            this.MovLabel.Text = "n/a";
            // 
            // ComprLabel
            // 
            this.ComprLabel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular);
            this.ComprLabel.Location = new System.Drawing.Point(194, 89);
            this.ComprLabel.Name = "ComprLabel";
            this.ComprLabel.Size = new System.Drawing.Size(37, 20);
            this.ComprLabel.Text = "n/a";
            // 
            // StrLabel
            // 
            this.StrLabel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular);
            this.StrLabel.Location = new System.Drawing.Point(194, 127);
            this.StrLabel.Name = "StrLabel";
            this.StrLabel.Size = new System.Drawing.Size(37, 20);
            this.StrLabel.Text = "n/a";
            // 
            // button7
            // 
            this.button7.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular);
            this.button7.Location = new System.Drawing.Point(237, 162);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(72, 23);
            this.button7.TabIndex = 25;
            this.button7.Text = "Zero";
            // 
            // StrainLoadLabel
            // 
            this.StrainLoadLabel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular);
            this.StrainLoadLabel.Location = new System.Drawing.Point(194, 50);
            this.StrainLoadLabel.Name = "StrainLoadLabel";
            this.StrainLoadLabel.Size = new System.Drawing.Size(37, 20);
            this.StrainLoadLabel.Text = "n/a";
            // 
            // SpdText
            // 
            this.SpdText.Location = new System.Drawing.Point(89, 197);
            this.SpdText.Name = "SpdText";
            this.SpdText.ReadOnly = true;
            this.SpdText.Size = new System.Drawing.Size(100, 23);
            this.SpdText.TabIndex = 11;
            // 
            // MovingText
            // 
            this.MovingText.Location = new System.Drawing.Point(89, 162);
            this.MovingText.Name = "MovingText";
            this.MovingText.ReadOnly = true;
            this.MovingText.Size = new System.Drawing.Size(100, 23);
            this.MovingText.TabIndex = 9;
            // 
            // StrText
            // 
            this.StrText.Location = new System.Drawing.Point(88, 124);
            this.StrText.Name = "StrText";
            this.StrText.ReadOnly = true;
            this.StrText.Size = new System.Drawing.Size(100, 23);
            this.StrText.TabIndex = 8;
            // 
            // ComprText
            // 
            this.ComprText.Location = new System.Drawing.Point(88, 86);
            this.ComprText.Name = "ComprText";
            this.ComprText.ReadOnly = true;
            this.ComprText.Size = new System.Drawing.Size(100, 23);
            this.ComprText.TabIndex = 7;
            // 
            // StrainLoadText
            // 
            this.StrainLoadText.Location = new System.Drawing.Point(88, 47);
            this.StrainLoadText.Name = "StrainLoadText";
            this.StrainLoadText.ReadOnly = true;
            this.StrainLoadText.Size = new System.Drawing.Size(100, 23);
            this.StrainLoadText.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular);
            this.label8.Location = new System.Drawing.Point(15, 200);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 20);
            this.label8.Text = "Speed";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular);
            this.label7.Location = new System.Drawing.Point(15, 165);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 20);
            this.label7.Text = "Moving";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular);
            this.label6.Location = new System.Drawing.Point(15, 127);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 20);
            this.label6.Text = "Stretch.";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular);
            this.label5.Location = new System.Drawing.Point(15, 89);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 20);
            this.label5.Text = "Compr.";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.label3.ForeColor = System.Drawing.Color.Indigo;
            this.label3.Location = new System.Drawing.Point(15, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 30);
            this.label3.Text = "Sensors";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular);
            this.label4.Location = new System.Drawing.Point(15, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 20);
            this.label4.Text = "Str. load";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.button6);
            this.panel2.Controls.Add(this.button5);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Location = new System.Drawing.Point(3, 39);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(430, 119);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.label2.ForeColor = System.Drawing.Color.Indigo;
            this.label2.Location = new System.Drawing.Point(19, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 32);
            this.label2.Text = "Oil pump";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(288, 50);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(119, 41);
            this.button6.TabIndex = 2;
            this.button6.Text = "Hi. pressure";
            this.button6.Click += new System.EventHandler(this.MovBtn_click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(153, 50);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(119, 41);
            this.button5.TabIndex = 1;
            this.button5.Text = "Low pressure";
            this.button5.Click += new System.EventHandler(this.MovBtn_click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(17, 50);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(119, 41);
            this.button4.TabIndex = 0;
            this.button4.Text = "On/Off";
            this.button4.Click += new System.EventHandler(this.MovBtn_click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Location = new System.Drawing.Point(3, 182);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(430, 129);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.label1.ForeColor = System.Drawing.Color.Indigo;
            this.label1.Location = new System.Drawing.Point(17, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 29);
            this.label1.Text = "Cylinder movement";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(288, 57);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(119, 41);
            this.button3.TabIndex = 2;
            this.button3.Text = "Stop";
            this.button3.Click += new System.EventHandler(this.PmpBtn_click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(153, 57);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(119, 41);
            this.button2.TabIndex = 1;
            this.button2.Text = "Backward";
            this.button2.Click += new System.EventHandler(this.PmpBtn_click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(17, 57);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 41);
            this.button1.TabIndex = 0;
            this.button1.Text = "Forward";
            this.button1.Click += new System.EventHandler(this.PmpBtn_click);
            // 
            // LogPage
            // 
            this.LogPage.Controls.Add(this.LogList);
            this.LogPage.Location = new System.Drawing.Point(4, 25);
            this.LogPage.Name = "LogPage";
            this.LogPage.Size = new System.Drawing.Size(761, 320);
            this.LogPage.Text = "Log";
            // 
            // LogList
            // 
            this.LogList.Location = new System.Drawing.Point(-1, 13);
            this.LogList.Name = "LogList";
            this.LogList.Size = new System.Drawing.Size(763, 306);
            this.LogList.TabIndex = 0;
            this.LogList.Tag = "LogListBox";
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 393);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(769, 24);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(769, 417);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.MainTabControl);
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "StendGA";
            this.MainTabControl.ResumeLayout(false);
            this.ControlPage.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.LogPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem SettingsMenu;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage ControlPage;
        private System.Windows.Forms.TabPage LogPage;
        private System.Windows.Forms.ListBox LogList;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label StrainLoadLabel;
        private System.Windows.Forms.TextBox SpdText;
        private System.Windows.Forms.TextBox MovingText;
        private System.Windows.Forms.TextBox StrText;
        private System.Windows.Forms.TextBox ComprText;
        private System.Windows.Forms.TextBox StrainLoadText;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label SpdLabel;
        private System.Windows.Forms.Label MovLabel;
        private System.Windows.Forms.Label ComprLabel;
        private System.Windows.Forms.Label StrLabel;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.StatusBar statusBar;
        private System.Windows.Forms.CheckBox ConEnCheck;
    }
}

