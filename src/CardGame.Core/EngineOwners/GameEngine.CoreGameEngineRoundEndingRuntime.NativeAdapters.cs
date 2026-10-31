#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreRoundEndingModeRulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRoundEndingModeRulesOperations
    {
        public ref readonly global::CardGame.Core.ModeRuleSet _modeRules => ref engine._modeRules;
    }

    [ProgramCapability]
    private sealed class CoreRoundEndingPendingCardsMovedBatchesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRoundEndingPendingCardsMovedBatchesOperations
    {
        public ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CardMovementBatchContext> _pendingCardsMovedBatches => ref engine._pendingCardsMovedBatches;
    }

    [ProgramCapability]
    private sealed class CoreRoundEndingContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRoundEndingContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreRoundEndingResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRoundEndingResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreRoundEndingKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRoundEndingKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}
