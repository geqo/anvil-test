// Scripts/Gameplay/Jobs/JobDriver_Cook.cs
using UnityEngine;

public class JobDriver_Cook : JobDriver
{
    private float _workLeft;
    private RecipeDefinition _recipe;
    public override string JobType => JobDefOf.Cook;

    /// <summary>
    /// Конструктор, который принимает ID рецепта.
    /// </summary>
    public JobDriver_Cook(string recipeId)
    {
        // Находим и сохраняем определение рецепта из ModManager'а
        if (ModManager.Instance.AllRecipes.TryGetValue(recipeId, out var recipeDef))
        {
            _recipe = recipeDef;
        }
    }

    public override void Setup(Pawn worker)
    {
        base.Setup(worker);

        if (_recipe == null)
        {
            Debug.LogError($"JobDriver_Cook не может начаться без валидного рецепта! Возможно, ID рецепта не был передан в конструктор.");
            OnFinish();
            return;
        }

        // Устанавливаем начальное количество работы из сохраненного рецепта.
        _workLeft = _recipe.workAmount;
        Debug.Log($"Пешка {Pawn.Name} начинает готовить '{_recipe.id}'. Нужно выполнить работы: {_workLeft}");
    }

    public override void Tick()
    {
        float workPerSecond = 1f;
        float finalWorkSpeed = StatSystem.CalculateStatValue(Pawn, StatDefOf.GlobalWorkSpeed, workPerSecond);
        _workLeft -= finalWorkSpeed * Time.deltaTime;

        if (_workLeft <= 0)
        {
            // (В будущем здесь будет логика создания предмета)
            Debug.Log($"<color=green>Пешка {Pawn.Name} закончила готовить '{_recipe.id}'!</color>");
            OnFinish();
        }
    }
}
