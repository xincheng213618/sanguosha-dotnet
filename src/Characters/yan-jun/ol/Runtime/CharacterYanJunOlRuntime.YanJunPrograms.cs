#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYanJunOlRuntime
{
    internal int[] MatchingOwnSettledActionCardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate)
    {
        if (_contentRegistryOperations._contentRegistry?.ProgramDependencies.RequiresFact(ProgramTrackedFact.NativeSettledActionDiscard) != true)
            return [];
        var settled = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SettledActionCardDiscardEvent>().Where(e => e.BatchId == batch.Id && e.ActorSeat == candidate.OwnerSeat).Select(e => e.MovementSequence).ToHashSet();
        return batch.Movements.Select((movement, index) => (movement, index))
            .Where(item => settled.Contains(item.movement.Sequence) && IsXunxianDiscardEntryCurrent(item.movement))
            .Select(item => item.index).Order().ToArray();
    }

    internal int CurrentPlayPhaseEventIndex(int ownerSeat)
    {
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        return Array.FindLastIndex(facts, e => e is PhaseChangedEvent phase && phase.Phase == TurnPhase.Play && phase.ActorSeat == ownerSeat);
    }

    SkillProgramStepOutcome IYanJunProgramHost.GuanchaoChoosePattern(ProgramSkillFrame frame, SkillProgramEffect effect) => GuanchaoProgramChoosePattern(frame, effect);
    SkillProgramStepOutcome IYanJunProgramHost.GuanchaoRankDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => GuanchaoProgramRankDraw(frame, effect);
    [ProgramPipelineHook(ProgramPipelinePhase.SelectLegacyDiscardIndexes, 600)]
    internal int[]? SelectWindowIndexes_MatchingOwnSettledActionCardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger)
    {
        var features = ProgramInstructionResolver.Default.Features(trigger);
        if (features.HasOperation(SkillProgramEffectOp.XunxianGiftUsedCard))
            return MatchingOwnSettledActionCardIndexes(batch, candidate);
        return null;
    }
}
