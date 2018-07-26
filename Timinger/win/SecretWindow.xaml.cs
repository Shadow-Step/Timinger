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
    /// Логика взаимодействия для SecretWindow.xaml
    /// </summary>
    public partial class SecretWindow : Window
    {
        ViewModel viewModel;
        public SecretWindow(ViewModel model)
        {
            InitializeComponent();
            viewModel = model;
            this.DataContext = viewModel;
            SecretBox.Document = (FlowDocument)this.FindResource("SecretTopic");
            WarningBox.Document = (FlowDocument)this.FindResource("WarningTopic");
        }
    }
}
