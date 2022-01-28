using System.Collections.Generic;
using System.Numerics;
using Microsoft.Z3;
using Symbolica.Expression;

namespace Symbolica.Computation;

internal abstract class Integer : IValue
{
    protected Integer(Bits size)
    {
        Size = size;
    }

    public Bits Size { get; }
    public abstract IEnumerable<IValue> Children { get; }
    public abstract string? PrintedValue { get; }

    public BigInteger AsConstant(IContext context)
    {
        return AsBitVector(context).AsConstant();
    }

    public abstract BitVecExpr AsBitVector(IContext context);
    public abstract BoolExpr AsBool(IContext context);

    public FPExpr AsFloat(IContext context)
    {
        return context.CreateExpr(c => c.MkFPToFP(AsBitVector(context), Size.GetSort(context)));
    }
}
