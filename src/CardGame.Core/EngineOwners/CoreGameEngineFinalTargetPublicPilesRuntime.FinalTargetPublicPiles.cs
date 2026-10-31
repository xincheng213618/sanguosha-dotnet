#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineFinalTargetPublicPilesRuntime
{
    SkillProgramStepOutcome IFinalTargetPublicPileHost.ExecuteFinalTargetPublicPile(SkillProgramEffect e, ProgramSkillFrame f) => ExecuteFinalTargetPublicPile(e, f);
    internal CardUseFrame FinalTargetPileUse(ProgramSkillFrame frame)
    {
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } use } || use.ActorSeat != frame.OwnerSeat || !ProgramCardKindMatch.Contains(CollectFinalTargetCardInPublicPileDescriptor.Kinds, use.EffectiveKind) || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(p => p.Id == use.ParentCardUseFrameId)is not { Action: { Type: CardActionType.Use } a } parent || a.ActionId != use.CardActionId || a.ActorSeat != frame.OwnerSeat || a.EffectiveKind != use.EffectiveKind)
            throw new InvalidOperationException("Final-target collection lost its actual finalized card use.");
        return parent;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.AfterInitialPayments, 30, SkillProgramEffectOp.CollectFinalTargetCardInPublicPile)]
    internal bool CanCollectFinalTargetPile(int ownerSeat, ProgramSkillWindowContext context) => _coreProgramLifecycleOperations.GetProgramTargetSeats(ownerSeat, SkillProgramTargetKind.CurrentCardUseTargets, context).Any(seat => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count >= _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Count && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]).Count > 0);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 130)]
    internal bool CanActivatePublicPileFlow(CharacterState owner, ProgramInstructionFeatures features) => !features.HasOperation(SkillProgramEffectOp.ObtainPublicPileCard) || features.ForOperation(SkillProgramEffectOp.ObtainPublicPileCard).All(e => _corePublicPileSourcesOperations.ReferencedPublicPileSources(owner.Seat, e.SkillIds.Single()).Any(source => _corePublicPileSourcesOperations.PublicPileCards(source).Count > 0)) && (!features.HasOperation(SkillProgramEffectOp.DiscardPublicZoneAfterHandPayment) || _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Any(p => _corePhysicalOwnedCardQueriesOperations.GetEquipment(p).Any(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner.Seat, card, CardLocation.Equipment(p.Seat), OwnedCardMoveIntent.Discard)) || _corePhysicalOwnedCardQueriesOperations.GetJudgment(p).Count > 0));
    internal IReadOnlyList<PromptChoice> FinalTargetPileChoices(ProgramSkillFrame frame, SkillProgramEffect e)
    {
        if (e.Op == SkillProgramEffectOp.ObtainPublicPileCard)
            return _corePublicPileSourcesOperations.ReferencedPublicPileSources(frame.OwnerSeat, e.SkillIds.Single(), frame.SkillInstanceId).SelectMany(source => _corePublicPileSourcesOperations.PublicPileCards(source)).Select(card => new PromptChoice(new($"pile-flow.frame-{(frame.Id)}.card-{(card.Id)}"), "获得【" + CoreGameEnginePublicPersistentPilesRuntime.PublicPileCardLabel(card) + "】", [card.Id], [], new Dictionary<string, string> { ["program-action"] = "public-pile-flow", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToArray();
        var seats = e.Op == SkillProgramEffectOp.CollectFinalTargetCardInPublicPile ? FinalTargetPileUse(frame).TargetSeats.Distinct().Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count >= _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Count) : _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat);
        var zones = e.Op == SkillProgramEffectOp.CollectFinalTargetCardInPublicPile ? new[]
        {
            CardZoneKind.Hand,
            CardZoneKind.Equipment
        }

        : new[]
        {
            CardZoneKind.Equipment,
            CardZoneKind.Judgment
        };
        return seats.SelectMany(seat => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frame.Id, frame.OwnerSeat, seat, zones, e.Op == SkillProgramEffectOp.CollectFinalTargetCardInPublicPile ? OwnedCardMoveIntent.Transfer : OwnedCardMoveIntent.Discard).Select(choice => choice with { Id = new($"pile-flow.frame-{(frame.Id)}.owner-{(seat)}." + choice.Id.Value), Description = _coreMatchParticipantsPlayersAccess._players[seat].Name + "：" + choice.Description, Targets = [seat], Parameters = new Dictionary<string, string>(choice.Parameters) { ["program-action"] = "public-pile-flow" } })).ToArray();
    }

    internal SkillProgramStepOutcome ExecuteFinalTargetPublicPile(SkillProgramEffect e, ProgramSkillFrame frame)
    {
        if (e.Op == SkillProgramEffectOp.ExchangePublicPileHand)
            return _corePublicPersistentPilesOperations.ExecutePublicPile(e, frame);
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        if (e.Op == SkillProgramEffectOp.ObtainPublicPileCard && !_corePublicPileSourcesOperations.ReferencedPublicPileSources(frame.OwnerSeat, e.SkillIds.Single(), frame.SkillInstanceId).Any(source => _corePublicPileSourcesOperations.PublicPileCards(source).Count > 0))
            return SkillProgramStepOutcome.Continue;
        var choices = FinalTargetPileChoices(frame, e);
        if (choices.Count == 0)
            return SkillProgramStepOutcome.Continue;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, e.Op == SkillProgramEffectOp.CollectFinalTargetCardInPublicPile ? "从符合条件的实际目标取一张手牌或装备牌，置入公开牌堆。" : e.Op == SkillProgramEffectOp.ObtainPublicPileCard ? "获得一张公开牌堆牌。" : "弃置任意角色装备区或判定区的一张牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = e.Op == SkillProgramEffectOp.CollectFinalTargetCardInPublicPile,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("public-pile-flow")]
    internal void ResolveFinalTargetPileChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Public pile flow lost its owner.");
        var e = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (e.Op is not (SkillProgramEffectOp.CollectFinalTargetCardInPublicPile or SkillProgramEffectOp.ObtainPublicPileCard or SkillProgramEffectOp.DiscardPublicZoneAfterHandPayment) || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("Public pile flow lost its instruction.");
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "公开牌堆技能来源失效。");
            return;
        }

        var valid = FinalTargetPileChoices(frame, e).SingleOrDefault(c => c.Id == selected.Id) ?? throw new InvalidOperationException("Public pile choice no longer names a valid published slot.");
        Card card;
        CardLocation from;
        CardLocation to;
        long? actionId = null;
        int? target = null;
        if (e.Op == SkillProgramEffectOp.ObtainPublicPileCard)
        {
            var source = _corePublicPileSourcesOperations.ReferencedPublicPileSources(frame.OwnerSeat, e.SkillIds.Single(), frame.SkillInstanceId).SingleOrDefault(source => _corePublicPileSourcesOperations.PublicPileCards(source).Any(card => card.Id == valid.Cards.Single())) ?? throw new InvalidOperationException("Public pile source changed.");
            card = _corePublicPileSourcesOperations.PublicPileCards(source).Single(c => c.Id == valid.Cards.Single());
            from = source.Location;
            to = CardLocation.Hand(frame.OwnerSeat);
        }
        else
        {
            var seat = valid.Targets.Single();
            var zone = Enum.Parse<CardZoneKind>(valid.Parameters["source-zone"]);
            var slot = int.Parse(valid.Parameters["slot-index"]);
            from = new(zone, seat);
            card = _cardZonesOperations._cardZones.CardsAt(from)[slot];
            to = CardLocation.DiscardPile;
            if (e.Op == SkillProgramEffectOp.CollectFinalTargetCardInPublicPile)
            {
                actionId = FinalTargetPileUse(frame).Action!.ActionId;
                target = seat;
                to = _corePublicPileSourcesOperations.EnsurePublicPileSource(frame, 0).Location;
            }
        }

        if (e.Op == SkillProgramEffectOp.DiscardPublicZoneAfterHandPayment && _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(frame.OwnerSeat, card, from, OwnedCardMoveIntent.Discard))
            throw new InvalidOperationException("Public-zone discard is no longer available.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        var reason = new CardMoveReason(e.Op == SkillProgramEffectOp.CollectFinalTargetCardInPublicPile ? "skill-program.public-pile.collect-final-target" : e.Op == SkillProgramEffectOp.ObtainPublicPileCard ? "skill-program.public-pile.obtain" : "skill-program.public-pile.field-discard");
        var discardInvocation = e.Op == SkillProgramEffectOp.DiscardPublicZoneAfterHandPayment ? _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(frame.OwnerSeat, frame.OwnerSeat, [card.Id], reason, frame.Id) : null;
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, to, reason, movement => _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(movement, frame.OwnerSeat, frame.Id), disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(discardInvocation);
        if (actionId is { } id && _cardZonesOperations._cardZones.GetLocation(card.Id) == to)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FinalTargetCardStoredEvent(id, frame.OwnerSeat, target!.Value, card.Id));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
    }
}
