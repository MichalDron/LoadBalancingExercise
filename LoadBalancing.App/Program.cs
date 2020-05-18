using System;

namespace LoadBalancing.App
{
    internal class Program
    {
        private static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.WriteLine("Application started");

            await Startup.RunAsync();
        }
    }
}
