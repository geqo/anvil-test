// Scripts/Gameplay/Jobs/JobDriver.cs

/// <summary>
/// Абстрактный класс, содержащий ЛОГИКУ выполнения задачи.
/// </summary>
public abstract class JobDriver
{
    public Pawn Pawn { get; private set; }
    public Job Job { get; set; } // Ссылка на данные о задаче

    public bool IsFinished { get; protected set; }
    public abstract string JobType { get; }

    public virtual void Setup(Pawn worker)
    {
        this.Pawn = worker;
    }

    public abstract void Tick();

    public virtual void OnFinish()
    {
        IsFinished = true;
    }
}
