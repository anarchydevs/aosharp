using AOSharp.Common.Unmanaged.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOSharp.Core
{
    public class Settings
    {
        private readonly string _groupName;
        private List<string> _variables = new List<string>();

        public Variant this[string name]
        {
            get
            {
                if (!_variables.Contains(name))
                    return null;

                return DistributedValue.GetDValue($"{_groupName}__{name}", false);
            }

            set
            {
                DistributedValue.SetDValue($"{_groupName}__{name}", value);
            }
        }

        public Settings(string groupName)
        {
            _groupName = groupName;
        }

        public void AddVariable(string name, int value)
        {
            DistributedValue.Create($"{_groupName}__{name}", value);
            _variables.Add(name);
        }

        public void AddVariable(string name, float value)
        {
            DistributedValue.Create($"{_groupName}__{name}", value);
            _variables.Add(name);
        }

        public void AddVariable(string name, bool value)
        {
            DistributedValue.Create($"{_groupName}__{name}", value);
            _variables.Add(name);
        }

        public void Save()
        {
            
        }

        public void Load()
        {

        }
    }
}
