// Scripts/Gameplay/Jobs/Job.cs

using UnityEngine;

public class Job
{
    public JobDriver Driver { get; set; }

    // Данные для задачи
    public Entity TargetA { get; set; } // Основная цель (например, предмет для переноски)
    public Vector2Int TargetB { get; set; } // Вторичная цель (например, точка назначения)

    public Job(Entity targetA = null)
    {
        this.TargetA = targetA;
    }
}
