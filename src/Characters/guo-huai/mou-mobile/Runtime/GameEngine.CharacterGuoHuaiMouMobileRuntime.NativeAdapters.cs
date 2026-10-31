#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterGuoHuaiMouMobileCoreRecoveryReplacementDamageRidesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuoHuaiMouMobileCoreRecoveryReplacementDamageRidesOperations
    {
        public bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => engine.RecoveryReplacementDamageRidesEntries.RecoveryReplacementFrameRidesOn(ride, beneath);
    }

    [ProgramCapability]
    private sealed class CharacterGuoHuaiMouMobileResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuoHuaiMouMobileResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterGuoHuaiMouMobileCoreDamageObserverCursorQueriesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuoHuaiMouMobileCoreDamageObserverCursorQueriesOperations
    {
        public bool TypedDamagePostEventEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => engine.DamageObserverCursorQueriesEntries.TypedDamagePostEventEdge(parent, child);
    }

    [ProgramCapability]
    private sealed class CharacterGuoHuaiMouMobileResolutionStackAdapter2(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuoHuaiMouMobileResolutionStackOperations2
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack() => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterGuoHuaiMouMobileCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuoHuaiMouMobileCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool IsSameNameHandObserverEdge(int child) => engine.SameNameHandDiscardOrDamageObserversEntries.IsSameNameHandObserverEdge(child);
    }

    [ProgramCapability]
    private sealed class CharacterGuoHuaiMouMobileCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuoHuaiMouMobileCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterGuoHuaiMouMobileCharacterGuoHuaiMouMobileAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuoHuaiMouMobileCharacterGuoHuaiMouMobileOperations
    {
        public bool MouGhFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.GuoHuaiMouMobileEntries.MouGhFirstChild(f, child);
        public bool MouGhSuffix(int index) => engine.GuoHuaiMouMobileEntries.MouGhSuffix(index);
    }

    [ProgramCapability]
    private sealed class CharacterGuoHuaiMouMobileCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuoHuaiMouMobileCoreObserverSubtreesOperations
    {
        public bool IsMovementObserverEdge(int child) => engine.ObserverSubtreesEntries.IsMovementObserverEdge(child);
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterGuoHuaiMouMobileCharacterYanPuOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuoHuaiMouMobileCharacterYanPuOlOperations
    {
        public bool IsYanPuDyingExitEdge(int index) => engine.YanPuOlEntries.IsYanPuDyingExitEdge(index);
    }

    [ProgramCapability]
    private sealed class CharacterGuoHuaiMouMobileKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuoHuaiMouMobileKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterGuoHuaiMouMobileContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuoHuaiMouMobileContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
