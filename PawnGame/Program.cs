using System;

namespace PawnGame
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            using var game = new PawnGame.Game1();
            game.Run();
        }
    }
}
