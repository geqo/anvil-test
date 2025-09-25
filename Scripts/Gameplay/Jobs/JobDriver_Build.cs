using UnityEngine;

public class JobDriver_Build : JobDriver
{
    private float _workLeft;
    private RecipeDefinition _recipe;

    public override string JobType => JobDefOf.Build;

    public JobDriver_Build(string recipeId)
    {
        if (ModManager.Instance.AllRecipes.TryGetValue(recipeId, out var recipeDef))
        {
            _recipe = recipeDef;
        }
    }

    public override void Setup(Pawn worker)
    {
        base.Setup(worker);
        if (_recipe == null) { OnFinish(); return; }

        _workLeft = _recipe.workAmount;
        Debug.Log($"Пешка {Pawn.Name} начинает строить '{_recipe.resultItemId}'.");
    }

    public override void Tick()
    {
        float workPerSecond = 1f;
        float finalWorkSpeed = StatSystem.CalculateStatValue(Pawn, StatDefOf.GlobalWorkSpeed, workPerSecond);
        _workLeft -= finalWorkSpeed * Time.deltaTime;

        if (_workLeft <= 0)
        {
            // (Здесь будет логика создания Building и удаления ресурсов)
            Debug.Log($"<color=yellow>Пешка {Pawn.Name} построила '{_recipe.resultItemId}'!</color>");
            OnFinish();
        }
    }
}
