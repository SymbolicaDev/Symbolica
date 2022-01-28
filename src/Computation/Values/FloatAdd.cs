using System.Collections.Generic;
using Microsoft.Z3;
using Symbolica.Computation.Values.Constants;

namespace Symbolica.Computation.Values;

internal sealed class FloatAdd : Float
{
    private readonly IValue _left;
    private readonly IValue _right;

    private FloatAdd(IValue left, IValue right)
        : base(left.Size)
    {
        _left = left;
        _right = right;
    }

    public override IEnumerable<IValue> Children => new[] { _left, _right };

    public override string? PrintedValue => null;

    public override FPExpr AsFloat(IContext context)
    {
        return context.CreateExpr(c => c.MkFPAdd(c.MkFPRNE(), _left.AsFloat(context), _right.AsFloat(context)));
    }

    public static IValue Create(IValue left, IValue right)
    {
        return Binary(left, right,
            (l, r) => new ConstantSingle(l + r),
            (l, r) => new ConstantDouble(l + r),
            (l, r) => new FloatAdd(l, r));
    }
}
