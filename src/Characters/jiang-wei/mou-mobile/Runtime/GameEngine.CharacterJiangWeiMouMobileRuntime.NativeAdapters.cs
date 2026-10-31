#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterJiangWeiMouMobileKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiangWeiMouMobileKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterJiangWeiMouMobileContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiangWeiMouMobileContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterJiangWeiMouMobileCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiangWeiMouMobileCoreObserverSubtreesOperations
    {
        public bool IsMovementObserverEdge(int child) => engine.ObserverSubtreesEntries.IsMovementObserverEdge(child);
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterJiangWeiMouMobileCharacterJiangWeiMouMobileAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiangWeiMouMobileCharacterJiangWeiMouMobileOperations
    {
        public bool MouJwNativeEdge(int n) => engine.JiangWeiMouMobileEntries.MouJwNativeEdge(n);
        public bool MouJwFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.JiangWeiMouMobileEntries.MouJwFirstChild(f, child);
        public bool MouJwSuffix(int n) => engine.JiangWeiMouMobileEntries.MouJwSuffix(n);
    }

    [ProgramCapability]
    private sealed class CharacterJiangWeiMouMobileCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiangWeiMouMobileCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool IsSameNameHandObserverEdge(int child) => engine.SameNameHandDiscardOrDamageObserversEntries.IsSameNameHandObserverEdge(child);
    }

    [ProgramCapability]
    private sealed class CharacterJiangWeiMouMobileCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiangWeiMouMobileCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterJiangWeiMouMobileKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiangWeiMouMobileKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long resolutionId, global::CardGame.Core.FactionCardRequestPurpose purpose, int ownerSeat, global::System.Collections.Generic.IReadOnlyList<int> candidateSeats, string providerFactionId, global::CardGame.Core.NativeOwners.CardAttackHandle? responseAttack = default, int? targetSeat = default, long? programSkillFrameId = default, global::CardGame.Core.CardKind requiredKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? qinglongCrescentBlade = default, global::CardGame.Core.FactionResponsePolicySource? policySource = default, string? assistedResultBind = default) => engine.ComposeNativeFactionCardRequestHandle(resolutionId, purpose, ownerSeat, candidateSeats, providerFactionId, responseAttack, targetSeat, programSkillFrameId, requiredKind, borrowedSword, qinglongCrescentBlade, policySource, assistedResultBind);
    }

    [ProgramCapability]
    private sealed class CharacterJiangWeiMouMobileCharacterYanPuOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiangWeiMouMobileCharacterYanPuOlOperations
    {
        public bool IsYanPuDyingExitEdge(int index) => engine.YanPuOlEntries.IsYanPuDyingExitEdge(index);
    }

    [ProgramCapability]
    private sealed class CharacterJiangWeiMouMobileCoreRecoveryReplacementDamageRidesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiangWeiMouMobileCoreRecoveryReplacementDamageRidesOperations2
    {
        public bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => engine.RecoveryReplacementDamageRidesEntries.RecoveryReplacementFrameRidesOn(ride, beneath);
    }

    [ProgramCapability]
    private sealed class CharacterJiangWeiMouMobileResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiangWeiMouMobileResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack() => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterJiangWeiMouMobileResolutionStackAdapter2(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiangWeiMouMobileResolutionStackOperations2
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterJiangWeiMouMobileKernelCoreGameEngineAdvancedLifecycleHostAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiangWeiMouMobileKernelCoreGameEngineAdvancedLifecycleHostOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost() => engine.ComposeNativeProgramSkillHost();
    }

    [ProgramCapability]
    private sealed class CharacterJiangWeiMouMobileCoreDamageObserverCursorQueriesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiangWeiMouMobileCoreDamageObserverCursorQueriesOperations
    {
        public bool TypedDamagePostEventEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => engine.DamageObserverCursorQueriesEntries.TypedDamagePostEventEdge(parent, child);
    }
}
