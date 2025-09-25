using UnityEngine;

public class Pawn_JobTracker
{
    public Job CurrentJob { get; private set; }
    public bool IsIdle => CurrentJob == null;

    public void StartJob(Job job)
    {
        if (!IsIdle)
        {
            Debug.LogWarning("Пешка пытается начать новую работу, не закончив старую!");
            return;
        }
        CurrentJob = job;
        // --- ИСПРАВЛЕНИЕ ---
        Debug.Log($"Пешка {job.Driver.Pawn.Name} начала работу: {job.Driver.JobType}");
    }

    public void FinishJob()
    {
        if (IsIdle) return;
        // --- ИСПРАВЛЕНИЕ ---
        Debug.Log($"Пешка {CurrentJob.Driver.Pawn.Name} закончила работу: {CurrentJob.Driver.JobType}");
        CurrentJob = null;
    }
}
