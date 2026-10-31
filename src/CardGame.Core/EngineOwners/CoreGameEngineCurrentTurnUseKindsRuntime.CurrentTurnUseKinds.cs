#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCurrentTurnUseKindsRuntime
{
    internal bool TracksCurrentTurnUseKinds => _contentRegistryOperations._contentRegistry?.ProgramDependencies.TracksCurrentTurnUseKinds == true;
    internal bool TracksActualTurnUseOrdinal => _contentRegistryOperations._contentRegistry?.ProgramDependencies.TracksActualTurnUseOrdinal == true;
    internal bool HasOngoingActualUseTurn => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber > 0 && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase is not (TurnPhase.NotStarted or TurnPhase.Finished);

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 250)]
    internal void ObserveCurrentTurnUseKinds(IGameEvent payload)
    {
        if (!TracksCurrentTurnUseKinds)
            return;
        if (payload is CardActionAcceptedEvent accepted)
        {
            var response = accepted.Action;
            if ((TracksActualTurnUseOrdinal || _coreCurrentRoundActualUsesOperations.TracksCurrentRoundActualUses || _coreWholeActualTurnUsesOperations.TracksWholeActualTurnUseOrdinal) && response.Type == CardActionType.Response && response.EffectiveKind == CardKind.Dodge && (response.ActorSeat == response.ProviderSeat || _characterLuSuMobileShiOperations.IsShiLsAutonomousResponse(response)) && response.ResponderSeat == response.ActorSeat && response.RequesterSeat is null)
            {
                // Defending against a Slash is a use of Dodge. Arrow Barrage and
                // supplied cards remain responses, with no actual-use ordinal.
                var defendingUse = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(use => use.Action?.ActionId == response.ParentActionId && use.CardAttack?.TargetSeat == response.ActorSeat);
                if (defendingUse?.CardAttack is not { EffectiveCardKind: { } incoming } attack || !CoreNativeCardClassificationsRuntime.IsSlashCard(incoming) || response.OpponentSeat != attack.SourceSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(response.ActorSeat))
                    return;
                if (_coreCurrentRoundActualUsesOperations.CurrentUseKindDeduplicationHistory().OfType<CurrentTurnCardUseKindsRecordedEvent>().Any(e => e.CardActionId == response.ActionId))
                    return;
                RecordCurrentTurnUseKinds(defendingUse.Id, response.ActorSeat, response, CardKind.Dodge, false);
                return;
            }

            if (response.Type != CardActionType.Response || response.EffectiveKind != CardKind.Nullification || response.ActorSeat != response.ProviderSeat && !_characterLuSuMobileShiOperations.IsShiLsAutonomousResponse(response) || response.ResponderSeat != response.ActorSeat || response.RequesterSeat is not null)
                return;
            var window = _resolutionStackOperations._resolutionStack.OfType<NullificationWindowFrame>().LastOrDefault();
            if (window is null || !_coreDelayedNullificationOperations.Value.HasNullificationEffectOwner(window) || response.ParentActionId != _coreDelayedNullificationOperations.Value.NullificationParentActionId(window) || response.OpponentSeat != window.SourceSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(response.ActorSeat))
                throw new InvalidOperationException("Actual Nullification use requires its exact live counterspell window and parent use.");
            if (_coreCurrentRoundActualUsesOperations.CurrentUseKindDeduplicationHistory().OfType<CurrentTurnCardUseKindsRecordedEvent>().Any(e => e.CardActionId == response.ActionId))
                return;
            RecordCurrentTurnUseKinds(window.Id, response.ActorSeat, response, CardKind.Nullification, true);
            return;
        }

        if (payload is not CardUseDeclaredEvent declared)
            return;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Id == declared.ResolutionId);
        if (use is null || use.SourceSeat != declared.SourceSeat || use.CardId != declared.CardId || use.CardKind != declared.CardKind || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(use.SourceSeat))
            throw new InvalidOperationException("Actual turn use requires its exact declared card-use frame.");
        var action = use.Action;
        if (action is not null && action.Type != CardActionType.Use)
            return;
        if (action is not null && (action.ActorSeat != use.SourceSeat || action.EffectiveKind != use.CardKind))
            throw new InvalidOperationException("Actual turn use differs from its frozen effective action.");
        // Legacy virtual uses have a real CardUse frame but deliberately no
        // Action. They add a type, never a manufactured hand suit or action.
        if (action is null && (use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || !IsActualTurnLegacyVirtualUse(use)))
            return;
        if (_coreCurrentRoundActualUsesOperations.CurrentUseKindDeduplicationHistory().OfType<CurrentTurnCardUseKindsRecordedEvent>().Any(e => !e.IsNullificationUse && e.OriginFrameId == use.Id))
            return;
        RecordCurrentTurnUseKinds(use.Id, use.SourceSeat, action, use.CardKind, false);
    }

    internal bool IsActualTurnLegacyVirtualUse(CardUseFrame use)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
        if (index <= 0 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame producer || producer.OwnerSeat != use.SourceSeat || producer.TriggerId is null && !TracksActualTurnUseOrdinal && !_coreCurrentRoundActualUsesOperations.TracksCurrentRoundActualUses && !_coreWholeActualTurnUsesOperations.TracksWholeActualTurnUseOrdinal && !_characterCaoZhiLeOperations.HasLeCaoZhiWenlanCapability)
            return false;
        var program = _contentRegistryOperations._contentRegistry.GetSkill(producer.SkillId).Program;
        if (program is null || program.GameplayHash != producer.GameplayHash)
            return false;
        var paused = ProgramInstructionResolver.Default.Resolve(producer, program).GetPausedInstruction(producer.InstructionIndex).Effect;
        if (use.CardKind == CardKind.Slash)
            return producer.SelectedTargetSeats.Count == 1 && producer.SelectedTargetSeats.SequenceEqual(use.TargetSeats) && paused is { Op: SkillProgramEffectOp.UseVirtualCard, OutputKind: CardKind.Slash, UseCardActionWindows: false };
        if (use.CardKind != CardKind.Alcohol || paused.Op != SkillProgramEffectOp.UseVirtualDyingAlcohol || producer.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context || context.TargetSeat != use.SourceSeat || !use.TargetSeats.SequenceEqual([use.SourceSeat]))
            return false;
        var dying = _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().SingleOrDefault(d => d.Id == context.ParentFrameId);
        return dying is not null && dying.VictimSeat == use.SourceSeat && dying.ResponderIndex >= 0 && dying.ResponderIndex < dying.ResponderSeats.Count && dying.ResponderSeats[dying.ResponderIndex] == use.SourceSeat;
    }

    internal void RecordCurrentTurnUseKinds(long frameId, int actorSeat, CardActionContext? action, CardKind effectiveKind, bool nullificationUse, int? programInstructionIndex = null)
    {
        if (_contentRegistryOperations._contentRegistry.ProgramDependencies.TracksEffectiveUseNames)
            _characterCaiYongLeOperations.CaptureFeibaiDeclaredName(frameId, action, effectiveKind, programInstructionIndex);
        var handSuit = action is { ProviderSeat: var provider } && provider == action.ActorSeat && action.PhysicalCards.Count > 0 && action.PhysicalCards.All(c => c.From == CardLocation.Hand(action.ActorSeat)) ? ActualTurnSuitBit(action.EffectiveSuit) : 0;
        var category = CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(effectiveKind);
        // Freeze declaration order, not completion order: nested uses may complete
        // before this one, and Action-null legacy virtual uses still occupy a slot.
        var ordinal = TracksActualTurnUseOrdinal && HasOngoingActualUseTurn ? checked(_corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent or TurnEndedEvent).OfType<CurrentTurnCardUseKindsRecordedEvent>().Count(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && e.ActorSeat == actorSeat) + 1) : (int? )null;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CurrentTurnCardUseKindsRecordedEvent(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, actorSeat, frameId, action?.ActionId, handSuit, 1 << (int)category, nullificationUse, ordinal, _coreCurrentRoundActualUsesOperations.TracksCurrentRoundActualUses || _coreWholeActualTurnUsesOperations.TracksWholeActualTurnUseOrdinal ? action?.Type ?? CardActionType.Use : null, _coreCurrentRoundActualUsesOperations.TracksCurrentRoundActualUses || _coreWholeActualTurnUsesOperations.TracksWholeActualTurnUseOrdinal ? action?.ProviderSeat ?? actorSeat : null, _coreWholeActualTurnUsesOperations.TracksWholeActualTurnUseOrdinal && _coreWholeActualTurnUsesOperations.HasOngoingWholeActualUseTurn ? checked(_coreWholeActualTurnUsesOperations.WholeActualTurnUseHistory().Length + 1) : null) { ProgramInstructionIndex = programInstructionIndex, EffectiveCardKind = _coreWholeActualTurnUsesOperations.TracksWholeActualTurnUseOrdinal || _contentRegistryOperations._contentRegistry.ProgramDependencies.TracksSlashDamageUseOrdinals ? effectiveKind : null });
    }

    internal int? CapturedActualTurnActorUseOrdinal(CardActionContext action)
    {
        if (!TracksActualTurnUseOrdinal || !HasOngoingActualUseTurn || action.ActionId <= 0 || action.Type == CardActionType.Response && (action.EffectiveKind is not (CardKind.Nullification or CardKind.Dodge) || action.ActorSeat != action.ProviderSeat || action.ResponderSeat != action.ActorSeat || action.RequesterSeat is not null))
            return null;
        var facts = _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent or TurnEndedEvent).OfType<CurrentTurnCardUseKindsRecordedEvent>().Where(e => e.CardActionId == action.ActionId).ToArray();
        if (facts.Length == 0)
            return _coreActualTurnLegacyCompletionOperations.CapturedNormalizedLegacyActualTurnOrdinal(action);
        if (facts is not [var fact] || fact.ActualTurnActorUseOrdinal is not > 0)
            throw new InvalidOperationException("An actual-turn use ordinal requires one positive declaration fact.");
        // Replacing the card-use actor must not lend the original actor's count
        // to the replacement. The turn and actor are part of the issued identity.
        return fact.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && fact.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && fact.ActorSeat == action.ActorSeat && fact.IsNullificationUse == (action.Type == CardActionType.Response && action.EffectiveKind == CardKind.Nullification) ? fact.ActualTurnActorUseOrdinal : null;
    }

    internal static int ActualTurnSuitBit(Suit? suit) => RuntimeSkills.RuntimeRules_Core_CurrentTurnUseKinds.ActualTurnSuitBit(suit);
    internal (int HandSuits, int Categories) CurrentTurnUseKinds(int actorSeat)
    {
        if (!TracksCurrentTurnUseKinds)
            return (0, 0);
        var hand = 0;
        var categories = 0;
        foreach (var fact in _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent or TurnEndedEvent).OfType<CurrentTurnCardUseKindsRecordedEvent>().Where(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && e.ActorSeat == actorSeat))
        {
            hand |= fact.HandSuitMask;
            categories |= fact.CardCategoryMask;
        }

        return (System.Numerics.BitOperations.PopCount((uint)hand), System.Numerics.BitOperations.PopCount((uint)categories));
    }

    internal bool CurrentTurnOwnerUsedSameSuitCards()
    {
        if (!TracksCurrentTurnUseKinds)
            return false;
        var mask = 0;
        var uses = 0;
        foreach (var fact in _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent or TurnEndedEvent).OfType<CurrentTurnCardUseKindsRecordedEvent>().Where(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && e.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat))
        {
            if (fact.HandSuitMask == 0)
                return false;
            mask |= fact.HandSuitMask;
            uses++;
        }

        return uses >= 2 && System.Numerics.BitOperations.PopCount((uint)mask) == 1;
    }

    internal int CurrentTurnCategoryDrawCount(ProgramSkillFrame frame, int targetSeat)
    {
        var context = frame.WindowContext;
        var window = _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == context?.ParentFrameId);
        var ownerFacts = window?.ParticipantFacts?.GetValueOrDefault(frame.OwnerSeat) ?? window?.Facts;
        if (targetSeat != frame.OwnerSeat || context?.Window != SkillProgramTriggerWindow.PlayEnding || window is null || window.Window != SkillProgramTriggerWindow.PlayEnding || window.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || context.SourceSeat != window.OwnerSeat || context.TargetSeat != window.OwnerSeat || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] is not { } candidate || candidate.OwnerSeat != frame.OwnerSeat || candidate.SkillId != frame.SkillId || candidate.SkillInstanceId != frame.SkillInstanceId || candidate.BindingId != frame.TriggerId || candidate.GameplayHash != frame.GameplayHash || context.OwnerSeat != frame.OwnerSeat || candidate.OccurrenceIndex != context.OccurrenceIndex || context.Facts?.CurrentTurnUsedCardCategoryCount is not { } count || ownerFacts?.CurrentTurnUsedCardCategoryCount != count || count is < 0 or > 3)
            throw new InvalidOperationException("Type-count draw requires its exact Play-ending candidate and frozen owner count.");
        return count;
    }
}
