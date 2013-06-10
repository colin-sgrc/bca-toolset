namespace SGRC.BCATools
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnImportYear = new System.Windows.Forms.ToolStripButton();
            this.btnImportMonth = new System.Windows.Forms.ToolStripButton();
            this.btnAbout = new System.Windows.Forms.ToolStripButton();
            this.txtSourceDatFile = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnChooseSourceDatFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnChooseDestinationDBFile = new System.Windows.Forms.Button();
            this.txtDestinationDBFile = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnImportYear,
            this.btnImportMonth,
            this.btnAbout});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(706, 55);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnImportYear
            // 
            this.btnImportYear.Image = ((System.Drawing.Image)(resources.GetObject("btnImportYear.Image")));
            this.btnImportYear.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnImportYear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnImportYear.Name = "btnImportYear";
            this.btnImportYear.Size = new System.Drawing.Size(223, 52);
            this.btnImportYear.Text = "Import Annual Data Advice File";
            this.btnImportYear.Click += new System.EventHandler(this.btnImportYear_Click);
            // 
            // btnImportMonth
            // 
            this.btnImportMonth.Image = ((System.Drawing.Image)(resources.GetObject("btnImportMonth.Image")));
            this.btnImportMonth.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnImportMonth.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnImportMonth.Name = "btnImportMonth";
            this.btnImportMonth.Size = new System.Drawing.Size(273, 52);
            this.btnImportMonth.Text = "Import Weekly/Monthly Data Advice File";
            this.btnImportMonth.Click += new System.EventHandler(this.btnImportMonth_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Image = ((System.Drawing.Image)(resources.GetObject("btnAbout.Image")));
            this.btnAbout.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAbout.Margin = new System.Windows.Forms.Padding(105, 1, 0, 2);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(101, 52);
            this.btnAbout.Text = "About...";
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // txtSourceDatFile
            // 
            this.txtSourceDatFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSourceDatFile.Location = new System.Drawing.Point(225, 74);
            this.txtSourceDatFile.Name = "txtSourceDatFile";
            this.txtSourceDatFile.Size = new System.Drawing.Size(438, 22);
            this.txtSourceDatFile.TabIndex = 1;
            this.txtSourceDatFile.Text = "<not set>";
            this.txtSourceDatFile.TextChanged += new System.EventHandler(this.txtSourceDatFile_TextChanged);
            // 
            // btnChooseSourceDatFile
            // 
            this.btnChooseSourceDatFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChooseSourceDatFile.Location = new System.Drawing.Point(669, 72);
            this.btnChooseSourceDatFile.Name = "btnChooseSourceDatFile";
            this.btnChooseSourceDatFile.Size = new System.Drawing.Size(22, 23);
            this.btnChooseSourceDatFile.TabIndex = 2;
            this.btnChooseSourceDatFile.Text = "...";
            this.btnChooseSourceDatFile.UseVisualStyleBackColor = true;
            this.btnChooseSourceDatFile.Click += new System.EventHandler(this.btnChooseSourceDatPath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(117, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Source DAT file";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(212, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Destination MS Access Database ";
            // 
            // btnChooseDestinationDBFile
            // 
            this.btnChooseDestinationDBFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChooseDestinationDBFile.Location = new System.Drawing.Point(669, 104);
            this.btnChooseDestinationDBFile.Name = "btnChooseDestinationDBFile";
            this.btnChooseDestinationDBFile.Size = new System.Drawing.Size(22, 23);
            this.btnChooseDestinationDBFile.TabIndex = 6;
            this.btnChooseDestinationDBFile.Text = "...";
            this.btnChooseDestinationDBFile.UseVisualStyleBackColor = true;
            this.btnChooseDestinationDBFile.Click += new System.EventHandler(this.btnChooseDestinationDBFile_Click);
            // 
            // txtDestinationDBFile
            // 
            this.txtDestinationDBFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDestinationDBFile.Location = new System.Drawing.Point(225, 106);
            this.txtDestinationDBFile.Name = "txtDestinationDBFile";
            this.txtDestinationDBFile.Size = new System.Drawing.Size(438, 22);
            this.txtDestinationDBFile.TabIndex = 2;
            this.txtDestinationDBFile.Text = "<not set>";
            this.txtDestinationDBFile.TextChanged += new System.EventHandler(this.txtDestinationDBFile_TextChanged);
            // 
            // txtLog
            // 
            this.txtLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.Location = new System.Drawing.Point(11, 149);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(680, 22);
            this.txtLog.TabIndex = 0;
            this.txtLog.TabStop = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 181);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSourceDatFile);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btnChooseDestinationDBFile);
            this.Controls.Add(this.txtDestinationDBFile);
            this.Controls.Add(this.btnChooseSourceDatFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BCA Data Advice Toolset";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnImportYear;
        private System.Windows.Forms.TextBox txtSourceDatFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnChooseSourceDatFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnChooseDestinationDBFile;
        private System.Windows.Forms.TextBox txtDestinationDBFile;
        private System.Windows.Forms.ToolStripButton btnImportMonth;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.ToolStripButton btnAbout;
    }
}