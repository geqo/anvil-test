using UnityEngine;

public class JobDriver_Smith : JobDriver
{
    private float _workLeft;
    private RecipeDefinition _recipe;

    public override string JobType => JobDefOf.Smith;

    public JobDriver_Smith(string recipeId)
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
        Debug.Log($"Пешка {Pawn.Name} начинает ковать '{_recipe.resultItemId}'.");
    }

    public override void Tick()
    {
        float workPerSecond = 1f;
        float finalWorkSpeed = StatSystem.CalculateStatValue(Pawn, StatDefOf.GlobalWorkSpeed, workPerSecond);
        _workLeft -= finalWorkSpeed * Time.deltaTime;

        if (_workLeft <= 0)
        {
            // (Здесь будет логика создания Item'а из материала)
            Debug.Log($"<color=orange>Пешка {Pawn.Name} выковала '{_recipe.resultItemId}'!</color>");
            OnFinish();
        }
    }
}
