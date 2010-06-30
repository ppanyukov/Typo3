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
        public MainWindow()
        {
            InitializeComponent();
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

            var workList = new WordList();
            workList.Load(inputDir.Text);
            MessageBox.Show(workList.WordCounter.WordMap.Count.ToString());
        }
    }
}
