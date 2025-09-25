using System.Collections.Generic;

public class ItemDefinition
{
    // --- ОБЩИЕ СВОЙСТВА ---
    public string id;
    public string name;
    public string description;
    public float weight;
    public int stackLimit = 1;

    // --- ФЛАГИ ПОВЕДЕНИЯ ---
    public bool isIngredient; // Является ли предмет ингридиентом для крафта
    public bool isMaterial; // Является ли этот предмет строительным материалом
    public bool canBeMoved = true; // Можно ли перемещать этот предмет (по умолчанию да)

    // --- СВОЙСТВА ВЗАИМОДЕЙСТВИЯ ---
    public bool isMineable; // Можно ли "добывать" (как дерево или руду)
    public List<YieldedItem> mineYield; // Что выпадает при добыче

    public bool isDeconstructible; // Можно ли разобрать (как постройку)
    public List<YieldedItem> deconstructYield; // Что выпадает при разборке

    // --- СВОЙСТВА ЕДЫ ---
    public FoodProperties food;
    // Вложенный класс для свойств еды. Он будет null, если предмет не является едой.
    public class FoodProperties
    {
        public string satisfiesNeed; // На какую потребность влияет, например "hunger"
        public float amount;         // На сколько утоляет потребность
    }

    // --- Свойства хранилища ---
    public class StorageProperties
    {
        public float maxWeight;
        // В будущем здесь будут фильтры: что можно хранить, а что нет
    }
    public StorageProperties storage;

    // --- Свойства материала ---
    public class StuffProperties
    {
        // Категория материала, например "Woody", "Metalic"
        public string stuffCategory;
        // Модификаторы, которые этот материал дает предметам
        public Dictionary<string, float> statModifiers; // "max_hp_multiplier": 0.8
    }
    public bool isMadeOfStuff; // Флаг: делается ли этот предмет из материала
    public StuffProperties stuff; // Свойства, если этот предмет САМ является материалом

}
