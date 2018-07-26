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
using System.Windows.Interop;
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
        IntPtr Hwnd = IntPtr.Zero;
        HwndSource source;

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
       

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                using (FileStream stream = new FileStream("alarm.mp3", FileMode.Open))
                {
                    stream.Close();
                }
            }
            catch (Exception)
            {
                using (Stream resstream = Application.GetResourceStream(new Uri("win/res/alarm.mp3", UriKind.Relative)).Stream)
                {
                    using (FileStream fstream = new FileStream("alarm.mp3", FileMode.OpenOrCreate))
                    {
                        while (resstream.Position < resstream.Length)
                        {
                            fstream.WriteByte((byte)resstream.ReadByte());
                        }
                    }

                }
            }
            viewModel = ViewModel.GetInstance();
            viewModel.ShowMessageDialog += (str)=>  MessageBox.Show(str);
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
            Hwnd = new WindowInteropHelper(this).Handle;
            source = HwndSource.FromHwnd(Hwnd);
            source.AddHook(MsgListener);
            KeyRegister.hWnd = Hwnd;
            viewModel.Alarm.HotKey = Config.GetInstance().TimerHotKey;
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
                viewModel.Alarm.ClosePlayer();
                KeyRegister.UnregisterKey(Hwnd);
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
            DefaultArmyTypeBox.SelectedIndex = 2;
        }
        private void AddAttackToGrid(object sender, RoutedEventArgs e)
        {
            var x = (string)DefaultArmyTypeBox.SelectedItem;
            viewModel.Target?.Attacks.Add(new Attack($"Attack {viewModel.Target.Attacks.Count+1}", 0, x));
            if (viewModel.Alarm.AttacksFromAttacks == true)
                viewModel.Alarm.SetTarget(viewModel.Target);
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
        private void ShowOptionsWindow(object sender, RoutedEventArgs e)
        {
            OptionsWindow window = new OptionsWindow(viewModel);
            window.Owner = this;
            window.Closed += (object obj, EventArgs args) => this.IsEnabled = true;
            this.IsEnabled = false;
            window.Show();
        }
        private void ShowEditTimeWindow(object sender, RoutedEventArgs e)
        {
            EditTimeWindow window = new EditTimeWindow(viewModel,viewModel.Alarm.NextAttack);
            window.Owner = this;
            window.Closed += (object obj, EventArgs args) => this.IsEnabled = true;
            this.IsEnabled = false;
            window.Show();
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

        private void SwitchToRUS(object sender, RoutedEventArgs e)
        {
            viewModel.SwitchLanguage("RUS");
            InitColumns();
            //DefaultArmyTypeBox.SelectedIndex = 2;
        }
        private void SwitchToENG(object sender, RoutedEventArgs e)
        {
            viewModel.SwitchLanguage("ENG");
            InitColumns();
            //DefaultArmyTypeBox.SelectedIndex = 2;
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

        private async void StartAlarm(object sender, RoutedEventArgs e)
        {
            if (viewModel.CountThreadStarted == false)
            {
                if (viewModel.Alarm.Started == false)
                {
                    if (viewModel.Alarm.Target == null || viewModel.Alarm.AttackList == null || viewModel.Alarm.AttackList.Count < 2)
                        return;
                    if(sender is MainWindow)
                    {
                        KeyRegister.mouse_event(KeyRegister.MOUSE_BUTTONDOWN, 0, 0, 0, IntPtr.Zero);
                        Thread.Sleep(50);
                        KeyRegister.mouse_event(KeyRegister.MOUSE_BUTTONUP, 0, 0, 0, IntPtr.Zero);
                    }
                    viewModel.Alarm.Started = true;
                    viewModel.Alarm.Player.Position = viewModel.Alarm.Player.NaturalDuration.TimeSpan;
                    Progress<bool> progress = new Progress<bool>((x) =>
                    {
                            viewModel.Alarm.PlaySound();
                    }
                    );
                    await Task.Factory.StartNew(() => { viewModel.Alarm.Start(progress); }, TaskCreationOptions.LongRunning);
                    viewModel.Alarm.Player.Stop();
                }
                else
                {
                    viewModel.Alarm.Started = false;
                }
            }
        }
        
        private void NoResultDialog()
        {
            MessageBox.Show(Timinger.Language.NoVariants);
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
        private IntPtr MsgListener(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    var l = lParam.ToInt32();
                    var w = wParam.ToInt32();
                    int key = KeyRegister.KeyValue;
                    int value = KeyRegister.VirtualKeyCodes[key];
                    
                    if (l == key || l == value || w == key || w == value)
                    {
                        StartAlarm(this, null);
                    }
                    break;
                default:
                    break;
            }

            return IntPtr.Zero;
        }
        private void AttacksGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var a = viewModel?.Target?.Attacks?.Count ?? 0;
            var b = viewModel?.Alarm?.AttackList?.Count ?? 0;
            if (viewModel.Alarm.AttacksFromAttacks && a != b)
                viewModel.Alarm.SetTarget(viewModel.Target);
        }
        private void TEMPChangeTime(object sender, RoutedEventArgs e)
        {
            var b = (Button)sender;
            if (b.Content.ToString() == "+")
                viewModel.Alarm.UnsafeNextTime = viewModel.Alarm.UnsafeNextTime.AddSeconds(1);
            if (b.Content.ToString() == "-")
                viewModel.Alarm.UnsafeNextTime.Subtract(TimeSpan.FromSeconds(1));
        }
    }   
}
