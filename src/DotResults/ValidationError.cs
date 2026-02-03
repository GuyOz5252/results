namespace DotResults;

public readonly record struct ValidationError(string Field, string Value, string ErrorMessage);
