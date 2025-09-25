using System.Collections.Generic;
using System.Text;

// Компонент, отвечающий за все потребности пешки.
public class Pawn_NeedsTracker
{
    private Pawn _pawn; // Ссылка на владельца

    // Словарь для хранения ТЕКУЩИХ значений потребностей.
    public Dictionary<string, float> Needs { get; private set; }

    public Pawn_NeedsTracker(Pawn pawn)
    {
        _pawn = pawn;
        Needs = new Dictionary<string, float>();

        // При создании, инициализируем все потребности максимальными значениями
        foreach (var needDef in ModManager.Instance.AllNeeds.Values)
        {
            Needs.Add(needDef.id, needDef.maxValue);
        }
    }

    // Метод для получения значения потребности с учетом модификаторов от черт характера
    public float GetValue(string needId)
    {
        if (Needs.ContainsKey(needId))
        {
            return Needs[needId];
        }
        return 0f;
    }

    public string GetStatus()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("  Потребности:");
        foreach (var need in Needs)
        {
            // F1 - форматирование с 1 знаком после запятой
            sb.AppendLine($"    - {need.Key}: {need.Value:F1}");
        }
        // Удаляем последний перенос строки для чистоты вывода
        if (sb.Length > 0) sb.Length--;
        return sb.ToString();
    }
}
