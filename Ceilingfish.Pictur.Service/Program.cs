using System;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using Serilog;

namespace Ceilingfish.Pictur.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {

            if (!args.Contains("-console"))
            {
                Log.Logger = new LoggerConfiguration()
                    .WriteTo
                    .NLog()
                    .CreateLogger();
                ServiceBase.Run(new UploaderService());
            }
            else
            {
                Log.Logger = new LoggerConfiguration()
                    .WriteTo
                    .ColoredConsole()
                    .CreateLogger();
                var token = new CancellationTokenSource();
                var uploader = new Uploader(token.Token);
                uploader.Execute();

                if (!Console.IsInputRedirected)
                {
                    Console.WriteLine("Ctrl-C to exit");

                    var key = Console.ReadKey();
                    while (!(key.Modifiers == ConsoleModifiers.Control && key.Key == ConsoleKey.C))
                    {
                        key = Console.ReadKey();
                    }

                    token.Cancel();
                }
                uploader.Wait();
            }
        }
    }
}
