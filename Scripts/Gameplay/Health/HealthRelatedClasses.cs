using UnityEngine;

// --- Hediff и его наследники ---

public abstract class Hediff
{
    protected Pawn _pawn;
    public virtual string Label => "Неизвестное состояние";
    public virtual float BleedRate => 0f;

    public Hediff(Pawn pawn) { _pawn = pawn; }
    public virtual void Tick() { }
}

public class Hediff_Wound : Hediff
{
    public override string Label => "Рана";
    public override float BleedRate => 5f;
    public Hediff_Wound(Pawn pawn) : base(pawn) { }
}

public class Hediff_Malnutrition : Hediff
{
    private NeedDefinition _cause;
    public string CausedByNeed => _cause.id;
    public override string Label => $"Истощение ({_cause.name})";

    public Hediff_Malnutrition(Pawn pawn, NeedDefinition cause) : base(pawn)
    {
        _cause = cause;
    }

    public override void Tick()
    {
        // Пытаемся найти торс, используя константу
        if (_pawn.Health.BodyParts.TryGetValue(BodyPartDefOf.Torso, out BodyPartInstance torso))
        {
            torso.CurrentHp -= 0.1f * Time.deltaTime;
        }
    }
}
