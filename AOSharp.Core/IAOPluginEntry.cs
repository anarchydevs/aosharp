using System;

namespace AOSharp.Core
{
    [Obsolete("Plugins should inherit from the AOPluginEntry class", false)]
    public interface IAOPluginEntry
    {
        void Run(string pluginDir);
    }

#pragma warning disable 618
    public abstract class AOPluginEntry : IAOPluginEntry
#pragma warning restore 618
    {
        public abstract void Run(string pluginDir);
    }
}
