#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreModeEncountersKernelCoreGameEngineModeEncountersAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreModeEncountersKernelCoreGameEngineModeEncountersOperations
    {
        public global::CardGame.Core.NativeOwners.ModeEncounterHost CreateModeEncounterHost() => engine.ComposeNativeModeEncounterHost();
    }
}
