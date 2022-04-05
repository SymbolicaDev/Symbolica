﻿using System.Numerics;
using Symbolica.Abstraction;
using Symbolica.Expression;

namespace Symbolica.Representation.Operands;

public sealed class ConstantInteger : IOperand
{
    private readonly Size _size;
    private readonly BigInteger _value;

    public ConstantInteger(Size size, BigInteger value)
    {
        _size = size;
        _value = value;
    }

    public IExpression Evaluate(IState state)
    {
        return state.Space.CreateConstant(_size, _value);
    }
}
