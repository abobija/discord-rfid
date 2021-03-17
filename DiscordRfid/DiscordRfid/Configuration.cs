using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace DiscordRfid
{
    public class Configuration : INotifyPropertyChanged
    {
        public static string FileName = "config.json";

        protected static Configuration Singletone { get; set; }

        protected Configuration() { }

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
                        Singletone = new Configuration();
                        Singletone.Save();
                    }
                    else
                    {
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
                File.WriteAllText(FileName, JsonConvert.SerializeObject(Singletone, Formatting.Indented));
            }
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            if(! PropertyChangeNotifyEnabled)
            {
                return;
            }

            Save();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
