// Scripts/Data/BodyLayoutDefinition.cs
using System.Collections.Generic;

/// <summary>
/// Определение схемы тела. Описывает, из каких частей тела по умолчанию состоит существо.
/// </summary>
public class BodyLayoutDefinition
{
    /// <summary>
    /// Уникальный ID, например "humanoid", "animal"
    /// </summary>
    public string id;

    /// <summary>
    /// Список ID частей тела, которые входят в эту схему.
    /// </summary>
    public List<string> bodyParts;
}
