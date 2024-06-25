using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using PacMan.Hra;

namespace PacMan.GUI
{
    public partial class PacManGUI : Form
    {
        static private int dobaObnoveni = 20;
        static private int zvetseni = 2;
        
        private int velikostPole;

        private IHerniPlocha herniPlocha;
        private PozadiBludistePB pozadi;

        private PostavaPictureBox pacMan;
        private DuchPictureBox[] duchove;
        private string[] jmenaDuchu = new string[4] { "blinky", "pinky", "inky", "clyde" };

        private PictureBox[] ukazatelZivotu;
        private int pocetZivotu;
        private Label ukazatelCasu;
        private Label ukazatelSkore;
        private int uplynulyCas = 0;
        private Stopwatch stopky;

        System.Windows.Forms.Timer timer;

        public PacManGUI(IHerniPlocha plocha)
        {
            herniPlocha = plocha;
            velikostPole = herniPlocha.VelikostPolicka * zvetseni;

            pozadi = new PozadiBludistePB(herniPlocha.Bludiste, velikostPole);
            pacMan = new PostavaPictureBox("pac_man", velikostPole, zvetseni);

            duchove = new DuchPictureBox[jmenaDuchu.Length];
            for (int i = 0; i < jmenaDuchu.Length; i++)
            {
                duchove[i] = new DuchPictureBox(jmenaDuchu[i], velikostPole, zvetseni);
                Controls.Add(duchove[i]);
            }

            Controls.Add(pozadi);
            Controls.Add(pacMan);

            NactiZivoty();
            NactiCas();
            NactiSkore();

            InitializeComponent();
        }

        private void NactiZivoty()
        {
            ukazatelZivotu = new PictureBox[5];

            for (int i = 0; i < ukazatelZivotu.Length; i++)
            {
                ukazatelZivotu[i] = new PictureBox();

                ukazatelZivotu[i].Image = GUIZdroje.pac_man_right;
                ukazatelZivotu[i].Name = "Zivot";
                ukazatelZivotu[i].Size = new Size(velikostPole, velikostPole);
                ukazatelZivotu[i].SizeMode = PictureBoxSizeMode.Zoom;
                ukazatelZivotu[i].Location = new Point(velikostPole * i, pozadi.Height);
                ukazatelZivotu[i].Enabled = false;

                Controls.Add(ukazatelZivotu[i]);
            }
        }

        private void NactiSkore()
        {
            ukazatelSkore = new Label();

            ukazatelSkore.Name = "Skore";
            ukazatelSkore.Text = "Skóre...";
            ukazatelSkore.Size = new Size(pozadi.Width / 3, velikostPole);
            ukazatelSkore.ForeColor = Color.Orange;
            ukazatelSkore.Font = new Font("Arial", 22);
            ukazatelSkore.Location = new Point(pozadi.Width * 2 / 3, pozadi.Height + 5);

            Controls.Add(ukazatelSkore);
        }

        private void NactiCas()
        {
            ukazatelCasu = new Label();

            ukazatelCasu.Name = "Cas";
            ukazatelCasu.Text = "Čas...";
            ukazatelCasu.Size = new Size(pozadi.Width / 3, velikostPole);
            ukazatelCasu.ForeColor = Color.Orange;
            ukazatelCasu.Font = new Font("Arial", 22);
            ukazatelCasu.Location = new Point(pozadi.Width / 3, pozadi.Height + 5);

            Controls.Add(ukazatelCasu);
        }

        private void PrekreslitZivoty()
        {
            if (pocetZivotu == herniPlocha.PocetZivotu)
            {
                return;
            }

            for (int i = 0; i < ukazatelZivotu.Length; i++)
            {
                if (i < herniPlocha.PocetZivotu)
                {
                    ukazatelZivotu[i].Show();
                }
                else
                {
                    ukazatelZivotu[i].Hide();
                }
            }
            pocetZivotu = herniPlocha.PocetZivotu;
        }

        private void PrekresliSkore()
        {
            ukazatelSkore.Text = $"Skóre: {herniPlocha.Skore}";
        }

        private void PrekresliCas()
        {
            ukazatelCasu.Text = $"Čas: {uplynulyCas / 1000}";
        }

        private void SetTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = dobaObnoveni;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            stopky.Stop();
            int ms = stopky.Elapsed.Duration().Milliseconds;
            herniPlocha.Prepocitat(ms);
            stopky.Restart();
            uplynulyCas += ms;

            pacMan.Nastavit(herniPlocha.PacMan);

            duchove[0].Nastavit(herniPlocha.Blinky);
            duchove[1].Nastavit(herniPlocha.Pinky);
            duchove[2].Nastavit(herniPlocha.Inky);
            duchove[3].Nastavit(herniPlocha.Clyde);

            pozadi.ZobrazitTecky(herniPlocha.ZobrazeneTecky);
            pozadi.ZobrazitPosilovace(herniPlocha.ZobrazenePosilovace);

            PrekreslitZivoty();
            PrekresliSkore();
            PrekresliCas();

            if (herniPlocha.StavHry == StavHry.Prohra || herniPlocha.StavHry == StavHry.Vyhra)
            {
                bool vyhra = herniPlocha.StavHry == StavHry.Vyhra;

                timer.Stop();
                DialogResult odpoved = MessageBox.Show(
                   vyhra ? "Vyhráli jste, GRATULUJEME! Chcete začít novou hru?" : "Prohrali jste! Chcete začít novou hru?",
                   vyhra ? "*** Vyhra ***" : "Prohra :(",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Information);

                if (odpoved == DialogResult.Yes)
                {
                    herniPlocha.Obnovit();
                    timer.Start();
                    stopky = Stopwatch.StartNew();
                    uplynulyCas = 0;
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        private void PacManGUI_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new System.Drawing.Size(pozadi.Width, pozadi.Height + velikostPole + 5);
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            this.pozadi.SendToBack();
            this.BackColor = Color.Black;

            stopky = Stopwatch.StartNew();
            SetTimer();
        }

        private void PacManGUI_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                case Keys.Up:
                    herniPlocha.PacMan.PristiSmer = Smer.Nahoru;
                    break;
                case Keys.A:
                case Keys.Left:
                    herniPlocha.PacMan.PristiSmer = Smer.Vlevo;
                    break;
                case Keys.S:
                case Keys.Down:
                    herniPlocha.PacMan.PristiSmer = Smer.Dolu;
                    break;
                case Keys.D:
                case Keys.Right:
                    herniPlocha.PacMan.PristiSmer = Smer.Vpravo;
                    break;
            }
        }
    }
}
