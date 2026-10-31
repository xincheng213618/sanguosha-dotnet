#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreHellEncounterHostKernelCoreGameEngineHellEncounterHostAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreHellEncounterHostKernelCoreGameEngineHellEncounterHostOperations
    {
        public global::CardGame.Core.NativeOwners.HellEncounterHost CreateHellEncounterHost() => engine.ComposeNativeHellEncounterHost();
    }

    [ProgramCapability]
    private sealed class CoreHellEncounterHostModeRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreHellEncounterHostModeRuntimeOperations
    {
        public ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime => ref engine._modeRuntime;
    }
}
