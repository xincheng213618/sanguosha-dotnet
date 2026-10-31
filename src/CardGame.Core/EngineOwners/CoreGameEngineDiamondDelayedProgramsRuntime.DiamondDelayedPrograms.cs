#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDiamondDelayedProgramsRuntime
{
    internal bool DiamondPaymentLegal(int ownerSeat, Card card, string skillId, string? instanceId = null) => _coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive && _cardZonesOperations._cardZones.GetLocation(card.Id)is { OwnerSeat: var seat, Zone: CardZoneKind.Hand or CardZoneKind.Equipment } && seat == ownerSeat && !card.IsGeneralWeapon && !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(ownerSeat, skillId, instanceId ?? _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[ownerSeat], skillId), card) && _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[ownerSeat], card) == Suit.Diamond;
    internal bool DiamondUseTargetLegal(int ownerSeat, Card card, int targetSeat, string skillId, string? instanceId = null) => DiamondPaymentLegal(ownerSeat, card, skillId, instanceId) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) && targetSeat != ownerSeat && _coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive && !_coreMatchParticipantsPlayersAccess._players[targetSeat].JudgmentAreaAbolished && !_coreActualTurnsOperations.HasJudgmentEffectiveCard(_coreMatchParticipantsPlayersAccess._players[targetSeat], CardKind.Indulgence) && !_coreCardUseModulesOperations.IsTurnHandCardRestricted(_coreMatchParticipantsPlayersAccess._players[ownerSeat], card) && !_coreProgramPhaseColorRestrictionsOperations.IsPlayPhasePhysicalCardRestricted(_coreMatchParticipantsPlayersAccess._players[ownerSeat], card) && !_coreCardUseModulesOperations.IsCardUseForbidden(ownerSeat, CardKind.Indulgence, CardActionType.Use) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(ownerSeat, targetSeat, CardKind.Indulgence) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[targetSeat], CardKind.Indulgence, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[ownerSeat], card), global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[ownerSeat], card))) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(ownerSeat, targetSeat, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[ownerSeat], card));
    internal IEnumerable<(int Seat, Card Card)> DiamondJudgments() => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).OrderBy(p => p.Seat).SelectMany(p => _corePhysicalOwnedCardQueriesOperations.GetJudgment(p).Where(c => !_characterDaQiaoLeOperations.IsZixiQinJudgmentCard(c.Id) && _coreActualTurnsOperations.GetJudgmentEffectiveCardKind(c) == CardKind.Indulgence).OrderBy(c => c.Id).Select(c => (p.Seat, c)));
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 100)]
    internal bool CanActivateDiamondDelayed(CharacterState owner, SkillProgramActivation activation, string skillId) => !activation.Effects.Any(e => e.Op == SkillProgramEffectOp.UseDiamondDelayedOrDiscard) || _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(owner)).Where(c => DiamondPaymentLegal(owner.Seat, c, skillId)).Any(c => DiamondJudgments().Any() || _coreMatchParticipantsPlayersAccess._players.Any(p => DiamondUseTargetLegal(owner.Seat, c, p.Seat, skillId)));
    internal SkillProgramStepOutcome BeginDiamondDelayed(ProgramSkillFrame frame)
    {
        if (frame.TriggerId is not null || frame.SelectedCardIds.Count != 1 || frame.SelectedTargetSeats.Count != 0 || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != frame.OwnerSeat)
            throw new InvalidOperationException("Diamond delayed use needs one owner payment in actual Play.");
        var card = _coreResponseEntityExchangeOperations.EntityAtCurrentLocation(frame.SelectedCardIds.Single());
        if (!DiamondPaymentLegal(frame.OwnerSeat, card, frame.SkillId, frame.SkillInstanceId))
            throw new InvalidOperationException("Diamond payment is not a real eligible owner entity.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { DiamondDelayed = new(card.Id, _cardZonesOperations._cardZones.GetLocation(card.Id), _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], card), _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, ProgramDiamondDelayedStage.Choosing) });
        PublishDiamondDelayed(frame);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> DiamondDelayedChoices(ProgramSkillFrame frame)
    {
        var d = frame.DiamondDelayed!;
        var card = _coreResponseEntityExchangeOperations.EntityAtCurrentLocation(d.CardId);
        Dictionary<string, string> P(string branch) => new()
        {
            ["program-action"] = "diamond-delayed",
            ["frame-id"] = frame.Id.ToString(),
            ["branch"] = branch
        };
        return _coreMatchParticipantsPlayersAccess._players.Where(p => DiamondUseTargetLegal(frame.OwnerSeat, card, p.Seat, frame.SkillId, frame.SkillInstanceId)).Select(p => new PromptChoice(new($"diamond-delayed.{(frame.Id)}.use.{(p.Seat)}"), $"对 {(p.Name)} 使用【乐不思蜀】", [], [p.Seat], P("use"))).Concat(DiamondJudgments().Select(j => new PromptChoice(new($"diamond-delayed.{(frame.Id)}.discard.{(j.Card.Id)}"), $"弃置 {(_coreMatchParticipantsPlayersAccess._players[j.Seat].Name)} 判定区的【乐不思蜀】", [j.Card.Id], [j.Seat], P("discard")))).ToArray();
    }

    internal void PublishDiamondDelayed(ProgramSkillFrame frame)
    {
        var choices = DiamondDelayedChoices(frame);
        if (choices.Count == 0)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "国色的支付或目标已失效。");
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, $"{(skill.Name)}：选择使用或弃置。", [], [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal bool IsDiamondDelayedChoiceLegal(PromptChoice choice)
    {
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.DiamondDelayed is not { Stage: ProgramDiamondDelayedStage.Choosing } d || choice.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString() || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId) || d.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || d.TurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _cardZonesOperations._cardZones.GetLocation(d.CardId) != d.Source || !DiamondPaymentLegal(frame.OwnerSeat, _coreResponseEntityExchangeOperations.EntityAtCurrentLocation(d.CardId), frame.SkillId, frame.SkillInstanceId))
            return false;
        return DiamondDelayedChoices(frame).Any(c => c.Id == choice.Id && c.Cards.SequenceEqual(choice.Cards) && c.Targets.SequenceEqual(choice.Targets) && c.Parameters.OrderBy(p => p.Key).SequenceEqual(choice.Parameters.OrderBy(p => p.Key)));
    }

    [ProgramAnswer("diamond-delayed")]
    internal void ResolveDiamondDelayedChoice(PromptChoice choice)
    {
        if (!IsDiamondDelayedChoiceLegal(choice))
            throw new InvalidOperationException("The exact diamond payment or target is stale.");
        var frame = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack[^1];
        var d = frame.DiamondDelayed!;
        var card = _coreResponseEntityExchangeOperations.EntityAtCurrentLocation(d.CardId);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (choice.Parameters["branch"] == "use")
        {
            var target = choice.Targets.Single();
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { DiamondDelayed = d with { Stage = ProgramDiamondDelayedStage.Using, TargetSeat = target } });
            var id = _coreNativeCardUseOperations.BeginCardUse(card, frame.OwnerSeat, [target], CardKind.Indulgence, conversionSource: new(frame.SkillId, frame.ActivationId, frame.OwnerSeat, frame.SkillInstanceId));
            _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { DiamondDelayed = frame.DiamondDelayed!with { UseFrameId = id } });
            _corePhysicalMovementFacadesOperations.MoveCard(card, d.Source, CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
            _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, frame.OwnerSeat, [target], LegalActionKind.Indulgence, playedCardKind: CardKind.Indulgence);
            return;
        }

        var judgment = _coreResponseEntityExchangeOperations.EntityAtCurrentLocation(choice.Cards.Single());
        var from = _cardZonesOperations._cardZones.GetLocation(judgment.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { DiamondDelayed = d with { Stage = ProgramDiamondDelayedStage.Moving, TargetSeat = choice.Targets.Single(), JudgmentCardId = judgment.Id } });
        var discardInvocation = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(frame.OwnerSeat, frame.OwnerSeat, [card.Id, judgment.Id], new("skill-program.diamond-delayed.discard"), frame.Id);
        var batch = _coreCardMovementProgramsOperations.BeginCardMovementBatch([d.Source, from], [CardLocation.DiscardPile]);
        var movements = new List<CardMovementRecord>();
        var committed = false;
        try
        {
            _cardZonesOperations._cardZones.MoveBatch([new(card.Id, d.Source, CardLocation.DiscardPile), new(judgment.Id, from, CardLocation.DiscardPile)]);
            foreach (var pair in new[]
            {
                (Card: card, From: d.Source),
                (Card: judgment, From: from)
            }

            )
            {
                movements.Add(_corePhysicalMovementFacadesOperations.RecordMovement(pair.Card, pair.From, CardLocation.DiscardPile, new("skill-program.diamond-delayed.discard")));
                _coreEquipmentSkillsOperations.ResolveEquipmentSkillGrant(pair.Card, pair.From, CardLocation.DiscardPile);
                _corePhysicalMovementFacadesOperations.ClearJudgmentEffectiveKindAfterMove(pair.Card, pair.From, CardLocation.DiscardPile);
                _corePhysicalMovementFacadesOperations.ResolveSilverLionRemoval(pair.Card, pair.From, new("skill-program.diamond-delayed.discard"));
                _corePhysicalMovementFacadesOperations.ResolveWoodenOxMove(pair.Card, pair.From, CardLocation.DiscardPile);
                _coreActualDiscardPhaseLedgerOperations.CollectDiscardPhaseHandDiscard(pair.Card, pair.From, CardLocation.DiscardPile);
            }

            committed = true;
        }
        finally
        {
            _coreCardMovementProgramsOperations.CompleteCardMovementBatch(batch, movements, committed);
        }

        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(discardInvocation);
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [NativeUseReturn(110)]
    internal void ContinueProgramAfterDiamondDelayedUse(long completedUseId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.DiamondDelayed is not { Stage: ProgramDiamondDelayedStage.Using } d)
            return;
        if (d.UseFrameId != completedUseId || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseFinishedEvent>().Any(e => e.ResolutionId == completedUseId && e.CardId == d.CardId && e.CardKind == CardKind.Indulgence))
            throw new InvalidOperationException("Diamond delayed use returned from a different child.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { DiamondDelayed = d with { Stage = ProgramDiamondDelayedStage.Moving } });
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 860)]
    internal bool ResumeDiamondDelayed(long frameId)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        if (frame.DiamondDelayed is not { } d)
            return false;
        if (d.Stage == ProgramDiamondDelayedStage.Using || frame.PendingMovementContinuation is not null)
            return true;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "国色的持有人或来源已失效。");
            return true;
        }

        if (d.Stage == ProgramDiamondDelayedStage.Choosing)
        {
            if (_cardZonesOperations._cardZones.GetLocation(d.CardId) != d.Source || !DiamondPaymentLegal(frame.OwnerSeat, _coreResponseEntityExchangeOperations.EntityAtCurrentLocation(d.CardId), frame.SkillId, frame.SkillInstanceId))
                _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "国色锁定的实体已失效。");
            else
                PublishDiamondDelayed(frame);
            return true;
        }

        if (d.Stage == ProgramDiamondDelayedStage.Moving)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { DiamondDelayed = d with { Stage = ProgramDiamondDelayedStage.Drawing } });
            _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], 1, log: true);
            if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.AwaitChild)
                return true;
            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DiamondDelayed = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    internal void AssertDiamondDelayed(ProgramSkillFrame frame, SkillProgramEffect? paused)
    {
        if (frame.DiamondDelayed is not { } d)
            return;
        if (paused?.Op != SkillProgramEffectOp.UseDiamondDelayedOrDiscard || frame.TriggerId is not null || frame.SelectedCardIds.Count != 1 || frame.SelectedCardIds[0] != d.CardId || d.Source.OwnerSeat != frame.OwnerSeat || d.Source.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || d.EffectiveSuit != Suit.Diamond || d.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || d.TurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || !Enum.IsDefined(d.Stage) || d.Stage == ProgramDiamondDelayedStage.Using && (d.UseFrameId is null || !_resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(c => c.Id == d.UseFrameId && c.SourceSeat == frame.OwnerSeat && c.CardId == d.CardId && c.CardKind == CardKind.Indulgence && _resolutionStackOperations._resolutionStack.IndexOf(frame) + 1 == _resolutionStackOperations._resolutionStack.IndexOf(c) && c.Action is { } a && a.PhysicalCards.Count == 1 && a.PhysicalCards[0].CardId == d.CardId && a.PhysicalCards[0].From == d.Source && a.ActorSeat == frame.OwnerSeat && a.EffectiveSuit == d.EffectiveSuit && a.ConversionChain.SequenceEqual([new CardConversionSource(frame.SkillId, frame.ActivationId, frame.OwnerSeat, frame.SkillInstanceId)]))))
            throw new InvalidOperationException("Diamond delayed draft lost its exact owning operation or child.");
        if (d.Stage == ProgramDiamondDelayedStage.Choosing && ReferenceEquals(frame, _resolutionStackOperations._resolutionStack.LastOrDefault()) && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } prompt || prompt.PlayerSeat != frame.OwnerSeat || prompt.Choices.Count == 0 || prompt.Choices.Any(c => !IsDiamondDelayedChoiceLegal(c))))
            throw new InvalidOperationException("Diamond delayed draft lost its exact choice.");
    }

    SkillProgramStepOutcome global::CardGame.Core.IDiamondDelayedProgramHost.UseDiamondDelayedOrDiscard(ProgramSkillFrame frame) => BeginDiamondDelayed(frame);
}
