/// <summary>
///     DTO для отправки пользователя.
/// </summary>
public class PersonResponseDto
{
    /// <summary>
    ///     Идентификатор пользователя.
    /// </summary>
    public long Id { get; set; }

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