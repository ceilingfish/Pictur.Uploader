using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace Ceilingfish.Pictur.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

            if (Console.IsInputRedirected)
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

                Console.WriteLine("Ctrl-C to exit");

                ConsoleKeyInfo key = Console.ReadKey();
                while (!(key.Modifiers == ConsoleModifiers.Control && key.Key == ConsoleKey.C))
                {
                    key = Console.ReadKey();
                }
            }
        }
    }
}
