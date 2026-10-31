#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterSunQuanAnniversaryGodContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunQuanAnniversaryGodContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterSunQuanAnniversaryGodCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunQuanAnniversaryGodCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterSunQuanAnniversaryGodRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunQuanAnniversaryGodRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CharacterSunQuanAnniversaryGodKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunQuanAnniversaryGodKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterSunQuanAnniversaryGodResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunQuanAnniversaryGodResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
