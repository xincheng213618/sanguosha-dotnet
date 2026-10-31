#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreModeAllegianceAllegiancePolicyAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreModeAllegianceAllegiancePolicyOperations
    {
        public ref global::CardGame.Core.IMatchModeAllegiancePolicy? _allegiancePolicy => ref engine._allegiancePolicy;
    }

    [ProgramCapability]
    private sealed class CoreModeAllegianceModeRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreModeAllegianceModeRuntimeOperations
    {
        public ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime => ref engine._modeRuntime;
    }
}
