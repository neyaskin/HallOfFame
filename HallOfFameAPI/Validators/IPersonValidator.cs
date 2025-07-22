using HallOfFameAPI.DTOs;

namespace HallOfFameAPI.Validators;

public interface IPersonValidator
{
    ValidationResult Validate(PersonCreateDto personDto);
    ValidationResult Validate(PersonUpdateDto personDto);
}