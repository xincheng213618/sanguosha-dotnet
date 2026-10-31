#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterSharedGameEngineOlMoRumoObserversCoreDamageObserverCursorQueriesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSharedGameEngineOlMoRumoObserversCoreDamageObserverCursorQueriesOperations
    {
        public bool TypedDamagePostEventEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => engine.DamageObserverCursorQueriesEntries.TypedDamagePostEventEdge(parent, child);
    }

    [ProgramCapability]
    private sealed class CharacterSharedGameEngineOlMoRumoObserversCharacterSharedGameEngineOlMoRumoObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSharedGameEngineOlMoRumoObserversCharacterSharedGameEngineOlMoRumoObserversOperations
    {
        public bool OlMoRumoLifecycleEdge(int index) => engine.SharedGameEngineOlMoRumoObserversEntries.OlMoRumoLifecycleEdge(index);
        public bool OlMoRumoFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => engine.SharedGameEngineOlMoRumoObserversEntries.OlMoRumoFirstChild(frame, child);
        public bool OlMoRumoObserverSuffix(int index) => engine.SharedGameEngineOlMoRumoObserversEntries.OlMoRumoObserverSuffix(index);
    }

    [ProgramCapability]
    private sealed class CharacterSharedGameEngineOlMoRumoObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSharedGameEngineOlMoRumoObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterSharedGameEngineOlMoRumoObserversCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSharedGameEngineOlMoRumoObserversCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool IsSameNameHandObserverEdge(int child) => engine.SameNameHandDiscardOrDamageObserversEntries.IsSameNameHandObserverEdge(child);
    }

    [ProgramCapability]
    private sealed class CharacterSharedGameEngineOlMoRumoObserversCoreRecoveryReplacementDamageRidesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSharedGameEngineOlMoRumoObserversCoreRecoveryReplacementDamageRidesOperations2
    {
        public bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => engine.RecoveryReplacementDamageRidesEntries.RecoveryReplacementFrameRidesOn(ride, beneath);
    }

    [ProgramCapability]
    private sealed class CharacterSharedGameEngineOlMoRumoObserversCharacterYanPuOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSharedGameEngineOlMoRumoObserversCharacterYanPuOlOperations
    {
        public bool IsYanPuDyingExitEdge(int index) => engine.YanPuOlEntries.IsYanPuDyingExitEdge(index);
    }

    [ProgramCapability]
    private sealed class CharacterSharedGameEngineOlMoRumoObserversResolutionStackAdapter2(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSharedGameEngineOlMoRumoObserversResolutionStackOperations2
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack() => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterSharedGameEngineOlMoRumoObserversCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSharedGameEngineOlMoRumoObserversCoreObserverSubtreesOperations2
    {
        public bool IsMovementObserverEdge(int child) => engine.ObserverSubtreesEntries.IsMovementObserverEdge(child);
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }
}
