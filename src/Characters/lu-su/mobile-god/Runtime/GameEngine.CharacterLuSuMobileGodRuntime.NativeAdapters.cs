#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterLuSuMobileGodRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuSuMobileGodRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CharacterLuSuMobileGodKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuSuMobileGodKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CharacterLuSuMobileGodContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuSuMobileGodContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterLuSuMobileGodResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuSuMobileGodResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterLuSuMobileGodCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuSuMobileGodCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
