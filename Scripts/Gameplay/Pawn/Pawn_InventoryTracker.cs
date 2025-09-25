using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Управляет инвентарем и предметами в руках пешки.
/// </summary>
public class Pawn_InventoryTracker
{
    private Pawn _pawn;
    private List<Item> _inventoryItems;

    // --- ПУБЛИЧНЫЕ СВОЙСТВА ---
    public Item CarriedInHands { get; private set; }
    public IReadOnlyList<Item> InventoryItems => _inventoryItems.AsReadOnly();

    // --- ВЫЧИСЛЯЕМЫЕ СВОЙСТВА ---
    public bool IsCarryingAnything => CarriedInHands != null || _inventoryItems.Count > 0;
    public float CurrentInventoryWeight => _inventoryItems.Sum(item => item.Def.weight * item.StackCount);
    public float MaxInventoryWeight => StatSystem.CalculateStatValue(_pawn, StatDefOf.CarryCapacity, _pawn.KindDef.baseCarryCapacity);

    public Pawn_InventoryTracker(Pawn pawn)
    {
        _pawn = pawn;
        _inventoryItems = new List<Item>();
    }

    /// <summary>
    /// Пытается добавить предмет в основной инвентарь (рюкзак).
    /// </summary>
    public bool TryAddItemToInventory(Item item)
    {
        if (CurrentInventoryWeight + item.Def.weight * item.StackCount > MaxInventoryWeight)
        {
            // Не хватает места по весу
            return false;
        }

        // (В будущем здесь будет логика объединения стаков)
        _inventoryItems.Add(item);
        Debug.Log($"Предмет {item.Def.name} x{item.StackCount} добавлен в инвентарь пешки {_pawn.Name}");
        return true;
    }

    /// <summary>
    /// Пешка берет предмет в руки (например, оружие или временный предмет для переноски).
    /// </summary>
    public bool TryPickupItemToHands(Item item)
    {
        if (CarriedInHands != null) return false;
        CarriedInHands = item;
        return true;
    }

    /// <summary>
    /// Бросает предмет из рук.
    /// </summary>
    public Item DropItemFromHands()
    {
        if (CarriedInHands == null) return null;
        Item itemToDrop = CarriedInHands;
        CarriedInHands = null;
        return itemToDrop;
    }

    /// <summary>
    /// Выбрасывает все предметы из инвентаря (для склада).
    /// </summary>
    public List<Item> DropAllInventory()
    {
        var items = new List<Item>(_inventoryItems);
        _inventoryItems.Clear();
        return items;
    }
}
