//This should really be some kind of UI that allows you to load and reload plugins

using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Bootstrap.IPC;
using EasyHook;
using Newtonsoft.Json;

namespace TestLoader
{
    public class Program
    {
        static void Main(string[] args)
        {
            IPCClient client = PromptForClient();

            Console.Clear();
            Console.WriteLine("AOSharp Injected!");

            client.Send(new LoadAssemblyMessage()
            {
                Assemblies = new List<string>()
                {
                    Directory.GetCurrentDirectory() + @"\..\..\..\TestPlugin\bin\Debug\TestPlugin.dll",
                    Directory.GetCurrentDirectory() + @"\..\..\..\CombatHandler\bin\Debug\CombatHandler.dll",
                    Directory.GetCurrentDirectory() + @"\..\..\..\MissionHelper\bin\Debug\MissionHelper.dll"
                }
            });

            Console.WriteLine("Plugins loaded.");

            Console.ReadLine();
        }

        public static IPCClient PromptForClient()
        {
            Process[] aoClients = Process.GetProcessesByName("AnarchyOnline");

            if (aoClients.Length == 0)
                Console.WriteLine("Error: There are no Anarchy Online clients open.");

            Console.WriteLine("Select a character.\n");

            int clientIndex = 0;
            foreach (Process aoClient in aoClients)
            {
                try
                {
                    Console.WriteLine("[{0}] {1}", clientIndex, aoClient.MainWindowTitle.Split('-', ' ')[4]);
                    clientIndex++;
                }
                catch
                {
                    Console.WriteLine("[{0}] No Character Selected", clientIndex);
                    clientIndex++;
                }
            }

            int selected;
            string line = Console.ReadLine();
            while (!int.TryParse(line, out selected) && selected > aoClients.Length && selected < 0)
            {
                Console.WriteLine("Invalid selection");
                Console.WriteLine("Select a character.");
                line = Console.ReadLine();
            }

            return Inject(aoClients[selected]);
        }

        public static IPCClient Attach(string name)
        {
            Process[] aoProcs = Process.GetProcessesByName("AnarchyOnline");

            if (aoProcs.Length == 0)
                throw new Exception($"No Anarchy Online clients found.");

            foreach (Process proc in aoProcs)
            {
                //Try to obtain the player name from the window title.
                string[] titleParts = proc.MainWindowTitle.Split('-', ' ');
                if (titleParts.Length > 4 && titleParts[4] == name)
                {
                    return Inject(proc);
                }
            }

            throw new Exception($"{name} was not found on any client.");
        }

        private static IPCClient Inject(Process proc)
        {
            RemoteHooking.Inject(proc.Id, "AOSharp.Bootstrap.dll", string.Empty, proc.Id.ToString(System.Globalization.CultureInfo.InvariantCulture));

            IPCClient pipe = new IPCClient(proc.Id.ToString());
            pipe.Connect();

            return pipe;
        }
    }
}
