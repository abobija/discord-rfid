using Newtonsoft.Json;
using Serilog;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace DiscordRfid.Services
{
    public class Configuration : INotifyPropertyChanged
    {
        public static string FileName = "config.json";

        public static int RecentPackagesLoadLimit = 20;

        #region Should not be changed
        public static string MasterRoleName = "RFIDM";
        public static string SlaveRoleName = "RFIDS";
        public static string ChannelName = "rfidx";
        #endregion

        protected Configuration()
        {
            Log.Debug("Configuration constructor");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected static bool Exists => File.Exists(FileName);
        protected bool PropertyChangeNotifyEnabled = false;
        private object fileLock = new object();

        private string _token;
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

        private void Save()
        {
            lock(fileLock)
            {
                Log.Verbose("Saving configuration");
                File.WriteAllText(FileName, JsonConvert.SerializeObject(_instance, Formatting.Indented));
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

        #region Singletone
        protected static Configuration _instance;

        public static Configuration Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (!Exists)
                    {
                        Log.Debug("Configuration not found. Creating new one.");
                        _instance = new Configuration();
                        _instance.Save();
                    }
                    else
                    {
                        Log.Debug("Configuration found");
                        _instance = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(FileName));
                    }

                    _instance.PropertyChangeNotifyEnabled = true;
                }

                return _instance;
            }
        }
        #endregion
    }
}
