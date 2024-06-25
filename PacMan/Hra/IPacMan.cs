using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Hra
{
    public interface IPacMan : IPohyblivyObjekt
    {
        Smer PristiSmer { get; set; }
    }
}
