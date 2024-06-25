using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Hra
{
    // Mod pohybu / volby cile
    // Pronasledovani - sleduje PacMana
    // CestaDomu - cil je nastaven na domovskou souradnici (PacMan si muze chvilku oddzchnut)
    // Panika - PacMan sezral posilovac, duch zdrha a v panice nahodne vybira smer
    public enum ModPohybu
    {
        Pronasledovani,
        Rozptyl,
        Panika
    }

    public enum Policko
    {
        Zed,
        Dvere,
        Tecka,
        Posilovac,
        Nic
    }

    public enum Smer
    {
        Nahoru,
        Dolu,
        Vlevo,
        Vpravo
    }

    public enum StavHry
    {
        Vyhra,
        Prohra,
        Hraje
    }

    public class Pozice
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Pozice(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static double Vzdalenost(Pozice A, Pozice B)
        {
            double vzdalenostX = Math.Abs(A.X - B.X);
            double vzdalenostY = Math.Abs(A.Y - B.Y);

            return Math.Sqrt(Math.Pow(vzdalenostX, 2) + Math.Pow(vzdalenostY, 2));
        }
    }
}
