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
            IPCClient client = Attach("");

            client.Send(new LoadAssemblyMessage()
            {
                Assemblies = new List<string>()
                {
                    Directory.GetCurrentDirectory() + @"\..\..\..\TestPlugin\bin\Debug\TestPlugin.dll",
                    Directory.GetCurrentDirectory() + @"\..\..\..\CombatHandler\bin\Debug\CombatHandler.dll"
                }
            });

            Console.ReadLine();
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
