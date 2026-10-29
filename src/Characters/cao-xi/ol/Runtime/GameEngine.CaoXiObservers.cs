namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool CaoXiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.CaoXiJianxuan is not { Stage: CaoXiJianxuanStage.DrawChildren } r || !ValidCaoXiJianxuan(f)) return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == f.Id && moved.ResumeProgramFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat &&
                moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 &&
                moved.Batch.Movements.All(m => _cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After &&
                    (m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.RecipientSeat!.Value) && m.Reason.Value == CaoXiDrawReason || m.Reason == CardMoveReasons.Reshuffle));
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id &&
            skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }
    private ProgramSkillFrame? CaoXiObserverRoot() => FindProgramObserverRoot(static (e, f, c) => e.CaoXiFirstChild(f, c),
        static (e, i) => e.HasObserverSuffix(i, static (x, n) => x.IsMovementObserverEdge(n) || x.RecoveryReplacementFrameRidesOn(x._resolutionStack[n], x._resolutionStack[n - 1]),
            static (x, n, d) => x.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 530)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 640)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 670)]
    private bool HasCaoXiDamageObserver(long id) => CaoXiObserverRoot() is { } root &&
        _resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) &&
        _resolutionStack.FindIndex(f => f.Id == id) > _resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 1450)]
    private bool IsCaoXiDying() => ActiveDying is { } dying && CaoXiObserverRoot() is { } root &&
        _resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26810)]
    private bool AdvanceCaoXiSubtree() => _pendingDecision is null && CaoXiObserverRoot() is not null && TryAdvanceObserverSubtree(true);
}
