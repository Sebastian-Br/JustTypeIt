using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTypeIt
{
    internal class Word
    {
        public Word()
        {
            RecentErrorsMax = 4;
            OriginalWord = "";
            Definition = "";
            RecentErrors = 0;
        }

        public string OriginalWord { get; set; }
        public string Definition { get; set; }

        public int RecentErrors { get; set; }

        private int RecentErrorsMax { get; set; }

        public void AddError()
        {
            if(RecentErrors > RecentErrorsMax)
            {
                RecentErrors = RecentErrorsMax;
            }
            else
            {
                RecentErrors += 2;
            }
        }

        public void AddSuccess()
        {
            if (RecentErrors > 0)
            {
                RecentErrors--;
            }
            else
            {
                return;
            }
        }
    }
}