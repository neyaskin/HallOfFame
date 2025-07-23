namespace HallOfFameAPI.Services;


/// <summary>
///     Класс для результата работы сервиса, содержащий статус, данные и сообщение об ошибке.
/// </summary>
public class ServiceResult<T>
{
    private ServiceResult(bool isSuccess, T data, string errorMessage)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    ///     Статус результата.
    /// </summary>
    public bool IsSuccess { get; set; }
    
    /// <summary>
    ///     Данные результата.
    /// </summary>
    public T Data { get; set; }
    
    /// <summary>
    ///     Сообщение об ошибке.
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    ///     Успешный результат работы сервиса.
    /// </summary>
    public static ServiceResult<T> Success(T data)
    {
        return new ServiceResult<T>(true, data, null);
    }
}