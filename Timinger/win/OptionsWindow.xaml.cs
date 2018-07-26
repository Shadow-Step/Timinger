using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window , INotifyPropertyChanged
    {
        double Volume;
        bool DetailTime;
        double WhenDetail;
        bool AutoClick;
        double AlarmTime;
        string HotKey;

        ViewModel viewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        public OptionsWindow(ViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = viewModel;
            Volume = viewModel.Alarm.Volume;
            DetailTime = viewModel.Alarm.DetailTime;
            WhenDetail = viewModel.Alarm.WhenDetail;
            AutoClick = viewModel.Alarm.AutoClick;
            AlarmTime = viewModel.Alarm.AlarmTime;
            HotKey = viewModel.Alarm.HotKey;
        }
        private async void PlaySoundButton(object sender, RoutedEventArgs e)
        {
            if (SoundPlayed.IsChecked == false)
            {
                SoundPlayed.IsChecked = true;
                viewModel.Alarm.PlaySound();
            }
            else
            {
                SoundPlayed.IsChecked = false;
                viewModel.Alarm.Player.Stop();
                viewModel.Alarm.Player.Position = new TimeSpan();
            }
                
        }
        void INotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        void Accept(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        void Cancel(object sender, RoutedEventArgs e)
        {
            viewModel.Alarm.HotKey = HotKey;
            viewModel.Alarm.AlarmTime = AlarmTime;
            viewModel.Alarm.DetailTime = DetailTime;
            viewModel.Alarm.WhenDetail = WhenDetail;
            viewModel.Alarm.AutoClick = AutoClick;
            viewModel.Alarm.Volume = Volume;
            this.Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(sender is TextBox box && box.Text.ToString().ToLower() == "shadowstep")
            {
                SecretWindow window = new SecretWindow(viewModel);
                window.Owner = this;
                window.Closed += (object obj, EventArgs args) => this.IsEnabled = true;
                this.IsEnabled = false;
                window.Show();
            }
        }
    }
}
