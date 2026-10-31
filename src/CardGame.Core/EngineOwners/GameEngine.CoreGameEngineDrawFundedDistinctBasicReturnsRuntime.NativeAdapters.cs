#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreDrawFundedDistinctBasicReturnsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicReturnsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreDrawFundedDistinctBasicReturnsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicReturnsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreDrawFundedDistinctBasicReturnsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicReturnsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreDrawFundedDistinctBasicReturnsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicReturnsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}
