using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Главный управляющий класс игры.
/// Отвечает за создание и хранение состояния мира (карты),
/// а также за запуск симуляции и создание начальных сущностей.
/// </summary>
public class WorldController : MonoBehaviour
{
    public static WorldController Instance { get; private set; }

    public Map CurrentMap { get; private set; }

    // Ссылки на все "умные" системы
    private NeedsSystem _needsSystem;
    private HealthSystem _healthSystem;
    private JobSearchSystem _jobSearchSystem;
    private JobExecutionSystem _jobExecutionSystem;

    // Список для отслеживания всех пешек (для удобства)
    private List<Pawn> _worldPawns = new List<Pawn>();

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        // 1. Находим все системы на этом же игровом объекте
        _needsSystem = GetComponent<NeedsSystem>();
        _healthSystem = GetComponent<HealthSystem>();
        _jobSearchSystem = GetComponent<JobSearchSystem>();
        _jobExecutionSystem = GetComponent<JobExecutionSystem>();

        Debug.Log("--- WorldController: Начало симуляции ---");

        // 2. Создаем игровой мир
        CurrentMap = new Map(50, 50);

        // 3. Создаем и размещаем начальные сущности
        var human = CreateAndRegisterPawn(PawnKindDefOf.Humanlike, new List<string> { "tough", "workaholic" });
        var catlike = CreateAndRegisterPawn(PawnKindDefOf.Catlike, new List<string> { "gourmand" });
        var animal = CreateAndRegisterPawn(PawnKindDefOf.AnimalCat, new List<string> { "fast_walker" });

        CurrentMap.SpawnEntity(human, new Vector2Int(10, 10));
        CurrentMap.SpawnEntity(catlike, new Vector2Int(12, 10));
        CurrentMap.SpawnEntity(animal, new Vector2Int(10, 12));

        var rawFoodDef = ModManager.Instance.AllItems["raw_food"];
        var foodStack = new Item(rawFoodDef, 20);
        CurrentMap.SpawnEntity(foodStack, new Vector2Int(15, 15));

        // 4. Создаем начальные задачи
        JobQueue.Instance.AddJob(new JobTicket(JobDefOf.Cook, "cook_simple_meal"));
        JobQueue.Instance.AddJob(new JobTicket(JobDefOf.Haul));
    }

    private Pawn CreateAndRegisterPawn(string pawnKindId, List<string> traitIds)
    {
        List<TraitDefinition> traits = new List<TraitDefinition>();
        foreach (string id in traitIds)
        {
            if (ModManager.Instance.AllTraits.TryGetValue(id, out TraitDefinition traitDef))
            {
                traits.Add(traitDef);
            }
        }

        var pawn = new Pawn(pawnKindId, traits);
        if (pawn.KindDef == null) return null;

        _worldPawns.Add(pawn);

        // Регистрируем пешку во всех системах
        _needsSystem.RegisterPawn(pawn);
        _healthSystem.RegisterPawn(pawn);
        _jobSearchSystem.RegisterPawn(pawn);
        _jobExecutionSystem.RegisterPawn(pawn);

        return pawn;
    }
}
