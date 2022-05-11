﻿using Symbolica.Abstraction;
using Symbolica.Expression;

namespace Symbolica.Representation.Functions;

internal sealed class VariadicStart : IFunction
{
    public VariadicStart(FunctionId id, IParameters parameters)
    {
        Id = id;
        Parameters = parameters;
    }

    public FunctionId Id { get; }
    public IParameters Parameters { get; }

    public void Call(IExpressionFactory exprFactory, IState state, ICaller caller, IArguments arguments)
    {
        var address = arguments.Get(0);

        state.Memory.Write(state.Space, address, state.Stack.GetInitializedVaList(state.Space));
    }
}
