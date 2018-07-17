using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Timinger
{
    public enum CardEffect
    {
        none,
        x2,
        x3,
        x4,
        x5,
        x6
    }
    public enum ArmyType
    {
        Captain,
        Army,
        Unknown
    }

    [Serializable]
    public class Attack : IComparable<Attack>, ICloneable
    {
        //public static double delta = 3.00; //3.018

        public string   Name { get; set; }

        public double   Time { get; set; }
        public string   TimeStr
        {
            get { return strfun.SecondsToTimeString(Time); }
            set { this.Time = strfun.ParseTimeString(value); }
        }
        
        public Timinger.ArmyType armytype = Timinger.ArmyType.Unknown;
        public string ArmyTypeStr
        {
            get
            {
                foreach (var item in Language.type_dict)
                {
                    if (item.Value == armytype)
                        return item.Key;
                }
                return "Error";
            }
            set
            {
                this.armytype = Language.type_dict[value];
            }
        }

        public CardEffect Card = CardEffect.none;
        public string CardStr
        {
            get
            {
                switch (Card)
                {
                    case CardEffect.none:
                        return Language.No;
                    case CardEffect.x2:
                        return "x2";
                    case CardEffect.x3:
                        return "x3";
                    case CardEffect.x4:
                        return "x4";
                    case CardEffect.x5:
                        return "x5";
                    case CardEffect.x6:
                        return "x6";
                    default:
                        return Language.No;
                }
            }
        }
        public bool Active { get; set; } = true;

        public Attack(string name, double time, string type)
        {
            Name = name;
            Time = time;
            ArmyTypeStr = type;
        }
        public Attack(string name, double time,ArmyType type,CardEffect card, bool active)
        {
            Name = name;
            Time = time;
            Card = card;
            armytype = type;
            Active = active;
        }

        public bool EnableCap(double delta)
        {
            if (armytype == ArmyType.Unknown)
            {
                armytype = Timinger.ArmyType.Captain;
                Time *= delta;
                return true;
            }
            return false;
        }
        public bool EnableCard(CardEffect card)
        {
            if (Card == CardEffect.none)
            {
                switch (card)
                {
                    case CardEffect.none:
                        break;
                    case CardEffect.x2:
                        Time /= 2;
                        break;
                    case CardEffect.x3:
                        Time /= 3;
                        break;
                    case CardEffect.x4:
                        Time /= 4;
                        break;
                    case CardEffect.x5:
                        Time /= 5;
                        break;
                    case CardEffect.x6:
                        Time /= 6;
                        break;
                    default:
                        break;
                }
                this.Card = card;
                return true;
            }
            return false;
        }

        public int CompareTo(Attack other)
        {
            return Time.CompareTo(other.Time);
        }
        public object Clone()
        {
            return new Attack(this.Name, this.Time,this.armytype,this.Card, this.Active);
        }
    }
}
