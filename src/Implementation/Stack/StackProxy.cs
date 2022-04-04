using System.Collections.Generic;
using Symbolica.Abstraction;
using Symbolica.Expression;
using Symbolica.Expression.Values;
using Symbolica.Implementation.Memory;

namespace Symbolica.Implementation.Stack;

internal sealed class StackProxy : IStackProxy
{
    private readonly IMemoryProxy _memory;
    private readonly ISpace _space;
    private IPersistentStack _stack;

    public StackProxy(ISpace space, IMemoryProxy memory, IPersistentStack stack)
    {
        _space = space;
        _memory = memory;
        _stack = stack;
    }

    public bool IsInitialFrame => _stack.IsInitialFrame;
    public BasicBlockId PredecessorId => _stack.PredecessorId;
    public IEnumerable<string> Trace => _stack.StackTrace;

    public IStackProxy Clone(ISpace space, IMemoryProxy memory)
    {
        return new StackProxy(space, memory, _stack);
    }

    public void ExecuteNextInstruction(IState state)
    {
        _stack = _stack.MoveNextInstruction();
        _stack.Instruction.Execute(state);
    }

    public void Wind(ICaller caller, IInvocation invocation)
    {
        _stack = _stack.Wind(_space, _memory, caller, invocation);
    }

    public ICaller Unwind()
    {
        var (caller, stack) = _stack.Unwind(_memory);
        _stack = stack;

        return caller;
    }

    public void Save(Address address, bool useJumpBuffer)
    {
        _stack = _stack.Save(_memory, address, useJumpBuffer);
    }

    public InstructionId Restore(Address address, bool useJumpBuffer)
    {
        _stack = _stack.Restore(_space, _memory, address, useJumpBuffer);

        return _stack.Instruction.Id;
    }

    public void TransferBasicBlock(BasicBlockId id)
    {
        _stack = _stack.TransferBasicBlock(id);
    }

    public IExpression<IType> GetFormal(int index)
    {
        return _stack.GetFormal(index);
    }

    public IExpression<IType> GetInitializedVaList()
    {
        return _stack.GetInitializedVaList(_space);
    }

    public IExpression<IType> GetVariable(InstructionId id, bool useIncomingValue)
    {
        return _stack.GetVariable(id, useIncomingValue);
    }

    public void SetVariable(InstructionId id, IExpression<IType> variable)
    {
        _stack = _stack.SetVariable(id, variable);
    }

    public Address Allocate(Bits size)
    {
        var (address, stack) = _stack.Allocate(_memory, size);
        _stack = stack;

        return address;
    }
}
