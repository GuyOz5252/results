namespace Results;

public readonly record struct Error(string ErrorType, string ErrorCode, string ErrorMessage);
