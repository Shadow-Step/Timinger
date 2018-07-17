using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timinger
{
    public class Config
    {
        public static string ConfigPath = "config.cfg";
        Dictionary<string, string> config = new Dictionary<string, string>()
        {
            {"Language","RUS"},
            {"Delta","3.00"},
            {"LastProjectPath",""},
        };

        public string Language
        {
            get
            {
                return config["Language"];
            }
            set
            {
                config["Language"] = value;
            }
        }
        public string LastProjectPath
        {
            get { return config["LastProjectPath"]; }
            set { config["LastProjectPath"] = value; }
        }

        private double delta = 0;
        public double Delta
        {
            get
            {
                if (delta == 0)
                    double.TryParse(ParseDelta(), out delta);
                return delta == 0 ? 3.00 : delta;
            }
            set
            {
                delta = value; config["Delta"] = value.ToString();
            }
        }
        

        public void LoadFromFile()
        {
            string data;
            try
            {
                using (StreamReader stream = new StreamReader(ConfigPath))
                {
                    while ((data = stream.ReadLine()) != null)
                    {
                        for (int i = 0; i < data.Length; i++)
                        {
                            if (data[i] == '=')
                            {
                                string key = data.Substring(0, i);
                                string value = data.Substring(i + 1);
                                if(config.ContainsKey(key))
                                config[key] = value;
                                break;
                            }
                        }
                    }
                    stream.Close();
                }
            }
            catch (Exception)
            {
                SaveToFile();
            }
           
        }
        public void SaveToFile()
        {
            using (StreamWriter writer = new StreamWriter(ConfigPath))
            {
                foreach (var item in config)
                {
                    writer.WriteLine(item.Key + "=" + item.Value);
                }
                writer.Close();
            }
        }
        
        private string ParseDelta()
        {
            return config["Delta"].Replace('.', ',');
        }
    }
}
