﻿using System;
using System.Linq;
using Symbolica;
using Symbolica.Abstraction;
using Symbolica.Computation;
using Symbolica.Implementation;

var bytes = await Serializer.Serialize(args[0], args.LastOrDefault(a => a.StartsWith("--O")) ?? "--O0");
var executor = new Executor(new ContextFactory(), new Options(
    ulong.TryParse(args.SkipWhile(a => a != "--symbolic-file-size").Skip(1).FirstOrDefault(), out var symbolicFileSize) ? symbolicFileSize : 1,
    args.Contains("--use-symbolic-garbage"),
    args.Contains("--use-symbolic-addresses"),
    args.Contains("--use-symbolic-continuations")));

var (executedInstructions, exception) = await executor.Run(bytes);
Console.WriteLine($"Executed {executedInstructions} instructions.");

if (exception != null)
{
    Console.WriteLine(exception.Message);

    if (exception is StateException stateException)
        Console.WriteLine(string.Join(", ", stateException.Space.GetExample().Select(p => $"{p.Key}={p.Value}")));

    return 1;
}
else
{
    Console.WriteLine("No errors were found.");
}

return 0;
