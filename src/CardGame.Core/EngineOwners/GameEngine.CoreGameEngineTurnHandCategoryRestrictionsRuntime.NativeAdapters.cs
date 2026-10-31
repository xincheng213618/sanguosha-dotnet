#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreTurnHandCategoryRestrictionsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTurnHandCategoryRestrictionsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreTurnHandCategoryRestrictionsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTurnHandCategoryRestrictionsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreTurnHandCategoryRestrictionsTurnCardUseEffectsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTurnHandCategoryRestrictionsTurnCardUseEffectsOperations
    {
        public ref readonly global::CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects => ref engine._turnCardUseEffects;
    }

    [ProgramCapability]
    private sealed class CoreTurnHandCategoryRestrictionsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTurnHandCategoryRestrictionsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreTurnHandCategoryRestrictionsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTurnHandCategoryRestrictionsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
