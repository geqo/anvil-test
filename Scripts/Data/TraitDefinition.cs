// TraitDefinition.cs
using System.Collections.Generic;

public class TraitDefinition
{
    public class Modifier
    {
        public string stat;    // На какой стат влияем (e.g., "hunger_decay_rate")
        public string op;      // Операция: "multiply" (умножить) или "add" (добавить)
        public float value;    // Значение
    }

    public string id;
    public string name;
    public string description;
    public List<Modifier> modifiers;
    public List<string> disabledJobTypes;
}
