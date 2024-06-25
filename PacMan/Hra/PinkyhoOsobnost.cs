using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Hra
{
    internal class PinkyhoOsobnost : IOsobnost
    {
        public Pozice Cil 
        {
            get
            {
                switch (pacMan.Smer)
                {
                    case Smer.Nahoru:
                        return new Pozice(pacMan.Pozice.X, pacMan.Pozice.Y - 4 * velikostPole);
                    case Smer.Vlevo:
                        return new Pozice(pacMan.Pozice.X - 4 * velikostPole, pacMan.Pozice.Y);
                    case Smer.Dolu:
                        return new Pozice(pacMan.Pozice.X, pacMan.Pozice.Y + 4 * velikostPole);
                    case Smer.Vpravo:
                        return new Pozice(pacMan.Pozice.X + 4 * velikostPole, pacMan.Pozice.Y);
                    default:
                        return new Pozice(0, 0);
                }
            }
        }

        public Pozice CilRozptylu { get; private set; }

        public bool ZustavaVDomecku { get { return false; } }

        private IPohyblivyObjekt pacMan;
        private int velikostPole;

        public PinkyhoOsobnost(IPohyblivyObjekt pacMan, Pozice cilRozptylu, int velikostPole)
        {
            this.pacMan = pacMan;
            CilRozptylu = cilRozptylu;
            this.velikostPole = velikostPole;
        }
    }
}
