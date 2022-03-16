﻿using Microsoft.Z3;
using Symbolica.Expression;

namespace Symbolica.Computation.Values;

internal sealed record ZeroExtend : BitVector
{
    private readonly IValue _value;

    private ZeroExtend(Bits size, IValue value)
        : base(size)
    {
        _value = value;
    }

    public override BitVecExpr AsBitVector(IContext context)
    {
        return context.CreateExpr(c => c.MkZeroExt((uint) (Size - _value.Size), _value.AsBitVector(context)));
    }

    public static IValue Create(Bits size, IValue value)
    {
        return size > value.Size
            ? value is IConstantValue v
                ? v.AsUnsigned().Extend(size)
                : new ZeroExtend(size, value)
            : value;
    }
}
