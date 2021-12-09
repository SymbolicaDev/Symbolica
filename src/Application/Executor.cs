﻿using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Symbolica.Application.Collection;
using Symbolica.Application.Computation;
using Symbolica.Application.Implementation;
using Symbolica.Computation;
using Symbolica.Deserialization;
using Symbolica.Implementation;
using Symbolica.Implementation.System;
using Symbolica.Representation;

namespace Symbolica.Application
{
    internal sealed class Executor
    {
        private readonly IContextFactory _contextFactory;
        private readonly Options _options;

        public Executor(IContextFactory contextFactory, Options options)
        {
            _contextFactory = contextFactory;
            _options = options;
        }

        public async Task<(ulong, Exception?)> Run(byte[] bytes)
        {
            var module = DeserializerFactory.Create(new DeclarationFactory()).DeserializeModule(bytes);

            var collectionFactory = new CollectionFactory();

            var spaceFactory = new SpaceFactory(
                new SymbolFactory(), new ModelFactory(),
                _contextFactory, collectionFactory);

            var executableFactory = new ExecutableFactory(CreateFileSystem(), spaceFactory, collectionFactory);

            using var statePool = new StatePool();
            statePool.Add(executableFactory.CreateInitial(module, _options));

            return await statePool.Wait();
        }

        private static IFileSystem CreateFileSystem()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? new WslFileSystem(new FileSystem())
                : new FileSystem();
        }
    }
}
