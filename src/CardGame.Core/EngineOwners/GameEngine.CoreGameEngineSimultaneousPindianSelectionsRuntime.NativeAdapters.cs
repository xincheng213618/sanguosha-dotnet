#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreSimultaneousPindianSelectionsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianSelectionsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreSimultaneousPindianSelectionsRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianSelectionsRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CoreSimultaneousPindianSelectionsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianSelectionsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreSimultaneousPindianSelectionsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianSelectionsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}
