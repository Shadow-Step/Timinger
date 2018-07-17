using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Timinger
{
    /// <summary>
    /// Логика взаимодействия для DonateWindow.xaml
    /// </summary>
    public partial class DonateWindow : Window
    {
        public DonateWindow(Timinger.Language language)
        {
            InitializeComponent();
            this.DataContext = language;
            DonateTextBox.Document = (FlowDocument)this.FindResource(Timinger.Language.topics[Timinger.Language.Donate]);
        }
    }
}
