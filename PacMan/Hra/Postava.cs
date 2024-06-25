using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Hra
{
    internal class Postava
    {
        public Pozice Pozice { get; set; }
        public Pozice VychoziPozice { get; set; }
        public Smer Smer { get; protected set; }

        protected IBludiste bludiste;
        protected static int velikostPolicka;
        protected List<Policko> nedostupnaPolicka;

        public Postava(IBludiste bludiste, int velikostPolicka)
        {
            this.bludiste = bludiste;
            Pozice = new Pozice(0, 0);
            VychoziPozice = new Pozice(0, 0);
            Postava.velikostPolicka = velikostPolicka;

            nedostupnaPolicka = new List<Policko>() { Policko.Zed };
        }

        public virtual void Krok(int vzdalenost)
        {
            if (JeSmerVolny(Smer))
            {
                switch (Smer)
                {
                    case Smer.Nahoru:
                        Pozice = new Pozice(Pozice.X, Pozice.Y - vzdalenost);
                        if (Pozice.Y < 0)
                        {
                            Pozice.Y = (bludiste.PocetRadku - 1) * velikostPolicka;
                        }
                        break;
                    case Smer.Vlevo:
                        Pozice = new Pozice(Pozice.X - vzdalenost, Pozice.Y);
                        if (Pozice.X < 0)
                        {
                            Pozice.X = (bludiste.PocetSloupcu - 1) * velikostPolicka;
                        }
                        break;
                    case Smer.Dolu:
                        Pozice = new Pozice(Pozice.X, Pozice.Y + vzdalenost);
                        if (Pozice.Y / velikostPolicka >= bludiste.PocetRadku)
                        {
                            Pozice.Y = 0;
                        }
                        break;
                    case Smer.Vpravo:
                        Pozice = new Pozice(Pozice.X + vzdalenost, Pozice.Y);
                        if (Pozice.X / velikostPolicka >= bludiste.PocetSloupcu)
                        {
                            Pozice.X = 0;
                        }
                        break;
                }
            }
        }

        public virtual void Obnovit()
        {
            Pozice = new Pozice(VychoziPozice.X, VychoziPozice.Y);
        }

        private bool JePoziceVolna(Pozice pozice)
        {
            int radek = pozice.Y / velikostPolicka;
            int sloupec = pozice.X / velikostPolicka;

            if (radek < 0 || radek >= bludiste.PocetRadku ||
                sloupec < 0 || sloupec >= bludiste.PocetSloupcu)
            {
                // Umoznit cestu mimo rozsah bludiste (Projit portalem na druhou stranu)
                return true;
            }

            Policko policko = bludiste.VratPolicko(radek, sloupec);

            foreach (Policko p in nedostupnaPolicka)
            {
                if (policko == p)
                {
                    return false;
                }
            }
            return true;
        }

        protected bool JeSmerVolny(Smer smer)
        {
            switch (smer)
            {
                case Smer.Nahoru:
                    // y klesa;
                    // Volno pouze pokud je ve vodorovne ose (X) presne zarovnan na policko a ma nad sebou prostor
                    return Pozice.X % velikostPolicka == 0 && JePoziceVolna(new Pozice(Pozice.X, Pozice.Y - 1));
                case Smer.Vlevo:
                    // x klesa;
                    // Volno pouze pokud je ve svisle ose (Y) presne zarovnan na policko a ma vlevo prostor
                    return Pozice.Y % velikostPolicka == 0 && JePoziceVolna(new Pozice(Pozice.X - 1, Pozice.Y));
                case Smer.Dolu:
                    // y roste
                    // Volno pouze pokud je ve vodorovne ose (X) presne zarovnan na policko a ma pod sebou prostor
                    return Pozice.X % velikostPolicka == 0 && JePoziceVolna(new Pozice(Pozice.X, Pozice.Y + velikostPolicka + 1));
                case Smer.Vpravo:
                    // x roste;
                    // Volno pouze pokud je ve svisle ose (Y) presne zarovnan na policko a ma vpravo prostor
                    return Pozice.Y % velikostPolicka == 0 && JePoziceVolna(new Pozice(Pozice.X + velikostPolicka + 1, Pozice.Y));
                default:
                    // Co kdyby existoval jeste nejakej jinej smer :D
                    return false;
            }
        }
    }
}
