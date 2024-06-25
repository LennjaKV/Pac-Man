using PacMan.Hra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.GUI
{
    internal class PostavaPictureBox : PictureBox
    {
        private Smer smer;
        public Smer SmerPohybu
        { 
            get { return smer; }
            set
            {
                if (value != smer || this.Image == null)
                {
                    smer = value;

                    switch (smer)
                    {
                        case Smer.Nahoru:
                            this.Image = otocenNahoru;
                            break;
                        case Smer.Vlevo:
                            this.Image = otocenDoleva;
                            break;
                        case Smer.Dolu:
                            this.Image = otocenDolu;
                            break;
                        case Smer.Vpravo:
                            this.Image = otocenDoprava;
                            break;
                    }
                }
            }
        }

        private int zvetseni;

        public void Nastavit(IPohyblivyObjekt objekt)
        {
            Location = new Point(objekt.Pozice.X * zvetseni, objekt.Pozice.Y * zvetseni);
            SmerPohybu = objekt.Smer;
        }

        protected Bitmap otocenNahoru;
        protected Bitmap otocenDoleva;
        protected Bitmap otocenDolu;
        protected Bitmap otocenDoprava;

        public PostavaPictureBox(string name, int velikostPolicka, int zvetseni)
        {
            Name = name;
            Size = new Size(velikostPolicka, velikostPolicka);
            SizeMode = PictureBoxSizeMode.Zoom;
            BackColor = Color.Transparent;
            this.zvetseni = zvetseni;

            Assembly assm = Assembly.GetExecutingAssembly();
            ResourceManager manager = new ResourceManager("PacMan.GUI.GUIZdroje", assm);

            object horni = manager.GetObject($"{name}_up");
            object levy = manager.GetObject($"{name}_left");
            object dolni = manager.GetObject($"{name}_down");
            object pravy = manager.GetObject($"{name}_right");

            otocenNahoru = (Bitmap) horni;
            otocenDoleva = (Bitmap) levy;
            otocenDolu = (Bitmap) dolni;
            otocenDoprava = (Bitmap) pravy;
        }
    }
}
