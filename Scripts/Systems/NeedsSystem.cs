using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NeedsSystem : MonoBehaviour
{
    private List<Pawn> _managedPawns = new List<Pawn>();

    public void RegisterPawn(Pawn pawn)
    {
        _managedPawns.Add(pawn);
    }

    void Update()
    {
        if (_managedPawns.Count == 0) return;

        for (int i = _managedPawns.Count - 1; i >= 0; i--)
        {
            Pawn pawn = _managedPawns[i];

            if (pawn.IsDead())
            {
                _managedPawns.RemoveAt(i);
                continue;
            }

            var needsTracker = pawn.Needs;
            List<string> needIds = new List<string>(needsTracker.Needs.Keys);

            foreach (string needId in needIds)
            {
                NeedDefinition needDef = ModManager.Instance.AllNeeds[needId];
                float baseDecay = needDef.decayPerSecond;

                // Формируем ID стата, используя константы
                string statId = $"{needId}_decay_rate"; // e.g., "hunger_decay_rate"

                float finalDecayRate = StatSystem.CalculateStatValue(pawn, statId, baseDecay);

                needsTracker.Needs[needId] -= finalDecayRate * Time.deltaTime;

                if (needsTracker.Needs[needId] <= 0)
                {
                    needsTracker.Needs[needId] = 0;
                    bool alreadyHasMalnutrition = pawn.Health.GlobalHediffs.Any(h => h is Hediff_Malnutrition mal && mal.CausedByNeed == needId);
                    if (!alreadyHasMalnutrition)
                    {
                        var malnutritionHediff = new Hediff_Malnutrition(pawn, needDef);
                        pawn.Health.AddHediff(malnutritionHediff);
                        Debug.Log($"<color=orange>У пешки {pawn.Name} началось состояние '{malnutritionHediff.Label}'!</color>");
                    }
                }
            }
        }
    }
}
