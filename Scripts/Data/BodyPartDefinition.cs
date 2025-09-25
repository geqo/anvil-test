public class BodyPartDefinition
{
    public string id;       // Уникальный ID, e.g., "torso", "heart"
    public string name;     // Отображаемое имя, "Торс"
    public float maxHp;     // Максимальное здоровье
    public bool isVital;    // Приводит ли уничтожение к мгновенной смерти
    public bool isNatural;  // Является ли часть тела органической
}
