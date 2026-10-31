#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterZhenJiHappyGodKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhenJiHappyGodKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterZhenJiHappyGodCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhenJiHappyGodCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool SameNameHandObserverSuffix(int index) => engine.SameNameHandDiscardOrDamageObserversEntries.SameNameHandObserverSuffix(index);
    }

    [ProgramCapability]
    private sealed class CharacterZhenJiHappyGodCharacterZhenJiHappyGodAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhenJiHappyGodCharacterZhenJiHappyGodOperations
    {
        public bool HappyZjFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.ZhenJiHappyGodEntries.HappyZjFirstChild(f, child);
    }

    [ProgramCapability]
    private sealed class CharacterZhenJiHappyGodContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhenJiHappyGodContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterZhenJiHappyGodResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhenJiHappyGodResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterZhenJiHappyGodCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhenJiHappyGodCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
