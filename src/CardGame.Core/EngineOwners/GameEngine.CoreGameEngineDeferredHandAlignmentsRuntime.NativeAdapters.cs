#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreDeferredHandAlignmentsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDeferredHandAlignmentsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreDeferredHandAlignmentsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDeferredHandAlignmentsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreDeferredHandAlignmentsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDeferredHandAlignmentsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreDeferredHandAlignmentsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDeferredHandAlignmentsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
