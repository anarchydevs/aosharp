using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace AOSharp.Core
{
    public class SettingsManager<T> where T : PluginSettings
    {
        private FileInfo _globalSettingsFile;
        private FileInfo _playerSettingsFile;
        private T _pluginSettings;
        private JsonSerializerSettings _globalSerializerSettings;
        private JsonSerializerSettings _playerSerializerSettings;
        private EventWaitHandle _globalSettingsfileWritingWaitHandle;

        public T Settings
        {
            get
            {
                return _pluginSettings;
            }
        }

        public SettingsManager(string globalSettingsPath, string playerSettingsPath)
        {
            _globalSerializerSettings = new JsonSerializerSettings() { ContractResolver = new PluginSettingsResolver(SettingScope.Global), NullValueHandling = NullValueHandling.Ignore, ObjectCreationHandling = ObjectCreationHandling.Replace };
            _playerSerializerSettings = new JsonSerializerSettings() { ContractResolver = new PluginSettingsResolver(SettingScope.Player), NullValueHandling = NullValueHandling.Ignore, ObjectCreationHandling = ObjectCreationHandling.Replace };
            _globalSettingsfileWritingWaitHandle = new EventWaitHandle(true, EventResetMode.AutoReset, "5be25fc8-b2ab-4174-9808-759346b6784b");
            _globalSettingsFile = new FileInfo(globalSettingsPath);
            _playerSettingsFile = new FileInfo(playerSettingsPath);

            _pluginSettings = Activator.CreateInstance<T>();

            LoadSettings();
            _pluginSettings.PropertyChanged += OnPropertyChanged;
        }
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveSettings();
        }

        public void SaveSettings()
        {
            if (_globalSettingsfileWritingWaitHandle.WaitOne())
            {
                try
                {
                    File.WriteAllText(_globalSettingsFile.FullName, JsonConvert.SerializeObject(_pluginSettings, Formatting.Indented, _globalSerializerSettings));
                }
                finally
                {
                    _globalSettingsfileWritingWaitHandle.Set();
                }
            }
            SaveGlobalSettings();
            SavePlayerSettings();
        }

        private void SaveGlobalSettings()
        {
            try
            {
                File.WriteAllText(_globalSettingsFile.FullName, JsonConvert.SerializeObject(_pluginSettings, Formatting.Indented, _globalSerializerSettings));
            }
            finally
            {
                _globalSettingsfileWritingWaitHandle.Set();
            }
        }

        private void SavePlayerSettings()
        {
            File.WriteAllText(_playerSettingsFile.FullName, JsonConvert.SerializeObject(_pluginSettings, Formatting.Indented, _playerSerializerSettings));
        }

        private void LoadSettings()
        {
            // Write default settings if they do not already exist
            if (!_globalSettingsFile.Exists)
            {
                SaveGlobalSettings();
            }

            if (!_playerSettingsFile.Exists)
            {
                SavePlayerSettings();
            }

            JObject globalSettings = JObject.Parse(File.ReadAllText(_globalSettingsFile.FullName));
            JObject playerSettings = JObject.Parse(File.ReadAllText(_playerSettingsFile.FullName));
            globalSettings.Merge(playerSettings);

            _pluginSettings = JsonConvert.DeserializeObject<T>(globalSettings.ToString(), new JsonSerializerSettings() { ObjectCreationHandling = ObjectCreationHandling.Replace});
        }
    }
    public abstract class PluginSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public enum SettingScope
    {
        Global,
        Player
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class PluginSettingAttribute : Attribute
    {
        public SettingScope Scope { get; private set; }
        public PluginSettingAttribute(SettingScope scope)
        {
            Scope = scope;
        }
    }

    public class PluginSettingsResolver : DefaultContractResolver
    {
        SettingScope _settingScope;
        public PluginSettingsResolver(SettingScope settingScope)
        {
            _settingScope = settingScope;
        }
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var allProperties = base.CreateProperties(type, memberSerialization);

            // filter out only the properties we want to serialize/deserialize
            var propertyNamesToSerialize = new List<string>();
            foreach (var property in type.GetProperties(~BindingFlags.FlattenHierarchy))
            {
                var pluginSettingAttribute = property.GetCustomAttribute<PluginSettingAttribute>();
                if (pluginSettingAttribute != null && pluginSettingAttribute.Scope == _settingScope)
                {
                    propertyNamesToSerialize.Add(property.Name);
                }
            }

            return allProperties.Where(p => propertyNamesToSerialize.Any(a => a == p.PropertyName)).ToList();
        }
    }
}
