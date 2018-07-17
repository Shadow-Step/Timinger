using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timinger
{
    [Serializable]
    public class Target
    {
        public string Name { get; set; }
        public ObservableCollection<Attack> attacks = new ObservableCollection<Attack>();
        public List<Attack> Best { get; set; }
        public Target(string name)
        {
            Name = name;
        }
    }
}
