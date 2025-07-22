using HallOfFameAPI.Data.Entities;

namespace HallOfFameAPI.Data.Repositories;

/// <summary>
///     Интерфейс управления данными о пользователях.
/// </summary>
public interface IPersonRepository
{
    /// <summary>
    ///     Метод получения данных о всех пользователях.
    /// </summary>
    /// <returns>Данные о пользователях.</returns>
    Task<IEnumerable<Person>> GetAllPersonAsync();

    /// <summary>
    ///     Метод получения данных о пользователе.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>Данные пользователя.</returns>
    Task<Person> GetPersonByIdAsync(long id);

    /// <summary>
    ///     Добавление данных о новом пользователе.
    /// </summary>
    /// <param name="person">
    ///     <seealso cref="Person" />
    /// </param>
    /// <returns></returns>
    Task<Person> AddPersonAsync(Person person);

    /// <summary>
    ///     Метод обновления данных о пользователе.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="person">
    ///     <seealso cref="Person" />
    /// </param>
    /// <returns>
    ///     <seealso cref="Person" />
    /// </returns>
    Task<Person> UpdatePersonAsync(Person person);

    /// <summary>
    ///     Метод удаления пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    Task DeletePersonAsync(long id);

    /// <summary>
    ///     Метод удаления умений пользователя.
    /// </summary>
    /// <param name="skills">Список умений пользователя. <seealso cref="Skill" /></param>
    void RemoveSkills(ICollection<Skill> skills);
}