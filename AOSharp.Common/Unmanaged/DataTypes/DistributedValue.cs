using System;
using AOSharp.Common.Unmanaged.Imports;

namespace AOSharp.Common.Unmanaged.DataTypes
{ 
    public class DistributedValue
    {
        public static void Create(string name, int value)
        {
            Create(name, Variant.Create(value));
        }

        public static void Create(string name, float value)
        {
            Create(name, Variant.Create(value));
        }

        public static void Create(string name, bool value)
        {
            Create(name, Variant.Create(value));
        }

        public static void Create(string name, Variant value)
        {
            StdString nameStr = StdString.Create(name);
            DistributedValue_c.AddVariable(nameStr.Pointer, value.Pointer, false, false);
        }

        public static void LoadConfig(string path, int category, bool addVariables)
        {
            StdString pathStr = StdString.Create(path);
            DistributedValue_c.LoadConfig(pathStr.Pointer, category, addVariables);
        }

        public static void SaveConfig(string path, int category)
        {
            StdString pathStr = StdString.Create(path);
            DistributedValue_c.SaveConfig(pathStr.Pointer, category);
        }
    }
}
