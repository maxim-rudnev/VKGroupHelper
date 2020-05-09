using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI
{
    public static class Globals
    {
        public static readonly string SettingsFile = "Settings.xml";

        public static Settings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = Settings.GetSettings();
                }

                return _settings;
            }
        }

        private static Settings _settings;
    }
}
