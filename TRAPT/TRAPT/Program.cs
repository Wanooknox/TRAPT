using System;

namespace TRAPT
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (TraptMain game = new TraptMain())
            {
                game.Run();
            }
        }
    }
#endif
}

