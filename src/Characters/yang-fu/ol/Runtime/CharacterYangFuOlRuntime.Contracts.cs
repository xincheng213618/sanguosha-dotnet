#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterYangFuOlCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseVirtualSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Suit? physicalSuit = default, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool ignoreDistance = false, global::CardGame.Core.CardConversionSource? conversionSource = default, bool actualUnconvertedPhysical = false, bool countsTowardLimit = true);
    bool CanUseSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slashCard, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, bool ignoreDistance = false, long? existingUseFrameId = default, bool noEffectiveRank = false, int? specificEffectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
}

internal interface ICharacterYangFuOlCharacterFuRongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedFuRongDistance(long? existingUseFrameId, int ownerSeat);
}

internal interface ICharacterYangFuOlCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default);
}

internal interface ICharacterYangFuOlCoreCurrentUsePhysicalClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCurrentUsePhysicalCardClaim(long frameId, int cardId);
}

internal interface ICharacterYangFuOlCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICharacterYangFuOlCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICharacterYangFuOlCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IgnoresProgramSlashLimit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source);
}

internal interface ICharacterYangFuOlCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
    bool IsMovementObserverEdge(int child);
    bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying);
}

internal interface ICharacterYangFuOlCorePrivateOfferReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICharacterYangFuOlCoreCurrentSlashFireOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertCurrentSlashFirePolicy(global::CardGame.Core.CardUseFrame use);
}

internal interface ICharacterYangFuOlCoreNativeBorrowedSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICharacterYangFuOlCoreKenshangPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsKenshangRemovedMaterial(long useId, int cardId);
}

internal interface ICharacterYangFuOlCharacterYanPuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsYanPuDyingExitEdge(int index);
}

internal interface ICharacterYangFuOlCharacterZhangYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasZhangYanLangdaoTargetTail(global::CardGame.Core.CardUseFrame use);
    bool IsZhangYanSujiRemovedMaterial(long useId, int cardId);
}

internal interface ICharacterYangFuOlCoreDesignatedExtraTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDesignatedExtraTargetTail(global::CardGame.Core.CardUseFrame use);
    bool TryGetDesignatedExtraTargetUse(int owner, global::CardGame.Core.ProgramSkillWindowContext context, out global::CardGame.Core.CardUseFrame use, out global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    bool DesignatedExtraTargetCandidateMatches(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    bool CanBeDistanceFreeDesignatedTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target);
}

internal interface ICharacterYangFuOlCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardDistanceExemption(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, long? cardUseFrameId = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default);
}

internal interface ICharacterYangFuOlCharacterWuJingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuJingHejiRemovedMaterial(long useId, int cardId);
}

internal interface ICharacterYangFuOlCoreJudgedRankSlashPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedJudgedRankSlashQuota(long useId, int actor);
}

internal interface ICharacterYangFuOlCharacterQianZhaoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeifuTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICharacterYangFuOlResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterYangFuOlCharacterGuanYuRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool GuanFirstRedAvailable(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Suit? suit);
}

internal interface ICharacterYangFuOlCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
    void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update);
}

internal interface ICharacterYangFuOlCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterYangFuOlCoreCharacterStateProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterYangFuOlCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICharacterYangFuOlCoreRecipientCategorySlashTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRecipientCategorySlashTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICharacterYangFuOlCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}

internal interface ICharacterYangFuOlCoreGrantedEntityPhasesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedGrantedPhaseEntityDistance(long? useId, int actor);
}

internal interface ICharacterYangFuOlCoreRoundGainedSourceUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedRoundGainedBasicBonus(long useId, int actor);
}

internal interface ICharacterYangFuOlContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterYangFuOlCoreWholeActualTurnUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? CapturedWholeActualTurnActorUseOrdinal(global::CardGame.Core.CardActionContext action);
}

internal interface ICharacterYangFuOlCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICharacterYangFuOlYangFuOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.YangFuRuntime Get();
}

internal interface ICharacterYangFuOlCoreTriggeredCardProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome UseTriggeredProgramVirtualSlash(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat);
}

internal interface ICharacterYangFuOlCoreForeignSelectedCardSlashReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSelectedForeignCardSlashRemovedMaterial(long useId, int cardId);
}

internal interface ICharacterYangFuOlCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICharacterYangFuOlCoreSkillOwnershipWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginAdvancedSkillsChanged(long parentFrameId);
}

internal interface ICharacterYangFuOlCoreCardUseDebitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseDebitIdentity? GetCardUseDebit(long actionId);
    void RecordSlashUseDebit(long cardUseFrameId, int actorSeat);
}

internal interface ICharacterYangFuOlCharacterLiuPanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPijingTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICharacterYangFuOlCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterYangFuOlCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome UseProgramRandomDeckEquipment(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, string bind);
    bool RandomEquipmentFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
    bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit);
}

internal interface ICharacterYangFuOlCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
}

internal interface ICharacterYangFuOlCoreResponseEntityExchangeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExchangedCardClaim(long actionId, int cardId);
}

internal interface ICharacterYangFuOlCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICharacterYangFuOlCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICharacterYangFuOlCoreProgramCardUseRolesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void UpdateProgramRoleCardUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext action);
}

internal interface ICharacterYangFuOlCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICharacterYangFuOlCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICharacterYangFuOlCharacterLuKangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedWuLuKangDixianUse(long useId, int actorSeat);
}

internal interface ICharacterYangFuOlCoreProvenanceCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedProvenanceUseDistance(long? useId, int owner);
}

internal interface ICharacterYangFuOlCoreHandComparisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSelfTargetForbiddenAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICharacterYangFuOlKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
}

internal interface ICharacterYangFuOlCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICharacterYangFuOlCoreAlternatingSuitAndCompletedTopOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPhaseSuitAllowance(int seat, global::CardGame.Core.Suit? suit);
}
