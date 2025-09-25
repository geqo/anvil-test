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

        // Используем for, чтобы можно было безопасно удалять мертвых пешек из списка
        for (int i = _managedPawns.Count - 1; i >= 0; i--)
        {
            Pawn pawn = _managedPawns[i];

            // 1. Проверяем, жива ли пешка, и удаляем из списка, если нет.
            if (pawn.IsDead())
            {
                _managedPawns.RemoveAt(i);
                continue;
            }

            // Получаем компонент потребностей
            var needsTracker = pawn.Needs;
            List<string> needIds = new List<string>(needsTracker.Needs.Keys);

            foreach (string needId in needIds)
            {
                NeedDefinition needDef = ModManager.Instance.AllNeeds[needId];

                // 2. Рассчитываем модификаторы от черт характера
                float decayMultiplier = 1f;
                string statToModify = $"{needId}_decay_rate"; // e.g., "hunger_decay_rate"

                foreach (var trait in pawn.Traits)
                {
                    foreach (var modifier in trait.modifiers)
                    {
                        if (modifier.stat == statToModify && modifier.op == "multiply")
                        {
                            decayMultiplier *= modifier.value;
                        }
                    }
                }

                // 3. Применяем изменение потребности
                needsTracker.Needs[needId] -= needDef.decayPerSecond * decayMultiplier * Time.deltaTime;

                // 4. Логика последствий при достижении нуля
                if (needsTracker.Needs[needId] <= 0)
                {
                    needsTracker.Needs[needId] = 0;

                    // Вместо убийства, добавляем Hediff (например, "Голодание")
                    // Проверяем, нет ли у пешки уже такого состояния
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
