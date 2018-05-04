using System;

namespace SendArchive.Settings
{
    /// <summary>
    /// Interface for describing the service with settings
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Name file settings
        /// </summary>
        string NameFileSettings { get; }

        /// <summary>
        /// Method of saving settings to a file
        /// </summary>
        /// <param name="settings">Settings</param>
        void SaveSettings(Settings settings);

        /// <summary>
        /// Method of loading settings from a file
        /// </summary>
        /// <param name="callback">Delegate to return settings</param>
        void LoadSettings(Action<Settings> callback);

        /// <summary>
        /// Method for obtaining default settings.
        /// If there is no file settings or you need to reset the settings
        /// </summary>
        /// <param name="callback">Delegate to return default settings</param>
        void GetDefaultSettings(Action<Settings> callback);
    }
}