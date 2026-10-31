#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDongZhaoMouOlRuntime
{
    internal IEnumerable<MouDongZhaoYishiGrantedEvent> ActiveMouYishiGrants(int owner) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouDongZhaoYishiGrantedEvent>().Where(e => e.Source.OwnerSeat == owner && e.Suit != Suit.None && e.OwnTurnStarts == _coreProgramEventHistoryOperations.Value.ProgramEventHistory<TurnStartedEvent>().Count(t => t.ActorSeat == owner)).GroupBy(e => e.Suit).Select(g => g.Last());
    internal IReadOnlyList<MouDongZhaoLossSource> CaptureMouYishiSources(IEnumerable<CardLocation> locations)
    {
        if (!HasMouDongZhao || !_coreMatchSetupSetupCompleteAccess._setupComplete)
            return[];
        return Array.AsReadOnly(locations.Where(l => l.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && l.OwnerSeat is not null).Distinct().SelectMany(location => ActiveMouYishiGrants(location.OwnerSeat!.Value).SelectMany(grant => _cardZonesOperations._cardZones.CardsAt(location).Where(card => _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[location.OwnerSeat.Value], card) == grant.Suit).Select(card => new MouDongZhaoLossSource(grant.FrameId, grant.Source, grant.GameplayHash, grant.Suit, card.Id, location)))).ToArray());
    }

    internal bool MouYishiNonUseLoss(CardMovementBatchContext batch, CardMovementRecord move, int owner, long grantId) => move.Sequence > 0 && move.From.OwnerSeat == owner && move.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && !(move.To.OwnerSeat == owner && move.To.Zone is CardZoneKind.Hand or CardZoneKind.Equipment) && batch.MouYishiSources.Any(s => s.GrantFrameId == grantId && s.CardId == move.CardId && s.From == move.From && s.Source.OwnerSeat == owner) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeTrueUseProcessingEntryEvent>().Any(e => e.EntrySequence == move.Sequence && e.CardId == move.CardId && e.From == move.From);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.Collection, 33500, SkillProgramEffectOp.DrawMouDongZhaoYishiLoss)]
    internal bool CollectMouYishiIssuedOnly(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => false;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 33501, SkillProgramEffectOp.DrawMouDongZhaoYishiLoss)]
    internal bool CanRunMouYishiIssuedOnly(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => IsIssuedMouYishiCandidate(candidate, context);
    internal IEnumerable<ProgramTriggerCandidate> CollectMouYishiLossCandidates(CardMovementBatchContext batch)
    {
        if (!HasMouDongZhao)
            yield break;
        foreach (var grantId in batch.MouYishiSources.Select(s => s.GrantFrameId).Distinct())
        {
            var grant = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouDongZhaoYishiGrantedEvent>().Single(g => g.FrameId == grantId);
            var owner = _coreMatchParticipantsPlayersAccess._players[grant.Source.OwnerSeat];
            if (!owner.IsAlive)
                continue;
            if (!batch.Movements.Any(m => m.Sequence > grant.MovementSequence && MouYishiNonUseLoss(batch, m, owner.Seat, grant.FrameId)) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouDongZhaoYishiLossDrawIssuedEvent>().Any(e => e.GrantFrameId == grant.FrameId && e.BatchId == batch.Id))
                continue;
            yield return new(owner.Seat, MouDongZhaoComposition.Yishi, "follow-suit-loss", grant.Source.SkillInstanceId, grant.GameplayHash, 0, checked((int)grant.FrameId));
        }
    }

    internal MouDongZhaoYishiGrantedEvent? MouYishiCandidateGrant(ProgramTriggerCandidate candidate, CardMovementBatchContext batch)
    {
        if (candidate.SkillId != MouDongZhaoComposition.Yishi || candidate.BindingId != "follow-suit-loss" || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(candidate.OwnerSeat))
            return null;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouDongZhaoYishiGrantedEvent>().SingleOrDefault(g => g.FrameId == candidate.OccurrenceIndex && g.Source.OwnerSeat == candidate.OwnerSeat && g.Source.SkillInstanceId == candidate.SkillInstanceId && g.GameplayHash == candidate.GameplayHash && batch.Movements.Any(m => m.Sequence > g.MovementSequence && MouYishiNonUseLoss(batch, m, candidate.OwnerSeat, g.FrameId)));
    }

    internal ProgramSkillWindowContext CreateMouYishiContext(CardsMovedTriggerWindowFrame window, ProgramTriggerCandidate candidate) => new(SkillProgramTriggerWindow.CardsMoved, window.Id, candidate.OwnerSeat, SourceSeat: candidate.OwnerSeat, TargetSeat: candidate.OwnerSeat, OccurrenceIndex: candidate.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]), MovementBatch: window.Batch);
    [ProgramIssuedTrigger(20)]
    internal bool IsIssuedMouYishiCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(candidate.SkillId)?.Program?.GameplayHash == candidate.GameplayHash && context.Window == SkillProgramTriggerWindow.CardsMoved && context.OwnerSeat == candidate.OwnerSeat && context.MovementBatch is { } batch && context.ParentFrameId == batch.Id && context.OccurrenceIndex == candidate.OccurrenceIndex && _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(w => w.Id == batch.Id)is { } parent && parent.Batch.Movements.SequenceEqual(batch.Movements) && parent.Candidates.Any(c => c == candidate) && MouYishiCandidateGrant(candidate, batch)is { } grant && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouDongZhaoYishiLossDrawIssuedEvent>().Any(e => e.GrantFrameId == grant.FrameId && e.BatchId == batch.Id);
    internal SkillProgramStepOutcome BeginMouYishiLoss(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouDongZhao is not null)
        {
            ContinueMouDongZhao(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (f.InstructionIndex != 1 || f.WindowContext is not { MovementBatch: { } batch } context || !IsIssuedMouYishiCandidate(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0, context.OccurrenceIndex), context))
            throw new InvalidOperationException("Yishi requires its original unconsumed matching-suit non-Use loss batch.");
        var grant = MouYishiCandidateGrant(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0, context.OccurrenceIndex), batch)!;
        var receipt = new MouDongZhaoReceipt(MouDongZhaoSource(f), f.GameplayHash, SkillProgramEffectOp.DrawMouDongZhaoYishiLoss, MouDongZhaoStage.Drawing, context.ParentFrameId, _coreActualRoundProgressRoundNumberAccess._roundNumber, f.OwnerSeat, RequestedDraw: 1, GrantFrameId: grant.FrameId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDongZhao = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDongZhaoYishiLossDrawIssuedEvent(f.Id, grant.FrameId, batch.Id, receipt.Source, receipt.GameplayHash, grant.Suit));
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, new("skill-program.mou-ol:yishi.loss-draw"));
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 4400)]
    internal void CollectWindow_CollectMouYishiLossCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectMouYishiLossCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 3500)]
    internal ProgramSkillWindowContext? TryCreateMouYishiContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (candidate.SkillId == MouDongZhaoComposition.Yishi && candidate.BindingId == "follow-suit-loss" && MouYishiCandidateGrant(candidate, frame.Batch)is not null)
            return CreateMouYishiContext(frame, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.BeginMovementBatch, 200)]
    internal void CaptureMovementBatchStartMouYishiSources(CardMovementBatchBuilder batch) => batch.MouYishiSources = CaptureMouYishiSources(batch.SourceCountsBefore.Keys);
}
