namespace EquipmentLoanManager.Core.Results;

public class OperationResult
{
    protected OperationResult(bool succeeded, string message)
    {
        Succeeded = succeeded;
        Message = message;
    }

    public bool Succeeded { get; }

    public string Message { get; }

    public static OperationResult Success(string message) => new(true, message);

    public static OperationResult Failure(string message) => new(false, message);
}

public sealed class OperationResult<T> : OperationResult
{
    private OperationResult(bool succeeded, string message, T? value)
        : base(succeeded, message)
    {
        Value = value;
    }

    public T? Value { get; }

    public static OperationResult<T> Success(T value, string message) => new(true, message, value);

    public static new OperationResult<T> Failure(string message) => new(false, message, default);
}
