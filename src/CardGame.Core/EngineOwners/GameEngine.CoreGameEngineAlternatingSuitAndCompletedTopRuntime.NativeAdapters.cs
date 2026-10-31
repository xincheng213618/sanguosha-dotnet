#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreAlternatingSuitAndCompletedTopContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAlternatingSuitAndCompletedTopContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreAlternatingSuitAndCompletedTopCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAlternatingSuitAndCompletedTopCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreAlternatingSuitAndCompletedTopKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAlternatingSuitAndCompletedTopKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreAlternatingSuitAndCompletedTopResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAlternatingSuitAndCompletedTopResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
