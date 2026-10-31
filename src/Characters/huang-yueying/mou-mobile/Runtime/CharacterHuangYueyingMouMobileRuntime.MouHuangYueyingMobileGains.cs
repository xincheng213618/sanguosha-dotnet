#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangYueyingMouMobileRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.Collection, 31500, SkillProgramEffectOp.TransferMouHuangYueyingQicaiGains)]
    internal bool CollectMouHyQiOnlyIssued(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => false;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 31500, SkillProgramEffectOp.TransferMouHuangYueyingQicaiGains)]
    internal bool CanRunMouHyQiOnlyIssued(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => IsIssuedMouHyQiCandidate(candidate, context);
    internal void CaptureMouHyQiGains(CardMovementBatchContext batch)
    {
        if (!HasMouHuangYueyingMobile)
            return;
        foreach (var grant in _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuangYueyingQiGrantedEvent>().GroupBy(e => e.RecipientSeat).Select(g => g.Last()).ToArray())
        {
            if (!_coreMatchParticipantsPlayersAccess._players[grant.Source.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[grant.RecipientSeat].IsAlive)
                continue;
            var remaining = MouHyQiRemaining(grant);
            if (remaining == 0)
                continue;
            foreach (var pair in batch.Movements.Select((m, i) => (Movement: m, Index: i)).OrderBy(p => p.Movement.Sequence))
            {
                var m = pair.Movement;
                if (remaining == 0)
                    break;
                if (m.Sequence <= grant.MovementSequence || m.To != CardLocation.Hand(grant.RecipientSeat) || _coreActualHandGainProgramsOperations.ActualHandGainOriginalSource(m, batch) == CardLocation.Hand(grant.RecipientSeat) || !CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(m.CardKind) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuangYueyingQiGainClaimedEvent>().Any(e => e.GrantId == grant.GrantId && e.GainSequence == m.Sequence))
                    continue;
                var source = grant.Source with
                {
                    BindingId = "receive-three"
                };
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHuangYueyingQiGainClaimedEvent(grant.GrantId, batch.Id, pair.Index, source, grant.GameplayHash, grant.RecipientSeat, m.CardId, _coreActualHandGainProgramsOperations.ActualHandGainOriginalSource(m, batch), m.Sequence, --remaining));
            }

            SyncMouHyQiMarker(batch.Id, grant.RecipientSeat);
        }
    }

    internal IEnumerable<ProgramTriggerCandidate> CollectMouHyQiGainCandidates(CardMovementBatchContext batch) => !HasMouHuangYueyingMobile ? [] : _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuangYueyingQiGainClaimedEvent>().Where(e => e.BatchId == batch.Id && _coreMatchParticipantsPlayersAccess._players[e.Source.OwnerSeat].IsAlive && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouHuangYueyingQiClaimSettledEvent>().Any(t => t.GrantId == e.GrantId && t.GainSequence == e.GainSequence)).Select(e => new ProgramTriggerCandidate(e.Source.OwnerSeat, e.Source.SkillId, e.Source.BindingId, e.Source.SkillInstanceId, e.GameplayHash, 0, e.MovementIndex)).ToArray();
    internal MouHuangYueyingQiGainClaimedEvent? MouHyQiBatchClaim(ProgramTriggerCandidate candidate, CardMovementBatchContext batch)
    {
        if (candidate.SkillId != MouHuangYueyingMobileComposition.Qicai || candidate.BindingId != "receive-three" || candidate.OccurrenceIndex < 0 || candidate.OccurrenceIndex >= batch.Movements.Count)
            return null;
        var claim = _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuangYueyingQiGainClaimedEvent>().SingleOrDefault(e => e.BatchId == batch.Id && e.MovementIndex == candidate.OccurrenceIndex && e.Source == new CardConversionSource(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId) && e.GameplayHash == candidate.GameplayHash);
        if (claim is null || batch.Movements[claim.MovementIndex] is not { } m || m.CardId != claim.CardId || m.Sequence != claim.GainSequence || m.To != CardLocation.Hand(claim.GainingSeat) || _coreActualHandGainProgramsOperations.ActualHandGainOriginalSource(m, batch) != claim.OriginalFrom || !CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(m.CardKind))
            return null;
        var grant = _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuangYueyingQiGrantedEvent>().SingleOrDefault(g => g.GrantId == claim.GrantId);
        return grant is not null && grant.RecipientSeat == claim.GainingSeat && (grant.Source with
        {
            BindingId = "receive-three"
        }

        ) == claim.Source && grant.GameplayHash == claim.GameplayHash && claim.GainSequence > grant.MovementSequence ? claim : null;
    }

    internal bool IsMouHyQiGainCandidate(ProgramTriggerCandidate candidate, CardMovementBatchContext batch) => MouHyQiBatchClaim(candidate, batch)is not null;
    internal MouHuangYueyingQiGainClaimedEvent? MouHyQiClaim(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.CardsGained || context.OwnerSeat != candidate.OwnerSeat || context.TargetSeat != candidate.OwnerSeat || context.MovementBatch is not { } batch || batch.Id != context.ParentFrameId || context.MovementIndex != candidate.OccurrenceIndex || context.OccurrenceIndex != candidate.OccurrenceIndex || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(f => f.Id == batch.Id)is not { } parent || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] != candidate || !parent.Batch.Movements.SequenceEqual(batch.Movements))
            return null;
        var parentIndex = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == parent.Id);
        if (parentIndex != _resolutionStackOperations._resolutionStack.Count - 1 && (_resolutionStackOperations._resolutionStack[parentIndex + 1] is not ProgramSkillFrame child || child.OwnerSeat != candidate.OwnerSeat || child.SkillId != candidate.SkillId || child.TriggerId != candidate.BindingId || child.SkillInstanceId != candidate.SkillInstanceId || child.GameplayHash != candidate.GameplayHash || child.WindowContext != context))
            return null;
        var claim = MouHyQiBatchClaim(candidate, batch);
        return claim is not null && context.SourceSeat == claim.GainingSeat ? claim : null;
    }

    internal ProgramSkillWindowContext CreateMouHyQiGainContext(CardsMovedTriggerWindowFrame parent, ProgramTriggerCandidate c)
    {
        var claim = MouHyQiBatchClaim(c, parent.Batch) ?? throw new InvalidOperationException("Qicai gain context requires its complete original batch claim.");
        return new(SkillProgramTriggerWindow.CardsGained, parent.Id, c.OwnerSeat, SourceSeat: claim.GainingSeat, TargetSeat: c.OwnerSeat, OccurrenceIndex: c.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]), MovementBatch: parent.Batch, MovementIndex: c.OccurrenceIndex);
    }

    [ProgramIssuedTrigger(110)]
    internal bool IsIssuedMouHyQiCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && MouHyQiClaim(c, context)is { } claim && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouHuangYueyingQiClaimSettledEvent>().Any(e => e.GrantId == claim.GrantId && e.GainSequence == claim.GainSequence);
    internal SkillProgramStepOutcome BeginMouHyQiTransfer(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouHuangYueyingMobile is not null)
        {
            ContinueMouHy(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (f.WindowContext is not { } context || !IsIssuedMouHyQiCandidate(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0, context.OccurrenceIndex), context))
            throw new InvalidOperationException("Qicai transfer requires its exact original accepted actual gain claim.");
        var claim = MouHyQiClaim(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0, context.OccurrenceIndex), context)!;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var from = CardLocation.Hand(claim.GainingSeat);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouHuangYueyingMobile = new(MouHySource(f), f.GameplayHash, SkillProgramEffectOp.TransferMouHuangYueyingQicaiGains, MouHuangYueyingMobileStage.MovementChildren, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, context.ParentFrameId, claim.GainingSeat, Paid: true, CardId: claim.CardId, From: from, Before: before, After: before, GrantId: claim.GrantId, ClaimSequence: claim.GainSequence), PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[claim.GainingSeat].IsAlive && _cardZonesOperations._cardZones.GetLocation(claim.CardId) == from && _coreMatchOutcomeWinnerAccess._winner == Winner.None)
        {
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(claim.CardId), from, CardLocation.Hand(f.OwnerSeat), new("skill-program.mou-mobile:qicai.transfer"), _characterHuangQuanMouOperations.NativeGiveCompletion(f.Id, claim.GainingSeat, f.OwnerSeat, [claim.CardId], before, m => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHuangYueyingQiTransferredEvent(f.Id, claim.GrantId, claim.GainSequence, claim.CardId, m.From, m.To, m.Sequence))), disclosure: CardMovementDisclosure.FaceDown);
        }

        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouHuangYueyingMobile = f.MouHuangYueyingMobile!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueMouHy(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 3100)]
    internal void CollectWindow_CollectMouHyQiGainCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectMouHyQiGainCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 2300)]
    internal ProgramSkillWindowContext? TryCreateMouHyQiGainContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsMouHyQiGainCandidate(candidate, frame.Batch))
            return CreateMouHyQiGainContext(frame, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 3700)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureMouHyQiGains(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureMouHyQiGains(completed);
        return completed;
    }
}
