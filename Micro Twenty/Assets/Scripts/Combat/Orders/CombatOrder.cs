using System;
namespace MicroTwenty
{
    public interface CombatOrder
    {
        bool IsDone ();

        void Update (float deltaSeconds);

        void Draw ();

        CombatUnit GetCombatUnit ();
    }
}
