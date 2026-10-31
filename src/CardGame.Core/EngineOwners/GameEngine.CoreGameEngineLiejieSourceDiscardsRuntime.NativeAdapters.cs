#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreLiejieSourceDiscardsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreLiejieSourceDiscardsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreLiejieSourceDiscardsCoreLiejieSourceDiscardsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreLiejieSourceDiscardsCoreLiejieSourceDiscardsOperations
    {
        public bool LiejieSourceDiscardFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => engine.LiejieSourceDiscardsEntries.LiejieSourceDiscardFirstChild(frame, child);
    }

    [ProgramCapability]
    private sealed class CoreLiejieSourceDiscardsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreLiejieSourceDiscardsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreLiejieSourceDiscardsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreLiejieSourceDiscardsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreLiejieSourceDiscardsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreLiejieSourceDiscardsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreLiejieSourceDiscardsCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreLiejieSourceDiscardsCoreObserverSubtreesOperations2
    {
        public bool HasPaidDamageObserverSuffix(int rootIndex) => engine.ObserverSubtreesEntries.HasPaidDamageObserverSuffix(rootIndex);
    }
}
