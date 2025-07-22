namespace HallOfFameAPI.DTOs;

/// <summary>
///     DTO для обновления данных пользователей.
/// </summary>
public class PersonUpdateDto
{
    /// <summary>
    ///     Имя пользователя.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Отображаемое имя пользователя.
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    ///     Список умений пользователя.
    /// </summary>
    public List<SkillDto> Skills { get; set; }
}