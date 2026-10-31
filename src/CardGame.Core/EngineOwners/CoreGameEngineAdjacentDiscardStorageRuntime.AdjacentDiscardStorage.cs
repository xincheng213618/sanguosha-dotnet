#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAdjacentDiscardStorageRuntime
{
    internal bool TracksAdjacentDiscardStorage => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.StoreAdjacentDiscardedSlash) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.OfferShanheSourceChunlaoStorage);

    internal CardKind? FreezeAdjacentDiscardPaymentIdentity(Card card, CardLocation from, CardLocation to, CardMoveReason reason)
    {
        if (!TracksAdjacentDiscardStorage || from.OwnerSeat is not { } source || !_coreMatchParticipantsPlayersAccess._players[source].IsAlive || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment) || to != CardLocation.DiscardPile && to != CardLocation.Processing || !CoreGameEngineProgramDiscardOriginsRuntime.IsProgramDiscardOriginReason(reason))
            return null;
        var kind = from.Zone == CardZoneKind.Hand ? _coreActionQueriesOperations.GetProgramCardIdentityMatches(_coreMatchParticipantsPlayersAccess._players[source], card).FirstOrDefault()?.Identity.OutputKind ?? card.Kind : card.Kind;
        return CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Contains(kind) ? kind : null;
    }

    internal void CaptureAdjacentDiscardOrigin(Card card, CardMovementRecord move, CardKind? frozenIdentity)
    {
        if (!TracksAdjacentDiscardStorage)
            return;
        var carried = _resolutionStackOperations._resolutionStack.SelectMany(f => f.PendingAdjacentDiscardOrigins?.Entries ?? []).SingleOrDefault(r => r.CardId == card.Id);
        if (carried is not null)
        {
            var owner = _resolutionStackOperations._resolutionStack.Single(f => f.Id == carried.OwnerFrameId);
            var remaining = owner.PendingAdjacentDiscardOrigins!.Entries.Where(r => r.CardId != card.Id).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeFrame(owner.Id, owner with { PendingAdjacentDiscardOrigins = remaining.Length == 0 ? null : new(remaining) });
            var previous = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(m => m.CardId == card.Id && m.Sequence < move.Sequence);
            if (move.To == CardLocation.DiscardPile && move.From == CardLocation.Processing && previous is not null && previous.Sequence == carried.EntryMovementSequence && _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(move) == carried.From)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramAdjacentDiscardOriginEvent(move.Sequence, card.Id, carried.SourceSeat, carried.PreviousLivingSeat, carried.NextLivingSeat, carried.EffectiveKind));
            // An outside move, cancellation or non-discard cleanup irreversibly ends this original discard provenance.
            return;
        }

        if (move.From.OwnerSeat is not { } source || !_coreMatchParticipantsPlayersAccess._players[source].IsAlive || move.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment) || move.To != CardLocation.DiscardPile && move.To != CardLocation.Processing || !CoreGameEngineProgramDiscardOriginsRuntime.IsProgramDiscardOriginReason(move.Reason) || frozenIdentity is not { } kind || !CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Contains(kind))
            return;
        var living = _coreTableOrderOperations.CurrentTableOrder().Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).ToArray();
        var index = Array.IndexOf(living, source);
        if (index < 0)
            return;
        var previousSeat = living[(index + living.Length - 1) % living.Length];
        var nextSeat = living[(index + 1) % living.Length];
        if (move.To == CardLocation.DiscardPile)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramAdjacentDiscardOriginEvent(move.Sequence, card.Id, source, previousSeat, nextSeat, kind));
        else
        {
            var owner = _resolutionStackOperations._resolutionStack.LastOrDefault() ?? throw new InvalidOperationException("A private discard payment requires its actual owning frame.");
            var origin = new PendingAdjacentDiscardOrigin(owner.Id, move.Sequence, card.Id, move.From, move.Reason.Value, source, previousSeat, nextSeat, kind);
            _coreRuntimeOperations.ReplaceRuntimeFrame(owner.Id, owner with { PendingAdjacentDiscardOrigins = new((owner.PendingAdjacentDiscardOrigins?.Entries ?? []).Append(origin).ToArray()) });
        }
    }

    internal ProgramAdjacentDiscardOriginEvent? AdjacentDiscardOrigin(CardMovementRecord move) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramAdjacentDiscardOriginEvent>().SingleOrDefault(e => e.MovementSequence == move.Sequence && e.CardId == move.CardId);
    internal int[] MatchingAdjacentDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => batch.Movements.Select((move, index) => (move, index, origin: AdjacentDiscardOrigin(move))).Where(x => x.move.To == CardLocation.DiscardPile && _cardZonesOperations._cardZones.GetLocation(x.move.CardId) == CardLocation.DiscardPile && x.origin is { } origin && CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Contains(origin.EffectiveKind) && (candidate.OwnerSeat == origin.SourceSeat || candidate.OwnerSeat == origin.PreviousLivingSeat || candidate.OwnerSeat == origin.NextLivingSeat)).Select(x => x.index).ToArray();
    SkillProgramStepOutcome IAdjacentDiscardAndRoundAlcoholHost.StoreAdjacentDiscardedSlash(ProgramSkillFrame frame, CardZoneKind zone) => BeginAdjacentDiscardStorage(frame, zone);
    SkillProgramStepOutcome IAdjacentDiscardAndRoundAlcoholHost.OfferCurrentSlashFireAndExtraTarget(ProgramSkillFrame frame) => _coreCurrentSlashFireOffersOperations.BeginCurrentSlashFireOffer(frame);
    SkillProgramStepOutcome IAdjacentDiscardAndRoundAlcoholHost.PayCompletedUseDiscardOrLoseHp(ProgramSkillFrame frame) => _coreCompletedUsePaymentsOperations.Value.BeginCompletedUsePayment(frame);
    SkillProgramStepOutcome IAdjacentDiscardAndRoundAlcoholHost.UseRoundPricedPileDyingAlcohol(ProgramSkillFrame frame, string stateId, CardZoneKind zone) => _coreRoundPricedPileAlcoholOperations.BeginRoundPileAlcohol(frame, stateId, zone);
    internal SkillProgramStepOutcome BeginAdjacentDiscardStorage(ProgramSkillFrame input, CardZoneKind zone)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.AdjacentDiscardStorage is not null || f.WindowContext is not { Window: SkillProgramTriggerWindow.DiscardPileReceived, MovementBatch: { } batch, MovementIndex: { } index } || index < 0 || index >= batch.Movements.Count)
            throw new InvalidOperationException("Adjacent storage lost its exact original discarded entity.");
        var move = batch.Movements[index];
        var origin = AdjacentDiscardOrigin(move);
        if (origin is null || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(move) || move.To != CardLocation.DiscardPile || !CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Contains(origin.EffectiveKind) || f.OwnerSeat != origin.SourceSeat && f.OwnerSeat != origin.PreviousLivingSeat && f.OwnerSeat != origin.NextLivingSeat)
            throw new InvalidOperationException("Adjacent storage lost its frozen identity or neighbor proof.");
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f) || _cardZonesOperations._cardZones.GetLocation(move.CardId) != CardLocation.DiscardPile)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "弃牌实体或持久区来源失效，未储存牌。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Single(c => c.Id == move.CardId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AdjacentDiscardStorage = new(f.InstructionIndex, card.Id, move.Sequence, 0, zone) });
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DiscardPile, new(zone, f.OwnerSeat), new($"skill-program.{(f.SkillId)}.adjacent-discard-store"), stored =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { AdjacentDiscardStorage = active.AdjacentDiscardStorage!with { StoreMovementSequence = stored.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramAdjacentDiscardStoredEvent(f.Id, new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), card.Id, move.Sequence, stored.Sequence, zone));
        });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 520)]
    internal bool ResumeAdjacentDiscardStorage(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.AdjacentDiscardStorage is not { })
            return false;
        AssertAdjacentDiscardStorage(f);
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AdjacentDiscardStorage = null });
        return false;
    }

    internal void AssertAdjacentDiscardStorage(ProgramSkillFrame f)
    {
        if (f.AdjacentDiscardStorage is not { } r)
            return;
        var paused = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
        if (paused.Op != SkillProgramEffectOp.StoreAdjacentDiscardedSlash || r.InstructionIndex != f.InstructionIndex || paused.DestinationZone != r.Zone || f.WindowContext is not { Window: SkillProgramTriggerWindow.DiscardPileReceived, MovementBatch: { } batch, MovementIndex: { } index } || index < 0 || index >= batch.Movements.Count || batch.Movements[index].Sequence != r.DiscardMovementSequence || batch.Movements[index].CardId != r.CardId || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence == r.StoreMovementSequence && m.CardId == r.CardId && m.From == CardLocation.DiscardPile && m.To == new CardLocation(r.Zone, f.OwnerSeat) && m.Reason.Value == $"skill-program.{(f.SkillId)}.adjacent-discard-store") || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramAdjacentDiscardStoredEvent>().Any(e => e.ProgramFrameId == f.Id && e.Source == new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) && e.CardId == r.CardId && e.DiscardMovementSequence == r.DiscardMovementSequence && e.StoreMovementSequence == r.StoreMovementSequence && e.Zone == r.Zone))
            throw new InvalidOperationException("Stored discard receipt lost its exact committed movement/source.");
    }

    [ProgramPipelineHook(ProgramPipelinePhase.SelectLegacyDiscardIndexes, 1000)]
    internal int[]? SelectWindowIndexes_MatchingAdjacentDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger)
    {
        var features = ProgramInstructionResolver.Default.Features(trigger);
        if (features.HasOperation(SkillProgramEffectOp.StoreAdjacentDiscardedSlash))
            return MatchingAdjacentDiscardIndexes(batch, candidate);
        return null;
    }
}
