/*
 * Copyright (C) 2024 Marshall Lalonde (AKA xxxman360)
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
 */

namespace LogonVistaOrbInit
{
    partial class Configuration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Configuration));
            this.logonCheck = new System.Windows.Forms.CheckBox();
            this.startupCheck = new System.Windows.Forms.CheckBox();
            this.enableCheck = new System.Windows.Forms.CheckBox();
            this.audioCheck = new System.Windows.Forms.CheckBox();
            this.bkgClrLabel = new System.Windows.Forms.Label();
            this.colorText = new System.Windows.Forms.TextBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.colorPicker = new System.Windows.Forms.Button();
            this.shutdownCheck = new System.Windows.Forms.CheckBox();
            this.schemeCheck = new System.Windows.Forms.CheckBox();
            this.customstartupCheck = new System.Windows.Forms.CheckBox();
            this.StartupSoundFilePath = new System.Windows.Forms.TextBox();
            this.BrowseStartupFileButton = new System.Windows.Forms.Button();
            this.StartupTest = new System.Windows.Forms.Button();
            this.logoffCheck = new System.Windows.Forms.CheckBox();
            this.unlockCheck = new System.Windows.Forms.CheckBox();
            this.lockCheck = new System.Windows.Forms.CheckBox();
            this.animationCheck = new System.Windows.Forms.CheckBox();
            this.pictureORB = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureORB)).BeginInit();
            this.SuspendLayout();
            // 
            // logonCheck
            // 
            this.logonCheck.AutoSize = true;
            this.logonCheck.Checked = true;
            this.logonCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.logonCheck.Location = new System.Drawing.Point(25, 145);
            this.logonCheck.Name = "logonCheck";
            this.logonCheck.Size = new System.Drawing.Size(111, 17);
            this.logonCheck.TabIndex = 0;
            this.logonCheck.Text = "Play Logon sound";
            this.logonCheck.UseVisualStyleBackColor = true;
            this.logonCheck.CheckedChanged += new System.EventHandler(this.logonCheck_CheckedChanged);
            // 
            // startupCheck
            // 
            this.startupCheck.AutoSize = true;
            this.startupCheck.Checked = true;
            this.startupCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.startupCheck.Location = new System.Drawing.Point(25, 72);
            this.startupCheck.Name = "startupCheck";
            this.startupCheck.Size = new System.Drawing.Size(115, 17);
            this.startupCheck.TabIndex = 1;
            this.startupCheck.Text = "Play Startup sound";
            this.startupCheck.UseVisualStyleBackColor = true;
            this.startupCheck.CheckedChanged += new System.EventHandler(this.startupCheck_CheckedChanged);
            // 
            // enableCheck
            // 
            this.enableCheck.AutoSize = true;
            this.enableCheck.Checked = true;
            this.enableCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.enableCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enableCheck.Location = new System.Drawing.Point(12, 12);
            this.enableCheck.Name = "enableCheck";
            this.enableCheck.Size = new System.Drawing.Size(152, 17);
            this.enableCheck.TabIndex = 2;
            this.enableCheck.Text = "Enable LogonVistaOrb";
            this.enableCheck.UseVisualStyleBackColor = true;
            this.enableCheck.CheckedChanged += new System.EventHandler(this.enableCheck_CheckedChanged);
            // 
            // audioCheck
            // 
            this.audioCheck.AutoSize = true;
            this.audioCheck.Checked = true;
            this.audioCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.audioCheck.Location = new System.Drawing.Point(12, 309);
            this.audioCheck.Name = "audioCheck";
            this.audioCheck.Size = new System.Drawing.Size(256, 30);
            this.audioCheck.TabIndex = 3;
            this.audioCheck.Text = "Wait for audio services before running animation \n(Recommended for slower compute" +
    "rs)";
            this.audioCheck.UseVisualStyleBackColor = true;
            this.audioCheck.CheckedChanged += new System.EventHandler(this.audioCheck_CheckedChanged);
            // 
            // bkgClrLabel
            // 
            this.bkgClrLabel.AutoSize = true;
            this.bkgClrLabel.Location = new System.Drawing.Point(134, 356);
            this.bkgClrLabel.Name = "bkgClrLabel";
            this.bkgClrLabel.Size = new System.Drawing.Size(95, 13);
            this.bkgClrLabel.TabIndex = 4;
            this.bkgClrLabel.Text = "Background Color:";
            this.bkgClrLabel.Click += new System.EventHandler(this.bkgClrLabel_Click);
            // 
            // colorText
            // 
            this.colorText.Location = new System.Drawing.Point(235, 353);
            this.colorText.Name = "colorText";
            this.colorText.Size = new System.Drawing.Size(100, 20);
            this.colorText.TabIndex = 5;
            this.colorText.Text = "#FF000000";
            this.colorText.TextChanged += new System.EventHandler(this.colorText_TextChanged);
            // 
            // colorDialog1
            // 
            this.colorDialog1.AnyColor = true;
            this.colorDialog1.FullOpen = true;
            this.colorDialog1.SolidColorOnly = true;
            // 
            // colorPicker
            // 
            this.colorPicker.Location = new System.Drawing.Point(341, 342);
            this.colorPicker.Name = "colorPicker";
            this.colorPicker.Size = new System.Drawing.Size(75, 40);
            this.colorPicker.TabIndex = 6;
            this.colorPicker.Text = "Choose Color";
            this.colorPicker.UseVisualStyleBackColor = true;
            this.colorPicker.Click += new System.EventHandler(this.colorPicker_Click);
            // 
            // shutdownCheck
            // 
            this.shutdownCheck.AutoSize = true;
            this.shutdownCheck.Checked = true;
            this.shutdownCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.shutdownCheck.Location = new System.Drawing.Point(25, 237);
            this.shutdownCheck.Name = "shutdownCheck";
            this.shutdownCheck.Size = new System.Drawing.Size(129, 17);
            this.shutdownCheck.TabIndex = 7;
            this.shutdownCheck.Text = "Play Shutdown sound";
            this.shutdownCheck.UseVisualStyleBackColor = true;
            this.shutdownCheck.CheckedChanged += new System.EventHandler(this.shutdownCheck_CheckedChanged);
            // 
            // schemeCheck
            // 
            this.schemeCheck.AutoSize = true;
            this.schemeCheck.Checked = true;
            this.schemeCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.schemeCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.schemeCheck.Location = new System.Drawing.Point(12, 286);
            this.schemeCheck.Name = "schemeCheck";
            this.schemeCheck.Size = new System.Drawing.Size(195, 17);
            this.schemeCheck.TabIndex = 8;
            this.schemeCheck.Text = "Use sounds from my Sound scheme";
            this.schemeCheck.UseVisualStyleBackColor = true;
            this.schemeCheck.CheckedChanged += new System.EventHandler(this.schemeCheck_CheckedChanged);
            // 
            // customstartupCheck
            // 
            this.customstartupCheck.AutoSize = true;
            this.customstartupCheck.Checked = true;
            this.customstartupCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.customstartupCheck.Location = new System.Drawing.Point(40, 95);
            this.customstartupCheck.Name = "customstartupCheck";
            this.customstartupCheck.Size = new System.Drawing.Size(143, 17);
            this.customstartupCheck.TabIndex = 9;
            this.customstartupCheck.Text = "Customise Startup sound";
            this.customstartupCheck.UseVisualStyleBackColor = true;
            this.customstartupCheck.CheckedChanged += new System.EventHandler(this.customstartupCheck_CheckedChanged);
            // 
            // StartupSoundFilePath
            // 
            this.StartupSoundFilePath.Location = new System.Drawing.Point(12, 118);
            this.StartupSoundFilePath.Name = "StartupSoundFilePath";
            this.StartupSoundFilePath.Size = new System.Drawing.Size(306, 20);
            this.StartupSoundFilePath.TabIndex = 12;
            this.StartupSoundFilePath.TextChanged += new System.EventHandler(this.StartupSoundFilePath_TextChanged);
            // 
            // BrowseStartupFileButton
            // 
            this.BrowseStartupFileButton.Location = new System.Drawing.Point(356, 115);
            this.BrowseStartupFileButton.Name = "BrowseStartupFileButton";
            this.BrowseStartupFileButton.Size = new System.Drawing.Size(60, 25);
            this.BrowseStartupFileButton.TabIndex = 11;
            this.BrowseStartupFileButton.Text = "Browse";
            this.BrowseStartupFileButton.UseVisualStyleBackColor = true;
            this.BrowseStartupFileButton.Click += new System.EventHandler(this.BrowseStartupFileButton_Click);
            // 
            // StartupTest
            // 
            this.StartupTest.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("StartupTest.BackgroundImage")));
            this.StartupTest.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.StartupTest.Cursor = System.Windows.Forms.Cursors.Default;
            this.StartupTest.FlatAppearance.BorderSize = 5;
            this.StartupTest.Location = new System.Drawing.Point(319, 116);
            this.StartupTest.Name = "StartupTest";
            this.StartupTest.Size = new System.Drawing.Size(24, 24);
            this.StartupTest.TabIndex = 13;
            this.StartupTest.UseVisualStyleBackColor = true;
            this.StartupTest.Click += new System.EventHandler(this.StartupTest_Click);
            // 
            // logoffCheck
            // 
            this.logoffCheck.AutoSize = true;
            this.logoffCheck.Checked = true;
            this.logoffCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.logoffCheck.Location = new System.Drawing.Point(25, 168);
            this.logoffCheck.Name = "logoffCheck";
            this.logoffCheck.Size = new System.Drawing.Size(111, 17);
            this.logoffCheck.TabIndex = 14;
            this.logoffCheck.Text = "Play Logoff sound";
            this.logoffCheck.UseVisualStyleBackColor = true;
            this.logoffCheck.CheckedChanged += new System.EventHandler(this.logoffCheck_CheckedChanged);
            // 
            // unlockCheck
            // 
            this.unlockCheck.AutoSize = true;
            this.unlockCheck.Checked = true;
            this.unlockCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.unlockCheck.Location = new System.Drawing.Point(25, 191);
            this.unlockCheck.Name = "unlockCheck";
            this.unlockCheck.Size = new System.Drawing.Size(115, 17);
            this.unlockCheck.TabIndex = 15;
            this.unlockCheck.Text = "Play Unlock sound";
            this.unlockCheck.UseVisualStyleBackColor = true;
            this.unlockCheck.CheckedChanged += new System.EventHandler(this.unlockCheck_CheckedChanged);
            // 
            // lockCheck
            // 
            this.lockCheck.AutoSize = true;
            this.lockCheck.Checked = true;
            this.lockCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.lockCheck.Location = new System.Drawing.Point(25, 214);
            this.lockCheck.Name = "lockCheck";
            this.lockCheck.Size = new System.Drawing.Size(105, 17);
            this.lockCheck.TabIndex = 16;
            this.lockCheck.Text = "Play Lock sound";
            this.lockCheck.UseVisualStyleBackColor = true;
            this.lockCheck.CheckedChanged += new System.EventHandler(this.lockCheck_CheckedChanged);
            // 
            // animationCheck
            // 
            this.animationCheck.AutoSize = true;
            this.animationCheck.Checked = true;
            this.animationCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.animationCheck.Location = new System.Drawing.Point(25, 43);
            this.animationCheck.Name = "animationCheck";
            this.animationCheck.Size = new System.Drawing.Size(244, 17);
            this.animationCheck.TabIndex = 17;
            this.animationCheck.Text = "Enable Windows Vista Startup ORB Animation";
            this.animationCheck.UseVisualStyleBackColor = true;
            this.animationCheck.CheckedChanged += new System.EventHandler(this.animationCheck_CheckedChanged);
            // 
            // pictureORB
            // 
            this.pictureORB.BackColor = System.Drawing.Color.Transparent;
            this.pictureORB.Enabled = false;
            this.pictureORB.ErrorImage = null;
            this.pictureORB.Image = global::LogonVistaOrbInit.Properties.Resources.icon;
            this.pictureORB.InitialImage = null;
            this.pictureORB.Location = new System.Drawing.Point(270, 36);
            this.pictureORB.Name = "pictureORB";
            this.pictureORB.Size = new System.Drawing.Size(30, 30);
            this.pictureORB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureORB.TabIndex = 18;
            this.pictureORB.TabStop = false;
            this.pictureORB.Click += new System.EventHandler(this.VistaORB);
            // 
            // Configuration
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(428, 392);
            this.Controls.Add(this.pictureORB);
            this.Controls.Add(this.animationCheck);
            this.Controls.Add(this.lockCheck);
            this.Controls.Add(this.unlockCheck);
            this.Controls.Add(this.logoffCheck);
            this.Controls.Add(this.StartupTest);
            this.Controls.Add(this.BrowseStartupFileButton);
            this.Controls.Add(this.StartupSoundFilePath);
            this.Controls.Add(this.customstartupCheck);
            this.Controls.Add(this.schemeCheck);
            this.Controls.Add(this.shutdownCheck);
            this.Controls.Add(this.colorPicker);
            this.Controls.Add(this.colorText);
            this.Controls.Add(this.bkgClrLabel);
            this.Controls.Add(this.audioCheck);
            this.Controls.Add(this.enableCheck);
            this.Controls.Add(this.startupCheck);
            this.Controls.Add(this.logonCheck);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Configuration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AttemptedClose);
            this.Load += new System.EventHandler(this.Configuration_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureORB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox logonCheck;
        private System.Windows.Forms.CheckBox startupCheck;
        private System.Windows.Forms.CheckBox enableCheck;
        private System.Windows.Forms.CheckBox audioCheck;
        private System.Windows.Forms.Label bkgClrLabel;
        private System.Windows.Forms.TextBox colorText;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button colorPicker;
        private System.Windows.Forms.CheckBox shutdownCheck;
        private System.Windows.Forms.CheckBox schemeCheck;
        private System.Windows.Forms.CheckBox customstartupCheck;
        private System.Windows.Forms.TextBox StartupSoundFilePath;
        private System.Windows.Forms.Button BrowseStartupFileButton;
        private System.Windows.Forms.Button StartupTest;
        private System.Windows.Forms.CheckBox logoffCheck;
        private System.Windows.Forms.CheckBox unlockCheck;
        private System.Windows.Forms.CheckBox lockCheck;
        private System.Windows.Forms.CheckBox animationCheck;
        private System.Windows.Forms.PictureBox pictureORB;
    }
}

