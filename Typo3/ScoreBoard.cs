using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Typo3
{
    class ScoreBoard : INotifyPropertyChanged
    {
        private int _hits;
        public int Hits
        {
            get { return _hits; }
            set
            {
                _hits = value;
                OnPropertyChanged("Hits");
            }
        }

        private int _misses;
        public int Misses
        {
            get { return _misses; }
            set
            {
                _misses = value;
                OnPropertyChanged("Misses");
            }
        }

        private int _skips;
        public int Skips
        {
            get { return _skips; }
            set
            {
                _skips = value;
                OnPropertyChanged("Skips");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
