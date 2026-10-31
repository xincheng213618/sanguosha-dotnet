#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreMatchedJudgmentPlacementsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreMatchedJudgmentPlacementsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreMatchedJudgmentPlacementsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreMatchedJudgmentPlacementsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreMatchedJudgmentPlacementsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreMatchedJudgmentPlacementsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreMatchedJudgmentPlacementsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreMatchedJudgmentPlacementsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
