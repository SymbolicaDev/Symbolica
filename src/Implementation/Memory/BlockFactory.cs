﻿using Symbolica.Expression;
using Symbolica.Implementation.Exceptions;

namespace Symbolica.Implementation.Memory;

internal sealed class BlockFactory : IBlockFactory
{
    public IPersistentBlock Create(ISpace space, Section section, IExpression address, Bits size)
    {
        return new PersistentBlock(section, address, space.CreateGarbage(size));
    }

    public IPersistentBlock CreateInvalid()
    {
        return InvalidBlock.Instance;
    }

    private sealed class InvalidBlock : IPersistentBlock
    {
        private InvalidBlock()
        {
        }

        public static IPersistentBlock Instance => new InvalidBlock();

        public bool IsValid => false;
        public IExpression Address => throw new ImplementationException("Invalid block has no address.");
        public IExpression Data => throw new ImplementationException("Invalid block has no data.");

        public IPersistentBlock Move(IExpression address, Bits size)
        {
            return this;
        }

        public bool CanFree(ISpace space, Section section, IExpression address)
        {
            return false;
        }

        public Result TryWrite(ISpace space, IExpression address, IExpression value)
        {
            return Result.Failure(space);
        }

        public Result TryRead(ISpace space, IExpression address, Bits size)
        {
            return Result.Failure(space);
        }
    }
}
