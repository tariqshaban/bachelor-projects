namespace Game
{
    partial class Loading
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Loading));
            this.panel1 = new System.Windows.Forms.Panel();
            this.picboxPB = new System.Windows.Forms.PictureBox();
            this.Players = new System.Windows.Forms.TableLayoutPanel();
            this.labelEx21 = new LabelEx.LabelEx();
            this.labelEx28 = new LabelEx.LabelEx();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picboxPB)).BeginInit();
            this.Players.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.picboxPB);
            this.panel1.Controls.Add(this.Players);
            this.panel1.Location = new System.Drawing.Point(27, 66);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1136, 490);
            this.panel1.TabIndex = 0;
            // 
            // picboxPB
            // 
            this.picboxPB.Location = new System.Drawing.Point(3, 437);
            this.picboxPB.Name = "picboxPB";
            this.picboxPB.Size = new System.Drawing.Size(1130, 50);
            this.picboxPB.TabIndex = 0;
            this.picboxPB.TabStop = false;
            // 
            // Players
            // 
            this.Players.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Players.BackColor = System.Drawing.Color.Transparent;
            this.Players.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.Players.ColumnCount = 2;
            this.Players.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.62963F));
            this.Players.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.Players.Controls.Add(this.labelEx21, 1, 0);
            this.Players.Controls.Add(this.labelEx28, 0, 0);
            this.Players.Location = new System.Drawing.Point(0, 20);
            this.Players.Margin = new System.Windows.Forms.Padding(0);
            this.Players.Name = "Players";
            this.Players.RowCount = 1;
            this.Players.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Players.Size = new System.Drawing.Size(460, 365);
            this.Players.TabIndex = 0;
            // 
            // labelEx21
            // 
            this.labelEx21.BackColor = System.Drawing.Color.DimGray;
            this.labelEx21.BorderStyle = LabelEx.LabelEx.BorderType.Squared;
            this.labelEx21.Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold);
            this.labelEx21.ForeColor = System.Drawing.Color.Black;
            this.labelEx21.Image = null;
            this.labelEx21.Location = new System.Drawing.Point(337, 4);
            this.labelEx21.Name = "labelEx21";
            this.labelEx21.OutlineColor = System.Drawing.Color.White;
            this.labelEx21.ShadowDepth = 1;
            this.labelEx21.ShowTextShadow = true;
            this.labelEx21.Size = new System.Drawing.Size(119, 66);
            this.labelEx21.TabIndex = 0;
            this.labelEx21.Text = "Faction";
            this.labelEx21.TextPatternImage = null;
            // 
            // labelEx28
            // 
            this.labelEx28.BackColor = System.Drawing.Color.DimGray;
            this.labelEx28.BorderStyle = LabelEx.LabelEx.BorderType.Squared;
            this.labelEx28.Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold);
            this.labelEx28.ForeColor = System.Drawing.Color.Black;
            this.labelEx28.Image = null;
            this.labelEx28.Location = new System.Drawing.Point(4, 4);
            this.labelEx28.Name = "labelEx28";
            this.labelEx28.OutlineColor = System.Drawing.Color.White;
            this.labelEx28.ShadowDepth = 1;
            this.labelEx28.ShowTextShadow = true;
            this.labelEx28.Size = new System.Drawing.Size(326, 66);
            this.labelEx28.TabIndex = 0;
            this.labelEx28.Text = "Player";
            this.labelEx28.TextPatternImage = null;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // Loading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1378, 780);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Loading";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Loading";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Loading_Load);
            this.SizeChanged += new System.EventHandler(this.Restore);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Min);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picboxPB)).EndInit();
            this.Players.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel Players;
        private LabelEx.LabelEx labelEx21;
        private LabelEx.LabelEx labelEx28;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox picboxPB;
    }
}