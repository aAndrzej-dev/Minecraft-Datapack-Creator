
namespace MinecraftDatapackCreator.Forms
{
    partial class AboutForm
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
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblJtfEditorVersion = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.llblMinecraftWiki = new System.Windows.Forms.LinkLabel();
            this.llblLicense = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblInstanceId = new System.Windows.Forms.Label();
            this.lblJTFVersion = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Location = new System.Drawing.Point(36, 124);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(71, 18);
            this.lblCopyright.TabIndex = 4;
            this.lblCopyright.Text = "Copyright";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(36, 100);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(110, 18);
            this.lblVersion.TabIndex = 5;
            this.lblVersion.Text = "ProductVersion";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblName.Location = new System.Drawing.Point(108, 31);
            this.lblName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(289, 39);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Datapack Creator";
            // 
            // lblJtfEditorVersion
            // 
            this.lblJtfEditorVersion.AutoSize = true;
            this.lblJtfEditorVersion.Location = new System.Drawing.Point(36, 148);
            this.lblJtfEditorVersion.Name = "lblJtfEditorVersion";
            this.lblJtfEditorVersion.Size = new System.Drawing.Size(139, 18);
            this.lblJtfEditorVersion.TabIndex = 6;
            this.lblJtfEditorVersion.Text = "JTF Editor Version: ";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(36, 237);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(575, 70);
            this.label1.TabIndex = 7;
            this.label1.Text = "Minecraft Datapack Template Files are based on articles on the Minecraft wiki at " +
    "Fandom and is licensed under the Creative Commons Attribution-Share Alike Licens" +
    "e.";
            // 
            // llblMinecraftWiki
            // 
            this.llblMinecraftWiki.ActiveLinkColor = System.Drawing.Color.MediumOrchid;
            this.llblMinecraftWiki.AutoSize = true;
            this.llblMinecraftWiki.BackColor = System.Drawing.Color.Transparent;
            this.llblMinecraftWiki.LinkColor = System.Drawing.Color.RoyalBlue;
            this.llblMinecraftWiki.Location = new System.Drawing.Point(453, 237);
            this.llblMinecraftWiki.Name = "llblMinecraftWiki";
            this.llblMinecraftWiki.Size = new System.Drawing.Size(69, 18);
            this.llblMinecraftWiki.TabIndex = 8;
            this.llblMinecraftWiki.TabStop = true;
            this.llblMinecraftWiki.Text = "Minecraft";
            this.llblMinecraftWiki.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LlblMinecraftWiki_LinkClicked);
            // 
            // llblLicense
            // 
            this.llblLicense.ActiveLinkColor = System.Drawing.Color.MediumOrchid;
            this.llblLicense.AutoSize = true;
            this.llblLicense.BackColor = System.Drawing.Color.Transparent;
            this.llblLicense.LinkColor = System.Drawing.Color.RoyalBlue;
            this.llblLicense.Location = new System.Drawing.Point(260, 255);
            this.llblLicense.Name = "llblLicense";
            this.llblLicense.Size = new System.Drawing.Size(340, 18);
            this.llblLicense.TabIndex = 8;
            this.llblLicense.TabStop = true;
            this.llblLicense.Text = "Creative Commons Attribution-Share Alike License";
            this.llblLicense.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LlblLicense_LinkClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MinecraftDatapackCreator.Properties.Resources.Icon;
            this.pictureBox1.Location = new System.Drawing.Point(37, 18);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // lblInstanceId
            // 
            this.lblInstanceId.AutoSize = true;
            this.lblInstanceId.Location = new System.Drawing.Point(36, 196);
            this.lblInstanceId.Name = "lblInstanceId";
            this.lblInstanceId.Size = new System.Drawing.Size(85, 18);
            this.lblInstanceId.TabIndex = 6;
            this.lblInstanceId.Text = "Instance ID:";
            // 
            // lblJTFVersion
            // 
            this.lblJTFVersion.AutoSize = true;
            this.lblJTFVersion.Location = new System.Drawing.Point(36, 172);
            this.lblJTFVersion.Name = "lblJTFVersion";
            this.lblJTFVersion.Size = new System.Drawing.Size(92, 18);
            this.lblJTFVersion.TabIndex = 6;
            this.lblJTFVersion.Text = "JTF Version:";
            // 
            // AboutForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(642, 297);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.llblLicense);
            this.Controls.Add(this.llblMinecraftWiki);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblJTFVersion);
            this.Controls.Add(this.lblInstanceId);
            this.Controls.Add(this.lblJtfEditorVersion);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblName);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About Minecraft Datapack Creator";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.AboutForm_HelpButtonClicked);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblName;
        private Label lblJtfEditorVersion;
        private Label label1;
        private LinkLabel llblMinecraftWiki;
        private LinkLabel llblLicense;
        private PictureBox pictureBox1;
        private Label lblInstanceId;
        private Label lblJTFVersion;
    }
}