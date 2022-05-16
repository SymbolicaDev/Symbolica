﻿using System.Collections.Generic;
using Microsoft.Z3;
using Symbolica.Expression;

namespace Symbolica.Computation.Values;

internal sealed record UnsignedRemainder : BitVector
{
    private readonly IValue _left;
    private readonly IValue _right;

    private UnsignedRemainder(IValue left, IValue right)
        : base(left.Size)
    {
        _left = left;
        _right = right;
    }

    public override BitVecExpr AsBitVector(ISolver solver)
    {
        using var left = _left.AsBitVector(solver);
        using var right = _right.AsBitVector(solver);
        return solver.Context.MkBVURem(left, right);
    }

    public override bool Equals(IValue? other)
    {
        return Equals(other as UnsignedRemainder);
    }

    public static IValue Create(IValue left, IValue right)
    {
        return left is IConstantValue l && right is IConstantValue r
            ? l.AsUnsigned().Remainder(r.AsUnsigned())
            : new UnsignedRemainder(left, right);
    }

    public override (HashSet<(IValue, IValue)> subs, bool) IsEquivalentTo(IValue other)
    {
        return other is UnsignedRemainder v
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
}
