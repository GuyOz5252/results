namespace DotResults;

public readonly record struct Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public Error Error
    {
        get => IsFailure
            ? throw new InvalidOperationException("Cannot access an error of a successful result")
            : field;
        init;
    }

    public ValidationError[] ValidationErrors
    {
        get => IsFailure
            ? throw new InvalidOperationException("Cannot access validation errors of a successful result")
            : field;
        init;
    }
    
    private Result(bool isSuccess, Error error, ValidationError[]? validationErrors)
    {
        switch (isSuccess)
        {
            case true when error != default:
                throw new ArgumentException("Success result cannot have an error", nameof(error));
            case false when error == default && validationErrors == null:
                throw new ArgumentException("Failure result must have an error or validation errors", nameof(error));
            default:
                IsSuccess = isSuccess;
                Error = error;
                ValidationErrors = validationErrors ?? [];
                break;
        }
    }

    public static Result Success()
    {
        return new Result(true, default, null);
    }

    public static Result Failure(Error error)
    {
        return new Result(false, error, null);
    }

    public static Result ValidationFailure(params ValidationError[] validationErrors)
    {
        return new Result(true, default, validationErrors);
    }
    
    public static implicit operator Result(Error error) => new(false, error, null);
    public static implicit operator Result(ValidationError[] validationErrors) =>
        new(false, default, validationErrors);
}

public readonly record struct Result<T>
{
    public T Value
    {
        get => IsFailure
            ? throw new InvalidOperationException("Cannot access a value of a failure result")
            : field;
        private init;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    
    public Error Error
    {
        get => IsSuccess
            ? throw new InvalidOperationException("Cannot access an error of a successful result")
            : field;
        private init;
    }

    public ValidationError[] ValidationErrors
    {
        get => IsFailure
            ? throw new InvalidOperationException("Cannot access validation errors of a successful result")
            : field;
        init;
    }
    
    private Result(bool isSuccess, T? value, Error error, ValidationError[]? validationErrors)
    {
        IsSuccess = isSuccess;
        Value = value!;
        Error = error;
        ValidationErrors = validationErrors ?? [];
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, default, null);
    }

    public static Result<T> Failure(Error error)
    {
        return new Result<T>(false, default, error, null);
    }
    
    public static implicit operator Result<T>(T value) => new(true, value, default, null);
    public static implicit operator Result<T>(Error error) => new(false, default, error, null);
    public static implicit operator Result<T>(ValidationError[] validationErrors) =>
        new(false, default, default, validationErrors);
    
    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(Value) : onFailure(Error);
    }
}
