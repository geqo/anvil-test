using UnityEngine;

// --- Hediff и его наследники ---

/// <summary>
/// Базовый абстрактный класс для всех состояний здоровья (раны, болезни, эффекты).
/// "Hediff" - Health Difference.
/// </summary>
public abstract class Hediff
{
    protected Pawn _pawn; // Ссылка на пешку, у которой это состояние

    public virtual string Label => "Неизвестное состояние"; // Имя для отображения в UI
    public virtual float BleedRate => 0f;                   // Кровотечение в ед/сек

    public Hediff(Pawn pawn)
    {
        _pawn = pawn;
    }

    /// <summary>
    /// Метод, который вызывается каждый кадр из HealthSystem для обновления логики состояния.
    /// </summary>
    public virtual void Tick() { }
}

/// <summary>
/// Представляет собой физическую рану на части тела.
/// Основные эффекты: кровотечение и боль (боль будет рассчитываться в StatSystem).
/// </summary>
public class Hediff_Wound : Hediff
{
    public override string Label => "Рана";

    // Раны вызывают кровотечение. Более серьезные раны могут иметь больший BleedRate.
    public override float BleedRate => 5f;

    public Hediff_Wound(Pawn pawn) : base(pawn) { }
}

/// <summary>
/// Глобальное состояние, возникающее, когда одна из потребностей пешки достигает нуля.
/// Не привязано к части тела.
/// </summary>
public class Hediff_Malnutrition : Hediff
{
    private NeedDefinition _cause;

    // Свойство, чтобы можно было проверить, от какой именно потребности страдает пешка
    public string CausedByNeed => _cause.id;

    public override string Label => $"Истощение ({_cause.name})";

    public Hediff_Malnutrition(Pawn pawn, NeedDefinition cause) : base(pawn)
    {
        _cause = cause;
    }

    /// <summary>
    /// Логика истощения: каждую секунду наносим небольшой урон торсу,
    /// имитируя разрушение организма от голода/жажды/недостатка сна.
    /// HealthSystem будет обрабатывать этот урон и проверять смерть.
    /// </summary>
    public override void Tick()
    {
        // Пытаемся найти торс
        if (_pawn.Health.BodyParts.TryGetValue("torso", out BodyPartInstance torso))
        {
            // Уменьшаем здоровье торса на небольшую величину каждый кадр
            torso.CurrentHp -= 0.1f * Time.deltaTime;
        }
    }
}

