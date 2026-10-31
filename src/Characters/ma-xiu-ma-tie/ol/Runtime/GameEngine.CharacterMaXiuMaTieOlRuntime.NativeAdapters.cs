#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterMaXiuMaTieOlKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterMaXiuMaTieOlKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterMaXiuMaTieOlCharacterMaXiuMaTieOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterMaXiuMaTieOlCharacterMaXiuMaTieOlOperations
    {
        public bool KenshangFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.MaXiuMaTieOlEntries.KenshangFirstChild(f, child);
    }

    [ProgramCapability]
    private sealed class CharacterMaXiuMaTieOlCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterMaXiuMaTieOlCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterMaXiuMaTieOlResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterMaXiuMaTieOlResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterMaXiuMaTieOlCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterMaXiuMaTieOlCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool SameNameHandObserverSuffix(int index) => engine.SameNameHandDiscardOrDamageObserversEntries.SameNameHandObserverSuffix(index);
    }

    [ProgramCapability]
    private sealed class CharacterMaXiuMaTieOlContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterMaXiuMaTieOlContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
