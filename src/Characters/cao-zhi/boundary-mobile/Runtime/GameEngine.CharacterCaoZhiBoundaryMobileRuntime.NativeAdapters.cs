#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterCaoZhiBoundaryMobileResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoZhiBoundaryMobileResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterCaoZhiBoundaryMobileContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoZhiBoundaryMobileContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterCaoZhiBoundaryMobileRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoZhiBoundaryMobileRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CharacterCaoZhiBoundaryMobileKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoZhiBoundaryMobileKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CharacterCaoZhiBoundaryMobileCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoZhiBoundaryMobileCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
