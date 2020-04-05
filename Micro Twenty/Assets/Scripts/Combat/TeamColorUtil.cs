using System;
namespace MicroTwenty
{
    public static class TeamColorUtil
    {
        public static UnityEngine.Color GetColorForTeam (int teamId)
        {
            switch (teamId) {
            case 0:
                return UnityEngine.Color.cyan;
            case 1:
                return UnityEngine.Color.red;
            default:
                return UnityEngine.Color.magenta;
            }
        }
    }
}
