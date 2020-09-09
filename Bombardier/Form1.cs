using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bombardier
{
    public partial class Form1 : Form
    {
        bool gameStart = false;
        int interval = 0;
        GameFildBilder gameFild = new GameFildBilder();
        PictureBox[,] visualArray = new PictureBox[GameFildBilder.gameFildSize.X, GameFildBilder.gameFildSize.Y];

        public Form1()
        {
            InitializeComponent();
            FillVisualArray();
            gameFild.BuildGameFild();
            gameFild.VisualUpdate(ref visualArray);
        }
        public void GamePlay(object sender, EventArgs e)
        {
            if (gameFild.HitCheck())
            {
                timer1.Enabled = false;
                MessageBox.Show("You loose!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Restart();
            }
            else
            {
                gameFild.Move();
                gameFild.VisualUpdate(ref visualArray);
            }
        }
        #region // заповнення visualArray тобто привязуємо PictureBox на формі до елементів масиву visualArray
        private void FillVisualArray()
        {
            //
            visualArray[0, 0] = p00;
            visualArray[0, 1] = p01;
            visualArray[0, 2] = p02;
            visualArray[0, 3] = p03;
            visualArray[0, 4] = p04;
            visualArray[0, 5] = p05;
            visualArray[0, 6] = p06;
            visualArray[0, 7] = p07;
            visualArray[0, 8] = p08;
            visualArray[0, 9] = p09;
            visualArray[0, 10] = p010;
            visualArray[0, 11] = p011;
            //
            visualArray[1, 0] = p10;
            visualArray[1, 1] = p11;
            visualArray[1, 2] = p12;
            visualArray[1, 3] = p13;
            visualArray[1, 4] = p14;
            visualArray[1, 5] = p15;
            visualArray[1, 6] = p16;
            visualArray[1, 7] = p17;
            visualArray[1, 8] = p18;
            visualArray[1, 9] = p19;
            visualArray[1, 10] = p110;
            visualArray[1, 11] = p111;
            //
            visualArray[2, 0] = p20;
            visualArray[2, 1] = p21;
            visualArray[2, 2] = p22;
            visualArray[2, 3] = p23;
            visualArray[2, 4] = p24;
            visualArray[2, 5] = p25;
            visualArray[2, 6] = p26;
            visualArray[2, 7] = p27;
            visualArray[2, 8] = p28;
            visualArray[2, 9] = p29;
            visualArray[2, 10] = p210;
            visualArray[2, 11] = p211;
            //
            visualArray[3, 0] = p30;
            visualArray[3, 1] = p31;
            visualArray[3, 2] = p32;
            visualArray[3, 3] = p33;
            visualArray[3, 4] = p34;
            visualArray[3, 5] = p35;
            visualArray[3, 6] = p36;
            visualArray[3, 7] = p37;
            visualArray[3, 8] = p38;
            visualArray[3, 9] = p39;
            visualArray[3, 10] = p310;
            visualArray[3, 11] = p311;
            //
            visualArray[4, 0] = p40;
            visualArray[4, 1] = p41;
            visualArray[4, 2] = p42;
            visualArray[4, 3] = p43;
            visualArray[4, 4] = p44;
            visualArray[4, 5] = p45;
            visualArray[4, 6] = p46;
            visualArray[4, 7] = p47;
            visualArray[4, 8] = p48;
            visualArray[4, 9] = p49;
            visualArray[4, 10] = p410;
            visualArray[4, 11] = p411;
            //
            visualArray[5, 0] = p50;
            visualArray[5, 1] = p51;
            visualArray[5, 2] = p52;
            visualArray[5, 3] = p53;
            visualArray[5, 4] = p54;
            visualArray[5, 5] = p55;
            visualArray[5, 6] = p56;
            visualArray[5, 7] = p57;
            visualArray[5, 8] = p58;
            visualArray[5, 9] = p59;
            visualArray[5, 10] = p510;
            visualArray[5, 11] = p511;
            //
            visualArray[6, 0] = p60;
            visualArray[6, 1] = p61;
            visualArray[6, 2] = p62;
            visualArray[6, 3] = p63;
            visualArray[6, 4] = p64;
            visualArray[6, 5] = p65;
            visualArray[6, 6] = p66;
            visualArray[6, 7] = p67;
            visualArray[6, 8] = p68;
            visualArray[6, 9] = p69;
            visualArray[6, 10] = p610;
            visualArray[6, 11] = p611;
        }
        #endregion
        private void button1_Click(object sender, EventArgs e)
        {

            if (radioButton1.Checked == true) { interval = 1000; }
            else if (radioButton2.Checked == true) { interval = 500; }
            else if (radioButton3.Checked == true) { interval = 250; }
            else { MessageBox.Show("Please select game mode!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            timer1.Interval = interval;
            timer1.Enabled = true;

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Task newTask = new Task(gameFild.CreateBomb);

            if (timer1.Enabled == true)
            {
                if (e.KeyCode == Keys.Space)
                {
                    newTask.Start();
                    newTask.Wait();
                }

            }
            else
            {
                MessageBox.Show("Press \"start\" button", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void Restart()
        {
            timer1.Enabled = false;
            interval = 0;
            gameFild.BuildGameFild();
            gameFild.ReStartCoordinate();
            gameFild.VisualUpdate(ref visualArray);

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            new Thread(() => { Invoke((MethodInvoker)delegate { Restart(); }); }).Start();
        }
    }
}
