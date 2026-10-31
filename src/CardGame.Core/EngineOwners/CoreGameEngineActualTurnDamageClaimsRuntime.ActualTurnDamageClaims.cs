#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineActualTurnDamageClaimsRuntime
{
    internal const string ActualTurnDamageClaimReason = "program.actual-turn-damage-entity.claim";
    internal bool TracksActualTurnDamageEntities => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ClaimActualTurnDamageEntities);

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 310)]
    internal void ObserveActualTurnDamageEntities(IGameEvent payload)
    {
        if (!TracksActualTurnDamageEntities || _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber <= 0 || payload is not DamageAppliedEvent { Amount: > 0 } applied || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || damage.ParentFrameId != attack.ResolutionId || damage.SourceSeat != applied.SourceSeat || damage.TargetSeat != applied.TargetSeat || damage.Amount != applied.Amount || damage.Nature != applied.Nature || attack.SourceSeat != applied.SourceSeat || attack.TargetSeat != applied.TargetSeat || attack.IsSourceLess != applied.SourceLess || !attack.DamageWasApplied)
            return;
        if (!_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DamageRequestedEvent>().Any(e => e.ResolutionId == damage.Id && e.SourceSeat == applied.SourceSeat && e.TargetSeat == applied.TargetSeat && e.Amount == applied.Amount && e.Nature == applied.Nature && e.SourceCard == attack.EffectiveCardKind && e.SourceLess == applied.SourceLess))
            return;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ActualTurnCardDamageEntityEvent>().Any(e => e.DamageFrameId == damage.Id) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ActualTurnCardDamageEmptyEvent>().Any(e => e.DamageFrameId == damage.Id))
            return;
        var owner = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == attack.ResolutionId);
        IReadOnlyList<int> materials;
        long? actionId = null;
        var actor = attack.CardUserSeat;
        var provider = actor;
        var delayed = false;
        if (owner is CardUseFrame use && use.SourceSeat == actor && use.CardKind == attack.EffectiveCardKind)
        {
            if (use.Action is { Type: CardActionType.Use } action && action.ActorSeat == actor && action.EffectiveKind == use.CardKind)
            {
                actionId = action.ActionId;
                provider = action.ProviderSeat;
                materials = action.PhysicalCards.Select(c => c.CardId).ToArray();
                if (use.PhysicalCardIds is { } original && !original.SequenceEqual(materials))
                    return;
            }
            else if (use.Action is null && use.CardId == 0 && use.PhysicalCardIds is { Count: 0 } && (_coreCurrentTurnUseKindsOperations.IsActualTurnLegacyVirtualUse(use) || _coreDamageJudgmentSuitPaymentsOperations.LegacyDamageJudgmentVirtualProducer(use, applied.TargetSeat)is not null))
                materials = [];
            else
                return;
            if (!MatchesDeclaredActualDamageUse(use, actor, provider))
                return;
        }
        else if (owner is JudgmentFrame { Continuation: JudgmentContinuationKind.Lightning, DelayedCardId: { } held, Succeeded: true, CardAttack: { IsDelayedJudgmentDamage: true, EffectiveCardKind: CardKind.Lightning } } judgment && attack.IsDelayedJudgmentDamage && attack.EffectiveCardKind == CardKind.Lightning && judgment.TargetSeat == actor && judgment.CardAttack.PhysicalCardIds.SequenceEqual([held]) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<LightningResolvedEvent>().Any(e => e.ResolutionId == judgment.Id && e.CardId == held && e.JudgmentTargetSeat == actor && e.Hit && e.DamageAmount == attack.DamageAmount))
        {
            // The held Lightning is the causal material. judgment.CardId is the
            // separately revealed judgment card and is deliberately not read.
            materials = [held];
            delayed = true;
        }
        else if (owner is ProgramSkillFrame { CardAttack: { PhysicalCardIds.Count: 0, EffectiveCardKind: CardKind.Duel } } virtualDuel && attack.EffectiveCardKind == CardKind.Duel && _contentRegistryOperations._contentRegistry.GetSkill(virtualDuel.SkillId).Program is { } program && program.GameplayHash == virtualDuel.GameplayHash && ProgramInstructionResolver.Default.Resolve(virtualDuel, program).GetPausedInstruction(virtualDuel.InstructionIndex).Effect.Op == SkillProgramEffectOp.UseVirtualDuel)
            materials = [];
        else
            return; // Pure skill damage has no card; it cannot invent claimable entities.
        if (attack.EffectiveCardKind is not { } kind || materials.Any(id => id <= 0) || materials.Distinct().Count() != materials.Count || !attack.PhysicalCards.Select(c => c.Id).SequenceEqual(materials))
            return;
        if (materials.Count == 0)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ActualTurnCardDamageEmptyEvent(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, damage.Id, attack.ResolutionId, actionId, actor, applied.SourceSeat, applied.TargetSeat, kind, applied.Amount, applied.Nature, applied.SourceLess));
            return;
        }

        for (var index = 0; index < materials.Count; index++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ActualTurnCardDamageEntityEvent(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, damage.Id, attack.ResolutionId, actionId, actor, provider, applied.SourceSeat, applied.TargetSeat, kind, applied.Amount, applied.Nature, applied.SourceLess, materials[index], index, materials.Count, delayed));
    }

    internal bool MatchesDeclaredActualDamageUse(CardUseFrame use, int actor, int provider)
    {
        var declared = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseDeclaredEvent>().Where(e => e.ResolutionId == use.Id && e.CardId == use.CardId && e.CardKind == use.CardKind).ToArray();
        if (declared is not [var original] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(original.SourceSeat))
            return false;
        var replacements = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramCardUseActorReplacedEvent>().Where(e => e.CardUseFrameId == use.Id).ToArray();
        if (replacements.Length > 0 && (use.Action is not { } action || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Any(e => e.Action.ActionId == action.ActionId && e.Action.Type == CardActionType.Use && e.Action.ActorSeat == original.SourceSeat && e.Action.ProviderSeat == provider && e.Action.EffectiveKind == use.CardKind && e.Action.PhysicalCards.SequenceEqual(action.PhysicalCards))))
            return false;
        var cursor = original.SourceSeat;
        foreach (var replacement in replacements)
        {
            if (replacement.PreviousActorSeat != cursor || replacement.OwnerSeat != cursor || replacement.ProviderSeat != provider || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(replacement.ActorSeat) || replacement.ActorSeat == cursor || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramBindingStartedEvent>().Any(e => e.FrameId == replacement.FrameId && e.SkillId == replacement.SkillId && e.OwnerSeat == replacement.OwnerSeat && e.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized))
                return false;
            cursor = replacement.ActorSeat;
        }

        return cursor == actor && use.SourceSeat == actor;
    }

    internal IReadOnlyList<ActualTurnCardDamageEntityEvent> EligibleActualTurnDamageEntities(int owner) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ActualTurnCardDamageEntityEvent>().Where(e => e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && e.VictimSeat == owner && e.Amount > 0 && e.MaterialCount > 0 && e.CardId > 0 && _cardZonesOperations._cardZones.GetLocation(e.CardId) == CardLocation.DiscardPile).GroupBy(e => e.CardId).Select(g => g.OrderBy(e => e.DamageFrameId).ThenBy(e => e.MaterialIndex).First()).OrderBy(e => e.DamageFrameId).ThenBy(e => e.MaterialIndex).ToArray();
    internal bool MatchesActualTurnDamageEnding(ProgramSkillFrame root)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1] is TurnEndingBoundaryFrame end && end.OwnerSeat == root.OwnerSeat && end.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && end.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && root.WindowContext is { Window: SkillProgramTriggerWindow.TurnEnding } c && c.ParentFrameId == end.Id && c.OwnerSeat == root.OwnerSeat && c.SourceSeat == root.OwnerSeat && c.TargetSeat == root.OwnerSeat && end.ItemIndex >= 0 && end.ItemIndex < end.Items.Count && end.Items[end.ItemIndex].Candidate is { } candidate && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(root, candidate) && c.OccurrenceIndex == candidate.OccurrenceIndex;
    }

    internal SkillProgramStepOutcome BeginActualTurnDamageClaim(ProgramSkillFrame frame)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (frame.ActualTurnDamageClaim is not null || frame.InstructionIndex != 1 || !MatchesActualTurnDamageEnding(frame))
            throw new InvalidOperationException("A damage-entity claim requires its independent original actual Ending candidate exactly once.");
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(frame))
            return SkillProgramStepOutcome.Continue;
        var facts = EligibleActualTurnDamageEntities(frame.OwnerSeat);
        var ids = facts.Select(e => e.CardId).ToArray();
        var before = _coreCardMovementJournalCardMovementsAccess._cardMovements.Count == 0 ? 0 : _coreCardMovementJournalCardMovementsAccess._cardMovements[^1].Sequence;
        var receipt = new ProgramActualTurnDamageClaimReceipt(frame.InstructionIndex - 1, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, frame.WindowContext!.ParentFrameId, frame.WindowContext.OccurrenceIndex, ids, before, before + ids.Length);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ActualTurnDamageClaim = receipt, PendingMovementContinuation = ids.Length == 0 ? null : new(frame.OwnerSeat, 0, null) });
        if (ids.Length == 0)
            return SkillProgramStepOutcome.Continue;
        // All entities move in one batch; both the frozen claim and the original
        // damage facts are committed before any gain observer can pause.
        var cards = facts.Select(fact => _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Single(card => card.Id == fact.CardId)).ToArray();
        var claimIndex = 0;
        _corePhysicalMovementFacadesOperations.MoveCards(cards, CardLocation.DiscardPile, CardLocation.Hand(frame.OwnerSeat), new(ActualTurnDamageClaimReason), beforeFact: movement =>
        {
            var index = claimIndex++;
            var fact = facts[index];
            if (movement.CardId != fact.CardId)
                throw new InvalidOperationException("The claimed damage entities changed their frozen movement order.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ActualTurnDamageEntityClaimedEvent(frame.Id, new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, movement.CardId, index, fact.DamageFrameId, movement.Sequence));
        }, emitCardMovedEvents: false);
        // Preserve any native queue added to the real owning frame by movement.
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var after = _coreCardMovementJournalCardMovementsAccess._cardMovements[^1].Sequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ActualTurnDamageClaim = receipt with { SequenceAfter = after } });
        return _corePaidColorConversionsOperations.AwaitPaidColorDamageClaimMovements(frame.Id);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 700)]
    internal bool CanRunPaidColorDamageClaim(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects.Any(e => PaidColorDuelOperation.IsPayment(e.Op)))
            return context.Window == SkillProgramTriggerWindow.DrawPhaseEnded && context.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Count > 0;
        return true; // The mandatory Ending branch is independent, including an empty result.
    }
}
