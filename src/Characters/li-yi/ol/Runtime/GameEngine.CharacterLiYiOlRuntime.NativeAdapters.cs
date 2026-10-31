#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterLiYiOlCoreRecoveryReplacementDamageRidesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLiYiOlCoreRecoveryReplacementDamageRidesOperations
    {
        public bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => engine.RecoveryReplacementDamageRidesEntries.RecoveryReplacementFrameRidesOn(ride, beneath);
    }

    [ProgramCapability]
    private sealed class CharacterLiYiOlKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLiYiOlKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterLiYiOlCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLiYiOlCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterLiYiOlResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLiYiOlResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack() => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterLiYiOlCharacterYanPuOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLiYiOlCharacterYanPuOlOperations
    {
        public bool IsYanPuDyingExitEdge(int index) => engine.YanPuOlEntries.IsYanPuDyingExitEdge(index);
    }

    [ProgramCapability]
    private sealed class CharacterLiYiOlKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLiYiOlKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long resolutionId, global::CardGame.Core.FactionCardRequestPurpose purpose, int ownerSeat, global::System.Collections.Generic.IReadOnlyList<int> candidateSeats, string providerFactionId, global::CardGame.Core.NativeOwners.CardAttackHandle? responseAttack = default, int? targetSeat = default, long? programSkillFrameId = default, global::CardGame.Core.CardKind requiredKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? qinglongCrescentBlade = default, global::CardGame.Core.FactionResponsePolicySource? policySource = default, string? assistedResultBind = default) => engine.ComposeNativeFactionCardRequestHandle(resolutionId, purpose, ownerSeat, candidateSeats, providerFactionId, responseAttack, targetSeat, programSkillFrameId, requiredKind, borrowedSword, qinglongCrescentBlade, policySource, assistedResultBind);
    }

    [ProgramCapability]
    private sealed class CharacterLiYiOlObservedSkillGrantRevisionsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLiYiOlObservedSkillGrantRevisionsOperations
    {
        public ref readonly global::System.Collections.Generic.Dictionary<int, long> _observedSkillGrantRevisions => ref engine._observedSkillGrantRevisions;
    }

    [ProgramCapability]
    private sealed class CharacterLiYiOlContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLiYiOlContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterLiYiOlResolutionStackAdapter2(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLiYiOlResolutionStackOperations2
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterLiYiOlCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLiYiOlCoreObserverSubtreesOperations2
    {
        public bool HasObserverSuffix(int rootIndex, global::System.Func<int, bool> edge, global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? completedDyingSuffix) => engine.ObserverSubtreesEntries.HasObserverSuffix(rootIndex, edge, completedDyingSuffix);
        public bool IsMovementObserverEdge(int child) => engine.ObserverSubtreesEntries.IsMovementObserverEdge(child);
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterLiYiOlCharacterLiYiOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLiYiOlCharacterLiYiOlOperations
    {
        public bool LiYiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.LiYiOlEntries.LiYiFirstChild(f, child);
    }
}
