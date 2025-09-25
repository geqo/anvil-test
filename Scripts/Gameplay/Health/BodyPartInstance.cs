using System.Collections.Generic;

// Экземпляр части тела. У каждой пешки свой. Это "тупой" контейнер данных.
public class BodyPartInstance
{
    public BodyPartDefinition Def { get; private set; }
    public float CurrentHp { get; set; }
    public List<Hediff> Hediffs { get; private set; }

    public BodyPartInstance(BodyPartDefinition def)
    {
        Def = def;
        CurrentHp = def.maxHp;
        Hediffs = new List<Hediff>();
    }

    public bool IsDestroyed() => CurrentHp <= 0;

    public bool IsNatural() => Def.isNatural;
}
