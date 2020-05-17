using System;

namespace LoadBalancing.App
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Startup.Run();

            Console.WriteLine("Type any key to close.");
            Console.ReadKey();
        }
    }
}
