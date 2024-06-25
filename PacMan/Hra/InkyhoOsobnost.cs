using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Hra
{
    internal class InkyhoOsobnost : IOsobnost
    {
        public Pozice Cil
        {
            get
            {
                Pozice start;

                switch (pacMan.Smer)
                {
                    case Smer.Nahoru:
                        start = new Pozice(pacMan.Pozice.X, pacMan.Pozice.Y - 2 * velikostPole);
                        break;
                    case Smer.Vlevo:
                        start = new Pozice(pacMan.Pozice.X - 2 * velikostPole, pacMan.Pozice.Y);
                        break;
                    case Smer.Dolu:
                        start = new Pozice(pacMan.Pozice.X, pacMan.Pozice.Y + 2 * velikostPole);
                        break;
                    case Smer.Vpravo:
                        start = new Pozice(pacMan.Pozice.X + 2 * velikostPole, pacMan.Pozice.Y);
                        break;
                    default:
                        start = new Pozice(0, 0);
                        break;
                }

                int vektorX = 2 * (start.X - inky.Pozice.X);
                int vektorY = 2 * (start.Y - inky.Pozice.Y);

                return new Pozice(inky.Pozice.X + vektorX, inky.Pozice.Y + vektorY);
            }
        }

        public Pozice CilRozptylu { get; private set; }

        public bool ZustavaVDomecku { get { return hra.ZobrazeneTecky.Count - hra.PocetTecek < 30; } }

        private IPohyblivyObjekt pacMan;
        private IPohyblivyObjekt inky;
        private IHerniPlocha hra;
        private int velikostPole;

        public InkyhoOsobnost(IPohyblivyObjekt pacMan, IPohyblivyObjekt blinky, IHerniPlocha hra, Pozice cilRozptylu, int velikostPole)
        {
            this.pacMan = pacMan;
            this.inky = blinky;
            this.hra = hra;
            CilRozptylu = cilRozptylu;
            this.velikostPole = velikostPole;
        }
    }
}
