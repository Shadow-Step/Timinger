using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timinger
{
    class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Config Config { get; set; }

        private bool ruslang;
        public bool RusLang
        {
            get { return this.ruslang; }
            set { this.ruslang = value;INotifyPropertyChanged("RusLang"); }

        }
        private bool englang;
        public bool EngLang
        {
            get { return this.englang; }
            set { this.englang = value; INotifyPropertyChanged("EngLang"); }

        }

        private Language language;
        public Language Language
        {
            get { return language; }
            set
            {
                language = value;
                INotifyPropertyChanged("Language");
            }
        }

        private string timpath;
        public string TimPath
        {
            get { return timpath; }
            set { timpath = value; Config.LastProjectPath = timpath; INotifyPropertyChanged("TimPath"); }
        }
        
        public ViewModel()
        {
            Config = new Config();
            Config.LoadFromFile();
            Language = new Language();
            //switch (config.Language)
            //{
            //    case "RUS":
            //        RusLang = true;
            //        EngLang = false;
            //        break;
            //    case "ENG":
            //        RusLang = false;
            //        EngLang = true;
            //        break;
            //}
        }

        void INotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public void SwitchLanguage(string local)
        {
            if(local != Language.CurrentLanguage)
            {
                Language = new Language(local);
                Config.Language = local;
                switch (Config.Language)
                {
                    case "RUS":
                        RusLang = true;
                        EngLang = false;
                        break;
                    case "ENG":
                        RusLang = false;
                        EngLang = true;
                        break;
                }
            }
            
        }
        public void FindBest()
        {

        }
    }
}
