#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoZhongOlRuntime
{
    internal bool IsBiriTrigger(SkillProgramTrigger t) => t.Effects is [{ Op: SkillProgramEffectOp.ResolveBiriFirstNonDrawGain }];
    internal void CaptureBiriPotentialGainBatch(CardMovementBatchContext batch)
    {
        if (!HasZhaoZhongCapability || !ZhaoZhongActualTurnOpen || batch.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber)
            return;
        var gains = batch.Movements.Where(m => m.To.Zone == CardZoneKind.Hand && m.To.OwnerSeat is { } owner && _coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) && _coreActualHandGainProgramsOperations.ActualHandGainOriginalSource(m, batch) != CardLocation.Hand(owner)).GroupBy(m => (m.CardId, m.To.OwnerSeat)).Select(g => g.OrderBy(m => m.Sequence).Last()).OrderBy(m => m.Sequence).ToArray();
        if (gains.Length == 0)
            return;
        foreach (var m in gains)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BiriPotentialGainEvent(batch.Id, m.Sequence, m.CardId, m.To.OwnerSeat!.Value, batch.TurnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat));
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, p.Seat)))
        {
            var source = _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.SelectMany(i => i.Program.Triggers.Where(IsBiriTrigger).Select(t => (Source: new CardConversionSource(i.SkillId, t.Id, owner.Seat, i.SkillInstanceId), Hash: i.Program.GameplayHash))).OrderBy(s => s.Source.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault();
            if (source.Source is not null)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BiriPotentialHolderEvent(batch.Id, source.Source, source.Hash));
        }
    }

    internal BiriPotentialGainEvent? FirstBiriGain(int turn, int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<BiriPotentialGainEvent>().Where(e => e.ActualTurn == turn && e.TurnOwner == owner).OrderBy(e => e.Sequence).FirstOrDefault(e => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.SingleOrDefault(m => m.Sequence == e.Sequence)is { } m && m.CardId == e.CardId && m.To == CardLocation.Hand(e.RecipientSeat) && !_coreZhaoZhongProvenanceOperations.IsNativeDrawGainMovement(m));
    internal IReadOnlyList<ProgramTriggerCandidate> CollectBiriMovementCandidates(CardMovementBatchContext batch)
    {
        // Most classic tables have no current Biri holder, and an already paid
        // holder has nothing to collect. Avoid rebuilding every earlier native
        // draw's proof before discovering either case. Potential gain facts are
        // still captured, so a later skill grant cannot retroclaim this turn.
        var holders = _coreProgramEventHistoryOperations.ProgramEventHistory<BiriPotentialHolderEvent>().Where(e => e.BatchId == batch.Id && _coreMatchParticipantsPlayersAccess._players[e.Source.OwnerSeat].IsAlive).ToArray();
        if (holders.Length == 0)
            return[];
        var potential = _coreProgramEventHistoryOperations.ProgramEventHistory<BiriPotentialGainEvent>().FirstOrDefault(e => e.BatchId == batch.Id);
        if (potential is null)
            return[];
        var issued = _coreProgramEventHistoryOperations.ProgramEventHistory<BiriDrawIssuedEvent>();
        var eligible = holders.Where(e => !issued.Any(p => p.Source.OwnerSeat == e.Source.OwnerSeat && p.ActualTurn == potential.ActualTurn && p.TurnOwner == potential.TurnOwner)).ToArray();
        if (eligible.Length == 0 || FirstBiriGain(potential.ActualTurn, potential.TurnOwner)is not { } first || first.BatchId != batch.Id)
            return[];
        return Array.AsReadOnly(eligible.Select(e => new ProgramTriggerCandidate(e.Source.OwnerSeat, e.Source.SkillId, e.Source.BindingId, e.Source.SkillInstanceId, e.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(new ProgramTriggerCandidate(e.Source.OwnerSeat, e.Source.SkillId, e.Source.BindingId, e.Source.SkillInstanceId, e.GameplayHash, 0)).Priority)).ToArray());
    }

    internal ProgramSkillWindowContext CreateBiriMovementContext(CardsMovedTriggerWindowFrame w, ProgramTriggerCandidate c) => new(SkillProgramTriggerWindow.CardsGained, w.Id, c.OwnerSeat, SourceSeat: w.Batch.OriginOwnerSeat, TargetSeat: c.OwnerSeat, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]), MovementBatch: w.Batch, OccurrenceIndex: c.OccurrenceIndex);
    [ProgramIssuedTrigger(170)]
    internal bool IsIssuedBiriCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!IsBiriTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(c)) || context.Window != SkillProgramTriggerWindow.CardsGained || context.OwnerSeat != c.OwnerSeat || context.MovementBatch is not { } batch || context.ParentFrameId != batch.Id || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == batch.Id)is not CardsMovedTriggerWindowFrame w || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || w.Candidates[w.CandidateIndex] != c || !_coreProgramEventHistoryOperations.ProgramEventHistory<BiriPotentialHolderEvent>().Any(e => e.BatchId == batch.Id && e.Source == new CardConversionSource(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId) && e.GameplayHash == c.GameplayHash))
            return false;
        var potential = _coreProgramEventHistoryOperations.ProgramEventHistory<BiriPotentialGainEvent>().FirstOrDefault(e => e.BatchId == batch.Id);
        return potential is not null && FirstBiriGain(potential.ActualTurn, potential.TurnOwner)is { } first && first.BatchId == batch.Id && !_coreProgramEventHistoryOperations.ProgramEventHistory<BiriDrawIssuedEvent>().Any(e => e.Source.OwnerSeat == c.OwnerSeat && e.ActualTurn == first.ActualTurn && e.TurnOwner == first.TurnOwner);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 1600)]
    internal void CollectWindow_CollectBiriMovementCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectBiriMovementCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 2300)]
    internal bool ExcludeOrdinaryMovement_IsBiriTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsBiriTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 1500)]
    internal ProgramSkillWindowContext? TryCreateBiriMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsBiriTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(candidate)))
            return CreateBiriMovementContext(frame, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 5100)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureBiriPotentialGainBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureBiriPotentialGainBatch(completed);
        return completed;
    }
}
