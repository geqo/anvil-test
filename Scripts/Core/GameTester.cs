using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Тестовый класс для запуска симуляции.
/// Создает начальных пешек и животных, регистрирует их в системах
/// и выводит их статус в консоль для отладки.
/// </summary>
public class GameTester : MonoBehaviour
{
    private List<Pawn> _worldPawns = new List<Pawn>();
    private NeedsSystem _needsSystem;
    private HealthSystem _healthSystem;
    private JobSearchSystem _jobSearchSystem;
    private JobExecutionSystem _jobExecutionSystem;
    private float _statusUpdateTimer = 0f;
    private const float STATUS_UPDATE_INTERVAL = 2f;
    private Map _currentMap;

    void Start()
    {
        _currentMap = new Map(50, 50); // Создаем карту 50x50 клеток
        _needsSystem = FindFirstObjectByType<NeedsSystem>();
        _healthSystem = FindFirstObjectByType<HealthSystem>();
        _jobSearchSystem = FindFirstObjectByType<JobSearchSystem>();
        _jobExecutionSystem = FindFirstObjectByType<JobExecutionSystem>();

        if (_needsSystem == null || _healthSystem == null || _jobSearchSystem == null || _jobExecutionSystem == null)
        {
            Debug.LogError("На сцене отсутствуют NeedsSystem или HealthSystem! Не могу запустить тест.");
            return;
        }

        Debug.Log("--- GameTester: Начало симуляции ---");

        var human = CreateAndRegisterPawn(PawnKindDefOf.Humanlike, new List<string> { "tough", "workaholic" });
        var catlike = CreateAndRegisterPawn(PawnKindDefOf.Catlike, new List<string> { "gourmand" });
        var animal = CreateAndRegisterPawn(PawnKindDefOf.AnimalCat, new List<string> { "fast_walker" });

        _currentMap.SpawnEntity(human, new Vector2Int(10, 10));
        _currentMap.SpawnEntity(catlike, new Vector2Int(12, 10));
        _currentMap.SpawnEntity(animal, new Vector2Int(10, 12));

        var rawFoodDef = ModManager.Instance.AllItems["raw_food"];
        var foodStack = new Item(rawFoodDef, 20); // Стак из 20 единиц сырой еды
        _currentMap.SpawnEntity(foodStack, new Vector2Int(15, 15));

        JobQueue.Instance.AddJob(new JobTicket("cook_simple_meal"));
        Debug.Log("Добавлена задача 'Приготовить еду' в глобальную очередь.");
        JobQueue.Instance.AddJob(new JobTicket(JobDefOf.Haul));
        Debug.Log("Добавлена задача переноски в глобальную очередь.");
    }

    void Update()
    {
        _statusUpdateTimer += Time.deltaTime;
        if (_statusUpdateTimer >= STATUS_UPDATE_INTERVAL)
        {
            _statusUpdateTimer = 0f;
            Debug.Log("----------------- ОБНОВЛЕНИЕ СТАТУСА -----------------");
            foreach (var pawn in _worldPawns)
            {
                if (pawn != null) // Добавлена проверка на случай ошибки создания
                {
                    Debug.Log(pawn.GetStatus());
                }
            }
        }
    }

    /// <summary>
    /// Вспомогательный метод для создания, регистрации и добавления пешки в мир.
    /// </summary>
    private Pawn CreateAndRegisterPawn(string pawnKindId, List<string> traitIds)
    {
        // Собираем определения черт из ModManager по их ID
        List<TraitDefinition> traits = new List<TraitDefinition>();
        foreach (string id in traitIds)
        {
            if (ModManager.Instance.AllTraits.TryGetValue(id, out TraitDefinition traitDef))
            {
                traits.Add(traitDef);
            }
        }

        // Создаем новый экземпляр Pawn
        var pawn = new Pawn(pawnKindId, traits);

        // Если пешка не смогла создаться (например, из-за неверного ID), выходим
        if (pawn.KindDef == null) return null;

        // Добавляем его в наш список "жителей мира"
        _worldPawns.Add(pawn);

        // Регистрируем его в глобальных системах
        _needsSystem.RegisterPawn(pawn);
        _healthSystem.RegisterPawn(pawn);
        _jobSearchSystem.RegisterPawn(pawn);
        _jobExecutionSystem.RegisterPawn(pawn);

        return pawn;
    }
}
