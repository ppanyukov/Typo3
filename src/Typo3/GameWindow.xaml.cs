using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
using Typo3.Properties;

namespace Typo3
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private Attempt _attempt;

        private ScoreBoard _scores;

        SoundPlayer _soundPlayerHit = new SoundPlayer(@"media\hit.wav");
        SoundPlayer _soundPlayerMiss = new SoundPlayer(@"media\miss.wav");
        SoundPlayer _soundPlayerSkip = new SoundPlayer(@"media\skip.wav");
        SoundPlayer _soundGameOver = new SoundPlayer(@"media\gameover.wav");

        public GameWindow()
        {
            InitializeComponent();


            ToggleFullScreen();
            this.Background = new SolidColorBrush(Colors.WhiteSmoke);

            _scores = new ScoreBoard(Settings.Default.MaxGameTime);
            scoreGrid.DataContext = _scores;
            this.DataContext = _scores;
            _scores.GameOver += _scores_GameOver;
        }

        void _scores_GameOver(object sender, EventArgs e)
        {
            Action a = () =>
            {
                gameDock.Children.Clear();

                Viewbox vb = new Viewbox();
                vb.Stretch = Stretch.Uniform;

                TextBlock t = new TextBlock();
                t.Text = "GAME OVER";
                t.Foreground = new SolidColorBrush(Colors.Red);
                t.FontWeight = FontWeights.UltraBold;

                vb.Child = t;
                gameDock.Children.Add(vb);
                _soundGameOver.Play();
            };

            Dispatcher.BeginInvoke(a);
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
                            _scores.AddTime(Settings.Default.HitTimeReward);
                            _soundPlayerHit.Play();
                        }
                        else
                        {
                            _scores.Misses = _scores.Misses + 1;
                            _scores.RemoveTime(Settings.Default.MissTimePenalty);
                            _soundPlayerMiss.Play();
                        }
                    }
                    else
                    {
                        _scores.Skips = _scores.Skips + 1;
                        _scores.RemoveTime(Settings.Default.SkipTimePenalty);
                        _soundPlayerSkip.Play();
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
            if(Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Enter)
            {
                ToggleFullScreen();
            }
            if(e.Key == Key.Escape)
            {
                this.Close();
            }

        }

        private void Window_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(_attempt == null)
            {
                return;
            }

            if(_scores.IsGameOver)
            {
                return;
            }

            // ignore CTRL
            if(Keyboard.Modifiers == ModifierKeys.Control)
            {
                return;
            }
            if(Keyboard.IsKeyDown(Key.Escape))
            {
                return;
                ToggleFullScreen();
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

        private void ToggleFullScreen()
        {
            if(this.WindowStyle == System.Windows.WindowStyle.None)
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;   
            }
        }
    }
}
