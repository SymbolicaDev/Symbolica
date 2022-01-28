﻿using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.Z3;
using Symbolica.Collection;
using Symbolica.Expression;

namespace Symbolica.Computation.Values.Constants;

internal sealed class ConstantBitVector : BitVector, IConstantValue
{
    private readonly IPersistentList<byte> _value;

    private ConstantBitVector(Bits size, IPersistentList<byte> value)
        : base(size)
    {
        _value = value;
    }

    public override IEnumerable<IValue> Children => Enumerable.Empty<IValue>();

    public override string? PrintedValue => $"BitVector of size {_value.Count}";

    public override BitVecExpr AsBitVector(IContext context)
    {
        return AsUnsigned().AsBitVector(context);
    }

    public ConstantBitVector AsBitVector(ICollectionFactory collectionFactory)
    {
        return this;
    }

    public ConstantUnsigned AsUnsigned()
    {
        return ConstantUnsigned.Create(Size, new BigInteger(_value.ToArray(), true));
    }

    public ConstantSigned AsSigned()
    {
        return AsUnsigned().AsSigned();
    }

    public ConstantBool AsBool()
    {
        return AsUnsigned().AsBool();
    }

    public ConstantSingle AsSingle()
    {
        return AsSigned().AsSingle();
    }

    public ConstantDouble AsDouble()
    {
        return AsSigned().AsDouble();
    }

    public ConstantBitVector Read(ConstantUnsigned offset, Bits size)
    {
        return new(size, _value.GetRange(GetIndex(offset), GetCount(size)));
    }

    public ConstantBitVector Write(ConstantUnsigned offset, ConstantBitVector value)
    {
        return new(Size, _value.SetRange(GetIndex(offset), value._value));
    }

    public static ConstantBitVector Create(ICollectionFactory collectionFactory, ConstantUnsigned value)
    {
        return new ConstantBitVector(value.Size,
            collectionFactory.CreatePersistentList<byte>().AddRange(GetBytes(value.Size, value)));
    }

    private static IEnumerable<byte> GetBytes(Bits size, BigInteger value)
    {
        var bytes = new byte[GetCount(size)];
        value.TryWriteBytes(bytes, out _, true);

        return bytes;
    }

    private static int GetIndex(BigInteger offset)
    {
        return (int) (uint) ((Bits) (uint) offset).ToBytes();
    }

    private static int GetCount(Bits size)
    {
        return (int) (uint) size.ToBytes();
    }
}
