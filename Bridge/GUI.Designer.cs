namespace Bridge
{
  partial class GUI
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
      this.exitBtn = new System.Windows.Forms.Button();
      this.saveBtn = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.txtAccessToken = new System.Windows.Forms.TextBox();
      this.errorLabel = new System.Windows.Forms.Label();
      this.closeBtn = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // exitBtn
      // 
      this.exitBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(192)))), ((int)(((byte)(181)))));
      this.exitBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.exitBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.exitBtn.Location = new System.Drawing.Point(158, 398);
      this.exitBtn.Name = "exitBtn";
      this.exitBtn.Size = new System.Drawing.Size(96, 29);
      this.exitBtn.TabIndex = 1;
      this.exitBtn.Text = "Exit Vipora";
      this.exitBtn.UseVisualStyleBackColor = false;
      this.exitBtn.Click += new System.EventHandler(this.ExitButton_Click);
      // 
      // saveBtn
      // 
      this.saveBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(192)))), ((int)(((byte)(181)))));
      this.saveBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.saveBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.saveBtn.Location = new System.Drawing.Point(288, 398);
      this.saveBtn.Name = "saveBtn";
      this.saveBtn.Size = new System.Drawing.Size(96, 29);
      this.saveBtn.TabIndex = 2;
      this.saveBtn.Text = "Save";
      this.saveBtn.UseVisualStyleBackColor = false;
      this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(20, 83);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(101, 17);
      this.label1.TabIndex = 3;
      this.label1.Text = "Access Token:";
      // 
      // txtAccessToken
      // 
      this.txtAccessToken.Location = new System.Drawing.Point(168, 83);
      this.txtAccessToken.Name = "txtAccessToken";
      this.txtAccessToken.Size = new System.Drawing.Size(216, 22);
      this.txtAccessToken.TabIndex = 4;
      // 
      // errorLabel
      // 
      this.errorLabel.Location = new System.Drawing.Point(23, 288);
      this.errorLabel.Name = "errorLabel";
      this.errorLabel.Size = new System.Drawing.Size(361, 78);
      this.errorLabel.TabIndex = 5;
      this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // closeBtn
      // 
      this.closeBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(192)))), ((int)(((byte)(181)))));
      this.closeBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.closeBtn.Location = new System.Drawing.Point(23, 398);
      this.closeBtn.Name = "closeBtn";
      this.closeBtn.Size = new System.Drawing.Size(96, 29);
      this.closeBtn.TabIndex = 6;
      this.closeBtn.Text = "Close";
      this.closeBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.closeBtn.UseVisualStyleBackColor = false;
      this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
      // 
      // GUI
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(240)))), ((int)(((byte)(237)))));
      this.ClientSize = new System.Drawing.Size(430, 450);
      this.Controls.Add(this.closeBtn);
      this.Controls.Add(this.errorLabel);
      this.Controls.Add(this.txtAccessToken);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.saveBtn);
      this.Controls.Add(this.exitBtn);
      this.ForeColor = System.Drawing.SystemColors.Window;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "GUI";
      this.ShowInTaskbar = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.Text = "Vipora";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.Button exitBtn;
    private System.Windows.Forms.Button saveBtn;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txtAccessToken;
    private System.Windows.Forms.Label errorLabel;
    private System.Windows.Forms.Button closeBtn;
  }
}