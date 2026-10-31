#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterLuoXianOlCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuoXianOlCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterLuoXianOlContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuoXianOlContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterLuoXianOlCharacterLuoXianOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuoXianOlCharacterLuoXianOlOperations
    {
        public bool DailiFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => engine.LuoXianOlEntries.DailiFirstChild(frame, child);
    }

    [ProgramCapability]
    private sealed class CharacterLuoXianOlCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuoXianOlCoreObserverSubtreesOperations2
    {
        public bool HasMovementObserverSuffix(int rootIndex) => engine.ObserverSubtreesEntries.HasMovementObserverSuffix(rootIndex);
    }

    [ProgramCapability]
    private sealed class CharacterLuoXianOlResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuoXianOlResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterLuoXianOlKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuoXianOlKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}
