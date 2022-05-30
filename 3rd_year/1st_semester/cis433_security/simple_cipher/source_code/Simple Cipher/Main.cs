/*************************************************************
I've chosen C# to add GUI and it has many similarities wih C++ 
You can either select a text file or manually enter the text
*************************************************************/
using System;
using System.IO;
using System.Windows.Forms;

namespace Simple_Cipher
{
    public partial class Simple_Cipher : Form
    {
        //Defining necessary variables
        bool clicked = false;
        readonly Char[] Alpha = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private LabelEx.LabelEx[] Swipe;
        public Simple_Cipher()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Defining an array of labels
            Swipe = new LabelEx.LabelEx[] { A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z };
        }

        private void Slider_Scroll(object sender, EventArgs e)
        {
            //Manages moving the letters in the slider
            for (int i = 0; i < 26; i++)
                Swipe[i].Text = Alpha[(i + Slider.Value) % 26] + "";
            Proccess();
        }

        //Listeners
        private void Input_Click(object sender, EventArgs e)
        {
            //To clear the textbox
            if (!clicked)
            {
                Input.Text = "";
                clicked = true;
            }
        }
        private void Input_TextChanged(object sender, EventArgs e)
        {
            Proccess();
        }
        private void Cipher_CheckedChanged(object sender, EventArgs e)
        {
            Proccess();
        }
        private void Symbols_CheckedChanged(object sender, EventArgs e)
        {
            Proccess();
        }
        private void Spaces_CheckedChanged(object sender, EventArgs e)
        {
            Proccess();
        }


        public void Proccess()
        {
            if (clicked)
            {
                Output.Clear();
                //Iterates through the input textbox and ciphers the text based on the key value
                for (int i = 0; i < Input.Text.Length; i++)
                    if (Char.IsLetter(Input.Text[i]))
                    {
                        if (Cipher.Checked)
                        {
                            if (Char.IsUpper(Input.Text[i]))
                                Output.AppendText((char)(((int)(Input.Text[i]) - 65 + Slider.Value) % 26 + 65) + "");
                            else
                                Output.AppendText(Char.ToLower((char)(((int)(Input.Text[i]) - 97 + Slider.Value) % 26 + 97)) + "");
                        }
                        else
                            if (Char.IsUpper(Input.Text[i]))
                            Output.AppendText((char)(((int)(Input.Text[i]) - 65 - Slider.Value + 26) % 26 + 65) + "");
                        else
                            Output.AppendText(Char.ToLower((char)(((int)(Input.Text[i]) - 97 - Slider.Value + 26) % 26 + 97)) + "");
                    }
                    else if (Input.Text[i].Equals('\n') && Spaces.Checked)
                        Output.AppendText("\r\n");
                    else if (Input.Text[i].Equals(' ') && Spaces.Checked)
                        Output.AppendText(" ");
                    else if (!Input.Text[i].Equals('\n') && Symbols.Checked && (Char.IsNumber(Input.Text[i]) || Char.IsSymbol(Input.Text[i]) || Char.IsPunctuation(Input.Text[i])))
                        Output.AppendText(Input.Text[i] + "");
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //Open a dialog that allows the user to navigate through and choose the text file
            using (OpenFileDialog ofd = new OpenFileDialog
            {
                //File type restricions
                Title = "File Browser",
                Filter = "txt files (*.txt)|*.txt"
            })
            {
                //If the operation was successful,the text file contents will be stored in a string
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    clicked = true;
                    string path = ofd.FileName;
                    Input.Text = File.ReadAllText(ofd.FileName);
                }
            }
        }
    }
}