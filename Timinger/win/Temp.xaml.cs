using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для Temp.xaml
    /// </summary>
    
    public enum TestEnum
    {
        first,
        second,
        third
    }

    public partial class Temp : Window
    {
        public static ObservableCollection<string> vs = new ObservableCollection<string>() { "first", "second", "third" };
        
        public Temp()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vs[0] = "первый";
            vs[1] = "второй";
            vs[2] = "третий"; 
        }
    }
    
}
