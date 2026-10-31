#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreDyingOwnedCardSelectionsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDyingOwnedCardSelectionsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreDyingOwnedCardSelectionsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDyingOwnedCardSelectionsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreDyingOwnedCardSelectionsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDyingOwnedCardSelectionsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreDyingOwnedCardSelectionsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDyingOwnedCardSelectionsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
