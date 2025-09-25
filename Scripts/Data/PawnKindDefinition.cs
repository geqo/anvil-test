// Scripts/Data/PawnKindDefinition.cs

using System.Collections.Generic;
using UnityEngine.Rendering;

/// <summary>
/// Определяет "вид" или "расу" существа.
/// Связывает тип (пешка, животное), схему тела и другие базовые параметры.
/// </summary>
public class PawnKindDefinition
{
    public string id;           // "human", "cat"
    public PawnType pawnType;   // Pawn, Animal, Robot
    public string bodyLayoutId; // "humanoid", "catlike"
    public List<string> availableSkills;
    public float baseCarryCapacity = 25f;
}
