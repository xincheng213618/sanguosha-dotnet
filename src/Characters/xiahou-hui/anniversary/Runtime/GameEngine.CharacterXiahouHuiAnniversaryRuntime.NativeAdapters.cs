#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterXiahouHuiAnniversaryResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXiahouHuiAnniversaryResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterXiahouHuiAnniversaryRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXiahouHuiAnniversaryRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CharacterXiahouHuiAnniversaryContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXiahouHuiAnniversaryContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterXiahouHuiAnniversaryKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXiahouHuiAnniversaryKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterXiahouHuiAnniversaryCharacterXiahouHuiAnniversaryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXiahouHuiAnniversaryCharacterXiahouHuiAnniversaryOperations
    {
        public bool XiahouHuiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.XiahouHuiAnniversaryEntries.XiahouHuiFirstChild(f, child);
    }

    [ProgramCapability]
    private sealed class CharacterXiahouHuiAnniversaryCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXiahouHuiAnniversaryCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool SameNameHandObserverSuffix(int index) => engine.SameNameHandDiscardOrDamageObserversEntries.SameNameHandObserverSuffix(index);
    }

    [ProgramCapability]
    private sealed class CharacterXiahouHuiAnniversaryCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXiahouHuiAnniversaryCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
