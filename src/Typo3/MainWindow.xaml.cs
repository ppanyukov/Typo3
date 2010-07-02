using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using SymbolCount;

namespace Typo3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WordList _wordList;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Load word list and start game straight away.
            LoadWordList(null, null);
            StartButtonClick(null, null);
        }



        private void LoadWordList(object sender, RoutedEventArgs e)
        {
            if(!Directory.Exists(inputDir.Text))
            {
                MessageBox.Show(
                    "Selected directory does not exist.", 
                    "Bad thing", 
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            _wordList = new WordList();
            _wordList.Load(inputDir.Text);
            //MessageBox.Show(string.Format(
            //    "Loaded list of {0} words.",
            //    _wordList.WordCounter.WordMap.Count));

            showWordListButton.IsEnabled = true;
            startButton.IsEnabled = true;
        }

        private void ShowWordList(object sender, RoutedEventArgs e)
        {
            var wordListWindow = new WordListWindow();
            wordListWindow.WordList = _wordList;
            wordListWindow.Show();
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            if (_wordList != null)
            {
                var gameWindow = new GameWindow();
                gameWindow.Picker = _wordList.WordPicker;
                gameWindow.ShowActivated = true;
                gameWindow.Show();
            }
        }
    }
}
