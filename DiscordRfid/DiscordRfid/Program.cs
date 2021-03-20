using DiscordRfid.Views;
using Serilog;
using System;
using System.IO;
using System.Windows.Forms;

namespace DiscordRfid
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel
#if DEBUG
                .Verbose()
#else
                .Information()
#endif
                .WriteTo.File(Path.Combine("log", "log.txt"),
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            Log.Information("Starting point. Booting up");

            AppDomain.CurrentDomain.UnhandledException += (o, args) =>
                Log.Error(args.ExceptionObject as Exception, "Unhandled exception");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
