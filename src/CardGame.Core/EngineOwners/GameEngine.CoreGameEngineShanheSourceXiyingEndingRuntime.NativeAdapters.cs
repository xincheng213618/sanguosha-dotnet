#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreShanheSourceXiyingEndingContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheSourceXiyingEndingContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreShanheSourceXiyingEndingRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheSourceXiyingEndingRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CoreShanheSourceXiyingEndingResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheSourceXiyingEndingResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreShanheSourceXiyingEndingCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheSourceXiyingEndingCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreShanheSourceXiyingEndingKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheSourceXiyingEndingKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}
