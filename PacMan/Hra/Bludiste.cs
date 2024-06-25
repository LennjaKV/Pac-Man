using PacMan.Mapy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Hra
{
    internal class Bludiste : IBludiste
    {
        public int PocetRadku { get; private set; }
        public int PocetSloupcu { get; private set; }

        public Tuple<int, int> Vychod { get; private set; }
        public Tuple<int, int> StartPacMana { get; private set; }
        // Blinky
        public Tuple<int, int> StartBlinkyho { get; private set; }
        public Tuple<int, int> RozptylBlinkyho { get; private set; }
        // Pinky
        public Tuple<int, int> StartPinkyho { get; private set; }
        public Tuple<int, int> RozptylPinkyho { get; private set; }
        // Inky
        public Tuple<int, int> StartInkyho { get; private set; }
        public Tuple<int, int> RozptylInkyho { get; private set; }
        // Clyde
        public Tuple<int, int> StartClyda { get; private set; }
        public Tuple<int, int> RozptylClyda { get; private set; }

        public Policko VratPolicko(int indexRadku, int indexSloupce)
        {
            if (indexRadku >= PocetRadku || indexSloupce >= PocetSloupcu)
            {
                throw new ArgumentException($"Policko [{indexRadku}, {indexSloupce}] neexistuje");
            }
            return policka[indexRadku, indexSloupce];
        }

        private Policko[,] policka;

        private static Policko[,] NactiBludiste(Stream? stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("null stream");
            }

            List<string> radky = new List<string>();
            using (var reader = new StreamReader(stream))
            {
                string radek;
                while ((radek = reader.ReadLine()) != null)
                {
                    radky.Add(radek);
                }
            }

            int pocetRadku = radky.Count;
            int pocetSloupcu = radky[0].Length;

            Policko[,] policka = new Policko[pocetRadku, pocetSloupcu];

            for (int radekIdx = 0; radekIdx < pocetRadku; radekIdx++)
            {
                for (int sloupecIdx = 0; sloupecIdx < pocetSloupcu; sloupecIdx++)
                {
                    if (radky[radekIdx].Length != pocetSloupcu)
                    {
                        throw new ArgumentException("Vsechny radky musi byt stejne dlouhe");
                    }

                    switch (radky[radekIdx][sloupecIdx])
                    {
                        case 'X':
                            policka[radekIdx, sloupecIdx] = Policko.Zed;
                            break;
                        case '.':
                            policka[radekIdx, sloupecIdx] = Policko.Tecka;
                            break;
                        case 'o':
                            policka[radekIdx, sloupecIdx] = Policko.Posilovac;
                            break;
                        case '^':
                            policka[radekIdx, sloupecIdx] = Policko.Dvere;
                            break;
                        case ' ':
                            policka[radekIdx, sloupecIdx] = Policko.Nic;
                            break;
                        default:
                            throw new ArgumentException("Nezname policko");
                    }
                }
            }

            return policka;
        }

        static private Tuple<int, int> NactiPozici(ResourceManager manager, string zdroj)
        {
            var radekStr = manager.GetString($"{zdroj}.radekIdx");
            var sloupecStr = manager.GetString($"{zdroj}.sloupecIdx");

            if (radekStr == null || sloupecStr == null)
            {
                throw new ArgumentException("Nepodarilo se nacist pozici ze zdroje");
            }

            int radekIdx = Int32.Parse(radekStr);
            int sloupecIdx = Int32.Parse(sloupecStr);

            return new Tuple<int, int>(radekIdx, sloupecIdx);
        }

        private void NactiInfo(ResourceManager manager)
        {
            Vychod = NactiPozici(manager, "Vychod");
            StartPacMana = NactiPozici(manager, "PacManStart");
            // Blinky
            StartBlinkyho = NactiPozici(manager, "BlinkyStart");
            RozptylBlinkyho = NactiPozici(manager, "BlinkyRozptyl");
            // Pinky
            StartPinkyho = NactiPozici(manager, "PinkyStart");
            RozptylPinkyho = NactiPozici(manager, "PinkyRozptyl");
            // Inky
            StartInkyho = NactiPozici(manager, "InkyStart");
            RozptylInkyho = NactiPozici(manager, "InkyRozptyl");
            // Clyde
            StartClyda = NactiPozici(manager, "ClydeStart");
            RozptylClyda = NactiPozici(manager, "ClydeRozptyl");
        }

        public Bludiste(string zdrojBludiste, string zdrojBludisteInfo)
        {
            Assembly assm = Assembly.GetExecutingAssembly();

            using (var stream = assm.GetManifestResourceStream(zdrojBludiste))
            {
                policka = NactiBludiste(stream);
                PocetRadku = policka.GetLength(0);
                PocetSloupcu = policka.GetLength(1);
            }

            ResourceManager manager = new ResourceManager(zdrojBludisteInfo, assm);
            NactiInfo(manager);
        }
    }
}
