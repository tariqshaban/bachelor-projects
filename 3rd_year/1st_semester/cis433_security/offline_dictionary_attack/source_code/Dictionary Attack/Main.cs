using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Dictionary_Attack
{
    public partial class Main : Form
    {
        List<string> Passwords_Hashed = new List<string>();
        List<string> Passwords = new List<string>();
        int Pass_Rows = 0;
        string path;
        public Main()
        {
            InitializeComponent();
        }

        public string CalculateMD5Hash(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        public static bool IsMD5(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return false;
            }

            return Regex.IsMatch(input, "^[0-9a-fA-F]{32}$", options: RegexOptions.Compiled);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Visible = false;
        }
     
        private void Button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "File Browser",
                Filter = "txt files (*.txt)|*.txt"
            })
            {
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Passwords_Hashed = File.ReadAllLines(ofd.FileName).ToList();

                    List.AutoScroll = false;
                    List.Controls.Clear();
                    List.RowStyles.Clear();
                    List.RowCount = 1;
                    List.RowStyles.Add(new RowStyle(SizeType.AutoSize, 20F));
                    List.Controls.Add(new LabelEx.LabelEx() { Text = "Hashed Passsword", ShadowDepth = 1, ShowTextShadow = true, Size = new Size(477, 52), BackColor = Color.DimGray, OutlineColor = Color.LightSkyBlue, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
                    List.Controls.Add(new LabelEx.LabelEx() { Text = "Password", ShadowDepth = 1, ShowTextShadow = true, Size = new Size(317, 52), BackColor = Color.DimGray, OutlineColor = Color.LightSkyBlue, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
                    List.AutoScroll = true;
                    List.HorizontalScroll.Visible = false;
                    List.SuspendLayout();
                    int counter = 0;
                    foreach (string line in Passwords_Hashed)
                    {
                        if (line.Length != 0)
                        {
                            if (IsMD5(line.ToUpper()))
                            {
                                List.RowCount++;
                                List.Controls.Add(new LabelEx.LabelEx() { Text = line.ToUpper(), ShadowDepth = 1, ShowTextShadow = true, Size = new Size(477, 52), BackColor = Color.Transparent, OutlineColor = Color.White, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
                                List.Controls.Add(new LabelEx.LabelEx() { Text = "", ShadowDepth = 1, ShowTextShadow = true, Size = new Size(317, 52), BackColor = Color.Transparent, OutlineColor = Color.White, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
                                counter++;
                            }
                            else
                            {
                                MessageBox.Show("The Password File Was Not In The Form Of MD5","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);  
                                button4.PerformClick();
                                break;
                            }
                        }
                    }
                    List.ResumeLayout();
                    Pass_Rows = counter;
                }
            }
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "File Browser",
                Filter = "txt files (*.txt)|*.txt"
            })
            {
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    label1.Visible = true;
                    path = ofd.FileName;
                }
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (Passwords_Hashed.Count == 0 || path.Length == 0)
                MessageBox.Show("Files Are Either Not Chosen or Empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                Passwords = new List<string>(Pass_Rows);
                for (int i = 0; i < Passwords.Capacity; i++)
                    Passwords.Add("Not Found!");
                int counter = 0;

                if (Passwords.Capacity >= 4)
                {
                    Thread T1 = new Thread(() => Threader(0, Convert.ToInt32(Math.Ceiling(Passwords.Capacity / 4.0))));
                    Thread T2 = new Thread(() => Threader(Convert.ToInt32(Math.Ceiling(Passwords.Capacity / 4.0)), Convert.ToInt32(Math.Ceiling(Passwords.Capacity / 2.0))));
                    Thread T3 = new Thread(() => Threader(Convert.ToInt32(Math.Ceiling(Passwords.Capacity / 2.0)), Convert.ToInt32(Math.Ceiling(Passwords.Capacity / 1.5))));
                    Thread T4 = new Thread(() => Threader(Convert.ToInt32(Math.Ceiling(Passwords.Capacity / 1.5)), Passwords.Capacity));
                    T1.Start();
                    T2.Start();
                    T3.Start();
                    T4.Start();
                    T1.Join();
                    T2.Join();
                    T3.Join();
                    T4.Join();
                }
                else
                {
                    Thread T1 = new Thread(() => Threader(0, Passwords.Capacity));
                    T1.Start();
                    T1.Join();
                }

                List.AutoScroll = false;
                List.Controls.Clear();
                List.RowStyles.Clear();
                List.RowCount = 1;
                List.RowStyles.Add(new RowStyle(SizeType.AutoSize, 20F));
                List.Controls.Add(new LabelEx.LabelEx() { Text = "Hashed Passsword", ShadowDepth = 1, ShowTextShadow = true, Size = new Size(477, 52), BackColor = Color.DimGray, OutlineColor = Color.LightSkyBlue, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
                List.Controls.Add(new LabelEx.LabelEx() { Text = "Password", ShadowDepth = 1, ShowTextShadow = true, Size = new Size(317, 52), BackColor = Color.DimGray, OutlineColor = Color.LightSkyBlue, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
                List.AutoScroll = true;
                List.HorizontalScroll.Visible = false;
                List.SuspendLayout();
                foreach (string line in Passwords_Hashed)
                {
                    if (line.Length != 0)
                    {
                        List.RowCount++;
                        List.Controls.Add(new LabelEx.LabelEx() { Text = line.ToUpper(), ShadowDepth = 1, ShowTextShadow = true, Size = new Size(477, 52), BackColor = Color.Transparent, OutlineColor = Color.White, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
                        if(Passwords[counter]=="Not Found!")
                        List.Controls.Add(new LabelEx.LabelEx() { Text = Passwords[counter], ShadowDepth = 1, ShowTextShadow = true, Size = new Size(317, 52), BackColor = Color.Transparent, OutlineColor = Color.White, BorderColor = Color.Black, ForeColor = Color.Red, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
                        else
                        List.Controls.Add(new LabelEx.LabelEx() { Text = Passwords[counter], ShadowDepth = 1, ShowTextShadow = true, Size = new Size(317, 52), BackColor = Color.Transparent, OutlineColor = Color.White, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
                        counter++;
                    }
                }
                List.ResumeLayout();

            }
        }

        public void Threader(int start,int end)
        {
            int counter = start;
            List<string> Partial = Passwords_Hashed.GetRange(start,end-start);
            foreach (string line in Partial)
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = String.Empty;
                    while ((s = sr.ReadLine()) != null)
                    {
                        if (line.Length != 0)
                        {
                            if (CalculateMD5Hash(s).ToUpper().Equals(line.ToUpper()))
                            {
                                Passwords[counter]=s;
                                break;
                            }
                        }
                    }
                }
                counter++;
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            Passwords_Hashed.Clear();
            Passwords.Clear();
            path = "";
            List.AutoScroll = false;
            List.Controls.Clear();
            List.RowStyles.Clear();
            List.RowCount = 1;
            List.RowStyles.Add(new RowStyle(SizeType.AutoSize, 20F));
            List.Controls.Add(new LabelEx.LabelEx() { Text = "Hashed Passsword", ShadowDepth = 1, ShowTextShadow = true, Size = new Size(477, 52), BackColor = Color.DimGray, OutlineColor = Color.LightSkyBlue, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
            List.Controls.Add(new LabelEx.LabelEx() { Text = "Password", ShadowDepth = 1, ShowTextShadow = true, Size = new Size(317, 52), BackColor = Color.DimGray, OutlineColor = Color.LightSkyBlue, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
            List.AutoScroll = true;
            List.HorizontalScroll.Visible = false;
        }
    }
}