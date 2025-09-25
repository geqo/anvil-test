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

        // Оптимизация: ищем только одну свободную пешку, а не создаем целый список
        var idlePawn = _managedPawns.FirstOrDefault(p => p.Jobs.IsIdle);
        if (idlePawn == null) return;

        JobTicket ticket = _jobQueue.Jobs.Peek();

        if (CanPawnDoJob(idlePawn, ticket))
        {
            Job newJob = null;
            switch (ticket.JobType)
            {
                case JobDefOf.Cook:
                    // --- ИСПРАВЛЕНИЕ ---
                    // Проверяем, что рецепт существует ПЕРЕД созданием задачи
                    if (ModManager.Instance.AllRecipes.ContainsKey(ticket.RecipeId))
                    {
                        newJob = new Job(null); // Цели пока нет, пешка будет работать "на месте"
                        newJob.Driver = new JobDriver_Cook(ticket.RecipeId);
                    }
                    break;
                case JobDefOf.Haul:
                    newJob = new Job(ticket.TargetA); // Передаем цель в задачу
                    newJob.TargetB = ticket.TargetB;
                    newJob.Driver = new JobDriver_Haul();
                    break;
                case JobDefOf.Build:
                    if (ModManager.Instance.AllRecipes.ContainsKey(ticket.RecipeId))
                    {
                        newJob = new Job(null);
                        newJob.Driver = new JobDriver_Build(ticket.RecipeId);
                    }
                    break;
                case JobDefOf.Smith:
                    if (ModManager.Instance.AllRecipes.ContainsKey(ticket.RecipeId))
                    {
                        newJob = new Job(null);
                        newJob.Driver = new JobDriver_Smith(ticket.RecipeId);
                    }
                    break;
            }

            if (newJob != null)
            {
                _jobQueue.GetJob(); // Убираем билет из очереди

                // Передаем пешку и задачу в исполнителя
                newJob.Driver.Job = newJob;
                newJob.Driver.Setup(idlePawn);

                // Назначаем пешке полностью собранную задачу
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
