namespace DevBootstrap.Client;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        this.clbRepos = new System.Windows.Forms.CheckedListBox();
        this.txtStatus = new System.Windows.Forms.TextBox();
        this.lblRepos = new System.Windows.Forms.Label();
        this.lblStatus = new System.Windows.Forms.Label();
        this.SuspendLayout();

        // lblRepos
        this.lblRepos.AutoSize = true;
        this.lblRepos.Location = new System.Drawing.Point(12, 9);
        this.lblRepos.Name = "lblRepos";
        this.lblRepos.Size = new System.Drawing.Size(78, 15);
        this.lblRepos.Text = "Repositories";

        // clbRepos
        this.clbRepos.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.clbRepos.CheckOnClick = true;
        this.clbRepos.FormattingEnabled = true;
        this.clbRepos.Location = new System.Drawing.Point(12, 27);
        this.clbRepos.Name = "clbRepos";
        this.clbRepos.Size = new System.Drawing.Size(560, 220);
        this.clbRepos.TabIndex = 0;

        // lblStatus
        this.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        this.lblStatus.AutoSize = true;
        this.lblStatus.Location = new System.Drawing.Point(12, 260);
        this.lblStatus.Name = "lblStatus";
        this.lblStatus.Size = new System.Drawing.Size(42, 15);
        this.lblStatus.Text = "Status";

        // txtStatus
        this.txtStatus.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.txtStatus.Location = new System.Drawing.Point(12, 278);
        this.txtStatus.Multiline = true;
        this.txtStatus.Name = "txtStatus";
        this.txtStatus.ReadOnly = true;
        this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.txtStatus.Size = new System.Drawing.Size(560, 150);
        this.txtStatus.TabIndex = 1;

        // MainForm
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(584, 441);
        this.Controls.Add(this.lblRepos);
        this.Controls.Add(this.clbRepos);
        this.Controls.Add(this.lblStatus);
        this.Controls.Add(this.txtStatus);
        this.Name = "MainForm";
        this.Text = "Dev.Bootstrap";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private System.Windows.Forms.CheckedListBox clbRepos;
    private System.Windows.Forms.TextBox txtStatus;
    private System.Windows.Forms.Label lblRepos;
    private System.Windows.Forms.Label lblStatus;
}
