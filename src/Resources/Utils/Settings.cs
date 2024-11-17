using Microsoft.Extensions.Configuration;

namespace SampleTest.Resources.Utils
{
    public class Settings
    {
        private static volatile Settings _instance;
        private static object _syncRoot = new object();
        private IConfiguration _configuration;

        private Settings() { }

        public static Settings Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new Settings();
                    }
                }

                return _instance;
            }
        }

        public static void Initialize(IConfiguration configuration)
        {
            Instance._configuration = configuration;
        }

        public string AppSettings(string value, string defaultValue = null)
        {
            if (_configuration.GetSection(value).Exists())
            {
                return _configuration.GetSection(value).Value;
            }
            else if (_configuration.GetSection(value.Replace(":", "__")).Exists())
            {
                return _configuration.GetSection(value.Replace(":", "__")).Value;
            }

            if (defaultValue == null)
                throw new Exception($"Não foi possível encontrar a chave {value} no arquivo de appsettings.json");

            return defaultValue;
        }

        public string GetEnvironment()
        {
            var chave = AppSettings("ENV", "");

            if (string.IsNullOrEmpty(chave))
                throw new Exception($"Não foi possível encontrar a chave ENV no arquivo de configuração");

            return chave;
        }
    }
}
