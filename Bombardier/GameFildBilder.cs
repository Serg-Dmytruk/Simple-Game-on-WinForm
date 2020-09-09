using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.VisualStyles;
using System.Threading;

namespace Bombardier
{
    class GameFildBilder
    {
        static private string path = Directory.GetCurrentDirectory() + @"\res\"; //шлях до картинок
        private Random rnd = new Random(DateTime.Now.Millisecond); //для рандому висоти будинків
        public static readonly Point gameFildSize = new Point(7, 12); // ширина та висота ігрового поля
        private Point planePosition = new Point(0, 0); //позиція літака
        private Point prewPlanePosition = new Point(0, 0); //попередня позиція літака
        private Point prewBombPosition = new Point(0, 0); //попердня позиція бомби літака
        private Point bombPosition = new Point(0, 0); //позиція бомби 
        private Point cleaningExp = new Point(0, 0);//позиція спрайта вибуху щоб підчистити
        public int[,] checkArray = new int[gameFildSize.X, gameFildSize.Y]; // масив для розуміння що де знаходиться на ігровому полі (0 - пустота, 1 - будинок)
        private Image housePart = Image.FromFile(path + @"wall.png"); //спрайт стіни
        private Image bomb = Image.FromFile(path + "bomb.png"); //спрайт бомби
        private Image explosion = Image.FromFile(path + "exp.png"); //спрайт вибуху
        private Image plane = Image.FromFile(path + "plane.png"); //спрайт літака
        bool bombExist = false; //чи існує бомба

        private enum state { empty = 0, housePart = 1, bomb = 3, explosion = 4, plane = 5 }; // стан клітинкі на ігровому полі
        public void BuildGameFild()//будує ігрове поле
        {
            List<int> houseHeight = new List<int>();

            for (int i = 0; i < gameFildSize.Y; i++)
            {
                houseHeight.Add(rnd.Next(1, gameFildSize.X - 2)); // Рандомимо висоту будинків
            }
            // заповнюємо ігрове поле пустотою
            for (int i = 0; i < gameFildSize.X; i++)
            {
                for (int j = 0; j < gameFildSize.Y; j++)
                {
                    checkArray[i, j] = (int)state.empty;
                }
            }
            //заповнюємо поле будинками відносно зарандомленої висоти у колекції houseHeight
            int tmpY = 0;
            foreach (int i in houseHeight)
            {
                int tmpX = gameFildSize.X - 1;
                for (int j = 0; j < i; j++)
                {
                    checkArray[tmpX, tmpY] = (int)state.housePart;
                    tmpX--;
                }
                tmpY++;
            }
            checkArray[0, 0] = 5; //ставимо літак на початок
        }
        public void VisualUpdate(ref PictureBox[,] VisualArray) // в залежності від того що записано у checkArray вимальовується ігрова мапа
        {
            for (int i = 0; i < gameFildSize.X; i++)
            {
                for (int j = 0; j < gameFildSize.Y; j++)
                {
                    if (checkArray[i, j] == (int)state.housePart) { VisualArray[i, j].Image = housePart; }
                    else if (checkArray[i, j] == (int)state.bomb) { VisualArray[i, j].Image = bomb; }
                    else if (checkArray[i, j] == (int)state.explosion) { VisualArray[i, j].Image = explosion; }
                    else if (checkArray[i, j] == (int)state.plane) { VisualArray[i, j].Image = plane; }
                    else { VisualArray[i, j].Image = null; }
                }
            }
        }
        public void Move()
        {
            //if (HitCheck())
            //{
            //    //MessageBox.Show("You loose!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
            //else
            //{
                //рух літака
                prewPlanePosition = planePosition;//записуємо попередню позицію літака
                if (planePosition.X == gameFildSize.X - 1) { planePosition.X = 0; } // якщо літак вийде за межі скинути до початку
                planePosition.Y++; // рух літака
                if (planePosition.Y == gameFildSize.Y) { planePosition.Y = 0; planePosition.X++; } //перехід літака на нижню лінію
                checkArray[planePosition.X, planePosition.Y] = (int)state.plane; //запис на ігрву мапу де зараз літак
                checkArray[prewPlanePosition.X, prewPlanePosition.Y] = (int)state.empty; //підтераємо літак з попередньої позиції
            //}
            //рух бомби
            checkArray[cleaningExp.X, cleaningExp.Y] = (int)state.empty; // підчищаємо спрайт вибуху
            if (bombExist == true)
            {
                bool exp = false;// коли робити вибух
                if (bombPosition.X + 1 <= gameFildSize.X - 1) // перевірка чи не виходимо за межі масиву
                {
                    if (checkArray[bombPosition.X + 1, bombPosition.Y] == (int)state.housePart) { exp = true; }
                    bombPosition.X++;
                }
                prewBombPosition = new Point(bombPosition.X - 1, bombPosition.Y);

                if (exp) //робимо вибух
                {
                    checkArray[prewBombPosition.X, prewBombPosition.Y] = (int)state.empty;
                    checkArray[bombPosition.X, bombPosition.Y] = (int)state.explosion;
                    cleaningExp = new Point(bombPosition.X, bombPosition.Y);
                    bombExist = false;//можна знову кидати бомбу
                }
                else //бомба рухається далі
                {
                    checkArray[bombPosition.X, bombPosition.Y] = (int)state.bomb;
                    checkArray[prewBombPosition.X, prewBombPosition.Y] = (int)state.empty;
                }
            }

        }
        public void CreateBomb()
        {
            if (!bombExist)
            {
                bombPosition = new Point(planePosition.X, planePosition.Y);
                if (bombPosition.X >= gameFildSize.X) { bombPosition.X = gameFildSize.X - 1; }
                checkArray[bombPosition.X, bombPosition.Y] = (int)state.bomb;
                bombExist = true;
            }
        }
        public void ReStartCoordinate()
        {
             planePosition = new Point(0, 0); //позиція літака
             prewPlanePosition = new Point(0, 0); //попередня позиція літака
             prewBombPosition = new Point(0, 0); //попердня позиція бомби літака
             bombPosition = new Point(0, 0); //позиція бомби 
             cleaningExp = new Point(0, 0);//позиція спрайта вибуху щоб підчистити
        }

        public bool HitCheck() //перевірка зіткнення
        {
            Point check = planePosition;
            if (planePosition.Y >= gameFildSize.Y - 1)
            {
                check.X++; check.Y = 0;
                if (checkArray[check.X, check.Y] == (int)state.housePart) { return true; }
            }
            else
            {
                if(checkArray[check.X, check.Y + 1] == (int)state.housePart) { return true; }
            }
            return false;
        }
    }
}
