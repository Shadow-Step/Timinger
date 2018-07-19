using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Timinger
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel viewModel;
        Thread count_thread = null;

        public List<double> speed_list = new List<double>()
        {
            1.00,1.25,1.50,1.75,2.00,2.25,3.00,4.50,6.00
        };
        public static List<string> speed_str = new List<string>()
        {
            "0(0%)","1(25%)","2(50%)","3(75%)","4(100%)","5(125%)","6(200%)","7(350%)","8(500%)"
        };
        public double army_speed = 0;
        public double cap_speed = 0;
        public Timinger.ArmyType default_type = Timinger.ArmyType.Unknown;
        public string DefaultType
        {
            get
            {
                foreach (var item in Timinger.Language.type_dict)
                {
                    if (item.Value == default_type)
                        return item.Key;
                }
                return "Error";
            }
            set
            {
                this.default_type = Timinger.Language.type_dict[value];
            }
        }
        //Delegate
        public delegate bool Action(Attack attack);
       
        public double res = 0;
        
        public static List<Attack> count_attacks = new List<Attack>();
        

        public MainWindow()
        {
            InitializeComponent();
            viewModel = ViewModel.GetInstance();
            viewModel.ShowMessageDialog += (str)=> MessageBox.Show(str);
            
            this.DataContext = viewModel;
            
            if (viewModel.Config.LastProjectPath != null)
            {
                try
                {
                    LoadProject(viewModel.Config.LastProjectPath);
                }
                catch (Exception)
                {

                    viewModel.TimPath = null;
                }
            }
            InitColumns();
            DefaultArmyTypeBox.SelectedIndex = 2;
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var result = MessageBox.Show(Timinger.Language.ExitMessage,Timinger.Language.Exit, MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
                e.Cancel = true;
            else
            {
                count_thread?.Abort();
                count_thread?.Join();
                viewModel.Config.SaveToFile();
            }
            
        }
       
        private void Count(object sender, RoutedEventArgs e)
        {
            if(count_thread == null || count_thread.IsAlive == false)
            {
                count_thread = new Thread(viewModel.FindBest);
                count_thread.Start();
            }
            else
            {
                count_thread.Abort();
                count_thread.Join();
            }
        }

        private void ArmySpeedBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            army_speed = speed_list[ArmySpeedBox.SelectedIndex];
        }
        private void CapSpeedBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cap_speed = speed_list[CapSpeedBox.SelectedIndex];
        }
        private void AddTarget(object sender, RoutedEventArgs e)
        {
            viewModel.Targets.Add(new Target($"Target {viewModel.Targets.Count+1}"));
            if (TargetGrid.Items.Count == 1)
                TargetGrid.SelectedIndex = 0;
        }
        private void AddAttackToGrid(object sender, RoutedEventArgs e)
        {
            var x = (string)DefaultArmyTypeBox.SelectedItem;
            viewModel.Target?.Attacks.Add(new Attack($"Attack {viewModel.Target.Attacks.Count+1}", 0, x));
        }
        private void DeleteAttackFromGrid(object sender, RoutedEventArgs e)
        {
            var x = AttacksGrid.SelectedIndex;
            if (x != -1)
            {
                viewModel.Target.Attacks.RemoveAt(x);
                if (x > 0)
                    AttacksGrid.SelectedIndex = x - 1;
            }
        }

        private void ShowHelpMenu(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow(viewModel.Language);
            helpWindow.Owner = this;
            helpWindow.Closed += (object obj, EventArgs a ) => this.IsEnabled = true;
            this.IsEnabled = false;
            helpWindow.Show();
        }
        private void ShowDonateMenu(object sender, RoutedEventArgs e)
        {
            DonateWindow donateWindow = new DonateWindow(viewModel.Language);
            donateWindow.Owner = this;
            donateWindow.Closed += (object obj, EventArgs a) => this.IsEnabled = true;
            this.IsEnabled = false;
            donateWindow.Show();
        }
        private void ShowAboutWindow(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow(viewModel.Language);
            window.Owner = this;
            window.Closed += (object obj,EventArgs args) =>this.IsEnabled = true;
            this.IsEnabled = false;
            window.Show();
        }

        private void SwitchToRUS(object sender, RoutedEventArgs e)
        {
            viewModel.SwitchLanguage("RUS");
            InitColumns();
            DefaultArmyTypeBox.SelectedIndex = 2;
        }
        private void SwitchToENG(object sender, RoutedEventArgs e)
        {
            viewModel.SwitchLanguage("ENG");
            InitColumns();
            DefaultArmyTypeBox.SelectedIndex = 2;
        }
        public void InitColumns()
        {
            ColumnArmyType.Header = Timinger.Language.Type;
            ColumnArmyType2.Header = Timinger.Language.Type;
            ColumnCard.Header = Timinger.Language.Card;
            ColumnName.Header = Timinger.Language.Name;
            ColumnName2.Header = Timinger.Language.Name;
            ColumnTime.Header = Timinger.Language.Time;
            ColumnTime2.Header = Timinger.Language.Time;
            ColumnActive.Header = Timinger.Language.Active;
        }

        private void TestButton(object sender, RoutedEventArgs e)
        {
            //Temp temp = new Temp();
            //temp.Show();
            Random rand = new Random();
            viewModel.SwitchLanguage("ENG");
        }

        //Edit later
        private void NoResultDialog()
        {
            MessageBox.Show(Timinger.Language.NoVariants);
        }
        private void ShowCopyScreen(object sender, RoutedEventArgs e)
        {
            if (viewModel.Target != null)
            {
                var x = (Button)e.Source;
                if (x.Name == "ButtonCopyAttack")
                {
                    CopyWindow copyWindow = new CopyWindow(viewModel.Language, viewModel.Target.Name, viewModel.Target.Attacks.ToList());
                    copyWindow.Owner = this;
                    copyWindow.Closed += (object obj, EventArgs a) => this.IsEnabled = true;
                    this.IsEnabled = false;
                    copyWindow.Show();
                }
                else
                {
                    if (viewModel.Target.Best != null)
                    {
                        CopyWindow copyWindow = new CopyWindow(viewModel.Language, viewModel.Target.Name, viewModel.Target.Best.ToList());
                        copyWindow.Owner = this;
                        copyWindow.Closed += (object obj, EventArgs a) => this.IsEnabled = true;
                        this.IsEnabled = false;
                        copyWindow.Show();
                    }
                }
            }
        }
        
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CreateNewFile(object sender, RoutedEventArgs e)
        {
            if(viewModel.Targets.Count != 0)
            {
                if (MessageBox.Show(Timinger.Language.CreateNewFile, Timinger.Language.New, MessageBoxButton.YesNo) == MessageBoxResult.No)
                    return;
            }
            viewModel.Targets.Clear();
            viewModel.Target = null;
            viewModel.TimPath = null;

        }
        private void SaveFile(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(viewModel.TimPath))
            {
                BinaryFormatter binary = new BinaryFormatter();
                using (FileStream stream = new FileStream(viewModel.TimPath, FileMode.Open))
                {
                    binary.Serialize(stream, viewModel.Targets);
                }
            }
            else
            {
                SaveAsFile(sender,e);
            }
            
        }
        private void SaveAsFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.RestoreDirectory = true;
            saveFile.Filter = "Timinger file (*.tim)|*tim";
            saveFile.DefaultExt = ".tim";
            if ((bool)saveFile.ShowDialog())
            {
                viewModel.TimPath = saveFile.FileName;
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(viewModel.TimPath, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(stream, viewModel.Targets);
                }
            }
        }
        private void LoadFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Timinger file (*.tim)|*tim";
            if((bool)fileDialog.ShowDialog())
            {
                LoadProject(fileDialog.FileName);
            }
        }
        private void LoadProject(string path)
        {
            viewModel.TimPath = path;
            BinaryFormatter binary = new BinaryFormatter();
            using (FileStream stream = new FileStream(viewModel.TimPath, FileMode.Open))
            {
                viewModel.Targets = (ObservableCollection<Target>)binary.Deserialize(stream);
            }
        }
        private void LockUnlockView(bool value)
        {
            GroupBoxTargets.IsEnabled = value;
            GroupBoxAttacks.IsEnabled = value;
            UnsafeSettings.IsEnabled = value;

        }

        private void UnsafeSettingsOn(object sender, RoutedEventArgs e)
        {
            if(viewModel.UnsafeMode == true)
            {
                MessageBox.Show(Timinger.Language.UnsafeWarning);
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Config.GetInstance().FirstRun == true)
            {
                LanguageWindow window = new LanguageWindow(this);
                
                window.Owner = this;
                window.Closed += (object obj, EventArgs a) =>
                {
                    this.IsEnabled = true;
                };
                this.IsEnabled = false;
                window.Show();
                
            }
        }
    }   
}
