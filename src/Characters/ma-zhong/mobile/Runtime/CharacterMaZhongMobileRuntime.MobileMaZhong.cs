#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaZhongMobileRuntime
{
    internal const string MobileFuman = "mobile:fuman", MobileFumanActivation = "give-physical-slash", MobileFumanReward = "gifted-slash-used";
    internal const string MobileFumanGiftReason = "skill-program.mobile:fuman.MoveBoundCards";
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 78800, SkillProgramEffectOp.PrepareMobileMaZhongFuman)]
    internal bool CanOfferMobileFuman(ProgramActivationLegalityContext gate) => _corePhysicalOwnedCardQueriesOperations.GetHand(gate.Owner).Any(card => card.Kind is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash);
    void global::CardGame.Core.IMobileMaZhongHost.Prepare(ProgramSkillFrame frame) => PrepareMobileMaZhongFuman(frame);
    internal void PrepareMobileMaZhongFuman(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var binding = _coreProgramLifecycleOperations.GetProgramCardSet(frame, "fuman-gift");
        if (frame.SkillId != MobileFuman || frame.ActivationId != MobileFumanActivation || frame.TriggerId is not null || frame.WindowContext is not null || frame.InstructionIndex != 3 || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || frame.SelectedCardIds.Count != 0 || binding.CardIds is not [var id] || !binding.SourceLocations.SequenceEqual([CardLocation.Hand(frame.OwnerSeat)]) || binding.Visibility != SkillProgramCardSetVisibility.Private || frame.SelectedTargetSeats is not [var recipient] || recipient == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId) || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(frame.OwnerSeat) || _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind is not (CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash) || _skillRuntimeStateOperations._skillRuntimeState.GetUsage(frame.OwnerSeat, frame.SkillId, $"{(MobileFumanActivation)}:target:{(recipient)}", SkillUsageScope.Phase) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileMaZhongFumanPreparedEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("Mobile Fuman requires its original actual-Play quota, physical hand Slash and other living recipient.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileMaZhongFumanPreparedEvent(frame.Id, new(frame.SkillId, frame.ActivationId, frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, recipient, id, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
    }

    internal CardMovementRecord? MobileFumanGiftMovement(MobileMaZhongFumanPreparedEvent prepared) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.FirstOrDefault(move => move.CardId == prepared.CardId && move.Sequence > prepared.Before && move.From == CardLocation.Hand(prepared.Source.OwnerSeat) && move.To == CardLocation.Hand(prepared.RecipientSeat) && move.CardKind == prepared.CardKind && move.TurnNumber == prepared.TurnNumber && move.Reason.Value == MobileFumanGiftReason);
    internal bool MobileFumanGiftPaid(ProgramSkillFrame frame)
    {
        if (frame.SkillId != MobileFuman || frame.TriggerId is not null || frame.ActivationId != MobileFumanActivation || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileMaZhongFumanPreparedEvent>().Where(e => e.FrameId == frame.Id).ToArray()is not [var prepared] || prepared.Source != new CardConversionSource(frame.SkillId, frame.ActivationId, frame.OwnerSeat, frame.SkillInstanceId) || prepared.Hash != frame.GameplayHash || frame.SelectedCardIds.Count != 0 || frame.InstructionIndex != 4 || frame.CardSetBindings.SingleOrDefault(binding => binding.Name == "fuman-gift")is not { } binding || !binding.CardIds.SequenceEqual([prepared.CardId]) || !binding.SourceLocations.SequenceEqual([CardLocation.Hand(frame.OwnerSeat)]) || !frame.SelectedTargetSeats.SequenceEqual([prepared.RecipientSeat]) || MobileFumanGiftMovement(prepared)is null || binding.Visibility != SkillProgramCardSetVisibility.Private)
            return false;
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.PaidInitialSelection, 78800)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 78800)]
    internal bool KeepMobileFumanPaidGift(ProgramSkillFrame frame) => MobileFumanGiftPaid(frame);
    internal bool MobileFumanPreparedSourceValid(MobileMaZhongFumanPreparedEvent prepared) => prepared.Source.SkillId == MobileFuman && prepared.Source.BindingId == MobileFumanActivation && _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(MobileFuman)?.Program?.GameplayHash == prepared.Hash && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == prepared.FrameId && e.OwnerSeat == prepared.Source.OwnerSeat && e.SkillId == MobileFuman && e.ActivationId == MobileFumanActivation) == 1;
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 78800)]
    internal void CollectMobileFumanUse(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> candidates)
    {
        if (window != SkillProgramTriggerWindow.CardUseCommitted || action.Type != CardActionType.Use || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || action.ActorSeat != action.ProviderSeat || action.PhysicalCards is not [var material] || material.From != CardLocation.Hand(action.ActorSeat) || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame use || use.Action?.ActionId != action.ActionId)
            return;
        var payment = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(move => move.CardId == material.CardId && move.From == material.From && move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use);
        if (payment is null)
            return;
        foreach (var prepared in _coreProgramEventHistoryOperations.ProgramEventHistory<MobileMaZhongFumanPreparedEvent>().Where(e => e.CardId == material.CardId && e.RecipientSeat == action.ActorSeat && _coreMatchParticipantsPlayersAccess._players[e.Source.OwnerSeat].IsAlive))
        {
            if (!MobileFumanPreparedSourceValid(prepared) || MobileFumanGiftMovement(prepared)is not { } gift || gift.Sequence >= payment.Sequence || _coreProgramEventHistoryOperations.ProgramEventHistory<TurnEndedEvent>().Any(e => e.ActorSeat == prepared.RecipientSeat && e.TurnNumber > prepared.TurnNumber) || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.CardId == prepared.CardId && move.Sequence > gift.Sequence && move.Sequence < payment.Sequence && move.From == CardLocation.Hand(prepared.RecipientSeat)))
                continue;
            var entitlement = new MobileMaZhongFumanUseEntitledEvent(prepared.FrameId, use.Id, action.ActionId, prepared.Source, prepared.Hash, prepared.RecipientSeat, prepared.CardId, payment.Sequence);
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MobileMaZhongFumanUseEntitledEvent>().Contains(entitlement))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(entitlement);
            var source = prepared.Source;
            var program = _contentRegistryOperations._contentRegistry.GetSkill(MobileFuman).Program!;
            var trigger = program.Triggers.Single(t => t.Id == MobileFumanReward);
            if (candidates.Any(c => c.OwnerSeat == source.OwnerSeat && c.SkillId == MobileFuman && c.SkillInstanceId == source.SkillInstanceId && c.TriggerId == MobileFumanReward))
                continue;
            var target = action.TargetSeats.Count == 1 ? action.TargetSeats.Single() : -1;
            var facts = _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[source.OwnerSeat], action);
            candidates.Add(new(source.OwnerSeat, target, MobileFuman, MobileFumanReward, program.GameplayHash, source.SkillInstanceId, trigger.Priority, _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, source.OwnerSeat, target, facts)));
        }
    }

    internal bool ExactMobileFumanUseCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != MobileFuman || candidate.BindingId != MobileFumanReward || context is not { Window: SkillProgramTriggerWindow.CardUseCommitted, CardUse: { } card } || context.OwnerSeat != candidate.OwnerSeat || _contentRegistryOperations._contentRegistry.GetSkill(MobileFuman).Program?.GameplayHash != candidate.GameplayHash || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || window.ParentFrameId != card.ParentCardUseFrameId || window.Action.ActionId != card.CardActionId || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Id == window.ParentFrameId)is not { Action: { Type: CardActionType.Use } action } use || action.ActionId != card.CardActionId || action.ActorSeat != card.ActorSeat || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || action.ActorSeat != action.ProviderSeat || action.PhysicalCards is not [var material] || material.From != CardLocation.Hand(action.ActorSeat) || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count)
            return false;
        var current = window.Candidates[window.CandidateIndex];
        if (current.OwnerSeat != candidate.OwnerSeat || current.SkillId != candidate.SkillId || current.TriggerId != candidate.BindingId || current.SkillInstanceId != candidate.SkillInstanceId || current.GameplayHash != candidate.GameplayHash || _coreCardActionsOperations.CreateCardActionProgramContext(window, current) != context)
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<MobileMaZhongFumanUseEntitledEvent>().Any(e => e.UseFrameId == use.Id && e.ActionId == action.ActionId && e.Source.OwnerSeat == candidate.OwnerSeat && e.Source.SkillId == candidate.SkillId && e.Source.SkillInstanceId == candidate.SkillInstanceId && e.Hash == candidate.GameplayHash && e.RecipientSeat == action.ActorSeat && e.CardId == material.CardId && _coreProgramEventHistoryOperations.ProgramEventHistory<MobileMaZhongFumanPreparedEvent>().Any(p => p.FrameId == e.GiftFrameId && p.Source == e.Source && p.Hash == e.Hash && MobileFumanPreparedSourceValid(p) && MobileFumanGiftMovement(p)is { } gift && gift.Sequence < e.PaymentSequence));
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 78800)]
    internal bool AdmitIssuedMobileFumanUse(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => ExactMobileFumanUseCandidate(candidate, context) && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 78800, SkillProgramEffectOp.Draw)]
    internal bool QualifyMobileFumanUse(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => candidate.SkillId != MobileFuman || ExactMobileFumanUseCandidate(candidate, context);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 78800)]
    internal bool KeepIssuedMobileFuman(ProgramSkillFrame frame) => MobileFumanGiftPaid(frame) || frame is { SkillId: MobileFuman, TriggerId: MobileFumanReward, WindowContext: { } context } && ExactMobileFumanUseCandidate(new(frame.OwnerSeat, frame.SkillId, frame.TriggerId, frame.SkillInstanceId, frame.GameplayHash, 0), context);
}
