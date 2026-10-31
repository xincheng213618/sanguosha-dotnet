#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMengHuoMouMobileRuntime
{
    internal bool MouMhPhaseParent(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { } c || c.OwnerSeat != f.OwnerSeat || c.SourceSeat != f.OwnerSeat)
            return false;
        return _resolutionStackOperations._resolutionStack.SingleOrDefault(w => w.Id == c.ParentFrameId) switch
        {
            PlayPhaseStartingBoundaryFrame p when c.Window == SkillProgramTriggerWindow.PlayPhaseStarting && p.OwnerSeat == f.OwnerSeat && p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count && p.Items[p.ItemIndex].Candidate is { } candidate => global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate),
            ProgramLifecycleTriggerWindowFrame p when p.Window == c.Window && p.OwnerSeat == f.OwnerSeat && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count => global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex]),
            _ => false
        };
    }

    internal bool ValidMouMh(ProgramSkillFrame f)
    {
        if (f.MouMengHuo is not { } r || r.Source != MouMhSource(f) || r.GameplayHash != f.GameplayHash || f.InstructionIndex != 1 || f.WindowContext is not { } c || c.ParentFrameId != r.WindowId || c.OwnerSeat != f.OwnerSeat || c.SourceSeat != f.OwnerSeat || f.ActivationId != f.TriggerId || f.SkillId != (r.Obtain ? MouMengHuoComposition.Huoshou : MouMengHuoComposition.Zaiqi) || f.TriggerId != (r.Obtain ? "obtain-at-play-start" : "spend-at-discard-end") || c.Window != (r.Obtain ? SkillProgramTriggerWindow.PlayPhaseStarting : SkillProgramTriggerWindow.DiscardPhaseEnded) || !MouMhPhaseParent(f) || !Enum.IsDefined(r.Stage) || r.Budget is < 0 or > 7 || r.Cursor < 0 || r.Cursor > r.Targets.Count || r.Targets.Count > r.Budget || r.Targets.Distinct().Count() != r.Targets.Count || r.Targets.Any(t => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(t)) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<MouMengHuoStartedEvent>().Count(e => e == new MouMengHuoStartedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, r.Obtain, r.Budget)) != 1)
            return false;
        if (r.Obtain)
            return r.Stage == MouMengHuoStage.ObtainChildren && r.Accepted && r.Budget == 0 && r.Targets.Count == 0 && r.Payment is null && r.Draw is null;
        if (!r.Accepted)
            return r.Stage == MouMengHuoStage.Selecting && r.Cursor == 0 && r.Payment is null && r.Draw is null && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouMengHuoChargeSpentEvent>().Any(e => e.FrameId == f.Id);
        var selected = _coreProgramEventHistoryOperations.ProgramEventHistory<MouMengHuoTargetSelectedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
        if (selected.Length != r.Targets.Count || selected.Where((e, i) => e.Index != i || e.TargetSeat != r.Targets[i]).Any() || _coreProgramEventHistoryOperations.ProgramEventHistory<MouMengHuoChargeSpentEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.Count == r.Targets.Count && e.Before - e.After == e.Count) != 1)
            return false;
        if (r.Payment is { } p && (r.Cursor >= r.Targets.Count || p.From.OwnerSeat != r.Targets[r.Cursor] || p.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouMengHuoDiscardPaidEvent>().Count(e => e == new MouMengHuoDiscardPaidEvent(f.Id, r.Cursor, r.Targets[r.Cursor], p)) != 1 || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == p.Sequence && m.CardId == p.CardId && m.From == p.From && m.To == p.To && m.CardKind == p.PrintedKind && m.Reason.Value == MouMhDiscardReason)))
            return false;
        if (r.Stage == MouMengHuoStage.DrawChildren)
            return r.Draw is { } d && d.RecipientSeat == f.OwnerSeat && d.RequestedCount == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<MouMengHuoDrawReturnedEvent>().Count(e => e == new MouMengHuoDrawReturnedEvent(f.Id, r.Cursor, 1, d.ActualDrawIds.Count, d.ReplacementGainedIds.Count, d.SequenceBefore, d.SequenceAfter, d.AttemptFrameId)) == 1;
        return r.Draw is null && r.Stage is MouMengHuoStage.RecipientChoice or MouMengHuoStage.DiscardChildren or MouMengHuoStage.RecoveryChildren or MouMengHuoStage.Drawing;
    }

    internal void SealMouMh(long id, long before)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouMengHuo = f.MouMengHuo!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence))
        {
            var fact = new MouMengHuoMovementSignedEvent(id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence);
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouMengHuoMovementSignedEvent>().Contains(fact))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
        }
    }

    internal bool DrainMouMh(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 30900)]
    internal bool IsMouMhMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => effect is not null && MouMengHuoComposition.Handles(effect.Op) && ValidMouMh(f) && pending == f.PendingMovementContinuation && pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && f.MouMengHuo!.Stage is MouMengHuoStage.ObtainChildren or MouMengHuoStage.DiscardChildren or MouMengHuoStage.RecoveryChildren or MouMengHuoStage.DrawChildren;
    internal bool MouMhFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouMh(f))
            return false;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return f.NativeDraw is not null && f.MouMengHuo!.Stage == MouMengHuoStage.Drawing;
        if (IsMouMhSilverLionChild(f, child))
            return true;
        if (child is RecoveryReplacementFrame recovery)
            return recovery.ParentFrameId == f.Id && _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp) && hp.Change.ParentFrameId == f.Id && _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReceiptMatches(f, hp, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreProgramEventHistoryOperations.ProgramEventHistory<MouMengHuoMovementSignedEvent>().Contains(new(f.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence)));
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool IsMouMhSilverLionChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouMh(f) || f.MouMengHuo is not { Stage: MouMengHuoStage.DiscardChildren, Payment: { PrintedKind: CardKind.SilverLion, From.Zone: CardZoneKind.Equipment } paid } r || r.Cursor >= r.Targets.Count || paid.From.OwnerSeat != r.Targets[r.Cursor] || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == paid.Sequence && m.CardId == paid.CardId && m.CardKind == CardKind.SilverLion && m.From == paid.From && m.To == paid.To && m.Reason.Value == MouMhDiscardReason))
            return false;
        var recipient = r.Targets[r.Cursor];
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == MouMhDiscardReason && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == recipient && recovery.Attempt.TargetSeat == recipient;
        return child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount > 0 && hp.Change.SourceSeat == recipient && hp.Change.TargetSeat == recipient && _coreProgramEventHistoryOperations.ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(e => e.ResolutionId > f.Id && e.ResolutionId < hp.Change.Id && e.PlayerSeat == recipient && e.Reason.Value == MouMhDiscardReason && e.RecoveredAmount == hp.Change.Amount && e.RemainingHp == hp.Change.HpAfter);
    }

    internal bool MouMhObserverSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame f && _characterMengHuoMouMobileOperations.MouMhFirstChild(f, _resolutionStackOperations2._resolutionStack()[n]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? MouMhObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterMengHuoMouMobileOperations.MouMhFirstChild(f, child), (i) => _characterMengHuoMouMobileOperations.MouMhObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 30900)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 30900)]
    internal bool HasMouMhDamageObserver(long id) => MouMhObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 30900)]
    internal bool HasMouMhBeforeDamageObserver(long id) => MouMhObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 30900)]
    internal bool IsMouMhDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MouMhObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 30900)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 30900)]
    internal bool IsMouMhRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (!ValidMouMh(f))
            return false;
        if (IsMouMhSilverLionChild(f, recovery))
            return true;
        if (recovery.ParentFrameId == f.Id)
            return _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return index >= 0 && child > index + 1 && MouMhFirstChild(f, _resolutionStackOperations._resolutionStack[index + 1]) && MouMhObserverSuffix(index) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 30900)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 30900)]
    internal bool KeepPaidMouMh(ProgramSkillFrame f) => f.MouMengHuo is { Accepted: true } && ValidMouMh(f);
    [ProgramContinuation(ProgramContinuationKind.Resume, 30900)]
    internal bool ResumeMouMh(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouMengHuo: not null } f || f.Id != id)
            return false;
        ContinueMouMh(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 30900)]
    internal bool ReturnMouMhMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeMouMh(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 30900)]
    internal bool AdvanceMouMhChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouMhObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
