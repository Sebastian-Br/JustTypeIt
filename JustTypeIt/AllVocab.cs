﻿using NLog;
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
            EasyWords = new();
            random = new();
        }
        public List<Word> Words { get; set; }

        public List<Word> HardWords { get; set; }

        public List<Word> EasyWords { get; set; }

        public Word RecentWord { get; set; }
        public Word CurrentWord { get; set; }

        public bool Check(string studentAnswer)
        {
            try
            {
                string studentAnswerGeneralized = studentAnswer.Trim();
                studentAnswerGeneralized = GenericString.RemoveSpacesAroundCommata(studentAnswerGeneralized);
                if (String.Equals(CurrentWord.DefinitionGeneralized, studentAnswerGeneralized, StringComparison.OrdinalIgnoreCase))
                {
                    CurrentWord.AddSuccess();
                    if(CurrentWord.IsWellKnown())
                    {
                        if(!EasyWords.Contains(CurrentWord))
                        {
                            EasyWords.Add(CurrentWord);
                        }
                    }

                    if (CurrentWord.RecentErrors <= 0)
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
                    if(!CurrentWord.IsWellKnown())
                    {
                        if (EasyWords.Contains(CurrentWord))
                        {
                            EasyWords.Remove(CurrentWord);
                        }
                    }

                    if(CurrentWord.RecentErrors > 0)
                    {
                        if (!HardWords.Contains(CurrentWord))
                        {
                            HardWords.Add(CurrentWord);
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        public Word GetNextWord()
        {
            try
            {
                if(HardWords.Count == 1 && (random.Next() % 2) == 0)
                {
                    CurrentWord = HardWords[0];
                    return CurrentWord;
                }
                else if (HardWords.Count > 1 && (random.Next() % 2) == 0)
                {
                    int index = random.Next(HardWords.Count);
                    CurrentWord = HardWords[index];
                    return CurrentWord;
                }
                else
                {
                    int reroll_times = 1;

                    for(int i = 0; i < reroll_times; i++)
                    {
                        int index = random.Next(Words.Count);
                        CurrentWord = Words[index];
                        if (EasyWords.Contains((Word)CurrentWord))
                        {
                            //reroll
                        }
                        else
                        {
                            break;
                        }
                    }

                    return CurrentWord;
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex);
                return null;
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
                            newWord.DefinitionGeneralized = newWord.Definition.Trim();
                            newWord.DefinitionGeneralized = GenericString.RemoveSpacesAroundCommata(newWord.DefinitionGeneralized);
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