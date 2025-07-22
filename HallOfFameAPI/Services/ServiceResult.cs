namespace HallOfFameAPI.Services;

public class ServiceResult<T>
{
    private ServiceResult(bool isSuccess, T data, string errorMessage)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
    }

    public bool IsSuccess { get; set; }
    public T Data { get; set; }
    public string ErrorMessage { get; set; }

    public static ServiceResult<T> Success(T data)
    {
        return new ServiceResult<T>(true, data, null);
    }

    public static ServiceResult<T> Failure(string errorMessage)
    {
        return new ServiceResult<T>(false, default, errorMessage);
    }
}