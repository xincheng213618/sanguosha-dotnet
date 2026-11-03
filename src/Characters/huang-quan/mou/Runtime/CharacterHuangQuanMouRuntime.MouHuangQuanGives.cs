#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangQuanMouRuntime
{
    internal bool HasMouHuangQuan => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouHuangQuanTuicheng);

    internal IReadOnlyList<ProgramTriggerCandidate> CollectMouHqGiveCandidates(CardMovementBatchContext batch)
    {
        var result = new List<ProgramTriggerCandidate>();
        foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
            foreach (var c in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(p, SkillProgramTriggerWindow.CardsMoved).Where(c => c.SkillId == MouHqTuicheng && c.BindingId == "after-native-give"))
                foreach (var gift in _givenCards.GiftsForBatch(batch, p.Seat))
                    result.Add(c with { OccurrenceIndex = Array.FindIndex(batch.Movements.ToArray(), m => m.Sequence == gift.CompletionSequence) });
        return Array.AsReadOnly(result.ToArray());
    }

    internal bool IsMouHqGiveTrigger(SkillProgramTrigger trigger) => trigger.Effects is [{ Op: SkillProgramEffectOp.ResolveMouHuangQuanTuicheng }];
    internal ProgramSkillWindowContext CreateMouHqGiveContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate c)
    {
        var gift = _givenCards.GiftsForBatch(frame.Batch, c.OwnerSeat).Single(g => g.CompletionSequence == frame.Batch.Movements[c.OccurrenceIndex].Sequence);
        return new(SkillProgramTriggerWindow.CardsMoved, frame.Id, c.OwnerSeat, SourceSeat: c.OwnerSeat, TargetSeat: gift.Recipient, Amount: gift.Movements.Count, OccurrenceIndex: c.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]), MovementBatch: frame.Batch, MovementIndex: c.OccurrenceIndex)
        {
            MouHqGift = gift
        };
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 500)]
    internal void CollectWindow_CollectMouHqGiveCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectMouHqGiveCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 1000)]
    internal bool ExcludeOrdinaryMovement_IsMouHqGiveTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsMouHqGiveTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 3400)]
    internal ProgramSkillWindowContext? TryCreateMouHqGiveContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
        if (IsMouHqGiveTrigger(trigger))
            return CreateMouHqGiveContext(frame, candidate);
        return null;
    }
}
