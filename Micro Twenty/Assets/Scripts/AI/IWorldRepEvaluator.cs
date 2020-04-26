using System;
namespace MicroTwenty
{
    public interface IWorldRepEvaluator
    {
        float Evaluate (WorldRep node);
        bool IsGameOver (WorldRep worldRep);
    }
}
