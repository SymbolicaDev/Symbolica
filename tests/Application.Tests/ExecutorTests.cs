﻿using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Symbolica.Abstraction;
using Symbolica.Implementation;
using Xunit;

namespace Symbolica.Application
{
    public class ExecutorTests
    {
        [Theory]
        [ClassData(typeof(FailTestData))]
        private async Task ShouldFail(string directory, Options options)
        {
            var result = await Executor.Run(directory, options);

            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<StateException>()
                .Which.Space.GetExample().Select(p => p.Key).Should().BeEquivalentTo(
                    await File.ReadAllLinesAsync(Path.Combine(directory, "symbols")));
        }

        [Theory]
        [ClassData(typeof(PassTestData))]
        private async Task ShouldPass(string directory, Options options)
        {
            var result = await Executor.Run(directory, options);

            result.IsSuccess.Should().BeTrue();
        }

        private sealed class FailTestData : TestData
        {
            public FailTestData()
                : base("fail")
            {
            }
        }

        private sealed class PassTestData : TestData
        {
            public PassTestData()
                : base("pass")
            {
            }
        }

        private class TestData : TheoryData<string, Options>
        {
            protected TestData(string status)
            {
                foreach (var directory in Directory.EnumerateDirectories(Path.Combine("..", "..", "..", "..", status)))
                foreach (var useSymbolicGarbage in new[] {false, true})
                foreach (var useSymbolicAddresses in new[] {false, true})
                foreach (var useSymbolicContinuations in new[] {false, true})
                    Add(directory, new Options(useSymbolicGarbage, useSymbolicAddresses, useSymbolicContinuations));
            }
        }
    }
}
