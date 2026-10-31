#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoAngMouRuntime
{
    internal static bool IsMouCaFengmin(SkillProgramTrigger t) => t.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveMouCaoAngFengmin);
    internal IEnumerable<ProgramTriggerCandidate> CollectMouCaEquipmentCandidates(CardMovementBatchContext batch)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouCaoAngFengmin))
            yield break;
        var losses = batch.Movements.Select((m, i) => (m, i)).Where(x => x.m.From.Zone == CardZoneKind.Equipment && x.m.From != x.m.To && x.m.From.OwnerSeat is { } seat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(x.m)).GroupBy(x => x.m.From.OwnerSeat!.Value).Select(g => g.First().i).ToArray();
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
            foreach (var candidate in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.CardsMoved))
            {
                if (!IsMouCaFengmin(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) || MouCaoAng.Used(owner.Seat, candidate.SkillInstanceId))
                    continue;
                foreach (var i in losses)
                    yield return candidate with
                    {
                        OccurrenceIndex = i
                    };
            }
    }

    internal ProgramSkillWindowContext CreateMouCaEquipmentContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        var m = frame.Batch.Movements[candidate.OccurrenceIndex];
        var loser = m.From.OwnerSeat!.Value;
        if (m.From.Zone != CardZoneKind.Equipment || m.From == m.To || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            throw new InvalidOperationException("Fengmin requires a real equipment departure.");
        return new(SkillProgramTriggerWindow.CardsMoved, frame.Id, candidate.OwnerSeat, SourceSeat: loser, TargetSeat: candidate.OwnerSeat, OccurrenceIndex: candidate.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]), MovementBatch: frame.Batch, MovementIndex: candidate.OccurrenceIndex);
    }

    internal bool CanRunMouCaEquipment(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (c.SkillId != MouCaoAngRuntime.Fengmin || !IsMouCaFengmin(_coreProgramLifecycleOperations.GetProgramTrigger2(c)) || context.Window != SkillProgramTriggerWindow.CardsMoved || context.OwnerSeat != c.OwnerSeat || context.TargetSeat != c.OwnerSeat || context.OccurrenceIndex != c.OccurrenceIndex || context.MovementIndex != c.OccurrenceIndex || context.MovementBatch is not { } batch || batch.Id != context.ParentFrameId || c.OccurrenceIndex < 0 || c.OccurrenceIndex >= batch.Movements.Count || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(x => x.Id == batch.Id)is not { } parent || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] != c || !parent.Batch.Movements.SequenceEqual(batch.Movements) || MouCaoAng.Used(c.OwnerSeat, c.SkillInstanceId))
            return false;
        var m = batch.Movements[c.OccurrenceIndex];
        return m.From.Zone == CardZoneKind.Equipment && m.From.OwnerSeat == context.SourceSeat && m.From != m.To && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 400)]
    internal void CollectWindow_CollectMouCaEquipmentCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectMouCaEquipmentCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 900)]
    internal bool ExcludeOrdinaryMovement_IsMouCaFengmin(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsMouCaFengmin(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 1300)]
    internal ProgramSkillWindowContext? TryCreateMouCaEquipmentContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsMouCaFengmin(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)))
            return CreateMouCaEquipmentContext(frame, candidate);
        return null;
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 60, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveMouCaMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsMouCaFengmin(trigger) ? CanRunMouCaEquipment(candidate, context) : null;
}
