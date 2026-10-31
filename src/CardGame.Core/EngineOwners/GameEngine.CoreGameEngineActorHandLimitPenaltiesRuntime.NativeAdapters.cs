#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreActorHandLimitPenaltiesResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActorHandLimitPenaltiesResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreActorHandLimitPenaltiesContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActorHandLimitPenaltiesContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreActorHandLimitPenaltiesCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActorHandLimitPenaltiesCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreActorHandLimitPenaltiesKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActorHandLimitPenaltiesKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}
