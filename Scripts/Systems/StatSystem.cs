using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// "Умная" статическая система для расчета конечных значений характеристик (статов) пешки.
/// Собирает базовое значение и применяет все модификаторы от черт, болезней, одежды и т.д.
/// </summary>
public static class StatSystem
{
    /// <summary>
    /// Рассчитывает конечное значение стата для указанной пешки.
    /// </summary>
    /// <param name="pawn">Пешка, для которой производится расчет.</param>
    /// <param name="statId">Уникальный идентификатор стата (например, "hunger_decay_rate").</param>
    /// <param name="baseValue">Базовое значение стата (например, из NeedDefinition).</param>
    /// <returns>Итоговое значение стата после применения всех модификаторов.</returns>
    public static float CalculateStatValue(Pawn pawn, string statId, float baseValue)
    {
        float finalValue = baseValue;

        // --- Этап 1: Модификаторы от черт характера ---
        // Сначала применяем модификаторы, которые добавляют значение
        foreach (var trait in pawn.Traits)
        {
            if (trait.modifiers == null) continue;

            foreach (var modifier in trait.modifiers)
            {
                if (modifier.stat == statId && modifier.op == StatDefOf.OpAdd)
                {
                    finalValue += modifier.value;
                }
            }
        }

        // Затем применяем модификаторы, которые умножают значение
        foreach (var trait in pawn.Traits)
        {
            if (trait.modifiers == null) continue;

            foreach (var modifier in trait.modifiers)
            {
                if (modifier.stat == statId && modifier.op == StatDefOf.OpMultiply)
                {
                    finalValue *= modifier.value;
                }
            }
        }

        // --- Этап 2: Модификаторы от Hediffs (ранений, болезней) ---
        // (Здесь в будущем можно будет добавить логику)
        // Пример: ранение ноги может накладывать множитель 0.5 на стат "move_speed"

        // --- Этап 3: Модификаторы от экипировки ---
        // (Здесь в будущем можно будет добавить логику)

        return finalValue;
    }
}
