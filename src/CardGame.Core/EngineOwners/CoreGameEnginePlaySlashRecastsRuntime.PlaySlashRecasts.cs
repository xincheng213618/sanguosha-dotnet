#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePlaySlashRecastsRuntime
{
    SkillProgramStepOutcome IEndingPairSlashLossHost.DrawEndingPair(ProgramSkillFrame f, string state) => _coreEndingPairDrawsOperations.Value.BeginEndingPairDraw(f, state);
    SkillProgramStepOutcome IEndingPairSlashLossHost.RecastSelectedPhysicalSlash(ProgramSkillFrame f) => BeginActualPlaySlashRecast(f);
    internal SkillProgramStepOutcome BeginActualPlaySlashRecast(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.PlaySlashRecast is not null || f.TriggerId is not null || f.InstructionIndex != 1 || f.SelectedCardIds is not [var id] || f.SelectedTargetSeats.Count != 0 || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId < 1)
            throw new InvalidOperationException("Slash recast requires its original one-card, zero-target actual Play activation.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f) || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(f.OwnerSeat))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "原手牌、来源或参与者已失效，没有支付重铸。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var card = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Single(c => c.Id == id);
        if (!CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind))
            throw new InvalidOperationException("The active recast requires a printed physical Slash, Fire Slash or Thunder Slash.");
        var before = _coreEndingPairDrawsOperations.Value.EndingPairSequence;
        f = _coreNativeRecastInvocationsOperations.BeginFrameNativeRecastInvocation(f, f.OwnerSeat, [id]);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null), PlaySlashRecast = new(f.InstructionIndex, _coreEndingPairDrawsOperations.Value.EndingPairSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, f.OwnerSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, id, card.Kind, before, before) });
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(f.OwnerSeat), CardLocation.DiscardPile, CardMoveReasons.RecastDiscard);
        var after = _coreEndingPairDrawsOperations.Value.EndingPairSequence;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PlaySlashRecast = f.PlaySlashRecast!with { SequenceAfter = after } });
        var r = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id).PlaySlashRecast!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlaySlashRecastPaidEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurnNumber, r.ActualTurnOwnerSeat, r.PhaseActorSeat, r.PhaseInstanceId, id, card.Kind, before, after));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 440)]
    internal bool ResumeEndingPairOrSlashRecast(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id)
            return false;
        if (f.EndingPairDraw is not null)
        {
            _coreEndingPairDrawsOperations.Value.ContinueEndingPairDraw(id);
            return true;
        }

        if (f.PlaySlashRecast is not { } r)
            return false;
        if (!ValidPlaySlashRecast(f))
            throw new InvalidOperationException("A paid Slash recast lost its original actual phase/material or exact cost/reward invoice.");
        if (r.AwaitingMovement)
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
                return true;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null, PlaySlashRecast = r with { AwaitingMovement = false, CostDrained = true } });
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.PlaySlashRecast!;
        }

        if (!r.DrawIssued)
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
            if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
            {
                // The real paid recast survives cancellation; an unattempted
                // reward must not be recorded as an issued zero-card draw.
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecastEvent(owner.Seat, r.CardId, r.CardKind, 0));
                f = _coreNativeRecastInvocationsOperations.CompleteFrameNativeRecastInvocation(f, 0);
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { PlaySlashRecast = null, PendingMovementContinuation = null });
                _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), "重铸成本已结算，来源、参与者或游戏失效，尚未发行的摸牌取消。");
                return true;
            }

            var before = _coreEndingPairDrawsOperations.Value.EndingPairSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null), PlaySlashRecast = r with { DrawIssued = true, AwaitingMovement = true, DrawSequenceBefore = before } });
            var count = _corePhysicalDrawFacadesOperations.DrawCards(owner, 1, true, CardMoveReasons.RecastDraw).Count;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PlaySlashRecast = f.PlaySlashRecast!with { DrawSequenceAfter = _coreEndingPairDrawsOperations.Value.EndingPairSequence, ActualDrawCount = count } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlaySlashRecastDrawIssuedEvent(id, owner.Seat, before, _coreEndingPairDrawsOperations.Value.EndingPairSequence, count));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecastEvent(owner.Seat, r.CardId, r.CardKind, count));
            _coreNativeRecastInvocationsOperations.CompleteFrameNativeRecastInvocation(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), count);
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PlaySlashRecast = null });
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), "游戏已结束，重铸后继取消。");
            return true;
        }

        // The fresh executor applies source/liveness cancellation after paid children.
        return false;
    }

    internal bool ValidPlaySlashRecast(ProgramSkillFrame f)
    {
        if (f.PlaySlashRecast is not { } r || r.InstructionIndex != f.InstructionIndex || f.InstructionIndex != 1 || f.TriggerId is not null || r.Source != _coreEndingPairDrawsOperations.Value.EndingPairSource(f) || r.GameplayHash != f.GameplayHash || string.IsNullOrWhiteSpace(r.Source.SkillInstanceId) || r.ActualTurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || r.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || r.PhaseActorSeat != f.OwnerSeat || r.PhaseInstanceId != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != f.OwnerSeat || !f.SelectedCardIds.SequenceEqual([r.CardId]) || f.SelectedTargetSeats.Count != 0 || !CoreNativeCardClassificationsRuntime.IsSlashCard(r.CardKind) || r.SequenceBefore < 0 || r.SequenceAfter <= r.SequenceBefore || ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Instructions is not [{ Op: SkillProgramEffectOp.RecastSelectedPhysicalSlash }] || _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && m.CardId == r.CardId && m.CardKind == r.CardKind && m.From == CardLocation.Hand(f.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.RecastDiscard) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PlaySlashRecastPaidEvent>().Count(e => e.ProgramFrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.ActualTurnNumber == r.ActualTurnNumber && e.ActualTurnOwnerSeat == r.ActualTurnOwnerSeat && e.PhaseActorSeat == r.PhaseActorSeat && e.PhaseInstanceId == r.PhaseInstanceId && e.CardId == r.CardId && e.CardKind == r.CardKind && e.SequenceBefore == r.SequenceBefore && e.SequenceAfter == r.SequenceAfter) != 1)
            return false;
        if (r.AwaitingMovement && (f.PendingMovementContinuation is not { SubjectSeat: var subject, BeforeCount: 0, CoverageResultBind: null } || subject != f.OwnerSeat))
            return false;
        var draws = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PlaySlashRecastDrawIssuedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        return !r.DrawIssued ? r.DrawSequenceBefore == 0 && r.DrawSequenceAfter == 0 && r.ActualDrawCount == 0 && draws.Length == 0 : r.CostDrained && r.DrawSequenceBefore >= r.SequenceAfter && r.DrawSequenceAfter >= r.DrawSequenceBefore && r.ActualDrawCount is >= 0 and <= 1 && draws is [var draw] && draw.OwnerSeat == f.OwnerSeat && draw.SequenceBefore == r.DrawSequenceBefore && draw.SequenceAfter == r.DrawSequenceAfter && draw.ActualCount == r.ActualDrawCount && _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > r.DrawSequenceBefore && m.Sequence <= r.DrawSequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason == CardMoveReasons.RecastDraw) == r.ActualDrawCount;
    }
}
