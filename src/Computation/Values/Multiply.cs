using System;
using System.Collections.Generic;
using Microsoft.Z3;
using Symbolica.Computation.Values.Constants;
using Symbolica.Expression;

namespace Symbolica.Computation.Values;

internal sealed class Multiply : BitVector
{
    private readonly IValue _left;
    private readonly IValue _right;

    private Multiply(IValue left, IValue right)
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
        return context.CreateExpr(c => c.MkBVMul(t1, t2));
    }

    private static IValue ShortCircuit(IValue left, ConstantUnsigned right)
    {
        return right.IsZero
            ? right
            : right.IsOne
                ? left
                : Create(left, right);
    }

    private static IValue Create(IValue left, ConstantUnsigned right)
    {
        return left switch
        {
            IConstantValue l => l.AsUnsigned().Multiply(right),
            Multiply l => Create(l._left, Create(l._right, right)),
            Address<Bits> l => l.Multiply(right),
            Address<Bytes> l => l.Multiply(right),
            _ => new Multiply(left, right)
        };
    }

    public static IValue Create(IValue left, IValue right)
    {
        return (left, right) switch
        {
            (IConstantValue l, _) => ShortCircuit(right, l.AsUnsigned()),
            (_, IConstantValue r) => ShortCircuit(left, r.AsUnsigned()),
            (Address<Bits> l, Address<Bits> r) => Create(r.Aggregate(), l.Aggregate()),
            (Address<Bytes> l, Address<Bytes> r) => Create(r.Aggregate(), l.Aggregate()),
            (Address<Bits>, Address<Bytes>) => throw new Exception("Cannot multiply addresses of differrent size types"),
            (Address<Bytes>, Address<Bits>) => throw new Exception("Cannot multiply addresses of differrent size types"),
            (Address<Bits> l, _) => Create(l.Aggregate(), right),
            (Address<Bytes> l, _) => Create(l.Aggregate(), right),
            (_, Address<Bits> r) => Create(left, r.Aggregate()),
            (_, Address<Bytes> r) => Create(left, r.Aggregate()),
            _ => new Multiply(left, right)
        };
    }
}
