using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Timers;

namespace Typo3
{
    class ScoreBoard : INotifyPropertyChanged
    {

        private Timer _timer;

        public ScoreBoard() :this(new TimeSpan(0,1,0))
        {
        }

        public ScoreBoard(TimeSpan maxGameTime)
        {
            _maxGameTime = maxGameTime;
            _timer = new Timer();
            _timer.AutoReset = false;
            _timer.Interval = 1000;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        void  _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
 	        OnPropertyChanged("TimeLeft");
            if(TimeLeft.TotalMilliseconds <= 0)
            {
                _timer.Stop();
                if(GameOver != null)
                {
                    GameOver(this, EventArgs.Empty);
                }
            }
            else
            {
                _timer.Start();
            }
        }

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


        private TimeSpan _maxGameTime;

        public TimeSpan TimeLeft
        {
            get
            {
                var elapsedTime = (DateTime.Now - StartTime);
                var res = _maxGameTime - elapsedTime;
                if(res.Ticks <= 0)
                {
                    return new TimeSpan(0);
                }
                else
                {
                    return new TimeSpan(res.Days, res.Hours, res.Minutes, res.Seconds);
                }
            }
        }


        public event EventHandler GameOver;

        private DateTime _startTime = DateTime.Now;
        public DateTime StartTime
        {
            get { return _startTime; }
        }

        public bool IsGameOver
        {
            get
            {
                return TimeLeft.Ticks <= 0;
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

        public void AddTime(TimeSpan timeSpan)
        {
            _maxGameTime += timeSpan;
        }

        public void RemoveTime(TimeSpan timeSpan)
        {
            _maxGameTime -= timeSpan;
        }
    }
}
