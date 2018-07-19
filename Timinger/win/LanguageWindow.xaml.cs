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
    /// Логика взаимодействия для LanguageWindow.xaml
    /// </summary>
    public partial class LanguageWindow : Window
    {
        MainWindow parent;
        public Dictionary<string, string> locals = new Dictionary<string, string>()
        {
            {"Русский","RUS" },
            {"English","ENG" }
        };
        public LanguageWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            parent = mainWindow;
            ListBoxChosenLanguage.ItemsSource = locals;
            ListBoxChosenLanguage.DisplayMemberPath = "Key";
            ListBoxChosenLanguage.SelectedIndex = 0;
        }

        private void ListBoxChosenLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var local = (KeyValuePair<string,string>)ListBoxChosenLanguage.SelectedItem;
            ViewModel.GetInstance().SwitchLanguage(local.Value);
            parent.InitColumns();
        }

        private void ButtonAccept_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
