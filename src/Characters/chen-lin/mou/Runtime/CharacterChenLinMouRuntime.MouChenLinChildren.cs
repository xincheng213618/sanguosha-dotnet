#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChenLinMouRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 37000)]
    internal bool MouChenLinMovement(ProgramSkillFrame f, SkillProgramEffect? op, ProgramMovementContinuation p) => (f.MouChenLin is not null && ValidMouChenLin(f) && op?.Op is SkillProgramEffectOp.ResolveMouChenLinZhuanwen or SkillProgramEffectOp.ResolveMouChenLinYaozuo || f.MouChenLinInvitation is { Stage: MouChenLinInvitationStage.MovementChildren } && MouChenLinRules.ValidInvitation(f) && op?.Op == SkillProgramEffectOp.ResolveMouChenLinYaozuo) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouChenLinFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouChenLin is null && f.MouChenLinInvitation is { Stage: MouChenLinInvitationStage.MovementChildren } invitation && MouChenLinRules.ValidInvitation(f))
        {
            if (child is CardsMovedTriggerWindowFrame gift && f.PendingMovementContinuation is { } paid && MouChenLinMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), paid))
                return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, gift) && gift.Batch.ParentFrameId == f.Id && gift.Batch.OriginOwnerSeat == f.OwnerSeat && gift.Batch.OriginSkillId == f.SkillId && gift.Batch.OriginSkillInstanceId == f.SkillInstanceId && gift.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.CardId == invitation.PendingCard?.Id && m.Sequence > invitation.Before && m.Sequence <= invitation.After);
            if (invitation.PendingCard is { Kind: CardKind.SilverLion, From.Zone: CardZoneKind.Equipment } giftLion && giftLion.From.OwnerSeat is { } giftHolder && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == giftLion.Id && m.Sequence > invitation.Before && m.Sequence <= invitation.After))
            {
                if (child is RecoveryReplacementFrame rr)
                    return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(rr, f) && rr.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && rr.Attempt.TargetSeat == giftHolder && rr.Attempt.SourceSeat == giftHolder;
                if (child is HpChangedTriggerWindowFrame hp)
                    return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.TargetSeat == giftHolder;
            }

            return child is ProgramLifecycleTriggerWindowFrame giftState && giftState.ResumeProgramFrameId == f.Id && giftState.Window == SkillProgramTriggerWindow.SkillsChanged && giftState.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        }

        if (!ValidMouChenLin(f) || f.MouChenLin is not { } r)
            return false;
        if (child is CardUseFrame use)
            return MouChenLinUseRidesOn(f, use);
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && MouChenLinMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && (r.Stage == MouChenLinStage.UseChildren || m.Sequence > r.Before && m.Sequence <= r.After));
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

    internal bool MouChenLinEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MouChenLinFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]);
    internal bool MouChenLinSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterChenLinMouOperations.MouChenLinEdge(i), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? MouChenLinRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterChenLinMouOperations.MouChenLinFirstChild(f, c), (n) => _characterChenLinMouOperations.MouChenLinSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 37000)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 37000)]
    internal bool MouChenLinDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouChenLinRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 37000)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 37000)]
    internal bool MouChenLinDamageWindow(long id) => MouChenLinRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 37000)]
    internal bool MouChenLinBeforeDamage(long id) => MouChenLinRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 37000)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 37000)]
    internal bool MouChenLinRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (MouChenLinFirstChild(f, recovery))
            return true;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n + 1 && MouChenLinFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && MouChenLinSuffix(n) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 37000)]
    internal bool MouChenLinAdvanceChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouChenLinRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
