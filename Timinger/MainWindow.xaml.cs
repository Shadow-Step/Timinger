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
        ViewModel viewModel = new ViewModel();
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
        public List<List<Attack>> variants = new List<List<Attack>>();
        public static List<Attack> count_attacks = new List<Attack>();
        public static ObservableCollection<Target> targets = new ObservableCollection<Target>();
        public static Target target = null;

        public MainWindow()
        {
            InitializeComponent();
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
            //InitLocal(config.Language);
            
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var result = MessageBox.Show(Timinger.Language.ExitMessage,Timinger.Language.Exit, MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
                e.Cancel = true;
            viewModel.Config.SaveToFile();
        }
       
        private void Test(object sender, RoutedEventArgs e)
        {
            if(count_thread == null || count_thread.IsAlive == false)
            {
                count_thread = new Thread(new ParameterizedThreadStart(FindBest));
                count_thread.Start(new ThreadArgs(target.attacks.ToList(),0,0,0));
            }
                
        }
        private void FindBest(object args)
        {
            if (target == null || target.attacks.Count < 2 || CheckCaps() == false)
                return;

            List<Attack> attacks = CopyList(target.attacks.ToList());
            attacks.RemoveAll((x) => x.Active == false);
            if (attacks.Count < 2)
                return;
            variants.Clear();
            target.Best = null;

            if (UnlockUnsafeSettings.IsChecked == true)
            {
                if (ArmySpeedBox.SelectedIndex != speed_list.Count - 1 || CapSpeedBox.SelectedIndex != speed_list.Count - 1)
                {
                    foreach (var item in attacks)
                    {
                        switch (item.armytype)
                        {
                            case ArmyType.Captain:
                                item.Time *= speed_list[CapSpeedBox.SelectedIndex];
                                break;
                            case ArmyType.Army:
                                item.Time *= speed_list[ArmySpeedBox.SelectedIndex];
                                break;
                            case ArmyType.Unknown:
                                item.Time *= speed_list[ArmySpeedBox.SelectedIndex];
                                break;
                        }
                    }
                }
            }

            int card2 = 0;
            int card3 = 0;
            int card5 = 0;

            variants.Add(attacks);
            Parse(attacks, (x) => x.EnableCap(viewModel.Config.Delta), 0, attacks.Count);

            if (TryParseToDigit(X2Box, out card2) > 0)
            {
                for (int i = 0, size = variants.Count; i < size; i++)
                {
                    var arr = CopyList(variants[i]);
                    Parse(arr, (x) => x.EnableCard(CardEffect.x2), 0, card2);
                }
            }
            if (TryParseToDigit(X3Box, out card3) > 0)
            {
                for (int i = 0, size = variants.Count; i < size; i++)
                {
                    var arr = CopyList(variants[i]);
                    Parse(arr, (x) => x.EnableCard(CardEffect.x3), 0, card3);
                }
            }
            if (TryParseToDigit(X5Box, out card5) > 0)
            {
                for (int i = 0, size = variants.Count; i < size; i++)
                {
                    var arr = CopyList(variants[i]);
                    Parse(arr, (x) => x.EnableCard(CardEffect.x5), 0, card5);
                }
            }

            double result_time = 0;
            foreach (var item in variants)
            {
                if (UnlockUnsafeSettings.IsChecked == true)
                {
                    if (ArmySpeedBox.SelectedIndex != speed_list.Count - 1 || CapSpeedBox.SelectedIndex != speed_list.Count - 1)
                    {
                        foreach (var att in item)
                        {
                            switch (att.armytype)
                            {
                                case ArmyType.Captain:
                                    att.Time /= speed_list[CapSpeedBox.SelectedIndex];
                                    break;
                                case ArmyType.Army:
                                    att.Time /= speed_list[ArmySpeedBox.SelectedIndex];
                                    break;
                            }
                        }
                    }
                }
                item.Sort();
                bool good = true;
                int caps = 0;
                for (int i = 1; i < item.Count; i++)
                {
                    if (item[i].Time - item[i - 1].Time < int.Parse(RestTimeTextBox.Text))
                    {
                        good = false;
                        break;
                    }
                }
                foreach (var cap in item)
                {
                    if (CheckBoxForceCaptain.IsChecked == true)
                    {
                        if (cap.armytype == ArmyType.Captain)
                            caps++;
                    }
                }
                if (CheckBoxForceCaptain.IsChecked == true && caps == 0)
                    continue;

                var temp = item[item.Count - 1].Time - item[0].Time;
                if (good && (temp < result_time || result_time == 0))
                {
                    result_time = item[item.Count - 1].Time - item[0].Time;
                    target.Best = item;
                }
            }

            if (target.Best == null)
                NoResultDialog();
            else
            {
                foreach (var item in target.Best)
                {
                    if (item.armytype == ArmyType.Unknown)
                        item.armytype = ArmyType.Army;
                }
                target.Best.Reverse();
                TotalTimeTextBlock.Text = Timinger.Language.TotalTimeToAttack + ": " + strfun.SecondsToTimeString(target.Best.First().Time - target.Best.Last().Time);
                ResultGrid.ItemsSource = target.Best;
            }

        }
        private void FindBest(object sender, RoutedEventArgs e)
        {
            if (target == null || target.attacks.Count < 2 || CheckCaps() == false)
                return;

            List<Attack> attacks = CopyList(target.attacks.ToList());
            attacks.RemoveAll((x) => x.Active == false);
            if (attacks.Count < 2)
                return;
            variants.Clear();
            target.Best = null;

            if (UnlockUnsafeSettings.IsChecked == true)
            {
                if (ArmySpeedBox.SelectedIndex != speed_list.Count - 1 || CapSpeedBox.SelectedIndex != speed_list.Count - 1)
                {
                    foreach (var item in attacks)
                    {
                        switch (item.armytype)
                        {
                            case ArmyType.Captain:
                                item.Time *= speed_list[CapSpeedBox.SelectedIndex];
                                break;
                            case ArmyType.Army:
                                item.Time *= speed_list[ArmySpeedBox.SelectedIndex];
                                break;
                            case ArmyType.Unknown:
                                item.Time *= speed_list[ArmySpeedBox.SelectedIndex];
                                break;
                        }
                    }
                }
            }

            int card2 = 0;
            int card3 = 0;
            int card5 = 0;

            variants.Add(attacks);
            Parse(attacks, (x) => x.EnableCap(viewModel.Config.Delta), 0, attacks.Count);
            
            if (TryParseToDigit(X2Box,out card2) > 0)
            {
                for (int i = 0, size = variants.Count; i < size; i++)
                {
                    var arr = CopyList(variants[i]);
                    Parse(arr, (x) => x.EnableCard(CardEffect.x2), 0, card2);
                }
            }
            if (TryParseToDigit(X3Box,out card3) > 0)
            {
                for (int i = 0, size = variants.Count; i < size; i++)
                {
                    var arr = CopyList(variants[i]);
                    Parse(arr, (x) => x.EnableCard(CardEffect.x3), 0, card3);
                }
            }
            if (TryParseToDigit(X5Box,out card5) > 0)
            {
                for (int i = 0, size = variants.Count; i < size; i++)
                {
                    var arr = CopyList(variants[i]);
                    Parse(arr, (x) => x.EnableCard(CardEffect.x5), 0, card5);
                }
            }

            double result_time = 0;
            foreach (var item in variants)
            {
                if (UnlockUnsafeSettings.IsChecked == true)
                {
                    if (ArmySpeedBox.SelectedIndex != speed_list.Count - 1 || CapSpeedBox.SelectedIndex != speed_list.Count - 1)
                    {
                        foreach (var att in item)
                        {
                            switch (att.armytype)
                            {
                                case ArmyType.Captain:
                                    att.Time /= speed_list[CapSpeedBox.SelectedIndex];
                                    break;
                                case ArmyType.Army:
                                    att.Time /= speed_list[ArmySpeedBox.SelectedIndex];
                                    break;
                            }
                        }
                    }
                }
                item.Sort();
                bool good = true;
                int caps = 0;
                for (int i = 1; i < item.Count; i++)
                {
                    if (item[i].Time - item[i - 1].Time < int.Parse(RestTimeTextBox.Text))
                    {
                        good = false;
                        break;
                    }
                }
                foreach (var cap in item)
                {
                    if (CheckBoxForceCaptain.IsChecked == true)
                    {
                        if (cap.armytype == ArmyType.Captain)
                            caps++;
                    }
                }
                if (CheckBoxForceCaptain.IsChecked == true && caps == 0)
                    continue;

                var temp = item[item.Count - 1].Time - item[0].Time;
                if (good && (temp < result_time || result_time == 0))
                {
                    result_time = item[item.Count-1].Time - item[0].Time;
                    target.Best = item;
                }
            }

            if (target.Best == null)
                NoResultDialog();
            else
            {
                foreach (var item in target.Best)
                {
                    if (item.armytype == ArmyType.Unknown)
                        item.armytype = ArmyType.Army;
                }
                target.Best.Reverse();
                TotalTimeTextBlock.Text = Timinger.Language.TotalTimeToAttack + ": " + strfun.SecondsToTimeString(target.Best.First().Time - target.Best.Last().Time);
                ResultGrid.ItemsSource = target.Best;
            }
            
        }
        private void Parse(List<Attack> arr, Action action, int pos, int times)
        {
            if (times == 0 || pos >= arr.Count)
                return;
            for (int i = pos; i < arr.Count; i++)
            {
                List<Attack> parsing = CopyList(arr);
                var executed = action(parsing[i]);
                if(executed)
                    variants.Add(parsing);
                Parse(parsing, action, i + 1, times - 1);
            }
        }
        private int TryParseToDigit(TextBox text,out int res)
        {
            if(int.TryParse(text.Text,out res) == false)
            {
                text.Text = 0.ToString();
            }
            return res;
        }
        private List<Attack> CopyList(List<Attack>parent)
        {
            List<Attack> result = new List<Attack>(parent.Count);
            foreach (var item in parent)
            {
                result.Add((Attack)item.Clone());
            }
            return result;
        }
        private bool CheckCaps()
        {
            if(CheckBoxForceCaptain.IsChecked == true)
            {
                foreach (var item in target.attacks)
                {
                    if (item.armytype == ArmyType.Captain || item.armytype == ArmyType.Unknown)
                        return true;
                }
                MessageBox.Show(Timinger.Language.NoSlotForCaptain);
                return false;
            }
            return true;
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
            targets.Add(new Target($"Target {targets.Count+1}"));
            if (TargetGrid.Items.Count == 1)
                TargetGrid.SelectedIndex = 0;
        }
        private void AddAttackToGrid(object sender, RoutedEventArgs e)
        {
            var x = (string)DefaultArmyTypeBox.SelectedItem;
            target?.attacks.Add(new Attack($"Attack {target.attacks.Count+1}", 0, x));
        }
        private void DeleteAttackFromGrid(object sender, RoutedEventArgs e)
        {
            var x = AttacksGrid.SelectedIndex;
            if (x != -1)
            {
                target.attacks.RemoveAt(x);
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
            TargetChanged(this, null);
        }
        private void SwitchToENG(object sender, RoutedEventArgs e)
        {
            viewModel.SwitchLanguage("ENG");
            InitColumns();
            DefaultArmyTypeBox.SelectedIndex = 2;
            TargetChanged(this, null);
            
        }
        private void InitColumns()
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
            if (target != null)
            {
                var x = (Button)e.Source;
                if (x.Name == "ButtonCopyAttack")
                {
                    CopyWindow copyWindow = new CopyWindow(viewModel.Language,target.Name, target.attacks.ToList());
                    copyWindow.Owner = this;
                    copyWindow.Closed += (object obj, EventArgs a) => this.IsEnabled = true;
                    this.IsEnabled = false;
                    copyWindow.Show();
                }
                else
                {
                    if (target.Best != null)
                    {
                        CopyWindow copyWindow = new CopyWindow(viewModel.Language,target.Name, target.Best);
                        copyWindow.Owner = this;
                        copyWindow.Closed += (object obj, EventArgs a) => this.IsEnabled = true;
                        this.IsEnabled = false;
                        copyWindow.Show();
                    }
                }
            }
        }
        private void TargetChanged(object sender, SelectionChangedEventArgs e)
        {
            target = (Target)TargetGrid.SelectedItem;
            if (target != null)
            {
                AttacksGrid.ItemsSource = null;
                AttacksGrid.ItemsSource = target.attacks;
                ResultGrid.ItemsSource = target.Best;
                if (target.Best != null)
                    TotalTimeTextBlock.Text = Timinger.Language.TotalTimeToAttack + ": " + strfun.SecondsToTimeString(target.Best.First().Time - target.Best.Last().Time);
                else
                    TotalTimeTextBlock.Text = "";
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CreateNewFile(object sender, RoutedEventArgs e)
        {
            if(targets.Count != 0)
            {
                if (MessageBox.Show(Timinger.Language.CreateNewFile, Timinger.Language.New, MessageBoxButton.YesNo) == MessageBoxResult.No)
                    return;
            }
            targets.Clear();
            variants.Clear();
            target = null;
            AttacksGrid.ItemsSource = null;
            ResultGrid.ItemsSource = null;
            viewModel.TimPath = null;

        }
        private void SaveFile(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(viewModel.TimPath))
            {
                BinaryFormatter binary = new BinaryFormatter();
                using (FileStream stream = new FileStream(viewModel.TimPath, FileMode.Open))
                {
                    binary.Serialize(stream, targets);
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
                    formatter.Serialize(stream, targets);
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
                targets = (ObservableCollection<Target>)binary.Deserialize(stream);
                TargetGrid.ItemsSource = targets;
                AttacksGrid.ItemsSource = null;
                ResultGrid.ItemsSource = null;
            }
        }

        private void UnsafeSettingsOn(object sender, RoutedEventArgs e)
        {
            if(UnlockUnsafeSettings.IsChecked == true)
            {
                MessageBox.Show(Timinger.Language.UnsafeWarning);
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }   
}
