using System;
using System.IO;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using AOSharp.Core;

namespace AOSharp.Core
{
    public interface IAOPluginEntry
    {
        void Run(string pluginDir);
        void Teardown();
    }

    public class AOPluginEntry : IAOPluginEntry
    {
        internal Logger _logger;
        internal LoggingLevelSwitch _loggingLevelSwitch;
        internal FileInfo _logFilePath;
        internal string _logFormat = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {PluginName}: {Message:lj}{NewLine}{Exception}";

        public Logger Logger { get { return _logger; } }
        public DirectoryInfo PluginDataDirectory { get; private set; }
        public FileInfo GlobalSettingsFile => new FileInfo(Path.Combine(PluginDataDirectory.FullName, "GlobalSettings.json"));
        public FileInfo PlayerSettingsFile => new FileInfo(Path.Combine(PluginDataDirectory.FullName, DynelManager.LocalPlayer.Name, "PlayerSettings.json"));

        public SettingsManager<T> UseSettingsManager<T>() where T : PluginSettings
        {
            return new SettingsManager<T>(GlobalSettingsFile.FullName, PlayerSettingsFile.FullName);
        }

        public LogEventLevel LogEventLevel { 
            get
            {
                return _loggingLevelSwitch.MinimumLevel;
            }
            set
            {
                _loggingLevelSwitch.MinimumLevel = value;
            }
        }

        public void Init(string assemblyName)
        {
            PluginDataDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AOSharp", assemblyName));
            _logFilePath = new FileInfo(Path.Combine(PluginDataDirectory.FullName, "log.txt"));
            
            if (!PluginDataDirectory.Exists) {
                PluginDataDirectory.Create();
            }

            if (!_logFilePath.Directory.Exists) {
                _logFilePath.Directory.Create();
            }

            if (!GlobalSettingsFile.Directory.Exists) {
                GlobalSettingsFile.Directory.Create();
            }

            if (!PlayerSettingsFile.Directory.Exists) {
                PlayerSettingsFile.Directory.Create();
            }

            _loggingLevelSwitch= new LoggingLevelSwitch();
            _logger = new LoggerConfiguration()
                .Enrich.WithProperty("PluginName", assemblyName)
                .MinimumLevel.Verbose()
                .WriteTo.Debug(outputTemplate: _logFormat)
                .WriteTo.File(_logFilePath.FullName, levelSwitch: _loggingLevelSwitch, outputTemplate: _logFormat)
                .CreateLogger();
        }
        
        public virtual void Run(string pluginDir)
        {
        }

        public virtual void Teardown() 
        {
        }
    }
}
