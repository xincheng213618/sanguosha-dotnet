#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CorePaidColorConversionsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePaidColorConversionsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CorePaidColorConversionsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePaidColorConversionsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CorePaidColorConversionsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePaidColorConversionsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CorePaidColorConversionsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePaidColorConversionsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CorePaidColorConversionsTurnCardUseEffectsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePaidColorConversionsTurnCardUseEffectsOperations
    {
        public ref readonly global::CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects => ref engine._turnCardUseEffects;
    }
}
