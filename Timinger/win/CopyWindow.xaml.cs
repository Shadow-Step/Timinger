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
    /// Логика взаимодействия для CopyWindow.xaml
    /// </summary>
    public partial class CopyWindow : Window
    {
        string target_name;
        List<Attack> list;

        public CopyWindow(Timinger.Language language, string target_name, List<Attack> list)
        {
            this.target_name = target_name;
            this.list = list;
            InitializeComponent();
            this.DataContext = language;
            
            
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BuildText(sender, e);
        }

        public void BuildText(object sender, RoutedEventArgs e)
        {
            if (TargetCheckBox == null || AttackNameCheckBox == null || ArmyTypeCheckBox == null)
                return;
            StringBuilder builder = new StringBuilder();
            if ((bool)TargetCheckBox.IsChecked)
                builder.AppendLine($"{Timinger.Language.Target}: {target_name}");
            foreach (var item in list)
            {
                if ((bool)AttackNameCheckBox?.IsChecked)
                {
                    builder.Append(item.Name);
                    builder.Append(" - ");
                }
                builder.Append(item.TimeStr);
                if ((bool)ArmyTypeCheckBox.IsChecked)
                {
                    builder.Append(" - ");
                    builder.Append(item.ArmyTypeStr);
                }
                builder.AppendLine();
            }
            CopyBox.Text = builder.ToString();
        }
    }
}
