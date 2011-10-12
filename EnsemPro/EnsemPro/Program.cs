using System;

namespace EnsemPro
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameEngine game = new GameEngine())
            {
                game.Run();
            }
        }
    }
#endif
}

