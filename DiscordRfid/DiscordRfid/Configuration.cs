using Newtonsoft.Json;
using Serilog;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace DiscordRfid
{
    public class Configuration : INotifyPropertyChanged
    {
        public static string FileName = "config.json";

        protected static Configuration Singletone { get; set; }

        protected Configuration()
        {
            Log.Debug("Configuration constructor");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected static bool Exists => File.Exists(FileName);
        protected bool PropertyChangeNotifyEnabled = false;
        private object fileLock = new object();

        private string _token { get; set; }
        public string Token
        {
            get => _token;
            set
            {
                if (value != _token)
                {
                    _token = value;
                    OnPropertyChanged();
                }
            }
        }

        public static Configuration Instance
        {
            get
            {
                if (Singletone == null)
                {
                    if (!Exists)
                    {
                        Log.Debug("Configuration not found. Creating new one.");
                        Singletone = new Configuration();
                        Singletone.Save();
                    }
                    else
                    {
                        Log.Debug("Configuration found");
                        Singletone = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(FileName));
                    }

                    Singletone.PropertyChangeNotifyEnabled = true;
                }

                return Singletone;
            }
        }

        private void Save()
        {
            lock(fileLock)
            {
                Log.Verbose("Saving configuration");
                File.WriteAllText(FileName, JsonConvert.SerializeObject(Singletone, Formatting.Indented));
            }
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            if(! PropertyChangeNotifyEnabled)
            {
                return;
            }

            Log.Verbose($"Configuration property \"{name}\" changed");
            Save();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
