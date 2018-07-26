using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Timinger
{
    public class Alarm : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private double[] timers;
        private int iterator = 0;
        private double currenttime;
        private Attack currentattack;
        private Attack nextattack;
        private bool autoclick = true;
        private bool detailtime = true;
        private double alarmtime = 60;
        private double whendetail = 10;
        private double notfairtime = 0.4;
        private bool attacksfromresult = false;
        private bool attacksfromattacs = true;
        private bool hasattacks;
        private bool notfairoption = false;
        private bool started = false;
        private List<Attack> attack_list;
        private string hotkey = "F2";

        public List<Attack> AttackList
        {
            get { return attack_list; }
            set { attack_list = value; INotifyPropertyChanged("AttackList"); }
        }
        public string SoundPath { get; set; }
        public MediaPlayer Player { get; set; }
        public Target Target { get; set; }
        public double Volume
        {
            get { return Player.Volume; }
            set
            {
                Player.Volume = value;
                Config.GetInstance().TimerVolume = value;
                INotifyPropertyChanged("Volume");
            }
        }
        public double CurrentTime
        {
            get { return currenttime; }
            set
            {
                currenttime = value;
                INotifyPropertyChanged("CurrentTime");
                INotifyPropertyChanged("CurrentTimeStr");
            }
        }
        public string CurrentTimeStr
        {
            get
            {
                if (DetailTime == true)
                {
                    if (CurrentTime > WhenDetail || CurrentTime <= 0)
                    {
                        if (TimeAlign != "Center")
                        {
                            TimeAlign = "Center";
                            INotifyPropertyChanged("TimeAlign");
                        }
                        return strfun.SecondsToTimeString(CurrentTime);
                    }
                    else
                    {
                        if (TimeAlign != "Left")
                        {
                            TimeAlign = "Left";
                            INotifyPropertyChanged("TimeAlign");
                        }
                        return CurrentTime.ToString();
                    }

                }
                else
                    return strfun.SecondsToTimeString(CurrentTime);
            }
        }
        public Attack CurrentAttack
        {
            get { return currentattack; }
            set
            {
                currentattack = value;
                INotifyPropertyChanged("CurrentAttack");
            }
        }
        public Attack NextAttack
        {
            get { return nextattack; }
            set
            {
                nextattack = value;
                INotifyPropertyChanged("NextAttack");
            }
        }
        public int Iterator
        {
            get { return iterator; }
            set { iterator = value; INotifyPropertyChanged("Iterator"); }
        }
        public bool Started
        {
            get { return started; }
            set
            {
                started = value;
                INotifyPropertyChanged("Started");
            }
        }
        public bool DetailTime
        {
            get { return detailtime; }
            set
            {
                detailtime = value;
                Config.GetInstance().TimerDetailTime = value;
                INotifyPropertyChanged("DetailTime");
            }
        }
        public double WhenDetail
        {
            get { return whendetail; }
            set
            {
                whendetail = value;
                Config.GetInstance().TimerSecondsToDetail = value;
                INotifyPropertyChanged("WhenDetail");
            }
        }
        public double NotFairTime
        {
            get { return notfairtime; }
            set
            {
                notfairtime = value;
                INotifyPropertyChanged("NotFairTime");
            }
        }
        public bool AutoClick
        {
            get { return autoclick; }
            set
            {
                autoclick = value;
                Config.GetInstance().TimerAutoClick = value;
                INotifyPropertyChanged("AutoClick");
            }
        }
        public bool AttacksFromResult
        {
            get { return attacksfromresult; }
            set
            {
                if (Started == false)
                {
                    attacksfromresult = value;
                    Config.GetInstance().TimerAttacksFromResultTable = value;
                    SetTarget(Target);
                    INotifyPropertyChanged("AttacksFromResult");
                }
            }
        }
        public bool AttacksFromAttacks
        {
            get { return attacksfromattacs; }
            set
            {
                if (Started == false)
                {
                    attacksfromattacs = value;
                    SetTarget(Target);
                    INotifyPropertyChanged("AttacksFromAttacks");
                }
            }
        }
        public bool HasAttacks
        {
            get { return attack_list == null ? false : attack_list.Count > 1; }
            set { hasattacks = value; }
        }
        public bool NotFairOption
        {
            get { return notfairoption; }
            set
            {
                notfairoption = value;
                INotifyPropertyChanged("NotFairOption");
            }
        }
        public double AlarmTime
        {
            get { return alarmtime; }
            set
            {
                alarmtime = value;
                Config.GetInstance().TimerSecondsToSignal = value;
                INotifyPropertyChanged("AlarmTime");
            }
        }
        public string TimeAlign { get; set; } = "Center";
        public string HotKey
        {
            get { return hotkey; }
            set
            {
                hotkey = value;
                Config.GetInstance().TimerHotKey = value;
                KeyRegister.RegisterKey(value);
                INotifyPropertyChanged("HotKey");
            }
        }
        public DateTime UnsafeNextTime {get;set;}

        public Alarm()
        {
            Player = new MediaPlayer();
            Player.Open(new Uri("alarm.mp3", UriKind.Relative));
            Config cfg = Config.GetInstance();
            Volume = cfg.TimerVolume;
            AlarmTime = cfg.TimerSecondsToSignal;
            WhenDetail = cfg.TimerSecondsToDetail;
            AutoClick = cfg.TimerAutoClick;
            AttacksFromResult = cfg.TimerAttacksFromResultTable;
            AttacksFromAttacks = !cfg.TimerAttacksFromResultTable;
            DetailTime = cfg.TimerDetailTime;
        }
        public bool Start(IProgress<bool> progress)
        {
            
            if (attack_list == null || attack_list.Count < 2)
                return false;
            attack_list.Sort();
            attack_list.Reverse();
            Iterator = 1;
            bool alarm = false;
            bool NotFairClick = false;
            CurrentAttack = attack_list[iterator - 1];
            NextAttack = attack_list[iterator];
            UnsafeNextTime = DateTime.Now.AddSeconds(CurrentAttack.Time - NextAttack.Time);
            while (Started == true && iterator < attack_list.Count)
            {
                var time = UnsafeNextTime.Subtract(DateTime.Now);
                CurrentTime = time.TotalSeconds;
                //
                if (NotFairOption == true && NotFairClick == false)
                {
                    if (CurrentTime <= NotFairTime && NotFairTime >= 0)
                    {
                        KeyRegister.Click();
                        NotFairClick = true;
                    }
                    else if (NotFairTime < 0 && CurrentTime <= 0)
                    {
                        new Thread(() => KeyRegister.Click((int)Math.Abs(NotFairTime * 1000))).Start();
                    }
                }
                //
                if (CurrentTime <= AlarmTime && alarm == false)
                {
                    alarm = true;
                    progress.Report(true);
                }
                if (CurrentTime <= 0)
                {
                    Iterator++;
                    if(iterator >= attack_list.Count)
                    {
                        Started = false;
                        CurrentTime = 0;
                        Iterator = 0;
                        return true;
                    }
                    else
                    {
                        NotFairClick = false;
                        CurrentAttack = attack_list[iterator - 1];
                        NextAttack = attack_list[iterator];
                        UnsafeNextTime = DateTime.Now.AddSeconds(CurrentAttack.Time - NextAttack.Time);
                        CurrentTime = UnsafeNextTime.Subtract(DateTime.Now).TotalSeconds;
                        alarm = false;
                    }
                }
            }
            CurrentTime = 0;
            Iterator = 0;
            return true;
        }
        public void SetTimers(List<Attack> args)
        {
            if (args.Count < 2)
                return;
            args.Sort();
            iterator = 0;
            timers = new double[args.Count - 1];
            for (int i = args.Count-1,iter = 0; i > 0; i--,iter++)
            {
                timers[iter] = Math.Abs(args[i].Time - args[i - 1].Time);
            }
        }
        public void SetTarget(Target target)
        {
            Target = target;
            
            if (Target == null)
            {
                CurrentAttack = null;
                NextAttack = null;
                AttackList = null;
                return;
            }
            else
            {
                if(AttacksFromResult == true)
                {
                    AttackList = Target.Best?.ToList() ?? null;
                }
                else
                {
                    AttackList = Target.Attacks?.ToList() ?? null;
                }
                if (AttackList != null && AttackList.Count > 1)
                {
                    AttackList.Sort();
                    AttackList.Reverse();
                    NextAttack = AttackList[0];
                }
            }
            INotifyPropertyChanged("HasAttacks");
            INotifyPropertyChanged("AttackList");
        }
        public void PlaySound()
        {
            Player.Position = new TimeSpan();
            Player.Play();
        }
        public void ClosePlayer()
        {
            if(Player != null)
            {
                Player.Stop();
                Player.Close();
            }
        }
        void INotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
