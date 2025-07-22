namespace HallOfFameAPI.Validators;

public class ValidationResult
{
    public ValidationResult(bool isValid, string errorMessage = null)
    {
        IsValid = isValid;
        ErrorMessage = errorMessage;
    }

    public bool IsValid { get; }
    public string ErrorMessage { get; }

    public static ValidationResult Success()
    {
        return new ValidationResult(true);
    }

    public static ValidationResult Failure(string errorMessage)
    {
        return new ValidationResult(false, errorMessage);
    }
}