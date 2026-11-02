#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouFeiLeRuntime
{
    internal static CardConversionSource LingkongSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_zhou_fei_le_LeZhouFeiLingkong.LingkongSource(f);
    internal static bool IsLingkongGainTrigger(SkillProgramTrigger t) => RuntimeSkills.RuntimeRules_zhou_fei_le_LeZhouFeiLingkong.IsLingkongGainTrigger(t);
    internal bool HasLingkongEntityMark(int cardId) => _cardEntityMarks.Has(cardId, "lingkong");
    internal bool HasLingkongHandEntityMark(int ownerSeat, int cardId) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Hand(ownerSeat) && HasLingkongEntityMark(cardId);
    internal bool IsLingkongHandLimitExempt(CharacterState owner, int cardId) => _coreNationalSkillsOperations.HasRuntimeSkill(owner, "le:lingkong") && HasLingkongHandEntityMark(owner.Seat, cardId);

    private IEnumerable<(HandGainOccurrence Gain, ProgramTriggerCandidate Candidate)> QualifiedLingkongGains(CardMovementBatchContext batch) =>
        (batch.HandGains ?? []).Where(gain => gain.FirstOutsideDraw && gain.InitiallyAlive &&
                gain.Timing is { } timing && (timing.Phase != TurnPhase.Draw || timing.PhaseActorSeat != gain.RecipientSeat) &&
                gain.Incoming.Any(card => card.IsActualGain))
            .OrderBy(gain => gain.RecipientSeat)
            .SelectMany(gain => gain.InitialSources.Where(candidate => candidate.OwnerSeat == gain.RecipientSeat &&
                    IsLingkongGainTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)))
                .OrderByDescending(candidate => candidate.Priority).ThenBy(candidate => candidate.SkillInstanceId, StringComparer.Ordinal)
                .Take(1).Select(candidate => (gain, candidate)));

    private HandGainOccurrence? LingkongGain(CardMovementBatchContext batch, CardConversionSource source, string hash) =>
        QualifiedLingkongGains(batch).Where(item => new CardConversionSource(item.Candidate.SkillId, item.Candidate.BindingId,
                item.Candidate.OwnerSeat, item.Candidate.SkillInstanceId) == source && item.Candidate.GameplayHash == hash)
            .Select(item => item.Gain).SingleOrDefault();

    internal IReadOnlyList<ProgramTriggerCandidate> CollectLingkongMovementCandidates(CardMovementBatchContext batch) =>
        QualifiedLingkongGains(batch).Select(item => item.Candidate).ToArray();

    internal ProgramSkillWindowContext CreateLingkongMovementContext(CardsMovedTriggerWindowFrame window, ProgramTriggerCandidate candidate) =>
        new(SkillProgramTriggerWindow.CardsGained, window.Id, candidate.OwnerSeat, SourceSeat: window.Batch.OriginOwnerSeat,
            TargetSeat: candidate.OwnerSeat, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]),
            MovementBatch: window.Batch, OccurrenceIndex: candidate.OccurrenceIndex);

    [ProgramIssuedTrigger(180)]
    internal bool IsIssuedLingkongCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) =>
        context is { Window: SkillProgramTriggerWindow.CardsGained, MovementBatch: { } batch } &&
        context.OwnerSeat == candidate.OwnerSeat && context.TargetSeat == candidate.OwnerSeat &&
        _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault() is { } window &&
        window.Id == context.ParentFrameId && window.Batch == batch && window.Candidates.ElementAtOrDefault(window.CandidateIndex) == candidate &&
        LingkongGain(batch, new(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId), candidate.GameplayHash) is not null;

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 28310, SkillProgramEffectOp.MarkLingkongFirstOutsideDrawGain)]
    internal bool CanOfferLingkong(ProgramTriggerLegalityContext gate) => IsIssuedLingkongCandidate(gate.Candidate, gate.Context);

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28310)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28310)]
    internal bool PermitIssuedLingkong(ProgramSkillFrame frame)
    {
        if (frame.SkillId != "le:lingkong" || frame.TriggerId != "mark-first-outside-draw-gain" ||
            frame.InstructionIndex is < 0 or > 1 || frame.WindowContext is not { } context)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == frame.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1] is CardsMovedTriggerWindowFrame window &&
            window.Id == context.ParentFrameId && window.Candidates.ElementAtOrDefault(window.CandidateIndex) is { } candidate &&
            RuntimeSkills.RuntimeRules_Core_DamageTargetMountObserver.MountObserverCandidateMatches(frame, candidate) &&
            IsIssuedLingkongCandidate(candidate, context);
    }

    SkillProgramStepOutcome ILeZhouFeiLingkongHost.MarkLingkongGain(ProgramSkillFrame frame) => MarkLingkongGain(frame);
    internal SkillProgramStepOutcome MarkLingkongGain(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var batch = frame.WindowContext!.MovementBatch!;
        var gain = LingkongGain(batch, LingkongSource(frame), frame.GameplayHash)!;
        var cards = gain.Incoming.Where(card => card.IsActualGain).Select(card => card.CardId).ToArray();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LingkongGainMarkedEvent(frame.Id, batch.Id,
            LingkongSource(frame), frame.GameplayHash, gain.ActualTurn, gain.Timing!.ActualTurnOwnerSeat, cards.Length));
        _cardEntityMarkOperations.Mark(frame.Id, "lingkong", cards);
        return SkillProgramStepOutcome.Continue;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 1700)]
    internal void CollectWindow_CollectLingkongMovementCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectLingkongMovementCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 2400)]
    internal bool ExcludeOrdinaryMovement_IsLingkongGainTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsLingkongGainTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 1600)]
    internal ProgramSkillWindowContext? TryCreateLingkongMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) =>
        IsLingkongGainTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) ? CreateLingkongMovementContext(frame, candidate) : null;
}
