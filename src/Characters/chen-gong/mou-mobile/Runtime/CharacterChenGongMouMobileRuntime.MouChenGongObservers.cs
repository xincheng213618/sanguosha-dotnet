#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChenGongMouMobileRuntime
{
    internal bool ValidMouChenGongGift(ProgramSkillFrame f)
    {
        if (f.MouChenGongGift is not { } r || !MouChenGongGiftRoot(f) || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.Before > r.After || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            return false;
        var gifts = _coreProgramEventHistoryOperations.ProgramEventHistory<MouChenGongGiftPaidEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Gift is not { } gift)
            return r.Stage == MouChenGongGiftStage.GiftChoice && gifts.Length == 0 && r.Branch is null && r.Loss is null && r.Draw is null && !r.CeGained;
        if (gift.From.OwnerSeat != f.OwnerSeat || gift.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || gift.Sequence <= 0 || gifts is not [var paid] || paid != new MouChenGongGiftPaidEvent(f.Id, r.RecipientSeat, gift.CardId, gift.PrintedKind, gift.From, gift.To, gift.Sequence) || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == gift.Sequence && m.CardId == gift.CardId && m.CardKind == gift.PrintedKind && m.From == gift.From && m.To == gift.To && m.Reason.Value == MouChenGongGiftReason) != 1)
            return false;
        var branches = _coreProgramEventHistoryOperations.ProgramEventHistory<MouChenGongBranchAcceptedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Branch is null)
            return r.Stage is MouChenGongGiftStage.GiftChildren or MouChenGongGiftStage.RecipientChoice && branches.Length == 0 && r.Loss is null && r.Draw is null && !r.CeGained;
        if (r.Branch is not ("lose-hp" or "draw") || branches is not [var branch] || branch != new MouChenGongBranchAcceptedEvent(f.Id, r.RecipientSeat, r.Branch))
            return false;
        var issued = _coreProgramEventHistoryOperations.ProgramEventHistory<MouChenGongLossIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var losses = _coreProgramEventHistoryOperations.ProgramEventHistory<MouChenGongLossReturnedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Branch == "draw" && (r.Stage is not (MouChenGongGiftStage.Drawing or MouChenGongGiftStage.DrawChildren) || r.Loss is not null || losses.Length != 0 || issued.Length != 0 || r.CeGained))
            return false;
        if (r.Branch == "lose-hp")
        {
            if (issued is not [var issue] || issue != new MouChenGongLossIssuedEvent(f.Id, r.RecipientSeat, 1))
                return false;
            if (r.Loss is not { } loss)
                return r.Stage == MouChenGongGiftStage.LosingHp && losses.Length == 0 && r.Draw is null && !r.CeGained;
            if (loss.HpBefore < 0 || loss.HpAfter < 0 || loss.ActualAmount != loss.HpBefore - loss.HpAfter || loss.ActualAmount is < 0 or > 1 || losses is not [var returned] || returned != new MouChenGongLossReturnedEvent(f.Id, r.RecipientSeat, loss.ChangeId, loss.HpBefore, loss.HpAfter, loss.ActualAmount, loss.IdentityPrevented))
                return false;
            if (loss.IdentityPrevented ? loss.ChangeId != 0 || loss.ActualAmount != 0 || !_coreProgramEventHistoryOperations.ProgramEventHistory<IdentityGeneralAppearedEvent>().Any(e => e.Receipt.Entry == IdentityAppearanceEntry.ProgramHpLoss && e.Receipt.ParentFrameId == f.Id && e.Receipt.OwnerSeat == r.RecipientSeat && e.Receipt.RequestedDeduction == 1 && e.Receipt.OriginalInstructionIndex == f.InstructionIndex && e.Receipt.RestoredHp == loss.HpBefore) : loss.ChangeId <= 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillHpLostEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.TargetSeat == r.RecipientSeat && e.Amount == loss.ActualAmount && e.RemainingHp == loss.HpAfter) != 1)
                return false;
        }

        var draws = _coreProgramEventHistoryOperations.ProgramEventHistory<MouChenGongDrawReturnedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Draw is not { } draw)
            return draws.Length == 0 && !r.CeGained && r.Stage is MouChenGongGiftStage.LossChildren or MouChenGongGiftStage.Drawing;
        var seat = r.Branch == "lose-hp" ? f.OwnerSeat : r.RecipientSeat;
        var count = r.Branch == "lose-hp" ? 2 : 1;
        return r.Stage == MouChenGongGiftStage.DrawChildren && draw.RecipientSeat == seat && draw.RequestedCount == count && draws is [var returnedDraw] && returnedDraw == new MouChenGongDrawReturnedEvent(f.Id, seat, count, draw.ActualDrawIds.Count, draw.ReplacementGainedIds.Count, draw.SequenceBefore, draw.SequenceAfter, draw.AttemptFrameId) && (!r.CeGained || r.Branch == "lose-hp");
    }

    internal bool ValidMouChenGongRelease(ProgramSkillFrame f)
    {
        if (f.MouChenGongRelease is not { } r || !MouChenGongReleaseRoot(f) || !Enum.IsDefined(r.Stage) || r.Before > r.After || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            return false;
        var accepted = _coreProgramEventHistoryOperations.ProgramEventHistory<MouChenGongReleaseAcceptedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var damage = _coreProgramEventHistoryOperations.ProgramEventHistory<MouChenGongReleaseDamageIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var cleared = _coreProgramEventHistoryOperations.ProgramEventHistory<MouChenGongCeClearedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.TargetSeat is not { } target || r.FrozenX is not { } x)
            return r.Stage == MouChenGongReleaseStage.TargetChoice && r.TargetSeat is null && r.FrozenX is null && !r.Cleared && accepted.Length == 0 && damage.Length == 0 && cleared.Length == 0;
        return _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && target != f.OwnerSeat && x > 0 && accepted is [var accept] && accept == new MouChenGongReleaseAcceptedEvent(f.Id, target, x) && damage is [var issue] && issue == new MouChenGongReleaseDamageIssuedEvent(f.Id, f.OwnerSeat, target, x) && (r.Cleared ? r.Stage == MouChenGongReleaseStage.ClearChildren && cleared is [var clear] && clear.OwnerSeat == f.OwnerSeat && clear.FrozenX == x && clear.ClearedCount >= 0 : r.Stage == MouChenGongReleaseStage.Damage && cleared.Length == 0);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29600)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 29600)]
    internal bool PermitAcceptedMouChenGong(ProgramSkillFrame f)
    {
        if (f.MouChenGongGift is { Gift: not null })
            return ValidMouChenGongGift(f);
        return f.MouChenGongRelease is { FrozenX: > 0 } && ValidMouChenGongRelease(f);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29601)]
    internal bool PermitIssuedMouChenGong(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && PermitMouChenGongZhichiAfterLoss(f);
    internal void SealMouChenGongMovements(long id, long before)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouChenGongGift = f.MouChenGongGift is { } gift ? gift with { After = after } : null });
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= after))
        {
            var signed = new MouChenGongMovementSignedEvent(id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence);
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouChenGongMovementSignedEvent>().Contains(signed))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(signed);
        }
    }

    internal bool MouChenGongMovementSigned(ProgramSkillFrame f, CardMovementRecord m) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouChenGongMovementSignedEvent>().Count(e => e == new MouChenGongMovementSignedEvent(f.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence)) == 1;
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 370)]
    internal bool IsMouChenGongMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => e?.Op == SkillProgramEffectOp.ResolveMouChenGongMingceGift && f.MouChenGongGift is { Stage: MouChenGongGiftStage.GiftChildren or MouChenGongGiftStage.LosingHp or MouChenGongGiftStage.LossChildren or MouChenGongGiftStage.DrawChildren } && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && ValidMouChenGongGift(f);
    internal bool DrainMouChenGongChildren(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal bool MouChenGongFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouChenGongGift is { Gift: not null } r && ValidMouChenGongGift(f))
        {
            if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
                return r.Stage == MouChenGongGiftStage.Drawing && f.NativeDraw is not null;
            if (child is DyingFrame dying)
                return r.Stage == MouChenGongGiftStage.LossChildren && r.Loss is { ActualAmount: > 0, HpAfter: 0 } && dying.ParentFrameId == f.Id && dying.VictimSeat == r.RecipientSeat && dying.KillerSeat is null && dying.Continuation == DyingContinuationKind.ProgramSkill;
            if (child is IdentityAppearanceFrame identity)
                return r.Stage == MouChenGongGiftStage.LosingHp && identity.Receipt.Entry == IdentityAppearanceEntry.ProgramHpLoss && identity.Receipt.ParentFrameId == f.Id && identity.Receipt.OwnerSeat == r.RecipientSeat && identity.Receipt.RequestedDeduction == 1 && identity.Receipt.OriginalInstructionIndex == f.InstructionIndex;
            if (_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, f))
                return true;
            if (child is HpChangedTriggerWindowFrame hp)
                return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && (r.Stage == MouChenGongGiftStage.LossChildren && r.Loss?.ChangeId == hp.Change.Id && hp.Change.TargetSeat == r.RecipientSeat && hp.Change.Kind == HpChangeKind.Loss || r.Stage == MouChenGongGiftStage.GiftChildren && r.Gift is { PrintedKind: CardKind.SilverLion, From.Zone: CardZoneKind.Equipment } && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.Kind == HpChangeKind.Recovery);
            if (f.PendingMovementContinuation is { } pending && IsMouChenGongMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending) && child is CardsMovedTriggerWindowFrame moved)
                return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && MouChenGongMovementSigned(f, m));
        }
        else if (f.MouChenGongRelease is { Stage: MouChenGongReleaseStage.Damage, FrozenX: > 0, TargetSeat: not null } && ValidMouChenGongRelease(f) && f.AttackAttempt is { } a && f.AttackReturn is not null && a.SourceSeat == f.OwnerSeat && !a.SourceLess)
            return _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child);
        else
            return false;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouChenGongObserverSuffix(int index) => _resolutionStackOperations._resolutionStack[index + 1] is DyingFrame original && index + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(index + 1, original) || _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame f && _characterChenGongMouMobileOperations.MouChenGongFirstChild(f, _resolutionStackOperations2._resolutionStack()[n]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? MouChenGongObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterChenGongMouMobileOperations.MouChenGongFirstChild(f, child), (n) => _characterChenGongMouMobileOperations.MouChenGongObserverSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1350)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1460)]
    internal bool HasMouChenGongDamageObserver(long id) => MouChenGongObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1490)]
    internal bool HasMouChenGongBeforeDamageObserver(long id) => MouChenGongObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 740)]
    internal bool IsMouChenGongDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MouChenGongObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 520)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 510)]
    internal bool IsMouChenGongRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return index >= 0 && child > index + 1 && MouChenGongFirstChild(f, _resolutionStackOperations._resolutionStack[index + 1]) && MouChenGongObserverSuffix(index) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 29600)]
    internal bool ResumeMouChenGong(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id)
            return false;
        if (f.MouChenGongGift is not null)
        {
            ContinueMouChenGongGift(id);
            return true;
        }

        if (f.MouChenGongRelease is not null)
        {
            ContinueMouChenGongRelease(id);
            return true;
        }

        return false;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 29600)]
    internal bool ReturnMouChenGongMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeMouChenGong(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 29600)]
    internal bool AdvanceMouChenGongChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouChenGongObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
