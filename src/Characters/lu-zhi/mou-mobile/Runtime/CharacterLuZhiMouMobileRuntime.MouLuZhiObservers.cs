#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuZhiMouMobileRuntime
{
    internal bool ValidMouLuZhi(ProgramSkillFrame f)
    {
        if (f.MouLuZhi is not { } r || r.Source != MouLuZhiSource(f) || r.GameplayHash != f.GameplayHash || f.InstructionIndex != 1 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Op || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.Before > r.After || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Cards is not System.Collections.IList { IsReadOnly: true } || r.Cards.Distinct().Count() != r.Cards.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuZhiStartedEvent>().Count(e => e == new MouLuZhiStartedEvent(f.Id, r.Source, r.GameplayHash, r.Op, r.ActionId, r.Phase)) != 1)
            return false;
        if (r.Op == SkillProgramEffectOp.ResolveMouLuZhiZhenliangYang)
        {
            if (f.SkillId != MouLuZhiZhenliang || f.ActivationId != "yang-damage" || f.TriggerId is not null || f.WindowContext is not null || r.Phase is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) || r.Ren is null || r.ActionId is not null || r.Cards.Count > r.RequiredCards)
                return false;
            if (r.Stage is MouLuZhiStage.MovementChildren or MouLuZhiStage.DamageIssued)
            {
                if (r.RequiredCards < 1 || r.Cards.Count != r.RequiredCards || r.IsRed is null || _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuZhiPaidEvent>().Count(e => e == new MouLuZhiPaidEvent(f.Id, r.TargetSeat, r.RequiredCards, r.IsRed, r.Before, r.After)) != 1 || r.Cards.Any(id => !_coreProgramEventHistoryOperations.ProgramEventHistory<MouLuZhiMovementEvent>().Any(e => e.FrameId == f.Id && e.CardId == id && e.From.OwnerSeat == f.OwnerSeat && e.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && e.Reason.Value == MouLuZhiDiscardReason && e.Sequence > r.Before && e.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.Sequence && m.CardId == id && m.From == e.From && m.To == e.To && m.Reason == e.Reason))))
                    return false;
            }
        }
        else if (f.ActivationId != f.TriggerId || f.WindowContext is not { } context || context.OwnerSeat != f.OwnerSeat || (r.Op == SkillProgramEffectOp.InitializeMouLuZhiMingren ? f.SkillId != MouLuZhiMingren || f.TriggerId != "initial-ren" || context.Window != SkillProgramTriggerWindow.GameStarting : r.Op == SkillProgramEffectOp.ExchangeMouLuZhiMingren ? f.SkillId != MouLuZhiMingren || f.TriggerId != "exchange-ren" || context.Window != SkillProgramTriggerWindow.TurnEnding : r.Op != SkillProgramEffectOp.ResolveMouLuZhiZhenliangYin || f.SkillId != MouLuZhiZhenliang || context.CardUse?.CardActionId != r.ActionId || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame w || w.Action.ActionId != r.ActionId || global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.GetCardActionWindow(w) != context.Window))
            return false;
        if (r.Stage == MouLuZhiStage.DrawChildren && (r.Draw is not { } d || d.RecipientSeat != r.TargetSeat || d.RequestedCount != 2 || _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuZhiDrawReturnedEvent>().Count(e => e == new MouLuZhiDrawReturnedEvent(f.Id, r.TargetSeat, 2, d.ActualDrawIds.Count, d.ReplacementGainedIds.Count, d.AttemptFrameId)) != 1))
            return false;
        return true;
    }

    internal void SealMouLuZhiMovements(long id, long before)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouLuZhi = f.MouLuZhi!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence).ToArray())
        {
            var fact = new MouLuZhiMovementEvent(id, m.CardId, m.From, m.To, m.Reason, m.Sequence);
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouLuZhiMovementEvent>().Contains(fact))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
        }
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 31000)]
    internal bool IsMouLuZhiMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation pending) => f.MouLuZhi is { Stage: MouLuZhiStage.MovementChildren or MouLuZhiStage.DrawChildren } r && e?.Op == r.Op && pending == f.PendingMovementContinuation && pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && ValidMouLuZhi(f);
    internal bool MouLuZhiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouLuZhi(f) || f.MouLuZhi is not { } r)
            return false;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouLuZhiStage.Drawing && f.NativeDraw is not null;
        if (MouLuZhiSilverLionChild(f, child))
            return true;
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && IsMouLuZhiMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuZhiMovementEvent>().Contains(new(f.Id, m.CardId, m.From, m.To, m.Reason, m.Sequence)));
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouLuZhiSilverLionChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouLuZhi is not { Op: SkillProgramEffectOp.ResolveMouLuZhiZhenliangYang, Stage: MouLuZhiStage.MovementChildren } r)
            return false;
        var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuZhiMovementEvent>().FirstOrDefault(e => e.FrameId == f.Id && r.Cards.Contains(e.CardId) && e.From == CardLocation.Equipment(f.OwnerSeat) && e.Reason.Value == MouLuZhiDiscardReason && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.CardKind == CardKind.SilverLion));
        if (paid is null)
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == MouLuZhiDiscardReason && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat;
        return child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount > 0 && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.TargetSeat == f.OwnerSeat && _coreProgramEventHistoryOperations.ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(e => e.ResolutionId > f.Id && e.ResolutionId < hp.Change.Id && e.PlayerSeat == f.OwnerSeat && e.Reason.Value == MouLuZhiDiscardReason && e.RecoveredAmount == hp.Change.Amount && e.RemainingHp == hp.Change.HpAfter);
    }

    internal bool MouLuZhiObserverEdge(int n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[n - 1], _resolutionStackOperations._resolutionStack[n]) || _resolutionStackOperations._resolutionStack[n - 1] is ProgramSkillFrame f && MouLuZhiFirstChild(f, _resolutionStackOperations._resolutionStack[n]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]);
    internal bool MouLuZhiObserverSuffix(int index) => _resolutionStackOperations._resolutionStack[index + 1] is DyingFrame dying && index + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(index + 1, dying) || _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _characterLuZhiMouMobileOperations.MouLuZhiObserverEdge(n), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? MouLuZhiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterLuZhiMouMobileOperations.MouLuZhiFirstChild(f, c), (n) => _characterLuZhiMouMobileOperations.MouLuZhiObserverSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 31000)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 31000)]
    internal bool HasMouLuZhiDamageObserver(long id) => MouLuZhiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 31000)]
    internal bool HasMouLuZhiBeforeDamageObserver(long id) => MouLuZhiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 31000)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 31000)]
    internal bool IsMouLuZhiDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MouLuZhiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 31000)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 31000)]
    internal bool IsMouLuZhiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (MouLuZhiSilverLionChild(f, recovery))
            return true;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return index >= 0 && child > index + 1 && MouLuZhiFirstChild(f, _resolutionStackOperations._resolutionStack[index + 1]) && MouLuZhiObserverSuffix(index) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 31000)]
    internal bool ResumeMouLuZhi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouLuZhi: { } r } f || f.Id != id)
            return false;
        // CompleteProgramAttack clears both native attack fields before it
        // returns to the ordinary executor. Do not consume that completed
        // return as another movement continuation.
        if (r.Stage == MouLuZhiStage.DamageIssued)
            return f.AttackAttempt is not null || f.AttackReturn is not null;
        ContinueMouLuZhi(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 31000)]
    internal bool ReturnMouLuZhiMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeMouLuZhi(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 31000)]
    internal bool AdvanceMouLuZhiChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouLuZhiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 31000)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 31000)]
    internal bool ContinueAcceptedMouLuZhi(ProgramSkillFrame f) => f.MouLuZhi is { } r && ValidMouLuZhi(f) && (r.Op == SkillProgramEffectOp.InitializeMouLuZhiMingren || r.Stage is MouLuZhiStage.Drawing or MouLuZhiStage.DrawChildren or MouLuZhiStage.MovementChildren or MouLuZhiStage.DamageIssued);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 31000)]
    internal void AssertMouLuZhi(ProgramSkillFrame f)
    {
        if (f.MouLuZhi is not null && !ValidMouLuZhi(f))
            throw new InvalidOperationException("Mou Lu Zhi lost its owning receipt or paid movement.");
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 31000)]
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 31000)]
    internal void CleanupMouLuZhiRen()
    {
        if (!HasMouLuZhiCapability || !_cardZonesOperations._cardZones.HasPrivatePersistentCards())
            return;
        foreach (var ren in _coreMatchParticipantsPlayersAccess._players.SelectMany(p => LiveMouLuZhiRen(p.Seat)).Where(r => !_coreMatchParticipantsPlayersAccess._players[r.Source.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.Source.OwnerSeat].SkillGrants.Grants.Any(g => g.SkillId == r.Source.SkillId && g.SkillInstanceId == r.Source.SkillInstanceId)).ToArray())
        {
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(ren.CardId), ren.Location, CardLocation.DiscardPile, new("skill-program.mou-mobile:mingren.source-lost"));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuZhiRenRemovedEvent(ren));
        }
    }

    [ProgramViewContributor(ProgramViewContribution.PrivateWhirlPiles, 10)]
    internal IReadOnlyList<PrivateWhirlPileSnapshot>? AppendMouLuZhiRenPiles(IReadOnlyList<PrivateWhirlPileSnapshot>? existing, int owner, int viewer, bool revealAll)
    {
        var ren = LiveMouLuZhiRen(owner);
        if (ren.Length == 0)
            return existing;
        return Array.AsReadOnly((existing ?? []).Concat(ren.Select(r => new PrivateWhirlPileSnapshot { SkillId = MouLuZhiMingren, PileName = "任", Count = 1, Cards = owner == viewer || revealAll ? [_coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId))] : null })).ToArray());
    }
}
