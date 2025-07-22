using HallOfFameAPI.DTOs;

namespace HallOfFameAPI.Validators;

public class PersonValidator : IPersonValidator
{
    public ValidationResult Validate(PersonCreateDto personDto)
    {
        if (personDto == null)
            return ValidationResult.Failure("PersonDto is null");

        return ValidateCommon(personDto.Name, personDto.DisplayName, personDto.Skills);
    }

    public ValidationResult Validate(PersonUpdateDto personDto)
    {
        if (personDto == null)
            return ValidationResult.Failure("PersonDto is null");

        return ValidateCommon(personDto.Name, personDto.DisplayName, personDto.Skills);
    }

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