#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTangTangOlRuntime
{
    internal static bool TangTangMoving(TangTangStage stage) => global::CardGame.Core.RuntimeSkills.RuntimeRules_tang_tang_ol_TangTangObservers.TangTangMoving(stage);

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 460)]
    internal bool IsTangTangMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.TangTang is { Paid: true } r && e?.Op == r.Op && p == f.PendingMovementContinuation && p.BeforeCount == 0 && p.CoverageResultBind is null && (TangTangMoving(r.Stage) && p.SubjectSeat == (r.Stage == TangTangStage.TakeChildren ? r.Peer : r.Stage == TangTangStage.DrawChildren ? r.Recipient : f.OwnerSeat) || r.Stage is TangTangStage.HpIssuing or TangTangStage.HpChildren && p.SubjectSeat == r.Peer);
    internal bool IsTangTangPaidMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.TangTang!;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        if (r.Stage == TangTangStage.HpChildren)
            return m.Sequence > r.Before && m.From.OwnerSeat == r.Peer && m.To == CardLocation.DiscardPile && m.Reason is var reason && (reason == CardMoveReasons.DeathDiscard || reason == CardMoveReasons.WoodenOxGrainDiscard) && !_coreMatchParticipantsPlayersAccess._players[r.Peer].IsAlive && _coreProgramEventHistoryOperations.ProgramEventHistory<ZexingHpLossIssuedEvent>().Any(e => e.FrameId == f.Id && e.Peer == r.Peer && e.After == 0);
        if (m.Sequence <= r.Before || m.Sequence > r.After)
            return false;
        if (r.Stage == TangTangStage.DrawChildren)
            return m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.Recipient) && m.Reason.Value == ZhiyiDrawReason || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<TangTangEntityMovedEvent>().Any(e => e.FrameId == f.Id && e.Step == r.Step && e.Stage == r.Stage && e.CardId == m.CardId && e.Kind == m.CardKind && e.From == m.From && e.To == m.To && e.Sequence == m.Sequence))
            return true;
        return r.Grain.Contains(m.CardId) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && r.Materials.Any(c => c.Kind == CardKind.WoodenOx && c.From.Zone == CardZoneKind.Equipment && m.From == CardLocation.WoodenOxGrain(c.From.OwnerSeat!.Value));
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 210)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 200)]
    internal bool IsTangTangRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.TangTang is { Paid: true } r && r.Stage is TangTangStage.TakeChildren or TangTangStage.GiftChildren && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == recovery.Attempt.TargetSeat && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == (r.Stage == TangTangStage.TakeChildren ? ZexingTakeReason : ZexingGiftReason) && r.Materials.Any(c => c.Kind == CardKind.SilverLion && c.From.Zone == CardZoneKind.Equipment && c.From.OwnerSeat == recovery.Attempt.TargetSeat);
    internal bool TangTangFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.TangTang is not { Paid: true } r)
            return false;
        if (child is IdentityAppearanceFrame appearance)
            return r.Stage == TangTangStage.HpIssuing && appearance.Receipt.Entry == IdentityAppearanceEntry.ProgramHpLoss && appearance.Receipt.ParentFrameId == f.Id && appearance.Receipt.OwnerSeat == r.Peer && appearance.Receipt.RequestedDeduction == 1 && appearance.Receipt.OriginalInstructionIndex == f.InstructionIndex;
        if (child is DyingFrame dying)
            return r.Stage == TangTangStage.HpChildren && dying.ParentFrameId == f.Id && dying.VictimSeat == r.Peer && dying.Continuation == DyingContinuationKind.ProgramSkill && dying.KillerSeat is null && _coreProgramEventHistoryOperations.ProgramEventHistory<ZexingHpLossIssuedEvent>().Any(e => e.FrameId == f.Id && e.Peer == r.Peer && e.After == 0);
        if (child is RecoveryReplacementFrame recovery)
            return IsTangTangRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
        {
            if (hp.Change.ParentFrameId != f.Id || hp.ResumeFrameId != f.Id)
                return false;
            if (r.Stage == TangTangStage.HpChildren)
                return hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Loss && hp.Change.SourceSeat is null && hp.Change.TargetSeat == r.Peer && hp.Change.Amount == 1;
            return TangTangMoving(r.Stage) && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == hp.Change.TargetSeat && hp.Change.Amount == 1 && r.Materials.Any(c => c.Kind == CardKind.SilverLion && c.From.Zone == CardZoneKind.Equipment && c.From.OwnerSeat == hp.Change.TargetSeat);
        }

        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == f.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id);
        return child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is not null && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsTangTangPaidMovement(f, m));
    }

    internal ProgramSkillFrame? TangTangObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterTangTangOlOperations.TangTangFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 970)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1080)]
    internal bool HasTangTangDamageObserver(long id) => TangTangObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1110)]
    internal bool HasTangTangBeforeDamageObserver(long id) => TangTangObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27530)]
    internal bool AdvanceTangTangChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && TangTangObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    internal IEnumerable<CardSnapshot> TangTangPrivatelyViewedCards(int viewerSeat) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.OwnerSeat == viewerSeat && f.TangTang?.Stage == TangTangStage.TakeCards).SelectMany(f => f.TangTang!.Viewed.Where(id => _cardZonesOperations._cardZones.Contains(CardLocation.Hand(f.TangTang.Peer), id))).Distinct().Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id)));
    [ProgramViewContributor(ProgramViewContribution.PublicCardsBeforeResolution, 20)]
    internal IEnumerable<CardSnapshot> ProjectTangTangPublicShownCards() => TangTangPublicShownCardIds().Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))with { PublicHandOwnerSeat = _cardZonesOperations._cardZones.GetLocation(id).Zone == CardZoneKind.Hand ? _cardZonesOperations._cardZones.GetLocation(id).OwnerSeat : null });
    internal IEnumerable<int> TangTangPublicShownCardIds() => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.TangTang is { Paid: true, Shown: not null }).Where(f => _cardZonesOperations._cardZones.Contains(CardLocation.Hand(f.OwnerSeat), f.TangTang!.Shown!.CardId) || _cardZonesOperations._cardZones.Contains(CardLocation.Equipment(f.OwnerSeat), f.TangTang!.Shown!.CardId)).Select(f => f.TangTang!.Shown!.CardId).Distinct();
}
