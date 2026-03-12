using DotResults;

namespace DotResults.Tests;

public class ResultTests
{
    [Fact]
    public void NonGenericResult_Success_ShouldBeSuccess()
    {
        var result = Result.Success();
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void GenericResult_Success_ShouldBeSuccess()
    {
        var result = Result<int>.Success(42);
        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void GenericResult_ImplicitConversion_ShouldBeSuccess()
    {
        Result<int> result = 42;
        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void FluentSyntax_ProposedUsage()
    {
        // Now this should work via type inference
        var result = Result.Success(42);
        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void FluentSyntax_Failure()
    {
        var error = new Error("Type", "Code", "Message");
        var result = Result.Failure<int>(error);
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void FluentSyntax_ValidationFailure()
    {
        var validationError = new ValidationError("Field", "Value", "Message");
        var result = Result.ValidationFailure<int>(validationError);
        Assert.True(result.IsFailure);
        Assert.Contains(validationError, result.ValidationErrors);
    }
}
