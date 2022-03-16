﻿using FluentAssertions;
using Symbolica.Computation.Values.TestData;
using Symbolica.Expression;
using Xunit;

namespace Symbolica.Computation.Values;

public class ZeroExtendTests
{
    [Theory]
    [ClassData(typeof(ExtendTestData))]
    private void ShouldCreateEquivalentBitVectors(Bits size,
        IValue value0,
        IValue value1)
    {
        using var context = PooledContext.Create();

        var result0 = ZeroExtend.Create(size, value0).AsBitVector(context).Simplify();
        var result1 = ZeroExtend.Create(size, value1).AsBitVector(context).Simplify();

        result0.Should().BeEquivalentTo(result1);
    }

    [Theory]
    [ClassData(typeof(ExtendTestData))]
    private void ShouldCreateEquivalentBooleans(Bits size,
        IValue value0,
        IValue value1)
    {
        using var context = PooledContext.Create();

        var result0 = ZeroExtend.Create(size, value0).AsBool(context).Simplify();
        var result1 = ZeroExtend.Create(size, value1).AsBool(context).Simplify();

        result0.Should().BeEquivalentTo(result1);
    }
}
