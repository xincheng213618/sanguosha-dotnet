#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterGongsunZanMouOlCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGongsunZanMouOlCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool IsSameNameHandObserverEdge(int child) => engine.SameNameHandDiscardOrDamageObserversEntries.IsSameNameHandObserverEdge(child);
    }

    [ProgramCapability]
    private sealed class CharacterGongsunZanMouOlCharacterGongsunZanMouOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGongsunZanMouOlCharacterGongsunZanMouOlOperations
    {
        public bool MouGszFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.GongsunZanMouOlEntries.MouGszFirstChild(f, child);
        public bool MouGszSuffix(int i) => engine.GongsunZanMouOlEntries.MouGszSuffix(i);
    }

    [ProgramCapability]
    private sealed class CharacterGongsunZanMouOlResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGongsunZanMouOlResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterGongsunZanMouOlCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGongsunZanMouOlCoreObserverSubtreesOperations
    {
        public bool IsMovementObserverEdge(int child) => engine.ObserverSubtreesEntries.IsMovementObserverEdge(child);
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterGongsunZanMouOlCharacterYanPuOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGongsunZanMouOlCharacterYanPuOlOperations
    {
        public bool IsYanPuDyingExitEdge(int index) => engine.YanPuOlEntries.IsYanPuDyingExitEdge(index);
    }

    [ProgramCapability]
    private sealed class CharacterGongsunZanMouOlCoreRecoveryReplacementDamageRidesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGongsunZanMouOlCoreRecoveryReplacementDamageRidesOperations2
    {
        public bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => engine.RecoveryReplacementDamageRidesEntries.RecoveryReplacementFrameRidesOn(ride, beneath);
    }

    [ProgramCapability]
    private sealed class CharacterGongsunZanMouOlCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGongsunZanMouOlCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterGongsunZanMouOlCoreDamageObserverCursorQueriesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGongsunZanMouOlCoreDamageObserverCursorQueriesOperations
    {
        public bool TypedDamagePostEventEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => engine.DamageObserverCursorQueriesEntries.TypedDamagePostEventEdge(parent, child);
    }

    [ProgramCapability]
    private sealed class CharacterGongsunZanMouOlResolutionStackAdapter2(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGongsunZanMouOlResolutionStackOperations2
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack() => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterGongsunZanMouOlKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGongsunZanMouOlKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterGongsunZanMouOlContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGongsunZanMouOlContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
