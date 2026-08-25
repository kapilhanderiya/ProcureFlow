using ProcureFlow.Domain.Common.Exceptions;
using ProcureFlow.Domain.Common.ValueObjects;

namespace ProcureFlow.Domain.Tests.Common.ValueObjects;
public class AddressTests
{
    [Fact]
    public void Constructor_ShouldCreateAddress()
    {
        var address = new Address(
            "123 Business Street",
            null,
            "Ponda",
            "Goa",
            "403401",
            "India");

        Assert.Equal("123 Business Street", address.AddressLine1);
        Assert.Equal("Ponda", address.City);
        Assert.Equal("Goa", address.State);
        Assert.Equal("403401", address.PostalCode);
        Assert.Equal("India", address.Country);
    }

    [Fact]
    public void Constructor_ShouldRejectEmptyStreet()
    {
        var action = () => new Address(
            "",
            null,
            "Ponda",
            "Goa",
            "403401",
            "India");

        Assert.Throws<DomainException>(action);
    }
}
