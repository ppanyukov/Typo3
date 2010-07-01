using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SymbolCount;

namespace Typo3
{
    /// <summary>
    /// Interaction logic for WordListWindow.xaml
    /// </summary>
    public partial class WordListWindow : Window
    {
        public WordListWindow()
        {
            InitializeComponent();

        }


        private WordList _wordList;
        public WordList WordList
        {
            get { return _wordList; }
            set
            {
                _wordList = value;
                wordList.DataContext = _wordList.WordCounter.WordMap.OrderByDescending((x) => x.Value);
            }
        }
    }
}
