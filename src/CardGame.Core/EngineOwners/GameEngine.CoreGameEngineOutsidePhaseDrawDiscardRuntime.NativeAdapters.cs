#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreOutsidePhaseDrawDiscardKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOutsidePhaseDrawDiscardKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreOutsidePhaseDrawDiscardResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOutsidePhaseDrawDiscardResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreOutsidePhaseDrawDiscardCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOutsidePhaseDrawDiscardCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreOutsidePhaseDrawDiscardContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOutsidePhaseDrawDiscardContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
