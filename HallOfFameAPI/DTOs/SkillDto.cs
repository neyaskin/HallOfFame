/// <summary>
///     DTO для создания нового умения.
/// </summary>
public class SkillDto
{
    /// <summary>
    ///     Наименование умения.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Уровень владения умением.
    /// </summary>
    public byte Level { get; set; }
}