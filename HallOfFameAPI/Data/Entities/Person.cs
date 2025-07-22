namespace HallOfFameAPI.Data.Entities;

/// <summary>
///     Сущность для описания пользователя.
/// </summary>
public class Person
{
    /// <summary>
    ///     Идентификатор пользователя
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
    public List<Skill> Skills { get; set; } = new();
}