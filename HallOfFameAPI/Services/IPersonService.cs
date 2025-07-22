using HallOfFameAPI.DTOs;

namespace HallOfFameAPI.Services;

/// <summary>
///     Интерфейс сервиса управления пользователями.
/// </summary>
public interface IPersonService
{
    /// <summary>
    ///     Метод получения всех пользователей.
    /// </summary>
    /// <returns>Список пользователей.</returns>
    Task<IEnumerable<PersonResponseDto>> GetAllPersonsAsync();

    /// <summary>
    ///     Метод получения пользователя по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>Пользователь.</returns>
    Task<PersonResponseDto> GetPersonByIdAsync(long id);

    /// <summary>
    ///     Метод добавления нового пользователя.
    /// </summary>
    /// <param name="person">
    ///     <seealso cref="PersonCreateDto" />
    /// </param>
    /// <returns></returns>
    Task<ServiceResult<PersonResponseDto>> AddPersonAsync(PersonCreateDto? person);

    /// <summary>
    ///     Метод обновления пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="person">Данные для обновления. <seealso cref="PersonUpdateDto" /></param>
    /// <returns>
    ///     <seealso cref="PersonResponseDto" />
    /// </returns>
    Task<ServiceResult<PersonResponseDto>> UpdatePersonAsync(long id, PersonUpdateDto person);

    /// <summary>
    ///     Метод удаления пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    Task DeletePersonAsync(long id);
}