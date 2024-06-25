using PacMan.Hra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace PacMan.GUI
{
    public class PozadiBludistePB : PictureBox
    {
        private static Color barvaZdi = Color.FromArgb(255, 73, 115, 151);
        private static Color barvaDveri = Color.Gray;

        private List<bool> zobrazeneTecky = new List<bool>();
        private List<bool> zobrazenePosilovace = new List<bool>();

        private List<PictureBox> tecky = new List<PictureBox>();
        private List<PictureBox> posilovace = new List<PictureBox>();
        private Image teckaImg;
        private Image posilovacImg;

        public PozadiBludistePB(IBludiste bludiste, int velikostPolicka)
        {
            NactiTecku(velikostPolicka);
            NactiPosilovac(velikostPolicka);

            this.Image = PripravBludiste(bludiste, velikostPolicka);
            this.Width = this.Image.Width;
            this.Height = this.Image.Height;
        }

        public void ZobrazitTecky(List<bool> teckyKZobrazeni)
        {
            for(int i = 0; i < tecky.Count; i++)
            {
                if (teckyKZobrazeni[i] != zobrazeneTecky[i])
                {
                    if (teckyKZobrazeni[i])
                    {
                        tecky[i].Show();
                    }
                    else
                    {
                        tecky[i].Hide();
                    }
                    zobrazeneTecky[i] = teckyKZobrazeni[i];
                }
            }
        }

        public void ZobrazitPosilovace(List<bool> posilovaceKZobrazeni)
        {
            for (int i = 0; i < posilovace.Count; i++)
            {
                if (posilovaceKZobrazeni[i] != zobrazenePosilovace[i])
                {
                    if (posilovaceKZobrazeni[i])
                    {
                        posilovace[i].Show();
                    }
                    else
                    {
                        posilovace[i].Hide();
                    }
                    zobrazenePosilovace[i] = posilovaceKZobrazeni[i];
                }
            }
        }

        private void NactiTecku(int velikostPolicka)
        {
            int rozmer = velikostPolicka / 4;
            int poloha = velikostPolicka / 2 - rozmer / 2;

            Bitmap bmp = new Bitmap(velikostPolicka, velikostPolicka);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.FillEllipse(Brushes.Yellow, new Rectangle(poloha, poloha, rozmer, rozmer));
            }
            teckaImg = bmp;
        }

        private void NactiPosilovac(int velikostPolicka)
        {
            int rozmer = velikostPolicka / 2;
            int poloha = velikostPolicka / 2 - rozmer / 2;

            Bitmap bmp = new Bitmap(velikostPolicka, velikostPolicka);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.FillEllipse(Brushes.Orange, new Rectangle(poloha, poloha, rozmer, rozmer));
            }
            posilovacImg = bmp;
        }

        private void PripravitTecku(Point pozice, Size velikost)
        {
            PictureBox teckaPB = new PictureBox();
            teckaPB.Name = "Tecka";
            teckaPB.Image = teckaImg;
            teckaPB.Location = pozice;
            teckaPB.Size = velikost;
            teckaPB.SizeMode = PictureBoxSizeMode.Zoom;
            teckaPB.BackColor = Color.Transparent;
            teckaPB.Hide();
            zobrazeneTecky.Add(false);
            tecky.Add(teckaPB);
            Controls.Add(teckaPB);
        }

        private void PripravitPosilovac(Point pozice, Size velikost)
        {
            PictureBox posilovacPB = new PictureBox();
            posilovacPB.Name = "Posilovac";
            posilovacPB.Image = posilovacImg;
            posilovacPB.Location = pozice;
            posilovacPB.Size = velikost;
            posilovacPB.SizeMode = PictureBoxSizeMode.Zoom;
            posilovacPB.BackColor = Color.Transparent;
            posilovacPB.Hide();
            zobrazenePosilovace.Add(false);
            posilovace.Add(posilovacPB);
            Controls.Add(posilovacPB);
        }

        private Bitmap PripravBludiste(IBludiste bludiste, int velikostPolicka)
        {
            int pocetRadku = bludiste.PocetRadku;
            int pocetSloupcu = bludiste.PocetSloupcu;

            Bitmap bmp = new Bitmap(pocetSloupcu * velikostPolicka, pocetRadku * velikostPolicka);
            using (Graphics g = Graphics.FromImage(bmp))
            using (SolidBrush stetecZdi = new SolidBrush(barvaZdi))
            using (SolidBrush stetecDveri = new SolidBrush(barvaDveri))
            {
                int levyPosun = 0;
                int horniPosun = 0;

                for (int radek = 0; radek < pocetRadku; radek++)
                {
                    for (int sloupec = 0; sloupec < pocetSloupcu; sloupec++)
                    {
                        switch (bludiste.VratPolicko(radek, sloupec))
                        {
                            case Policko.Zed:
                                g.FillRectangle(stetecZdi, new Rectangle(levyPosun, horniPosun, velikostPolicka, velikostPolicka));
                                break;
                            case Policko.Dvere:
                                g.FillRectangle(stetecDveri, new Rectangle(levyPosun, horniPosun, velikostPolicka, velikostPolicka));
                                break;
                            case Policko.Tecka:
                                PripravitTecku(new Point(levyPosun, horniPosun), new Size(velikostPolicka, velikostPolicka));
                                break;
                            case Policko.Posilovac:
                                PripravitPosilovac(new Point(levyPosun, horniPosun), new Size(velikostPolicka, velikostPolicka));
                                break;
                        }
                        levyPosun += velikostPolicka;
                    }
                    horniPosun += velikostPolicka;
                    levyPosun = 0;
                }
            }

            return bmp;
        }
    }
}
