#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBaiLingYunAnniversaryRuntime
{
    internal bool ValidAnniversaryBly(ProgramSkillFrame f)
    {
        if (f.AnniversaryBly is not { } r || f.InstructionIndex != 1 || f.SkillId != AnniversaryBaiLingYunComposition.Linghui || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } w || r.Source != AnniversaryBlySource(f) || r.Hash != f.GameplayHash || r.Turn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.ParentId != w.ParentFrameId || !Enum.IsDefined(r.Stage) || r.Viewed.Count > 3 || r.Viewed.Distinct().Count() != r.Viewed.Count || r.ViewBefore < 0 || r.ViewAfter < r.ViewBefore || r.ViewAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryBlyStartedEvent>().Count(e => e == new AnniversaryBlyStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.Turn, r.Viewed.Count)) != 1)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not TurnEndingBoundaryFrame p || p.Id != r.ParentId || p.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || p.ItemIndex < 0 || p.ItemIndex >= p.Items.Count || p.Items[p.ItemIndex].Candidate is not { } c || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c))
            return false;
        var viewed = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.ViewBefore && m.Sequence <= r.ViewAfter && m.Reason == AnniversaryBlyReason("view")).ToArray();
        return viewed.Select(m => m.CardId).SequenceEqual(r.Viewed) && viewed.All(m => m.From == CardLocation.DrawPile && m.To == AnniversaryBlyLease(f));
    }

    internal bool AnniversaryBlyUsePaid(ProgramSkillFrame f) => f.AnniversaryBly is { Plan: { } p } r && r.Viewed.Contains(p.CardId) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == p.CardId && m.Sequence > r.ViewAfter && m.From == AnniversaryBlyLease(f) && m.To == CardLocation.Processing && m.Reason == AnniversaryBlyReason("use")) == 1;
    internal bool AnniversaryBlyMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (f.AnniversaryBly is not { } r || !ValidAnniversaryBly(f) || effect?.Op != SkillProgramEffectOp.ResolveAnniversaryBaiLingYunLinghui || pending.SubjectSeat != f.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null)
            return false;
        var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= r.After).ToArray();
        var lease = AnniversaryBlyLease(f);
        return r.Stage switch
        {
            AnniversaryBlyStage.Viewing => moves.Select(m => m.CardId).SequenceEqual(r.Viewed) && moves.All(m => m.From == CardLocation.DrawPile && m.To == lease && m.Reason == AnniversaryBlyReason("view")),
            AnniversaryBlyStage.UsePayment => moves is [var one] && r.Plan?.CardId == one.CardId && one.From == lease && one.To == CardLocation.Processing && one.Reason == AnniversaryBlyReason("use"),
            AnniversaryBlyStage.GainChildren => _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryBlyGainCompletedEvent>().Count(e => e == new AnniversaryBlyGainCompletedEvent(f.Id, r.GainId is null ? 0 : 1, r.Before, r.After)) == 1 && moves.All(m => r.Viewed.Contains(m.CardId) && (m.From == lease && (m.Reason == AnniversaryBlyReason("gain") && m.CardId == r.GainId || m.To == CardLocation.DrawPile && m.Reason == AnniversaryBlyReason("return")) || r.UseId == 0 && r.Plan?.CardId == m.CardId && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && m.Reason == AnniversaryBlyReason("cancelled"))),
            AnniversaryBlyStage.NativeTail => r.Returned && _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => r.NativeBatches.Contains(b.Id)).All(b => b.ParentFrameId == r.UseId && b.AwaitingProgramFrameId is null && b.OriginOwnerSeat == f.OwnerSeat && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId && b.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m)) && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryBlyNativeBatchEvent>().Count(e => e == new AnniversaryBlyNativeBatchEvent(f.Id, r.UseId, b.Id, b.Movements.Count)) == 1),
            _ => false
        };
    }
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.ProcessingAllowance, 43600)]
    internal bool AnniversaryBlyProcessingAllowance(IReadOnlyList<Card> cards) => cards is [var card] && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.AnniversaryBly is not null)is { AnniversaryBly.Plan.CardId: var id } root && id == card.Id && ValidAnniversaryBly(root) && AnniversaryBlyUsePaid(root);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 43600)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 43600)]
    internal bool RetainAnniversaryBly(ProgramSkillFrame f) => f.AnniversaryBly is not null && ValidAnniversaryBly(f);
    internal bool AnniversaryBlyFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.AnniversaryBly is not { } r || !ValidAnniversaryBly(f))
            return false;
        if (child is CardUseFrame use)
            return r.Stage == AnniversaryBlyStage.NativeUse && IsAnniversaryBlyNative(use);
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } p && AnniversaryBlyMovementProof(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is RecoveryReplacementFrame recovery)
            return recovery.ParentFrameId == f.Id && recovery.Return.ResumeFrameId == f.Id && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? AnniversaryBlyObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.AnniversaryBlyFirstChild(f, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 43600)]
    internal bool AnniversaryBlyDyingObserver() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && AnniversaryBlyObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 43600)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 43600)]
    internal bool AnniversaryBlyRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.AnniversaryBly is null)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent && AnniversaryBlyFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && (child == parent + 1 || Enumerable.Range(parent + 2, child - parent - 1).All(i => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i))) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 43600)]
    internal bool AdvanceAnniversaryBlyChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && AnniversaryBlyObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
