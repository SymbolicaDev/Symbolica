﻿using System.Collections.Generic;
using Microsoft.Z3;
using Symbolica.Expression;

namespace Symbolica.Computation.Values;

internal sealed class Equal : Bool
{
    private readonly IValue _left;
    private readonly IValue _right;

    private Equal(IValue left, IValue right)
    {
        _left = left;
        _right = right;
    }

    public override IEnumerable<IValue> Children => new[] { _left, _right };

    public override string? PrintedValue => null;

    public override BoolExpr AsBool(IContext context)
    {
        return _left is Bool || _right is Bool
            ? context.CreateExpr(c => c.MkEq(_left.AsBool(context), _right.AsBool(context)))
            : context.CreateExpr(c => c.MkEq(_left.AsBitVector(context), _right.AsBitVector(context)));
    }

    public static IValue Create(IValue left, IValue right)
    {
        return (left, right) switch
        {
            (IConstantValue l, IConstantValue r) => l.AsUnsigned().Equal(r.AsUnsigned()),
            (Address<Bits> l, _) => Create(l.Aggregate(), right),
            (Address<Bytes> l, _) => Create(l.Aggregate(), right),
            (_, Address<Bits> r) => Create(left, r.Aggregate()),
            (_, Address<Bytes> r) => Create(left, r.Aggregate()),
            _ => new Equal(left, right)
        };
    }
}
