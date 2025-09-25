public class ItemDefinition
{
    // Вложенный класс для свойств еды. Он будет null, если предмет не является едой.
    public class FoodProperties
    {
        public string satisfiesNeed; // На какую потребность влияет, например "hunger"
        public float amount;         // На сколько утоляет потребность
    }

    // --- ОБЩИЕ СВОЙСТВА ДЛЯ ВСЕХ ПРЕДМЕТОВ ---
    public string id;
    public string name;
    public string description;

    // --- СПЕЦИФИЧНЫЕ СВОЙСТВА ---
    public bool isIngredient;   // Является ли этот предмет ингредиентом для крафта
    public FoodProperties food; // Свойства еды (будет null, если это не еда)
}
