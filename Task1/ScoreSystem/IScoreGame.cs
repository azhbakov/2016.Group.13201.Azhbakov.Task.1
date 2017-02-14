using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task1 {
    internal interface IScoreGame {
        void SubscribeToNewScore (Action<int> reaction);
    }
}
