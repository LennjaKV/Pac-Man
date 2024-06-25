using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Hra
{
    public interface IHerniPlocha
    {
        IBludiste Bludiste { get; }
        StavHry StavHry { get; }

        IPacMan PacMan { get; }
        IDuch Blinky { get; }
        IDuch Pinky { get; }
        IDuch Inky { get; }
        IDuch Clyde { get; }

        int VelikostPolicka { get; }
        int PocetZivotu { get; }
        int Skore { get; }

        int PocetTecek { get; }
        int PocetPosilovacu { get; }

        List<bool> ZobrazeneTecky { get; }
        List<bool> ZobrazenePosilovace { get; }

        void Prepocitat(int ms);
        void Obnovit();
    }
}
