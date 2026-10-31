#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreDrawDiscardCategoryRefundResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDrawDiscardCategoryRefundResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreDrawDiscardCategoryRefundCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDrawDiscardCategoryRefundCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreDrawDiscardCategoryRefundContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDrawDiscardCategoryRefundContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreDrawDiscardCategoryRefundKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDrawDiscardCategoryRefundKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}
