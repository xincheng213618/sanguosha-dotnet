#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterBaoXinOlCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterBaoXinOlCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterBaoXinOlResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterBaoXinOlResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterBaoXinOlKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterBaoXinOlKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterBaoXinOlContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterBaoXinOlContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterBaoXinOlCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterBaoXinOlCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool SameNameHandObserverSuffix(int index) => engine.SameNameHandDiscardOrDamageObserversEntries.SameNameHandObserverSuffix(index);
    }

    [ProgramCapability]
    private sealed class CharacterBaoXinOlObservedSkillGrantRevisionsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterBaoXinOlObservedSkillGrantRevisionsOperations
    {
        public ref readonly global::System.Collections.Generic.Dictionary<int, long> _observedSkillGrantRevisions => ref engine._observedSkillGrantRevisions;
    }

    [ProgramCapability]
    private sealed class CharacterBaoXinOlRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterBaoXinOlRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CharacterBaoXinOlCharacterBaoXinOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterBaoXinOlCharacterBaoXinOlOperations
    {
        public bool BaoXinFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.BaoXinOlEntries.BaoXinFirstChild(f, child);
    }
}
