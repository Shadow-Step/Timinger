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
    /// Логика взаимодействия для EditTimeWindow.xaml
    /// </summary>
    public partial class EditTimeWindow : Window
    {
        ViewModel Model;
        Attack Attack;
        public EditTimeWindow(ViewModel model, Attack attack)
        {
            InitializeComponent();
            Model = model;
            this.DataContext = Model;
            Attack = attack;
            OriginalTime.Text = Attack.TimeStr;
            ChangedTime.Text = Attack.TimeStr;
        }
        private void Accept(object sender, RoutedEventArgs e)
        {
            var x = strfun.ParseTimeString(ChangedTime.Text);
            if(x < Attack.Time && Model.Alarm.NextAttack == Attack)
            {
                Model.Alarm.UnsafeNextTime = Model.Alarm.UnsafeNextTime.Subtract(TimeSpan.FromSeconds(Attack.Time - x));
                Attack.Time = x;
            }
            else if(x > Attack.Time && Model.Alarm.NextAttack == Attack)
            {
                Model.Alarm.UnsafeNextTime = Model.Alarm.UnsafeNextTime.AddSeconds(x - Attack.Time);
                Attack.Time = x;
            }
        }
    }
}
