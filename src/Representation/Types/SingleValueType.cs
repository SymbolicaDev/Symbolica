﻿using System;
using Symbolica.Abstraction;
using Symbolica.Expression;

namespace Symbolica.Representation.Types;

public sealed class SingleValueType : IType
{
    public SingleValueType(Bits size)
    {
        Size = size;
    }

    public Bits Size { get; }

    public IType GetType(ISpace space, IExpression index)
    {
        throw new NotImplementedException();
    }

    public IExpression GetOffsetBits(ISpace space, IExpression index)
    {
        throw new NotImplementedException();
    }

    public IExpression GetOffsetBytes(ISpace space, IExpression index)
    {
        throw new NotImplementedException();
    }
}
