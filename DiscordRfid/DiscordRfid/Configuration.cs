using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace DiscordRfid
{
    public class Configuration : INotifyPropertyChanged
    {
        public static string FileName = "config.json";

        private static Configuration ConfigurationSingleton { get; set; }

        protected Configuration() { }

        public event PropertyChangedEventHandler PropertyChanged;

        public static bool Exists => File.Exists(FileName);

        object fileLock = new object();

        private string _token { get; set; }
        public string Token
        {
            get => _token;
            set
            {
                if (value != _token)
                {
                    _token = value;
                    Save();
                    OnPropertyChanged();
                }
            }
        }

        public static Configuration Load()
        {
            if(ConfigurationSingleton == null)
            {
                if (!Exists)
                {
                    ConfigurationSingleton = new Configuration();
                    ConfigurationSingleton.Save();
                }
                else
                {
                    ConfigurationSingleton = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(FileName));
                }
            }

            return ConfigurationSingleton;
        }

        private void Save()
        {
            lock(fileLock)
            {
                File.WriteAllText(FileName, JsonConvert.SerializeObject(ConfigurationSingleton, Formatting.Indented));
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
