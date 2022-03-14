﻿using System.Collections.Generic;
using Microsoft.Z3;

namespace Symbolica.Computation.Values;

internal sealed class SignedGreater : Bool
{
    private readonly IValue _left;
    private readonly IValue _right;

    private SignedGreater(IValue left, IValue right)
    {
        _left = left;
        _right = right;
    }

    public override IEnumerable<IValue> Children => new[] { _left, _right };

    public override string? PrintedValue => null;

    public override BoolExpr AsBool(IContext context)
    {
        using var t1 = _left.AsBitVector(context);
        using var t2 = _right.AsBitVector(context);
        return context.CreateExpr(c => c.MkBVSGT(t1, t2));
    }

    public static IValue Create(IValue left, IValue right)
    {
        return left is IConstantValue l && right is IConstantValue r
            ? l.AsSigned().Greater(r.AsSigned())
            : new SignedGreater(left, right);
    }
}
