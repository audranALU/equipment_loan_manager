namespace EquipmentLoanManager.Core.Results;

public class OperationResult
{
    /// <summary>Creates a result with a status and message.</summary>
    protected OperationResult(bool succeeded, string message)
    {
        Succeeded = succeeded;
        Message = message;
    }

    public bool Succeeded { get; }

    public string Message { get; }

    /// <summary>Creates a successful result.</summary>
    public static OperationResult Success(string message) => new(true, message);

    /// <summary>Creates a failed result.</summary>
    public static OperationResult Failure(string message) => new(false, message);
}

public sealed class OperationResult<T> : OperationResult
{
    /// <summary>Creates a result with a status, message and value.</summary>
    private OperationResult(bool succeeded, string message, T? value)
        : base(succeeded, message)
    {
        Value = value;
    }

    public T? Value { get; }

    /// <summary>Creates a successful result with a value.</summary>
    public static OperationResult<T> Success(T value, string message) => new(true, message, value);

    /// <summary>Creates a failed result with no value.</summary>
    public static new OperationResult<T> Failure(string message) => new(false, message, default);
}
