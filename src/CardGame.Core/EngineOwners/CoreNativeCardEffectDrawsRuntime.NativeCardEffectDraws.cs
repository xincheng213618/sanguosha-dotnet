#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeCardEffectDrawsRuntime
{
    internal bool TryBeginNativeDrawTwo(NullificationWindowFrame pending)
    {
        if (!_coreProgramDrawTwoBonusOperations.HasNativeDrawTwoCapability)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame use || use.Id != pending.ParentFrameId || pending.ActionKind != LegalActionKind.DrawTwo || pending.EffectCardKind != CardKind.DrawTwo || use.NativeDraw is not null || use.NativeCardEffectDraw is not null || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            throw new InvalidOperationException("DrawTwo requires its actual resolved effect and owning CardUse.");
        if (pending.Id < 0 || pending.Id == 0 && (use.Action is null || !_coreUnnullifiableOrdinaryTricksOperations.IsOriginalOrdinaryTrickCannotNullify(use.Id) && !_characterLuMengMouOperations.IsYingboUnrespondable(use.Id) && !_coreIssuedPlayUsePoliciesOperations.IsIssuedCardUnrespondable(use.Id)))
            throw new InvalidOperationException("A windowless DrawTwo requires its exact already-issued native no-response policy.");
        var recipient = global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending) ?? pending.SourceSeat;
        var receipt = new NativeCardEffectDrawReceipt(pending.Id, pending.EffectCardId, pending.EffectCardKind, recipient, use.TargetIndex, NativeCardEffectDrawStage.Requesting)
        {
            RequestedCount = _coreProgramDrawTwoBonusOperations.NativeDrawTwoCount(pending.SourceSeat),
            BypassedActionId = pending.Id == 0 ? use.Action!.ActionId : null,
            BypassedEffectOrdinal = pending.Id == 0 ? 1 + _coreProgramEventHistoryOperations.ProgramEventHistory<NativeCardEffectDrawStartedEvent>().Count(fact => fact.UseFrameId == use.Id && fact.NullificationFrameId == 0) : null,
            AwaitingProgramFrameId = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault()is { } parent && global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime.IsAwaitingProgramMovement(parent) ? parent.Id : null
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(use with { NativeCardEffectDraw = receipt, NativeDraw = new NativeDrawProducerReceipt(0, receipt.RequestedCount, null, SkillProgramCardSetVisibility.Private, CardMoveReasons.Draw) { Recipients = [recipient], ReturnKind = NativeDrawAttemptReturnKind.CardUseDrawTwo } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NativeCardEffectDrawStartedEvent(use.Id, pending.Id, receipt.EffectCardId, receipt.EffectCardKind, recipient, use.TargetIndex, receipt.AwaitingProgramFrameId) { RequestedCount = receipt.RequestedCount, BypassedActionId = receipt.BypassedActionId, BypassedEffectOrdinal = receipt.BypassedEffectOrdinal });
        ContinueNativeDrawTwo(use.Id);
        return true;
    }

    [NativeFrameRoute(typeof(CardUseFrame), 30, typeof(NativeFrameRouteConditions), "NativeCardEffectDraw")]
    internal void ContinueNativeDrawTwo(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { NativeCardEffectDraw: { } receipt } use || use.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            throw new InvalidOperationException("A native card draw resumed without its exact quiet owning effect.");
        AssertNativeCardEffectDraw(use);
        if (receipt.Stage == NativeCardEffectDrawStage.Requesting)
        {
            var paid = use.NativeDraw!;
            if (paid.ActiveAttemptId is not null && !paid.AttemptReturned)
                throw new InvalidOperationException("A native card draw resumed before its issued attempt returned.");
            if (!paid.AttemptReturned && _coreNativeDrawAttemptsOperations.TryBeginNativeDrawAttempt(use))
                return;
            var before = _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence;
            var ids = paid.Replacement is null && _coreMatchParticipantsPlayersAccess._players[receipt.RecipientSeat].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[receipt.RecipientSeat], receipt.RequestedCount, true, CardMoveReasons.Draw) : Array.AsReadOnly(Array.Empty<int>());
            var outcome = new NativeDrawOutcome(receipt.RecipientSeat, receipt.RequestedCount, paid.ActiveAttemptId, before, _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence, paid.Replacement)
            {
                ActualDrawIds = ids
            };
            use = (CardUseFrame)_resolutionStackOperations._resolutionStack.Last();
            _coreRuntimeOperations.ReplaceRuntimeTop(use = use with { NativeDraw = null, NativeCardEffectDraw = receipt = receipt with { Stage = NativeCardEffectDrawStage.DrawingChildren, Outcome = outcome } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NativeCardEffectDrawAppliedEvent(id, receipt.NullificationFrameId, receipt.RecipientSeat, ids.Count, outcome.SequenceBefore, outcome.SequenceAfter) { BypassedEffectOrdinal = receipt.BypassedEffectOrdinal });
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.NativeCardEffectDraw) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.NativeCardEffectDraw) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.NativeCardEffectDraw) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return;
        use = (CardUseFrame)_resolutionStackOperations._resolutionStack.Last();
        AssertNativeCardEffectDraw(use);
        receipt = use.NativeCardEffectDraw!;
        // Retire this exact effect before FinishCardUse can repeat the card or
        // advance to its next target. Each genuine later Draw2 gets a new request.
        _coreRuntimeOperations.ReplaceRuntimeTop(use with { NativeCardEffectDraw = null, NativeDraw = null });
        var card = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(id, receipt.EffectCardId);
        _coreSequentialTrickTargetsOperations.MoveFinishedTrickCard(id, card);
        _coreMatchLogOperations.AddLog("CardEffect", $"{(_coreMatchParticipantsPlayersAccess._players[receipt.RecipientSeat].Name)} 的【无中生有】结算完毕。", receipt.RecipientSeat);
        _coreNativeCardUseCompletionOperations.FinishCardUse(id, card, receipt.EffectCardKind);
    }

    internal bool IsNativeCardEffectDrawMovement(CardUseFrame use, CardMovementBatchContext batch) => use.NativeCardEffectDraw is { Stage: NativeCardEffectDrawStage.DrawingChildren, Outcome: { } outcome } receipt && batch.ParentFrameId == use.Id && batch.AwaitingProgramFrameId == receipt.AwaitingProgramFrameId && batch.Movements.Count > 0 && batch.Movements.All(m => m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter && (m.From == CardLocation.DrawPile && m.Reason == CardMoveReasons.Draw && (outcome.ActualDrawIds.Contains(m.CardId) && m.To == CardLocation.Hand(outcome.RecipientSeat) || _coreHappyWeiYanGainDestinationsOperations.IsHappyWeiYanRedirectedGain(m, CardLocation.Hand(outcome.RecipientSeat))) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle));
    internal bool IsNativeCardEffectDrawMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame frame) => frame.ResumeNativeCardEffectDrawFrameId == use.Id && frame.Id == frame.Batch.Id && frame.ResumeProgramFrameId is null && !global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.HasAlternateProgramMovementReturn(frame with { ResumeNativeCardEffectDrawFrameId = null }) && IsNativeCardEffectDrawMovement(use, frame.Batch);
    internal bool IsNativeCardEffectDrawStateReturn(CardUseFrame use, ProgramLifecycleTriggerWindowFrame frame) => use.NativeCardEffectDraw is { Stage: NativeCardEffectDrawStage.DrawingChildren } && frame.ResumeProgramFrameId == use.Id && frame.CharacterStateContinuation == CharacterStateContinuation.NativeCardEffectDraw && frame.ResumeCardId is null && frame.ResumeCardKind is null && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Count(e => e.Change.Id == frame.Id && e.Change.ParentFrameId == use.Id) == 1;
    internal bool HasNativeCardEffectDrawProcessing(IReadOnlyList<Card> processing)
    {
        var uses = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().ToArray();
        var draws = uses.Where(use => use.NativeCardEffectDraw is not null).ToArray();
        if (draws.Length == 0)
            return false;
        foreach (var use in draws)
            AssertNativeCardEffectDraw(use);
        // A pre-draw optional offer can be open before it creates a ProgramSkillFrame.
        // The original card and any real ancestor use still own their Processing entities.
        var materials = uses.SelectMany(use => use.PhysicalCardIds ?? [use.CardId]).ToHashSet();
        return processing.All(card => materials.Contains(card.Id));
    }

    internal void AssertNativeCardEffectDraw(CardUseFrame use)
    {
        if (use.NativeCardEffectDraw is not { } receipt)
            return;
        if (!_coreProgramDrawTwoBonusOperations.HasNativeDrawTwoCapability || receipt.RequestedCount < 2 || receipt.NullificationFrameId < 0 || (receipt.NullificationFrameId == 0 ? use.Action?.ActionId != receipt.BypassedActionId || receipt.BypassedActionId is not > 0 || receipt.BypassedEffectOrdinal is not > 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<NativeCardEffectDrawStartedEvent>().Count(e => e.UseFrameId == use.Id && e.NullificationFrameId == 0) != receipt.BypassedEffectOrdinal : receipt.BypassedActionId is not null || receipt.BypassedEffectOrdinal is not null) || receipt.EffectCardKind != CardKind.DrawTwo || global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) != CardKind.DrawTwo || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(receipt.RecipientSeat) || receipt.TargetIndex != use.TargetIndex || !Enum.IsDefined(receipt.Stage) || _coreProgramEventHistoryOperations.ProgramEventHistory<NativeCardEffectDrawStartedEvent>().Count(e => e.UseFrameId == use.Id && e.NullificationFrameId == receipt.NullificationFrameId && e.EffectCardId == receipt.EffectCardId && e.EffectCardKind == receipt.EffectCardKind && e.RecipientSeat == receipt.RecipientSeat && e.RequestedCount == receipt.RequestedCount && e.TargetIndex == receipt.TargetIndex && e.AwaitingProgramFrameId == receipt.AwaitingProgramFrameId && e.BypassedActionId == receipt.BypassedActionId && e.BypassedEffectOrdinal == receipt.BypassedEffectOrdinal) != 1 || receipt.AwaitingProgramFrameId is { } programId && !_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(parent => parent.Id == programId && global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime.IsAwaitingProgramMovement(parent)))
            throw new InvalidOperationException("A native card draw lost its once-issued resolved effect or recipient.");
        if (receipt.Stage == NativeCardEffectDrawStage.Requesting)
        {
            if (receipt.Outcome is not null || use.NativeDraw is not { } paid || paid.ReturnKind != NativeDrawAttemptReturnKind.CardUseDrawTwo || paid.InstructionIndex != 0 || paid.Recipients is not [var seat] || seat != receipt.RecipientSeat || paid.RecipientIndex != 0 || paid.RequestedCount != receipt.RequestedCount || paid.ResultBind is not null || paid.Operation is not null || paid.RecipientApplied || paid.Reason != CardMoveReasons.Draw || paid.Replacement is not null && !paid.AttemptReturned || paid.ActiveAttemptId is null && paid.AttemptReturned)
                throw new InvalidOperationException("DrawTwo changed its exact pre-draw producer contract.");
            return;
        }

        if (use.NativeDraw is not null || receipt.Outcome is not { } result || result.RecipientSeat != receipt.RecipientSeat || result.RequestedCount != receipt.RequestedCount || result.SequenceBefore > result.SequenceAfter || result.SequenceAfter > _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence || result.ActualDrawIds.Count > receipt.RequestedCount || result.ActualDrawIds.Distinct().Count() != result.ActualDrawIds.Count || result.ActualDrawIds is not System.Collections.IList { IsReadOnly: true } || result.Replacement is not null && result.ActualDrawIds.Count != 0 || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > result.SequenceBefore && m.Sequence <= result.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(receipt.RecipientSeat) && m.Reason == CardMoveReasons.Draw).OrderBy(m => m.Sequence).Select(m => m.CardId).SequenceEqual(result.ActualDrawIds) || _coreProgramEventHistoryOperations.ProgramEventHistory<NativeCardEffectDrawAppliedEvent>().Count(e => e.UseFrameId == use.Id && e.NullificationFrameId == receipt.NullificationFrameId && e.RecipientSeat == receipt.RecipientSeat && e.ActualDrawCount == result.ActualDrawIds.Count && e.SequenceBefore == result.SequenceBefore && e.SequenceAfter == result.SequenceAfter && e.BypassedEffectOrdinal == receipt.BypassedEffectOrdinal) != 1 || result.AttemptFrameId is { } attempt && _coreProgramEventHistoryOperations.ProgramEventHistory<NativeDrawAttemptDecidedEvent>().Count(e => e.AttemptFrameId == attempt && e.ParentFrameId == use.Id && e.RecipientSeat == receipt.RecipientSeat && e.RequestedCount == receipt.RequestedCount && e.Replaced == (result.Replacement is not null)) != 1)
            throw new InvalidOperationException("DrawTwo lost its original draw materials or exact replacement return.");
    }
}
