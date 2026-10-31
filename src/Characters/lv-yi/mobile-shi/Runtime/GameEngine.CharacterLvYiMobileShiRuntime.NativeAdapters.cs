#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterLvYiMobileShiKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLvYiMobileShiKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CharacterLvYiMobileShiContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLvYiMobileShiContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterLvYiMobileShiCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLvYiMobileShiCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterLvYiMobileShiResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLvYiMobileShiResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterLvYiMobileShiRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLvYiMobileShiRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }
}
