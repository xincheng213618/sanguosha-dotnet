#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreVirtualOrdinaryTrickUsesCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreVirtualOrdinaryTrickUsesResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreVirtualOrdinaryTrickUsesKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreVirtualOrdinaryTrickUsesContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
