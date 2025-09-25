using System.Collections.Generic;
using UnityEngine;

public class JobQueue : MonoBehaviour
{
    // 1. Создаем Singleton для глобального доступа
    public static JobQueue Instance { get; private set; }

    public Queue<Job> Jobs { get; private set; } = new Queue<Job>();

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

    public void AddJob(Job job)
    {
        Jobs.Enqueue(job);
        Debug.Log($"Новая задача добавлена в очередь: {job.JobType}");
    }

    public Job GetJob()
    {
        if (Jobs.Count > 0)
        {
            return Jobs.Dequeue();
        }
        return null;
    }
}
