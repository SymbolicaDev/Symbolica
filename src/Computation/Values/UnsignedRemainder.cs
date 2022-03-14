﻿using System.Collections.Generic;
using Microsoft.Z3;

namespace Symbolica.Computation.Values;

internal sealed class UnsignedRemainder : BitVector
{
    private readonly IValue _left;
    private readonly IValue _right;

    private UnsignedRemainder(IValue left, IValue right)
        : base(left.Size)
    {
        _left = left;
        _right = right;
    }

    public override IEnumerable<IValue> Children => new[] { _left, _right };

    public override string? PrintedValue => null;

    public override BitVecExpr AsBitVector(IContext context)
    {
        using var t1 = _left.AsBitVector(context);
        using var t2 = _right.AsBitVector(context);
        return context.CreateExpr(c => c.MkBVURem(t1, t2));
    }

    public static IValue Create(IValue left, IValue right)
    {
        return left is IConstantValue l && right is IConstantValue r
            ? l.AsUnsigned().Remainder(r.AsUnsigned())
            : new UnsignedRemainder(left, right);
    }
}
