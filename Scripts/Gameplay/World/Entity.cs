// Scripts/Gameplay/World/Entity.cs
using UnityEngine;

/// <summary>
/// Абстрактный базовый класс для всего, что может существовать в игровом мире (пешки, предметы, здания).
/// </summary>
public abstract class Entity
{
    // Уникальный ID для этого конкретного экземпляра объекта (не путать с ID из Definition)
    public int UniqueId { get; private set; }
    private static int _nextId = 0;
    public virtual ItemDefinition Def { get; protected set; } // Определение самого предмета
    public ItemDefinition StuffDef { get; set; } // Определение материала (null, если не применимо)

    // Определение объекта (данные из JSON)
    // public virtual EntityDefinition Def { get; } // Пока закомментируем, добавим позже

    // Позиция в мире (в координатах сетки)
    public Vector2Int Position { get; set; }

    protected Entity()
    {
        UniqueId = _nextId++;
    }
}
