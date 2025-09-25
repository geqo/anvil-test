using System.Collections.Generic;
using UnityEngine;

public class JobExecutionSystem : MonoBehaviour
{
    private List<Pawn> _managedPawns = new List<Pawn>();

    public void RegisterPawn(Pawn pawn)
    {
        _managedPawns.Add(pawn);
    }

    void Update()
    {
        foreach (var pawn in _managedPawns)
        {
            if (pawn.Jobs.IsIdle)
            {
                continue;
            }

            // --- ИСПРАВЛЕНИЕ ---
            // Получаем исполнителя (Driver) из текущей задачи
            JobDriver currentDriver = pawn.Jobs.CurrentJob.Driver;

            // Выполняем "тик" логики исполнителя
            currentDriver.Tick();

            // Проверяем, сообщил ли исполнитель о завершении
            if (currentDriver.IsFinished)
            {
                pawn.Jobs.FinishJob();
            }
        }
    }
}
