#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreExtraEffectsAndRankGuessingContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreExtraEffectsAndRankGuessingContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreExtraEffectsAndRankGuessingKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreExtraEffectsAndRankGuessingKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreExtraEffectsAndRankGuessingResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreExtraEffectsAndRankGuessingResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreExtraEffectsAndRankGuessingCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreExtraEffectsAndRankGuessingCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
