using System;
using System.Windows.Input;

namespace Typo3
{
    class Attempt
    {
        private string _userString = string.Empty;
        private DateTime _endTime = DateTime.MaxValue;
        private bool _isFinished;
        private bool _isGood = true;

        public Attempt(string originalText)
        {
            OriginalText = originalText;
            StartTime = DateTime.Now;
        }

        public string OriginalText { get; private set; }
        public DateTime StartTime { get; private set; }

        public bool IsFinished
        {
            get { return _isFinished; }
        }

        public DateTime EndTime
        {
            get
            {
                if(IsFinished)
                {
                    return _endTime;
                }
                else
                {
                    return DateTime.Now;
                }
            }
        }

        public bool IsGood
        {
            get { return _isGood; }
        }

        public void AddText(string text)
        {
            if (IsFinished)
            {
                return;
            }


            var newString = _userString + text;
            if (OriginalText.StartsWith(newString, StringComparison.CurrentCulture))
            {
                if (newString.Length == OriginalText.Length)
                {
                    _isFinished = true;
                    _isGood = true;
                }
            }
            else
            {
                _isFinished = true;
                _isGood = false;
            }

            _userString = newString;
            if(IsFinished)
            {
                _endTime = DateTime.Now;
            }
        }

    }
}