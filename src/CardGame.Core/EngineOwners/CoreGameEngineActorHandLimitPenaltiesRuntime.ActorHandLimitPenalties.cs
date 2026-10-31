#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineActorHandLimitPenaltiesRuntime
{
    internal bool HasActorHandLimitPenaltyCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.GrantActorHandLimitPenalty);

    void global::CardGame.Core.IActorHandLimitPenaltyHost.GrantActorHandLimitPenalty(ProgramSkillFrame frame, int amount) => GrantProgramActorHandLimitPenalty(frame, amount);
    internal bool TryGetActorHandLimitPenaltyUse(int ownerSeat, ProgramSkillWindowContext context, out CardUseFrame use, out ProgramCardTriggerWindowFrame window)
    {
        use = null !;
        window = null !;
        if (context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } identity } || context.OwnerSeat != ownerSeat || context.TargetSeat != ownerSeat || identity.EventTargetSeat != ownerSeat || context.Facts?.CardActionTargetIsOwner != true || identity.ActorSeat == ownerSeat || context.SourceSeat != identity.ActorSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(identity.ActorSeat) || !_coreMatchParticipantsPlayersAccess._players[identity.ActorSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(item => item.Id == context.ParentFrameId)is not { } found || found.ParentFrameId != identity.ParentCardUseFrameId || found.CompletedResponseReturn is not null || found.ResponseCompletion is not null || found.CardSupplyCompletion is not null || found.Continuation is not (ProgramCardContinuation.Slash or ProgramCardContinuation.FinalizedTrick or ProgramCardContinuation.FinalizedSimpleCard) || global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.GetCardActionWindow(found) != context.Window || _coreCardUseLifecycleOperations.LifecycleCardUse(found.ParentFrameId)is not { Action: { Type: CardActionType.Use } action } actual || actual.ActorHandLimitAnnouncedTargets?.Contains(ownerSeat) != true || action.ActionId != identity.CardActionId || action.ActorSeat != identity.ActorSeat || actual.SourceSeat != identity.ActorSeat || action.EffectiveKind != actual.CardKind || identity.EffectiveKind != actual.CardKind || found.Action.Type != CardActionType.Use || found.Action.ActionId != action.ActionId || found.Action.ActorSeat != action.ActorSeat || found.Action.EffectiveKind != action.EffectiveKind || action.ResponderSeat is not null || action.OpponentSeat is not null || found.Action.ResponderSeat is not null || found.Action.OpponentSeat is not null || actual.DyingResponse is not null || global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(actual.CardKind) != SkillProgramCardCategory.Basic && !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(actual.CardKind) || action.PhysicalCards.Any(cost => cost.CardId <= 0) || action.PhysicalCards.DistinctBy(cost => cost.CardId).Count() != action.PhysicalCards.Count || !found.Action.PhysicalCards.Select(cost => cost.CardId).SequenceEqual(action.PhysicalCards.Select(cost => cost.CardId)))
            return false;
        var windowIndex = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == found.Id);
        if (windowIndex < 1 || _resolutionStackOperations._resolutionStack[windowIndex - 1].Id != actual.Id)
            return false;
        // The original designation window may retain its original target list
        // after a signed native target adjustment. Its exact frozen candidate
        // proves this owner's designation; only use identity and paid materials
        // must remain equal here.
        if (action.PhysicalCards.Count > 0)
        {
            if (_coreForeignPublicPileSlashesOperations.IsForeignPublicPileSlashUse(actual.Id))
            {
                if (actual.PhysicalCardIds is not { Count: 0 } || action.PhysicalCards.Any(cost => _cardZonesOperations._cardZones.GetLocation(cost.CardId) != CardLocation.DiscardPile))
                    return false;
            }
            else if (!action.PhysicalCards.Select(cost => cost.CardId).SequenceEqual(actual.PhysicalCardIds ?? [actual.CardId]) || action.PhysicalCards.Where(cost => !_coreCurrentUsePhysicalClaimsOperations.IsCurrentUsePhysicalCardClaim(actual.Id, cost.CardId) && !(_coreForeignSelectedCardSlashReturnsOperations.IsSelectedForeignCardSlashRemovedMaterial(actual.Id, cost.CardId) || _characterZhangYanOlOperations.IsZhangYanSujiRemovedMaterial(actual.Id, cost.CardId) || ((_coreKenshangPaymentsOperations.IsKenshangRemovedMaterial(actual.Id, cost.CardId) || _characterGuanHaiOlOperations.IsXieweiRemovedMaterial(actual.Id, cost.CardId)) || (_characterWuJingOlOperations.IsWuJingHejiRemovedMaterial(actual.Id, cost.CardId) || _characterSunYuanOlOperations.IsSayingRemovedMaterial(actual.Id, cost.CardId)))) && !_coreResponseEntityExchangeOperations.IsExchangedCardClaim(action.ActionId, cost.CardId)).Any(cost => _coreAlternativeViewAsCostsOperations.IsProgramAlternativeCost(action, cost.CardId) ? _cardZonesOperations._cardZones.GetLocation(cost.CardId) == CardLocation.Processing : _cardZonesOperations._cardZones.GetLocation(cost.CardId) != CardLocation.Processing))
                return false;
        }
        else if (actual.CardId != 0 || actual.PhysicalCardIds is not { Count: 0 } || !_coreChainedStateBasicReturnsOperations.IsChainedStateBasicUse(actual.Id) && !_coreTieredRoundZeroUsesOperations.IsTieredRoundZeroUse(actual.Id) && !_coreVirtualOrdinaryTrickUsesOperations.IsProgramVirtualOrdinaryTrickUse(actual.Id) && actual.VirtualBasicReturn is null && actual.CardAttack?.ProgramSkillCardUseFrameId is null && actual.AdjustedSlashReturn is null && !_coreDamageTargetObtainProgramsOperations.IsIssuedZeroEntityDuel(actual.Id))
            return false;
        use = actual;
        window = found;
        return true;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 830)]
    internal bool CanOfferActorHandLimitPenalty(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.GrantActorHandLimitPenalty))
            return true;
        ActorHandLimitPenaltyContract.ValidateTrigger(candidate.SkillId, trigger);
        if (!TryGetActorHandLimitPenaltyUse(candidate.OwnerSeat, context, out var use, out var window) || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(candidate, context, window) || trigger.Id != candidate.BindingId || _contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program is not { } program || program.GameplayHash != candidate.GameplayHash || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], candidate.SkillId, candidate.SkillInstanceId))
            return false;
        var source = new CardConversionSource(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId);
        return !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramActorHandLimitPenaltyGrantedEvent>().Any(issued => issued.Penalty.ActionId == use.Action!.ActionId && issued.Penalty.CardUseFrameId == use.Id && issued.Penalty.TargetSeat == candidate.OwnerSeat && issued.Penalty.Source.OwnerSeat == source.OwnerSeat && issued.Penalty.Source.SkillId == source.SkillId && issued.Penalty.Source.BindingId == source.BindingId);
    }

    internal void GrantProgramActorHandLimitPenalty(ProgramSkillFrame supplied, int amount)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame != supplied || frame.TriggerId is not { } binding || frame.WindowContext is not { } context || frame.InstructionIndex <= 0 || frame.PendingMovementContinuation is not null || frame.SelectedCardPayment is not null && frame.SelectedCardPaymentResult is null || amount <= 0 || !TryGetActorHandLimitPenaltyUse(frame.OwnerSeat, context, out var use, out var window) || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(new(frame.OwnerSeat, frame.SkillId, binding, frame.SkillInstanceId, frame.GameplayHash, 0), context, window) || !window.Activated || _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program is not { } program || program.GameplayHash != frame.GameplayHash)
            throw new InvalidOperationException("An actor hand-limit penalty lost its exact native use, frozen designated target, active candidate or instruction.");
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == frame.Id);
        if (index < 2 || _resolutionStackOperations._resolutionStack[index - 1].Id != window.Id || _resolutionStackOperations._resolutionStack[index - 2].Id != use.Id)
            throw new InvalidOperationException("An actor hand-limit penalty requires its immediate native designation parent.");
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger(frame);
        ActorHandLimitPenaltyContract.ValidateTrigger(frame.SkillId, trigger);
        var effectIndex = frame.InstructionIndex - 1;
        var effect = ProgramInstructionResolver.Default.Resolve(frame, program).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (effect.Op != SkillProgramEffectOp.GrantActorHandLimitPenalty || effect.Amount != amount || effect.Target != SkillProgramEffectTarget.Owner || effect.Condition.Kind != SkillProgramConditionKind.Always || effect.TargetReference is not null)
            throw new InvalidOperationException("An actor hand-limit penalty differs from its exact configured positive instruction.");
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory();
        if (history.OfType<ProgramBindingStartedEvent>().Count(started => started.FrameId == frame.Id && started.OwnerSeat == frame.OwnerSeat && started.SkillId == frame.SkillId && started.BindingId == binding && started.SkillInstanceId == frame.SkillInstanceId && started.Window == context.Window) != 1)
            throw new InvalidOperationException("An actor hand-limit penalty lost its original binding start.");
        var source = new CardConversionSource(frame.SkillId, binding, frame.OwnerSeat, frame.SkillInstanceId);
        var issued = history.OfType<ProgramActorHandLimitPenaltyGrantedEvent>().Where(item => item.Penalty.ProgramFrameId == frame.Id && item.Penalty.EffectIndex == effectIndex).ToArray();
        if (issued.Length != 0)
        {
            if (issued is not [var previous] || previous.Penalty.Source != source || previous.Penalty.GameplayHash != frame.GameplayHash || previous.Penalty.CardUseFrameId != use.Id || previous.Penalty.ActionId != use.Action!.ActionId || previous.Penalty.ActorSeat != context.CardUse!.ActorSeat || previous.Penalty.TargetSeat != frame.OwnerSeat || previous.Penalty.Amount != amount)
                throw new InvalidOperationException("An issued actor hand-limit penalty cannot be replaced or paid twice.");
            return;
        }

        if (_coreActualTurnProgressTurnNumberAccess._turnNumber <= 0 || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId) || history.OfType<ProgramActorHandLimitPenaltyGrantedEvent>().Any(item => item.Penalty.Source.OwnerSeat == source.OwnerSeat && item.Penalty.Source.SkillId == source.SkillId && item.Penalty.Source.BindingId == source.BindingId && item.Penalty.ActionId == use.Action!.ActionId && item.Penalty.CardUseFrameId == use.Id && item.Penalty.TargetSeat == frame.OwnerSeat))
            throw new InvalidOperationException("A native use can issue its exact target penalty only once from a live source instance.");
        var penalty = new ActorHandLimitPenalty(checked(++_coreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess._actorHandLimitPenaltySequence), frame.Id, effectIndex, source, frame.GameplayHash, use.Id, use.Action!.ActionId, context.CardUse!.ActorSeat, frame.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber, amount);
        _coreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess._actorHandLimitPenalties.Add(penalty);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramActorHandLimitPenaltyGrantedEvent(penalty));
    }

    internal IEnumerable<RuleQueryContribution> ActorHandLimitPenaltyContributions(CharacterState player)
    {
        foreach (var penalty in _coreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess._actorHandLimitPenalties.Where(item => item.ActorSeat == player.Seat))
            yield return new FiniteRuleQueryContribution($"actor-hand-limit-penalty:{(penalty.GrantSequence)}", SkillRuleOperation.Add, -penalty.Amount);
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 10)]
    internal void ExpireActorHandLimitPenalties(TurnEndedEvent ended)
    {
        if (!HasActorHandLimitPenaltyCapability || _coreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess._actorHandLimitPenalties.Count == 0 || !_coreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess._actorHandLimitPenalties.Any(item => item.ActorSeat == ended.ActorSeat))
            return;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var endIndex = Array.FindLastIndex(history, item => item is TurnEndedEvent actual && actual == ended);
        if (ended.TurnNumber <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(ended.ActorSeat) || endIndex < 0 || history.OfType<TurnEndedEvent>().LastOrDefault() != ended)
            throw new InvalidOperationException("An actor hand-limit penalty expires only after its exact actual turn-ended fact.");
        var due = _coreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess._actorHandLimitPenalties.Where(penalty => penalty.ActorSeat == ended.ActorSeat && Array.FindIndex(history, item => item is ProgramActorHandLimitPenaltyGrantedEvent grant && grant.Penalty == penalty)is var grantIndex && grantIndex >= 0 && grantIndex < endIndex).ToArray();
        foreach (var penalty in due)
        {
            _coreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess._actorHandLimitPenalties.Remove(penalty);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramActorHandLimitPenaltyExpiredEvent(penalty, ended.TurnNumber, ended.ActorSeat, "actor-turn-ended"));
        }
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 60)]
    internal void ExpireActorHandLimitPenaltiesAfterGameEnded(GameEndedEvent ended) => ExpireAllActorHandLimitPenalties("game-ended");
    internal void ExpireAllActorHandLimitPenalties(string reason)
    {
        if (!HasActorHandLimitPenaltyCapability || _coreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess._actorHandLimitPenalties.Count == 0)
            return;
        if (string.IsNullOrWhiteSpace(reason) || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GameEndedEvent>().Any())
            throw new InvalidOperationException("Terminal actor hand-limit cleanup requires a real game-ended fact and explicit reason.");
        foreach (var penalty in _coreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess._actorHandLimitPenalties.ToArray())
        {
            _coreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess._actorHandLimitPenalties.Remove(penalty);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramActorHandLimitPenaltyExpiredEvent(penalty, null, null, reason));
        }
    }
}
