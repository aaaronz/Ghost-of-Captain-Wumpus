using System;

namespace WumpusXNA
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GUI game = new GUI())
            {
                game.Run();
            }
        }
    }
#endif
}

