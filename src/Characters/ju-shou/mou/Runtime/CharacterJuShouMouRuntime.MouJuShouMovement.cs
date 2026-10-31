#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJuShouMouRuntime
{
    internal bool MouJsMovementParent(ProgramSkillFrame f)
    {
        if (f.SkillId != MouJuShouRuntime.Muwang || f.TriggerId != "first-loss" || f.ActivationId != f.TriggerId || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardsMoved } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return at > 0 && _resolutionStackOperations._resolutionStack[at - 1] is CardsMovedTriggerWindowFrame p && p.Id == w.ParentFrameId && p.Batch.Id == w.MovementBatch?.Id && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex]) && IsIssuedMouJsMovementCandidate(p.Candidates[p.CandidateIndex], w);
    }

    internal void CaptureMouJsMovementBatch(CardMovementBatchContext batch)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouJuShouMuwang))
            return;
        if (batch.Movements.Any(m => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.TurnNumber != batch.TurnNumber))
            throw new InvalidOperationException("Muwang requires a genuinely committed original native movement batch.");
        MouJuShou.ObserveBatch(new(batch.Id, batch.TurnNumber, batch.Movements));
    }

    internal MouJsMuwangQueuedEvent? MouJsQueue(ProgramTriggerCandidate c, CardMovementBatchContext batch) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouJsMuwangQueuedEvent>().SingleOrDefault(q => q.Ordinal == c.OccurrenceIndex && q.Source.SkillId == c.SkillId && q.Source.BindingId == c.BindingId && q.Source.OwnerSeat == c.OwnerSeat && q.Source.SkillInstanceId == c.SkillInstanceId && q.GameplayHash == c.GameplayHash && q.Turn == batch.TurnNumber && q.BatchId == batch.Id);
    internal IEnumerable<ProgramTriggerCandidate> CollectMouJsMovementCandidates(CardMovementBatchContext batch)
    {
        if (_contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(MouJuShouRuntime.Muwang)?.Program is not { } program)
            yield break;
        var trigger = program.Triggers.Single(t => t.Id == "first-loss");
        foreach (var q in _coreProgramEventHistoryOperations.ProgramEventHistory<MouJsMuwangQueuedEvent>().Where(q => q.BatchId == batch.Id && q.Turn == batch.TurnNumber && _coreMatchParticipantsPlayersAccess._players[q.Source.OwnerSeat].IsAlive && q.GameplayHash == program.GameplayHash && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouJsMuwangSettledEvent>().Any(e => e.Ordinal == q.Ordinal)))
            yield return new(q.Source.OwnerSeat, q.Source.SkillId, q.Source.BindingId, q.Source.SkillInstanceId, q.GameplayHash, trigger.Priority, q.Ordinal);
    }

    internal ProgramSkillWindowContext CreateMouJsMovementContext(CardsMovedTriggerWindowFrame p, ProgramTriggerCandidate c) => new(SkillProgramTriggerWindow.CardsMoved, p.Id, c.OwnerSeat, SourceSeat: c.OwnerSeat, TargetSeat: c.OwnerSeat, OccurrenceIndex: c.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]), MovementBatch: p.Batch);
    [ProgramIssuedTrigger(10)]
    internal bool IsIssuedMouJsMovementCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (c.SkillId != MouJuShouRuntime.Muwang || c.BindingId != "first-loss" || w.Window != SkillProgramTriggerWindow.CardsMoved || w.OwnerSeat != c.OwnerSeat || w.SourceSeat != c.OwnerSeat || w.TargetSeat != c.OwnerSeat || w.OccurrenceIndex != c.OccurrenceIndex || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == w.ParentFrameId)is not CardsMovedTriggerWindowFrame p || p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count || p.Candidates[p.CandidateIndex] != c || w.MovementBatch is not { } batch || batch.Id != p.Batch.Id || !batch.Movements.SequenceEqual(p.Batch.Movements) || MouJsQueue(c, batch)is not { } q || q.Turn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || q.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program?.GameplayHash || batch.Movements.Any(m => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m)))
            return false;
        if (q.Kind == MouJsMuwangKind.ChooseFirst)
        {
            var choices = _coreProgramEventHistoryOperations.ProgramEventHistory<MouJsFirstCandidateEvent>().Where(e => e.Ordinal == q.Ordinal).ToArray();
            return choices.Length > 0 && choices.Select(e => e.CardId).Distinct().Count() == choices.Length && choices.Min(e => e.Sequence) == q.FirstSequence && choices.All(e => batch.Movements.Any(m => m.CardId == e.CardId && m.CardKind == e.Kind && m.From == e.From && m.From.OwnerSeat == c.OwnerSeat && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment && m.From != m.To && m.Sequence == e.Sequence));
        }

        if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouJsFirstChosenEvent>().Any(e => e.Source == q.Source && e.Turn == q.Turn && e.CardId == q.CardId && e.FirstSequence == q.FirstSequence))
            return false;
        return q.Kind == MouJsMuwangKind.Recover ? batch.Movements.Any(m => m.CardId == q.CardId && m.To == CardLocation.DiscardPile && m.Sequence >= q.FirstSequence) : batch.Movements.Any(m => m.CardId == q.CardId && m.From.OwnerSeat == c.OwnerSeat && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment && m.From != m.To && _coreProgramEventHistoryOperations.ProgramEventHistory<MouJsRecoveredEvent>().Any(e => e.Source == q.Source && e.Turn == q.Turn && e.CardId == q.CardId && e.ReturnSequence < m.Sequence));
    }

    internal MouJsMuwangQueuedEvent? MouJsMuwangOpportunity(ProgramSkillFrame f)
    {
        if (!MouJsMovementParent(f) || f.WindowContext is not { MovementBatch: { } batch } w)
            return null;
        var p = _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(x => x.Id == w.ParentFrameId);
        return MouJsQueue(p.Candidates[p.CandidateIndex], batch);
    }

    internal bool MouJsReclaimable(MouJsFirstLoss loss, CardMovementRecord discarded)
    {
        if (discarded.CardId != loss.CardId || discarded.TurnNumber != loss.Turn || discarded.To != CardLocation.DiscardPile || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(discarded) || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.SingleOrDefault(m => m.CardId == loss.CardId && m.Sequence == loss.FirstSequence)is not { } first || first.From.OwnerSeat != loss.Source.OwnerSeat || first.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment))
            return false;
        // The selected first-loss invoice follows this exact physical entity through
        // later real transfers in the same actual turn. Processing never creates a
        // second owned loss; only its original HEJ departure names the first loss.
        return discarded.Sequence >= first.Sequence;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 300)]
    internal void CollectWindow_CollectMouJsMovementCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectMouJsMovementCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 800)]
    internal bool ExcludeOrdinaryMovement_ResolveMouJuShouMuwang(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveMouJuShouMuwang);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 1200)]
    internal ProgramSkillWindowContext? TryCreateMouJsMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (candidate.SkillId == MouJuShouRuntime.Muwang && candidate.BindingId == "first-loss")
            return CreateMouJsMovementContext(frame, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 4400)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureMouJsMovementBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureMouJsMovementBatch(completed);
        return completed;
    }
}
