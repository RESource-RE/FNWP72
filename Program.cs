using System;
using Mortar;

namespace FNWP72
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (var game = new TheGame())
            {
                game.Run();
            }
        }
    }
}
