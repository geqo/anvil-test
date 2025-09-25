// Scripts/Data/YieldedItem.cs
using UnityEngine;

/// <summary>
/// Вспомогательный класс для описания ресурса, который выпадает
/// при добыче (mining) или разборке (deconstruction).
/// </summary>
[System.Serializable] // Позволяет Unity корректно обрабатывать этот класс
public class YieldedItem
{
    public string itemId; // ID предмета, который выпадает (например, "resource_wood")
    public int minAmount; // Минимальное количество
    public int maxAmount; // Максимальное количество
}
