using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProcureFlow.Domain.Common.Exceptions;
using ProcureFlow.Domain.Common.ValueObjects;

namespace ProcureFlow.Domain.Tests.Common.ValueObjects;
public class MoneyTests
{
    [Fact]
    public void Constructor_ShouldCreateMoney()
    {
        var money = new Money(1000m, "INR");

        Assert.Equal(1000m, money.Amount);
        Assert.Equal("INR", money.Currency);
    }

    [Fact]
    public void Constructor_ShouldRejectNegativeAmount()
    {
        var action = () => new Money(-100m, "INR");

        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void Add_ShouldCalculateTotal()
    {
        var first = new Money(1000m, "INR");
        var second = new Money(500m, "INR");

        var result = first.Add(second);

        Assert.Equal(1500m, result.Amount);
        Assert.Equal("INR", result.Currency);
    }

    [Fact]
    public void Add_ShouldRejectDifferentCurrencies()
    {
        var first = new Money(1000m, "INR");
        var second = new Money(500m, "USD");

        var action = () => first.Add(second);

        Assert.Throws<DomainException>(action);
    }
}