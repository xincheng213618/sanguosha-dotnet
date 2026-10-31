#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYanRouOlRuntime
{
    internal const string YanRouDiscardReason = "skill-program.yan-rou.choutao.discard", YanRouDrawReason = "skill-program.yan-rou.xiangshu.draw";
    internal CardConversionSource YanRouSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IYanRouHost.Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginYanRou(f, op);
    internal bool YanRouSpent(int owner, string skill, string hash) => _coreProgramEventHistoryOperations.ProgramEventHistory<YanRouXiangshuSpentEvent>().Any(e => e.Source.OwnerSeat == owner && e.Source.SkillId == skill && e.GameplayHash == hash);
    internal int YanRouTurnDamage(int owner) => Math.Min(5, _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent).OfType<DamageAppliedEvent>().Where(e => !e.SourceLess && e.SourceSeat == owner && e.Amount > 0).Sum(e => e.Amount));
    internal bool YanRouParent(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c)
    {
        if (c.OwnerSeat != candidate.OwnerSeat)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == c.ParentFrameId);
        if (parent is ProgramCardTriggerWindowFrame w)
            return c.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.GetCardActionWindow(w) == c.Window && w.Action.Type == CardActionType.Use && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(w.Action.EffectiveKind) && c.CardUse is { } card && card.CardActionId == w.Action.ActionId && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(w.Candidates[w.CandidateIndex]) == candidate && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(use => use.Id == card.ParentCardUseFrameId && use.Action?.ActionId == card.CardActionId && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind));
        return parent is TurnEndingBoundaryFrame end && c.Window == SkillProgramTriggerWindow.TurnEnding && end.OwnerSeat == candidate.OwnerSeat && end.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && end.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && c.SourceSeat == end.OwnerSeat && c.TargetSeat == end.OwnerSeat && end.ItemIndex >= 0 && end.ItemIndex < end.Items.Count && end.Items[end.ItemIndex].Candidate == candidate && c.OccurrenceIndex == candidate.OccurrenceIndex;
    }

    internal bool YanRouCanDiscard(int chooser, int payer, Card card, CardLocation from) => from.OwnerSeat == payer && from.Zone is (CardZoneKind.Hand or CardZoneKind.Equipment) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(chooser, card, from, OwnedCardMoveIntent.Discard) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(chooser, card, from, OwnedCardMoveIntent.Discard);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26800)]
    internal bool CanOfferYanRou(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context)
    {
        if (t.Effects is not [var effect] || !YanRouComposition.IsOperation(effect.Op))
            return true;
        if (!YanRouParent(c, context))
            return false;
        if (effect.Op == SkillProgramEffectOp.RecoverAndDrawByTurnDamage)
            return !YanRouSpent(c.OwnerSeat, c.SkillId, c.GameplayHash) && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && _coreTreatedWoundedOperations.IsRuleWounded(p));
        var card = context.CardUse!;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(u => u.Id == card.ParentCardUseFrameId);
        var payer = use.SourceSeat;
        return _coreMatchParticipantsPlayersAccess._players[payer].IsAlive && (t.OwnerRelation == SkillProgramCardActionOwnerRelation.Actor ? c.OwnerSeat == payer : context.TargetSeat == c.OwnerSeat && use.Action!.EffectiveDesignatedTargetSeats.Contains(c.OwnerSeat)) && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[payer]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(payer)).Any(ca => YanRouCanDiscard(c.OwnerSeat, payer, ca, _cardZonesOperations._cardZones.GetLocation(ca.Id)));
    }

    internal SkillProgramStepOutcome BeginYanRou(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var c = f.WindowContext;
        if (f.YanRou is not null || f.InstructionIndex != 1 || f.TriggerId is null || c is null || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !CanOfferYanRou(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), _coreProgramLifecycleOperations.GetProgramTrigger(f), c))
            throw new InvalidOperationException("Yan Rou requires its exact current unpaid native candidate.");
        var use = op == SkillProgramEffectOp.DiscardSlashUserForUncancelable ? _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(u => u.Id == c.CardUse!.ParentCardUseFrameId) : null;
        var r = new YanRouReceipt(YanRouSource(f), f.GameplayHash, c.ParentFrameId, c.Window, op, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, use is null ? YanRouStage.TargetChoice : YanRouStage.DiscardChoice, use?.SourceSeat ?? f.OwnerSeat, use is null ? YanRouTurnDamage(f.OwnerSeat) : 0, use?.Id, use?.Action!.ActionId, use?.Action is { } a ? _coreCardUseDebitsOperations.GetCardUseDebit(a.ActionId) : null);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YanRou = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YanRouStartedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, r.Window, r.Operation, r.Turn, r.TurnOwner, r.Payer, r.Amount, r.UseId, r.ActionId));
        PublishYanRou(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> YanRouChoices(ProgramSkillFrame f)
    {
        var r = f.YanRou!;
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return[];
        if (r.Stage == YanRouStage.TargetChoice)
            return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _coreTreatedWoundedOperations.IsRuleWounded(p)).Select(p => new PromptChoice(new($"yan-rou.{(f.Id)}.target.{(p.Seat)}"), $"令{(p.Name)}回复{(r.Amount)}点体力并摸{(r.Amount)}张牌", [], [p.Seat], new Dictionary<string, string> { { "program-action", "yan-rou" }, { "frame-id", f.Id.ToString(CultureInfo.InvariantCulture) } })).ToArray());
        if (r.Stage != YanRouStage.DiscardChoice || !_coreMatchParticipantsPlayersAccess._players[r.Payer].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            return[];
        return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, r.Payer, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard, canSelect: (zone, card) => YanRouCanDiscard(f.OwnerSeat, r.Payer, card, new(zone, r.Payer))).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "yan-rou" } }).ToArray());
    }

    internal void PublishYanRou(ProgramSkillFrame f)
    {
        var choices = YanRouChoices(f);
        if (choices.Count == 0)
        {
            FinishYanRou(f, false);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.YanRou!.Stage == YanRouStage.TargetChoice ? "襄戍：选择已受伤角色。" : "仇讨：弃置此杀使用者的一张牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.YanRou.Payer,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("yan-rou")]
    internal void AnswerYanRou(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { YanRou: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], YanRouChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Yan Rou lost its actual private chooser or published opaque slot.");
        AssertYanRou(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == YanRouStage.TargetChoice)
        {
            var target = choice.Targets.Single();
            if (YanRouSpent(f.OwnerSeat, f.SkillId, f.GameplayHash))
                throw new InvalidOperationException("Xiangshu is permanently spent.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YanRouXiangshuSpentEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, r.Turn, target, r.Amount));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YanRou = r with { Target = target, Payer = target, Stage = YanRouStage.RecoveryChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(target, 0, null) });
            if (r.Amount > 0)
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, target, r.Amount, null, null);
            RecordYanRouMovement(f.Id);
            ContinueYanRou(f.Id);
            return;
        }

        if (!Enum.TryParse<CardZoneKind>(choice.Parameters.GetValueOrDefault("source-zone"), out var zone) || zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !int.TryParse(choice.Parameters.GetValueOrDefault("slot-index"), out var slot))
            throw new InvalidOperationException("Choutao lost its actual HE source slot.");
        var from = new CardLocation(zone, r.Payer);
        var cards = _cardZonesOperations._cardZones.CardsAt(from);
        if (slot < 0 || slot >= cards.Count)
            throw new InvalidOperationException("Choutao source slot is absent.");
        var card = cards[slot];
        var grain = zone == CardZoneKind.Equipment && card.Kind == CardKind.WoodenOx ? _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(_coreMatchParticipantsPlayersAccess._players[r.Payer]).Select(c => c.Id).ToArray() : [];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YanRou = r with { Stage = YanRouStage.DiscardChildren, CardId = card.Id, From = from, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, WoodenOxGrainIds = grain }, PendingMovementContinuation = new(r.Payer, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.Payer, [card.Id], new(YanRouDiscardReason));
        RecordYanRouMovement(f.Id);
        ContinueYanRou(f.Id);
    }

    internal void RecordYanRouMovement(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.YanRou!;
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var shuffles = r.Stage == YanRouStage.DrawChildren ? _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.ParentFrameId == id && b.ParentBatchId is null && b.AwaitingProgramFrameId == id && b.TurnNumber == r.Turn && b.OriginOwnerSeat == f.OwnerSeat && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId && b.Movements.Count > 0 && b.Movements.All(m => m.Sequence > r.Before && m.Sequence <= after && m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m)) && b.SourceCounts is [{ Location: var source, CountBefore: var discarded, CountAfter: 0 }] && source == CardLocation.DiscardPile && discarded == b.Movements.Count && b.DestinationCounts is [{ Location: var destination, CountBefore: 0, CountAfter: var replenished }] && destination == CardLocation.DrawPile && replenished == b.Movements.Count).Select(b => new YanRouReshuffle(b.Id, id, b.Movements[0].Sequence - 1, b.Movements[^1].Sequence) { Movements = b.Movements }).ToArray() : [];
        var count = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= after && (r.Stage != YanRouStage.DrawChildren || m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.Payer) && m.Reason.Value == YanRouDrawReason));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { YanRou = r with { After = after, ActualCount = count, Reshuffles = shuffles } });
        foreach (var s in shuffles)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YanRouReshuffleCapturedEvent(id, s.BatchId, s.Before, s.After, s.Movements.Count));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YanRouMovementIssuedEvent(id, r.Stage, r.Payer, r.CardId, r.From, r.Before, after, count));
    }

    internal bool DrainYanRou(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueYanRou(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { YanRou: { } r } f || f.Id != id)
            return;
        AssertYanRou(f);
        if (r.Stage is YanRouStage.TargetChoice or YanRouStage.DiscardChoice)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishYanRou(f);
            return;
        }

        if (DrainYanRou(f))
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == YanRouStage.DiscardChildren)
        {
            var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(u => u.Id == r.UseId && u.Action?.ActionId == r.ActionId);
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { Enhancements = u.Enhancements | CurrentCardEnhancement.Uncancelable });
            var refunded = r.Payer == f.OwnerSeat && r.Debit is { } debit && debit.ActorSeat == r.Payer && _coreCardUseDebitsOperations.TryRefundCardUseDebit(debit);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YanRouChoutaoAppliedEvent(f.Id, use.Id, r.ActionId!.Value, r.Payer, r.CardId!.Value, refunded));
            FinishYanRou(f, true);
            return;
        }

        if (r.Stage == YanRouStage.RecoveryChildren && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[r.Target!.Value].IsAlive && r.Amount > 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YanRou = r with { Stage = YanRouStage.DrawChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = 0, Reshuffles = [] }, PendingMovementContinuation = new(r.Payer, 0, null) });
            _coreProgramLifecycleOperations.DrawProgramCards(f.Id, r.Payer, r.Amount, null, null, SkillProgramCardSetVisibility.Private, new(YanRouDrawReason));
            RecordYanRouMovement(f.Id);
            ContinueYanRou(f.Id);
            return;
        }

        FinishYanRou(f, true);
    }

    internal void FinishYanRou(ProgramSkillFrame f, bool applied)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YanRouCompletedEvent(f.Id, applied));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YanRou = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 26800)]
    internal bool ResumeYanRou(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { YanRou: not null } f || f.Id != id)
            return false;
        ContinueYanRou(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 26800)]
    internal bool ReturnYanRou(ProgramSkillFrame f)
    {
        if (f.YanRou is null)
            return false;
        ContinueYanRou(f.Id);
        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 180)]
    internal bool IsYanRouAwaitedMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.YanRou is { } r && p.SubjectSeat == r.Payer && p.BeforeCount == 0 && p.CoverageResultBind is null && (e is null || YanRouComposition.IsOperation(e.Op));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26800)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26800)]
    internal bool ContinuePaidYanRou(ProgramSkillFrame f)
    {
        if (f.YanRou is null)
            return false;
        AssertYanRou(f);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.DiscardSlashUserForUncancelable, SkillProgramEffectOp.RecoverAndDrawByTurnDamage)]
    internal PromptChoice AiYanRou(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
}
