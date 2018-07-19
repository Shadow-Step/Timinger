using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Timinger
{
    class ViewModel : INotifyPropertyChanged
    {
        //Events
        public delegate bool Action(Attack attack);
        public delegate void MessageDialog(string message);
        public delegate void FindBestIsDone(List<Attack> Best);
        public event PropertyChangedEventHandler PropertyChanged;
        public event MessageDialog ShowMessageDialog;
        //Fields
        private static ViewModel viewModel = null;
        private List<Attack> temp_best = null;
        private bool ruslang;
        private bool englang;
        private bool isalive = true;
        private string timpath;
        private double totalpercents = 0;
        private Language language;
        private Target target;
        private ObservableCollection<Target> targets;
        

        private List<List<Attack>> variants = new List<List<Attack>>();
        private List<double> speed_list = new List<double>()
        {
            1.00,1.25,1.50,1.75,2.00,2.25,3.00,4.50,6.00
        };

        //Property
        public ObservableCollection<Target> Targets
        {
            get { return this.targets; }
            set
            {
                this.targets = value;
                INotifyPropertyChanged("Targets");
            }
        }
        public Target Target
        {
            get { return this.target; }
            set
            {
                this.target = value;
                    INotifyPropertyChanged("Target");                
            }
        }
        public Config Config { get; set; }
        public int Card2 { get; set; } = 0;
        public int Card3 { get; set; } = 0;
        public int Card5 { get; set; } = 0;
        public int ArmySpeedIndex { get; set; } = 8;
        public int CapSpeedIndex { get; set; } = 8;
        public int RestTime { get; set; } = 30;
        public bool UnsafeMode { get; set; } = false;
        public bool ForceCaptain { get; set; } = true;
        public string Variants
        {
            get { return Language.Variants + ": " + variants?.Count.ToString() ?? "0"; }
        }
        public bool RusLang
        {
            get { return this.ruslang; }
            set { this.ruslang = value;INotifyPropertyChanged("RusLang"); }

        }
        public bool EngLang
        {
            get { return this.englang; }
            set { this.englang = value; INotifyPropertyChanged("EngLang"); }

        }
        public Language Language
        {
            get { return language; }
            set
            {
                language = value;
                INotifyPropertyChanged("Language");
            }
        }
        public string TimPath
        {
            get { return timpath; }
            set { timpath = value; Config.LastProjectPath = timpath; INotifyPropertyChanged("TimPath"); }
        }
        public bool IsAlive
        {
            get { return this.isalive; }
            set
            {
                this.isalive = value;
                INotifyPropertyChanged("IsAlive");
                INotifyPropertyChanged("IsAliveStr");
            }
        }
        public string IsAliveStr
        {
            get { return isalive == true ? Language.Recount : Language.Abort; }
        }
        public double TotalPercents
        {
            get { return this.totalpercents; }
            set { this.totalpercents = value;INotifyPropertyChanged("TotalPercents"); }
        }
        //Methods

        private ViewModel()
        {
            Config = Config.GetInstance();
            Config.LoadFromFile();
            Language = new Language(Config.Language);
            Targets = new ObservableCollection<Target>();
            //switch (config.Language)
            //{
            //    case "RUS":
            //        RusLang = true;
            //        EngLang = false;
            //        break;
            //    case "ENG":
            //        RusLang = false;
            //        EngLang = true;
            //        break;
            //}
        }
        public static ViewModel GetInstance()
        {
            if(viewModel == null)
            {
                viewModel = new ViewModel();
            }
            return viewModel;
        }

        void INotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public void SwitchLanguage(string local)
        {
            if(local != Language.CurrentLanguage)
            {
                Language = new Language(local);
                Config.Language = local;
                switch (Config.Language)
                {
                    case "RUS":
                        RusLang = true;
                        EngLang = false;
                        break;
                    case "ENG":
                        RusLang = false;
                        EngLang = true;
                        break;
                }
            }
            INotifyPropertyChanged("IsAliveStr");
            var temp = Target;
            Target = null;
            Target = temp;
        }

        public void FindBest()
        {
            try
            {
                IsAlive = false;
                TotalPercents = 0;

                List<Attack> attacks = strfun.CopyList(Target.Attacks.ToList());
                attacks.RemoveAll((x) => x.Active == false);

                if (attacks.Count < 2)
                {
                    ShowMessageDialog(Language.NotEnoughAttacks);
                    IsAlive = true;
                    return;
                }
                if (CheckCaps() == false)
                {
                    IsAlive = true;
                    return;
                }
                if (UnsafeMode == true)
                {
                    if (ArmySpeedIndex != speed_list.Count - 1 || CapSpeedIndex != speed_list.Count - 1)
                {
                    foreach (var item in attacks)
                    {
                        switch (item.armytype)
                        {
                            case ArmyType.Captain:
                                item.Time *= speed_list[CapSpeedIndex];
                                break;
                            case ArmyType.Army:
                                item.Time *= speed_list[ArmySpeedIndex];
                                break;
                            case ArmyType.Unknown:
                                item.Time *= speed_list[ArmySpeedIndex];
                                break;
                        }
                    }
                }
                }

                variants.Clear();
                temp_best = null;

                AddVariant(attacks);
                TotalPercents += 5;
                Parse(attacks, (x) => x.EnableCap(UnsafeMode == true ? Config.Delta : 3.00), 0, attacks.Count);
                if (Card2 > 0)
                {
                    for (int i = 0, size = variants.Count; i < size; i++)
                    {
                        TotalPercents += 1.0 / size * (5 + (Card3 == 0 ? 10 + (Card5 == 0 ? 80 : 0) : 0)); 
                        var arr = strfun.CopyList(variants[i]);
                        Parse(arr, (x) => x.EnableCard(CardEffect.x2), 0, Card2);
                    }
                }
                if (Card3 > 0)
                {
                    for (int i = 0, size = variants.Count; i < size; i++)
                    {
                        TotalPercents += 1.0 / size * (10 + (Card5 == 0 ? 80 : 0));
                        var arr = strfun.CopyList(variants[i]);
                     Parse(arr, (x) => x.EnableCard(CardEffect.x3), 0, Card3);
                    }
                }
                if (Card5 > 0)
                {
                    for (int i = 0, size = variants.Count; i < size; i++)
                    {
                        TotalPercents += 1.0 / size * 80;
                        var arr = strfun.CopyList(variants[i]);
                        Parse(arr, (x) => x.EnableCard(CardEffect.x5), 0, Card5);
                    }
                }

                if (temp_best == null)
                {
                    ShowMessageDialog(Timinger.Language.NoVariants);
                    IsAlive = true;
                }
                else
                {
                    foreach (var item in temp_best)
                    {
                    if (item.armytype == ArmyType.Unknown)
                        item.armytype = ArmyType.Army;
                    }
                    temp_best.Reverse();
                    Target.Best = new ObservableCollection<Attack>(temp_best);
                    TotalPercents = 0;
                    IsAlive = true;
                }
            }
            catch (Exception)
            {
                IsAlive = true;
                TotalPercents = 0;
                return;
            }
            TotalPercents = 0;
        }
        private bool CheckCaps()
        {
            if (ForceCaptain == true)
            {
                foreach (var item in Target.Attacks)
                {
                    if (item.armytype == ArmyType.Captain || item.armytype == ArmyType.Unknown)
                        return true;
                }
                ShowMessageDialog(Timinger.Language.NoSlotForCaptain);
                return false;
            }
            return true;
        }
        private void Parse(List<Attack> arr, Action action, int pos, int times)
        {
            if (times == 0 || pos >= arr.Count)
                return;
            for (int i = pos; i < arr.Count; i++)
            {
                List<Attack> parsing = strfun.CopyList(arr);
                var executed = action(parsing[i]);
                if (executed)
                    AddVariant(parsing);
                    Parse(parsing, action, i + 1, times - 1);
            }
        }
        private void CheckBest(List<Attack> obj)
        {
            List<Attack> temp_list = strfun.CopyList(obj);
            if (UnsafeMode == true)
            {
                if (ArmySpeedIndex != speed_list.Count - 1 || CapSpeedIndex != speed_list.Count - 1)
                {
                    foreach (var att in temp_list)
                    {
                        switch (att.armytype)
                        {
                            case ArmyType.Captain:
                                att.Time /= speed_list[CapSpeedIndex];
                                break;
                            case ArmyType.Army:
                            case ArmyType.Unknown:
                                att.Time /= speed_list[ArmySpeedIndex];
                                break;
                        }
                    }
                }
            }
            temp_list.Sort();
            int caps = 0;
            for (int i = 1; i < temp_list.Count; i++)
            {
                if (temp_list[i].Time - temp_list[i - 1].Time < RestTime)
                {
                    return;
                }
            }
            foreach (var cap in temp_list)
            {
                if (ForceCaptain == true)
                {
                    if (cap.armytype == ArmyType.Captain)
                        caps++;
                }
            }
            if (ForceCaptain == true && caps == 0)
                return;

            var temp = temp_list.Last().Time - temp_list.First().Time;
            if (temp_best == null || temp < temp_best.Last().Time - temp_best.First().Time)
            {
                temp_best = temp_list;
            }
        }
        private void AddVariant(List<Attack> var)
        {
            CheckBest(var);
            variants.Add(var);
            INotifyPropertyChanged("Variants");
        }
        private void Reverse(ObservableCollection<Attack>collection)
        {
            for (int l = 0, r = collection.Count-1; l < r; l++,r--)
            {
                Attack temp = collection[r];
                collection[r] = collection[l];
                collection[l] = temp;
            }
        }
        private void CountVariants(int attacks)
        {
            int x = attacks + (Card2 > 0 ? attacks : 0) + (Card3 > 0 ? attacks : 0) + (Card5 > 0 ? attacks : 0);
            
        }
    }
}
