#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangZhongAnniversaryGodRuntime
{
    internal bool AnniversaryHzTianchongHealthChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.SkillId != AnniversaryHzLieqiong || root.AnniversaryHuangZhong is not { Op: SkillProgramEffectOp.ResolveAnniversaryHuangZhongLieqiong, Body: AnniversaryHzBody.Tianchong, Stage: AnniversaryHzStage.TianchongChildren, HpPaid: true, Amount: > 0, DyingId: > 0 } paid || root.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, TargetSeat: { } target } || target != paid.Target || paid.Source != AnniversaryHzSource(root) || paid.GameplayHash != root.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHzBodyIssuedEvent>().Count(e => e.Id == root.Id && e.Source == paid.Source && e.Target == target && e.Body == AnniversaryHzBody.Tianchong) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillHpLostEvent>().Where(e => e.FrameId == root.Id).ToArray()is not [var loss] || loss.SkillId != root.SkillId || loss.TargetSeat != target || loss.Amount != paid.Amount || loss.RemainingHp != 0)
            return false;
        AssertAnniversaryHz(root);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Loss && hp.Change.SourceSeat is null && hp.Change.TargetSeat == target && hp.Change.HpBefore == paid.Amount && hp.Change.HpAfter == 0 && hp.Change.Amount == paid.Amount;
        return child is DyingFrame dying && dying.Id == paid.DyingId && dying.ParentFrameId == root.Id && dying.VictimSeat == target && dying.Continuation == DyingContinuationKind.ProgramSkill && dying.KillerSeat is null && _coreProgramEventHistoryOperations.ProgramEventHistory<PlayerDyingEvent>().Count(e => e.ResolutionId == dying.Id && e.VictimSeat == target && e.KillerSeat is null) == 1 && _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.Any(change => change.ParentFrameId == root.Id && change.Kind == HpChangeKind.Loss && change.SourceSeat is null && change.TargetSeat == target && change.HpBefore == paid.Amount && change.HpAfter == 0 && change.Amount == paid.Amount);
    }

    internal ProgramSkillFrame? AnniversaryHzTianchongObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _characterHuangZhongAnniversaryGodOperations.AnniversaryHzTianchongHealthChild(root, child), (index) => _characterHuangZhongAnniversaryGodOperations.AnniversaryHzTianchongSuffix(index));
    internal bool AnniversaryHzTianchongSuffix(int rootIndex) => _coreProgramHealthChildQueriesOperations.CreateProgramHealthChildQueries().PaidObserverSuffix(_resolutionStackOperations._resolutionStack[rootIndex].Id) || _coreObserverSubtreesOperations.HasObserverSuffix(rootIndex, (index) => _coreObserverSubtreesOperations2.IsMovementObserverEdge(index) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[index - 1], _resolutionStackOperations2._resolutionStack()[index]) || _resolutionStackOperations2._resolutionStack()[index] is CardUseFrame { ShanheSourceMouWine: not null } wine && _coreShanheSourceMouDongZhuoWineOperations.IsShanheSourceMouWine(wine.Id), (index, dying) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(index, dying));
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 39600)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 39600)]
    internal bool IsAnniversaryHzTianchongDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && AnniversaryHzTianchongObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 39600)]
    internal bool HasAnniversaryHzTianchongDamageObserver(long windowId) => AnniversaryHzTianchongObserverRoot()is { } root && root.WindowContext?.ParentFrameId == windowId && _resolutionStackOperations._resolutionStack.Any(frame => frame.Id == windowId && frame is DamageTriggerWindowFrame);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 39600)]
    internal bool AdvanceAnniversaryHzTianchongChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && AnniversaryHzTianchongObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
