using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace PacMan.Hra
{

    internal class Duch : Postava, IDuch
    {
        private ModPohybu modPohybu;
        public ModPohybu ModPohybu { 
            get
            {
                return modPohybu; 
            }
            set
            {
                // Pri zmene modu se vzdy otaci smer
                if (value != modPohybu)
                {
                    Smer = VratOpacnySmer(Smer);
                }
                modPohybu = value;
            }
        }
        public IOsobnost? Osobnost { get; set; } = null;

        private Pozice vychodZDomecku;
        bool opustilDomecek = false;

        private int uplynulyCas = 0;
        private bool zavrenyVDomecku = false;
        private int casPropusteni = 0;

        public Duch(IBludiste bludiste, int velikostPolicka) : base(bludiste, velikostPolicka)
        {
            Smer = Smer.Vlevo;
            ModPohybu = ModPohybu.Rozptyl;
            vychodZDomecku = new Pozice(bludiste.Vychod.Item2 * velikostPolicka, bludiste.Vychod.Item1 * velikostPolicka);
        }

        public void Krok(int vzdalenost, int pocetMs)
        {
            uplynulyCas += pocetMs;

            if (!opustilDomecek)
            {
                if (ProselDvermi())
                {
                    opustilDomecek = true;
                    ZavritDvere();
                    Smer = Smer.Vlevo;
                    base.Krok(vzdalenost);
                    return;
                }
                else if (zavrenyVDomecku)
                {
                    if (uplynulyCas > casPropusteni)
                    {
                        PropustitZDomecku();
                    }
                }
                else if (Osobnost.ZustavaVDomecku)
                {
                    ZavritDvere();
                }
                else
                {
                    OtevritDvere();
                }
            }

            // Prehodnocovat smer ma smysl pouze tehdy, jsme-li presne zarovnani na policku
            if (Pozice.X % velikostPolicka == 0 && Pozice.Y % velikostPolicka == 0)
            {
                ZvolSmer();
            }

            base.Krok(vzdalenost);
        }

        public override void Obnovit()
        {
            opustilDomecek = false;
            ModPohybu = ModPohybu.Rozptyl;
            base.Obnovit();
        }

        public void ZavritVDomecku(Pozice poziceDomecku, int doba)
        {
            zavrenyVDomecku = true;
            casPropusteni = uplynulyCas + doba;
            ZavritDvere();
        }

        private void PropustitZDomecku()
        {
            zavrenyVDomecku = false;
            OtevritDvere();
        }

        private void ZavritDvere()
        {
            if (nedostupnaPolicka.Contains(Policko.Dvere) == false)
            {
                nedostupnaPolicka.Add(Policko.Dvere);
            }
        }

        private void OtevritDvere()
        {
            nedostupnaPolicka.Remove(Policko.Dvere);
        }

        private bool ProselDvermi()
        {
            return Pozice.X == vychodZDomecku.X && Pozice.Y == vychodZDomecku.Y;
        }

        private static Smer VratOpacnySmer(Smer smer)
        {
            switch (smer)
            {
                case Smer.Nahoru:
                    return Smer.Dolu;
                case Smer.Vlevo:
                    return Smer.Vpravo;
                case Smer.Dolu:
                    return Smer.Nahoru;
                case Smer.Vpravo:
                    return Smer.Vlevo;
                default:
                    return Smer.Dolu;
            }
        }

        private Pozice ZvolCil()
        {
            if (!opustilDomecek)
            {
                return vychodZDomecku;
            }
            if (ModPohybu == ModPohybu.Rozptyl)
            {
                return Osobnost.CilRozptylu;
            }
            else
            { 
                return Osobnost.Cil;
            }
        }

        private static int SpocitejVzdalenost(Pozice A, Pozice B)
        {
            return (int) Pozice.Vzdalenost(A, B) / velikostPolicka;
        }

        // Teda tohle vazne nevybyra uplne nejkratsi cestu :D
        private Smer VyberNejkratsiCestu(List<Smer> mozneSmery)
        {
            Pozice cil = ZvolCil();

            int nejkratsiVzdalenost = Int32.MaxValue;
            Smer zvolenySmer = Smer.Nahoru;

            foreach (Smer smer in mozneSmery)
            {
                int vzdalenostVeSmeru = 0;
                switch (smer)
                {
                    case Smer.Nahoru:
                        vzdalenostVeSmeru = SpocitejVzdalenost(cil, new Pozice(Pozice.X, Pozice.Y - velikostPolicka));
                        break;
                    case Smer.Vlevo:
                        vzdalenostVeSmeru = SpocitejVzdalenost(cil, new Pozice(Pozice.X - velikostPolicka, Pozice.Y));
                        break;
                    case Smer.Dolu:
                        vzdalenostVeSmeru = SpocitejVzdalenost(cil, new Pozice(Pozice.X, Pozice.Y + velikostPolicka));
                        break;
                    case Smer.Vpravo:
                        vzdalenostVeSmeru = SpocitejVzdalenost(cil, new Pozice(Pozice.X + velikostPolicka, Pozice.Y));
                        break;
                }

                if (vzdalenostVeSmeru < nejkratsiVzdalenost)
                {
                    nejkratsiVzdalenost = vzdalenostVeSmeru;
                    zvolenySmer = smer;
                }
            }

            return zvolenySmer;
        }

        private void ZvolSmer()
        {
            List<Smer> volneSmery = new List<Smer>();
            
            if (JeSmerVolny(Smer.Nahoru)) { volneSmery.Add(Smer.Nahoru);}
            if (JeSmerVolny(Smer.Vlevo))  { volneSmery.Add(Smer.Vlevo); }
            if (JeSmerVolny(Smer.Dolu))   { volneSmery.Add(Smer.Dolu);  }
            if (JeSmerVolny(Smer.Vpravo)) { volneSmery.Add(Smer.Vpravo);}

            if (volneSmery.Count == 1)
            {
                // Jsme ve slepe ulicce, nezbyva nic nez jit jedinym volnym smerem (cos bude otocka o 180)
                Smer = volneSmery[0];
            }
            else
            {
                // Ted uz si muzeme trochu vzbyrat, musime tedy vyradit opacny smer (Duch nedela otocku o 180, kdyz nemusi)
                volneSmery.Remove(VratOpacnySmer(Smer));

                if (volneSmery.Count == 1)
                {
                    // Byly presne 2 volne smery, opacny sme odstranili, takzy zbyva jen jediny
                    Smer = volneSmery[0];
                }
                else
                {
                    // Krizovatka, muzem prehodnotit smer
                    if (ModPohybu != ModPohybu.Panika)
                    {
                        // Pokud nezdrhame pred PacManem, volime smer na zaklade cile (PacMana nebo domecku)
                        Smer = VyberNejkratsiCestu(volneSmery);
                    }
                    else
                    {
                        // Pokud panikarime, volime smer nahodne...
                        Random rnd = new Random();
                        Smer = volneSmery[rnd.Next(0, volneSmery.Count - 1)];
                    }
                }
            }
        }
    }
}
