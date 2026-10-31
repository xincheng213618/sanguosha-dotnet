#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreForeignActualTurnWindowRuntimePortsKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreForeignActualTurnWindowRuntimePortsKernelCoreMainOperations
    {
        public void CompleteGame() => engine.CompleteGame();
    }
}
