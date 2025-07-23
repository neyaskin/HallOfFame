using HallOfFameAPI.DTOs;

namespace HallOfFameAPI.Validators;

/// <summary>
///     Интерфейс валидатора для Person.
/// </summary>
public interface IPersonValidator
{
    /// <summary>
    ///     Валидация DTO для создания Person.
    /// </summary>
    /// <param name="personDto">DTO с данными для создания <seealso cref="PersonCreateDto"/></param>
    ValidationResult Validate(PersonCreateDto personDto);
    
    /// <summary>
    ///     Валидация DTO для обновления Person.
    /// </summary>
    /// <param name="personDto">DTO с данными для обновления <seealso cref="PersonUpdateDto"/></param>
    ValidationResult Validate(PersonUpdateDto personDto);
}