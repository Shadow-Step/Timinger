using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timinger
{
    [Serializable]
    public class Target : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Attack> best;

        public string Name { get; set; }
        public ObservableCollection<Attack> Attacks { get; set; }
        public ObservableCollection<Attack> Best
        {
            get { return this.best; }
            set
            {
                this.best = value;
                INotifyPropertyChanged("Best");
                INotifyPropertyChanged("TotalTime");
            }
        }
        public string TotalTime
        {
            get
            {
                if (Best != null)
                    return Language.TotalTimeToAttack + ": " + strfun.SecondsToTimeString(Best.First().Time - Best.Last().Time);
                else
                    return null;
            }
        }
        public Target(string name)
        {
            Name = name;
            Attacks = new ObservableCollection<Attack>();
        }

        void INotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
