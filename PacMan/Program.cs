using PacMan.GUI;
using PacMan.Hra;
using System.Reflection;
using System.Resources;

namespace PacMan
{
    internal static class Program
    {
        private static string bludiste = "PacMan.Mapy.bludiste_1.txt";
        private static string bludisteInfo = "PacMan.Mapy.bludiste_1";

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            try
            {
                HerniPlocha hra = new HerniPlocha(new Bludiste(bludiste, bludisteInfo));
                Application.Run(new PacManGUI(hra));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),
                    "Error Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
        }
    }
}