using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VKGroupHelperSDK.Domain;

namespace UI
{
    public class Settings
    {
        /// <summary>
        /// Функция получения данных настроек приложения из файла
        /// </summary>
        /// <returns></returns>
        public static Settings GetSettings()
        {
            Settings settings = null;
            string filename = Globals.SettingsFile;

            //проверка наличия файла
            if (File.Exists(filename))
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    XmlSerializer xser = new XmlSerializer(typeof(Settings));
                    settings = (Settings)xser.Deserialize(fs);
                    fs.Close();
                }
            }
            else
            {
                settings = new Settings();
            }

            return settings;
        }

        /// <summary>
        /// Процедура сохранения настроек в файл
        /// </summary>
        public void Save()
        {
            string filename = Globals.SettingsFile;

            if (File.Exists(filename)) File.Delete(filename);


            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                XmlSerializer xser = new XmlSerializer(typeof(Settings));
                xser.Serialize(fs, this);
                fs.Close();
            }
        }

        public void SetUsername(string text)
        {
            Username = Crypter.Encrypt(text);
        }
        public string GetUsername()
        {
            string res;

            try
            {
                res = Crypter.Decrypt(Username);
            }
            catch
            {
                res = Username;
            }

            return res;
        }

        public string GetPassword()
        {
            string res;

            try
            {
                res = Crypter.Decrypt(Password);
            }
            catch
            {
                res = Password;
            }

            return res;
        }

        public void SetPassword(string text)
        {
            Password = Crypter.Encrypt(text);
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public long GroupId { get; set; }

        public string ContentPath { get; set; }

        public int TotalPosts { get; set; }

        public int MaxPostOnDay { get; set; }

        public int PostStep { get; set; }

        public bool LoadPictures { get; set; }

        public bool LoadVideo { get; set; }

        public bool ThroughoutTheDay { get; set; }

        public bool DeleteAfterLoad { get; set; }

        public string Hashtags { get; set; }

        public int TimeMin { get; set; }

        public int TimeMax { get; set; }

        // Опрос
        public Poll Poll { get; set; } = new Poll();

        // Геопозиция
        public bool PlaceGeoPosition { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double LocationStep { get; set; }

        public int SquareWidth { get; set; }
    }
}

