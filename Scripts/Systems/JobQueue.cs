using System.Collections.Generic;
using UnityEngine;

public class JobQueue : MonoBehaviour
{
    // 1. Создаем Singleton для глобального доступа
    public static JobQueue Instance { get; private set; }

    public Queue<JobTicket> Jobs { get; private set; } = new Queue<JobTicket>();

    void Awake()
    {
        // 2. Настраиваем Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddJob(JobTicket jobTicket)
    {
        Jobs.Enqueue(jobTicket);
        Debug.Log($"Новая задача (рецепт: {jobTicket.RecipeId}) добавлена в очередь.");
    }

    public JobTicket GetJob()
    {
        if (Jobs.Count > 0)
        {
            return Jobs.Dequeue();
        }
        return null;
    }
}
