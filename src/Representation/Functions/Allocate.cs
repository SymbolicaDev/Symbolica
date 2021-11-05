﻿using System.Numerics;
using Symbolica.Abstraction;
using Symbolica.Expression;

namespace Symbolica.Representation.Functions
{
    internal sealed class Allocate : IFunction
    {
        public Allocate(FunctionId id, IParameters parameters)
        {
            Id = id;
            Parameters = parameters;
        }

        public FunctionId Id { get; }
        public IParameters Parameters { get; }

        public void Call(IState state, ICaller caller, IArguments arguments)
        {
            var size = arguments.Get(0);

            state.ForkAll(size, new CallAction(caller.Id));
        }

        private class CallAction : IForkAllAction
        {
            private readonly InstructionId _callerId;

            public CallAction(InstructionId callerId)
            {
                _callerId = callerId;
            }

            public IStateAction Run(BigInteger value) =>
                new SetVariable(_callerId, (Bytes)(uint)value);
        }

        private class SetVariable : IStateAction
        {
            private readonly InstructionId _callerId;
            private readonly Bytes _size;

            public SetVariable(InstructionId callerId, Bytes size)
            {
                _callerId = callerId;
                _size = size;
            }

            public Unit Run(IState state)
            {
                var address = _size == Bytes.Zero
                    ? state.Space.CreateConstant(state.Space.PointerSize, BigInteger.Zero)
                    : state.Memory.Allocate(_size.ToBits());

                state.Stack.SetVariable(_callerId, address);
                return new Unit();
            }
        }
    }
}
