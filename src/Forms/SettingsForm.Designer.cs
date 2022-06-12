namespace MinecraftDatapackCreator.Forms
{
    partial class SettingsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.btnTextEditorFont = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnJtfEditorFont = new System.Windows.Forms.Button();
            this.cbConditionsCount = new System.Windows.Forms.CheckBox();
            this.cbJsonFormatting = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(16, 16, 8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Text editor font";
            // 
            // btnTextEditorFont
            // 
            this.btnTextEditorFont.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTextEditorFont.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnTextEditorFont.Location = new System.Drawing.Point(123, 21);
            this.btnTextEditorFont.Name = "btnTextEditorFont";
            this.btnTextEditorFont.Size = new System.Drawing.Size(310, 23);
            this.btnTextEditorFont.TabIndex = 0;
            this.btnTextEditorFont.UseVisualStyleBackColor = true;
            this.btnTextEditorFont.Click += new System.EventHandler(this.BtnTextEditorFont_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 56);
            this.label2.Margin = new System.Windows.Forms.Padding(16, 8, 3, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Json editor font";
            // 
            // btnJtfEditorFont
            // 
            this.btnJtfEditorFont.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJtfEditorFont.Location = new System.Drawing.Point(123, 52);
            this.btnJtfEditorFont.Name = "btnJtfEditorFont";
            this.btnJtfEditorFont.Size = new System.Drawing.Size(310, 23);
            this.btnJtfEditorFont.TabIndex = 1;
            this.btnJtfEditorFont.UseVisualStyleBackColor = true;
            this.btnJtfEditorFont.Click += new System.EventHandler(this.BtnJtfEditorFont_Click);
            // 
            // cbConditionsCount
            // 
            this.cbConditionsCount.AutoSize = true;
            this.cbConditionsCount.Location = new System.Drawing.Point(25, 87);
            this.cbConditionsCount.Margin = new System.Windows.Forms.Padding(16, 8, 3, 8);
            this.cbConditionsCount.Name = "cbConditionsCount";
            this.cbConditionsCount.Size = new System.Drawing.Size(220, 19);
            this.cbConditionsCount.TabIndex = 2;
            this.cbConditionsCount.Text = "Show conditions count in json editor";
            this.cbConditionsCount.UseVisualStyleBackColor = true;
            // 
            // cbJsonFormatting
            // 
            this.cbJsonFormatting.AutoSize = true;
            this.cbJsonFormatting.Location = new System.Drawing.Point(25, 118);
            this.cbJsonFormatting.Margin = new System.Windows.Forms.Padding(16, 8, 3, 8);
            this.cbJsonFormatting.Name = "cbJsonFormatting";
            this.cbJsonFormatting.Size = new System.Drawing.Size(214, 19);
            this.cbJsonFormatting.TabIndex = 3;
            this.cbJsonFormatting.Text = "Remove whitespaces form json files";
            this.cbJsonFormatting.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(277, 384);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "OK";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(358, 384);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(445, 419);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cbJsonFormatting);
            this.Controls.Add(this.cbConditionsCount);
            this.Controls.Add(this.btnJtfEditorFont);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnTextEditorFont);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Minecraft Datapack Creator - Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button btnJtfEditorFont;
        private Label label2;
        private Button btnTextEditorFont;
        private Label label1;
        private FontDialog fontDialog;
        private CheckBox cbConditionsCount;
        private CheckBox cbJsonFormatting;
        private Button btnSave;
        private Button btnCancel;
    }
}