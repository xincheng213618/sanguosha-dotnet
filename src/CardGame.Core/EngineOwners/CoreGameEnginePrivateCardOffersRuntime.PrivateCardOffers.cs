#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePrivateCardOffersRuntime
{
    SkillProgramStepOutcome IPrivateOfferProgramHost.DepositPrivateCardOffer(ProgramSkillFrame f, string bind, string resolver) => DepositPrivateCardOffer(f, bind, resolver);
    SkillProgramStepOutcome IPrivateOfferProgramHost.ResolvePrivateCardOffer(ProgramSkillFrame f) => BeginDeferredPrivateOffer(f);
    SkillProgramStepOutcome IPrivateOfferProgramHost.ResolveGameTargetHandHp(ProgramSkillFrame f, string state) => _coreGameTargetHandHpChoicesOperations.BeginGameTargetHandHp(f, state);
    internal long PrivateOfferSequence => _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;

    internal static string PrivateOfferReason(ProgramSkillFrame f, string segment) => RuntimeSkills.RuntimeRules_Core_PrivateCardOffers.PrivateOfferReason(f, segment);
    internal static CardConversionSource PrivateOfferSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_Core_PrivateCardOffers.PrivateOfferSource(f);
    internal SkillProgramStepOutcome DepositPrivateCardOffer(ProgramSkillFrame supplied, string bind, string resolver)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var set = _coreProgramLifecycleOperations.GetProgramCardSet(f, bind);
        var program = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!;
        if (f.PrivateOffer is not null || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } context || f.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || context.OwnerSeat != f.OwnerSeat || f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || set.CardIds is not [var id] || !set.SourceLocations.SequenceEqual([CardLocation.Hand(f.OwnerSeat)]) || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(f.OwnerSeat) || program.Triggers.SingleOrDefault(t => t.Id == resolver)is not { Optional: false, Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } trigger || trigger.Effects is not [{ Op: SkillProgramEffectOp.ResolveDeferredPrivateCardOffer }])
            throw new InvalidOperationException("A deferred private offer requires one exact ending Hand entity, living other and mandatory resolver.");
        var grant = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.Grants.Single(g => g.SkillId == f.SkillId && g.SkillInstanceId == f.SkillInstanceId);
        var offer = new DeferredPrivateOfferIdentity(f.Id, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), resolver, f.GameplayHash, target, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber);
        var identity = new PrivateTurnHoldIdentity(f.Id, f.SkillId, f.SkillInstanceId, grant.SourceId, int.MaxValue)
        {
            DeferredOffer = offer
        };
        var location = new CardLocation(CardZoneKind.PrivateTurnHold, f.OwnerSeat, privateTurnHold: identity);
        _cardZonesOperations._cardZones.EnsurePrivateTurnHold(location);
        var before = PrivateOfferSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PrivateOffer = new(f.InstructionIndex, location, id, PrivateOfferStage.DepositChildren, before, before), PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Single(c => c.Id == id), CardLocation.Hand(f.OwnerSeat), location, new($"skill-program.{(f.SkillId)}.{(SkillProgramEffectOp.DepositBoundPrivateCardOffer)}"));
        var after = PrivateOfferSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { PrivateOffer = f.PrivateOffer!with { SequenceAfter = after } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PrivateCardOfferDepositedEvent(f.Id, PrivateOfferSource(f), f.GameplayHash, target, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, before, after));
        if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
            _corePrivateOfferReturnsOperations.ReturnPrivateOfferMovement(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool TryBeginDeferredPrivateOffers(CharacterState target)
    {
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DepositBoundPrivateCardOffer))
            return false;
        var locations = _cardZonesOperations._cardZones.PrivateTurnHoldLocations.Where(l => l.PrivateTurnHold?.DeferredOffer is { } o && o.TargetSeat == target.Seat && o.CreatedTurn < _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && l.OwnerSeat is { } source && _coreMatchParticipantsPlayersAccess._players[source].IsAlive && _cardZonesOperations._cardZones.CardsAt(l).Count == 1).OrderBy(l => l.PrivateTurnHold!.HoldId).ToArray();
        if (locations.Length == 0)
            return false;
        if (_resolutionStackOperations._resolutionStack.Count != 0 || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || target.Seat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
            throw new InvalidOperationException("A deferred offer must start at its exact clean actual turn boundary.");
        var candidates = locations.Select((location, index) =>
        {
            var h = location.PrivateTurnHold!;
            var o = h.DeferredOffer!;
            if (!ValidPrivateOfferDeposit(location, out _))
                throw new InvalidOperationException("A deferred offer lost its original private deposit ledger.");
            return new ProgramTriggerCandidate(location.OwnerSeat!.Value, h.SkillId, o.ResolverBindingId, h.SkillInstanceId, o.GameplayHash, 0, index);
        }).ToArray();
        var root = new ProgramLifecycleTriggerWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, target.Seat, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, Array.AsReadOnly(candidates), ProgramLifecycleContinuation.ResumeAfterDeferredPrivateOffers, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(target))
        {
            DeferredPrivateOffers = new(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, target.Seat, locations)
        };
        _coreRuntimeOperations.PushRuntimeFrame(root);
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramLifecycleTriggerWindowFrame>();
        return true;
    }

    internal bool ValidPrivateOfferDeposit(CardLocation location, out int cardId)
    {
        cardId = 0;
        if (location is not { Zone: CardZoneKind.PrivateTurnHold, OwnerSeat: { } source, PrivateTurnHold: { DeferredOffer: { } o } h } || source == o.TargetSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(source) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(o.TargetSeat) || h.ExpiresTurnNumber != int.MaxValue || h.HoldId != o.DepositFrameId || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(h.SkillId)?.Program is not { } program || program.GameplayHash != o.GameplayHash || program.Triggers.SingleOrDefault(t => t.Id == o.ResolverBindingId)?.Effects is not [{ Op: SkillProgramEffectOp.ResolveDeferredPrivateCardOffer }])
            return false;
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PrivateCardOfferDepositedEvent>().Where(e => e.FrameId == o.DepositFrameId).ToArray();
        if (facts is not [var fact] || fact.Source != new CardConversionSource(h.SkillId, o.DepositBindingId, source, h.SkillInstanceId) || fact.GameplayHash != o.GameplayHash || fact.TargetSeat != o.TargetSeat || fact.CreatedTurn != o.CreatedTurn || fact.After <= fact.Before)
            return false;
        var moves = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > fact.Before && m.Sequence <= fact.After).ToArray();
        if (moves is not [var move] || move.From != CardLocation.Hand(source) || move.To != location || move.Reason.Value != $"skill-program.{(h.SkillId)}.{(SkillProgramEffectOp.DepositBoundPrivateCardOffer)}")
            return false;
        cardId = move.CardId;
        return true;
    }

    internal CardLocation? CurrentDeferredPrivateOffer(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(p => p.Id == context.ParentFrameId)is not { Continuation: ProgramLifecycleContinuation.ResumeAfterDeferredPrivateOffers, DeferredPrivateOffers: { } due } parent || parent.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || due.TargetSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || due.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || due.Locations.Count != parent.Candidates.Count || parent.CandidateIndex < 0 || parent.CandidateIndex >= due.Locations.Count || parent.Candidates[parent.CandidateIndex] != candidate || context.OwnerSeat != candidate.OwnerSeat || context.SourceSeat != due.TargetSeat || context.TargetSeat != due.TargetSeat || context.OccurrenceIndex != candidate.OccurrenceIndex)
            return null;
        var location = due.Locations[parent.CandidateIndex];
        var h = location.PrivateTurnHold!;
        var o = h.DeferredOffer!;
        return location.OwnerSeat == candidate.OwnerSeat && h.SkillId == candidate.SkillId && h.SkillInstanceId == candidate.SkillInstanceId && o.GameplayHash == candidate.GameplayHash && o.ResolverBindingId == candidate.BindingId && o.TargetSeat == due.TargetSeat && o.CreatedTurn < due.TurnNumber && ValidPrivateOfferDeposit(location, out _) ? location : null;
    }

    internal bool CanRunDeferredPrivateOffer(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => CurrentDeferredPrivateOffer(candidate, context)is { } location && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat].IsAlive && _cardZonesOperations._cardZones.CardsAt(location).Count == 1 && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None;
    internal bool IsDeferredPrivateOfferResolver(ProgramTriggerCandidate candidate) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(candidate.SkillId)?.Program?.Triggers.SingleOrDefault(t => t.Id == candidate.BindingId)?.Effects is [{ Op: SkillProgramEffectOp.ResolveDeferredPrivateCardOffer }];
    internal CardLocation? DeferredPrivateOfferParent(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { } context)
            return null;
        return CurrentDeferredPrivateOffer(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0, context.OccurrenceIndex), context);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 710)]
    internal bool CanContinueIssuedPrivateOffer(ProgramSkillFrame f) => _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && DeferredPrivateOfferParent(f)is not null && (f.PrivateOffer is null || _corePrivateOfferReturnsOperations.ValidPrivateOfferReceipt(f));
    internal SkillProgramStepOutcome BeginDeferredPrivateOffer(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var location = DeferredPrivateOfferParent(f) ?? throw new InvalidOperationException("A private offer lost its exact due parent/source.");
        if (f.PrivateOffer is not null || !ValidPrivateOfferDeposit(location, out var id) || _cardZonesOperations._cardZones.GetLocation(id) != location || f.InstructionIndex != 1)
            throw new InvalidOperationException("A due private offer cannot be viewed twice or reconstructed from a different card.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PrivateOffer = new(1, location, id, PrivateOfferStage.ChoosingExchange, 0, 0) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PrivateCardOfferViewedEvent(f.Id, location.PrivateTurnHold!.HoldId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber));
        PublishPrivateOffer(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> PrivateOfferChoices(ProgramSkillFrame f)
    {
        var r = f.PrivateOffer!;
        var actor = r.Location.PrivateTurnHold!.DeferredOffer!.TargetSeat;
        var card = _cardZonesOperations._cardZones.CardsAt(r.Location).Single(c => c.Id == r.CardId);
        var category = CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(card.Kind);
        var choices = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[actor]).Where(c => CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(c.Kind) == category).Select(c => new PromptChoice(new($"private-offer.{(f.Id)}.give-{(c.Id)}"), $"交给来源【{(c.DisplayName)}】，获得暗置的【{(card.DisplayName)}】", [c.Id], [actor], new Dictionary<string, string> { ["program-action"] = "deferred-private-offer", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "exchange" })).ToList();
        choices.Add(new(new($"private-offer.{(f.Id)}.remove"), $"移去暗置的【{(card.DisplayName)}】并失去1点体力", [], [actor], new Dictionary<string, string> { ["program-action"] = "deferred-private-offer", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "remove" }));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishPrivateOffer(ProgramSkillFrame f)
    {
        var actor = f.PrivateOffer!.Location.PrivateTurnHold!.DeferredOffer!.TargetSeat;
        var choices = PrivateOfferChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, actor, "观看笔伐暗置牌并选择同类型手牌交换，或移去并失去体力。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [actor], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = actor,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[actor].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("deferred-private-offer")]
    internal void ResolveDeferredPrivateOfferChoice(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("No owning private offer.");
        if (f.PrivateOffer is not { Stage: PrivateOfferStage.ChoosingExchange } r || !_corePrivateOfferReturnsOperations.ValidPrivateOfferReceipt(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { IsPrivate: true } decision || decision.PlayerSeat != r.Location.PrivateTurnHold!.DeferredOffer!.TargetSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], PrivateOfferChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("The private offer choice changed its exact target or material.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var target = decision.PlayerSeat;
        var before = PrivateOfferSequence;
        if (selected.Parameters["branch"] == "exchange")
        {
            var id = selected.Cards.Single();
            var from = CardLocation.Hand(target);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PrivateOffer = r with { Stage = PrivateOfferStage.ExchangeChildren, SequenceBefore = before, SequenceAfter = before, PaymentCardId = id, PaymentFrom = from }, PendingMovementContinuation = new(target, 0, null) });
            _corePhysicalMovementFacadesOperations.MoveCard(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Single(c => c.Id == id), from, CardLocation.Hand(f.OwnerSeat), new(PrivateOfferReason(f, "exchange")));
        }
        else
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PrivateOffer = r with { Stage = PrivateOfferStage.RemovalChildren, SequenceBefore = before, SequenceAfter = before }, PendingMovementContinuation = new(target, 0, null) });
            _corePhysicalMovementFacadesOperations.MoveCard(_cardZonesOperations._cardZones.CardsAt(r.Location).Single(c => c.Id == r.CardId), r.Location, CardLocation.DiscardPile, new(PrivateOfferReason(f, "remove")));
        }

        CommitPrivateOfferSegment(f.Id);
        if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)))
            _corePrivateOfferReturnsOperations.ReturnPrivateOfferMovement(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void CommitPrivateOfferSegment(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var after = PrivateOfferSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PrivateOffer = f.PrivateOffer!with { SequenceAfter = after } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PrivateCardOfferSegmentIssuedEvent(f.Id, f.PrivateOffer!.Location.PrivateTurnHold!.HoldId, f.PrivateOffer.Stage, 1, f.PrivateOffer.SequenceBefore, after));
    }

    internal IReadOnlyList<CardSnapshot> DeferredPrivateOfferViewedCards(int viewer) => Array.AsReadOnly(_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.PrivateOffer is { Stage: PrivateOfferStage.ChoosingExchange } r && r.Location.PrivateTurnHold!.DeferredOffer!.TargetSeat == viewer && _corePrivateOfferReturnsOperations.ValidPrivateOfferReceipt(f) && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { IsPrivate: true, Kind: DecisionKind.ProgramTrigger } p && p.PlayerSeat == viewer && p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "deferred-private-offer" && c.Parameters.GetValueOrDefault("frame-id") == f.Id.ToString(CultureInfo.InvariantCulture))).Select(f => _coreSnapshotValueProjectionOperations.ToSnapshot(_cardZonesOperations._cardZones.CardsAt(f.PrivateOffer!.Location).Single(c => c.Id == f.PrivateOffer.CardId))).ToArray());
    [ProgramIssuedTrigger(700, ProgramIssuedTriggerMatch.Decision)]
    internal bool? DecideIssuedTrigger700(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsDeferredPrivateOfferResolver(candidate) ? CanRunDeferredPrivateOffer(candidate, context) : null;
}
