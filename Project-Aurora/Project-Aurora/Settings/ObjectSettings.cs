﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Settings
{
    public class ObjectSettings<T>
    {
        protected string SettingsSavePath { get; set; }
        public T Settings { get; protected set; }

        protected void SaveSettings()
        {
            this.SaveSettings(typeof(T));
        }

        protected void SaveSettings(Type settingsType)
        {
            if (Settings == null)
                Settings = (T)Activator.CreateInstance(settingsType);

            File.WriteAllText(SettingsSavePath, JsonConvert.SerializeObject(Settings, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented }));
        }

        protected void LoadSettings()
        {
            this.LoadSettings(typeof(T));
        }

        protected virtual void LoadSettings(Type settingsType)
        {
            if (File.Exists(SettingsSavePath))
            {
                try
                {
                    Settings = (T)JsonConvert.DeserializeObject(File.ReadAllText(SettingsSavePath), settingsType, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
                }
                catch (Exception exc)
                {
                    Global.logger.LogLine($"Exception occured while loading \"{this.GetType().Name}\" Settings.\nException:" + exc, Logging_Level.Error);
                    SaveSettings(settingsType);
                }
            }
            else
                SaveSettings(settingsType);
        }
    }
}
