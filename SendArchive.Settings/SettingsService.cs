using System;
using System.IO;
using System.Web.Script.Serialization;

namespace SendArchive.Settings
{
    /// <summary>
    /// The interface (ISettingsService) implementation class
    /// </summary>
    public class SettingsService : ISettingsService
    {
        private string _nameFilesSettings = "Settings.ini";
        public string NameFileSettings => _nameFilesSettings;

        #region Public Method

        /// <summary>
        /// Method of get default setting
        /// </summary>
        /// <param name="callback">Delegate fo return settings</param>
        public void GetDefaultSettings(Action<Settings> callback)
        {
            callback(new Settings()
            {
                Language = "ru"
            });
        }

        /// <summary>
        /// Method of load settings of a file. Serialization json string (file) to class
        /// </summary>
        /// <param name="callback">Delegate for return settings</param>
        public void LoadSettings(Action<Settings> callback)
        {
            Settings settings = null;

            // if the file exists, then read it and we will deserialize the resulting string into the class
            if (File.Exists(_nameFilesSettings))
            {
                try
                {
                    string set = File.ReadAllText(_nameFilesSettings);
                    var serializer = new JavaScriptSerializer();
                    settings = serializer.Deserialize<Settings>(set);
                }
                catch (Exception ex)
                {
                    //TODO if ever there is a logging
                    throw new Exception("Error");
                }

            }

            // if there is no file or there were read and convert errors, we get the default settings
            if (settings == null)
            {
                GetDefaultSettings((s) =>
                {
                    settings = s;
                });
            }

            callback(settings);
        }

        /// <summary>
        /// Method for saving settings. Serialize class setting to a string and write to a file
        /// </summary>
        /// <param name="settings"></param>
        public void SaveSettings(Settings settings)
        {
            try
            {
                var serializer = new JavaScriptSerializer();
                string set = serializer.Serialize(settings);
                File.WriteAllText(_nameFilesSettings, set);
            }
            catch (Exception ex)
            {
                //TODO if ever there is a logging
                throw new Exception("Error");
            }
        }
       
        #endregion Public Method
    }
}
