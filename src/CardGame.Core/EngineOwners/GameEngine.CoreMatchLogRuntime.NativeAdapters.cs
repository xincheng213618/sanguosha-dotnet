#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreMatchLogKernelCoreGameEngineObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreMatchLogKernelCoreGameEngineObserversOperations
    {
        public global::CardGame.Core.ObserverDispatcher Observers { get => engine.Observers; }
    }
}
