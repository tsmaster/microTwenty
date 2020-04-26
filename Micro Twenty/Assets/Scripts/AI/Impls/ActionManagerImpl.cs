namespace MicroTwenty
{
    internal class ActionManagerImpl : IActionManager
    {
        public int GetMovingPlayer (WorldRep worldRep)
        {
            // find the unit with the highest initiative

            var movingUnitIndex = worldRep.GetIndexOfMovingUnit ();

            // return that unit's team

            return worldRep.Chars [movingUnitIndex].TeamIndex;
        }

        public bool IsMaximising (int movingPlayer)
        {
            return movingPlayer == 0;
        }
    }
}