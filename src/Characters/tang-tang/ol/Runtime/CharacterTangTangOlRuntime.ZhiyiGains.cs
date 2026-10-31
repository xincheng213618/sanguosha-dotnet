#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTangTangOlRuntime
{
    internal bool HasTangTangZhiyi => _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveZhiyiGainColorBenefit) == true;

    internal static bool IsTangTangZhiyiTrigger(SkillProgramTrigger t) => RuntimeSkills.RuntimeRules_tang_tang_ol_Zhiyi_Gains.IsTangTangZhiyiTrigger(t);
    internal bool ZhiyiAvailable(int owner) => !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZhiyiOpportunityPaidEvent>().Any(e => e.OwnerSeat == owner && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);
    internal IEnumerable<ProgramTriggerCandidate> CollectTangTangZhiyiCandidates(CardMovementBatchContext batch) => batch.TangTangGains is not { Count: > 0 } gains ? [] : gains.GroupBy(g => g.OwnerSeat).Where(g => _coreMatchParticipantsPlayersAccess._players[g.Key].IsAlive && ZhiyiAvailable(g.Key)).SelectMany(g => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[g.Key], SkillProgramTriggerWindow.CardsMoved).Where(c => IsTangTangZhiyiTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(c))).Select(c => c with { OccurrenceIndex = TangTangGainOccurrence(batch, g) })).ToArray();
    internal ProgramSkillWindowContext CreateTangTangZhiyiContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate c) => new(SkillProgramTriggerWindow.CardsMoved, frame.Id, c.OwnerSeat, SourceSeat: c.OwnerSeat, TargetSeat: c.OwnerSeat, OccurrenceIndex: c.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat])with { MovedCardCount = frame.Batch.TangTangGains!.Count(g => g.OwnerSeat == c.OwnerSeat) }, MovementBatch: frame.Batch, MovementIndex: c.OccurrenceIndex);
    internal IReadOnlyList<TangTangGain>? CurrentTangTangGains(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!IsTangTangZhiyiTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(c)) || context.Window != SkillProgramTriggerWindow.CardsMoved || context.OwnerSeat != c.OwnerSeat || context.SourceSeat != c.OwnerSeat || context.TargetSeat != c.OwnerSeat || context.OccurrenceIndex != c.OccurrenceIndex || context.MovementIndex != c.OccurrenceIndex || context.MovementBatch is not { TangTangGains: { Count: > 0 } gains } batch || batch.Id != context.ParentFrameId || c.OccurrenceIndex < 0 || c.OccurrenceIndex >= batch.Movements.Count || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(w => w.Id == batch.Id)is not { } parent || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] != c || !parent.Batch.Movements.SequenceEqual(batch.Movements) || parent.Batch.TangTangGains is not { } original || !original.SequenceEqual(gains))
            return null;
        var selected = gains.Where(g => g.OwnerSeat == c.OwnerSeat).ToArray();
        if (selected.Length == 0 || c.OccurrenceIndex != TangTangGainOccurrence(batch, selected) || selected.Any(g => !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZhiyiGainFrozenEvent>().Any(e => e.Gain == g) || (g.NativeDrawId == 0 ? !batch.Movements.Any(m => m.CardId == g.CardId && m.Sequence == g.GainSequence && m.To == CardLocation.Hand(g.RecipientSeat)) : batch.NativeDrawInvocation is not { } draw || draw.InvocationId != g.NativeDrawId || draw.LastBatchId != batch.Id || draw.OwnerSeat != g.OwnerSeat || !_coreOutsidePhaseDrawDiscardOperations.ValidNativeDrawInvocation(draw) || !draw.Materials.Any(m => m.CardId == g.CardId && m.MovementSequence == g.GainSequence))))
            return null;
        return Array.AsReadOnly(selected);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27531)]
    internal bool QualifyTangTangZhiyi(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => !IsTangTangZhiyiTrigger(t) || ZhiyiAvailable(c.OwnerSeat) && CurrentTangTangGains(c, context)is not null;
    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 3700)]
    internal void CollectWindow_CollectTangTangZhiyiCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectTangTangZhiyiCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 1800)]
    internal bool ExcludeOrdinaryMovement_IsTangTangZhiyiTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsTangTangZhiyiTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 3100)]
    internal ProgramSkillWindowContext? TryCreateTangTangZhiyiContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsTangTangZhiyiTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)))
            return CreateTangTangZhiyiContext(frame, candidate);
        return null;
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 100, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveTangTangMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsTangTangZhiyiTrigger(trigger) ? CurrentTangTangGains(candidate, context)is not null : null;
}
