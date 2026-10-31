#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWholeActualTurnUsesRuntime
{
    internal bool TracksWholeActualTurnUseOrdinal => _contentRegistryOperations._contentRegistry.ProgramDependencies.TracksWholeActualTurnUseOrdinal;
    internal bool HasOngoingWholeActualUseTurn => _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber > 0 && _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && !_coreProgramEventHistoryOperations.ProgramEventHistory<ActualTurnCursorReturnedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && e.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat);

    internal CurrentTurnCardUseKindsRecordedEvent[] WholeActualTurnUseHistory() => _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent or ActualTurnCursorReturnedEvent).OfType<CurrentTurnCardUseKindsRecordedEvent>().Where(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat).ToArray();
    internal int? CapturedWholeActualTurnActorUseOrdinal(CardActionContext action)
    {
        if (!TracksWholeActualTurnUseOrdinal || !HasOngoingWholeActualUseTurn || action.ActionId <= 0 || action.Type == CardActionType.Response && (action.EffectiveKind is not (CardKind.Nullification or CardKind.Dodge) || action.ActorSeat != action.ProviderSeat || action.ResponderSeat != action.ActorSeat || action.RequesterSeat is not null))
            return null;
        var facts = WholeActualTurnUseHistory();
        var matching = facts.Where(e => e.CardActionId == action.ActionId).ToArray();
        if (matching.Length == 0)
            return null;
        if (matching is not [var current] || current.WholeActualTurnUseSequence is not > 0 || current.RoundActionType != action.Type || current.RoundProviderSeat != action.ProviderSeat)
            throw new InvalidOperationException("An actual-turn use requires its exact frozen declaration sequence and provider.");
        return CapturedWholeActualTurnUsePrefixOrdinal(current, action.ActorSeat, null);
    }

    internal int? CapturedWholeActualTurnTrickUseOrdinal(GameEngine.IDamageAttempt attack)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasWholeActualTurnTrickDamageModifiers || !HasOngoingWholeActualUseTurn || attack is not NativeOwners.CardAttackHandle || attack.EffectiveCardKind is not { } kind || CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(kind) != SkillProgramCardCategory.Trick)
            return null;
        var owner = _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == attack.ResolutionId);
        var instruction = owner is ProgramSkillFrame { CardAttack: { ProgramUseInstructionIndex: { } index } } ? index : (int? )null;
        if (owner is not CardUseFrame && instruction is null)
            return null;
        var matching = WholeActualTurnUseHistory().Where(fact => fact.OriginFrameId == attack.ResolutionId && !fact.IsNullificationUse && fact.RoundActionType == CardActionType.Use && fact.ProgramInstructionIndex == instruction).ToArray();
        if (matching is not [var current] || current.WholeActualTurnUseSequence is not > 0 || current.CardCategoryMask != 1 << (int)SkillProgramCardCategory.Trick)
            throw new InvalidOperationException("Trick damage requires its exact frozen native-use declaration.");
        return CapturedWholeActualTurnUsePrefixOrdinal(current, attack.CardUserSeat, SkillProgramCardCategory.Trick);
    }

    internal int? CapturedWholeActualTurnUsePrefixOrdinal(CurrentTurnCardUseKindsRecordedEvent current, int expectedActorSeat, SkillProgramCardCategory? category)
    {
        var facts = WholeActualTurnUseHistory();
        var replacements = _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent or ActualTurnCursorReturnedEvent).OfType<ProgramCardUseActorReplacedEvent>().GroupBy(e => e.CardUseFrameId).ToDictionary(group => group.Key, group => group.ToArray());
        var seen = new HashSet<(long? ActionId, long? LegacyUseId, int? InstructionIndex)>();
        var count = 0;
        for (var index = 0; index < current.WholeActualTurnUseSequence; index++)
        {
            if (index >= facts.Length || facts[index].WholeActualTurnUseSequence != index + 1)
                throw new InvalidOperationException("An actual-turn declaration sequence cannot skip or reorder an issued use.");
            var fact = facts[index];
            (long? ActionId, long? LegacyUseId, int? InstructionIndex) key = fact.CardActionId is not null ? (fact.CardActionId, null, null) : (null, fact.OriginFrameId, fact.ProgramInstructionIndex);
            if (!seen.Add(key) || fact.RoundActionType is not (CardActionType.Use or CardActionType.Response) || fact.RoundProviderSeat is not { } provider || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(provider))
                throw new InvalidOperationException("An actual-turn declaration requires one native use and frozen provider.");
            var actor = fact.ActorSeat;
            // A defending Dodge shares the attack's origin frame, but is its own
            // Response. Only the attacking Use follows that replacement chain.
            if (fact.RoundActionType == CardActionType.Use && replacements.TryGetValue(fact.OriginFrameId, out var changes))
                foreach (var change in changes)
                {
                    if (change.PreviousActorSeat != actor || change.OwnerSeat != actor || change.ProviderSeat != provider || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(change.ActorSeat))
                        throw new InvalidOperationException("An actual-turn use lost its native actor-replacement chain.");
                    actor = change.ActorSeat;
                }

            if (index == current.WholeActualTurnUseSequence - 1 && (fact != current || actor != expectedActorSeat))
                return null;
            if (actor == expectedActorSeat && (category is null || fact.CardCategoryMask == 1 << (int)category.Value))
                count = checked(count + 1);
        }

        return count;
    }
}
