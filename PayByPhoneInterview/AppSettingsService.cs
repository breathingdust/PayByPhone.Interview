using System.Configuration;

namespace PayByPhoneInterview
{
    public interface IAppSettingsService
    {
        string Get(string key);
    }

    public class AppSettingsService : IAppSettingsService
    {
         public string Get(string key)
         {
             return ConfigurationManager.AppSettings[key];
         }
    }
}