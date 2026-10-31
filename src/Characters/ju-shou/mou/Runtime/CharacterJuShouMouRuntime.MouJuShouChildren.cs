#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJuShouMouRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 36900)]
    internal bool MouJsMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.MouJuShou is { } r && effect?.Op == r.Operation && MouJuShou.Valid(f) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouJsFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!MouJuShou.Valid(f) || f.MouJuShou is not { } r)
            return false;
        if (child is CardUseFrame u)
            return MouJsNativeRidesOn(u, f);
        if (child is CardDeclarationFrame d)
            return MatchesMouJsDeclaration(f, d);
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && MouJsMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage is MouJuShouStage.DrawingThree or MouJuShouStage.DrawingOne;
        if (r.Stage == MouJuShouStage.HealthChildren)
        {
            if (child is HpChangedTriggerWindowFrame hp)
                return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Loss && hp.Change.SourceSeat is null && hp.Change.TargetSeat == r.Actor && hp.Change.Amount == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillHpLostEvent>().Any(e => e.FrameId == f.Id && e.TargetSeat == r.Actor && e.Amount == 1);
            if (child is DyingFrame dying)
                return dying.ParentFrameId == f.Id && dying.VictimSeat == r.Actor && dying.Continuation == DyingContinuationKind.ProgramSkill && dying.KillerSeat is null && _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.Any(p => p.ParentFrameId == f.Id && p.Kind == HpChangeKind.Loss && p.TargetSeat == r.Actor && p.Amount == 1);
        }

        if (r.Stage == MouJuShouStage.PenaltyChildren && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => r.Discard.Contains(m.CardId) && m.CardKind == CardKind.SilverLion && m.From == CardLocation.Equipment(r.Actor) && m.Sequence > r.Before && m.Sequence <= r.After))
        {
            if (child is RecoveryReplacementFrame rr)
                return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(rr, f) && rr.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && rr.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && rr.Attempt.SourceSeat == r.Actor && rr.Attempt.TargetSeat == r.Actor;
            if (child is HpChangedTriggerWindowFrame hp)
                return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.TargetSeat == r.Actor;
        }

        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouJsEdge(int i) => MouJsDeclarationEdge(i) || _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MouJsFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool MouJsSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterJuShouMouOperations.MouJsEdge(i), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? MouJsObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterJuShouMouOperations.MouJsFirstChild(f, c), (n) => _characterJuShouMouOperations.MouJsSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 36900)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 36900)]
    internal bool MouJsDamageWindow(long id) => MouJsObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 36900)]
    internal bool MouJsBeforeDamage(long id) => MouJsObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 36900)]
    internal bool MouJsDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouJsObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 36900)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 36900)]
    internal bool MouJsRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame rr)
    {
        if (MouJsFirstChild(f, rr))
            return true;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == rr.Id);
        return n >= 0 && c > n + 1 && MouJsFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && MouJsSuffix(n) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(rr, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 36900)]
    internal bool MouJsAdvanceChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouJsObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 36901)]
    internal void ObserveMouJsUse(IGameEvent payload)
    {
        if (payload is CardUseDeclaredEvent declared && _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is { Action: { Type: CardActionType.Use } action })
            MouJuShou.ObserveUse(action.ActorSeat, action.PhysicalCards.Select(c => c.CardId).ToArray());
    }
}
