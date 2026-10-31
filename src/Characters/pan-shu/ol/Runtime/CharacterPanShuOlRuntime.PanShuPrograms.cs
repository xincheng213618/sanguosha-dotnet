#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPanShuOlRuntime
{
    internal int CountNonConvertedActualUsesThisTurn(int ownerSeat) => _corePlayCardHistoryOperations.EventsSinceLastBoundary(item => item is TurnStartedEvent or TurnEndedEvent).OfType<CardActionAcceptedEvent>().Count(item => item.Action.Type == CardActionType.Use && item.Action.ActorSeat == ownerSeat && item.Action.ConversionChain.Count == 0 && item.Action.PhysicalCards.Count > 0 && item.Action.PhysicalCards.All(cost => cost.CardKind == item.Action.EffectiveKind));
    internal static bool IsOtherLastHandLossTrigger(SkillProgramTrigger trigger) => global::CardGame.Core.RuntimeSkills.RuntimeRules_pan_shu_ol_PanShuPrograms.IsOtherLastHandLossTrigger(trigger);
    internal IEnumerable<ProgramTriggerCandidate> CollectOtherLastHandLossCandidates(CardMovementBatchContext batch)
    {
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive))
            foreach (var candidate in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.CardsMoved))
            {
                var trigger = _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
                if (!IsOtherLastHandLossTrigger(trigger))
                    continue;
                var indexes = batch.Movements.Select((movement, index) => (movement, index)).Where(item => item.movement.From.Zone == CardZoneKind.Hand && item.movement.From.OwnerSeat is { } source && source != candidate.OwnerSeat && source != item.movement.To.OwnerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(source) && _coreMatchParticipantsPlayersAccess._players[source].IsAlive && _coreActualTurnProgressCurrentSeatPhaseAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat == source && LostLastHandCard(batch, source)).Select(item => item.index).ToArray();
                foreach (var index in indexes)
                {
                    var sourceCount = _coreProgramMovementSourceCountsOperations.ProgramMovementSourceCounts(batch, SkillProgramTriggerWindow.CardsMoved).FirstOrDefault(item => item.Count.Location == batch.Movements[index].From).Count;
                    var facts = _coreCardMovementProgramsOperations.CaptureCardsMovedTriggerFacts(owner, 1, sourceCount ?? new CardMovementSourceCount(batch.Movements[index].From, 0, 1), SkillProgramTriggerWindow.CardsMoved, batch.MovementTiming);
                    if (trigger.Condition.Evaluate(facts, candidate.SkillId, candidate.SkillInstanceId))
                        yield return candidate with
                        {
                            OccurrenceIndex = index
                        };
                }
            }
    }

    internal static bool LostLastHandCard(CardMovementBatchContext batch, int sourceSeat) => global::CardGame.Core.RuntimeSkills.RuntimeRules_pan_shu_ol_PanShuPrograms.LostLastHandCard(batch, sourceSeat);
    internal IReadOnlySet<SkillProgramCardCategory> CardIdsCategorySet(IReadOnlyList<int> cardIds)
    {
        var categories = new HashSet<SkillProgramCardCategory>();
        foreach (var cardId in cardIds)
        {
            var location = _cardZonesOperations._cardZones.GetLocation(cardId);
            if (location.OwnerSeat is null)
                continue;
            var card = _cardZonesOperations._cardZones.CardsAt(location).Single(item => item.Id == cardId);
            categories.Add(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(card.Kind));
        }

        return categories;
    }

    internal void SetForeignProgramBooleanState(ProgramSkillFrame frame, string targetSkillId, string stateId, bool value)
    {
        var instance = _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).ProgramInstances.SingleOrDefault(item => item.SkillId == targetSkillId) ?? throw new InvalidOperationException($"The paired program instance '{(targetSkillId)}' is not installed.");
        var definition = _coreProgramPersistentStateOperations.GetProgramBooleanStateDefinition2(targetSkillId, stateId);
        var key = new global::CardGame.Core.GameEngine.ProgramBooleanStateKey(frame.OwnerSeat, targetSkillId, instance.SkillInstanceId, stateId);
        if (_coreProgramPersistentStateOperations.GetProgramBooleanState2(frame.OwnerSeat, targetSkillId, instance.SkillInstanceId, stateId) == value)
            return;
        _coreGameEngineProgramPersistentStateProgramBooleanStatesAccess._programBooleanStates[key] = value;
        if (definition.Visibility == SkillProgramStateVisibility.Public)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramBooleanStateChangedEvent(frame.OwnerSeat, targetSkillId, instance.SkillInstanceId, stateId, value, definition.Visibility));
    }

    internal void ApplyProgramSkillRecovery(long frameId, int sourceSeat, int targetSeat)
    {
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        if (!target.IsAlive || !target.HasHpValue)
            return;
        var recovered = Math.Min(1, target.MaxHp - target.Hp);
        if (recovered <= 0)
            return;
        if (_coreRecoveryReplacementOperations.TryQueueRecoveryReplacement(frameId, sourceSeat, targetSeat, recovered, new(RecoveryAttemptProducer.Program, _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId).InstructionIndex)))
            return;
        var recovery = _coreNativeRecoveryFramesOperations.BeginRecovery(frameId, sourceSeat, targetSeat, recovered);
        target.Hp += recovered;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecoveryAppliedEvent(sourceSeat, targetSeat, recovered, target.Hp));
        _coreRuntimeOperations.PopResolutionFrame(recovery, ResolutionFrameKind.Recovery);
    }

    SkillProgramStepOutcome global::CardGame.Core.IPanShuProgramHost.ZhirenReorderTopByLength(ProgramSkillFrame frame, SkillProgramEffect effect) => ZhirenProgramReorderTopByLength(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.IPanShuProgramHost.ZhirenResolveFieldTiers(ProgramSkillFrame frame, SkillProgramEffect effect) => ZhirenProgramResolveFieldTiers(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.IPanShuProgramHost.YanerResolvePairBenefit(ProgramSkillFrame frame, SkillProgramEffect effect) => YanerProgramResolvePairBenefit(frame, effect);
    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 1300)]
    internal void CollectWindow_CollectOtherLastHandLossCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectOtherLastHandLossCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 2900)]
    internal bool ExcludeOrdinaryMovement_IsOtherLastHandLossTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsOtherLastHandLossTrigger(trigger);
    [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 260, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveOtherLastHandMovement(SkillProgramTrigger trigger, ProgramSkillWindowContext context, CardMovementBatchContext batch, CharacterState owner) => IsOtherLastHandLossTrigger(trigger) ? context.SourceSeat is { } loserSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(loserSeat) && loserSeat != owner.Seat && _coreMatchParticipantsPlayersAccess._players[loserSeat].IsAlive && _coreActualTurnProgressCurrentSeatPhaseAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat == loserSeat && LostLastHandCard(batch, loserSeat) : null;
}
