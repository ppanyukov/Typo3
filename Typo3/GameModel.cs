using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Typo3
{
    class GameModel
    {
        private IList<string> inputWords;

    }

    class Attempt
    {
        public string OriginalText { get; private set; }
        public string UserInput { get; private set; }
        public int TimeInTicks { get; private set; }

    }
}
