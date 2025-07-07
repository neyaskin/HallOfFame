namespace HallOfFameAPI.DTOs;

/// <summary>
/// DTO для отправки умения.
/// </summary>
public class SkillResponseDto
{
    /// <summary>
    /// Наименование умения.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Уровень владения умением.
    /// </summary>
    public byte Level { get; set; }
}