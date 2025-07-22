namespace HallOfFameAPI.Data.Entities;

/// <summary>
///     Сущность для описания умений.
/// </summary>
public class Skill
{
    /// <summary>
    ///     Идентификатор умения.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    ///     Название умения.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Уровень владения умением.
    /// </summary>
    public byte Level { get; set; }

    /// <summary>
    ///     Идентификатор пользователя
    /// </summary>
    public long PersonId { get; set; }

    /// <summary>
    ///     Ссылка на пользователя.
    /// </summary>
    public Person? Person { get; set; }
}