namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool WuAnGuoFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.WuAnGuoLiyong is not { Stage: WuAnGuoLiyongStage.GainChildren } r || !ValidWuAnGuoLiyong(f)) return false;
        if (child is CardsMovedTriggerWindowFrame moved) return moved.Batch.ParentFrameId == f.Id && moved.ResumeProgramFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat &&
            moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements is [var m] && _cardMovements.Contains(m) &&
            m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == r.ObtainedCardId && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == LiyongGainReason;
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id &&
            skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }
    private ProgramSkillFrame? WuAnGuoObserverRoot() => FindProgramObserverRoot(static (e, f, c) => e.WuAnGuoFirstChild(f, c),
        static (e, i) => e.HasObserverSuffix(i, static (x, n) => x.IsMovementObserverEdge(n) || x.RecoveryReplacementFrameRidesOn(x._resolutionStack[n], x._resolutionStack[n - 1]),
            static (x, n, d) => x.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 580)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 690)]
    private bool HasWuAnGuoDamageObserver(long id) => WuAnGuoObserverRoot() is { } root && _resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) &&
        _resolutionStack.FindIndex(f => f.Id == id) > _resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 720)]
    private bool HasWuAnGuoBeforeDamageObserver(long id) => HasWuAnGuoDamageObserver(id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 540)]
    private bool IsWuAnGuoDying() => ActiveDying is { } dying && WuAnGuoObserverRoot() is { } root && _resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27010)]
    private bool AdvanceWuAnGuoSubtree() => _pendingDecision is null && WuAnGuoObserverRoot() is not null && TryAdvanceObserverSubtree(true);
}
