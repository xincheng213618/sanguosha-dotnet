#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreSameTypeActualUseAidResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSameTypeActualUseAidResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreSameTypeActualUseAidCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSameTypeActualUseAidCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreSameTypeActualUseAidKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSameTypeActualUseAidKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreSameTypeActualUseAidContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSameTypeActualUseAidContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
