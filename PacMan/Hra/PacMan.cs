using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Hra
{
    internal class PacMan : Postava, IPacMan
    {
        public Smer PristiSmer { get; set; }

        public PacMan(IBludiste bludiste, int velikostPolicka) : base(bludiste, velikostPolicka)
        {
            Smer = Smer.Nahoru;
            nedostupnaPolicka.Add(Policko.Dvere);
        }

        public override void Krok(int vzdalenost)
        {
            if (JeSmerVolny(PristiSmer))
            {
                Smer = PristiSmer;
            }

            base.Krok(vzdalenost);
        }
    }
}
