#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePublicPileColorsRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IPublicPileColorHost.ExecutePublicPileColor(SkillProgramEffect e, ProgramSkillFrame f) => ExecutePublicPileColor(e, f);
    internal IEnumerable<PublicPersistentPileSource> SameGrantColorPiles(int seat, string sourceSkill, string consumerInstance)
    {
        var consumer = _coreMatchParticipantsPlayersAccess._players[seat].SkillGrants.Grants.FirstOrDefault(g => g.SkillInstanceId == consumerInstance);
        return _corePublicPileSourcesOperations.ReferencedPublicPileSources(seat, sourceSkill, consumerInstance).Where(s => s.SkillInstanceId == consumerInstance || consumer is not null && _coreMatchParticipantsPlayersAccess._players[seat].SkillGrants.Grants.Any(g => g.SkillId == s.SkillId && g.SkillInstanceId == s.SkillInstanceId && g.SourceId == consumer.SourceId)).Where(s => _corePublicPileSourcesOperations.PublicPileCards(s).Count == 1);
    }

    internal static bool? SuitColor(Suit? suit) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_PublicPileColors.SuitColor(suit);
    internal bool? PileColor(PublicPersistentPileSource s) => SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[s.OwnerSeat], _corePublicPileSourcesOperations.PublicPileCards(s).Single()));
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 120)]
    internal bool CanActivatePublicPileColor(CharacterState owner, string skill, ProgramInstructionFeatures features)
    {
        if (!features.HasOperation(SkillProgramEffectOp.PublicPileColorDamage))
            return true;
        if (_coreProgramConversionPolarityOperations.GetProgramConversionPolarity(owner.Seat, skill) != SkillPolarity.Yang)
            return false;
        var effect = features.First(SkillProgramEffectOp.PublicPileColorDamage)!;
        return _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Where(i => i.SkillId == skill).Any(i => effect.PublicPileDamageCostPolicy is not null ? _characterLuZhiMobileOperations.HpDifferencePileDamageTargets(owner.Seat, skill, i.SkillInstanceId, effect).Count > 0 : ColorDamageChoices(0, owner.Seat, skill, i.SkillInstanceId, effect).Count > 0);
    }

    internal IReadOnlyList<PromptChoice> ColorDamageChoices(long frameId, int owner, string skill, string instance, SkillProgramEffect e)
    {
        var result = new List<PromptChoice>();
        foreach (var pile in SameGrantColorPiles(owner, e.SkillIds.Single(), instance))
            foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[owner])).Where(c => SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[owner], c))is { } color && color == PileColor(pile) && !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner, skill, instance, c)))
                foreach (var target in _coreProgramLifecycleOperations.GetProgramTargetSeats(owner, SkillProgramTargetKind.OtherLivingInAttackRange))
                    result.Add(new(new($"public-pile-color.frame-{(frameId)}.pile-{(global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime.PublicPileIdentity(pile.SkillId, pile.SkillInstanceId))}.card-{(card.Id)}.target-{(target)}"), "弃置【" + global::CardGame.Core.EngineOwners.CoreGameEnginePublicPersistentPilesRuntime.PublicPileCardLabel(card) + "】，对 " + _coreMatchParticipantsPlayersAccess._players[target].Name + " 造成1点伤害。", [card.Id], [target], new Dictionary<string, string> { ["program-action"] = "public-pile-color", ["frame-id"] = frameId.ToString(), ["pile-instance"] = pile.SkillInstanceId }));
        return result;
    }

    internal ActionCardsDiscardedEvent? ColorActionFact(ProgramSkillWindowContext c) => c.MovementBatch is { } b ? _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ActionCardsDiscardedEvent>().LastOrDefault(e => e.BatchId == b.Id) : null;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeAdvancedPrerequisites, 30)]
    internal bool CanOfferPublicPileColor(CharacterState owner, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.RewardDiscardedActionColor))
            return true;
        var e = trigger.Effects.Single(x => x.Op == SkillProgramEffectOp.RewardDiscardedActionColor);
        var fact = ColorActionFact(context);
        return fact is { IsRed: { } } && fact.ActorSeat == owner.Seat && fact.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && owner.Seat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && _coreProgramConversionPolarityOperations.GetProgramConversionPolarity(owner.Seat, candidate.SkillId) == SkillPolarity.Yin && SameGrantColorPiles(owner.Seat, e.SkillIds.Single(), candidate.SkillInstanceId).Any(p => PileColor(p) == fact.IsRed) && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ActionColorRewardOfferedEvent>().Any(x => x.OwnerSeat == owner.Seat && x.SkillId == candidate.SkillId && x.ActionId == fact.ActionId && (x.ParentFrameId != context.ParentFrameId || x.SkillInstanceId != candidate.SkillInstanceId));
    }

    internal void RecordPublicPileColorOffer(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (_coreProgramLifecycleOperations.GetProgramTrigger(candidate).Effects.Any(e => e.Op == SkillProgramEffectOp.RewardDiscardedActionColor) && ColorActionFact(context)is { } fact && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ActionColorRewardOfferedEvent>().Any(e => e.OwnerSeat == candidate.OwnerSeat && e.SkillId == candidate.SkillId && e.ActionId == fact.ActionId))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ActionColorRewardOfferedEvent(candidate.OwnerSeat, candidate.SkillId, candidate.SkillInstanceId, context.ParentFrameId, fact.ActionId));
    }

    internal SkillProgramStepOutcome ExecutePublicPileColor(SkillProgramEffect e, ProgramSkillFrame frame)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        if (e.Op == SkillProgramEffectOp.PublicPileColorDamage && e.PublicPileDamageCostPolicy is not null)
            return _characterLuZhiMobileOperations.StartHpDifferencePileDamage(frame, e);
        if (e.Op == SkillProgramEffectOp.StoreBoundHandInPublicPile)
        {
            var cards = _coreProgramLifecycleOperations.GetProgramCardSet(frame, e.SourceBind!);
            if (cards.CardIds.Count == 0)
                return SkillProgramStepOutcome.Continue;
            var id = cards.CardIds.Single();
            if (_cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(frame.OwnerSeat))
                throw new InvalidOperationException("Initial public reserve requires a current owned hand entity.");
            var source = _corePublicPileSourcesOperations.EnsurePublicPileSource(frame, 1);
            if (_corePublicPileSourcesOperations.PublicPileCards(source).Count != 0)
                throw new InvalidOperationException("Initial reserve is already occupied.");
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
            _corePhysicalMovementFacadesOperations.MoveCard(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Single(c => c.Id == id), CardLocation.Hand(frame.OwnerSeat), source.Location, new("skill-program.public-pile.initial-hand"), disclosure: CardMovementDisclosure.FaceUp);
            if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
                _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        IReadOnlyList<PromptChoice> choices;
        if (e.Op == SkillProgramEffectOp.PublicPileColorDamage)
            choices = ColorDamageChoices(frame.Id, frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, e);
        else
        {
            var fact = ColorActionFact(frame.WindowContext!) ?? throw new InvalidOperationException("Action color reward lost its actual discard batch.");
            choices = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => new PromptChoice(new($"public-pile-color.frame-{(frame.Id)}.target-{(p.Seat)}"), "令 " + p.Name + " 摸一张牌。", [], [p.Seat], new Dictionary<string, string> { ["program-action"] = "public-pile-color", ["frame-id"] = frame.Id.ToString() })).ToArray();
        }

        if (choices.Count == 0)
            return SkillProgramStepOutcome.Continue;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, skill.Description, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = e.Op == SkillProgramEffectOp.PublicPileColorDamage,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("public-pile-color")]
    internal void ResolvePublicPileColorChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Color choice lost its program frame.");
        if (choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString() || choice.Targets.Count != 1)
            throw new InvalidOperationException("Color choice lost its exact cursor.");
        var e = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
        if (e.Op == SkillProgramEffectOp.PublicPileColorDamage)
        {
            if (_coreProgramConversionPolarityOperations.GetProgramConversionPolarity(f.OwnerSeat, f.SkillId) != SkillPolarity.Yang || !ColorDamageChoices(f.Id, f.OwnerSeat, f.SkillId, f.SkillInstanceId, e).Any(c => c.Id == choice.Id))
                throw new InvalidOperationException("Color cost no longer qualifies.");
            var source = SameGrantColorPiles(f.OwnerSeat, e.SkillIds.Single(), f.SkillInstanceId).Single(s => s.SkillInstanceId == choice.Parameters["pile-instance"]);
            var id = choice.Cards.Single();
            var at = _cardZonesOperations._cardZones.GetLocation(id);
            var card = _cardZonesOperations._cardZones.CardsAt(at).Single(c => c.Id == id);
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramConversionPolarityOperations.CommitProgramConversionPolarity(f);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PublicPileColorPayment = new(id, at, source.Location, source.SkillInstanceId, PileColor(source)!.Value, choice.Targets.Single()), PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _corePhysicalMovementFacadesOperations.MoveCard(card, at, CardLocation.DiscardPile, new("skill-program.public-pile.color-payment"), disclosure: CardMovementDisclosure.FaceUp);
            if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
                _coreRuntimeOperations.ReturnRuntimeProgramMovement(f.Id);
            return;
        }

        if (e.Op != SkillProgramEffectOp.RewardDiscardedActionColor || _coreProgramConversionPolarityOperations.GetProgramConversionPolarity(f.OwnerSeat, f.SkillId) != SkillPolarity.Yin || !_coreMatchParticipantsPlayersAccess._players[choice.Targets.Single()].IsAlive)
            throw new InvalidOperationException("Color reward lost its polarity or recipient.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreProgramConversionPolarityOperations.CommitProgramConversionPolarity(f);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreProgramLifecycleOperations.DrawProgramCards(f.Id, choice.Targets.Single(), 1, null, null, SkillProgramCardSetVisibility.Private, new("skill-program.public-pile.action-color.draw"));
        // Draw recipient can differ from owner; generic draw already retains its continuation.
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 940)]
    internal bool ResumePublicPileColorPayment(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { PublicPileColorPayment: { } } f || f.Id != id || f.PendingMovementContinuation is not null)
            return false;
        var payment = f.PublicPileColorPayment!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PublicPileColorPayment = null });
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[payment.TargetSeat].IsAlive)
        {
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        _coreSkillProgramsOperations.BeginProgramSkillDamage(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), payment.TargetSeat, 1, new ProgramParticipantReference(ProgramParticipantRef.Owner));
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.PublicPileColorDamage, SkillProgramEffectOp.RewardDiscardedActionColor)]
    internal PromptChoice SelectAiPublicPileColorChoice(PendingDecision p, ProgramSkillFrame f) => f.HpDifferencePileDamage is not null ? p.Choices.OrderByDescending(c => c.Targets.Count == 1 && !global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets[0]])).ThenBy(c => c.Cards.Count == 1 ? global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) : 0).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First() : p.Choices.OrderByDescending(c => c.Targets.Count == 1 && (c.Cards.Count == 0 ? global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets[0]]) : !global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets[0]]))).ThenBy(c => c.Cards.Count == 1 ? global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(c.Cards[0])).Single(x => x.Id == c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) : 0).ThenBy(c => c.Targets[0]).First();
}
