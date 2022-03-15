﻿using FluentAssertions;
using Symbolica.Computation.Values.TestData;
using Xunit;

namespace Symbolica.Computation.Values;

public class LogicalNotTests
{
    private static readonly IContext Context = ContextProxy.Create();

    [Theory]
    [ClassData(typeof(UnaryTestData))]
    private void ShouldCreateEquivalentBitVectors(
        IValue value0,
        IValue value1)
    {
        var result0 = LogicalNot.Create(value0).AsBitVector(Context).Simplify();
        var result1 = LogicalNot.Create(value1).AsBitVector(Context).Simplify();

        result0.Should().BeEquivalentTo(result1);
    }

    [Theory]
    [ClassData(typeof(UnaryTestData))]
    private void ShouldCreateEquivalentBooleans(
        IValue value0,
        IValue value1)
    {
        var result0 = LogicalNot.Create(value0).AsBool(Context).Simplify();
        var result1 = LogicalNot.Create(value1).AsBool(Context).Simplify();

        result0.Should().BeEquivalentTo(result1);
    }
}
