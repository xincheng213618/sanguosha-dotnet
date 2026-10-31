#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterHuaXiongMouMobileCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuaXiongMouMobileCoreObserverSubtreesOperations
    {
        public bool IsMovementObserverEdge(int child) => engine.ObserverSubtreesEntries.IsMovementObserverEdge(child);
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterHuaXiongMouMobileContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuaXiongMouMobileContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterHuaXiongMouMobileCharacterHuaXiongMouMobileAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuaXiongMouMobileCharacterHuaXiongMouMobileOperations
    {
        public bool MouHuaXiongFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.HuaXiongMouMobileEntries.MouHuaXiongFirstChild(f, child);
        public bool MouHuaXiongObserverSuffix(int index) => engine.HuaXiongMouMobileEntries.MouHuaXiongObserverSuffix(index);
    }

    [ProgramCapability]
    private sealed class CharacterHuaXiongMouMobileCharacterYanPuOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuaXiongMouMobileCharacterYanPuOlOperations
    {
        public bool IsYanPuDyingExitEdge(int index) => engine.YanPuOlEntries.IsYanPuDyingExitEdge(index);
    }

    [ProgramCapability]
    private sealed class CharacterHuaXiongMouMobileResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuaXiongMouMobileResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack() => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterHuaXiongMouMobileCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuaXiongMouMobileCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool IsSameNameHandObserverEdge(int child) => engine.SameNameHandDiscardOrDamageObserversEntries.IsSameNameHandObserverEdge(child);
    }

    [ProgramCapability]
    private sealed class CharacterHuaXiongMouMobileResolutionStackAdapter2(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuaXiongMouMobileResolutionStackOperations2
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterHuaXiongMouMobileKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuaXiongMouMobileKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CharacterHuaXiongMouMobileCoreRecoveryReplacementDamageRidesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuaXiongMouMobileCoreRecoveryReplacementDamageRidesOperations
    {
        public bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => engine.RecoveryReplacementDamageRidesEntries.RecoveryReplacementFrameRidesOn(ride, beneath);
    }

    [ProgramCapability]
    private sealed class CharacterHuaXiongMouMobileCoreDamageObserverCursorQueriesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuaXiongMouMobileCoreDamageObserverCursorQueriesOperations
    {
        public bool TypedDamagePostEventEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => engine.DamageObserverCursorQueriesEntries.TypedDamagePostEventEdge(parent, child);
    }
}
