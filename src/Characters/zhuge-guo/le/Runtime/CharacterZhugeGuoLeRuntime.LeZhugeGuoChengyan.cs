#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeGuoLeRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 30200, SkillProgramEffectOp.ResolveLeZhugeGuoChengyan)]
    internal bool CanOfferLeChengyan(ProgramTriggerLegalityContext gate) => gate.Context.CardUse is { } a && a.ActorSeat == gate.Candidate.OwnerSeat && (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) || global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(a.EffectiveKind)) && a.DesignatedTargetSeats?.Any(s => s != a.ActorSeat) == true && !_coreProgramEventHistoryOperations.ProgramEventHistory<LeChengyanCompletedEvent>().Any(e => e.UseId == a.ParentCardUseFrameId);
    internal SkillProgramStepOutcome StartLeChengyan(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != LeChengyan || f.ActivationId != f.TriggerId || f.TriggerId != "replace-use-effect" || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } a } c || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == c.ParentFrameId)is not ProgramCardTriggerWindowFrame w || _coreCardUseLifecycleOperations.LifecycleCardUse(w.ParentFrameId)is not { Action.Type: CardActionType.Use } use || use.Action.ActionId != a.CardActionId || a.ActorSeat != f.OwnerSeat || !(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) || global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(a.EffectiveKind)) || a.DesignatedTargetSeats?.Any(s => s != f.OwnerSeat) != true || f.LeChengyan is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<LeChengyanCompletedEvent>().Any(e => e.UseId == use.Id))
            throw new InvalidOperationException("Chengyan requires its one real targeted Use and native finalized window.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LeChengyan = new(LeZhugeGuoSource(f), f.GameplayHash, use.Id, a.CardActionId) });
        return _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, new(LeChengyanDrawReason));
    }

    internal bool ValidLeChengyan(ProgramSkillFrame f) => f.LeChengyan is { } r && r.Source == LeZhugeGuoSource(f) && r.GameplayHash == f.GameplayHash && f.InstructionIndex == 1 && Enum.IsDefined(r.Stage) && f.WindowContext?.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == f.WindowContext.ParentFrameId)is ProgramCardTriggerWindowFrame w && w.ParentFrameId == r.UseId && _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)?.Action is { Type: CardActionType.Use } a && a.ActionId == r.ActionId && a.ActorSeat == f.OwnerSeat && r.Before >= 0 && r.Before <= r.After && r.After <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveLeZhugeGuoChengyan)]
    internal void ReturnLeChengyanDraw(ProgramSkillFrame f, NativeDrawOutcome d)
    {
        if (!ValidLeChengyan(f) || f.LeChengyan!.Stage is not (LeChengyanStage.FirstDraw or LeChengyanStage.SecondDraw) || d.RecipientSeat != f.OwnerSeat || d.RequestedCount != 1 || d.ActualDrawIds.Count > 1)
            throw new InvalidOperationException("Chengyan requires its exact first or second native Draw1 return.");
        var r = f.LeChengyan;
        var second = r.Stage == LeChengyanStage.SecondDraw;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LeChengyan = r with { Stage = second ? LeChengyanStage.SecondDrawChildren : LeChengyanStage.RevealChildren, Draw = d, Before = d.SequenceBefore, After = d.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LeChengyanDrawReturnedEvent(f.Id, second, d.ActualDrawIds.Count, d.ReplacementGainedIds.Count, d.AttemptFrameId));
        if (second)
            foreach (var id in d.ActualDrawIds)
            {
                var m = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > d.SequenceBefore && m.Sequence <= d.SequenceAfter && m.CardId == id && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat));
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LeXidiEntityMarkedEvent(f.Id, LeZhugeGuoSource(f), id, m.Sequence, false));
            }

        ContinueLeChengyan(f.Id);
    }

    internal void ContinueLeChengyan(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LeChengyan: { } r } f || f.Id != id)
            return;
        if (!ValidLeChengyan(f))
            throw new InvalidOperationException("Chengyan lost its original Use, source or child return.");
        if (r.Stage is LeChengyanStage.FirstDraw or LeChengyanStage.SecondDraw)
            return;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.LeChengyan!;
        if (r.Stage == LeChengyanStage.RevealChildren && r.RevealedCardId is null)
        {
            var card = r.Draw!.ActualDrawIds is [var cardId] && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Hand(f.OwnerSeat) ? _coreAdvancedLifecycleOperations.GetAdvancedCard(cardId) : null;
            if (card is not null)
            {
                var kind = _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card);
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LeChengyan = r = r with { RevealedCardId = card.Id, RevealedKind = kind } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(f.Id, f.SkillId, f.TriggerId!, f.OwnerSeat, "chengyan-shown", [_coreSnapshotValueProjectionOperations.ToSnapshot(card)]));
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LeChengyanRevealedEvent(f.Id, r.UseId, r.ActionId, card.Id, kind));
                if (_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow(f.Id, "chengyan-drawn-hand"))
                    return;
                ContinueChengyanAfterPublicReveal(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            }
        }

        FinishLeChengyan(f, false);
    }

    internal void FinishLeChengyan(ProgramSkillFrame f, bool replaced)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LeChengyanCompletedEvent(f.Id, f.LeChengyan!.UseId, replaced));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LeChengyan = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [PublicHandRevealReturn("chengyan-drawn-hand")]
    internal void ContinueChengyanAfterPublicReveal(ProgramSkillFrame f)
    {
        var r = f.LeChengyan ?? throw new InvalidOperationException("Chengyan lost its native draw/reveal receipt.");
        if (r.RevealedCardId is null || r.RevealedKind is not { } kind)
            throw new InvalidOperationException("Chengyan lost its frozen shown identity.");
        if (kind == CardKind.Nullification)
        {
            FinishLeChengyan(f, false);
            return;
        }

        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind))
        {
            var use = _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId);
            if (use is null)
            {
                FinishLeChengyan(f, false);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { LeChengyanEffect = new(f.Id, r.Source, f.GameplayHash, r.ActionId, r.RevealedCardId.Value, kind) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LeChengyanEffectIssuedEvent(f.Id, r.UseId, r.ActionId, r.Source, r.RevealedCardId.Value, kind));
            FinishLeChengyan(f, true);
            return;
        }

        if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, LeChengyan = r with { Stage = LeChengyanStage.SecondDraw, Draw = null } });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, new(LeChengyanDrawReason));
            return;
        }

        FinishLeChengyan(f, false);
    }
}
