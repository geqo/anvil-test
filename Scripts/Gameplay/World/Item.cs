// Scripts/Gameplay/World/Item.cs

/// <summary>
/// Представляет собой предмет в мире, который можно подобрать и перенести.
/// </summary>
public class Item : Entity
{
    public override ItemDefinition Def { get; protected set; }
    public int StackCount { get; set; } // Количество предметов в стаке

    public Item(ItemDefinition def, int stackCount = 1)
    {
        Def = def;
        StackCount = stackCount;
    }
}
