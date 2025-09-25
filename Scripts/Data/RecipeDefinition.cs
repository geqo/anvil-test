using System.Collections.Generic;

public class RecipeDefinition
{
    public class Ingredient
    {
        public string itemId;
        public int amount;

        // Для выбора из нескольких категорий материалов (напр., дерево ИЛИ металл)
        public List<MaterialCost> materialCosts;
    }

    public string id;
    public string jobType;
    public List<Ingredient> ingredients; // Это поле теперь может содержать более сложные данные
    public string resultItemId;
    public float workAmount;
    public string skillUsed; // "cooking"
}
