﻿using Microsoft.Win32;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JustTypeIt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string ConfigurationFileContent = "";
        Logger logger = LogManager.GetCurrentClassLogger();

        AllVocab allVocab = new AllVocab();
        Word currentWord = new Word();
        private MediaPlayer mediaPlayer = new MediaPlayer();
        bool previousAttemptIncorrect = false;
        public MainWindow()
        {
            InitializeComponent();
            CorrectAnswerTextBox.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fileContent = "";
                var filePath = string.Empty;
                OpenFileDialog openFileDialog = new();
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Semicolon Seperated Values|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == true)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }

                    ConfigurationFileContent = fileContent;
                    if(allVocab.ParseFileContent(ConfigurationFileContent))
                    {
                        loadConfigButton.Visibility = Visibility.Collapsed;
                        MessageBox.Show("Parsing successful.", ":)");
                        currentWord = allVocab.GetNextWord();
                        WordTextBox.Text = currentWord.OriginalWord;
                        AnswerTextBox.Text = "";
                        TotalWordCountBlock.Text = allVocab.Words.Count.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong! Couldn't read that file. Thanks, bye." + ex, "Someone messed up!");
            }
        }

        private async void AnswerTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                if (allVocab.Words.Count > 0)
                {
                    AnswerTextBox.IsReadOnly = true;


                    if (allVocab.Check(AnswerTextBox.Text))
                    { // correct answer
                        HardWordCountBlock.Text = allVocab.GetHardWordCount().ToString();
                        EasyWordCountBlock.Text = allVocab.GetEasyWordCount().ToString();
                        EasyWordPercentageBlock.Text = allVocab.GetEasyWordsPercentage().ToString().Truncate(5);

                        if (CorrectAnswerTextBox.Visibility == Visibility.Visible)
                        {
                            CorrectAnswerTextBox.Visibility = Visibility.Hidden;
                        }
                        RecentErrorsBlock.Text = currentWord.RecentErrors.ToString();
                        if(currentWord.RecentErrors <= 0)
                        {
                            RecentErrorsBlock.Background = new SolidColorBrush(Color.FromRgb(1, 107, 1));
                        }
                        else
                        {
                            RecentErrorsBlock.Background = new SolidColorBrush(Color.FromRgb(161, 21, 0));
                        }

                        AnswerTextBox.Background = new SolidColorBrush(Color.FromRgb(21, 84, 2));
                        mediaPlayer.Volume = 0.75;
                        mediaPlayer.Open(new Uri("otlichnо.mp3", UriKind.Relative));
                        mediaPlayer.Play();
                        await Task.Delay(1000);
                        AnswerTextBox.Text = "";
                        AnswerTextBox.IsReadOnly = false;
                        currentWord =  allVocab.GetNextWord();
                        WordTextBox.Text = currentWord.OriginalWord;
                        RecentErrorsBlock.Text = currentWord.RecentErrors.ToString();

                        if (currentWord.RecentErrors <= 0)
                        {
                            RecentErrorsBlock.Background = new SolidColorBrush(Color.FromRgb(1, 107, 1));
                        }
                        else
                        {
                            RecentErrorsBlock.Background = new SolidColorBrush(Color.FromRgb(161, 21, 0));
                        }
                        AnswerTextBox.Background = new SolidColorBrush(Color.FromRgb(2, 43, 117));

                        if (currentWord.IsWellKnown())
                        {
                            WordTextBox.BorderBrush = new SolidColorBrush(Colors.Aquamarine);
                        }
                        else if(currentWord.RecentErrors >= 2)
                        {
                            WordTextBox.BorderBrush = new SolidColorBrush(Colors.Yellow);
                        }
                        else
                        {
                            WordTextBox.BorderBrush = new SolidColorBrush(Colors.Gray);
                        }
                    }
                    else
                    { // incorrect answer
                        HardWordCountBlock.Text = allVocab.GetHardWordCount().ToString();
                        EasyWordCountBlock.Text = allVocab.GetEasyWordCount().ToString();
                        EasyWordPercentageBlock.Text = allVocab.GetEasyWordsPercentage().ToString().Truncate(5);

                        if (previousAttemptIncorrect == false) // unused variable??
                        {
                            previousAttemptIncorrect = true;
                        }
                        mediaPlayer.Volume = 0.5;
                        mediaPlayer.Open(new Uri("sadtrombone.mp3", UriKind.Relative));
                        mediaPlayer.Play();
                        RecentErrorsBlock.Text = currentWord.RecentErrors.ToString();
                        RecentErrorsBlock.Background = new SolidColorBrush(Color.FromRgb(161, 21, 0));
                        await Task.Delay(1000);
                        AnswerTextBox.Background = new SolidColorBrush(Color.FromRgb(161, 21, 21));
                        CorrectAnswerTextBox.Text = allVocab.CurrentWord.Definition;
                        CorrectAnswerTextBox.Visibility = Visibility.Visible;
                        AnswerTextBox.IsReadOnly = false;
                    }
                }
            }
        }
    }
}