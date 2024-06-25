using PacMan.Mapy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace PacMan.Hra
{
    public class HerniPlocha : IHerniPlocha
    {
        public IBludiste Bludiste { get; private set; }
        public StavHry StavHry { get; private set; }

        public IPacMan PacMan { get; private set; }
        public IDuch Blinky { get; private set; }
        public IDuch Pinky { get; private set; }
        public IDuch Inky { get; private set; }
        public IDuch Clyde { get; private set; }

        public int VelikostPolicka { get; private set; } = 20;
        public int PocetZivotu { get; private set; }
        public int Skore { get; private set; }

        public List<bool> ZobrazeneTecky { get; private set; } = new List<bool>();
        public List<bool> ZobrazenePosilovace { get; private set; } = new List<bool>();

        public int PocetTecek { get; private set; }
        public int PocetPosilovacu { get; private set; }

        static private int velikostKroku = 2;

        private int[,] indexyTecek;
        private int[,] indexyPosilovacu;

        private PacMan pacMan;
        private Duch[] duchove;

        private SpravceModu spravceModu;

        public void Obnovit()
        {
            spravceModu = new SpravceModu();
            // 1. 7 sekund Scatter, potom 20 sekund Chase          
            spravceModu.PridatMod(7000, ModPohybu.Rozptyl);
            spravceModu.PridatMod(20000, ModPohybu.Pronasledovani);
            // 2. 7 sekund Scatter, potom 20 sekund Chase
            spravceModu.PridatMod(7000, ModPohybu.Rozptyl);
            spravceModu.PridatMod(20000, ModPohybu.Pronasledovani);
            // 3. 5 sekund Scatter, potom 20 sekund Chase
            spravceModu.PridatMod(5000, ModPohybu.Rozptyl);
            spravceModu.PridatMod(20000, ModPohybu.Pronasledovani);
            // 4. 5 sekund Scatter, potom trvale přepnout na Chase
            spravceModu.PridatMod(5000, ModPohybu.Rozptyl);
            spravceModu.PridatMod(20000, ModPohybu.Pronasledovani);

            PocetZivotu = 3;
            Skore = 0;

            NactiBludiste();
            NactiPostavy();

            StavHry = StavHry.Hraje;
        }

        public void Prepocitat(int cas)
        {
            spravceModu.Akttualizuj(cas);
            UdelatKrok(cas);

            SnistPolicko();
            OtestovatKolize();
           
            if (PocetTecek == 0 && PocetPosilovacu == 0)
            {
                StavHry = StavHry.Vyhra;
            }
        }

        private void Umrit()
        {
            PocetZivotu -= 1;
            if (PocetZivotu == 0)
            {
                StavHry = StavHry.Prohra;
            }
            else
            {
                NactiPostavy();
            }
        }

        private void SnistTecku(int radek, int sloupec)
        {
            int index = indexyTecek[radek, sloupec];

            if (index != -1 && index < ZobrazeneTecky.Count && ZobrazeneTecky[index])
            {
                ZobrazeneTecky[index] = false;
                PocetTecek--;
                Skore += 10;
            }
        }

        private void SnistPosilovac(int radek, int sloupec)
        {
            int index = indexyPosilovacu[radek, sloupec];

            if (index != -1 && index < ZobrazenePosilovace.Count && ZobrazenePosilovace[index])
            {
                ZobrazenePosilovace[index] = false;
                PocetPosilovacu--;
                Skore += 20;
                spravceModu.SpustPaniku();
            }
        }

        private void SnistPolicko()
        {
            if (pacMan.Pozice.X % VelikostPolicka == 0 && pacMan.Pozice.Y % VelikostPolicka == 0)
            {
                int radek = pacMan.Pozice.Y / VelikostPolicka;
                int sloupec = pacMan.Pozice.X / VelikostPolicka;

                SnistTecku(radek, sloupec);
                SnistPosilovac(radek, sloupec);
            }
        }

        private void OtestovatKolize()
        {
            foreach (Duch duch in duchove)
            {
                double vzdalenost = Pozice.Vzdalenost(pacMan.Pozice, duch.Pozice);
                if (vzdalenost <= VelikostPolicka / 2)
                {
                    if (duch.ModPohybu == ModPohybu.Panika)
                    {
                        duch.Obnovit();
                        duch.ZavritVDomecku(duch.VychoziPozice, 3000);
                        Skore += 100;
                    }
                    else
                    {
                        Umrit();
                    }
                }
            }
        }

        private void UdelatKrok(int cas)
        {
            pacMan.Krok(velikostKroku);
            foreach (Duch duch in duchove)
            {
                duch.Krok(velikostKroku, cas);
            }
        }

        public HerniPlocha(IBludiste bludiste)
        {
            Bludiste = bludiste;
            indexyTecek = new int[bludiste.PocetRadku, bludiste.PocetSloupcu];
            indexyPosilovacu = new int[bludiste.PocetRadku, bludiste.PocetSloupcu];

            Obnovit();
        }

        private void NactiBludiste()
        {
            ZobrazeneTecky.Clear();
            ZobrazenePosilovace.Clear();

            for (int radekIdx = 0; radekIdx < Bludiste.PocetRadku; radekIdx++)
            {
                for (int sloupecIdx = 0; sloupecIdx < Bludiste.PocetSloupcu; sloupecIdx++)
                {
                    switch (Bludiste.VratPolicko(radekIdx, sloupecIdx))
                    {
                        case Policko.Tecka:
                            indexyTecek[radekIdx, sloupecIdx] = ZobrazeneTecky.Count;
                            indexyPosilovacu[radekIdx, sloupecIdx] = -1;
                            ZobrazeneTecky.Add(true);
                            break;
                        case Policko.Posilovac:
                            indexyTecek[radekIdx, sloupecIdx] = -1;
                            indexyPosilovacu[radekIdx, sloupecIdx] = ZobrazenePosilovace.Count;
                            ZobrazenePosilovace.Add(true);
                            break;
                        default:
                            indexyTecek[radekIdx, sloupecIdx] = -1;
                            indexyPosilovacu[radekIdx, sloupecIdx] = -1;
                            break;
                    }
                }
            }

            PocetTecek = ZobrazeneTecky.Count;
            PocetPosilovacu = ZobrazenePosilovace.Count;
        }

        private void NactiPostavy()
        {
            pacMan = new PacMan(Bludiste, VelikostPolicka);
            duchove = new Duch[4];
            for (int i = 0; i < duchove.Length; i++)
            {
                duchove[i] = new Duch(Bludiste, VelikostPolicka);
            }

            Duch blinky = duchove[0];
            Duch pinky = duchove[1];
            Duch inky = duchove[2];
            Duch clyde = duchove[3];

            pacMan.VychoziPozice = new Pozice(Bludiste.StartPacMana.Item2 * VelikostPolicka, Bludiste.StartPacMana.Item1 * VelikostPolicka);
            PacMan = pacMan;

            // Blinky
            blinky.VychoziPozice = new Pozice(Bludiste.StartBlinkyho.Item2 * VelikostPolicka, Bludiste.StartBlinkyho.Item1 * VelikostPolicka);
            blinky.Osobnost = new BlinkyhoOsobnost(pacMan, new Pozice(Bludiste.RozptylBlinkyho.Item2 * VelikostPolicka, Bludiste.RozptylBlinkyho.Item1 * VelikostPolicka));
            // Pinky
            pinky.VychoziPozice = new Pozice(Bludiste.StartPinkyho.Item2 * VelikostPolicka, Bludiste.StartPinkyho.Item1 * VelikostPolicka);
            pinky.Osobnost = new PinkyhoOsobnost(pacMan, new Pozice(Bludiste.RozptylPinkyho.Item2 * VelikostPolicka, Bludiste.RozptylPinkyho.Item1 * VelikostPolicka), VelikostPolicka);
            // Inky
            inky.VychoziPozice = new Pozice(Bludiste.StartInkyho.Item2 * VelikostPolicka, Bludiste.StartInkyho.Item1 * VelikostPolicka);
            inky.Osobnost = new InkyhoOsobnost(pacMan, blinky, this, new Pozice(Bludiste.RozptylInkyho.Item2 * VelikostPolicka, Bludiste.RozptylInkyho.Item1 * VelikostPolicka), VelikostPolicka);
            // Clyde
            clyde.VychoziPozice = new Pozice(Bludiste.StartClyda.Item2 * VelikostPolicka, Bludiste.StartClyda.Item1 * VelikostPolicka);
            clyde.Osobnost = new ClydovaOsobnost(clyde, pacMan, this, new Pozice(Bludiste.RozptylClyda.Item2 * VelikostPolicka, Bludiste.RozptylClyda.Item1 * VelikostPolicka), VelikostPolicka);

            Blinky = blinky;
            Pinky = pinky;
            Inky = inky;
            Clyde = clyde;

            foreach (Duch duch in duchove)
            {
                spravceModu.PridatDucha(duch);
                duch.Obnovit();
            }
            pacMan.Obnovit();

            // :D blinky ma vychoyi pozici mimo domecek, ale pokud ho chzti, kdyz ma posilovac, tak chceme aby se vratil do domecku..
            blinky.VychoziPozice = pinky.VychoziPozice;
        }
    }
}
