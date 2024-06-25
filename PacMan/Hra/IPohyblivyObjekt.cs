using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Hra
{
    public interface IPohyblivyObjekt
    {
        Pozice Pozice { get; }
        Smer Smer { get; }
    }
}
