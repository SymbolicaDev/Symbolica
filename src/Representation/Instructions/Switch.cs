﻿using Symbolica.Abstraction;
using Symbolica.Expression;

namespace Symbolica.Representation.Instructions;

public sealed class Switch : IInstruction
{
    private readonly IOperand[] _operands;

    public Switch(InstructionId id, IOperand[] operands)
    {
        Id = id;
        _operands = operands;
    }

    public InstructionId Id { get; }

    public void Execute(IState state)
    {
        var expression = _operands[0].Evaluate(state);

        var action = new Transfer(expression, 2, _operands);
        action.Invoke(state);
    }

    private sealed class Transfer : IStateAction
    {
        private readonly IExpression<IType> _expression;
        private readonly int _index;
        private readonly IOperand[] _operands;

        public Transfer(IExpression<IType> expression, int index, IOperand[] operands)
        {
            _expression = expression;
            _index = index;
            _operands = operands;
        }

        public void Invoke(IState state)
        {
            if (_index < _operands.Length)
                TransferCase(state, _expression, _index);
            else
                TransferDefault(state);
        }

        private void TransferCase(IState state, IExpression<IType> expression, int index)
        {
            var value = _operands[index].Evaluate(state);
            var successorId = (BasicBlockId) (ulong) state.Space.GetSingleValue(_operands[index + 1].Evaluate(state));

            var isEqual = Expression.Values.Equal.Create(expression, value);

            state.Fork(isEqual,
                new TransferBasicBlock(successorId),
                new Transfer(expression, index + 2, _operands));
        }

        private void TransferDefault(IState state)
        {
            var successorId = (BasicBlockId) (ulong) state.Space.GetSingleValue(_operands[1].Evaluate(state));

            state.Stack.TransferBasicBlock(successorId);
        }
    }

    private sealed class TransferBasicBlock : IStateAction
    {
        private readonly BasicBlockId _id;

        public TransferBasicBlock(BasicBlockId id)
        {
            _id = id;
        }

        public void Invoke(IState state)
        {
            state.Stack.TransferBasicBlock(_id);
        }
    }
}
