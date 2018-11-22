using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AOSharp.Bootstrap.IPC
{
    public class LoadAssemblyMessage : IPCMessage
    {
        public string Assemblies;

        public LoadAssemblyMessage() : base((byte)HookOpCode.LoadAssembly) { }

        protected override void OnSerialize(BinaryWriter writer)
        {
            //writer.Write(JsonConvert.SerializeObject(Assemblies));
        }

        protected override void OnDeserialize(BinaryReader reader)
        {
            //Assemblies = JsonConvert.DeserializeObject<List<string>>(reader.ReadString());
        }
    }

    public class AssemblyCollection
    {
        public List<string> Assemblies;
        public List<string> Dependencies;
    }
}
