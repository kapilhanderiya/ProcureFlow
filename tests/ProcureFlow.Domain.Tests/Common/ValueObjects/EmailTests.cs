using ProcureFlow.Domain.Common.Exceptions;
using ProcureFlow.Domain.Common.ValueObjects;

namespace ProcureFlow.Domain.Tests.Common.ValueObjects;
public class EmailTests
{
    [Fact]
    public void Constructor_ShouldNormalizeEmail()
    {
        var email = new Email("  JOHN.DOE@EXAMPLE.COM  ");

        Assert.Equal("john.doe@example.com", email.Value);
    }

    [Fact]
    public void Constructor_ShouldRejectEmptyEmail()
    {
        var action = () => new Email("");

        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void Constructor_ShouldRejectInvalidEmail()
    {
        var action = () => new Email("invalid-email");

        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void EqualEmails_ShouldBeEqual()
    {
        var first = new Email("john@example.com");
        var second = new Email("JOHN@EXAMPLE.COM");

        Assert.Equal(first, second);
    }
}