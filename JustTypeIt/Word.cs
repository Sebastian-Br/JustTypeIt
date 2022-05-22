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
            AllTimeCorrectAnswers = 0;
            AllTimeINCorrectAnswers = 0;
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

        private int AllTimeCorrectAnswers { get; set; }
        private int AllTimeINCorrectAnswers { get; set; }

        public bool IsWellKnown()
        {
            if(AllTimeINCorrectAnswers + AllTimeCorrectAnswers <= 4)
            {
                if(AllTimeINCorrectAnswers == 0 && AllTimeCorrectAnswers > 2) { return true; }
            }
            else
            {
                if(AllTimeCorrectAnswers * 5 >= AllTimeINCorrectAnswers)
                {
                    return true;
                }
            }

            return false;
        }

        public void AddError()
        {
            RecentErrors += 2;
            AllTimeINCorrectAnswers++;
            if (RecentErrors > RecentErrorsMax)
            {
                RecentErrors = RecentErrorsMax;
            }
        }

        public void AddSuccess()
        {
            RecentErrors--;
            AllTimeCorrectAnswers++;
            if (RecentErrors < 0)
            {
                RecentErrors = 0;
            }
        }
    }
}