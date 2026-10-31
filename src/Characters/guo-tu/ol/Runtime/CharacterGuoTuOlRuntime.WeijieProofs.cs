#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoTuOlRuntime
{
    internal bool WeijieNativeNeedCurrent(WeijieReceipt r)
    {
        var original = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == r.Need.RequestFrameId);
        return original is not null && CaptureWeijieNeed(r.OriginalDecision, original)is { } n && SameWeijieNeed(n, r.Need);
    }

    internal bool ValidWeijiePayment(WeijiePaidOrigin p)
    {
        if (p.AttemptId <= p.Need.RequestFrameId || p.Source.SkillId != "ol:weijie" || p.Source.BindingId != "native-basic-request" || p.Source.OwnerSeat != p.Need.ProviderSeat || string.IsNullOrWhiteSpace(p.Source.SkillInstanceId) || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(p.Source.SkillId)?.Program is not { } program || program.GameplayHash != p.GameplayHash || p.ActualTurn <= 0 || p.ActualTurnOwner == p.Source.OwnerSeat || p.From != CardLocation.Hand(p.VictimSeat) || p.PaymentSequence <= 0 || p.PaymentBatchId <= 0)
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<WeijieCostPaidEvent>().Count(e => e == new WeijieCostPaidEvent(p.AttemptId, p.Source, p.GameplayHash, p.ActualTurn, p.ActualTurnOwner, p.VictimSeat, p.CardId, p.PrintedKind, p.EffectiveName, p.From, p.PaymentSequence, p.PaymentBatchId)) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<WeijieCostPaidEvent>().Count(e => e.Source.OwnerSeat == p.Source.OwnerSeat && e.ActualTurn == p.ActualTurn && e.ActualTurnOwner == p.ActualTurnOwner) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == p.PaymentSequence && m.CardId == p.CardId && m.From == p.From && m.To == CardLocation.DiscardPile && m.Reason.Value == WeijieDiscardReason) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<WeijieDeclaredEvent>().Count(e => e.FrameId == p.AttemptId && e.OutputKind == p.OutputKind && e.TargetSeat == p.TargetSeat) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<WeijieVictimSelectedEvent>().Count(e => e.FrameId == p.AttemptId && e.VictimSeat == p.VictimSeat) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<WeijieStartedEvent>().Count(e => e.FrameId == p.AttemptId && e.Source == p.Source && e.GameplayHash == p.GameplayHash && e.ActualTurn == p.ActualTurn && e.ActualTurnOwner == p.ActualTurnOwner && e.Intent == p.Need.Intent && e.OwnerFrameId == p.Need.OwnerFrameId && e.RequestFrameId == p.Need.RequestFrameId) == 1;
    }

    internal bool WeijieParent(ProgramSkillFrame f)
    {
        if (f.Weijie is not { } r)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != r.Need.RequestFrameId)
            return false;
        var native = _resolutionStackOperations._resolutionStack[index - 1];
        return native is ResponseWindowFrame response ? response.ParentFrameId == r.Need.OwnerFrameId : native.Id == r.Need.OwnerFrameId && native is CardUseFrame or ProgramSkillFrame or DyingFrame;
    }
    internal bool WeijieFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Weijie is not { Stage: WeijieStage.CostChildren, Paid: { } p } r)
            return false;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } changed)
            return changed.ResumeProgramFrameId == f.Id && changed.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == f.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id);
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Id == p.PaymentBatchId && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.Movements is [var m] && m.Sequence == p.PaymentSequence && m.CardId == p.CardId && m.From == p.From && m.To == CardLocation.DiscardPile && m.Reason.Value == WeijieDiscardReason;
    }

    internal ProgramSkillFrame? WeijieObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.WeijieFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27462)]
    internal bool AdvanceWeijieChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WeijieObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    internal ProgramSkillFrame? ActiveWeijieDraft() => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.Weijie is not null);
}
