﻿using System;
using System.Collections.Generic;
using Microsoft.Z3;
using Symbolica.Expression;

namespace Symbolica.Computation.Values;

internal sealed record UnsignedLessOrEqual : Bool
{
    private readonly IValue _left;
    private readonly IValue _right;

    private UnsignedLessOrEqual(IValue left, IValue right)
    {
        _left = left;
        _right = right;
    }

    public override BoolExpr AsBool(ISolver solver)
    {
        using var left = _left.AsBitVector(solver);
        using var right = _right.AsBitVector(solver);
        return solver.Context.MkBVULE(left, right);
    }

    public override bool Equals(IValue? other)
    {
        return Equals(other as UnsignedLessOrEqual);
    }

    public static IValue Create(IValue left, IValue right)
    {
        return left is IConstantValue l && right is IConstantValue r
            ? l.AsUnsigned().LessOrEqual(r.AsUnsigned())
            : new UnsignedLessOrEqual(left, right);
    }

    public override (HashSet<(IValue, IValue)> subs, bool) IsEquivalentTo(IValue other)
    {
        return other is UnsignedLessOrEqual v
            ? _left.IsEquivalentTo(v._left)
                .And(_right.IsEquivalentTo(v._right))
            : (new(), false);
    }

    public override IValue Substitute(IReadOnlyDictionary<IValue, IValue> subs)
    {
        return subs.TryGetValue(this, out var sub)
            ? sub
            : Create(_left.Substitute(subs), _right.Substitute(subs));
    }

    public override object ToJson()
    {
        return new
        {
            Type = GetType().Name,
            Size = (uint) Size,
            Left = _left.ToJson(),
            Right = _right.ToJson()
        };
    }

    public override int GetEquivalencyHash()
    {
        return HashCode.Combine(GetType().Name, _left.GetEquivalencyHash(), _right.GetEquivalencyHash());
    }
}
