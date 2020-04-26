using System;
using System.Collections.Generic;

namespace MicroTwenty
{
    public class WorldRepEvaluatorImpl : IWorldRepEvaluator
    {
        public float Evaluate (WorldRep node)
        {
            if (IsGameOver (node)) {
                return EvaluateGameOver (node);
            }

            float p0Health = 0;
            float p1Health = 0;

            foreach (var c in node.Chars) {
                var curHealth = c.CurrentHealth;
                var maxHealth = c.MaxHealth;

                float healthFrac = ((float) curHealth) / maxHealth;

                if (c.TeamIndex == 0) {
                    p0Health += healthFrac;
                } else {
                    p1Health += healthFrac;
                }
            }
            return p0Health - p1Health;
        }

        private float EvaluateGameOver (WorldRep node)
        {
            float p0Health = 0;
            float p1Health = 0;

            foreach (var c in node.Chars) {
                var healthVal = (c.CurrentHealth > 0) ? 1 : 0;

                if (c.TeamIndex == 0) {
                    p0Health += healthVal;
                } else {
                    p1Health += healthVal;
                }
            }
            return p0Health - p1Health;
        }

        public bool IsGameOver (WorldRep node)
        {
            Dictionary<int, bool> teamHasUnits = new Dictionary<int, bool> {
                [0] = false,
                [1] = false
            };
            foreach (var c in node.Chars) {
                if (c.IsAlive()) {
                    teamHasUnits [c.TeamIndex] = true;
                }
            }

            return !(teamHasUnits [0] && teamHasUnits [1]);
        }
    }
}
