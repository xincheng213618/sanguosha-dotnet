#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFeiYiOlRuntime
{
    internal bool IsFeiYiSingletonTrigger(SkillProgramTrigger t) => t.Effects is [{ Op: SkillProgramEffectOp.RevealSingletonAndArmRankEffects }];
    internal CardMovementBatchContext CaptureFeiYiSingletonBatch(CardMovementBatchContext batch)
    {
        if (!HasFeiYiHezhong)
            return batch;
        var transitions = batch.SourceCounts.Concat(batch.DestinationCounts ?? []).Where(c => c.Location.Zone == CardZoneKind.Hand && c.Location.OwnerSeat is not null).GroupBy(c => c.Location.OwnerSeat!.Value).Select(group =>
        {
            var counts = group.Distinct().ToArray();
            if (counts.Length != 1)
                throw new InvalidOperationException("One atomic hand operation cannot have contradictory singleton counts.");
            var c = counts[0];
            var owner = group.Key;
            return c.CountBefore != 1 && c.CountAfter == 1 && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner])is [var card] ? new FeiYiSingletonTransition(owner, c.CountBefore, c.CountAfter, card.Id, card.Rank) : null;
        }).OfType<FeiYiSingletonTransition>().ToArray();
        return transitions.Length == 0 ? batch : batch with
        {
            FeiYiSingletons = transitions
        };
    }

    internal IEnumerable<ProgramTriggerCandidate> CollectFeiYiSingletonCandidates(CardMovementBatchContext batch)
    {
        foreach (var t in (batch.FeiYiSingletons ?? []).Where(t => FeiYiSingletonIsWholeOperation(batch, t)))
            foreach (var c in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[t.OwnerSeat], SkillProgramTriggerWindow.CardsMoved).Where(c => IsFeiYiSingletonTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(c))))
                yield return c;
    }

    internal bool FeiYiSingletonIsWholeOperation(CardMovementBatchContext batch, FeiYiSingletonTransition transition) => !batch.Movements.Where(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(transition.OwnerSeat)).Any(m => _coreProgramEventHistoryOperations.ProgramEventHistory<NativeDrawInvocationRecordedEvent>().Any(draw => draw.OwnerSeat == transition.OwnerSeat && draw.ActualCount > 1 && draw.TurnNumber == batch.TurnNumber && draw.ParentFrameId == batch.ParentFrameId && draw.Reason == m.Reason && m.Sequence > draw.SequenceBefore && m.Sequence <= draw.SequenceAfter));
    internal ProgramSkillWindowContext CreateFeiYiSingletonContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate c) => new(SkillProgramTriggerWindow.CardsMoved, frame.Id, c.OwnerSeat, SourceSeat: c.OwnerSeat, TargetSeat: c.OwnerSeat, OccurrenceIndex: c.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]), MovementBatch: frame.Batch);
    internal FeiYiSingletonTransition? CurrentFeiYiSingleton(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!IsFeiYiSingletonTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(c)) || context.Window != SkillProgramTriggerWindow.CardsMoved || context.OwnerSeat != c.OwnerSeat || context.OccurrenceIndex != c.OccurrenceIndex || context.MovementBatch is not { } batch || batch.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] != c || window.Batch.Id != batch.Id || !window.Batch.Movements.SequenceEqual(batch.Movements) || !(window.Batch.FeiYiSingletons ?? []).SequenceEqual(batch.FeiYiSingletons ?? []) || batch.FeiYiSingletons?.SingleOrDefault(t => t.OwnerSeat == c.OwnerSeat)is not { } transition || transition.Before == 1 || transition.After != 1 || !FeiYiSingletonIsWholeOperation(batch, transition) || !_cardZonesOperations._cardZones.Contains(CardLocation.Hand(c.OwnerSeat), transition.CardId) || !FeiYiDirectionAvailable(c.OwnerSeat, true) && !FeiYiDirectionAvailable(c.OwnerSeat, false))
            return null;
        return transition;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26600)]
    internal bool CanOfferFeiYiSingleton(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => !IsFeiYiSingletonTrigger(t) || CurrentFeiYiSingleton(c, context)is not null;
    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 2400)]
    internal void CollectWindow_CollectFeiYiSingletonCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectFeiYiSingletonCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 3300)]
    internal bool ExcludeOrdinaryMovement_IsFeiYiSingletonTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsFeiYiSingletonTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 2800)]
    internal ProgramSkillWindowContext? TryCreateFeiYiSingletonContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsFeiYiSingletonTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(candidate)))
            return CreateFeiYiSingletonContext(frame, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 1400)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureFeiYiSingletonBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => CaptureFeiYiSingletonBatch(completed);
    [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 120, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveFeiYiMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsFeiYiSingletonTrigger(trigger) ? CurrentFeiYiSingleton(candidate, context)is not null : null;
}
