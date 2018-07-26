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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Timinger
{
    /// <summary>
    /// Логика взаимодействия для HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        private string selected;

        public HelpWindow(Timinger.Language language)
        {
            InitializeComponent();
            this.DataContext = language;
            if(Config.GetInstance().Language != "RUS")
            {
                TextBlockMechanical.Text = "Machine translation!";
            }
            TopicListBox.ItemsSource = Timinger.Language.topics;

            CommonTreeItem.ItemsSource = Timinger.Language.topics;
            TimerTreeItem.ItemsSource = Timinger.Language.TimerTopics;
        }

        private void TopicListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selected = ((KeyValuePair<string,string>)(TopicListBox.SelectedItem)).Value;
            if(selected != null)
            ContentBlock.Document = (FlowDocument)this.FindResource(selected);
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(sender is TreeView item && item.SelectedItem is KeyValuePair<string,string>)
            {
                var x = (KeyValuePair<string, string>)item.SelectedItem;
                ContentBlock.Document = (FlowDocument)this.FindResource(x.Value);
            }
        }
    }
}
