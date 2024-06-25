using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Hra
{
    internal class ClydovaOsobnost : IOsobnost
    {
        public Pozice Cil
        {
            get
            {
                if (Pozice.Vzdalenost(pacMan.Pozice, clyde.Pozice) / velikostPole > 8)
                {
                    return CilRozptylu;
                }
                else
                {
                    return pacMan.Pozice;
                }
            }
        }

        public Pozice CilRozptylu { get; private set; }

        public bool ZustavaVDomecku { get { return hra.ZobrazeneTecky.Count - hra.PocetTecek < hra.ZobrazeneTecky.Count / 3; } }

        private IPohyblivyObjekt clyde;
        private IPohyblivyObjekt pacMan;
        private IHerniPlocha hra;
        private int velikostPole;

        public ClydovaOsobnost(IPohyblivyObjekt duch, IPohyblivyObjekt pacMan, IHerniPlocha hra, Pozice cilRozptylu, int velikostPole)
        {
            this.clyde = duch;
            this.pacMan = pacMan;
            this.hra = hra;
            CilRozptylu = cilRozptylu;
            this.velikostPole = velikostPole;
        }
    }
}
