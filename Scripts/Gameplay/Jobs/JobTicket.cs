// Scripts/Gameplay/Jobs/JobTicket.cs

using UnityEngine;


/// <summary>
/// Простой "билет" или "заявка" на работу, которая помещается в глобальную очередь.
/// Содержит только информацию, необходимую для создания конкретного экземпляра Job.
/// </summary>
public class JobTicket
{
    public string JobType { get; private set; } // "Cook", "Haul". ОБЯЗАТЕЛЬНОЕ ПОЛЕ.
    public string RecipeId { get; private set; }

    // В будущем здесь будут цели:
    public Entity TargetA; // Предмет для переноски
    public Vector2Int TargetB; // Место назначения

    /// <summary>
    /// Конструктор для работ, основанных на рецепте.
    /// </summary>
    public JobTicket(string jobType, string recipeId)
    {
        JobType = jobType;
        RecipeId = recipeId;
    }

    /// <summary>
    /// Конструктор для работ с миром (у которых нет рецепта).
    /// </summary>
    public JobTicket(string jobType/*, object target = null*/) // Цель пока закомментирована
    {
        JobType = jobType;
        RecipeId = null;
    }
}
