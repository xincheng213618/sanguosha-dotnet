#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreFaultBoundaryCommandSessionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreFaultBoundaryCommandSessionOperations
    {
        public ref readonly global::CardGame.Core.CommandSession _commandSession => ref engine._commandSession;
    }

    [ProgramCapability]
    private sealed class CoreFaultBoundaryKernelCoreGameEngineCommandSessionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreFaultBoundaryKernelCoreGameEngineCommandSessionOperations
    {
        public global::CardGame.Core.ICommandSessionRuntime CommandRuntime { get => engine.CommandRuntime; }
    }
}
