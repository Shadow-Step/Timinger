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

        public Config config;

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
            set { timpath = value; INotifyPropertyChanged("TimPath"); }
        }
        
        public ViewModel()
        {
            config = new Config();
            config.LoadFromFile();
            Language = new Language(config.Language);
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
                config.Language = local;
            }
            
        }
    }
}
