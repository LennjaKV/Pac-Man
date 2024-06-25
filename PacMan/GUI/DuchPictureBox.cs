using PacMan.Hra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.GUI
{
    internal class DuchPictureBox : PostavaPictureBox
    {
        private Bitmap otocenNahoruNove;
        private Bitmap otocenDolevaNove;
        private Bitmap otocenDoluNove;
        private Bitmap otocenDopravaNove;

        private Bitmap otocenNahoruPuvodni;
        private Bitmap otocenDolevaPuvodni;
        private Bitmap otocenDoluPuvodni;
        private Bitmap otocenDopravaPuvodni;

        public DuchPictureBox(string name, int velikostPolicka, int zvetseni) : base(name, velikostPolicka, zvetseni)
        {
            otocenNahoruPuvodni = new Bitmap(otocenNahoru);
            otocenDolevaPuvodni = new Bitmap(otocenDoleva);
            otocenDoluPuvodni = new Bitmap(otocenDolu);
            otocenDopravaPuvodni = new Bitmap(otocenDoprava);

            otocenNahoruNove = PripravObrazek(otocenNahoru);
            otocenDolevaNove = PripravObrazek(otocenDoleva);
            otocenDoluNove = PripravObrazek(otocenDolu);
            otocenDopravaNove = PripravObrazek(otocenDoprava);
        }

        public void Nastavit(IDuch duch)
        {
            if (duch.ModPohybu == ModPohybu.Panika)
            {
                NastavNove();
            }
            else
            {
                NastavPuvodni();
            }

            base.Nastavit(duch);
        }

        private void NastavPuvodni()
        {
            otocenNahoru = otocenNahoruPuvodni;
            otocenDoleva = otocenDolevaPuvodni;
            otocenDoprava = otocenDopravaPuvodni;
            otocenDolu = otocenDoluPuvodni;
        }

        private void NastavNove()
        {
            otocenNahoru = otocenNahoruNove;
            otocenDoleva = otocenDolevaNove;
            otocenDoprava = otocenDopravaNove;
            otocenDolu = otocenDoluNove;
        }

        private Bitmap PripravObrazek(Bitmap zdroj)
        {
            Bitmap bmp = new Bitmap(otocenNahoru);
            using (Graphics g = Graphics.FromImage(bmp))
            using (SolidBrush stetec = new SolidBrush(Color.FromArgb(150, 0, 0, 255)))
            {
                g.FillRectangle(stetec, new Rectangle(0, 0, bmp.Width, bmp.Height));
            }
            return bmp;
        }
    }
}
