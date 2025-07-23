namespace HallOfFameAPI.Validators;

/// <summary>
///     Класс для результата валидации, содержащий статус и сообщение об ошибке.
/// </summary>
public class ValidationResult
{
    /// <summary>
    ///     Статус валидации.
    /// </summary>
    public bool IsValid { get; }
    
    /// <summary>
    ///     Сообщение об ошибке.
    /// </summary>
    public string ErrorMessage { get; }
    
    public ValidationResult(bool isValid, string errorMessage = null)
    {
        IsValid = isValid;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    ///     Успешный результат валидации.
    /// </summary>
    public static ValidationResult Success()
    {
        return new ValidationResult(true);
    }

    /// <summary>
    ///     Неуспешный результат с сообщением об ошибке.
    /// </summary>
    /// <param name="errorMessage">Описание ошибки</param>
    public static ValidationResult Failure(string errorMessage)
    {
        return new ValidationResult(false, errorMessage);
    }
}