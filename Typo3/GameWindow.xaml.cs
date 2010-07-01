﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using SymbolCount;

namespace Typo3
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private Attempt _attempt;

        private ScoreBoard _scores = new ScoreBoard();

        public GameWindow()
        {
            InitializeComponent();

            scoreGrid.DataContext = _scores;
        }

        public WordPicker Picker{ get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NewAttempt();
        }


        private void NewAttempt()
        {
            var nextWord = Picker.NextWord;
            NewAttempt(nextWord);
        }

        private void NewAttempt(string nextWord)
        {
            
            Action a = () =>
            {
                if (_attempt != null)
                {
                    if (_attempt.IsFinished)
                    {
                        if (_attempt.IsGood)
                        {
                            _scores.Hits = _scores.Hits + 1;
                        }
                        else
                        {
                            _scores.Misses = _scores.Misses + 1;
                        }
                    }
                    else
                    {
                        _scores.Skips = _scores.Skips + 1;
                    }

                }

                _attempt = null;
                boardBox.Text = nextWord;
                boardBox.Foreground = new SolidColorBrush(Colors.Black);
                inputBox.Text = string.Empty;
                _attempt = new Attempt(nextWord);
            };

            Dispatcher.BeginInvoke(a);
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {

        }

        private void Window_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(_attempt == null)
            {
                return;
            }

            if(e.Text == "\r")
            {
                NewAttempt();
                return;
            }

            if (e.Text == " ")
            {
                if(_attempt.IsGood)
                {
                    NewAttempt();                    
                }
                else
                {
                    NewAttempt(_attempt.OriginalText);
                }
            }

            if (!_attempt.IsFinished)
            {
                _attempt.AddText(e.Text);
                inputBox.Text = _attempt.UserString;
                if (_attempt.IsFinished)
                {
                    if (_attempt.IsGood)
                    {
                        boardBox.Foreground = new SolidColorBrush(Colors.DarkGreen);
                    }
                    else
                    {
                        boardBox.Foreground = new SolidColorBrush(Colors.Tomato);
                    }
                }
            }
        }
    }
}
