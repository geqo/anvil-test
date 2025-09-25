// Scripts/Gameplay/Pawn/Pawn_SkillTracker.cs
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// Компонент-данные, который хранит информацию о навыках пешки (уровень, опыт).
/// </summary>
public class Pawn_SkillTracker
{
    // Внутренний класс для хранения данных о конкретном навыке
    public class Skill
    {
        public SkillDefinition Def;
        public int Level;
        public float Xp;

        public Skill(SkillDefinition def)
        {
            Def = def;
            Level = 0; // Начальный уровень
            Xp = 0;
        }
    }

    // Словарь для хранения всех навыков пешки
    public Dictionary<string, Skill> Skills { get; private set; }

    public Pawn_SkillTracker(Pawn pawn)
    {
        Skills = new Dictionary<string, Skill>();

        // Инициализируем только те навыки, которые доступны для вида этой пешки
        if (pawn.KindDef.availableSkills != null)
        {
            foreach (string skillId in pawn.KindDef.availableSkills)
            {
                if (ModManager.Instance.AllSkills.TryGetValue(skillId, out SkillDefinition skillDef))
                {
                    Skills.Add(skillId, new Skill(skillDef));
                }
            }
        }
    }

    public bool HasSkill(string skillId)
    {
        return Skills.ContainsKey(skillId);
    }

    public string GetStatus()
    {
        if (Skills.Count == 0) return "  Навыки: Отсутствуют";

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("  Навыки:");
        foreach (var skill in Skills.Values)
        {
            sb.AppendLine($"    - {skill.Def.name}: {skill.Level}");
        }
        if (sb.Length > 0) sb.Length--;
        return sb.ToString();
    }
}
