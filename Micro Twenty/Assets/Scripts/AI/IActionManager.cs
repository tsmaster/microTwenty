using System;
namespace MicroTwenty
{
    public interface IActionManager
    {
        int GetMovingPlayer (WorldRep baseEnvironment);
        bool IsMaximising (int movingPlayer);
    }
}
