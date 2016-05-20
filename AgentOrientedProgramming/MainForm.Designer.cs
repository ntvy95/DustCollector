﻿namespace AgentOrientedProgramming
{
    partial class MainForm
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
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inputFromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setRoomsSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setObstaclesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SetRandom_Obstacles = new System.Windows.Forms.ToolStripMenuItem();
            this.SetManually_Obstacles = new System.Windows.Forms.ToolStripMenuItem();
            this.setDustToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SetRandom_Dust = new System.Windows.Forms.ToolStripMenuItem();
            this.SetManually_Dust = new System.Windows.Forms.ToolStripMenuItem();
            this.setAgentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SetRandom_Agent = new System.Windows.Forms.ToolStripMenuItem();
            this.SetManually_Agent_Parent = new System.Windows.Forms.ToolStripMenuItem();
            this.SetManually_Agent = new System.Windows.Forms.ToolStripMenuItem();
            this.SetManually_Agent_Direction = new System.Windows.Forms.ToolStripMenuItem();
            this.SetManually_Agent_UP = new System.Windows.Forms.ToolStripMenuItem();
            this.SetManually_Agent_DOWN = new System.Windows.Forms.ToolStripMenuItem();
            this.SetManually_Agent_LEFT = new System.Windows.Forms.ToolStripMenuItem();
            this.SetManually_Agent_RIGHT = new System.Windows.Forms.ToolStripMenuItem();
            this.controlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.controlToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(709, 24);
            this.MainMenu.TabIndex = 0;
            this.MainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inputFromFileToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // inputFromFileToolStripMenuItem
            // 
            this.inputFromFileToolStripMenuItem.Name = "inputFromFileToolStripMenuItem";
            this.inputFromFileToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.inputFromFileToolStripMenuItem.Text = "Input from file...";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setRoomsSizeToolStripMenuItem,
            this.setObstaclesToolStripMenuItem,
            this.setDustToolStripMenuItem,
            this.setAgentToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // setRoomsSizeToolStripMenuItem
            // 
            this.setRoomsSizeToolStripMenuItem.Name = "setRoomsSizeToolStripMenuItem";
            this.setRoomsSizeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.setRoomsSizeToolStripMenuItem.Text = "Set room\'s size";
            this.setRoomsSizeToolStripMenuItem.Click += new System.EventHandler(this.SetRoomSize_Click);
            // 
            // setObstaclesToolStripMenuItem
            // 
            this.setObstaclesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SetRandom_Obstacles,
            this.SetManually_Obstacles});
            this.setObstaclesToolStripMenuItem.Name = "setObstaclesToolStripMenuItem";
            this.setObstaclesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.setObstaclesToolStripMenuItem.Text = "Set obstacles";
            // 
            // SetRandom_Obstacles
            // 
            this.SetRandom_Obstacles.Name = "SetRandom_Obstacles";
            this.SetRandom_Obstacles.Size = new System.Drawing.Size(152, 22);
            this.SetRandom_Obstacles.Text = "Set random";
            this.SetRandom_Obstacles.Click += new System.EventHandler(this.SetRandom_Obstacles_Click);
            // 
            // SetManually_Obstacles
            // 
            this.SetManually_Obstacles.Name = "SetManually_Obstacles";
            this.SetManually_Obstacles.Size = new System.Drawing.Size(152, 22);
            this.SetManually_Obstacles.Text = "Set manually";
            this.SetManually_Obstacles.Click += new System.EventHandler(this.SetManually_Obstacles_Click);
            // 
            // setDustToolStripMenuItem
            // 
            this.setDustToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SetRandom_Dust,
            this.SetManually_Dust});
            this.setDustToolStripMenuItem.Name = "setDustToolStripMenuItem";
            this.setDustToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.setDustToolStripMenuItem.Text = "Set dust";
            // 
            // SetRandom_Dust
            // 
            this.SetRandom_Dust.Name = "SetRandom_Dust";
            this.SetRandom_Dust.Size = new System.Drawing.Size(152, 22);
            this.SetRandom_Dust.Text = "Set random";
            this.SetRandom_Dust.Click += new System.EventHandler(this.SetRandom_Dust_Click);
            // 
            // SetManually_Dust
            // 
            this.SetManually_Dust.Name = "SetManually_Dust";
            this.SetManually_Dust.Size = new System.Drawing.Size(152, 22);
            this.SetManually_Dust.Text = "Set manually";
            this.SetManually_Dust.Click += new System.EventHandler(this.SetManually_Dust_Click);
            // 
            // setAgentToolStripMenuItem
            // 
            this.setAgentToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SetRandom_Agent,
            this.SetManually_Agent_Parent});
            this.setAgentToolStripMenuItem.Name = "setAgentToolStripMenuItem";
            this.setAgentToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.setAgentToolStripMenuItem.Text = "Set agent";
            // 
            // SetRandom_Agent
            // 
            this.SetRandom_Agent.Name = "SetRandom_Agent";
            this.SetRandom_Agent.Size = new System.Drawing.Size(152, 22);
            this.SetRandom_Agent.Text = "Set random";
            this.SetRandom_Agent.Click += new System.EventHandler(this.SetRandom_Agent_Click);
            // 
            // SetManually_Agent_Parent
            // 
            this.SetManually_Agent_Parent.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SetManually_Agent,
            this.SetManually_Agent_Direction});
            this.SetManually_Agent_Parent.Name = "SetManually_Agent_Parent";
            this.SetManually_Agent_Parent.Size = new System.Drawing.Size(152, 22);
            this.SetManually_Agent_Parent.Text = "Set manually";
            // 
            // SetManually_Agent
            // 
            this.SetManually_Agent.Name = "SetManually_Agent";
            this.SetManually_Agent.Size = new System.Drawing.Size(122, 22);
            this.SetManually_Agent.Text = "Set up";
            this.SetManually_Agent.Click += new System.EventHandler(this.SetManually_Agent_Click);
            // 
            // SetManually_Agent_Direction
            // 
            this.SetManually_Agent_Direction.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SetManually_Agent_UP,
            this.SetManually_Agent_DOWN,
            this.SetManually_Agent_LEFT,
            this.SetManually_Agent_RIGHT});
            this.SetManually_Agent_Direction.Enabled = false;
            this.SetManually_Agent_Direction.Name = "SetManually_Agent_Direction";
            this.SetManually_Agent_Direction.Size = new System.Drawing.Size(122, 22);
            this.SetManually_Agent_Direction.Text = "Direction";
            // 
            // SetManually_Agent_UP
            // 
            this.SetManually_Agent_UP.Name = "SetManually_Agent_UP";
            this.SetManually_Agent_UP.Size = new System.Drawing.Size(111, 22);
            this.SetManually_Agent_UP.Text = "UP";
            this.SetManually_Agent_UP.Click += new System.EventHandler(this.SetManually_Agent_UP_Click);
            // 
            // SetManually_Agent_DOWN
            // 
            this.SetManually_Agent_DOWN.Name = "SetManually_Agent_DOWN";
            this.SetManually_Agent_DOWN.Size = new System.Drawing.Size(111, 22);
            this.SetManually_Agent_DOWN.Text = "DOWN";
            this.SetManually_Agent_DOWN.Click += new System.EventHandler(this.SetManually_Agent_DOWN_Click);
            // 
            // SetManually_Agent_LEFT
            // 
            this.SetManually_Agent_LEFT.Name = "SetManually_Agent_LEFT";
            this.SetManually_Agent_LEFT.Size = new System.Drawing.Size(111, 22);
            this.SetManually_Agent_LEFT.Text = "LEFT";
            this.SetManually_Agent_LEFT.Click += new System.EventHandler(this.SetManually_Agent_LEFT_Click);
            // 
            // SetManually_Agent_RIGHT
            // 
            this.SetManually_Agent_RIGHT.Name = "SetManually_Agent_RIGHT";
            this.SetManually_Agent_RIGHT.Size = new System.Drawing.Size(111, 22);
            this.SetManually_Agent_RIGHT.Text = "RIGHT";
            this.SetManually_Agent_RIGHT.Click += new System.EventHandler(this.SetManually_Agent_RIGHT_Click);
            // 
            // controlToolStripMenuItem
            // 
            this.controlToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem});
            this.controlToolStripMenuItem.Name = "controlToolStripMenuItem";
            this.controlToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.controlToolStripMenuItem.Text = "Control";
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.startToolStripMenuItem.Text = "Start";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // MainPanel
            // 
            this.MainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.MainPanel.Location = new System.Drawing.Point(0, 27);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(709, 294);
            this.MainPanel.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 322);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.MainMenu);
            this.MainMenuStrip = this.MainMenu;
            this.Name = "MainForm";
            this.Text = "Dust Collector";
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inputFromFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setRoomsSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setObstaclesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem controlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setAgentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SetRandom_Obstacles;
        private System.Windows.Forms.ToolStripMenuItem SetManually_Obstacles;
        private System.Windows.Forms.ToolStripMenuItem setDustToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SetRandom_Dust;
        private System.Windows.Forms.ToolStripMenuItem SetManually_Dust;
        private System.Windows.Forms.ToolStripMenuItem SetRandom_Agent;
        private System.Windows.Forms.ToolStripMenuItem SetManually_Agent_Parent;
        private System.Windows.Forms.ToolStripMenuItem SetManually_Agent;
        private System.Windows.Forms.ToolStripMenuItem SetManually_Agent_Direction;
        private System.Windows.Forms.ToolStripMenuItem SetManually_Agent_UP;
        private System.Windows.Forms.ToolStripMenuItem SetManually_Agent_DOWN;
        private System.Windows.Forms.ToolStripMenuItem SetManually_Agent_LEFT;
        private System.Windows.Forms.ToolStripMenuItem SetManually_Agent_RIGHT;
    }
}

