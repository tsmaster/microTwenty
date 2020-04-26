using System;
using System.Collections.Generic;

namespace MicroTwenty
{
    /// <summary>
    /// A factory that determines moves that are legal from this game state. Need not be comprehensive (see portfolio search, for example).
    /// </summary>
    public interface IActionGenerator
    {
        List<ICombatAction> GenerateMoves (WorldRep gameState);
    }
}
