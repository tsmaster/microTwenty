using System;
namespace MicroTwenty
{
    /// <summary>
    /// An action, which transforms a world rep to a new world rep
    /// (that is, returns a child that represents the changes after 
    /// the action has happened).
    /// </summary>
    public interface ICombatAction
    {
        WorldRep Apply (WorldRep before);

        CombatOrder GetCombatOrder (MapManager mapManager, HexMap hexMap);
    }
}
