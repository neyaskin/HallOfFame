using HallOfFameAPI.DTOs;

namespace HallOfFameAPI.Validators;

/// <summary>
/// Валидатор для проверки объектов Person (создания и обновления)
/// Проверяет корректность данных перед сохранением в систему
/// </summary>
public class PersonValidator : IPersonValidator
{
    /// <summary>
    /// Валидация DTO для создания Person
    /// </summary>
    /// <param name="personDto">Объект с данными для создания Person <seealso cref="PersonCreateDto"/></param>
    /// <returns>Результат валидации (успех/ошибка с сообщением)</returns>
    public ValidationResult Validate(PersonCreateDto personDto)
    {
        if (personDto == null)
            return ValidationResult.Failure("PersonDto is null");

        return ValidateCommon(personDto.Name, personDto.DisplayName, personDto.Skills);
    }

    /// <summary>
    /// Валидация DTO для обновления Person
    /// </summary>
    /// <param name="personDto">Объект с данными для обновления Person <seealso cref="PersonUpdateDto"/></param>
    /// <returns>Результат валидации (успех/ошибка с сообщением)</returns>
    public ValidationResult Validate(PersonUpdateDto personDto)
    {
        if (personDto == null)
            return ValidationResult.Failure("PersonDto is null");

        return ValidateCommon(personDto.Name, personDto.DisplayName, personDto.Skills);
    }

    /// <summary>
    /// Общий метод валидации основных полей Person и связанных навыков
    /// </summary>
    /// <param name="name">Имя Person</param>
    /// <param name="displayName">Отображаемое имя Person</param>
    /// <param name="skills">Список навыков Person</param>
    /// <returns>Результат валидации</returns>
    private ValidationResult ValidateCommon(string name, string displayName, IEnumerable<SkillDto> skills)
    {
        if (string.IsNullOrEmpty(name))
            return ValidationResult.Failure("PersonDto.Name is empty");

        if (displayName?.Length > 100)
            return ValidationResult.Failure("Length Person Display name more 100");

        if (name?.Length > 100)
            return ValidationResult.Failure("Length Person Name more 100");

        if (skills == null)
            return ValidationResult.Failure("Skills are null");

        foreach (var skillDto in skills)
        {
            if (skillDto.Name?.Length > 50)
                return ValidationResult.Failure("Skill name length more than 50");

            if (skillDto.Level < 1 || skillDto.Level > 10)
                return ValidationResult.Failure("Skill level must be between 1 and 10");
        }

        return ValidationResult.Success();
    }
}