namespace HallOfFameAPI.Exceptions;

/// <summary>
///     Исключение если не найден объект.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException()
    { }

    public NotFoundException(string message)
        : base(message)
    { }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
