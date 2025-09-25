// Scripts/Data/MaterialCost.cs

/// <summary>
/// Вспомогательный класс, описывающий стоимость постройки из одной конкретной категории материалов.
/// </summary>
[System.Serializable]
public class MaterialCost
{
    public string stuffCategory; // Категория материала (напр., "Woody", "Metallic")
    public int amount;           // Сколько единиц этой категории нужно
}
