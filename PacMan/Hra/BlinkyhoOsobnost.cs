using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Hra
{
    internal class BlinkyhoOsobnost : IOsobnost
    {
        public Pozice Cil
        {
            get { return pacMan.Pozice; }
        }

        public Pozice CilRozptylu { get; private set; }

        public bool ZustavaVDomecku { get { return false; } }

        private IPohyblivyObjekt pacMan;

        public BlinkyhoOsobnost(IPohyblivyObjekt pacMan, Pozice cilRozptylu)
        {
            this.pacMan = pacMan;
            CilRozptylu = cilRozptylu;
        }
    }
}
