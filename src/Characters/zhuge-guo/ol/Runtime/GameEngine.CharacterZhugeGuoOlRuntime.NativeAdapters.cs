#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterZhugeGuoOlCharacterZhugeGuoOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhugeGuoOlCharacterZhugeGuoOlOperations
    {
        public bool ZhugeGuoFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.ZhugeGuoOlEntries.ZhugeGuoFirstChild(root, child);
    }

    [ProgramCapability]
    private sealed class CharacterZhugeGuoOlResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhugeGuoOlResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterZhugeGuoOlCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhugeGuoOlCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterZhugeGuoOlCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhugeGuoOlCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool SameNameHandObserverSuffix(int index) => engine.SameNameHandDiscardOrDamageObserversEntries.SameNameHandObserverSuffix(index);
    }

    [ProgramCapability]
    private sealed class CharacterZhugeGuoOlKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhugeGuoOlKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CharacterZhugeGuoOlContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhugeGuoOlContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
