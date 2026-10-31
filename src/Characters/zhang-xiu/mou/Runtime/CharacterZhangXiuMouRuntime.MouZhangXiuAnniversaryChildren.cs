#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangXiuMouRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 36600)]
    internal bool MouZhangXiuAnniversaryMovement(ProgramSkillFrame f, SkillProgramEffect? op, ProgramMovementContinuation p) => f.MouZhangXiuAnniversary is not null && (op?.Op is SkillProgramEffectOp.ResolveMouZhangXiuFuxi or SkillProgramEffectOp.ResolveMouZhangXiuHaoyi) && ValidMouZhangXiuAnniversary(f) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouZhangXiuAnniversaryFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouZhangXiuAnniversary(f) || f.MouZhangXiuAnniversary is not { } r)
            return false;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouZhangXiuAnniversaryStage.Drawing && f.NativeDraw is not null;
        if (child is CardUseFrame use)
            return r.Stage == MouZhangXiuAnniversaryStage.NativeSlash && use.MouZhangXiuAnniversarySlash is { } o && o.FrameId == f.Id && use.Id == r.UseId && IsIssuedMouZhangXiuAnniversaryUse(use);
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && MouZhangXiuAnniversaryMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && (r.Stage == MouZhangXiuAnniversaryStage.SlashChildren || m.Sequence > r.Before && m.Sequence <= r.After));
        var lion = r.Cards.FirstOrDefault(c => c.From.Zone == CardZoneKind.Equipment && c.Kind == CardKind.SilverLion && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == c.Id && m.From == c.From && m.Sequence > r.Before && m.Sequence <= r.After));
        if (lion?.From.OwnerSeat is { } holder)
        {
            if (child is RecoveryReplacementFrame rr)
                return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(rr, f) && rr.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && rr.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && rr.Attempt.SourceSeat == holder && rr.Attempt.TargetSeat == holder;
            if (child is HpChangedTriggerWindowFrame hp)
                return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.TargetSeat == holder;
        }

        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouZhangXiuAnniversaryEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MouZhangXiuAnniversaryFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]);
    internal bool MouZhangXiuAnniversarySuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterZhangXiuMouOperations.MouZhangXiuAnniversaryEdge(i), (i, dying) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? MouZhangXiuAnniversaryRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterZhangXiuMouOperations.MouZhangXiuAnniversaryFirstChild(f, c), (n) => _characterZhangXiuMouOperations.MouZhangXiuAnniversarySuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 36600)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 36600)]
    internal bool MouZhangXiuAnniversaryDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouZhangXiuAnniversaryRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 36600)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 36600)]
    internal bool MouZhangXiuAnniversaryDamageWindow(long id) => MouZhangXiuAnniversaryRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 36600)]
    internal bool MouZhangXiuAnniversaryBeforeDamage(long id) => MouZhangXiuAnniversaryRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 36600)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 36600)]
    internal bool MouZhangXiuAnniversaryRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (MouZhangXiuAnniversaryFirstChild(f, recovery))
            return true;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n + 1 && MouZhangXiuAnniversaryFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && MouZhangXiuAnniversarySuffix(n) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 36600)]
    internal bool MouZhangXiuAnniversaryAdvanceChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouZhangXiuAnniversaryRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
