namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool SourceMouFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidSourceMou(f) || f.ShanheSourceMou is not { Paid: true } r) return false;
        if (child is CardUseFrame wine) return r.Stage == ShanheSourceMouStage.Wine && r.WineUseId == wine.Id && IsShanheSourceMouWine(wine.Id);
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } pending)
            return SourceMouMovement(f, GetPausedPrivateOfferEffect(f), pending) && ProgramMovementReturnMatches(f, moved) &&
                moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId &&
                moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(move => _cardMovements.Contains(move) && move.Sequence > r.Before && move.Sequence <= r.After);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id &&
                (r.Stage == ShanheSourceMouStage.ThresholdChildren && r.Ordinal == 3 && hp.Change.TargetSeat == r.Actor && hp.Change.Kind == HpChangeKind.Loss ||
                 r.Stage == ShanheSourceMouStage.MaximumChildren && hp.Change.TargetSeat == r.Payer && hp.Change.Kind is HpChangeKind.MaximumHp or HpChangeKind.Loss ||
                 r.Stage is ShanheSourceMouStage.OwnChildren or ShanheSourceMouStage.PeerChildren && hp.Change.TargetSeat == r.Payer && hp.Change.Kind == HpChangeKind.Recovery);
        if (child is RecoveryReplacementFrame recovery)
            return RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement &&
                recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == SourceMouCost &&
                recovery.Attempt.TargetSeat == r.Payer && r.Cards.Any(card => card.Kind == CardKind.SilverLion && card.From == CardLocation.Equipment(r.Payer));
        if (child is DyingFrame dying)
            return r.Stage == ShanheSourceMouStage.ThresholdChildren && r.Ordinal == 3 && dying.VictimSeat == r.Actor && DyingSuitsStructuralEdge(f, child);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id &&
            (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram ||
             state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }
    private bool SourceMouEdge(int index) => IsMovementObserverEdge(index) || IsSameNameHandObserverEdge(index) || IsYanPuDyingExitEdge(index) ||
        TypedDamagePostEventEdge(_resolutionStack[index - 1], _resolutionStack[index]) ||
        _resolutionStack[index - 1] is ProgramSkillFrame parent && SourceMouFirstChild(parent, _resolutionStack[index]) ||
        DyingSuitsStructuralEdge(_resolutionStack[index - 1], _resolutionStack[index]) ||
        NativeDrawAttemptRidesOn(_resolutionStack[index], _resolutionStack[index - 1]) || RecoveryReplacementFrameRidesOn(_resolutionStack[index], _resolutionStack[index - 1]);
    private bool SourceMouSuffix(int index) => _resolutionStack[index + 1] is DyingFrame dying && index + 2 < _resolutionStack.Count && IsSuspendedOrPaidRescueObserverSuffix(index + 1, dying) ||
        HasObserverSuffix(index, static (game, edge) => game.SourceMouEdge(edge), static (game, edge, frame) => game.IsSuspendedOrPaidRescueObserverSuffix(edge, frame));
    private ProgramSkillFrame? SourceMouRoot() => FindProgramObserverRoot(static (game, frame, child) => game.SourceMouFirstChild(frame, child), static (game, index) => game.SourceMouSuffix(index));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 62053)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 62053)]
    private bool SourceMouDamageWindow(long id) => SourceMouRoot() is { } root && _resolutionStack.FindIndex(frame => frame.Id == id && frame is DamageTriggerWindowFrame) > _resolutionStack.FindIndex(frame => frame.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 62053)]
    private bool SourceMouBeforeDamage(long id) => SourceMouRoot() is { } root && _resolutionStack.FindIndex(frame => frame.Id == id && frame is BeforeDamageProgramWindowFrame) > _resolutionStack.FindIndex(frame => frame.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 62053)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 62053)]
    private bool SourceMouDying() => ActiveDying is { } dying && SourceMouRoot() is { } root && _resolutionStack.FindIndex(frame => frame.Id == dying.Id) > _resolutionStack.FindIndex(frame => frame.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 62053)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 62053)]
    private bool SourceMouRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (SourceMouFirstChild(f, recovery)) return true;
        var root = _resolutionStack.FindIndex(frame => frame.Id == f.Id); var child = _resolutionStack.FindIndex(frame => frame.Id == recovery.Id);
        return root >= 0 && child > root + 1 && SourceMouFirstChild(f, _resolutionStack[root + 1]) && SourceMouSuffix(root) && RecoveryReplacementFrameRidesOn(recovery, _resolutionStack[child - 1]);
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 62053)]
    private bool AdvanceSourceMouChildren() => _pendingDecision is null && SourceMouRoot() is not null && TryAdvanceObserverSubtree(true);
}
