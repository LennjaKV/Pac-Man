using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Hra
{
    internal interface IOsobnost
    {
        Pozice Cil { get; }
        Pozice CilRozptylu { get; }
        bool ZustavaVDomecku { get; }
    }
}
