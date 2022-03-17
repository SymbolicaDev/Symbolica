﻿using FluentAssertions;
using Symbolica.Computation.Values.TestData;
using Xunit;

namespace Symbolica.Computation.Values;

public class NotTests
{
    [Theory]
    [ClassData(typeof(UnaryTestData))]
    private void ShouldCreateEquivalentBitVectors(
        IValue value0,
        IValue value1)
    {
        using var context = PooledContext.Create();

        var result0 = Not.Create(value0).AsBitVector(context).Simplify();
        var result1 = Not.Create(value1).AsBitVector(context).Simplify();

        result0.Should().BeEquivalentTo(result1);
    }

    [Theory]
    [ClassData(typeof(UnaryTestData))]
    private void ShouldCreateEquivalentBooleans(
        IValue value0,
        IValue value1)
    {
        using var context = PooledContext.Create();

        var result0 = Not.Create(value0).AsBool(context).Simplify();
        var result1 = Not.Create(value1).AsBool(context).Simplify();

        result0.Should().BeEquivalentTo(result1);
    }
}
