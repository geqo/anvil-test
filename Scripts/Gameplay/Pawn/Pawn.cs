using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pawn : Entity
{
    public string Name { get; private set; }
    private static int _pawnCounter = 0;

    // --- ССЫЛКА НА ОПРЕДЕЛЕНИЕ ---
    public PawnKindDefinition KindDef { get; private set; }

    // --- КОМПОНЕНТЫ-ДАННЫЕ ---
    public Pawn_HealthTracker Health { get; private set; }
    public Pawn_NeedsTracker Needs { get; private set; }
    public Pawn_JobTracker Jobs { get; private set; }
    public Pawn_SkillTracker Skills { get; private set; }
    public Pawn_InventoryTracker Inventory { get; private set; }

    // --- ВНЕШНИЕ ДАННЫЕ (ОПРЕДЕЛЕНИЯ) ---
    public List<TraitDefinition> Traits { get; private set; }

    // Конструктор теперь принимает ID вида, а не просто список черт
    public Pawn(string pawnKindId, List<TraitDefinition> assignedTraits)
    {
        if (!ModManager.Instance.AllPawnKinds.TryGetValue(pawnKindId, out var kindDef))
        {
            Debug.LogError($"Не удалось создать пешку: не найден PawnKindDefinition с ID '{pawnKindId}'");
            return;
        }

        _pawnCounter++;
        Name = $"Пешка {_pawnCounter} ({kindDef.id})"; // В имя добавим вид для наглядности
        KindDef = kindDef;
        Traits = assignedTraits;

        // Компоненты-данные теперь могут использовать KindDef для своей инициализации
        Health = new Pawn_HealthTracker(this);
        Needs = new Pawn_NeedsTracker(this);
        Jobs = new Pawn_JobTracker();
        Skills = new Pawn_SkillTracker(this);
        Inventory = new Pawn_InventoryTracker(this);

        Debug.Log($"Создана новая пешка '{Name}' с чертами: {string.Join(", ", Traits.Select(t => t.name))}");
    }

    public bool IsDead()
    {
        return Health.IsDead;
    }

    public string GetStatus()
    {
        if (IsDead()) return $"Пешка {Name} мертва.";
        return $"--- Статус пешки '{Name}' ---\n" +
               $"{Health.GetStatus()}\n" +
               $"{Needs.GetStatus()}" +
               $"{Skills.GetStatus()}";
    }
}
