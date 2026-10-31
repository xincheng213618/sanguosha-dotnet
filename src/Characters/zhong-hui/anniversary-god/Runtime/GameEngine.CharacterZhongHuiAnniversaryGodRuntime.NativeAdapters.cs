#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterZhongHuiAnniversaryGodCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhongHuiAnniversaryGodCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterZhongHuiAnniversaryGodKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhongHuiAnniversaryGodKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CharacterZhongHuiAnniversaryGodRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhongHuiAnniversaryGodRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CharacterZhongHuiAnniversaryGodContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhongHuiAnniversaryGodContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterZhongHuiAnniversaryGodResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhongHuiAnniversaryGodResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
