using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Newtonsoft.Json;

namespace AOSharp
{
    public class Plugin : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public string Path { get; set; }

        [JsonIgnore]
        public bool _isEnabled;

        [JsonIgnore]
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
