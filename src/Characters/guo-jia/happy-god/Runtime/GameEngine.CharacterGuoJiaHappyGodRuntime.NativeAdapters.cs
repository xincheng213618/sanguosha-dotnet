#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterGuoJiaHappyGodResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuoJiaHappyGodResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterGuoJiaHappyGodContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuoJiaHappyGodContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterGuoJiaHappyGodKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuoJiaHappyGodKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CharacterGuoJiaHappyGodCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuoJiaHappyGodCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterGuoJiaHappyGodCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuoJiaHappyGodCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool SameNameHandObserverSuffix(int index) => engine.SameNameHandDiscardOrDamageObserversEntries.SameNameHandObserverSuffix(index);
    }

    [ProgramCapability]
    private sealed class CharacterGuoJiaHappyGodCharacterGuoJiaHappyGodAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuoJiaHappyGodCharacterGuoJiaHappyGodOperations
    {
        public bool HappyGjFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.GuoJiaHappyGodEntries.HappyGjFirstChild(f, child);
    }
}
