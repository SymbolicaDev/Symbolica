﻿using System.Collections.Generic;
using Microsoft.Z3;
using Symbolica.Expression;

namespace Symbolica.Computation.Values;

internal sealed record RealToSigned : BitVector
{
    private readonly IRealValue _value;

    public RealToSigned(Bits size, IRealValue value)
        : base(size)
    {
        _value = value;
    }

    public override BitVecExpr AsBitVector(ISolver solver)
    {
        using var value = _value.AsReal(solver);
        using var intValue = solver.Context.MkReal2Int(value);
        return solver.Context.MkInt2BV((uint) Size, intValue);
    }

    public override bool Equals(IValue? other)
    {
        return Equals(other as RealToSigned);
    }

    public override (HashSet<(IValue, IValue)> subs, bool) IsEquivalentTo(IValue other)
    {
        return other is RealToSigned v
            ? _value.IsEquivalentTo(v._value)
                .And((new(), Size == v.Size))
            : (new(), false);
    }

    public override IValue Substitute(IReadOnlyDictionary<IValue, IValue> subs)
    {
        return subs.TryGetValue(this, out var sub)
            ? sub
            : this;
    }
}
