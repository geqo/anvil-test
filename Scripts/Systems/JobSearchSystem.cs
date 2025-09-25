using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JobSearchSystem : MonoBehaviour
{
    private List<Pawn> _managedPawns = new List<Pawn>();
    private JobQueue _jobQueue;

    void Start()
    {
        _jobQueue = JobQueue.Instance;
    }

    public void RegisterPawn(Pawn pawn)
    {
        _managedPawns.Add(pawn);
    }

        void Update()
    {
        if (_jobQueue.Jobs.Count == 0) return;
        
        var idlePawn = _managedPawns.FirstOrDefault(p => p.Jobs.IsIdle);
        if (idlePawn == null) return;

        JobTicket ticket = _jobQueue.Jobs.Peek();

        if (CanPawnDoJob(idlePawn, ticket))
        {
            Job newJob = null;
            switch (ticket.JobType)
            {
                case JobDefOf.Cook:
                    if (ModManager.Instance.AllRecipes.ContainsKey(ticket.RecipeId))
                    {
                        // --- ИСПРАВЛЕНИЕ ---
                        // Создаем задачу БЕЗ цели, так как она не нужна для готовки "на месте".
                        newJob = new Job();
                        // Передаем ID рецепта в конструктор исполнителя.
                        newJob.Driver = new JobDriver_Cook(ticket.RecipeId);
                    }
                    break;
                case JobDefOf.Haul:
                    // --- ИСПРАВЛЕНИЕ ---
                    // Создаем задачу, ПЕРЕДАВАЯ ей цели из билета.
                    newJob = new Job(ticket.TargetA, ticket.TargetB);
                    newJob.Driver = new JobDriver_Haul();
                    break;
            }

            if (newJob != null)
            {
                _jobQueue.GetJob();
                newJob.Driver.Job = newJob;
                newJob.Driver.Setup(idlePawn);
                idlePawn.Jobs.StartJob(newJob);
            }
        }
    }

    private bool CanPawnDoJob(Pawn pawn, JobTicket ticket)
    {
        if (pawn.KindDef.pawnType == PawnType.Animal) return false;

        foreach (var trait in pawn.Traits)
        {
            if (trait.disabledJobTypes != null && trait.disabledJobTypes.Contains(ticket.JobType))
            {
                return false;
            }
        }

        if (ticket.RecipeId != null)
        {
            if (!ModManager.Instance.AllRecipes.TryGetValue(ticket.RecipeId, out var recipeDef))
            {
                Debug.LogError($"Билет ссылается на несуществующий рецепт: {ticket.RecipeId}");
                return false;
            }

            if (string.IsNullOrEmpty(recipeDef.skillUsed)) return true;
            if (!pawn.Skills.HasSkill(recipeDef.skillUsed)) return false;
        }

        return true;
    }
}
