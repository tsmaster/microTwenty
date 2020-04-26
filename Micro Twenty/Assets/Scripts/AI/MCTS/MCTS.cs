using System;
using System.Collections.Generic;

/// See https://en.wikipedia.org/wiki/Monte_Carlo_tree_search

namespace MicroTwenty
{
    public class MCTS
    {
        public MCTS (WorldRep baseWorldRep, 
            IWorldRepEvaluator evaluator, 
            Dictionary<int, IActionGenerator> moveGenerators)
        {
        }
    }
}
