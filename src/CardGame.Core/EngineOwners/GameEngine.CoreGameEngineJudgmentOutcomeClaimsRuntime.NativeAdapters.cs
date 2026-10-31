#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreJudgmentOutcomeClaimsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreJudgmentOutcomeClaimsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreJudgmentOutcomeClaimsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreJudgmentOutcomeClaimsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreJudgmentOutcomeClaimsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreJudgmentOutcomeClaimsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreJudgmentOutcomeClaimsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreJudgmentOutcomeClaimsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
