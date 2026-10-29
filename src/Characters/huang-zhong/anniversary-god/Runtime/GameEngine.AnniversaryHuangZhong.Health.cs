namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool AnniversaryHzTianchongHealthChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.SkillId != AnniversaryHzLieqiong || root.AnniversaryHuangZhong is not
            { Op: SkillProgramEffectOp.ResolveAnniversaryHuangZhongLieqiong, Body: AnniversaryHzBody.Tianchong,
              Stage: AnniversaryHzStage.TianchongChildren, HpPaid: true, Amount: > 0, DyingId: > 0 } paid ||
            root.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, TargetSeat: { } target } ||
            target != paid.Target || paid.Source != AnniversaryHzSource(root) || paid.GameplayHash != root.GameplayHash ||
            ProgramEventHistory<AnniversaryHzBodyIssuedEvent>().Count(e => e.Id == root.Id && e.Source == paid.Source &&
                e.Target == target && e.Body == AnniversaryHzBody.Tianchong) != 1 ||
            ProgramEventHistory<ProgramSkillHpLostEvent>().Where(e => e.FrameId == root.Id).ToArray() is not [var loss] ||
            loss.SkillId != root.SkillId || loss.TargetSeat != target || loss.Amount != paid.Amount || loss.RemainingHp != 0)
            return false;
        AssertAnniversaryHz(root);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.Program &&
                hp.Change.Kind == HpChangeKind.Loss && hp.Change.SourceSeat is null && hp.Change.TargetSeat == target &&
                hp.Change.HpBefore == paid.Amount && hp.Change.HpAfter == 0 && hp.Change.Amount == paid.Amount;
        return child is DyingFrame dying && dying.Id == paid.DyingId && dying.ParentFrameId == root.Id && dying.VictimSeat == target &&
            dying.Continuation == DyingContinuationKind.ProgramSkill && dying.KillerSeat is null &&
            ProgramEventHistory<PlayerDyingEvent>().Count(e => e.ResolutionId == dying.Id && e.VictimSeat == target && e.KillerSeat is null) == 1 &&
            _pendingHpChanges.Any(change => change.ParentFrameId == root.Id && change.Kind == HpChangeKind.Loss &&
                change.SourceSeat is null && change.TargetSeat == target && change.HpBefore == paid.Amount && change.HpAfter == 0 && change.Amount == paid.Amount);
    }

    private ProgramSkillFrame? AnniversaryHzTianchongObserverRoot() => FindProgramObserverRoot(
        static (engine, root, child) => engine.AnniversaryHzTianchongHealthChild(root, child),
        static (engine, index) => engine.AnniversaryHzTianchongSuffix(index));

    private bool AnniversaryHzTianchongSuffix(int rootIndex) =>
        new ProgramHealthChildQueries(this).PaidObserverSuffix(_resolutionStack[rootIndex].Id) ||
        HasObserverSuffix(rootIndex, static (engine, index) =>
            engine.IsMovementObserverEdge(index) ||
            engine.TypedDamagePostEventEdge(engine._resolutionStack[index - 1], engine._resolutionStack[index]) ||
            engine._resolutionStack[index] is CardUseFrame { ShanheSourceMouWine: not null } wine &&
                engine.IsShanheSourceMouWine(wine.Id),
            static (engine, index, dying) => engine.IsSuspendedOrPaidRescueObserverSuffix(index, dying));

    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 39600)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 39600)]
    private bool IsAnniversaryHzTianchongDying() => ActiveDying is { } dying && AnniversaryHzTianchongObserverRoot() is { } root &&
        _resolutionStack.FindIndex(frame => frame.Id == dying.Id) > _resolutionStack.FindIndex(frame => frame.Id == root.Id);

    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 39600)]
    private bool HasAnniversaryHzTianchongDamageObserver(long windowId) => AnniversaryHzTianchongObserverRoot() is { } root &&
        root.WindowContext?.ParentFrameId == windowId && _resolutionStack.Any(frame => frame.Id == windowId && frame is DamageTriggerWindowFrame);

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 39600)]
    private bool AdvanceAnniversaryHzTianchongChildren() => _pendingDecision is null && AnniversaryHzTianchongObserverRoot() is not null &&
        TryAdvanceObserverSubtree(true);
}
