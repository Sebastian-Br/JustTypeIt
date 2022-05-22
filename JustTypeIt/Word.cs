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
            RecentErrorsMax = 3;
            OriginalWord = "";
            Definition = "";
            DefinitionGeneralized = "";
            RecentErrors = 0;
        }

        public string OriginalWord { get; set; }
        public string Definition { get; set; }

        /// <summary>
        /// Removes leading/trailing spaces, as well as spaces surrounding commata.
        /// Intended to make string-comparison more forgiving.
        /// </summary>
        public string DefinitionGeneralized { get; set; }

        public int RecentErrors { get; set; }

        private int RecentErrorsMax { get; set; }

        public void AddError()
        {
            RecentErrors += 2;
            if (RecentErrors > RecentErrorsMax)
            {
                RecentErrors = RecentErrorsMax;
            }
        }

        public void AddSuccess()
        {
            RecentErrors--;
            if (RecentErrors < 0)
            {
                RecentErrors = 0;
            }
        }
    }
}