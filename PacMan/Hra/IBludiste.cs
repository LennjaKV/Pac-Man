using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Hra
{
    public interface IBludiste
    {
        int PocetRadku { get; }
        int PocetSloupcu { get; }

        Tuple<int, int> Vychod { get; }
        Tuple<int, int> StartPacMana { get; }
        // Blinky
        Tuple<int, int> StartBlinkyho { get; }
        Tuple<int, int> RozptylBlinkyho { get; }
        // Pinky
        Tuple<int, int> StartPinkyho { get; }
        Tuple<int, int> RozptylPinkyho { get; }
        // Inky
        Tuple<int, int> StartInkyho { get; }
        Tuple<int, int> RozptylInkyho { get; }
        // Clyde
        Tuple<int, int> StartClyda { get; }
        Tuple<int, int> RozptylClyda { get; }

        Policko VratPolicko(int radek, int sloupec);
    }
}
