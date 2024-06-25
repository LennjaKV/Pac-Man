using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Hra
{
    internal class CasModu
    {
        public int Cas { get; set; }
        public ModPohybu Mod { get; set; }

        public CasModu(int cas, ModPohybu mod)
        {
            Cas = cas;
            Mod = mod;
        }
    }

    internal class SpravceModu
    {
        private List<Duch> duchove;
        private List<CasModu> mody;
        private int panika = 0;
        private int uplynulyCas = 0;

        public SpravceModu() 
        {
            duchove = new List<Duch>();
            mody = new List<CasModu>();
        }

        public void PridatMod(int cas, ModPohybu mod)
        {
            if (mody.Count > 0)
            {
                cas += mody.Last().Cas;
            }

            mody.Add(new CasModu(cas, mod));
        }

        public void PridatDucha(Duch duch)
        {
            duchove.Add(duch);
        }

        public void Akttualizuj(int cas)
        {
            uplynulyCas += cas;

            if (mody.Count > 1 && mody[0].Cas < uplynulyCas)
            {
                mody.RemoveAt(0);
            }

            foreach (Duch duch in duchove)
            {
                if (uplynulyCas > panika || duch.ModPohybu != ModPohybu.Panika)
                {
                    duch.ModPohybu = mody[0].Mod;
                }
            }
        }

        public void SpustPaniku()
        {
            panika = uplynulyCas + 7000;

            foreach (Duch duch in duchove)
            {
                duch.ModPohybu = ModPohybu.Panika;
            }
        }
    }
}
