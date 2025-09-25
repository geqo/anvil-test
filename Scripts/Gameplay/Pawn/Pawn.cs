using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Pawn теперь является "тупым" контейнером данных, связывающим все свои компоненты.
public class Pawn
{
    // --- ИДЕНТИФИКАЦИЯ ---
    public string Name { get; private set; }
    private static int _pawnCounter = 0;

    // --- КОМПОНЕНТЫ-ДАННЫЕ ---
    public Pawn_HealthTracker Health { get; private set; } // Отвечает за здоровье
    public Pawn_NeedsTracker Needs { get; private set; }   // Отвечает за потребности
    // Сюда в будущем можно будет добавить Pawn_JobTracker, Pawn_MindStateTracker и т.д.

    // --- ВНЕШНИЕ ДАННЫЕ (ОПРЕДЕЛЕНИЯ) ---
    public List<TraitDefinition> Traits { get; private set; }
    public Dictionary<string, int> Skills { get; private set; }

    public Pawn(List<TraitDefinition> assignedTraits)
    {
        _pawnCounter++;
        Name = $"Пешка {_pawnCounter}";
        Traits = assignedTraits;

        // Инициализируем компоненты-данные
        Health = new Pawn_HealthTracker(this);
        Needs = new Pawn_NeedsTracker(this);

        // Инициализируем навыки (пока что заглушка)
        Skills = new Dictionary<string, int> { { "cooking", 1 } };

        Debug.Log($"Создана новая пешка '{Name}' с чертами: {string.Join(", ", Traits.Select(t => t.name))}");
    }

    // --- ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ---
    public bool IsDead()
    {
        // Теперь смерть определяется состоянием здоровья
        return Health.IsDead;
    }

    public string GetStatus()
    {
        if (IsDead()) return $"Пешка {Name} мертва.";

        // Собираем статусы из всех компонентов
        return $"--- Статус пешки '{Name}' ---\n" +
               $"{Health.GetStatus()}\n" +
               $"{Needs.GetStatus()}";
    }
}
