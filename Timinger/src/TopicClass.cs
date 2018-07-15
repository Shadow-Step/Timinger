using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Timinger
{
    class TopicClass
    {
        public string Name { get; set; }
        public FlowDocument Content { get; set; }
        public TopicClass(string name, FlowDocument content)
        {
            Name = name;
            Content = content;
        }
    }
}
