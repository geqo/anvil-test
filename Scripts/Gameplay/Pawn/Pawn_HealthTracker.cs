using System.Collections.Generic;
using System.Linq;
using System.Text;

// Компонент, отвечающий за все аспекты здоровья пешки.
public class Pawn_HealthTracker
{
    private Pawn _pawn; // Ссылка на владельца
    public Dictionary<string, BodyPartInstance> BodyParts { get; private set; }
    public List<Hediff> GlobalHediffs { get; private set; } // Глобальные состояния (болезни)
    public bool IsDead { get; private set; }

    // Глобальные вычисляемые статы
    public float BloodLevel = 12000f; // Текущий уровень крови

    public Pawn_HealthTracker(Pawn pawn)
    {
        _pawn = pawn;
        IsDead = false;
        GlobalHediffs = new List<Hediff>();
        BodyParts = new Dictionary<string, BodyPartInstance>();

        // 1. Получаем схему тела пешки из ModManager.
        if (ModManager.Instance.AllBodyLayouts.TryGetValue(_pawn.KindDef.bodyLayoutId, out BodyLayoutDefinition layoutDef))
        {
            // 2. Итерируем только по тем частям тела, которые указаны в схеме.
            foreach (string partId in layoutDef.bodyParts)
            {
                // 3. Находим полное определение части тела по ее ID.
                if (ModManager.Instance.AllBodyParts.TryGetValue(partId, out BodyPartDefinition partDef))
                {
                    // 4. Создаем экземпляр этой части тела и добавляем пешке.
                    BodyParts.Add(partDef.id, new BodyPartInstance(partDef));
                }
                else
                {
                    UnityEngine.Debug.LogWarning($"В схеме тела '{layoutDef.id}' указана несуществующая часть тела: '{partId}'");
                }
            }
        }
        else
        {
            UnityEngine.Debug.LogError($"Не найдена схема тела с ID: '{_pawn.KindDef.bodyLayoutId}' для пешки '{_pawn.Name}'!");
        }
    }

    public void AddHediff(Hediff hediff, string bodyPartId = null)
    {
        if (bodyPartId != null && BodyParts.ContainsKey(bodyPartId))
        {
            // Если указана часть тела, добавляем Hediff ей
            BodyParts[bodyPartId].Hediffs.Add(hediff);
        }
        else
        {
            // Иначе это глобальный Hediff (болезнь)
            GlobalHediffs.Add(hediff);
        }
    }

    public void RemoveBodyPart(string bodyPartId)
    {
        if (BodyParts.ContainsKey(bodyPartId))
        {
            // Здесь может быть логика последствий (сильное кровотечение и т.д.)
            BodyParts.Remove(bodyPartId);
        }
    }

    public void CheckForDeath()
    {
        if (IsDead) return;

        // Причины смерти:
        // 1. Уничтожена жизненно важная часть тела
        if (BodyParts.Values.Any(part => part.IsDestroyed() && part.Def.isVital))
        {
            Die("Уничтожена жизненно важная часть тела.");
            return;
        }

        // 2. Закончилась кровь
        if (BloodLevel <= 0)
        {
            Die("Потеря крови.");
            return;
        }
    }

    private void Die(string reason)
    {
        IsDead = true;
        UnityEngine.Debug.Log($"<color=red>Пешка {_pawn.Name} умерла. Причина: {reason}</color>");
    }

    public string GetStatus()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"  Здоровье: Кровь ({BloodLevel:F0})");
        foreach (var part in BodyParts.Values)
        {
            if (part.CurrentHp < part.Def.maxHp)
            {
                sb.AppendLine($"    - {part.Def.name}: {part.CurrentHp:F1}/{part.Def.maxHp:F1}");
            }
        }
        return sb.ToString();
    }
}
