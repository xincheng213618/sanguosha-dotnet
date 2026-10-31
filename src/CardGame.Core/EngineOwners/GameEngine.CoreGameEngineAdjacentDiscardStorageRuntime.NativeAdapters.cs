#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreAdjacentDiscardStorageCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAdjacentDiscardStorageCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreAdjacentDiscardStorageContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAdjacentDiscardStorageContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreAdjacentDiscardStorageResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAdjacentDiscardStorageResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreAdjacentDiscardStorageKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAdjacentDiscardStorageKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}
