﻿using System.Collections.Generic;
using Microsoft.Z3;
using Symbolica.Computation.Values.Constants;

namespace Symbolica.Computation.Values;

internal sealed class LogicalAnd : Bool
{
    private readonly IValue _left;
    private readonly IValue _right;

    private LogicalAnd(IValue left, IValue right)
    {
        _left = left;
        _right = right;
    }

    public override IEnumerable<IValue> Children => new[] { _left, _right };

    public override string? PrintedValue => null;

    public override BoolExpr AsBool(IContext context)
    {
        using var t1 = _left.AsBool(context);
        using var t2 = _right.AsBool(context);
        return context.CreateExpr(c => c.MkAnd(t1, t2));
    }

    private static IValue ShortCircuit(IValue left, ConstantBool right)
    {
        return right
            ? LogicalNot.Create(LogicalNot.Create(left))
            : right;
    }

    public static IValue Create(IValue left, IValue right)
    {
        return right is IConstantValue r
            ? ShortCircuit(left, r.AsBool())
            : left is IConstantValue l
                ? ShortCircuit(right, l.AsBool())
                : new LogicalAnd(left, right);
    }
}
