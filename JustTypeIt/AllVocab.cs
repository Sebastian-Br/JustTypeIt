using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace JustTypeIt
{
    internal class AllVocab
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        public AllVocab()
        {
            Words = new();
            HardWords = new();
            RecentWord = new();
            random = new();
        }
        public List<Word> Words { get; set; }

        public List<Word> HardWords { get; set; }

        public Word RecentWord { get; set; }
        public Word CurrentWord { get; set; }

        public bool Check(string studentAnswer)
        {
            try
            {
                if(String.Equals(CurrentWord.Definition, studentAnswer, StringComparison.OrdinalIgnoreCase))
                {
                    CurrentWord.AddSuccess();
                    if (CurrentWord.RecentErrors <= 1)
                    {
                        if(HardWords.Contains(CurrentWord))
                        {
                            HardWords.Remove(CurrentWord);
                        }
                    }
                    RecentWord = CurrentWord;
                    return true;
                }
                else
                {
                    CurrentWord.AddError();
                    if(CurrentWord.RecentErrors > 1)
                    {
                        HardWords.Add(CurrentWord);
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        public string GetNextWord()
        {
            try
            {
                if(HardWords.Count == 1)
                {
                    CurrentWord = HardWords[0];
                    return CurrentWord.OriginalWord;
                }
                else if (HardWords.Count > 1 && (random.Next() % 2) == 0)
                {
                    int index = random.Next(HardWords.Count);
                    CurrentWord = HardWords[index];
                    return CurrentWord.OriginalWord;
                }
                else
                {
                    int index = random.Next(Words.Count);
                    CurrentWord = Words[index];
                    return CurrentWord.OriginalWord;
                }
            }
            catch(Exception ex)
            {
                return "#EXCEPTION#";
            }
        }

        public bool ParseFileContent(string fileContent)
        {
            try
            {
                Words = new();
                HardWords = new();
                CurrentWord = new();
                RecentWord = new();
                Regex parseWordsRegex = new("(?<word>.*);(?<definition>.*)");
                MatchCollection parsedWordsCollection = parseWordsRegex.Matches(fileContent);
                if(parsedWordsCollection.Count > 0)
                {
                    bool catchErrorMatch = true;
                    string errorMatchContent = "";
                    foreach (Match match in parsedWordsCollection)
                    {
                        if(match.Success)
                        {
                            Word newWord = new();
                            newWord.OriginalWord = match.Groups["word"].Value;
                            newWord.Definition = match.Groups["definition"].Value;
                            char lastChar = newWord.Definition[newWord.Definition.Length - 1];
                            if (!(Char.IsSeparator(lastChar) || Char.IsLetterOrDigit(lastChar)))
                            {
                                newWord.Definition = newWord.Definition.Remove(newWord.Definition.Length - 1);
                            }
                            Words.Add(newWord);
                        }
                        else if(catchErrorMatch)
                        {
                            catchErrorMatch = false;
                            errorMatchContent = match.Value;
                        }
                    }
                    return true;
                }
                else
                {
                    MessageBox.Show("Something went wrong! Couldn't parse the file's contents. Thanks, cya.", "Someone messed up!");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong! Couldn't parse the file's contents. Thanks, cya.", "Someone messed up!");
            }

            return false;
        }

        private Random random;
    }
}