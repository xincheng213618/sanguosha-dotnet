#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGongsunZanMouMobileRuntime
{
    internal bool ValidMouGszMobile(ProgramSkillFrame f)
    {
        if (f.MouGongsunZanMobile is not { } r || r.Source != MouGszMobileSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || f.InstructionIndex != 1 || r.WindowId != f.WindowContext?.ParentFrameId || r.Round < 1 || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || !Enum.IsDefined(r.Stage) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouGongsunZanMobileStartedEvent>().Count(e => e == new MouGongsunZanMobileStartedEvent(f.Id, r.Source, r.GameplayHash, r.Op, r.WindowId, r.Round, r.DamageFrameId, r.TargetSeat)) != 1)
            return false;
        if (r.Op == SkillProgramEffectOp.ResolveMouGongsunZanYicong)
        {
            if (f.SkillId != MouGongsunZanMobileComposition.Yicong || f.TriggerId != "round-charge-and-hu" || f.WindowContext?.Window != SkillProgramTriggerWindow.RoundStarting || r.TargetSeat != -1 || r.DamageFrameId != 0 || r.Charge is < 0 or > 4 || !_resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Any(w => w.Id == r.WindowId && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex])))
                return false;
            if (r.Accepted && _coreProgramEventHistoryOperations.ProgramEventHistory<MouGongsunZanMobileDistanceEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Round == r.Round && e.Charge == r.Charge && e.Offensive == r.Offensive) != 1)
                return false;
        }
        else if (r.Op != SkillProgramEffectOp.ResolveMouGongsunZanQiaomeng || f.SkillId != MouGongsunZanMobileComposition.Qiaomeng || f.TriggerId != "slash-damage-reward" || f.WindowContext is not { } c || MouGszMobileDamage(c, f.OwnerSeat)is not { } damage || damage.Id != r.DamageFrameId || damage.TargetSeat != r.TargetSeat || !_resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().Any(w => w.Id == r.WindowId && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex].ToProgramCandidate())))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<MouGongsunZanMobileStoredEvent>().Where(e => e.FrameId == f.Id).All(e => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.CardKind == e.Kind && m.From == e.From && m.To == e.To)) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouGongsunZanMobileDiscardedEvent>().Where(e => e.FrameId == f.Id).All(e => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.CardKind == e.Kind && m.From == e.From && m.To == e.To));
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 33600)]
    internal bool MouGszMobileMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => e is not null && MouGongsunZanMobileComposition.Handles(e.Op) && ValidMouGszMobile(f) && f.MouGongsunZanMobile!.Stage is MouGongsunZanMobileStage.MovementChildren or MouGongsunZanMobileStage.DrawChildren && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouGszMobileLion(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouGszMobile(f) || f.MouGongsunZanMobile is not { Stage: MouGongsunZanMobileStage.MovementChildren })
            return false;
        var p = _coreProgramEventHistoryOperations.ProgramEventHistory<MouGongsunZanMobileDiscardedEvent>().SingleOrDefault(e => e.FrameId == f.Id && e.Kind == CardKind.SilverLion && e.From.Zone == CardZoneKind.Equipment);
        if (p?.From.OwnerSeat is not { } seat)
            return false;
        if (child is RecoveryReplacementFrame r)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(r, f) && r.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && r.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && r.Attempt.Completion.MoveReason?.Value == "skill-program.mou-mobile:qiaomeng.discard" && r.Attempt.SourceSeat == seat && r.Attempt.TargetSeat == seat;
        return child is HpChangedTriggerWindowFrame h && h.Change.ParentFrameId == f.Id && h.ResumeFrameId == f.Id && h.Continuation == PostEventContinuation.AwaitedProgramMovement && h.Change.Kind == HpChangeKind.Recovery && h.Change.SourceSeat == seat && h.Change.TargetSeat == seat && _coreProgramEventHistoryOperations.ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(e => e.PlayerSeat == seat && e.Reason.Value == "skill-program.mou-mobile:qiaomeng.discard" && e.ResolutionId > f.Id && e.ResolutionId < h.Change.Id && e.RecoveredAmount == h.Change.Amount);
    }

    internal bool MouGszMobileChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouGszMobile(f))
            return false;
        var r = f.MouGongsunZanMobile!;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouGongsunZanMobileStage.Drawing && f.NativeDraw is not null;
        if (MouGszMobileLion(f, child))
            return true;
        if (child is RecoveryReplacementFrame recovery)
            return _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp);
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is not null && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouGszMobileSuffix(int n) => _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[i - 1], _resolutionStackOperations2._resolutionStack()[i]) || _resolutionStackOperations2._resolutionStack()[i - 1] is ProgramSkillFrame f && _characterGongsunZanMouMobileOperations.MouGszMobileChild(f, _resolutionStackOperations2._resolutionStack()[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, d));
    internal ProgramSkillFrame? MouGszMobileRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterGongsunZanMouMobileOperations.MouGszMobileChild(f, c), (n) => _characterGongsunZanMouMobileOperations.MouGszMobileSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 33600)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 33600)]
    internal bool MouGszMobileDamageProof(long id) => MouGszMobileRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 33600)]
    internal bool MouGszMobileBeforeDamage(long id) => MouGszMobileRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 33600)]
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 33600)]
    internal bool MouGszMobileDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouGszMobileRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 33600)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 33600)]
    internal bool MouGszMobileRecovery(ProgramSkillFrame f, RecoveryReplacementFrame child)
    {
        if (!ValidMouGszMobile(f))
            return false;
        if (MouGszMobileLion(f, child) || _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, child, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray()))
            return true;
        var a = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var b = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == child.Id);
        return a >= 0 && b > a + 1 && MouGszMobileChild(f, _resolutionStackOperations._resolutionStack[a + 1]) && MouGszMobileSuffix(a) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, _resolutionStackOperations._resolutionStack[b - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 33600)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 33600)]
    internal bool KeepMouGszMobile(ProgramSkillFrame f) => f.MouGongsunZanMobile is { Accepted: true } && ValidMouGszMobile(f);
    [ProgramContinuation(ProgramContinuationKind.Resume, 33600)]
    internal bool ResumeMouGszMobile(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouGongsunZanMobile: not null } f || f.Id != id)
            return false;
        ContinueMouGszMobile(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 33600)]
    internal bool ReturnMouGszMobileMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeMouGszMobile(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 33600)]
    internal bool AdvanceMouGszMobileChild() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouGszMobileRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 33600)]
    internal void AssertMouGszMobile(ProgramSkillFrame f)
    {
        if (f.MouGongsunZanMobile is not null && !ValidMouGszMobile(f))
            throw new InvalidOperationException("Mobile Gongsun Zan lost its own native trigger or paid child invoice.");
    }
}
