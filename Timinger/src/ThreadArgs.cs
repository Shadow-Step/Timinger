using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timinger
{
    class ThreadArgs
    {
        List<Attack> attacks;
        int card2;
        int card3;
        int card5;

        public ThreadArgs(List<Attack> attacks, int card2,int card3,int card5)
        {
            this.attacks = attacks;
            this.card2 = card2;
            this.card3 = card3;
            this.card5 = card5;
        }
    }
}
