// Job.cs
public class Job
{
    public string JobType { get; private set; } // "CookMeal", "HaulItem"
    // Дополнительные данные, например, цель (плита), ингредиенты и т.д.

    public Job(string jobType)
    {
        JobType = jobType;
    }
}
