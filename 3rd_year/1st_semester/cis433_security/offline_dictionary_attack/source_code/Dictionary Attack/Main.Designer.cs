namespace Dictionary_Attack
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.List = new System.Windows.Forms.TableLayoutPanel();
            this.labelEx35 = new LabelEx.LabelEx();
            this.labelEx37 = new LabelEx.LabelEx();
            this.button3 = new System.Windows.Forms.Button();
            this.Label = new LabelEx.LabelEx();
            this.label1 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.List.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 326);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 23);
            this.button1.TabIndex = 31;
            this.button1.Text = "Browse Password File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(142, 326);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(124, 23);
            this.button2.TabIndex = 33;
            this.button2.Text = "Browse Dictionary List";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // List
            // 
            this.List.AutoScroll = true;
            this.List.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.List.ColumnCount = 2;
            this.List.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.List.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.List.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.List.Controls.Add(this.labelEx35, 1, 0);
            this.List.Controls.Add(this.labelEx37, 0, 0);
            this.List.Location = new System.Drawing.Point(-7, 56);
            this.List.Name = "List";
            this.List.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.List.RowCount = 1;
            this.List.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.List.Size = new System.Drawing.Size(829, 255);
            this.List.TabIndex = 34;
            // 
            // labelEx35
            // 
            this.labelEx35.BackColor = System.Drawing.Color.DimGray;
            this.labelEx35.BorderStyle = LabelEx.LabelEx.BorderType.Squared;
            this.labelEx35.Font = new System.Drawing.Font("Comic Sans MS", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEx35.ForeColor = System.Drawing.Color.Black;
            this.labelEx35.Image = null;
            this.labelEx35.Location = new System.Drawing.Point(508, 4);
            this.labelEx35.Name = "labelEx35";
            this.labelEx35.OutlineColor = System.Drawing.Color.LightSkyBlue;
            this.labelEx35.ShadowDepth = 1;
            this.labelEx35.ShowTextShadow = true;
            this.labelEx35.Size = new System.Drawing.Size(317, 52);
            this.labelEx35.TabIndex = 0;
            this.labelEx35.TabStop = false;
            this.labelEx35.Text = "Password";
            this.labelEx35.TextPatternImage = null;
            // 
            // labelEx37
            // 
            this.labelEx37.BackColor = System.Drawing.Color.DimGray;
            this.labelEx37.BorderStyle = LabelEx.LabelEx.BorderType.Squared;
            this.labelEx37.Font = new System.Drawing.Font("Comic Sans MS", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEx37.ForeColor = System.Drawing.Color.Black;
            this.labelEx37.Image = null;
            this.labelEx37.Location = new System.Drawing.Point(24, 4);
            this.labelEx37.Name = "labelEx37";
            this.labelEx37.OutlineColor = System.Drawing.Color.LightSkyBlue;
            this.labelEx37.ShadowDepth = 1;
            this.labelEx37.ShowTextShadow = true;
            this.labelEx37.Size = new System.Drawing.Size(477, 52);
            this.labelEx37.TabIndex = 0;
            this.labelEx37.TabStop = false;
            this.labelEx37.Text = "Hashed Passsword";
            this.labelEx37.TextPatternImage = null;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(698, 326);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(124, 23);
            this.button3.TabIndex = 35;
            this.button3.Text = "Start";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // Label
            // 
            this.Label.BorderStyle = LabelEx.LabelEx.BorderType.Rounded;
            this.Label.Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold);
            this.Label.ForeColor = System.Drawing.Color.Transparent;
            this.Label.ForeColorTransparency = 0;
            this.Label.Image = null;
            this.Label.Location = new System.Drawing.Point(12, 12);
            this.Label.Name = "Label";
            this.Label.OutlineColor = System.Drawing.Color.RoyalBlue;
            this.Label.OutlineThickness = 2;
            this.Label.ShadowColor = System.Drawing.Color.White;
            this.Label.ShadowDepth = 1;
            this.Label.Size = new System.Drawing.Size(810, 38);
            this.Label.TabIndex = 32;
            this.Label.Text = "Tariq Shaban ";
            this.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Label.TextPatternImage = null;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.LimeGreen;
            this.label1.Location = new System.Drawing.Point(272, 329);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 16);
            this.label1.TabIndex = 36;
            this.label1.Text = "Loaded";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(568, 326);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(124, 23);
            this.button4.TabIndex = 37;
            this.button4.Text = "Clear";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(834, 361);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.List);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.Label);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "Dictionary Attack";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.List.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private LabelEx.LabelEx Label;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TableLayoutPanel List;
        private LabelEx.LabelEx labelEx35;
        private LabelEx.LabelEx labelEx37;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button4;
    }
}

