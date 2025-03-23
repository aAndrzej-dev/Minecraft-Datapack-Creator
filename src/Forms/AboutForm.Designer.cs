
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            lblCopyright = new Label();
            lblVersion = new Label();
            lblName = new Label();
            lblJtfEditorVersion = new Label();
            label1 = new Label();
            llblMinecraftWiki = new LinkLabel();
            llblLicense = new LinkLabel();
            pictureBox1 = new PictureBox();
            lblInstanceId = new Label();
            lblJTFVersion = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // lblCopyright
            // 
            lblCopyright.AutoSize = true;
            lblCopyright.Location = new Point(36, 124);
            lblCopyright.Name = "lblCopyright";
            lblCopyright.Size = new Size(71, 18);
            lblCopyright.TabIndex = 4;
            lblCopyright.Text = "Copyright";
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Location = new Point(36, 100);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(110, 18);
            lblVersion.TabIndex = 5;
            lblVersion.Text = "ProductVersion";
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Font = new Font("Microsoft Sans Serif", 26.25F, FontStyle.Regular, GraphicsUnit.Point);
            lblName.Location = new Point(108, 31);
            lblName.Margin = new Padding(4, 0, 4, 0);
            lblName.Name = "lblName";
            lblName.Size = new Size(289, 39);
            lblName.TabIndex = 3;
            lblName.Text = "Datapack Creator";
            // 
            // lblJtfEditorVersion
            // 
            lblJtfEditorVersion.AutoSize = true;
            lblJtfEditorVersion.Location = new Point(36, 148);
            lblJtfEditorVersion.Name = "lblJtfEditorVersion";
            lblJtfEditorVersion.Size = new Size(139, 18);
            lblJtfEditorVersion.TabIndex = 6;
            lblJtfEditorVersion.Text = "JTF Editor Version: ";
            // 
            // label1
            // 
            label1.Location = new Point(36, 237);
            label1.Name = "label1";
            label1.Size = new Size(575, 70);
            label1.TabIndex = 7;
            label1.Text = "Minecraft Datapack Template Files are based on articles on the Minecraft wiki at Fandom and is licensed under the Creative Commons Attribution-Share Alike License.";
            // 
            // llblMinecraftWiki
            // 
            llblMinecraftWiki.ActiveLinkColor = Color.MediumOrchid;
            llblMinecraftWiki.AutoSize = true;
            llblMinecraftWiki.BackColor = Color.Transparent;
            llblMinecraftWiki.LinkColor = Color.RoyalBlue;
            llblMinecraftWiki.Location = new Point(453, 237);
            llblMinecraftWiki.Name = "llblMinecraftWiki";
            llblMinecraftWiki.Size = new Size(69, 18);
            llblMinecraftWiki.TabIndex = 8;
            llblMinecraftWiki.TabStop = true;
            llblMinecraftWiki.Text = "Minecraft";
            llblMinecraftWiki.LinkClicked += LlblMinecraftWiki_LinkClicked;
            // 
            // llblLicense
            // 
            llblLicense.ActiveLinkColor = Color.MediumOrchid;
            llblLicense.AutoSize = true;
            llblLicense.BackColor = Color.Transparent;
            llblLicense.LinkColor = Color.RoyalBlue;
            llblLicense.Location = new Point(260, 255);
            llblLicense.Name = "llblLicense";
            llblLicense.Size = new Size(340, 18);
            llblLicense.TabIndex = 8;
            llblLicense.TabStop = true;
            llblLicense.Text = "Creative Commons Attribution-Share Alike License";
            llblLicense.LinkClicked += LlblLicense_LinkClicked;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.Icon;
            pictureBox1.Location = new Point(37, 18);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(64, 64);
            pictureBox1.TabIndex = 9;
            pictureBox1.TabStop = false;
            // 
            // lblInstanceId
            // 
            lblInstanceId.AutoSize = true;
            lblInstanceId.Location = new Point(36, 196);
            lblInstanceId.Name = "lblInstanceId";
            lblInstanceId.Size = new Size(85, 18);
            lblInstanceId.TabIndex = 6;
            lblInstanceId.Text = "Instance ID:";
            // 
            // lblJTFVersion
            // 
            lblJTFVersion.AutoSize = true;
            lblJTFVersion.Location = new Point(36, 172);
            lblJTFVersion.Name = "lblJTFVersion";
            lblJTFVersion.Size = new Size(92, 18);
            lblJTFVersion.TabIndex = 6;
            lblJTFVersion.Text = "JTF Version:";
            // 
            // AboutForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(642, 297);
            Controls.Add(pictureBox1);
            Controls.Add(llblLicense);
            Controls.Add(llblMinecraftWiki);
            Controls.Add(label1);
            Controls.Add(lblJTFVersion);
            Controls.Add(lblInstanceId);
            Controls.Add(lblJtfEditorVersion);
            Controls.Add(lblCopyright);
            Controls.Add(lblVersion);
            Controls.Add(lblName);
            Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            HelpButton = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AboutForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "About Minecraft Datapack Creator";
            HelpButtonClicked += AboutForm_HelpButtonClicked;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label lblCopyright;
        private Label lblVersion;
        private Label lblName;
        private Label lblJtfEditorVersion;
        private Label label1;
        private LinkLabel llblMinecraftWiki;
        private LinkLabel llblLicense;
        private PictureBox pictureBox1;
        private Label lblInstanceId;
        private Label lblJTFVersion;
    }
}