// Scripts/Gameplay/World/Building.cs

using UnityEditor.SceneManagement;


/// <summary>
/// Представляет собой установленный, стационарный объект на карте (стена, станок, мебель).
/// </summary>
public class Building : Entity
{
    public override ItemDefinition Def { get; protected set; }
    public StorageComponent Storage { get; private set; }

    public Building(ItemDefinition def)
    {
        Def = def;
        if (def.storage != null)
        {
            Storage = new StorageComponent(def.storage.maxWeight);
        }
    }

    // В будущем здесь может быть логика, специфичная для зданий,
    // например, компонент для хранения энергии, компонент для хранения предметов и т.д.
}
