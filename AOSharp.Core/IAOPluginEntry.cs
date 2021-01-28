using System;

namespace AOSharp.Core
{
    public interface IAOPluginEntry
    {
        void Run(string pluginDir);
        void Teardown();
    }

    public abstract class AOPluginEntry : IAOPluginEntry
    {
        public abstract void Run(string pluginDir);
        public virtual void Teardown() 
        {
        }
    }
}
