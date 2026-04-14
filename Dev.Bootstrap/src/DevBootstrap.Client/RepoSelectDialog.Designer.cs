namespace DevBootstrap.Client;

partial class RepoSelectDialog
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
        this.txtFilter = new System.Windows.Forms.TextBox();
        this.lblFilter = new System.Windows.Forms.Label();
        this.clbAvailable = new System.Windows.Forms.CheckedListBox();
        this.btnOk = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.SuspendLayout();

        // lblFilter
        this.lblFilter.AutoSize = true;
        this.lblFilter.Location = new System.Drawing.Point(12, 12);
        this.lblFilter.Name = "lblFilter";
        this.lblFilter.Size = new System.Drawing.Size(36, 15);
        this.lblFilter.Text = "Filter";

        // txtFilter
        this.txtFilter.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.txtFilter.Location = new System.Drawing.Point(54, 9);
        this.txtFilter.Name = "txtFilter";
        this.txtFilter.Size = new System.Drawing.Size(518, 23);
        this.txtFilter.TabIndex = 0;
        this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);

        // clbAvailable
        this.clbAvailable.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.clbAvailable.CheckOnClick = true;
        this.clbAvailable.FormattingEnabled = true;
        this.clbAvailable.Location = new System.Drawing.Point(12, 40);
        this.clbAvailable.Name = "clbAvailable";
        this.clbAvailable.Size = new System.Drawing.Size(560, 364);
        this.clbAvailable.TabIndex = 1;

        // btnOk
        this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        this.btnOk.Location = new System.Drawing.Point(416, 415);
        this.btnOk.Name = "btnOk";
        this.btnOk.Size = new System.Drawing.Size(75, 27);
        this.btnOk.TabIndex = 2;
        this.btnOk.Text = "Clone";
        this.btnOk.Click += new System.EventHandler(this.btnOk_Click);

        // btnCancel
        this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        this.btnCancel.Location = new System.Drawing.Point(497, 415);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(75, 27);
        this.btnCancel.TabIndex = 3;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

        // RepoSelectDialog
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(584, 451);
        this.Controls.Add(this.lblFilter);
        this.Controls.Add(this.txtFilter);
        this.Controls.Add(this.clbAvailable);
        this.Controls.Add(this.btnOk);
        this.Controls.Add(this.btnCancel);
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "RepoSelectDialog";
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Select Repositories to Clone";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private System.Windows.Forms.TextBox txtFilter;
    private System.Windows.Forms.Label lblFilter;
    private System.Windows.Forms.CheckedListBox clbAvailable;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Button btnCancel;
}
