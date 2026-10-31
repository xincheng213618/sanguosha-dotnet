#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreUnrespondableCounterspellsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreUnrespondableCounterspellsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreUnrespondableCounterspellsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreUnrespondableCounterspellsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreUnrespondableCounterspellsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreUnrespondableCounterspellsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreUnrespondableCounterspellsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreUnrespondableCounterspellsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
