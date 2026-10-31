#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangWenAnniversaryRuntime
{
    internal const string ZhangWenChoice = "anniversary-zhang-wen", ZhangWenReveal = "sibian-four", ZhangWenExtrema = "sibian-extrema";
    internal static CardMoveReason ZhangWenGain => new("skill-program.anniversary:sibian.extrema");
    internal static CardMoveReason ZhangWenGift => new("skill-program.anniversary:sibian.gift");
    internal static CardMoveReason ZhangWenCleanup => new("skill-program.anniversary:sibian.cleanup");

    internal static CardConversionSource ZhangWenSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    void global::CardGame.Core.IAnniversaryZhangWenHost.Refund(ProgramSkillFrame frame) => RefundZhangWenSongshu(frame);
    SkillProgramStepOutcome global::CardGame.Core.IAnniversaryZhangWenHost.Sibian(ProgramSkillFrame frame) => BeginZhangWenSibian(frame);
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 45100)]
    internal void CaptureZhangWenSongshu(ProgramSkillStartedEvent started)
    {
        if (started.SkillId != AnniversaryZhangWenComposition.Songshu || started.ActivationId != AnniversaryZhangWenComposition.Contest)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != started.FrameId || f.OwnerSeat != started.OwnerSeat || f.TriggerId is not null || f.InstructionIndex != 0 || f.ZhangWenSongshu is not null || f.SelectedCardIds.Count != 1 || f.SelectedTargetSeats.Count != 1 || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat)
            throw new InvalidOperationException("Songshu must freeze its own accepted Play debit before Pindian starts.");
        var activation = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Activation!;
        var key = (f.OwnerSeat, f.SkillId, activation.UsageGroup);
        var r = new ZhangWenSongshuReceipt(ZhangWenSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, activation.UsageGroup, _coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programUses.GetValueOrDefault(key), _coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programPhaseUses.GetValueOrDefault(key));
        if (r.TurnUses < 1 || r.PhaseUses != 1)
            throw new InvalidOperationException("Songshu requires its actually consumed one-per-Play use.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ZhangWenSongshu = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangWenSongshuAcceptedEvent(f.Id, r.Source, r.Hash, r.Turn, r.TurnOwner, r.PlayPhase, r.UsageGroup, r.TurnUses, r.PhaseUses));
    }

    internal void RefundZhangWenSongshu(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ZhangWenSongshu is not { Refunded: false } r || r.Source != ZhangWenSource(f) || r.Hash != f.GameplayHash || f.InstructionIndex != 2 || f.SkillId != AnniversaryZhangWenComposition.Songshu || f.ActivationId != AnniversaryZhangWenComposition.Contest || f.TriggerId is not null || r.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || r.TurnOwner != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || r.PlayPhase != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || f.PindianResultBindings is not [{ Name: "__active-pindian-result", SourceWon: true } result] || result.SourceSeat != f.OwnerSeat || f.SelectedTargetSeats is not [var opponent] || result.OpponentSeat != opponent || _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangWenSongshuAcceptedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.Hash == r.Hash && e.PlayPhase == r.PlayPhase) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangWenSongshuRefundedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Songshu cannot refund another frame, phase or a non-winning Pindian.");
        var key = (f.OwnerSeat, f.SkillId, r.UsageGroup);
        var turnBefore = _coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programUses.GetValueOrDefault(key);
        var phaseBefore = _coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programPhaseUses.GetValueOrDefault(key);
        if (turnBefore != r.TurnUses || phaseBefore != r.PhaseUses)
            throw new InvalidOperationException("Songshu's original debit changed before its winning refund.");
        _coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programUses[key] = turnBefore - 1;
        _coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programPhaseUses[key] = phaseBefore - 1;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ZhangWenSongshu = r with { Refunded = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangWenSongshuRefundedEvent(f.Id, r.Turn, r.PlayPhase, turnBefore, turnBefore - 1, phaseBefore, phaseBefore - 1));
    }

    internal SkillProgramStepOutcome BeginZhangWenSibian(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ZhangWenSibian is not null || f.SkillId != AnniversaryZhangWenComposition.Sibian || f.TriggerId != AnniversaryZhangWenComposition.Replacement || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { Window: SkillProgramTriggerWindow.DrawPhaseStarting } context)
            throw new InvalidOperationException("Sibian requires its accepted original normal-draw replacement.");
        var r = new ZhangWenSibianReceipt(ZhangWenSource(f), f.GameplayHash, context.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, ZhangWenSibianStage.RevealChildren);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangWenSibian = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangWenSibianStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.Turn));
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreProgramLifecycleOperations.RevealProgramTopCards(f.Id, f.OwnerSeat, 4, null, ZhangWenReveal, SkillProgramCardSetVisibility.Public);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var entities = _coreProgramLifecycleOperations.GetProgramCardSet(f, ZhangWenReveal).CardIds.Select(id => new ZhangWenSibianEntity(id, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Rank, _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == id).Sequence)).ToArray();
        var extrema = entities.Length == 0 ? [] : entities.Where(e => e.Rank == entities.Min(c => c.Rank) || e.Rank == entities.Max(c => c.Rank)).Select(e => e.CardId).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ZhangWenSibian = r with { Entities = entities, Extrema = extrema } });
        foreach (var(entity, index)in entities.Select((e, i) => (e, i)))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangWenSibianEntityEvent(f.Id, index, entity, extrema.Contains(entity.CardId)));
        AddZhangWenSibianStep(f.Id, ZhangWenSibianStage.RevealChildren, before);
        ContinueZhangWenSibian(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool ZhangWenEntityAvailable(ZhangWenSibianEntity entity) => _cardZonesOperations._cardZones.GetLocation(entity.CardId) == CardLocation.Processing && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == entity.CardId).Sequence == entity.RevealSequence;
    internal int[] ZhangWenRemainder(ProgramSkillFrame f) => f.ZhangWenSibian!.Entities.Where(e => !f.ZhangWenSibian.Extrema.Contains(e.CardId) && ZhangWenEntityAvailable(e)).Select(e => e.CardId).ToArray();
    internal IReadOnlyList<PromptChoice> ZhangWenSibianChoices(ProgramSkillFrame f)
    {
        if (f.ZhangWenSibian?.Stage != ZhangWenSibianStage.Recipient || ZhangWenRemainder(f).Length == 0)
            return[];
        var living = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).ToArray();
        var minimum = living.Min(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count);
        var choices = living.Where(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count == minimum).Select(p => new PromptChoice(new($"zhang-wen.{(f.Id)}.give.{(p.Seat)}"), "将剩余的牌全部交给" + p.Name, [], [p.Seat], new Dictionary<string, string> { ["program-action"] = ZhangWenChoice, ["mode"] = "give" })).ToList();
        choices.Add(new(new($"zhang-wen.{(f.Id)}.skip"), "不交给任何角色，将余牌置入弃牌堆", [], [], new Dictionary<string, string> { ["program-action"] = ZhangWenChoice, ["mode"] = "skip" }));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishZhangWenSibian(ProgramSkillFrame f)
    {
        RequireZhangWenSibian(f);
        var choices = ZhangWenSibianChoices(f);
        if (choices.Count == 0)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.OwnerSeat, choices, "思辩：可将全部余牌交给当前手牌最少的一名角色。", f.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(ZhangWenChoice)]
    internal void AnswerZhangWenSibian(PromptChoice answer)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZhangWenSibian: { Stage: ZhangWenSibianStage.Recipient } } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([answer], ZhangWenSibianChoices(f).Where(c => c.Id == answer.Id).ToArray()))
            throw new InvalidOperationException("Sibian requires the original owner's current whole-remainder choice.");
        RequireZhangWenSibian(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var target = answer.Parameters["mode"] == "give" ? answer.Targets.Single() : -1;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, ZhangWenSibian = f.ZhangWenSibian!with { Recipient = target } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangWenSibianRecipientEvent(f.Id, target));
        MoveZhangWenSibian(f, ZhangWenSibianStage.FinishChildren, ZhangWenRemainder(f), target < 0 ? CardLocation.DiscardPile : CardLocation.Hand(target), target < 0 ? ZhangWenCleanup : ZhangWenGift);
        ContinueZhangWenSibian(f.Id);
    }

    internal void AddZhangWenSibianStep(long id, ZhangWenSibianStage stage, int before)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var step = new ZhangWenSibianStep(stage, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ZhangWenSibian = f.ZhangWenSibian!with { Steps = f.ZhangWenSibian.Steps.Append(step).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangWenSibianStepEvent(id, step));
    }

    internal void MoveZhangWenSibian(ProgramSkillFrame f, ZhangWenSibianStage stage, IReadOnlyList<int> ids, CardLocation destination, CardMoveReason reason)
    {
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangWenSibian = f.ZhangWenSibian!with { Stage = stage }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (ids.Count > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, destination, reason, stage == ZhangWenSibianStage.FinishChildren && destination.OwnerSeat is { } recipient ? _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, recipient, ids, before) : null, disclosure: CardMovementDisclosure.FaceUp);
        AddZhangWenSibianStep(f.Id, stage, before);
    }

    internal bool DrainZhangWenSibian(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 45100)]
    internal bool ContinueZhangWenSibian(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZhangWenSibian: not null } own || own.Id != id)
            return false;
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { ZhangWenSibian: { } r } f && f.Id == id)
        {
            RequireZhangWenSibian(f);
            if (r.Stage != ZhangWenSibianStage.Recipient)
            {
                if (DrainZhangWenSibian(id))
                    return true;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                r = f.ZhangWenSibian!;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
                if (r.Stage == ZhangWenSibianStage.FinishChildren)
                {
                    _coreSkillProgramsOperations.FinishProgramSkill(f, true);
                    return true;
                }
            }

            if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                var cleanup = r.Entities.Where(ZhangWenEntityAvailable).Select(e => e.CardId).ToArray();
                MoveZhangWenSibian(f, ZhangWenSibianStage.FinishChildren, cleanup, CardLocation.DiscardPile, ZhangWenCleanup);
                continue;
            }

            if (r.Stage == ZhangWenSibianStage.RevealChildren)
            {
                var ids = r.Entities.Where(e => r.Extrema.Contains(e.CardId) && ZhangWenEntityAvailable(e)).Select(e => e.CardId).ToArray();
                _coreProgramLifecycleOperations.SetProgramCardSet(f.Id, ZhangWenExtrema, ids, SkillProgramCardSetVisibility.Public);
                MoveZhangWenSibian(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), ZhangWenSibianStage.GainChildren, ids, CardLocation.Hand(f.OwnerSeat), ZhangWenGain);
                continue;
            }

            if (r.Stage == ZhangWenSibianStage.GainChildren)
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangWenSibian = r with { Stage = ZhangWenSibianStage.Recipient } });
            PublishZhangWenSibian(f);
            return true;
        }

        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 45100)]
    internal bool ReturnZhangWenSibianMovement(ProgramSkillFrame f) => f.ZhangWenSibian is not null && ContinueZhangWenSibian(f.Id);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryZhangWenSibian)]
    internal PromptChoice ChooseZhangWenSibian(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Targets.Contains(f.OwnerSeat)) ?? p.Choices[0];
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 45100)]
    internal bool KeepZhangWenSibianPayment(ProgramSkillFrame f) => f.ZhangWenSibian is not null && ValidZhangWenSibian(f);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 45100)]
    internal bool KeepZhangWenAcceptedAfterSkillLoss(ProgramSkillFrame f)
    {
        if (f.ZhangWenSibian is not null)
            return ValidZhangWenSibian(f);
        if (f.ZhangWenSongshu is not { } r || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || f.SkillId != AnniversaryZhangWenComposition.Songshu || f.TriggerId is not null || f.ActivationId != AnniversaryZhangWenComposition.Contest || f.InstructionIndex is < 1 or > 4 || r.Source != ZhangWenSource(f) || r.Hash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.Hash || f.SelectedCardIds is not [var selectedCard] || f.SelectedTargetSeats is not [var opponent] || f.PindianResultBindings is not [{ Name: "__active-pindian-result" } result] || result.SourceSeat != f.OwnerSeat || result.OpponentSeat != opponent || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.ActivationId == f.ActivationId && e.OwnerSeat == f.OwnerSeat) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangWenSongshuAcceptedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var accepted] || accepted != new ZhangWenSongshuAcceptedEvent(f.Id, r.Source, r.Hash, r.Turn, r.TurnOwner, r.PlayPhase, r.UsageGroup, r.TurnUses, r.PhaseUses))
            return false;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().SkipWhile(e => e is not ZhangWenSongshuAcceptedEvent a || a.FrameId != f.Id).OfType<PindianResultDeterminedEvent>().Count(e => e.SkillId == f.SkillId && e.Result.SourceSeat == f.OwnerSeat && e.Result.OpponentSeat == opponent && e.Result.SourceCardId == selectedCard && e.Result.SourceRank == result.SourceRank && e.Result.OpponentRank == result.OpponentRank && e.Result.SourceWon == result.SourceWon) == 1;
    }
}
