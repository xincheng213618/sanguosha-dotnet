#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineFinalTargetGiftsRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IFinalTargetGiftHost.GiveOwnedCardToOtherFinalTargetAndDraw(ProgramSkillFrame frame) => BeginFinalTargetGift(frame);
    internal CardUseFrame FinalTargetGiftUse(ProgramSkillFrame frame)
    {
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } use } || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Id == use.ParentCardUseFrameId)is not { Action: { Type: CardActionType.Use } action } parent || action.ActionId != use.CardActionId || global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(action.EffectiveKind) != SkillProgramCardCategory.Trick || parent.Action!.EffectiveDesignatedTargetSeats.Distinct().Count() <= 1 || !parent.Action.EffectiveDesignatedTargetSeats.Contains(frame.OwnerSeat))
            throw new InvalidOperationException("Final-target gift lost its actual multi-target Trick action.");
        return parent;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeAdvancedPrerequisites, 10)]
    internal bool CanOfferFinalTargetGift(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, ProgramInstructionFeatures features)
    {
        if (!features.HasOperation(SkillProgramEffectOp.GiveOwnedCardToOtherFinalTargetAndDraw))
            return true;
        if (context.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || context.CardUse is not { } use || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Id == use.ParentCardUseFrameId)is not { Action: { Type: CardActionType.Use } action } parent || action.ActionId != use.CardActionId || global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(action.EffectiveKind) != SkillProgramCardCategory.Trick)
            return false;
        var targets = FinalTargetGiftSeats(parent);
        return targets.Count > 1 && targets.Contains(candidate.OwnerSeat) && targets.Any(s => s != candidate.OwnerSeat) && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Count > 0;
    }

    internal IReadOnlyList<int> FinalTargetGiftSeats(CardUseFrame use) => use.Action!.EffectiveDesignatedTargetSeats.Distinct().Where(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive && use.IneffectiveTargetSeats?.Contains(s) != true).ToArray();
    internal IReadOnlyList<PromptChoice> FinalTargetGiftChoices(ProgramSkillFrame frame)
    {
        var seats = FinalTargetGiftSeats(FinalTargetGiftUse(frame)).Where(s => s != frame.OwnerSeat);
        return seats.SelectMany(seat => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frame.Id, frame.OwnerSeat, frame.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer).Select(c => c with { Id = new($"final-gift.frame-{(frame.Id)}.target-{(seat)}.{(c.Id.Value)}"), Targets = [seat], Description = _coreMatchParticipantsPlayersAccess._players[seat].Name + "：" + c.Description, Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "final-target-gift" } })).ToArray();
    }

    internal SkillProgramStepOutcome BeginFinalTargetGift(ProgramSkillFrame frame)
    {
        var action = FinalTargetGiftUse(frame).Action!;
        var choices = FinalTargetGiftChoices(frame);
        if (choices.Count == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { FinalTargetGift = new(action.ActionId, "choice") });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "交给另一名最终目标一张手牌或装备牌，然后摸牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("final-target-gift")]
    internal void ResolveFinalTargetGiftChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Final gift lost owning frame.");
        if (frame.FinalTargetGift is not { Stage: "choice" } draft || draft.ActionId != FinalTargetGiftUse(frame).Action!.ActionId)
            throw new InvalidOperationException("Final gift lost action.");
        var valid = FinalTargetGiftChoices(frame).SingleOrDefault(c => c.Id == selected.Id) ?? throw new InvalidOperationException("Final gift no longer names legal payment/recipient.");
        var from = new CardLocation(Enum.Parse<CardZoneKind>(valid.Parameters["source-zone"]), frame.OwnerSeat);
        var card = _cardZonesOperations._cardZones.CardsAt(from)[int.Parse(valid.Parameters["slot-index"], System.Globalization.CultureInfo.InvariantCulture)];
        var recipient = valid.Targets.Single();
        var to = CardLocation.Hand(recipient);
        var reward = _coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind) ? 2 : 1;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { FinalTargetGift = draft with { Stage = "gift-movement", RecipientSeat = recipient, CardId = card.Id, Reward = reward } });
        var beforeOrdinal = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, to, new("skill-program.final-target-gift.give"), _characterHuangQuanMouOperations.NativeGiveCompletion(frame.Id, frame.OwnerSeat, recipient, [card.Id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null));
        var ordinal = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.Sequence > beforeOrdinal && m.CardId == card.Id && m.From == from && m.To == to && m.Reason.Value == "skill-program.final-target-gift.give")?.Sequence;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { FinalTargetGift = frame.FinalTargetGift!with { ReceiptOrdinal = ordinal } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramFinalTargetGiftCommittedEvent(frame.Id, draft.ActionId, frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, recipient, card.Id, reward, ordinal));
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 910)]
    internal bool ResumeFinalTargetGift(long frameId)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        if (frame.FinalTargetGift is not { } draft)
            return false;
        if (frame.PendingMovementContinuation is not null)
            return false;
        if (draft.Stage == "choice")
            return true;
        if (draft.Stage == "gift-movement" && draft.ReceiptOrdinal is not null && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { FinalTargetGift = draft with { Stage = "reward-movement" } });
            _coreProgramLifecycleOperations.DrawProgramCards(frame.Id, frame.OwnerSeat, draft.Reward, null, null, SkillProgramCardSetVisibility.Private, new("skill-program.final-target-gift.draw"));
            if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
                _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { FinalTargetGift = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }
}
