#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterPeiYuanShaoOlCharacterPeiYuanShaoOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterPeiYuanShaoOlCharacterPeiYuanShaoOlOperations
    {
        public bool FulueFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.PeiYuanShaoOlEntries.FulueFirstChild(f, child);
    }

    [ProgramCapability]
    private sealed class CharacterPeiYuanShaoOlCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterPeiYuanShaoOlCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool SameNameHandObserverSuffix(int index) => engine.SameNameHandDiscardOrDamageObserversEntries.SameNameHandObserverSuffix(index);
    }

    [ProgramCapability]
    private sealed class CharacterPeiYuanShaoOlResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterPeiYuanShaoOlResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterPeiYuanShaoOlCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterPeiYuanShaoOlCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterPeiYuanShaoOlContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterPeiYuanShaoOlContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterPeiYuanShaoOlKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterPeiYuanShaoOlKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }
}
