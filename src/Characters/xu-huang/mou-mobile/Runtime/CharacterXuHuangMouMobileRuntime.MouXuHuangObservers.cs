#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXuHuangMouMobileRuntime
{
    [ProgramPhaseUsedProjection(20)]
    internal int MouXuHuangProjectedPhaseUses(int owner, string skill, int fallback) => skill == MouXuHuangComposition.Duanliang && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) ? _coreProgramEventHistoryOperations.ProgramEventHistory<MouXuHuangMouyiStartedEvent>().Count(e => e.Source.OwnerSeat == owner && e.Source.SkillId == skill && e.Phase == phase) : fallback;
    internal bool ValidMouXuHuangShipo(ProgramSkillFrame f)
    {
        if (f.MouXuHuangShipo is not { } r || !MouXuHuangShipoRoot(f) || r.Source != MouXuHuangSource(f) || r.GameplayHash != f.GameplayHash || r.EndingWindowId != f.WindowContext!.ParentFrameId || r.Phase.Kind != ActualDiscardRecoveryPhaseKind.Ending || r.Phase.ActorSeat != f.OwnerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(r.Phase) || !Enum.IsDefined(r.Stage) || r.Cursor < 0 || r.Cursor > r.Targets.Count || r.Targets.Any(t => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(t) || t == f.OwnerSeat) || r.Targets.Distinct().Count() != r.Targets.Count || r.SelectedCardIds.Distinct().Count() != r.SelectedCardIds.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<MouXuHuangShipoStartedEvent>().Count(e => e == new MouXuHuangShipoStartedEvent(f.Id, r.Source, r.GameplayHash, r.EndingWindowId, r.Phase, r.OwnerHp)) != 1)
            return false;
        if (!r.Accepted)
            return r.Stage == MouXuHuangShipoStage.ModeChoice && r.Mode is null && r.Targets.Count == 0 && r.Gifts.Count == 0;
        if (r.Mode is not ("mode-single" or "mode-all") || r.Targets.Count == 0 || r.Mode == "mode-single" && r.Targets.Count != 1 || !r.Targets.SequenceEqual(r.Targets.OrderBy(t => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, t))) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouXuHuangShipoModeAcceptedEvent>().Count(e => e == new MouXuHuangShipoModeAcceptedEvent(f.Id, r.Mode, r.Targets.Count)) != 1)
            return false;
        var targets = _coreProgramEventHistoryOperations.ProgramEventHistory<MouXuHuangShipoTargetFrozenEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
        if (targets.Length != r.Targets.Count || targets.Where((e, i) => e.Index != i || e.TargetSeat != r.Targets[i]).Any())
            return false;
        var completed = _coreProgramEventHistoryOperations.ProgramEventHistory<MouXuHuangShipoRecipientCompletedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (completed.Length != r.Cursor || completed.Select(e => e.Cursor).Distinct().Count() != r.Cursor || completed.Any(e => e.Cursor < 0 || e.Cursor >= r.Cursor || e.TargetSeat != r.Targets[e.Cursor]))
            return false;
        if (r.Gifts.Select(g => g.Cursor).Distinct().Count() != r.Gifts.Count || r.Gifts.Any(g => g.Cursor < 0 || g.Cursor >= r.Targets.Count || g.GiverSeat != r.Targets[g.Cursor] || g.From != CardLocation.Hand(g.GiverSeat) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouXuHuangShipoGiftPaidEvent>().Count(e => e == new MouXuHuangShipoGiftPaidEvent(f.Id, g.Cursor, g.GiverSeat, g.CardId, g.To, g.Sequence)) != 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == g.Sequence && m.CardId == g.CardId && m.From == g.From && m.To == g.To && m.Reason.Value == MouXuHuangGiveReason) != 1))
            return false;
        var damage = _coreProgramEventHistoryOperations.ProgramEventHistory<MouXuHuangShipoDamageIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (damage.Select(e => e.Cursor).Distinct().Count() != damage.Length || damage.Any(e => e.Cursor < 0 || e.Cursor >= r.Targets.Count || e.SourceSeat != f.OwnerSeat || e.TargetSeat != r.Targets[e.Cursor] || e.Amount != 1 || r.Gifts.Any(g => g.Cursor == e.Cursor)))
            return false;
        if (r.Stage == MouXuHuangShipoStage.Damage && damage.Count(e => e.Cursor == r.Cursor) != 1)
            return false;
        var selected = _coreProgramEventHistoryOperations.ProgramEventHistory<MouXuHuangShipoTransferSelectedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
        return selected.Length == r.SelectedCardIds.Count && selected.Select((e, i) => e.Index == i && e.CardId == r.SelectedCardIds[i] && r.Gifts.Any(g => g.CardId == e.CardId && g.To == CardLocation.Hand(f.OwnerSeat))).All(v => v);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29500)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 29500)]
    internal bool PermitAcceptedMouXuHuang(ProgramSkillFrame f)
    {
        if (f.MouXuHuangDuanliang is null && f.MouXuHuangShipo is not { Accepted: true })
            return false;
        return true;
    }

    internal void SignMouXuHuangMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            throw new InvalidOperationException("Xu Huang cannot sign a nonexistent native movement.");
        var fact = new MouXuHuangMovementSignedEvent(f.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence);
        if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouXuHuangMovementSignedEvent>().Contains(fact))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
    }

    internal bool MouXuHuangMovementSigned(ProgramSkillFrame f, CardMovementRecord m) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouXuHuangMovementSignedEvent>().Count(e => e == new MouXuHuangMovementSignedEvent(f.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence)) == 1;
    internal void SealMouXuHuangMovements(long id, long before)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXuHuangDuanliang = f.MouXuHuangDuanliang is { } d ? d with { After = after } : null, MouXuHuangShipo = f.MouXuHuangShipo is { } s ? s with { After = after } : null });
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= after))
            SignMouXuHuangMovement(f, m);
    }
    internal bool IsMouXuHuangMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => e is not null && MouXuHuangComposition.Handles(e.Op) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && (f.MouXuHuangDuanliang is { Stage: MouXuHuangDuanliangStage.RefillChildren or MouXuHuangDuanliangStage.ObtainChildren or MouXuHuangDuanliangStage.NativeChildren } && MouXuHuangDuanliangRoot(f) || f.MouXuHuangShipo is { Stage: MouXuHuangShipoStage.GiftChildren or MouXuHuangShipoStage.TransferChildren } && ValidMouXuHuangShipo(f));
    internal bool DrainMouXuHuangChildren(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal bool MouXuHuangFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!PermitAcceptedMouXuHuang(f))
            return false;
        if (child is CardUseFrame use)
            return f.MouXuHuangDuanliang is { Stage: MouXuHuangDuanliangStage.NativeUse, Native: { } o } && use.Id == o.UseId && IsMouXuHuangNativeUse(use.Id);
        if (f.MouXuHuangShipo is { Stage: MouXuHuangShipoStage.Damage } s && f.AttackAttempt is { } a && f.AttackReturn is not null)
            return s.Cursor < s.Targets.Count && a.SourceSeat == f.OwnerSeat && a.TargetSeat == s.Targets[s.Cursor] && !a.SourceLess && _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child);
        if (f.PendingMovementContinuation is not { } p || !IsMouXuHuangMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
        {
            if (IsMouXuHuangNativeSettlementBatch(f, moved.Batch))
                return moved.ResumeProgramFrameId == f.Id;
            var before = f.MouXuHuangDuanliang?.Before ?? f.MouXuHuangShipo!.Before;
            var after = f.MouXuHuangDuanliang?.After ?? f.MouXuHuangShipo!.After;
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => m.Sequence > before && m.Sequence <= after && MouXuHuangMovementSigned(f, m));
        }

        if (_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, f))
            return true;
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouXuHuangNativeWindowEdge(int index)
    {
        if (_resolutionStackOperations._resolutionStack[index - 1] is CardUseFrame use && IsMouXuHuangNativeUse(use.Id) && _resolutionStackOperations._resolutionStack[index] is ProgramCardTriggerWindowFrame w)
            return w.ParentFrameId == use.Id && w.Action.ActionId == use.Action!.ActionId && w.Action.ActorSeat == use.SourceSeat && w.Action.EffectiveKind == use.CardKind;
        if (_resolutionStackOperations._resolutionStack[index - 1] is ProgramCardTriggerWindowFrame parent && _coreCardUseLifecycleOperations.LifecycleCardUse(parent.ParentFrameId)is { } native && IsMouXuHuangNativeUse(native.Id) && _resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame child && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count)
        {
            var c = parent.Candidates[parent.CandidateIndex];
            return global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(child, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(c)) && child.WindowContext == _coreCardActionsOperations.CreateCardActionProgramContext2(parent, c);
        }

        return false;
    }

    internal bool MouXuHuangObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _characterXuHuangMouMobileOperations.MouXuHuangNativeWindowEdge(n) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame f && _characterXuHuangMouMobileOperations.MouXuHuangFirstChild(f, _resolutionStackOperations2._resolutionStack()[n]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? MouXuHuangObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterXuHuangMouMobileOperations.MouXuHuangFirstChild(f, child), (index) => _characterXuHuangMouMobileOperations.MouXuHuangObserverSuffix(index));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 510)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 500)]
    internal bool IsMouXuHuangRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return index >= 0 && child > index + 1 && MouXuHuangFirstChild(f, _resolutionStackOperations._resolutionStack[index + 1]) && MouXuHuangObserverSuffix(index) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 29500)]
    internal bool ResumeMouXuHuang(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id)
            return false;
        if (f.MouXuHuangDuanliang is not null)
        {
            ContinueMouXuHuangDuanliang(id);
            return true;
        }

        if (f.MouXuHuangShipo is not null)
        {
            ContinueMouXuHuangShipo(id);
            return true;
        }

        return false;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 29500)]
    internal bool ReturnMouXuHuangMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeMouXuHuang(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 29500)]
    internal bool AdvanceMouXuHuangChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouXuHuangObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
