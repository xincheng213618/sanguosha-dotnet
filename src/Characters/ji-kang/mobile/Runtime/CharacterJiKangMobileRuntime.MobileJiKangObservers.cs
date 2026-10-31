#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiKangMobileRuntime
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 80900)]
    internal void AssertMobileJiKang(ProgramSkillFrame f)
    {
        if (f.MobileJiKang is not { } r)
            return;
        if (r.Source != MobileJiKangSource(f) || r.Hash != f.GameplayHash || f.InstructionIndex != 1 || !Enum.IsDefined(r.Stage) || r.Targets.Count != r.Targets.Distinct().Count() || r.Targets.Any(seat => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) || seat == f.OwnerSeat) || r.Cards.Count != r.Cards.Distinct().Count() || r.Cursor < 0 || r.Cursor > r.Targets.Count || r.Before > r.After || r.After > _coreCardMovementJournalMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileJiKangIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var issued] || issued != new MobileJiKangIssuedEvent(f.Id, r.Source, r.Hash, r.InitialHp, r.Killer) || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != (f.SkillId == "mobile:juexiang" ? SkillProgramEffectOp.ResolveMobileJiKangJuexiang : SkillProgramEffectOp.ResolveMobileJiKangQingxian))
            throw new InvalidOperationException("Mobile Ji Kang lost his original issuing identity, frozen participants or native cursor.");
        if (f.SkillId == "mobile:juexiang")
        {
            if (f.WindowContext is not { Window: SkillProgramTriggerWindow.OwnerDied } || r.Killer is { } killer && !_coreTableSeatQueriesOperations.IsValidPlayerSeat(killer))
                throw new InvalidOperationException("Juexiang lost its actual owner-death producer.");
        }
        else if (f.SkillId is not ("mobile:qingxian" or "mobile:canyun") || f.ActivationId != "equipment-comparison" || f.WindowContext is not null || r.InitialHp < 1 || r.Targets.Count > r.InitialHp)
            throw new InvalidOperationException("Qingxian lost its issued Play activation or frozen HP bound.");
        var selected = _coreProgramEventHistoryOperations.ProgramEventHistory<MobileJiKangTargetIssuedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
        if (selected.Length > 0 && (selected.Length != r.Targets.Count || selected.Where((e, index) => e.Source != r.Source || e.Index != index || e.TargetSeat != r.Targets[index]).Any()))
            throw new InvalidOperationException("Canyun lost its exact source-bound, paid target ledger.");
    }

    internal bool MobileJiKangPaidInstruction(ProgramSkillFrame f)
    {
        if (f.MobileJiKang is not { } r)
            return false;
        AssertMobileJiKang(f);
        if (f.SkillId == "mobile:juexiang")
            return f.WindowContext?.Window == SkillProgramTriggerWindow.OwnerDied;
        return r.Targets.Count > 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<MobileJiKangProgressEvent>().Any(e => e.FrameId == f.Id && e.Step == "discard" && e.Subject == f.OwnerSeat && e.Value == r.Targets.Count && e.After > e.Before);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 80900)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 80900)]
    internal bool KeepMobileJiKangIssued(ProgramSkillFrame f) => MobileJiKangPaidInstruction(f);
    internal bool MobileJiKangObserverChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MobileJiKang is not { } r)
            return false;
        AssertMobileJiKang(f);
        if (r.Stage is MobileJiKangStage.Health or MobileJiKangStage.KillerHealth)
            return _characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoHealthChoiceRidesOn(child, f);
        if (child is DyingFrame dying)
            return r.Stage is MobileJiKangStage.HealthChildren or MobileJiKangStage.KillerHealthChildren && dying.ParentFrameId == f.Id && dying.VictimSeat == r.Subject && dying.ResumesProgramSkill && dying.KillerSeat is null;
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp) && (r.Stage is MobileJiKangStage.HealthChildren or MobileJiKangStage.KillerHealthChildren && hp.Change.TargetSeat == r.Subject || r.Stage is MobileJiKangStage.CostChildren or MobileJiKangStage.KillerDiscardChildren or MobileJiKangStage.ClubChildren && hp.Change.Kind == HpChangeKind.Recovery);
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.Program && (r.Stage == MobileJiKangStage.HealthChildren && r.Comparison < 0 && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == r.Subject && recovery.Attempt.Amount == 1 || r.Stage is MobileJiKangStage.CostChildren or MobileJiKangStage.KillerDiscardChildren or MobileJiKangStage.ClubChildren && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == MobileJiKangDiscard);
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage is MobileJiKangStage.CostChildren or MobileJiKangStage.KillerDiscardChildren or MobileJiKangStage.ClubChildren && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After);
        return child is ProgramLifecycleTriggerWindowFrame lifecycle && lifecycle.ResumeProgramFrameId == f.Id && (lifecycle.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange || lifecycle is { Window: SkillProgramTriggerWindow.SkillsChanged, Continuation: ProgramLifecycleContinuation.ResumeParentProgram });
    }

    internal ProgramSkillFrame? MobileJiKangObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterJiKangMobileOperations.MobileJiKangObserverChild(f, child), (index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[i - 1], _resolutionStackOperations2._resolutionStack()[i]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying)));
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 80900)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 80900)]
    internal bool MobileJiKangDyingProof() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MobileJiKangObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 80900)]
    internal bool AdvanceMobileJiKangHealth() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MobileJiKangObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
