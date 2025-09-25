using System.Collections.Generic;
using System.Linq;

public class StorageComponent
{
    private List<Item> _storedItems = new List<Item>();
    public float MaxWeight { get; private set; }
    public float CurrentWeight => _storedItems.Sum(i => i.Def.weight * i.StackCount);

    public StorageComponent(float maxWeight)
    {
        MaxWeight = maxWeight;
    }

    public bool TryAddItem(Item item)
    {
        if (CurrentWeight + item.Def.weight * item.StackCount > MaxWeight)
        {
            return false;
        }
        // (Логика объединения стаков)
        _storedItems.Add(item);
        return true;
    }
}
