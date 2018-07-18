using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timinger
{
    public static class strfun
    {
        public static string SecondsToTimeString(double seconds)
        {
            int time = (int)seconds;
            List<int> temp = new List<int>();
            temp.Add(time / 3600);
            time %= 3600;
            temp.Add(time / 60);
            time %= 60;
            temp.Add(time);
            string str = "";
            foreach (var item in temp)
            {
                if (str.Length > 0)
                    str += ":";
                str += item.ToString().Length == 1 ? "0" + item.ToString() : item.ToString();
            }
            return str;
        }
        public static int ParseTimeString(string time)
        {
            List<int> temp = new List<int>(3) { 0, 0, 0 };
            for (int i = time.Length - 1, index = 0, mod = 1; i >= 0; i--)
            {
                if (char.IsDigit(time[i]))
                {
                    temp[index] += int.Parse(time[i].ToString()) * mod;
                    mod *= 10;
                }
                else
                {
                    index++;
                    if (index == 3)
                        return 0;
                    mod = 1;
                }

            }
            int result = 0;
            for (int i = 0, mod = 1; i < temp.Count; i++, mod *= 60)
            {
                result += temp[i] * mod;
            }
            return result;
        }
        public static List<Attack> CopyList(List<Attack> parent)
        {
            List<Attack> result = new List<Attack>(parent.Count);
            foreach (var item in parent)
            {
                result.Add((Attack)item.Clone());
            }
            return result;
        }
    }
}
