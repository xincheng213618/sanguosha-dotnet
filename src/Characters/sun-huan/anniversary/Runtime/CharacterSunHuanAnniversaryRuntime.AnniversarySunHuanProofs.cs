#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunHuanAnniversaryRuntime
{
    internal bool ValidSunHuanDraw(AnniversarySunHuanDraw d) => d.Source.SkillId == AnniversarySunHuanComposition.Skill && d.Source.BindingId == "target-draw" && _coreTableSeatQueriesOperations.IsValidPlayerSeat(d.Source.OwnerSeat) && d.Outcome.RecipientSeat == d.Source.OwnerSeat && d.Outcome.RequestedCount == 1 && d.Outcome.ActualDrawIds.Count <= 1 && d.Outcome.SequenceBefore >= 0 && d.Outcome.SequenceAfter >= d.Outcome.SequenceBefore && d.Outcome.SequenceAfter <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversarySunHuanDrawReturnedEvent>().Count(e => e == new AnniversarySunHuanDrawReturnedEvent(d.FrameId, d.Source.OwnerSeat, d.Turn, d.Outcome.ActualDrawIds.Count, d.Outcome.SequenceBefore, d.Outcome.SequenceAfter, d.Outcome.AttemptFrameId)) == 1 && d.Outcome.ActualDrawIds.All(id => id > 0 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == id && m.Sequence > d.Outcome.SequenceBefore && m.Sequence <= d.Outcome.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(d.Source.OwnerSeat) && m.Reason == SunHuanDrawReason) == 1);
    internal bool ValidSunHuan(ProgramSkillFrame f) => f.AnniversarySunHuan is { } r && f.SkillId == AnniversarySunHuanComposition.Skill && f.InstructionIndex == 1 && r.Source == SunHuanSource(f) && r.Hash == f.GameplayHash && r.Turn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && r.TurnOwner == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && r.ParentId == f.WindowContext?.ParentFrameId && r.Operation == _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op && Enum.IsDefined(r.Stage) && r.Before >= 0 && r.After >= r.Before && r.After <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversarySunHuanStartedEvent>().Count(e => e == new AnniversarySunHuanStartedEvent(f.Id, r.Source, r.Hash, r.Turn, r.TurnOwner, r.ParentId, r.Operation)) == 1 && (r.Operation == SkillProgramEffectOp.ResolveAnniversarySunHuanNijiDraw ? f.TriggerId == "target-draw" && f.WindowContext is { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } c } && _coreCardUseLifecycleOperations.LifecycleCardUse(c.ParentCardUseFrameId)is { Action: { Type: CardActionType.Use } a } use && a.ActionId == c.CardActionId && use.TargetSeats.Contains(f.OwnerSeat) : f.TriggerId == "ending-use-discard" && f.WindowContext?.Window == SkillProgramTriggerWindow.TurnEnding && SunHuanDraws(f.OwnerSeat, f.SkillInstanceId).Any(d => d.Hash == r.Hash && ValidSunHuanDraw(d)));
    internal bool SunHuanMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p)
    {
        if (f.AnniversarySunHuan is not { } r || effect?.Op != r.Operation || p != f.PendingMovementContinuation || p.SubjectSeat != f.OwnerSeat || p.BeforeCount != 0 || p.CoverageResultBind is not null || !ValidSunHuan(f))
            return false;
        if (r.Stage == AnniversarySunHuanStage.DrawChildren)
            return r.Draw is { } d && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].AnniversaryNijiDraws.Any(x => x.FrameId == f.Id && x.Outcome == d && ValidSunHuanDraw(x));
        if (r.Stage == AnniversarySunHuanStage.DiscardChildren)
        {
            return true;
        }

        return r.Stage == AnniversarySunHuanStage.NativeTail && r.Returned && r.Native is { } n && _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => r.NativeBatches.Contains(b.Id)).All(b => b.ParentFrameId == n.UseId && b.AwaitingProgramFrameId is null && b.OriginOwnerSeat == f.OwnerSeat && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId && b.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m)) && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversarySunHuanNativeBatchEvent>().Count(e => e == new AnniversarySunHuanNativeBatchEvent(f.Id, n.UseId, b.Id)) == 1);
    }

    internal bool SunHuanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.AnniversarySunHuan is not { } r || !ValidSunHuan(f))
            return false;
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == AnniversarySunHuanStage.Drawing && draw.Context.ParentFrameId == f.Id && draw.Context.RecipientSeat == f.OwnerSeat && draw.Context.RequestedCount == 1 && global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(draw, f);
        if (child is CardUseFrame use)
            return r.Stage == AnniversarySunHuanStage.NativeUse && IsSunHuanNative(use);
        if (child is CardDeclarationFrame declaration)
            return MatchesSunHuanDeclaration(f, declaration);
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } p && SunHuanMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        if (child is RecoveryReplacementFrame recovery)
            return recovery.ParentFrameId == f.Id && recovery.Return.ResumeFrameId == f.Id && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program);
    }

    internal ProgramSkillFrame? SunHuanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.SunHuanFirstChild(f, child), (i) => this.SunHuanObserverSuffix(i));
    internal bool SunHuanObserverSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (index) => _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(index) || this.SunHuanDeclarationEdge(index), (index, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(index, dying) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(index, dying) || _coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(index, dying) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(index, dying));
    internal bool SunHuanDeclarationEdge(int i)
    {
        if (i < 1 || _resolutionStackOperations._resolutionStack[i - 1] is not CardDeclarationFrame d || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == d.Return.ParentFrameId)is not ProgramSkillFrame root || !MatchesSunHuanDeclaration(root, d))
            return false;
        if (_resolutionStackOperations._resolutionStack[i] is CardDeclarationChallengeFrame challenge)
            return d.Stage == CardDeclarationStage.Challenging && challenge.ParentFrameId == d.Id && challenge.SeatIndex >= 0 && challenge.SeatIndex < challenge.EligibleSeats.Count;
        if (_resolutionStackOperations._resolutionStack[i] is CardsMovedTriggerWindowFrame moved)
            return d.Stage is CardDeclarationStage.Paying or CardDeclarationStage.Cleaning && d.ActiveChildFrameId == moved.Id && moved.ResumeDeclarationFrameId == d.Id && moved.Batch.ParentFrameId == d.Id && moved.Batch.Movements is [var m] && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.CardId == d.Payment.Cost.CardId && (d.Stage == CardDeclarationStage.Paying ? m.From == d.Payment.Cost.From && m.To == CardLocation.Processing && m.Reason.Value == "conversion.declaration.pay" : m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && m.Reason.Value is "conversion.declaration.failed" or "conversion.declaration.cancelled");
        return _resolutionStackOperations._resolutionStack[i] is ProgramLifecycleTriggerWindowFrame skills && d.Stage == CardDeclarationStage.Granting && skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.ResumeProgramFrameId == d.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeCardDeclaration;
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 45400)]
    internal bool SunHuanDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && SunHuanObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 45400)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 45400)]
    internal bool RetainSunHuan(ProgramSkillFrame f) => f.AnniversarySunHuan is not null && ValidSunHuan(f);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 45400)]
    internal bool AdvanceSunHuanChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && SunHuanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 45400)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 45400)]
    internal bool SunHuanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.AnniversarySunHuan is null)
            return false;
        var p = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return p >= 0 && c > p && SunHuanFirstChild(f, _resolutionStackOperations._resolutionStack[p + 1]) && (c == p + 1 || SunHuanObserverSuffix(p)) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }
}
