#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGanNingMobileGodRuntime
{
    internal const string MobileJieying = "mobile-god:jieying", MobileCampClaimReason = "skill.mobile-god.jieying.ended-hand";
    SkillProgramStepOutcome IMobileCampClaimHost.Claim(ProgramSkillFrame frame) => ClaimMobileCampHand(frame);
    internal bool IsMobileCampClaimContext(ProgramSkillFrame frame) => frame.SkillId is MobileJieying or "happy:jieying" && frame.WindowContext is { Window: SkillProgramTriggerWindow.AfterTurnEnded } context && _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().SingleOrDefault(f => f.Id == context.ParentFrameId)is { } parent && parent.OwnerSeat != frame.OwnerSeat && _coreAfterTurnEndedProgramsOperations.MatchesAfterTurnEndedChild(parent, frame, context);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 39825)]
    internal bool AllowsMobileCampClaim(ProgramTriggerLegalityContext gate) => gate.SkillId is not (MobileJieying or "happy:jieying") || gate.WindowContext?.Window != SkillProgramTriggerWindow.AfterTurnEnded || gate.WindowContext.SourceSeat is { } seat && seat != gate.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[seat].MarkerSourceCounts.GetValueOrDefault((PlayerMarkerKind.Camp, gate.OwnerSeat)) > 0;
    internal SkillProgramStepOutcome ClaimMobileCampHand(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!IsMobileCampClaimContext(frame) || frame.MobileCampClaim is not null || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame)?.Op != SkillProgramEffectOp.ClaimMobileEndedCampHand)
            throw new InvalidOperationException("Mobile Jieying requires its actual completed-turn child and original instruction.");
        var seat = frame.WindowContext!.SourceSeat!.Value;
        if (_coreMatchParticipantsPlayersAccess._players[seat].MarkerSourceCounts.GetValueOrDefault((PlayerMarkerKind.Camp, frame.OwnerSeat)) <= 0)
            return SkillProgramStepOutcome.Continue;
        // The mobile text removes Camp before the gain, and never returns it to the source.
        _coreStrategicProgramsOperations.SetStrategicMarker(seat, PlayerMarkerKind.Camp, frame.OwnerSeat, 0, frame);
        var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Select(c => c.Id).ToArray();
        var source = new CardConversionSource(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
        var receipt = new MobileCampClaimReceipt(source, frame.GameplayHash, frame.InstructionIndex, frame.WindowContext.ParentFrameId, seat, _coreActualTurnProgressTurnNumberAccess._turnNumber, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
        {
            Cards = cards
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { MobileCampClaim = receipt });
        void Complete(long batch, IReadOnlyList<CardMovementRecord> moves)
        {
            var live = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            var paid = receipt with
            {
                SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
                BatchId = batch
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(live with { MobileCampClaim = paid });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileCampClaimedEvent(frame.Id, source, receipt.EndedFrameId, seat, receipt.ActualTurn, cards.Length, receipt.SequenceBefore, paid.SequenceAfter, batch));
        }

        if (cards.Length == 0)
        {
            Complete(0, []);
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { MobileCampClaim = null });
            return SkillProgramStepOutcome.Continue;
        }

        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(cards, CardLocation.Hand(frame.OwnerSeat), new(MobileCampClaimReason), Complete);
        var outcome = _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat);
        if (outcome == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { MobileCampClaim = null });
        return outcome;
    }

    internal bool ValidMobileCampClaim(ProgramSkillFrame frame)
    {
        if (frame.MobileCampClaim is not { } r || r.Source != new CardConversionSource(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) || r.GameplayHash != frame.GameplayHash || r.InstructionIndex != frame.InstructionIndex || r.EndedFrameId != frame.WindowContext?.ParentFrameId || r.EndedSeat != frame.WindowContext?.SourceSeat || r.ActualTurn != _coreActualTurnProgressTurnNumberAccess._turnNumber || r.Cards.Count == 0 || r.Cards.Distinct().Count() != r.Cards.Count || r.BatchId <= 0 || r.SequenceAfter <= r.SequenceBefore || !IsMobileCampClaimContext(frame))
            return false;
        var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter).ToArray();
        return moves.Length == r.Cards.Count && moves.Select(m => m.CardId).SequenceEqual(r.Cards) && moves.All(m => m.From == CardLocation.Hand(r.EndedSeat) && m.To == CardLocation.Hand(frame.OwnerSeat) && m.Reason.Value == MobileCampClaimReason) && _coreProgramEventHistoryOperations.ProgramEventHistory<PlayerMarkerChangedEvent>().Count(e => e.ResolutionId == frame.Id && e.PlayerSeat == r.EndedSeat && e.Marker == PlayerMarkerKind.Camp && e.Count == 0 && e.SkillOwnerSeat == frame.OwnerSeat) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<MobileCampClaimedEvent>().Count(e => e.ProgramFrameId == frame.Id && e.Source == r.Source && e.EndedFrameId == r.EndedFrameId && e.EndedSeat == r.EndedSeat && e.ActualTurn == r.ActualTurn && e.Count == r.Cards.Count && e.BatchId == r.BatchId && e.SequenceBefore == r.SequenceBefore && e.SequenceAfter == r.SequenceAfter) == 1;
    }
    [ProgramContinuation(ProgramContinuationKind.Movement, 39825)]
    internal bool ReturnMobileCampClaim(ProgramSkillFrame frame)
    {
        if (frame.MobileCampClaim is null)
            return false;
        if (!ValidMobileCampClaim(frame))
            throw new InvalidOperationException("Mobile Jieying lost its already-paid movement child.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { MobileCampClaim = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 39825)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 39825)]
    internal bool RetainMobileCampClaim(ProgramSkillFrame frame) => frame.MobileCampClaim is not null;
}
