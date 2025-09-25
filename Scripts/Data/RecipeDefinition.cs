using System.Collections.Generic;

public class RecipeDefinition
{
    public class Ingredient
    {
        public string itemId;
        public int amount;
    }
    public string id;
    public List<Ingredient> ingredients;
    public string resultItemId;
    public float workTime;
    public string skillUsed; // "cooking"
}
