﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using Microsoft.Win32;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using AOSharp.Data;
using AOSharp.Models;

namespace AOSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        //private ObservableCollection<Profile> _profiles;
        //public ObservableCollection<Profile> Profiles { get { return _profiles; } }

        //private ObservableCollection<Assembly> _assemblies;
        //public ObservableCollection<Assembly> Assemblies { get { return _assemblies; } }

        public Config Config;

        private Profile _activeProfile;

        public Profile ActiveProfile
        {
            get { return _activeProfile; }
            set
            {
                _activeProfile = value;
                OnPropertyChanged("ActiveProfile");
            }
        }

        private bool _hasProfileSelected;

        public bool HasProfileSelected
        {
            get { return _hasProfileSelected; }
            set
            {
                _hasProfileSelected = value;
                OnPropertyChanged("HasProfileSelected");
            }
        }

        public MainWindow()
        {
            //_profiles = GetProfiles();
            //_assemblies = GetAssemblies();

            Config = Config.Load(Directories.ConfigFilePath);

            this.DataContext = this;

            InitializeComponent();

            PluginsDataGrid.DataContext = Config;
            ProfileListBox.DataContext = new ProfilesModel(Config);
        }

        private async void ShowAddPluginDialog(object sender, RoutedEventArgs e)
        {
            BaseMetroDialog dialog = (BaseMetroDialog)this.Resources["AddPluginDialog"];
            dialog.DataContext = new AddAssemblyModel();
            await this.ShowMetroDialogAsync(dialog);
            await dialog.WaitUntilUnloadedAsync();
        }

        private async void CloseAddPluginDialog(object sender, RoutedEventArgs e)
        {
            BaseMetroDialog dialog = (BaseMetroDialog)this.Resources["AddPluginDialog"];
            await this.HideMetroDialogAsync(dialog);
        }

        private void LocalPathDialogButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog() { Filter = "DLL Files (*.dll)|*.dll" };

            if (dialog.ShowDialog() == true)
            {
                BaseMetroDialog addPluginDialog = (BaseMetroDialog)this.Resources["AddPluginDialog"];
                ((AddAssemblyModel)addPluginDialog.DataContext).DllPath = dialog.FileName;
            }
        }

        private async void AddPluginButton_Click(object sender, RoutedEventArgs e)
        {
            BaseMetroDialog addPluginDialog = (BaseMetroDialog)this.Resources["AddPluginDialog"];
            AddAssemblyModel dataModel = (AddAssemblyModel)addPluginDialog.DataContext;

            if (string.IsNullOrEmpty(dataModel.DllPath))
            {
                await this.ShowMessageAsync("Error", "No plugin path specified.");
                return;
            }

            //Should never happen but just in case some idiot deletes a plugin after selecting it..
            if (!File.Exists(dataModel.DllPath))
                return;

            FileVersionInfo fileInfo = FileVersionInfo.GetVersionInfo(dataModel.DllPath);

            Config.Plugins.Add(Utils.HashFromFile(dataModel.DllPath), new Plugin()
            {
                Name = fileInfo.ProductName,
                Version = fileInfo.FileVersion,
                Path = dataModel.DllPath
            });
            Config.Save();

            await this.HideMetroDialogAsync(addPluginDialog);
        }

        private void PluginEnabledCheckBox_Click(object sender, RoutedEventArgs e)
        {
            Profile selectedProfile = (Profile)ProfileListBox.SelectedItem;

            if (selectedProfile == null)
                return;

            KeyValuePair<string, Plugin> plugin = (KeyValuePair<string, Plugin>)PluginsDataGrid.SelectedItem;

            if (plugin.Value.IsEnabled)
                selectedProfile.EnabledPlugins.Add(plugin.Key);
            else
                selectedProfile.EnabledPlugins.Remove(plugin.Key);

            if (!Config.Profiles.Contains(selectedProfile))
                Config.Profiles.Add(selectedProfile);

            Config.Save();
        }

        private void ProfileListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Profile profile = (Profile)ProfileListBox.SelectedItem;

            HasProfileSelected = profile != null;
            ActiveProfile = profile;

            if(profile == null)
            {
                foreach (Plugin plugin in Config.Plugins.Values)
                    plugin.IsEnabled = false;

                PluginsDataGrid.IsEnabled = false;
            }
            else if(!PluginsDataGrid.IsEnabled)
            {
                PluginsDataGrid.IsEnabled = true;
            }

            foreach (KeyValuePair<string, Plugin> plugin in Config.Plugins)
                plugin.Value.IsEnabled = profile.EnabledPlugins.Contains(plugin.Key);
        }

        private async void InjectButton_Clicked(object sender, RoutedEventArgs e)
        {
            Profile profile = (Profile)ProfileListBox.SelectedItem;

            if (profile == null)
                return;

            IEnumerable<string> plugins = Config.Plugins.Where(x => profile.EnabledPlugins.Contains(x.Key)).Select(x => x.Value.Path);

            if(!plugins.Any())
            {
                await this.ShowMessageAsync("Error", "No plugins selected.");
                return;
            }

            if(profile.Inject(plugins))
            {
                PluginsDataGrid.IsEnabled = false;
            }
            else
            {
                await this.ShowMessageAsync("Error", "Failed to inject.");
            }
        }

        private void EjectButton_Clicked(object sender, RoutedEventArgs e)
        {
            Profile profile = (Profile)ProfileListBox.SelectedItem;

            if (profile == null)
                return;

            profile.Eject();

            PluginsDataGrid.IsEnabled = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private async void RemovePlugin(object sender, RoutedEventArgs e)
        {

            //Check if we are currently injected before doing anything
            if (!PluginsDataGrid.IsEnabled)
                return;
            
            //Get the currently selected profile
            Profile selectedProfile = (Profile)ProfileListBox.SelectedItem;
            if (selectedProfile == null)
                return;
            
            //Get the currently selected item from the DataGrid and check if it is currently Enabled and if so disable it before removing
            KeyValuePair<string, Plugin> plugin = (KeyValuePair<string, Plugin>)PluginsDataGrid.SelectedItem;
            if (plugin.Value.IsEnabled)
                plugin.Value.IsEnabled = false;

            selectedProfile.EnabledPlugins.Remove(plugin.Key);

            //Remove the plugin from the Config and save the updated config
            Config.Plugins.Remove(plugin.Key);
            PluginsDataGrid.DataContext = Config;
            Config.Save();
        }
    }

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public sealed class BoolToVisibilityConverter : IValueConverter
    {
        public Visibility TrueValue { get; set; }
        public Visibility FalseValue { get; set; }

        public BoolToVisibilityConverter()
        {
            // set defaults
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return null;
            return (bool)value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (Equals(value, TrueValue))
                return true;
            if (Equals(value, FalseValue))
                return false;
            return null;
        }
    }
}
