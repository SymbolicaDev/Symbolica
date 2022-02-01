﻿using System.Numerics;
using Microsoft.Z3;
using Symbolica.Computation.Values;
using Symbolica.Computation.Values.Constants;

namespace Symbolica.Computation;

internal sealed class SymbolicUnsigned : BitVector
{
    private readonly BigInteger _value;

    public SymbolicUnsigned(ConstantUnsigned value)
        : base(value.Size)
    {
        _value = value;
    }

    public override BitVecExpr AsBitVector(Context context)
    {
        return context.MkBV(_value.ToString(), (uint) Size);
    }
}
