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
        private Config() { }

        private static Config cfg;
        private static string ConfigPath = "config.cfg";
        Dictionary<string, string> config = new Dictionary<string, string>()
        {
            {"Language","RUS"},
            {"Delta","3.00"},
            {"LastProjectPath",""},
            {"TimerHotKey","F2"},
            {"TimerVolume","50"},
            {"TimerSecondsToSignal","60"},
            {"TimerDetailTime","10"},
            {"TimerSecondsToDetail","10"},
            {"TimerAutoClick","1" },
            {"TimerAttacksFromResultTable","1"}
        };
        public bool FirstRun { get; set; } = true;

        public string   Language
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
        public string   LastProjectPath
        {
            get { return config["LastProjectPath"]; }
            set { config["LastProjectPath"] = value; }
        }
        private double  delta = 0;
        public double   Delta
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
        public string   TimerHotKey
        {
            get { return config["TimerHotKey"]; }
            set { config["TimerHotKey"] = value; }
        }
        public double   TimerVolume
        {
            get
            {
                var x = double.Parse(config["TimerVolume"]) / 100;
                if (x > 1 || x < 0)
                    return 0.5;
                else
                    return x;
            }
            set
            {
                config["TimerVolume"] = ((int)(value*100)).ToString();
            }
        }
        public double   TimerSecondsToSignal
        {
            get
            {
                return double.Parse(config["TimerSecondsToSignal"]);
            }
            set
            {
                config["TimerSecondsToSignal"] = value.ToString();
            }
        }
        public double   TimerSecondsToDetail
        {
            get
            {
                return double.Parse(config["TimerSecondsToDetail"]);
            }
            set
            {
                config["TimerSecondsToDetail"] = value.ToString();
            }
        }
        public bool     TimerAutoClick
        {
            get
            {
                var x = int.Parse(config["TimerAutoClick"]);
                if (x == 0)
                    return false;
                else
                    return true;
            }
            set
            {
                if (value == true)
                    config["TimerAutoClick"] = 1.ToString();
                else
                    config["TimerAutoClick"] = 0.ToString();
            }
        }
        public bool     TimerAttacksFromResultTable
        {
            get
            {
                var x = int.Parse(config["TimerAttacksFromResultTable"]);
                if (x == 0)
                    return false;
                else
                    return true;
            }
            set
            {
                if (value == true)
                    config["TimerAttacksFromResultTable"] = 1.ToString();
                else
                    config["TimerAttacksFromResultTable"] = 0.ToString();
            }
        }
        public bool     TimerDetailTime
        {
            get
            {
                var x = int.Parse(config["TimerDetailTime"]);
                if (x == 0)
                    return false;
                else
                    return true;
            }
            set
            {
                if (value == true)
                    config["TimerDetailTime"] = 1.ToString();
                else
                    config["TimerDetailTime"] = 0.ToString();
            }
        }

        public static Config GetInstance()
        {
            if(cfg == null)
            {
                cfg = new Config();
            }
            return cfg;
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
                    FirstRun = false;
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
        public static bool CheckExist()
        {
            try
            {
                using (StreamReader stream = new StreamReader(ConfigPath))
                {
                    stream.Close();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        private string ParseDelta()
        {
            return config["Delta"].Replace('.', ',');
        }
    }
}
