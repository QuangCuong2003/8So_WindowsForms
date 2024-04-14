using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _8So_WindowsForms
{
    public partial class fm8So : Form
    {
        int[,] matrix;
        Solution8Numbers solution8Numbers;
        Stack<int[,]> s;
        Button[,] btns;
        int n = 3;
        int countMove = 0;
        public fm8So()
        {
            InitializeComponent();
            matrix = new int[n, n];
            solution8Numbers = new Solution8Numbers();

            s = new Stack<int[,]>();
            btns = new Button[n, n];

            btns[0, 0] = btn1;
            btns[0, 1] = btn2;
            btns[0, 2] = btn3;
            btns[1, 0] = btn8;
            btns[1, 1] = btn0;
            btns[1, 2] = btn4;
            btns[2, 0] = btn7;
            btns[2, 1] = btn6;
            btns[2, 2] = btn5;

            btnStart.Enabled = false;
            btnStop.Enabled = false;
        }

        void loadNumbers(int[,] a, Button[,] b)
        {
            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(0); j++)
                {
                    if (a[i, j] == 0)
                    {
                        b[i, j].Text = "";
                        b[i, j].BackColor = Color.MediumSeaGreen;
                    }
                    else
                    {
                        b[i, j].Text = a[i, j].ToString();
                        b[i, j].BackColor = Color.White;
                    }
                }
        }
        void randomNumber()
        {
            matrix = solution8Numbers.randomMatrix(n);

            loadNumbers(matrix, btns);

            s = solution8Numbers.findResult(matrix);
            s.Pop();
            cbbSpeech.Text = cbbSpeech.Items[0].ToString();
            lblCountMove.Text = "0";
            countMove = 0;
            btnStart.Enabled = false;
            btnStop.Enabled = false;
            timerSpeech.Enabled = false;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            randomNumber();
            btnStart.Enabled = true;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            timerSpeech.Enabled = true;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
        }

        private void timerSpeech_Tick(object sender, EventArgs e)
        {
            switch (cbbSpeech.Text)
            {
                case "1": timerSpeech.Interval = 2000; break;
                case "2": timerSpeech.Interval = 1500; break;
                case "3": timerSpeech.Interval = 1000; break;
                case "4": timerSpeech.Interval = 500; break;
                case "5": timerSpeech.Interval = 250; break;
            }

            int[,] Temp;

            if (s.Count != 0)
            {
                Temp = s.Pop();
                loadNumbers(Temp, btns);

                countMove++;
                lblCountMove.Text = countMove.ToString();
            }
            else
                timerSpeech.Enabled = false;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timerSpeech.Enabled = false;
            btnStop.Enabled = false;
            btnStart.Enabled = true;
        }

        private void cbbSpeech_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
