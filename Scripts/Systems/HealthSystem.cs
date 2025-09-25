using System.Collections.Generic;
using UnityEngine;

// "Умная" система, которая обрабатывает логику здоровья для всех пешек.
public class HealthSystem : MonoBehaviour
{
    // Список всех живых пешек в мире (чтобы не проверять мертвых)
    private List<Pawn> _managedPawns = new List<Pawn>();

    public void RegisterPawn(Pawn pawn)
    {
        _managedPawns.Add(pawn);
    }

    void Update()
    {
        // Проходим в обратном порядке, чтобы можно было безопасно удалять мертвых из списка
        for (int i = _managedPawns.Count - 1; i >= 0; i--)
        {
            Pawn pawn = _managedPawns[i];

            // Получаем компонент здоровья
            var healthTracker = pawn.Health;

            // --- Обработка Hediff'ов и их эффектов ---
            // 1. Глобальные Hediff'ы (болезни)
            foreach (var hediff in healthTracker.GlobalHediffs)
            {
                hediff.Tick(); // Выполняем логику болезни
                healthTracker.BloodLevel -= hediff.BleedRate * Time.deltaTime;
            }

            // 2. Локальные Hediff'ы (раны на частях тела)
            foreach (var part in healthTracker.BodyParts.Values)
            {
                foreach (var hediff in part.Hediffs)
                {
                    hediff.Tick(); // Выполняем логику раны
                    healthTracker.BloodLevel -= hediff.BleedRate * Time.deltaTime;
                }
            }

            // --- Проверка на смерть ---
            healthTracker.CheckForDeath();
            if (healthTracker.IsDead)
            {
                _managedPawns.RemoveAt(i); // Убираем мертвую пешку из списка активных
            }
        }
    }
}
